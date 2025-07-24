using Abstraction.Base.Response;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Abstraction.Enums;
using Application.Base.Abstracts;
using Application.Features.Shared.FileManagment.Dtos;
using AutoMapper;
using Domain.Entities.Shared;
using Abstraction.Contract.Service.Storage;


namespace Application.Features.Shared.FileManagment.Commands.Add
{
    public class AddAttachmentCommandHandler : BaseResponseHandler, ICommandHandler<AddAttachmentCommand, BaseResponse<AttachmentDTO>>
    {

        #region Fileds
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IPreviewUrlHelper _previewUrlHelper;
        private readonly string _baseFolderPath= Directory.GetCurrentDirectory()+ "/wwwroot/";
        #endregion

        #region Constructors
        public AddAttachmentCommandHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger, IPreviewUrlHelper previewUrlHelper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _previewUrlHelper = previewUrlHelper;
        }
        #endregion

        #region Handle Functions
        public async Task<BaseResponse<AttachmentDTO>> Handle(AddAttachmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                    return BadRequest<AttachmentDTO>("the request can't be blank");
                if (request.File == null || request.File.Length == 0)
                    return BadRequest<AttachmentDTO>("File is missing or empty.");

                // Get module name from enum safely
                string? sourceModuleFolder = Enum.IsDefined(typeof(ModuleEnum), request.ModuleId)
                    ? Enum.GetName(typeof(ModuleEnum), request.ModuleId)
                    : Enum.GetName(typeof(ModuleEnum), ModuleEnum.Other);

                // Extract extension safely
                string? extension = Path.GetExtension(request.File.FileName)?.TrimStart('.');
                if (string.IsNullOrWhiteSpace(extension))
                    return BadRequest<AttachmentDTO>("Invalid file name or extension.");

                var id = Guid.NewGuid().ToString();
                var serverFileName = $"{id}.{extension}";
                var folderPath = Path.Combine(_baseFolderPath, sourceModuleFolder);
                var filePath = Path.Combine(folderPath, serverFileName);

                // Ensure directory exists
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.File.CopyToAsync(stream);
                    
                }
                var fileInfo = new FileInfo(filePath);
                long fileSizeInBytes = fileInfo.Length;

                // Construct URL
                //var url = $"{_baseFolderPath}/{sourceModuleFolder}/{serverFileName}";
                var url = $"/{sourceModuleFolder}/{serverFileName}";

                // Save attachment
                var file = _mapper.Map<Attachment>(request);
                file.Path = url;
                file.ContentType = extension;
                file.FileSize = (long)fileSizeInBytes;
                var result = await _repository.Attachments.AddAsync(file);
                if (result is null)
                    return BadRequest<AttachmentDTO>("Added Operation Failed.");

                // Generate preview URL for the saved file
                var previewUrl = await _previewUrlHelper.GeneratePreviewUrlAsync(url, request.ModuleId, cancellationToken);

                // Create DTO
                var attachment = new AttachmentDTO
                {
                    Id = result.Id,
                    FileName = request.FileName,
                    Folder = sourceModuleFolder,
                    Extension = extension,
                    ServerFileName = serverFileName,
                    URL = previewUrl 
                };

                return Success(attachment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: in AddFileCommand");
                return ServerError<AttachmentDTO>(ex.Message);
            }
        }

        #endregion

    }
}
