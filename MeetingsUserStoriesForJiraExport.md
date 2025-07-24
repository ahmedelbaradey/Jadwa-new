# Meetings User Stories for Jira Export

This document contains all 9 Meetings user stories extracted from the AssessmentStories.md file, formatted for easy import into Jira under a Meetings Epic.

## Epic Information
- **Epic Name**: Meetings
- **Epic Description**: Complete meeting management system for board meetings including scheduling, voting, live management, minutes generation, and electronic signatures.
- **Total Story Points**: 60

## User Stories Summary

| Story # | Title | Story Points | User Roles |
|---------|-------|--------------|------------|
| 1 | Propose Meeting Times for Voting | 8 | Legal Counsel, Board Secretary |
| 2 | Vote on Proposed Meeting Times | 5 | Board Member |
| 3 | Schedule a New Board Meeting | 13 | Legal Counsel, Board Secretary |
| 4 | Manage a Live Board Meeting | 8 | Board Secretary, Legal Counsel, Board Members |
| 5 | Generate and Circulate Meeting Minutes | 8 | Board Secretary, Legal Counsel |
| 6 | Electronically Sign Meeting Minutes | 8 | Board Member |
| 7 | View Scheduled Meetings | 3 | All Users |
| 8 | Modify an Upcoming Meeting | 5 | Board Secretary, Legal Counsel |
| 9 | Cancel an Upcoming Meeting | 2 | Board Secretary, Legal Counsel |

---

## Story 1: Propose Meeting Times for Voting

### Basic Information
- **Summary**: Propose Meeting Times for Voting
- **Story Points**: 8
- **User Story**: As a Board Secretary, I want to create a proposal with multiple date and time options for an upcoming meeting, so that I can send it to board members to vote on the most convenient time.
- **User Roles**: Legal Counsel, Board Secretary
- **Access Requirements**: Authenticated users with the role "Legal Counsel" or "Board Secretary" for the specific fund

### Description
This functionality allows authorized users—specifically the **Legal Counsel** and the **Board Secretary**—to create and send out a poll with several proposed meeting schedules to the fund's board members. The goal is to efficiently determine the most suitable time for an upcoming meeting by collecting votes electronically.

### Business Rules
- Subject is mandatory
- At least one proposed time required
- Maximum 4 time slots allowed
- Attachments must be PDFs
- Proposal saved with "Under Voting" status
- Notifications sent to all board members

### Process Flow
1. Navigate to "Meetings" and click "Propose New Meeting Time"
2. System shows form with all required fields
3. Enter "Meeting Subject" (required)
4. Optionally enter description
5. Optionally upload PDF attachments
6. Add 1–4 time slots using time picker UI
7. Click "Send for Voting"
8. System validates required fields
9. Save proposal to database with status = "Under Voting"
10. Notify all board members via email/in-app
11. Show success message and redirect

### Acceptance Criteria
- **Successful Proposal**: Given on proposal page, when valid input and submit, then save proposal, show success, notify
- **Missing Subject**: Given on proposal page, when submit without subject, then show error next to field
- **Missing Dates**: Given on proposal page, when submit without times, then show error
- **Attachment Handling**: Given upload and remove PDF, when remove attachment, then file appears then removed
- **Max Time Slots**: Given 4 time slots added, when try to add fifth, then disable button

### Data Entities
**Meeting_Time_Proposal**
- ProposalID (Auto-increment)
- FundID (Required relation)
- Subject (Required, max 255 chars)
- Description (Optional, max 1000 chars)
- Status (Required, values: Under Voting, Completed)
- CreatedByUserID (Required relation)
- CreationDate (Required, auto-generated)

**Proposed_Date**
- ProposedDateID (Auto-increment)
- ProposalID (Required relation)
- ProposedDateTime (Required, must be future)

**Proposal_Attachment**
- AttachmentID (Auto-increment)
- ProposalID (Required relation)
- FileName (Required, PDF only)
- FilePath (Required, valid path)
- UploadedDate (Required, auto-generated)

### Messages
- MSG-MTV-SUC-01: "Proposal sent for voting successfully."
- MSG-MTV-ERR-01: "Meeting Subject is required."
- MSG-MTV-ERR-02: "Please add at least one proposed date and time."
- MSG-MTV-ERR-03: "Invalid file type. Please upload a PDF file only."
- MSG-MTV-NOT-01: "A new vote has started for '[Subject]'."

