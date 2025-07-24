using Microsoft.Extensions.Localization;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MeetingManagement.State
{
    /// <summary>
    /// Factory class for creating Meeting State instances
    /// Provides centralized state creation with proper dependency injection
    /// Based on ResolutionStateFactory pattern and follows identical structure
    /// Ensures proper localization injection into each state instance
    /// </summary>
    public class MeetingStateFactory
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        /// <summary>
        /// Constructor that initializes the factory with required dependencies
        /// </summary>
        /// <param name="localizer">String localizer for localized messages</param>
        public MeetingStateFactory(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        /// <summary>
        /// Creates a meeting state instance based on the provided status
        /// </summary>
        /// <param name="status">The meeting status to create state for</param>
        /// <returns>IMeetingState instance corresponding to the status</returns>
        /// <exception cref="ArgumentException">Thrown when an invalid status is provided</exception>
        public IMeetingState CreateState(MeetingStatusEnum status)
        {
            return status switch
            {
                MeetingStatusEnum.Scheduled => CreateScheduledState(),
                MeetingStatusEnum.InProgress => CreateInProgressState(),
                MeetingStatusEnum.Finished => CreateFinishedState(),
                MeetingStatusEnum.Cancelled => CreateCancelledState(),
                MeetingStatusEnum.Postponed => CreatePostponedState(),
                _ => throw new ArgumentException($"Invalid meeting status: {status}", nameof(status))
            };
        }

        /// <summary>
        /// Creates a meeting state context with the specified initial state
        /// </summary>
        /// <param name="initialStatus">The initial status for the context</param>
        /// <returns>MeetingStateContext instance with the specified initial state</returns>
        public MeetingStateContext CreateContext(MeetingStatusEnum initialStatus)
        {
            var initialState = CreateState(initialStatus);
            return new MeetingStateContext(initialState, _localizer);
        }

        /// <summary>
        /// Creates a meeting state context with an existing state instance
        /// </summary>
        /// <param name="initialState">The initial state instance</param>
        /// <returns>MeetingStateContext instance with the specified initial state</returns>
        public MeetingStateContext CreateContext(IMeetingState initialState)
        {
            return new MeetingStateContext(initialState, _localizer);
        }

        /// <summary>
        /// Gets all available meeting states
        /// </summary>
        /// <returns>Collection of all available meeting states</returns>
        public IEnumerable<IMeetingState> GetAllStates()
        {
            return new IMeetingState[]
            {
                CreateScheduledState(),
                CreateInProgressState(),
                CreateFinishedState(),
                CreateCancelledState(),
                CreatePostponedState()
            };
        }

        /// <summary>
        /// Gets all available meeting status enum values
        /// </summary>
        /// <returns>Collection of all meeting status enum values</returns>
        public IEnumerable<MeetingStatusEnum> GetAllStatuses()
        {
            return Enum.GetValues<MeetingStatusEnum>();
        }

        #region Private Factory Methods

        /// <summary>
        /// Creates a scheduled meeting state instance
        /// </summary>
        /// <returns>ScheduledMeetingState instance</returns>
        private IMeetingState CreateScheduledState()
        {
            return new ScheduledMeetingState(_localizer);
        }

        /// <summary>
        /// Creates an in-progress meeting state instance
        /// </summary>
        /// <returns>InProgressMeetingState instance</returns>
        private IMeetingState CreateInProgressState()
        {
            return new InProgressMeetingState(_localizer);
        }

        /// <summary>
        /// Creates a finished meeting state instance
        /// </summary>
        /// <returns>FinishedMeetingState instance</returns>
        private IMeetingState CreateFinishedState()
        {
            return new FinishedMeetingState(_localizer);
        }

        /// <summary>
        /// Creates a cancelled meeting state instance
        /// </summary>
        /// <returns>CancelledMeetingState instance</returns>
        private IMeetingState CreateCancelledState()
        {
            return new CancelledMeetingState(_localizer);
        }

        /// <summary>
        /// Creates a postponed meeting state instance
        /// </summary>
        /// <returns>PostponedMeetingState instance</returns>
        private IMeetingState CreatePostponedState()
        {
            return new PostponedMeetingState(_localizer);
        }

        #endregion
    }
}
