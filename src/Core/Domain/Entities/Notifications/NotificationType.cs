using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Notifications
{
    public enum NotificationType
    {
        AddedToFund = 1,
        RemoveFromFund = 2,
        ChangeExitDate = 3,
        CompeleteFund = 4,
        ResolutionCreated = 5, // MSG002 - Resolution created notification
        ResolutionUpdated = 6, // MSG005 - Resolution updated notification
        FundActivated = 7,     // MSG008 - Fund activated notification
        BoardMemberAdded = 8,  // MSG002 - Board member added notification (to the new member)
        BoardMemberAddedToFund = 9, // MSG007 - Board member added notification (to fund stakeholders)
        ResolutionCancelled = 10, // MSG004 - Resolution cancelled notification
        ResolutionConfirmed = 11, // MSG002 - Resolution confirmed notification
        ResolutionRejected = 12,  // MSG004 - Resolution rejected notification
        ResolutionSentToVote = 13, // MSG002 - Resolution sent to vote notification
        NewResolutionCreatedFromApproved = 14, // MSG009 - New resolution created from approved/not approved
        ResolutionVotingSuspended = 15, // MSG007 - Resolution voting suspended notification (Alternative 1)
        ResolutionDataCompleted = 16, // MSG003 - Resolution data completed notification (JDWA-507)
        AddedToFundForManager = 17,
        UserRelieveOfDuties = 18, // MSG-EDIT-013 - User relieve of duties notification
        UserRoleUpdate = 19, // MSG-EDIT-014 - User role update notification
        SessionActivityReminder = 20,
        SessionExpiredReminder = 21,

 
        // Meeting Management Notifications
        MeetingTimeProposalCreated = 22, // MSG-MTV-NOT-01 - New meeting time proposal created
        MeetingTimeVotingCompleted = 23  // MSG-VMT-NOT-01 - Meeting time voting completed
 
        // Assessment Notifications
        AssessmentSubmittedForApproval = 22, // MSG002 - Assessment submitted for approval notification
        AssessmentApproved = 23, // MSG002 - Assessment approved notification
        AssessmentRejected = 24, // MSG004 - Assessment rejected notification
        AssessmentDistributed = 25, // MSG002 - Assessment distributed to board members notification
        AssessmentResponseSubmitted = 26, // MSG002 - Assessment response submitted notification
        AssessmentCompleted = 27 // MSG002 - Assessment completed notification
 
    }
}
