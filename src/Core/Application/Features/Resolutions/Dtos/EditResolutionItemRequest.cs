namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Data Transfer Object for updating an existing resolution item
    /// Used in EditResolutionItem command
    /// </summary>
    public record EditResolutionItemRequest : CreateResolutionItemRequest
    {
        /// <summary>
        /// Resolution item identifier for updates
        /// </summary>
        public int Id { get; set; }
    }
}
