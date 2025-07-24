namespace Application.Features.Resolutions.Validation
{
    /// <summary>
    /// Centralized validation constants for Resolution domain
    /// Provides consistent maximum length values across all validation classes
    /// Makes it easy to maintain and update validation rules from a single location
    /// </summary>
    public static class ValidationConstants
    {
        #region Resolution Basic Fields
        
        /// <summary>
        /// Maximum length for resolution description field
        /// Used in: BaseValidation, AddResolutionValidation, EditResolutionValidation
        /// </summary>
        public const int DESCRIPTION_MAX_LENGTH = 500;
        
        /// <summary>
        /// Maximum length for new type field when resolution type is "Other"
        /// Used in: BaseValidation, AddResolutionValidation, EditResolutionValidation
        /// </summary>
        public const int NEW_TYPE_MAX_LENGTH = 100;
        
        /// <summary>
        /// Maximum length for resolution code field
        /// Used in: AddResolutionValidation, EditResolutionValidation
        /// </summary>
        public const int RESOLUTION_CODE_MAX_LENGTH = 50;
        
        #endregion
        
        #region Resolution Items
        
        /// <summary>
        /// Maximum length for resolution item title field
        /// Used in: EditResolutionValidation, AddResolutionItemValidation
        /// </summary>
        public const int RESOLUTION_ITEM_TITLE_MAX_LENGTH = 200;
        
        /// <summary>
        /// Maximum length for resolution item description field
        /// Used in: EditResolutionValidation, AddResolutionItemValidation
        /// </summary>
        public const int RESOLUTION_ITEM_DESCRIPTION_MAX_LENGTH = 500;
        
        /// <summary>
        /// Maximum number of resolution items per resolution
        /// Used in: EditResolutionValidation, AddResolutionValidation
        /// </summary>
        public const int MAX_RESOLUTION_ITEMS_COUNT = 20;
        
        #endregion
        
        #region Conflict Management
        
        /// <summary>
        /// Maximum length for conflict notes field
        /// Used in: EditResolutionValidation, ResolutionItemConflictValidation
        /// </summary>
        public const int CONFLICT_NOTES_MAX_LENGTH = 500;
        
        /// <summary>
        /// Maximum number of conflict members per resolution item
        /// Used in: EditResolutionValidation, ResolutionItemConflictValidation
        /// </summary>
        public const int MAX_CONFLICT_MEMBERS_COUNT = 10;
        
        #endregion
        
        #region Attachments
        
        /// <summary>
        /// Maximum number of attachments per resolution
        /// Used in: EditResolutionValidation, AddResolutionValidation
        /// </summary>
        public const int MAX_ATTACHMENTS_COUNT = 10;
        
        /// <summary>
        /// Maximum file size in bytes (10 MB)
        /// Used in: EditResolutionValidation, AddResolutionValidation
        /// </summary>
        public const long MAX_FILE_SIZE_BYTES = 10 * 1024 * 1024; // 10 MB
        
        #endregion
        
        #region Business Rules
        
        /// <summary>
        /// Maximum number of independent board members allowed
        /// Used in: BoardMemberValidation, ResolutionValidation
        /// </summary>
        public const int MAX_INDEPENDENT_MEMBERS = 14;
        
        /// <summary>
        /// Minimum number of board members required for voting
        /// Used in: ResolutionValidation, VotingValidation
        /// </summary>
        public const int MIN_VOTING_MEMBERS = 3;
        
        #endregion
    }
}
