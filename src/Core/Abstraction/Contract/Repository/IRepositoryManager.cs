using Abstraction.Contract.Repository.Fund;
using Abstraction.Contract.Repository.Notifications;
using Abstraction.Contracts.Repository.Products;
using Abstraction.Contracts.Repository.Fund;
using Abstraction.Contracts.Repository.Resolution;

namespace Abstraction.Contracts.Repository
{
    public interface IRepositoryManager
    {
        ICategoryRepository Categories { get; }
        IFundRepository Funds { get; }
        IAttachmentRepository Attachments { get; }
        INotificationRepository Notifications { get; }
        IStatusHistoryRepository StatusHistory { get; }

        // Board Member Management
        IBoardMemberRepository BoardMembers { get; }

        // Fund Manager Management
        IFundManagerRepository FundManagers { get; }

        // Resolution Management
        IResolutionRepository Resolutions { get; }
        IResolutionItemRepository ResolutionItems { get; }
        IResolutionTypeRepository ResolutionTypes { get; }
        IResolutionItemConflictRepository ResolutionItemConflicts { get; }
        IResolutionStatusHistoryRepository ResolutionStatusHistory { get; }
        IResolutionVoteRepository ResolutionVotes { get; }
    }
}
