using Abstraction.Contract.Repository.Fund;
using Abstraction.Contract.Repository.Notifications;
using Abstraction.Contract.Service;
using Abstraction.Contract.Service.Notifications;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Abstraction.Contracts.Repository.Fund;
using Abstraction.Contracts.Repository.Products;
using Abstraction.Contracts.Repository.Resolution;
using Abstraction.Contract.Repository.AssessmentManagement;
using Domain.Entities.ResolutionManagement;
using Infrastructure.Data;
using Infrastructure.Repository.Fund;
using Infrastructure.Repository.Notifications;
using Infrastructure.Repository.Resolution;
using Infrastructure.Repository.AssessmentManagement;
using Microsoft.Extensions.Localization;
using Repository.Catalog;
using Resources;

namespace Infrastructure.Repository
{
    public sealed class ProductRepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _repositoryContext;
        private readonly Lazy<ICategoryRepository> _iCategpryRepository;
        private readonly Lazy<IFundRepository> _iFundRepository;
        private readonly Lazy<IAttachmentRepository> _iAttachmentRepository;
        private readonly Lazy<INotificationRepository> _iNotificationRepository;
        private readonly Lazy<IStatusHistoryRepository> _iStatusHistoryRepository;

        // Board Member Management
        private readonly Lazy<IBoardMemberRepository> _iBoardMemberRepository;

        // Fund Manager Management
        private readonly Lazy<IFundManagerRepository> _iFundManagerRepository;

        // Resolution Management
        private readonly Lazy<IResolutionRepository> _iResolutionRepository;
        private readonly Lazy<IResolutionItemRepository> _iResolutionItemRepository;
        private readonly Lazy<IResolutionItemConflictRepository> _iResolutionItemConflictRepository;
        private readonly Lazy<IResolutionTypeRepository> _iResolutionTypeRepository;
        private readonly Lazy<IResolutionVoteRepository> _iResolutionVoteRepository;
        private readonly Lazy<IResolutionStatusHistoryRepository> _iResolutionStatusHistoryRepository;

        // Assessment Management
        private readonly Lazy<IAssessmentRepository> _iAssessmentRepository;
        private readonly Lazy<IAssessmentQuestionRepository> _iAssessmentQuestionRepository;
        private readonly Lazy<IAssessmentResponseRepository> _iAssessmentResponseRepository;
        private readonly Lazy<IAnswerRepository> _iAnswerRepository;

        private readonly INotificationLocalizationService _localizationService;
        private readonly ILoggerManager _logger; // Optional logger for logging errors and warnings
 

        public ProductRepositoryManager(AppDbContext repositoryContext, INotificationLocalizationService localizationService, ILoggerManager logger, ICurrentUserService currentUserService)
        {
            _repositoryContext = repositoryContext;
            _localizationService = localizationService;
            _logger = logger;


            _iCategpryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(repositoryContext, currentUserService));
            _iFundRepository = new Lazy<IFundRepository>(() => new FundRepository(repositoryContext, currentUserService));
            _iAttachmentRepository = new Lazy<IAttachmentRepository>(() => new AttachmentRepository(repositoryContext, currentUserService));
            _iStatusHistoryRepository = new Lazy<IStatusHistoryRepository>(() => new StatusHistoryRepository(repositoryContext, currentUserService));
            _iNotificationRepository = new Lazy<INotificationRepository>(() => new NotificationRepository(repositoryContext, _localizationService, _logger, currentUserService));

            // Board Member Management
            _iBoardMemberRepository = new Lazy<IBoardMemberRepository>(() => new BoardMemberRepository(repositoryContext, currentUserService));

            // Fund Manager Management
            _iFundManagerRepository = new Lazy<IFundManagerRepository>(() => new FundManagerRepository(repositoryContext, currentUserService));



            // Resolution Management
            _iResolutionRepository = new Lazy<IResolutionRepository>(() => new ResolutionRepository(repositoryContext, currentUserService));
            _iResolutionItemRepository = new Lazy<IResolutionItemRepository>(() => new ResolutionItemRepository(repositoryContext, currentUserService));
            _iResolutionItemConflictRepository = new Lazy<IResolutionItemConflictRepository>(() => new ResolutionItemConflictRepository(repositoryContext, currentUserService));
            _iResolutionTypeRepository = new Lazy<IResolutionTypeRepository>(() => new ResolutionTypeRepository(repositoryContext, currentUserService));
            _iResolutionVoteRepository = new Lazy<IResolutionVoteRepository>(() => new ResolutionVoteRepository(repositoryContext, currentUserService));
            _iResolutionStatusHistoryRepository = new Lazy<IResolutionStatusHistoryRepository>(() =>  new ResolutionStatusHistoryRepository(repositoryContext, currentUserService));

            // Assessment Management
            _iAssessmentRepository = new Lazy<IAssessmentRepository>(() => new AssessmentRepository(repositoryContext, currentUserService));
            _iAssessmentQuestionRepository = new Lazy<IAssessmentQuestionRepository>(() => new AssessmentQuestionRepository(repositoryContext, currentUserService));
            _iAssessmentResponseRepository = new Lazy<IAssessmentResponseRepository>(() => new AssessmentResponseRepository(repositoryContext, currentUserService));
            _iAnswerRepository = new Lazy<IAnswerRepository>(() => new AnswerRepository(repositoryContext, currentUserService));
        }

        public ICategoryRepository Categories => _iCategpryRepository.Value;
        public IFundRepository Funds => _iFundRepository.Value;
        public IAttachmentRepository Attachments => _iAttachmentRepository.Value;
        public INotificationRepository Notifications => _iNotificationRepository.Value;
        public IStatusHistoryRepository StatusHistory => _iStatusHistoryRepository.Value;

        public IBoardMemberRepository BoardMembers => _iBoardMemberRepository.Value;

        public IFundManagerRepository FundManagers => _iFundManagerRepository.Value;

        public IResolutionRepository Resolutions => _iResolutionRepository.Value;

        public IResolutionItemRepository ResolutionItems => _iResolutionItemRepository.Value;

        public IResolutionTypeRepository ResolutionTypes => _iResolutionTypeRepository.Value;

        public IResolutionItemConflictRepository ResolutionItemConflicts => _iResolutionItemConflictRepository.Value;

        public IResolutionVoteRepository ResolutionVotes => _iResolutionVoteRepository.Value;
        public IResolutionStatusHistoryRepository ResolutionStatusHistory => _iResolutionStatusHistoryRepository.Value;

        // Assessment Management
        public IAssessmentRepository Assessments => _iAssessmentRepository.Value;
        public IAssessmentQuestionRepository AssessmentQuestions => _iAssessmentQuestionRepository.Value;
        public IAssessmentResponseRepository AssessmentResponses => _iAssessmentResponseRepository.Value;
        public IAnswerRepository Answers => _iAnswerRepository.Value;
    }
}