---

## Story 2: Vote on Proposed Meeting Times

### Basic Information
- **Summary**: Vote on Proposed Meeting Times
- **Story Points**: 5
- **User Story**: As a Board Member, I want to view the proposed times for a new meeting and cast my vote on my preferred options, so that my availability is considered for scheduling.
- **User Roles**: Board Member (Voter)
- **Access Requirements**: Authenticated Board Member for the specific fund

### Description
This user story describes the process by which Board Members cast their votes on proposed meeting times. After a Legal Counsel or Board Secretary creates a proposal, members receive a notification, review details, and select preferred time slot(s). The system aggregates the input and determines the final time based on fund-specific voting rules.

### Business Rules
- One vote per user per proposal
- Voting only during "Under Voting" status
- Result calculated per fund's rule
- At least one selection required
- Vote saved with timestamp

### Process Flow
1. Receives notification and opens voting page
2. Displays meeting details and attachments
3. Optionally downloads attachments
4. Displays proposed date/time options
5. Selects preferred time slot(s)
6. Clicks "Submit Vote"
7. Validates at least one selection
8. Saves vote to database
9. Checks if all members have voted
10. If last vote, updates status to "Completed" and sends notification
11. Displays success message

### Acceptance Criteria
- **Successfully Cast a Vote**: Given on voting page for active proposal, when select time and click "Submit Vote", then vote saved, show success message
- **Cast Final Deciding Vote**: Given last Board Member to vote, when submit vote, then status updated to "Completed", notify creator
- **Attempt to Vote Without Input**: Given on voting page, when click submit without selection, then show validation error
- **View Proposal Attachments**: Given attachments available, when click on document link, then document opens/downloads

### Data Entities
**Meeting_Time_Vote**
- VoteID (Auto-increment primary key)
- ProposalID (Required foreign key to proposal)
- UserID (Required foreign key, must be Board Member)
- ProposedDateID (Required foreign key, must belong to proposal)
- VoteTimestamp (Required, current timestamp)

### Messages
- MSG-VMT-SUC-01: "Your vote has been submitted successfully."
- MSG-VMT-ERR-01: "Please select at least one option before submitting."
- MSG-VMT-NOT-01: "Voting for the meeting '[Subject]' is now complete."
- MSG-VMT-INF-01: "Voting for this proposal is complete."
- MSG-VMT-INF-02: "You have already voted on this proposal."

---

## Story 3: Schedule a New Board Meeting

### Basic Information
- **Summary**: Schedule a New Board Meeting
- **Story Points**: 13
- **User Story**: As a Board Secretary, I want to create a new meeting with a specific date, time, location, agenda, and list of attendees, so that I can formally schedule the meeting and automatically send invitations to all participants.
- **User Roles**: Legal Counsel, Board Secretary (Creator)
- **Access Requirements**: User must be authenticated and have the role of "Legal Counsel" or "Board Secretary" for the fund

### Description
This function allows authorized users to formalize a meeting by defining all its parameters, including the subject, time, location (physical or online), attendees, agenda, and supporting documents. Once successfully created and saved, the system automatically dispatches invitations to all specified attendees, ensuring they are officially informed.

### Business Rules
- Subject is required
- Date must be in future
- End Time > Start Time
- Location specific logic (Room vs Online)
- At least one agenda item required
- Core attendees pre-selected based on roles

### Process Flow
1. Navigate to "Meetings" and click "Schedule New Meeting"
2. System displays the form with Basic Details, Attendees, Agenda, Attachments sections
3. Fill "Basic Details" with subject, type, date, time, location
4. Review and adjust Attendees (core attendees pre-selected)
5. Add Agenda Items (can add multiple)
6. Add Attachments (optional supporting files)
7. Click "Schedule Meeting"
8. System validates all required fields and business rules
9. System saves meeting to database
10. Invitations sent to all attendees
11. Show success message and redirect

### Acceptance Criteria
- **Online Meeting**: Given on scheduling page, when fill all fields, select Online, add agenda, then meeting created, Zoom link generated, invites sent
- **Physical Meeting**: Given on scheduling page, when fill all fields, select Meeting Room, add agenda, then meeting created, invites with location sent
- **Invalid Time**: Given enter Start 10 AM, End 9 AM, when click "Schedule", then error: "End time must be after start time"
- **No Agenda**: Given fill details, no agenda, when click "Schedule", then error: "At least one agenda item is required"

