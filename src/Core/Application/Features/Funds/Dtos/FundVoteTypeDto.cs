using Domain.Entities.FundManagement;

namespace Application.Features.Funds.Dtos
{
    /// <summary>
    /// Data Transfer Object for Fund Vote Type information
    /// Contains voting methodology configuration for a fund
    /// </summary>
    public record FundVoteTypeDto
    {
        /// <summary>
        /// Fund identifier
        /// </summary>
        public int FundId { get; set; }

        /// <summary>
        /// Fund name
        /// </summary>
        public string FundName { get; set; } = string.Empty;

        /// <summary>
        /// Voting type configured for the fund
        /// </summary>
        public VotingType VotingType { get; set; }

        /// <summary>
        /// Voting type ID (numeric value)
        /// </summary>
        public int VotingTypeId { get; set; }

        /// <summary>
        /// Voting type name in Arabic
        /// </summary>
        public string VotingTypeNameAr { get; set; } = string.Empty;

        /// <summary>
        /// Voting type name in English
        /// </summary>
        public string VotingTypeNameEn { get; set; } = string.Empty;

        /// <summary>
        /// Localized voting type name based on current culture
        /// </summary>
        public string LocalizedVotingTypeName => System.Globalization.CultureInfo.CurrentCulture.Name.StartsWith("ar") ? VotingTypeNameAr : VotingTypeNameEn;
    }
}
