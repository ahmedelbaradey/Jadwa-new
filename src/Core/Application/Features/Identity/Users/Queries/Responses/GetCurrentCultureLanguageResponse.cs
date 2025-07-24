namespace Application.Features.Identity.Users.Queries.Responses
{
    /// <summary>
    /// Response DTO for current culture language information
    /// Contains both the current request culture and user's preferred language
    /// </summary>
    public record GetCurrentCultureLanguageResponse
    {
        /// <summary>
        /// Current culture language from the HTTP request context
        /// Example: "ar-EG", "en-US"
        /// </summary>
        public string CurrentCulture { get; set; } = string.Empty;

        /// <summary>
        /// User's preferred language stored in the database
        /// Example: "ar-EG", "en-US"
        /// </summary>
        public string PreferredLanguage { get; set; } = string.Empty;
    }
}
