using Application.Features.Resolutions.Dtos;
using Application.Mapping.Resolutions;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.FundManagement;
using Domain.Entities.ResolutionManagement;
using System.Globalization;

namespace Application.Mapping
{
    /// <summary>
    /// Mapping configurations for retrieving Resolution entities
    /// Maps from domain entities to DTOs for read operations
    /// </summary>
    public partial class ResolutionsProfile
    {
        public void GetResolutionMapping()
        {
            // Resolution entity to ResolutionDto
            CreateMap<Resolution, ResolutionDto>();
            //    .ForMember(dest => dest.StatusDisplay, opt => opt.Ignore()); // Computed property

            // Resolution entity to ListResolutionDto
            CreateMap<Resolution, ListResolutionDto>()
                .ForMember(dest => dest.ResolutionTypeName, opt => opt.MapFrom(src => GetResolutionTypeName(src.ResolutionType)));

            // Resolution entity to SingleResolutionResponseView 
            CreateMap<Resolution, SingleResolutionResponseView>()
                .IncludeBase<Resolution, ResolutionDto>()
                .ForMember(dest => dest.FundName, opt => opt.MapFrom(src => src.Fund.Name))
                .ForMember(dest => dest.ResolutionType, opt => opt.MapFrom(src => src.ResolutionType))
                .ForMember(dest => dest.ItemsCount, opt => opt.MapFrom(src => src.ResolutionItems.Count))
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => src.UpdatedAt ?? src.CreatedAt))
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (int)src.Status))
                .ForMember(dest => dest.ResolutionStatus, opt => opt.MapFrom(src=>src.ResolutionStatus))
                .ForMember(dest => dest.StatusDisplay, opt => opt.MapFrom<ResolutionStatusDisplayResolver>())
                .ForMember(dest => dest.VotingTypeDisplay, opt => opt.MapFrom<VotingTypeDisplayResolver>())
                .ForMember(dest => dest.MemberVotingResultDisplay, opt => opt.MapFrom<MemberVotingResultDisplayResolver>())
                // Enhanced properties for advanced statuses
                .ForMember(dest => dest.ResolutionItems, opt => opt.MapFrom(src => src.ResolutionItems.OrderBy(ri => ri.DisplayOrder)))
                .ForMember(dest => dest.Attachment, opt => opt.MapFrom(src => src.Attachment))
                .ForMember(dest => dest.OtherAttachments, opt => opt.MapFrom(src => src.OtherAttachments.Select(oa => oa.Attachment)))
                .ForMember(dest => dest.RejectionReason, opt => opt.MapFrom(src => GetRejectionReason(src)))
                .ForMember(dest => dest.ParentResolutionCode, opt => opt.MapFrom(src => src.ParentResolution != null ? src.ParentResolution.Code : string.Empty))
                // Action permissions will be set in the handler based on user role and status
                .ForMember(dest => dest.CanConfirm, opt => opt.Ignore())
                .ForMember(dest => dest.CanReject, opt => opt.Ignore())
                .ForMember(dest => dest.CanEdit, opt => opt.Ignore())
                .ForMember(dest => dest.CanDownloadAttachments, opt => opt.Ignore());

            // Resolution entity to SingleResolutionResponse
            CreateMap<Resolution, SingleResolutionResponse>()
                .ForMember(dest => dest.ResolutionType, opt => opt.MapFrom(src => src.ResolutionType))
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => src.UpdatedAt ?? src.CreatedAt))
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (int)src.Status))
                .ForMember(dest => dest.ResolutionStatus, opt => opt.MapFrom(src => src.ResolutionStatus))
                .ForMember(dest=> dest.Code , opt => opt.MapFrom(src=> src.Code))
                .ForMember(dest => dest.CreatorID, opt => opt.MapFrom(src => src.CreatedBy))
                // Action permissions will be set in the handler based on user role and status
                .ForMember(dest => dest.CanConfirm, opt => opt.Ignore())
                .ForMember(dest => dest.CanReject, opt => opt.Ignore())
                .ForMember(dest => dest.CanEdit, opt => opt.Ignore());

            // Resolution entity to ResolutionDetailsDto
            CreateMap<Resolution, ResolutionDetailsDto>()
                .IncludeBase<Resolution, ResolutionDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.ResolutionItems.OrderBy(ri => ri.DisplayOrder)))
                .ForMember(dest => dest.OtherAttachments, opt => opt.MapFrom(src => src.OtherAttachments.Select(oa => oa.Attachment)))
                .ForMember(dest => dest.StatusHistory, opt => opt.MapFrom(src => src.ResolutionStatusHistories.OrderByDescending(rsh => rsh.CreatedAt)))
                .ForMember(dest => dest.Votes, opt => opt.MapFrom(src => src.Votes))
                .ForMember(dest => dest.ParentResolution, opt => opt.MapFrom(src => src.ParentResolution))
                .ForMember(dest => dest.ChildResolutions, opt => opt.MapFrom(src => src.ChildResolutions))
                .ForMember(dest => dest.CreatedByUser, opt => opt.MapFrom(src => src.CreatedBy > 0 ? "User" : "System"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            // Resolution entity to ResolutionResponse
            CreateMap<Resolution, ResolutionResponse>()
                .IncludeBase<Resolution, ResolutionDto>()
                .ForMember(dest => dest.Message, opt => opt.Ignore());

            // Resolution entity to ResolutionForEditDto (lightweight mapping for edit screen)
            CreateMap<Resolution, ResolutionForEditDto>()
                .IncludeBase<Resolution, ResolutionDto>()
                .ForMember(dest => dest.ResolutionItems, opt => opt.MapFrom(src => src.ResolutionItems.OrderBy(ri => ri.DisplayOrder)))
                .ForMember(dest => dest.Attachment, opt => opt.MapFrom(src => src.Attachment))
                .ForMember(dest => dest.FundInitiationDate, opt => opt.MapFrom(src => src.Fund.InitiationDate))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => CultureInfo.CurrentCulture.Name.StartsWith("ar", StringComparison.OrdinalIgnoreCase)
                ? src.ResolutionStatus.NameAr
                : src.ResolutionStatus.NameEn))
                .ForMember(dest => dest.ParentResolutionCode, opt => opt.MapFrom(src => src.ParentResolution != null ? src.ParentResolution.Code : string.Empty))
                .ForMember(dest => dest.OtherAttachments, opt => opt.MapFrom(src => src.OtherAttachments.Select(oa => oa.Attachment)));

            // ResolutionType entity to ResolutionTypeDto
            CreateMap<ResolutionType, ResolutionTypeDto>()
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.LocalizedName, opt => opt.Ignore()); // Computed property

            // ResolutionStatus entity to ResolutionStatusDto
            CreateMap<ResolutionStatus, ResolutionStatusDto>()
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Value, opt => opt.Ignore()) // Will be set from Resolution.Status
                .ForMember(dest => dest.Description, opt => opt.Ignore()) // Will be set from enum description
                .ForMember(dest => dest.LocalizedName, opt => opt.Ignore()); // Computed property

            // Attachment entity to AttachmentDto
            CreateMap<Domain.Entities.Shared.Attachment, AttachmentDto>()
                .ForMember(dest => dest.FilePath, opt => opt.Ignore()) // Will be populated with preview URL in handler
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.ContentType))
                .ForMember(dest => dest.UploadedBy, opt => opt.MapFrom(src => src.CreatedBy));

            CreateMap<ResolutionStatusHistory, ResolutionStatusHistoryDto>()
                 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.NewStatus))
                 .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.CreatedByUser.FullName));


        }



        /// <summary>
        /// Helper method to get localized resolution type name
        /// </summary>
        /// <param name="resolutionType">Resolution type entity</param>
        /// <returns>Localized resolution type name</returns>
        private static string GetResolutionTypeName(ResolutionType? resolutionType)
        {
            if (resolutionType == null)
                return "Unknown Type"; // Fallback for when ResolutionType is not loaded

            // TODO: Implement proper localization based on current culture
            // For now, return English name as default, fallback to Arabic if English is empty
            return !string.IsNullOrEmpty(resolutionType.NameEn) ? resolutionType.NameEn :
                   !string.IsNullOrEmpty(resolutionType.NameAr) ? resolutionType.NameAr :
                   "Unknown Type";
        }

       





        /// <summary>
        /// Helper method to get rejection reason from status history
        /// </summary>
        /// <param name="resolution">Resolution entity</param>
        /// <returns>Rejection reason if available</returns>
        private static string? GetRejectionReason(Resolution resolution)
        {
            if (resolution.Status != ResolutionStatusEnum.Rejected)
                return null;

            var rejectionHistory = resolution.ResolutionStatusHistories
                .Where(h => h.NewStatus == ResolutionStatusEnum.Rejected)
                .OrderByDescending(h => h.CreatedAt)
                .FirstOrDefault();

            return rejectionHistory?.RejectionReason;
        }


    }
}