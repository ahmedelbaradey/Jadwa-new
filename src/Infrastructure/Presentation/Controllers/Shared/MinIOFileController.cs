using Application.Features.Shared.FileManagment.Commands.MinIOUpload;
using Application.Features.Shared.FileManagment.Commands.MinIODownload;
using Application.Features.Shared.FileManagment.Commands.MinIOPreview;
using Application.Features.Shared.FileManagment.Commands.MinIODelete;
using Application.Features.Shared.FileManagment.Commands.MinIOUploadMultiple;
using Microsoft.AspNetCore.Mvc;
using Presentation.Bases;

namespace Presentation.Controllers.Shared
{
    /// <summary>
    /// Controller for MinIO file management operations
    /// Provides upload, download, and preview functionality for MinIO storage
    /// Separate from FileManagmentController to maintain existing functionality
    /// </summary>
    [Route("api/MinIO/[controller]")]
    [ApiController]
    //[Authorize]
    public class MinIOFileController : AppControllerBase
    {
        /// <summary>
        /// Upload a file to MinIO storage
        /// </summary>
        /// <param name="command">Upload command containing file and metadata</param>
        /// <returns>Attachment information with MinIO storage details</returns>
        [Consumes("multipart/form-data")]
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadFile(MinIOUploadCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        /// <summary>
        /// GetFileAsByte MinIO storage
        /// </summary>
        /// <param name="command">GetFileAsByte command containing file ID</param>
        /// <returns>File content as byte array</returns>
        [HttpPost("GetFileAsByte")]
        public async Task<IActionResult> GetFileAsByte(MinIODownloadCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        /// <summary>
        /// Generate a preview URL for a file in MinIO storage
        /// </summary>
        /// <param name="command">Preview command containing file ID and expiry settings</param>
        /// <returns>Presigned URL for file preview</returns>
        [HttpPost("Preview")]
        public async Task<IActionResult> GetPreviewUrl(MinIOPreviewCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        ///// <summary>
        ///// GetFileAsByte a file from MinIO storage by ID (GET endpoint for direct access)
        ///// </summary>
        ///// <param name="id">File attachment ID</param>
        ///// <param name="bucketName">Optional bucket name override</param>
        ///// <returns>File content as byte array</returns>
        //[HttpGet("GetFileAsByteById/{id}")]
        //public async Task<IActionResult> GetFileAsByteById(int id, [FromQuery] string? bucketName = null)
        //{
        //    var command = new MinIODownloadCommand { Id = id, BucketName = bucketName };
        //    var response = await Mediator.Send(command);
        //    return NewResult(response);
        //}

        ///// <summary>
        ///// Generate a preview URL for a file by ID (GET endpoint for direct access)
        ///// </summary>
        ///// <param name="id">File attachment ID</param>
        ///// <param name="expiryInMinutes">URL expiry time in minutes (default: 60)</param>
        ///// <param name="bucketName">Optional bucket name override</param>
        ///// <returns>Presigned URL for file preview</returns>
        //[HttpGet("PreviewFile/{id}")]
        //public async Task<IActionResult> GetPreviewUrlById(int id, [FromQuery] int expiryInMinutes = 60, [FromQuery] string? bucketName = null)
        //{
        //    var command = new MinIOPreviewCommand
        //    {
        //        Id = id,
        //        ExpiryInMinutes = expiryInMinutes,
        //        BucketName = bucketName
        //    };
        //    var response = await Mediator.Send(command);
        //    return NewResult(response);
        //}

        /// <summary>
        /// Download a file from minio storage
        /// </summary>
        /// <param name="id">File attachment ID</param>
        /// <param name="bucketName">Optional bucket name override</param>
        /// <returns>File content</returns>
        [HttpGet("DownloadFile/{id}")]
        public async Task<IActionResult> DownloadFile(int id, [FromQuery] string? bucketName = null)
        {
            var command = new MinIODownloadCommand { Id = id, BucketName = bucketName };
            var response = await Mediator.Send(command);

            if (response.Successed && response.Data != null)
            {
                return File(response.Data.FileBytes, response.Data.ContentType, response.Data.FileName);
            }

            return NewResult(response);
        }

        /// <summary>
        /// Delete a file from MinIO storage
        /// </summary>
        /// <param name="command">Delete command containing file ID</param>
        /// <returns>Success status of deletion operation</returns>
        [HttpDelete("DeleteFile")]
        public async Task<IActionResult> DeleteFile(MinIODeleteCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        ///// <summary>
        ///// Delete a file from MinIO storage by ID (DELETE endpoint for direct access)
        ///// </summary>
        ///// <param name="id">File attachment ID</param>
        ///// <param name="bucketName">Optional bucket name override</param>
        ///// <returns>Success status of deletion operation</returns>
        //[HttpDelete("DeleteFile/{id}")]
        //public async Task<IActionResult> DeleteFileById(int id, [FromQuery] string? bucketName = null)
        //{
        //    var command = new MinIODeleteCommand { Id = id, BucketName = bucketName };
        //    var response = await Mediator.Send(command);
        //    return NewResult(response);
        //}

        /// <summary>
        /// Upload multiple files to MinIO storage
        /// </summary>
        /// <param name="command">Multiple upload command containing files and metadata</param>
        /// <returns>List of uploaded file information</returns>
        [Consumes("multipart/form-data")]
        [HttpPost("UploadMultiple")]
        public async Task<IActionResult> UploadMultipleFiles(MinIOUploadMultipleCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
