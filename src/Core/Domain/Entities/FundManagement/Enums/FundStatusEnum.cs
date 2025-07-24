using System.ComponentModel;

namespace Domain.Entities.FundManagement
{
    /// <summary>
    /// Enumeration representing fund status values that map to StatusHistory table
    /// Based on StatusHistoryConfig seed data and Sprint.md requirements
    /// Used for fund lifecycle management and state transitions
    /// </summary>
    public enum FundStatusEnum
    {
        /// <summary>
        /// Fund is under construction - initial state
        /// Maps to StatusHistory ID: 1
        /// Arabic: "تحت الإنشاء"
        /// English: "Under Construction"
        /// </summary>
        [Description("Under Construction")]
        UnderConstruction = 1,

        /// <summary>
        /// Fund is waiting for board members to be added
        /// Maps to StatusHistory ID: 2
        /// Arabic: "لا يوجد أعضاء"
        /// English: "Waiting for Adding Members"
        /// </summary>
        [Description("Waiting for Adding Members")]
        WaitingForAddingMembers = 2,

        /// <summary>
        /// Fund is active - has 2 or more independent board members
        /// Maps to StatusHistory ID: 3
        /// Arabic: "نشط"
        /// English: "Active"
        /// </summary>
        [Description("Active")]
        Active = 3,

        /// <summary>
        /// Fund has exited - final state
        /// Maps to StatusHistory ID: 4
        /// Arabic: "تم التخارج"
        /// English: "Exited"
        /// </summary>
        [Description("Exited")]
        Exited = 4
    }
}