### Data Entities
**Meeting**
- MeetingID (Auto-increment primary key)
- FundID (Required foreign key)
- Subject (Required, max 255 chars, not empty)
- MeetingDate (Required, future date)
- StartTime (Required)
- EndTime (Required, > StartTime)
- LocationType (Required, "Online" or "Room")
- LocationDetails (Required, Zoom link or Room name)
- Status (Required, default "Scheduled")

**Meeting_Agenda_Item**
- AgendaItemID (Auto-increment primary key)
- MeetingID (Required foreign key)
- ItemSubject (Required, max 255 chars, not empty)
- ItemDescription (Optional, max 1000 chars)

### Messages
- MSG-SCM-SUC-01: "Meeting scheduled successfully, invitations sent."
- MSG-SCM-ERR-01: "Meeting Subject is required."
- MSG-SCM-ERR-02: "End time must be after start time."
- MSG-SCM-ERR-03: "Meeting date must be in the future."
- MSG-SCM-ERR-04: "At least one agenda item is required."
- MSG-SCM-NOT-01: "Invitation: You are invited to '[Subject]' on [Date] at [Time]."

---

---

## Story 4: Manage a Live Board Meeting

### Basic Information
- **Summary**: Manage a Live Board Meeting
- **Story Points**: 8
- **User Story**: As a Board Secretary, I want to start a scheduled meeting, mark attendees as present or absent, and officially close the meeting when finished, so that I can maintain an accurate record of the meeting's proceedings.
- **User Roles**: Board Secretary, Legal Counsel (Meeting Manager), Board Members (Attendees)
- **Access Requirements**: Meeting Manager must be authenticated with appropriate role; meeting must be scheduled

### Description
This user story covers the real-time management of a board meeting from start to finish. The Meeting Manager (Board Secretary or Legal Counsel) can start the meeting, track attendance, and formally close it. This creates an official record of who participated and establishes the foundation for generating meeting minutes.

### Business Rules
- Only scheduled meetings can be started
- Meeting can only be started by authorized users
- Attendance tracking is mandatory
- Meeting must be officially closed
- Status changes: Scheduled → In Progress → Finished

### Process Flow
1. Navigate to meeting details page for scheduled meeting
2. Click "Start Meeting" button
3. System updates status to "In Progress"
4. Display attendance tracking interface
5. Mark each attendee as Present/Absent
6. Manage meeting proceedings
7. Click "End Meeting" when finished
8. System updates status to "Finished"
9. Generate attendance summary
10. Enable minutes generation

### Acceptance Criteria
- **Start Meeting**: Given scheduled meeting, when click "Start Meeting", then status changes to "In Progress"
- **Track Attendance**: Given meeting in progress, when mark attendees, then attendance recorded
- **End Meeting**: Given meeting in progress, when click "End Meeting", then status changes to "Finished"
- **Generate Summary**: Given meeting finished, then attendance summary available

### Data Entities
**Meeting** (Status Updates)
- Status: "Scheduled" → "In Progress" → "Finished"
- ActualStartTime: Timestamp when meeting started
- ActualEndTime: Timestamp when meeting ended

**Meeting_Attendee** (Attendance Tracking)
- AttendeeID (Auto-increment)
- MeetingID (Required foreign key)
- UserID (Required foreign key)
- AttendanceStatus (Required: "Present", "Absent")
- MarkedByUserID (Required: who marked attendance)
- MarkedDateTime (Required: when marked)

### Messages
- MSG-MLM-SUC-01: "Meeting started successfully."
- MSG-MLM-SUC-02: "Meeting ended successfully."
- MSG-MLM-INF-01: "Attendance has been recorded for all participants."

---

## Story 5: Generate and Circulate Meeting Minutes

### Basic Information
- **Summary**: Generate and Circulate Meeting Minutes
- **Story Points**: 8
- **User Story**: As a Board Secretary, I want to draft the meeting minutes in a structured format and send them to attendees for electronic signature, so that I can create an official record of the meeting's decisions and outcomes.
- **User Roles**: Board Secretary, Legal Counsel (Minutes Creator)
- **Access Requirements**: User must be authenticated with appropriate role; meeting must be "Finished"

### Description
This user story enables the creation of formal meeting minutes after a meeting has concluded. The Secretary drafts the minutes content, reviews the circulation list (attendees who were present), and sends the document for electronic signatures to create the official record.

