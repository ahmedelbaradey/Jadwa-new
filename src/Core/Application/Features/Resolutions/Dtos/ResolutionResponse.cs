namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Response DTO for resolution operations
    /// Used for API responses with operation messages
    /// Based on requirements in Sprint.md for resolution operations
    /// </summary>
    public record ResolutionResponse : ResolutionDto
    {
        /// <summary>
        /// Success message for the operation
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
