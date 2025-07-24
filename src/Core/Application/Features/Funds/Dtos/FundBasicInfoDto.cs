using System.Globalization;
using Application.Common.Dtos;
using Domain.Entities.FundManagement;

namespace Application.Features.Funds.Dtos
{
    /// <summary>
    /// Data Transfer Object for Fund Basic Information
    /// Contains initiation date and voting type details for a fund
    /// </summary>
    public record FundBasicInfoDto
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
        /// Fund initiation date information
        /// </summary>
        public DateTime InitiationDate { get; set; } = new();

        /// <summary>
        /// Fund voting type information
        /// </summary>
        public FundVotingTypeInfo VotingType { get; set; } = new();
    }

    /// <summary>
    /// Data Transfer Object for Fund Initiation Date Information
    /// Contains all initiation date related data and formatting
    /// </summary>
    public record FundInitiationDateInfo
    {
        /// <summary>
        /// Fund initiation date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Initiation date formatted for Arabic culture (dd/MM/yyyy)
        /// </summary>
        public string DateAr => Date.ToString("dd/MM/yyyy", new CultureInfo("ar-EG"));

        /// <summary>
        /// Initiation date formatted for English culture (MM/dd/yyyy)
        /// </summary>
        public string DateEn => Date.ToString("MM/dd/yyyy", new CultureInfo("en-US"));

        /// <summary>
        /// ISO formatted initiation date (yyyy-MM-dd) for API consistency
        /// </summary>
        public string DateIso => Date.ToString("yyyy-MM-dd");

        /// <summary>
        /// Localized initiation date based on current culture
        /// </summary>
        public string LocalizedDate => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? DateAr : DateEn;

        /// <summary>
        /// Long formatted date for display purposes
        /// </summary>
        public string FormattedDate => CultureInfo.CurrentCulture.Name.StartsWith("ar")
            ? Date.ToString("dddd، dd MMMM yyyy", new CultureInfo("ar-EG"))
            : Date.ToString("dddd, MMMM dd, yyyy", new CultureInfo("en-US"));
    }

    /// <summary>
    /// Data Transfer Object for Fund Voting Type Information
    /// Contains all voting type related data and localization
    /// </summary>
    public record FundVotingTypeInfo : LocalizedDto
    {
        /// <summary>
        /// Voting type configured for the fund
        /// </summary>
        public VotingType Type { get; set; }

        /// <summary>
        /// Voting type ID (numeric value)
        /// </summary>
        public int TypeId { get; set; }

       }
}
