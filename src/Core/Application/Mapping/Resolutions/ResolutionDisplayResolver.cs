using AutoMapper;
using Application.Common.Helpers;
using Domain.Entities.ResolutionManagement;
using Domain.Entities.FundManagement;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Mapping.Resolutions
{
    /// <summary>
    /// Custom AutoMapper value resolver for localized resolution status display
    /// Provides localized text for resolution status using SharedResources
    /// </summary>
    public class ResolutionStatusDisplayResolver : IValueResolver<Resolution, object, string>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public ResolutionStatusDisplayResolver(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
        }

        public string Resolve(Resolution source, object destination, string destMember, ResolutionContext context)
        {
            return LocalizationHelper.GetResolutionStatusDisplay(source.Status, _localizer);
        }
    }

    /// <summary>
    /// Custom AutoMapper value resolver for localized voting type display
    /// Provides localized text for voting type using SharedResources
    /// </summary>
    public class VotingTypeDisplayResolver : IValueResolver<Resolution, object, string>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public VotingTypeDisplayResolver(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
        }

        public string Resolve(Resolution source, object destination, string destMember, ResolutionContext context)
        {
            return LocalizationHelper.GetVotingTypeDisplay(source.VotingType, _localizer);
        }
    }

    /// <summary>
    /// Custom AutoMapper value resolver for localized member voting result display
    /// Provides localized text for member voting result using SharedResources
    /// </summary>
    public class MemberVotingResultDisplayResolver : IValueResolver<Resolution, object, string>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public MemberVotingResultDisplayResolver(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
        }

        public string Resolve(Resolution source, object destination, string destMember, ResolutionContext context)
        {
            return LocalizationHelper.GetMemberVotingResultDisplay(source.MemberVotingResult, _localizer);
        }
    }
}
