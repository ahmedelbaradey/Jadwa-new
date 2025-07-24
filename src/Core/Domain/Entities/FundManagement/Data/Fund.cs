using Domain.Entities.Base;
using Domain.Entities.FundManagement.State;
using Domain.Entities.Notifications;
using Domain.Entities.ResolutionManagement;
using Domain.Entities.Shared;
using Domain.Entities.Startegies;
using Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities.FundManagement;

public class Fund : FullAuditedEntity
{
    public string? OldCode { get; set; }
    public required string  Name { get; set; }
    public int StrategyId { get; set; }
    public int PropertiesNumber { get; set; }
    public DateTime InitiationDate { get; set; }
    public DateTime? ExitDate { get; set; }
    public int AttachmentId { get; set; }
    public int VotingTypeId { get; set; }
    public int LegalCouncilId { get; set; }

    [ForeignKey("LegalCouncilId")]
    public User LegalCouncil { get; set; } 
    
    [ForeignKey("StrategyId")]
    public Strategy Strategy { get; set; }
    
    [ForeignKey("AttachmentId")]
    public Attachment Attachment { get; set; }
    public List<FundManager> FundManagers { get; set; }
    public List<FundBoardSecretary> FundBoardSecretaries { get; set; }
    public List<FundStatusHistory> FundStatusHistories { get; set; }
    public List<Notification> Notifications { get; set; }

    /// <summary>
    /// Collection navigation property to BoardMember entities
    /// Represents all board members associated with this fund
    /// </summary>
    public virtual ICollection<BoardMember> BoardMembers { get; set; } = new List<BoardMember>();

    /// <summary>
    /// Collection navigation property to Resolution entities
    /// Represents all resolutions created for this fund
    /// </summary>
    public virtual ICollection<Resolution> Resolutions { get; set; } = new List<Resolution>();

  

    #region State design pattern implementation
    [NotMapped]
    private IFundState _currentState;

    [NotMapped]
    public IFundState CurrentState => _currentState ??= FundStateFactory.GetInitialState();

    /// <summary>
    /// Sets the current state of the fund
    /// </summary>
    /// <param name="state">The new state to set</param>
    public void SetState(IFundState state)
    {
        _currentState = state;
    }

    /// <summary>
    /// Handles the current state logic
    /// </summary>
    public void Handle()
    {
        _currentState?.Handle(this);
    }

    /// <summary>
    /// Changes the fund state and updates status history
    /// Implements the State Pattern for fund lifecycle management
    /// For enhanced audit logging and notifications, use StateContext.ChangeStatusWithAudit()
    /// </summary>
    /// <param name="newState">The new state to transition to</param>
    public void ChangeState(IFundState newState)
    {
        var currentStatusId = Status?.Id ?? 0;
        var newStatusId = (int)newState.Status;

        // Only add history entry if status actually changes
        if (currentStatusId != newStatusId)
        {
            _currentState = newState;
            FundStatusHistories ??= new List<FundStatusHistory>();
            FundStatusHistories.Add(new FundStatusHistory
            {
                FundId = this.Id,
                StatusHistoryId = newStatusId
            });
        }
    }

    /// <summary>
    /// Transitions to a specific status using the State Pattern
    /// Validates the transition before applying it
    /// </summary>
    /// <param name="targetStatus">Target status to transition to</param>
    /// <returns>True if transition was successful, false if not allowed</returns>
    public bool TransitionToStatus(FundStatusEnum targetStatus)
    {
        var currentStatus = GetCurrentStatusEnum();

        if (!FundStateFactory.IsTransitionAllowed(currentStatus, targetStatus))
        {
            return false;
        }

        var newState = FundStateFactory.CreateState(targetStatus);
        ChangeState(newState);
        return true;
    }

    /// <summary>
    /// Gets the current status as FundStatusEnum
    /// </summary>
    /// <returns>Current fund status enum</returns>
    public FundStatusEnum GetCurrentStatusEnum()
    {
        var statusId = Status?.Id ?? 1; // Default to UnderConstruction
        return (FundStatusEnum)statusId;
    }

    /// <summary>
    /// Initializes the state from the current status
    /// Should be called after loading from database
    /// </summary>
    public void InitializeState()
    {
        var currentStatus = GetCurrentStatusEnum();
        _currentState = FundStateFactory.CreateState(currentStatus);
    }

    /// <summary>
    /// Validates if fund can be activated based on business rules
    /// Fund can be activated if it has 2 or more independent board members
    /// </summary>
    /// <returns>True if fund can be activated, false otherwise</returns>
    public bool CanActivate()
    {
        try
        {
            if (BoardMembers == null)
                return false;

            var independentMembersCount = BoardMembers
                .Count(bm => bm.IsActive && bm.MemberType == BoardMemberType.Independent);

            return independentMembersCount >= 2;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Attempts to activate the fund using basic state transition
    /// Enhanced activation with audit logging should be handled by the Application layer
    /// </summary>
    /// <returns>True if fund was activated, false otherwise</returns>
    public bool TryActivate()
    {
        try
        {
            var currentStatus = GetCurrentStatusEnum();
            if (currentStatus != FundStatusEnum.Active && CanActivate())
            {
                return TransitionToStatus(FundStatusEnum.Active);
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    #endregion




    public StatusHistory Status
    {
        get => CurrentStatus();
    }
    private StatusHistory CurrentStatus()
    {
        return FundStatusHistories?.OrderByDescending(x => x.CreatedAt)?.FirstOrDefault()?.StatusHistory ?? new();
    }
}