### Business Rules
- Only "Finished" meetings can have minutes generated
- Minutes content cannot be empty when sending for signature
- Only attendees marked "Present" receive signature requests
- Minutes status: Draft → Pending Signature → Completed

### Process Flow
1. Navigate to finished meeting details
2. Click "Generate Minutes" or access existing draft
3. System displays minutes drafting interface
4. Draft minutes content using rich text editor
5. Save as draft (optional intermediate step)
6. Review circulation list (present attendees)
7. Click "Send for Signature"
8. System validates content is not empty
9. Update status to "Pending Signature"
10. Send notifications to present attendees
11. Display success confirmation

### Acceptance Criteria
- **Draft Minutes**: Given finished meeting, when draft content and save, then minutes saved as draft
- **Send for Signature**: Given drafted minutes, when send for signature, then status updated, notifications sent
- **Prevent Empty Minutes**: Given empty content, when try to send, then validation error shown
- **Notify Only Attendees**: Given 3 of 5 marked present, when send minutes, then only 3 receive notifications

### Data Entities
**Meeting_Minutes**
- MinutesID (Auto-increment primary key)
- MeetingID (Required foreign key to finished meeting)
- Content (Optional rich text/JSON, mandatory before sending)
- Status (Required: "Draft", "Pending Signature", "Completed")
- CreatedByUserID (Required: Board Secretary/Legal Counsel)
- CreationDate (Required: auto-generated timestamp)

**Minutes_Signature**
- SignatureID (Auto-increment primary key)
- MinutesID (Required foreign key)
- SignatoryUserID (Required: must be attendee)
- SignatureStatus (Required: "Pending", "Signed")
- SignatureDate (Optional: filled when signed)

### Messages
- MSG-GMM-SUC-01: "Meeting minutes sent for signature successfully."
- MSG-GMM-ERR-01: "Minutes content cannot be empty."
- MSG-GMM-ERR-02: "Cannot circulate minutes - no recorded attendees."
- MSG-GMM-NOT-01: "Minutes for '[Meeting Subject]' ready for review and signature."

---

## Story 6: Electronically Sign Meeting Minutes

### Basic Information
- **Summary**: Electronically Sign Meeting Minutes
- **Story Points**: 8
- **User Story**: As a Board Member, I want to review drafted meeting minutes and apply my electronic signature to approve them, so that I can formally validate the meeting record.
- **User Roles**: Board Member (Signatory)
- **Access Requirements**: Must be authenticated Board Member marked as "Present" with pending signature request

### Description
This feature enables board members to review and electronically sign meeting minutes. After minutes are circulated, attendees receive notifications, review the content, and apply their pre-configured electronic signatures. The system tracks signatures and generates a final PDF when complete.

### Business Rules
- Only attendees can sign
- Signatures are final and cannot be undone
- Signature pulled from user profile
- Final PDF generated when all signatures collected
- Status changes to "Completed" when fully signed

### Process Flow
1. Receive notification and navigate to signing page
2. System displays minutes in read-only format
3. Display current signature status of all signatories
4. Click "Approve and E-Sign" button
5. Apply electronic signature from profile
6. Update status to "Signed" with timestamp
7. Check if all required attendees have signed
8. Generate final PDF if all signatures collected
9. Update minutes status to "Completed"
10. Display success message

### Acceptance Criteria
- **Successfully Sign**: Given signature request, when click "Approve and E-Sign", then status updated, message shown
- **Final Signatory**: Given last to sign, when submit, then final PDF generated, status "Completed"
- **View Final PDF**: Given completed meeting, when click minutes link, then shows final PDF with signatures
- **Unauthorized Access**: Given non-attendee, when try to sign, then error message, access denied

### Data Entities
**Minutes_Signature** (Updates)
- SignatureStatus: "Pending" → "Signed"
- SignatureDate: Timestamped on signing
- SignatureData: Pulled from user profile

**Meeting_Minutes** (Final Updates)
- Status: "Pending Signature" → "Completed"
- FinalDocumentPath: Stores signed PDF location

### Messages
- MSG-ESM-SUC-01: "Your signature has been successfully applied."
- MSG-ESM-ERR-01: "You are not authorized to perform this action."
- MSG-ESM-ERR-02: "Electronic signature not configured in profile."
- MSG-ESM-INF-01: "You have already signed these minutes."
- MSG-ESM-NOT-02: "Signing process for '[Meeting Subject]' complete, final document available."

