using Abstraction.Base.Response;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Application.Base.Abstracts;
using Application.Features.Shared.FileManagment.Commands.NewFolder;
using Application.Features.Shared.FileManagment.Dtos;
using AutoMapper;
using Domain.Entities.Shared;

namespace Application.Features.Shared.FileManagment.Commands.DownloadFile
{
    public class DownloadAttachmentCommandHandler : BaseResponseHandler, ICommandHandler<DownloadAttachmentCommand, BaseResponse<DownloadAttachmentDTO>>
    {

        #region Fileds
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public DownloadAttachmentCommandHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        #endregion 

        #region Handle Functions
        public async Task<BaseResponse<DownloadAttachmentDTO>> Handle(DownloadAttachmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var file = await _repository.Attachments.GetByIdAsync<Attachment>(request.Id ?? 0, false);

                if (file is null)
                    return BadRequest<DownloadAttachmentDTO>("File is missing or empty.");

                var contentType = "application/octet-stream"; // or detect based on file type

                var fileBytes = System.IO.File.ReadAllBytes(Directory.GetCurrentDirectory() + file.Path);

                var attachment = new DownloadAttachmentDTO
                {
                    Id = file.Id,
                    FileName = file.FileName,
                    ContentType = contentType,
                    FileBytes = fileBytes,
                    Path = file.Path,
                };

                return Success(attachment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: in DownloadFileCommand");
                return ServerError<DownloadAttachmentDTO>(ex.Message);
            }
        }

        #endregion

    }
}
