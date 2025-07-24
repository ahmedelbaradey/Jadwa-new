using Application.Features.Assessments.DTOs;
using AutoMapper;
using Domain.Entities.AssessmentManagement;
using System.Text.Json;
using Abstraction.Contract.Service;

namespace Application.Features.Assessments.Profiles
{
    /// <summary>
    /// AutoMapper profile for Assessment-related entities and DTOs
    /// Provides mapping configurations between domain entities and DTOs
    /// Based on existing profile patterns in the codebase
    /// </summary>
    public class AssessmentProfile : Profile
    {
        public AssessmentProfile()
        {
            CreateAssessmentMappings();
            CreateAssessmentQuestionMappings();
            CreateAssessmentResponseMappings();
            CreateAnswerMappings();
        }

        /// <summary>
        /// Creates mapping configurations for Assessment entity
        /// </summary>
        private void CreateAssessmentMappings()
        {
            // Assessment to AssessmentDto mapping
            CreateMap<Assessment, AssessmentDto>()
                .ForMember(dest => dest.FundName, opt => opt.MapFrom(src => src.Fund != null ? src.Fund.NameEn : string.Empty))
                .ForMember(dest => dest.TypeDisplayName, opt => opt.MapFrom(src => GetAssessmentTypeDisplayName(src.Type)))
                .ForMember(dest => dest.StatusDisplayName, opt => opt.MapFrom(src => GetAssessmentStatusDisplayName(src.Status)))
                .ForMember(dest => dest.ReviewerName, opt => opt.MapFrom(src => src.Reviewer != null ? src.Reviewer.FullName : null))
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.FullName : string.Empty))
                .ForMember(dest => dest.QuestionCount, opt => opt.MapFrom(src => src.Questions != null ? src.Questions.Count : 0))
                .ForMember(dest => dest.ResponseCount, opt => opt.MapFrom(src => src.Responses != null ? src.Responses.Count : 0))
                .ForMember(dest => dest.CompletedResponseCount, opt => opt.MapFrom(src =>
                    src.Responses != null ? src.Responses.Count(r => r.Status == ResponseStatus.Completed) : 0))
                .ForMember(dest => dest.AvailableActions, opt => opt.MapFrom(src => src.GetAvailableActions()))
                .ForMember(dest => dest.AllowedTransitions, opt => opt.MapFrom(src => src.GetAllowedTransitions()))
                .ForMember(dest => dest.AttachmentURL, opt => opt.Ignore()); // Will be set by service using IPreviewUrlHelper

            // AddAssessmentDto to Assessment mapping
            CreateMap<AddAssessmentDto, Assessment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.SaveAsDraft ? AssessmentStatus.Draft : AssessmentStatus.WaitingForApproval))
                .ForMember(dest => dest.Questions, opt => opt.Ignore())
                .ForMember(dest => dest.Responses, opt => opt.Ignore())
                .ForMember(dest => dest.Fund, opt => opt.Ignore())
                .ForMember(dest => dest.Reviewer, opt => opt.Ignore())
                .ForMember(dest => dest.ReviewedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ReviewedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ReviewerComments, opt => opt.Ignore())
                .ForMember(dest => dest.Attachment, opt => opt.Ignore());
        }

        /// <summary>
        /// Creates mapping configurations for AssessmentQuestion entity
        /// </summary>
        private void CreateAssessmentQuestionMappings()
        {
            // AssessmentQuestion to AssessmentQuestionDto mapping
            CreateMap<AssessmentQuestion, AssessmentQuestionDto>()
                .ForMember(dest => dest.QuestionTypeDisplayName, opt => opt.MapFrom(src => GetQuestionTypeDisplayName(src.QuestionType)))
                .ForMember(dest => dest.OptionsList, opt => opt.MapFrom(src => ParseOptionsFromJson(src.Options)));

            // AddAssessmentQuestionDto to AssessmentQuestion mapping
            CreateMap<AddAssessmentQuestionDto, AssessmentQuestion>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == 0 ? 0 : src.Id))
                .ForMember(dest => dest.AssessmentId, opt => opt.Ignore())
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => 
                    src.QuestionType == QuestionType.SingleChoice && src.Options.Any() 
                        ? JsonSerializer.Serialize(src.Options) 
                        : null))
                .ForMember(dest => dest.Assessment, opt => opt.Ignore())
                .ForMember(dest => dest.Answers, opt => opt.Ignore());
        }

        /// <summary>
        /// Creates mapping configurations for AssessmentResponse entity
        /// </summary>
        private void CreateAssessmentResponseMappings()
        {
            // AssessmentResponse to AssessmentResponseDto mapping (to be created later)
            // This will be implemented when we create response-related DTOs
        }

        /// <summary>
        /// Creates mapping configurations for Answer entity
        /// </summary>
        private void CreateAnswerMappings()
        {
            // Answer to AnswerDto mapping (to be created later)
            // This will be implemented when we create answer-related DTOs
        }

        /// <summary>
        /// Gets display name for assessment type based on current culture
        /// </summary>
        /// <param name="type">Assessment type</param>
        /// <returns>Localized type display name</returns>
        private static string GetAssessmentTypeDisplayName(AssessmentType type)
        {
            var isArabic = System.Globalization.CultureInfo.CurrentCulture.Name.StartsWith("ar");
            
            return type switch
            {
                AssessmentType.Questionnaire => isArabic ? "استبيان" : "Questionnaire",
                AssessmentType.Attachment => isArabic ? "مرفق" : "Attachment",
                _ => type.ToString()
            };
        }

        /// <summary>
        /// Gets display name for assessment status based on current culture
        /// </summary>
        /// <param name="status">Assessment status</param>
        /// <returns>Localized status display name</returns>
        private static string GetAssessmentStatusDisplayName(AssessmentStatus status)
        {
            var isArabic = System.Globalization.CultureInfo.CurrentCulture.Name.StartsWith("ar");
            
            return status switch
            {
                AssessmentStatus.Draft => isArabic ? "مسودة" : "Draft",
                AssessmentStatus.WaitingForApproval => isArabic ? "في انتظار الموافقة" : "Waiting for Approval",
                AssessmentStatus.Approved => isArabic ? "موافق عليه" : "Approved",
                AssessmentStatus.Rejected => isArabic ? "مرفوض" : "Rejected",
                AssessmentStatus.Active => isArabic ? "نشط" : "Active",
                AssessmentStatus.Completed => isArabic ? "مكتمل" : "Completed",
                _ => status.ToString()
            };
        }

        /// <summary>
        /// Gets display name for question type based on current culture
        /// </summary>
        /// <param name="type">Question type</param>
        /// <returns>Localized type display name</returns>
        private static string GetQuestionTypeDisplayName(QuestionType type)
        {
            var isArabic = System.Globalization.CultureInfo.CurrentCulture.Name.StartsWith("ar");
            
            return type switch
            {
                QuestionType.SingleChoice => isArabic ? "اختيار واحد" : "Single Choice",
                QuestionType.Text => isArabic ? "نص" : "Text",
                _ => type.ToString()
            };
        }

        /// <summary>
        /// Parses options from JSON string to list
        /// </summary>
        /// <param name="optionsJson">JSON string containing options</param>
        /// <returns>List of options</returns>
        private static List<string> ParseOptionsFromJson(string? optionsJson)
        {
            if (string.IsNullOrWhiteSpace(optionsJson))
                return new List<string>();

            try
            {
                return JsonSerializer.Deserialize<List<string>>(optionsJson) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }
    }
}