---

## Story 7: View Scheduled Meetings

### Basic Information
- **Summary**: View Scheduled Meetings
- **Story Points**: 3
- **User Story**: As any authenticated user, I want to view a list of all scheduled meetings for the funds I'm associated with, so that I can see upcoming meetings and their details.
- **User Roles**: All Users (Fund Manager, Board Member, Legal Counsel, Board Secretary)
- **Access Requirements**: User must be authenticated and associated with the fund

### Description
This user story provides a comprehensive view of all scheduled meetings for the funds a user is associated with. Users can see meeting details, status, and access relevant actions based on their role and the meeting status.

### Business Rules
- Users only see meetings for funds they're associated with
- Meeting list shows current and future meetings
- Past meetings may be included for reference
- Different actions available based on user role and meeting status

### Process Flow
1. Navigate to "Meetings" section
2. System displays list of meetings for user's associated funds
3. Show meeting details: subject, date, time, location, status
4. Display appropriate actions based on user role and meeting status
5. Allow filtering by date range, status, or fund
6. Enable sorting by date, subject, or status

### Acceptance Criteria
- **View Meeting List**: Given authenticated user, when navigate to meetings, then see list of associated meetings
- **Role-Based Actions**: Given different user roles, when view meetings, then see appropriate actions
- **Filter Meetings**: Given meeting list, when apply filters, then see filtered results
- **Meeting Details**: Given meeting in list, when click on meeting, then see detailed view

### Data Entities
This story primarily reads from existing Meeting entity and related tables:
- Meeting (all fields for display)
- Meeting_Attendee (to determine user association)
- Fund (to show fund names)

### Messages
- MSG-VSM-INF-01: "No meetings found for the selected criteria."
- MSG-VSM-INF-02: "You have [X] upcoming meetings."

---

## Story 8: Modify an Upcoming Meeting

### Basic Information
- **Summary**: Modify an Upcoming Meeting
- **Story Points**: 5
- **User Story**: As a Board Secretary or Legal Counsel, I want to edit the details of a scheduled meeting (subject, time, location, agenda, attendees), so that I can update the meeting information and notify all participants of the changes.
- **User Roles**: Board Secretary, Legal Counsel
- **Access Requirements**: User must be authenticated with appropriate role; meeting must be "Scheduled" status

### Description
This functionality allows authorized users to modify details of scheduled meetings before they occur. Changes can include basic details, attendees, agenda items, and attachments. All attendees are automatically notified of changes.

### Business Rules
- Only "Scheduled" meetings can be modified
- Only Board Secretary or Legal Counsel can modify meetings
- All attendees must be notified of changes
- Modified meetings maintain their original meeting ID
- Change history is tracked for audit purposes

### Process Flow
1. Navigate to scheduled meeting details
2. Click "Modify Meeting" button
3. System displays editable form with current meeting details
4. Make necessary changes to any section
5. Review updated attendee list
6. Click "Save Changes"
7. System validates all changes
8. Update meeting record in database
9. Send change notifications to all attendees
10. Display success message and updated meeting details

### Acceptance Criteria
- **Modify Basic Details**: Given scheduled meeting, when change subject/time/location and save, then meeting updated, notifications sent
- **Update Agenda**: Given scheduled meeting, when modify agenda items and save, then agenda updated, attendees notified
- **Change Attendees**: Given scheduled meeting, when add/remove attendees and save, then attendee list updated, notifications sent
- **Prevent Invalid Changes**: Given try to set end time before start time, when save, then validation error shown

### Data Entities
**Meeting** (Updates to existing record)
- All fields can be modified except MeetingID
- LastModifiedDate: Updated timestamp
- ModifiedByUserID: User who made changes

**Meeting_Change_Log** (New audit trail)
- ChangeLogID (Auto-increment primary key)
- MeetingID (Required foreign key)
- ChangeDescription (Required: what was changed)
- ChangedByUserID (Required: who made the change)
- ChangeDateTime (Required: when change was made)

### Messages
- MSG-MUM-SUC-01: "Meeting updated successfully. All attendees have been notified."
- MSG-MUM-ERR-01: "Cannot modify meeting - invalid status."
- MSG-MUM-ERR-02: "End time must be after start time."
- MSG-MUM-NOT-01: "Meeting '[Subject]' has been updated. Please review the changes."

