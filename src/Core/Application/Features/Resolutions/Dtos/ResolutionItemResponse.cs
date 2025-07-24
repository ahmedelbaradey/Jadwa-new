namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Response DTO for resolution item operations
    /// Used for API responses
    /// </summary>
    public record ResolutionItemResponse : ResolutionItemDto
    {
        /// <summary>
        /// Success message for the operation
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