---

## Story 9: Cancel an Upcoming Meeting

### Basic Information
- **Summary**: Cancel an Upcoming Meeting
- **Story Points**: 2
- **User Story**: As a Board Secretary or Legal Counsel, I want to cancel a scheduled meeting and provide a reason for the cancellation, so that I can inform all attendees that the meeting will not take place.
- **User Roles**: Board Secretary, Legal Counsel
- **Access Requirements**: User must be authenticated with appropriate role; meeting must be "Scheduled" status

### Description
This user story allows authorized users to cancel scheduled meetings when necessary. The cancellation requires a reason and automatically notifies all attendees. The meeting record is preserved for audit purposes with a "Cancelled" status.

### Business Rules
- Only "Scheduled" meetings can be cancelled
- Only Board Secretary or Legal Counsel can cancel meetings
- Cancellation reason is mandatory
- All attendees must be notified immediately
- Cancelled meetings cannot be reactivated (new meeting must be created)
- Meeting record is preserved for audit trail

### Process Flow
1. Navigate to scheduled meeting details
2. Click "Cancel Meeting" button
3. System displays cancellation confirmation dialog
4. Enter mandatory cancellation reason
5. Click "Confirm Cancellation"
6. System validates reason is provided
7. Update meeting status to "Cancelled"
8. Record cancellation details and timestamp
9. Send cancellation notifications to all attendees
10. Display success message and return to meetings list

### Acceptance Criteria
- **Cancel with Reason**: Given scheduled meeting, when provide reason and confirm cancellation, then meeting cancelled, attendees notified
- **Require Cancellation Reason**: Given try to cancel without reason, when confirm, then validation error shown
- **Prevent Invalid Cancellation**: Given meeting not in "Scheduled" status, when try to cancel, then action not available
- **Preserve Meeting Record**: Given cancelled meeting, when view meeting details, then see cancellation info and original details

### Data Entities
**Meeting** (Status Update)
- Status: "Scheduled" → "Cancelled"
- CancellationReason: Required text field
- CancelledByUserID: User who cancelled the meeting
- CancellationDate: Timestamp of cancellation

### Messages
- MSG-CUM-SUC-01: "Meeting cancelled successfully. All attendees have been notified."
- MSG-CUM-ERR-01: "Cancellation reason is required."
- MSG-CUM-ERR-02: "Cannot cancel meeting - invalid status."
- MSG-CUM-NOT-01: "Meeting '[Subject]' scheduled for [Date] has been cancelled. Reason: [Reason]"

---

## Implementation Notes for Jira Import

### Epic Structure
Create a parent Epic in Jira with the following details:
- **Epic Name**: Meetings
- **Epic Summary**: Complete meeting management system for board meetings
- **Epic Description**: Comprehensive meeting lifecycle management including time proposal voting, scheduling, live meeting management, minutes generation, electronic signatures, and meeting administration.

### Story Import Order
Import stories in the following order to maintain logical dependencies:
1. Story 7: View Scheduled Meetings (foundational viewing capability)
2. Story 1: Propose Meeting Times for Voting (initial scheduling process)
3. Story 2: Vote on Proposed Meeting Times (voting process)
4. Story 3: Schedule a New Board Meeting (formal scheduling)
5. Story 8: Modify an Upcoming Meeting (meeting management)
6. Story 9: Cancel an Upcoming Meeting (meeting management)
7. Story 4: Manage a Live Board Meeting (meeting execution)
8. Story 5: Generate and Circulate Meeting Minutes (post-meeting documentation)
9. Story 6: Electronically Sign Meeting Minutes (final documentation approval)

### Labels and Components
Consider adding these labels/components in Jira:
- Component: Meetings
- Labels: meeting-management, scheduling, voting, minutes, e-signature, notifications
- Priority: Based on business requirements (suggest High for core scheduling, Medium for advanced features)

### Story Points Summary
- Total Story Points: 60
- Average per Story: 6.7
- Complexity Distribution: 2 simple (2-3 points), 4 medium (5-8 points), 3 complex (8-13 points)

---

**Document Complete - Ready for Jira Import**

This document contains all 9 Meetings user stories with complete specifications including business rules, process flows, acceptance criteria, data entities, and messages. Each story is structured for easy import into Jira with all necessary details for development teams.
