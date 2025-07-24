
# User Story 1: Propose Meeting Times for Voting

## Summary
This functionality allows authorized users—specifically the **Legal Counsel** and the **Board Secretary**—to create and send out a poll with several proposed meeting schedules to the fund's board members. The goal is to efficiently determine the most suitable time for an upcoming meeting by collecting votes electronically.

---

## Target User Personas

- **Legal Counsel**: Responsible for creating meeting time proposals to ensure legal and governance alignment.
- **Board Secretary (أمين سر المجلس)**: Responsible for coordinating and scheduling board meetings.

---

## Main User Story

| Field | Description |
|-------|-------------|
| **Name** | Propose Meeting Times for Voting |
| **User Story** | As a Board Secretary, I want to create a proposal with multiple date and time options for an upcoming meeting, so that I can send it to board members to vote on the most convenient time. |
| **Story Points** | 8 |
| **User Roles** | Legal Counsel, Board Secretary |
| **Access Requirements** | Authenticated users with the role "Legal Counsel" or "Board Secretary" for the specific fund |
| **Trigger** | Manually initiated from the "Meetings" section |
| **Frequency of Use** | Medium |
| **Pre-condition** | Logged-in user on meetings list page |
| **Business Rules** | - Subject is mandatory<br>- At least one proposed time<br>- Maximum 4 time slots<br>- Attachments must be PDFs |
| **Post-condition** | Proposal saved with "Under Voting" status, notifications sent |
| **Risk** | Low – fallback to in-app alerts if notifications fail |
| **Assumptions** | Board member list is defined and up-to-date |
| **UX/UI Link** | To be linked to Figma wireframes |

---

## Process Flow

| Step | Action Description | Actor | Message Code | Notes |
|------|--------------------|-------|---------------|-------|
| 1 | Navigate to "Meetings" and click "Propose New Meeting Time" | Legal Counsel / Secretary | N/A | Initiates workflow |
| 2 | System shows form | System | N/A | Form includes all fields |
| 3 | Enter "Meeting Subject" | Legal Counsel / Secretary | MSG-MTV-ERR-01 | Required field |
| 4 | Optionally enter description | Legal Counsel / Secretary | N/A | - |
| 5 | Optionally upload PDF(s) | Legal Counsel / Secretary | MSG-MTV-ERR-03/04 | - |
| 6 | Add 1–4 time slots | Legal Counsel / Secretary | MSG-MTV-ERR-02 | Time picker UI |
| 7 | Click "Send for Voting" | Legal Counsel / Secretary | N/A | - |
| 8 | Form validation | System | MSG-MTV-ERR-01/02 | Fails if incomplete |
| 9 | Save proposal to DB | System | N/A | Set status = Under Voting |
|10 | Notify all board members | System | MSG-MTV-NOT-01 | Via email/in-app |
|11 | Show success and redirect | System | MSG-MTV-SUC-01 | - |

---

## Alternative Flows

| Scenario | Condition | Action | Message Code | Resolution |
|----------|-----------|--------|---------------|------------|
| Missing Subject | Subject not filled | Show error | MSG-MTV-ERR-01 | User must enter subject |
| No Proposed Times | No date/time added | Show error | MSG-MTV-ERR-02 | User must add a time |
| Cancel Creation | Click Cancel | Confirm discard | MSG-MTV-WRN-01 | Show modal |
| Invalid Attachment | Upload non-PDF | Show error | MSG-MTV-ERR-03 | Only PDF allowed |

---

## Acceptance Criteria

| Scenario | Given | When | Then |
|----------|-------|------|------|
| Successful Proposal | On proposal page | Valid input and submit | Save proposal, show success, notify |
| Missing Subject | On proposal page | Submit without subject | Show error next to field |
| Missing Dates | On proposal page | Submit without times | Show error |
| Attachment Handling | Upload and remove PDF | Remove attachment | File appears then removed |
| Max Time Slots | 4 time slots added | Try to add fifth | Disable button |

---

## Screen Elements

| ID | Type | English | Arabic | Required | Rules | Entity | Interaction |
|----|------|---------|--------|----------|--------|--------|-------------|
| ELM-MTV-001 | Title | Create Meeting Time Proposal | إنشاء تصويت على موعد اجتماع | N/A | N/A | N/A | <h1> |
| ELM-MTV-002 | Input | Meeting Subject | موضوع الاجتماع | Required | Not empty | Meeting_Time_Proposal.Subject | Text |
| ELM-MTV-003 | Text Area | Description | الوصف | Optional | Max 1000 chars | Meeting_Time_Proposal.Description | Text |
| ELM-MTV-004 | File Upload | Attachments | المرفقات | Optional | PDF only | Proposal_Attachment | File |
| ELM-MTV-005 | Dynamic List | Proposed Times | المواعيد المقترحة | Required | 1-4 entries | Proposed_Date | Picker |
| ELM-MTV-006 | Button | Add Proposed Time | إضافة موعد مقترح | N/A | Max 4 | - | Click |
| ELM-MTV-007 | Button | Send for Voting | إرسال للتصويت | N/A | Validation | - | Submit |
| ELM-MTV-008 | Button | Cancel | إلغاء | N/A | Discard form | - | Click |

---

## Data Entities

### Entity: Meeting_Time_Proposal

| Field | Arabic | Required | Type | Length | Rules | Sample |
|-------|--------|----------|------|--------|-------|--------|
| ProposalID | معرف المقترح | Yes | Integer | N/A | Auto-increment | 101 |
| FundID | معرف الصندوق | Yes | Relation | N/A | Must exist | 333 |
| Subject | الموضوع | Yes | Text | 255 | Not empty | Q3 Budget |
| Description | الوصف | No | Text | 1000 | - | Meeting for Q3 |
| Status | الحالة | Yes | Dropdown | 50 | Values: Under Voting, Completed | Under Voting |
| CreatedByUserID | معرف المنشئ | Yes | Relation | N/A | Must exist | 54 |
| CreationDate | تاريخ الإنشاء | Yes | DateTime | N/A | Default now | 2025-07-24 12:30:00 |

### Entity: Proposed_Date

| Field | Arabic | Required | Type | Rules | Sample |
|-------|--------|----------|------|-------|--------|
| ProposedDateID | معرف التاريخ المقترح | Yes | Integer | Auto-increment | 201 |
| ProposalID | معرف المقترح | Yes | Relation | Must exist | 101 |
| ProposedDateTime | التاريخ والوقت المقترح | Yes | DateTime | Must be in future | 2025-08-01 10:00 |

### Entity: Proposal_Attachment

| Field | Arabic | Required | Type | Rules | Sample |
|-------|--------|----------|------|-------|--------|
| AttachmentID | معرف المرفق | Yes | Integer | Auto-increment | 301 |
| ProposalID | معرف المقترح | Yes | Relation | Must exist | 101 |
| FileName | اسم الملف | Yes | Text | PDF only | Q2_Report.pdf |
| FilePath | مسار الملف | Yes | Text | Valid path | /attachments/q2_report.pdf |
| UploadedDate | تاريخ الرفع | Yes | DateTime | Default now | 2025-07-24 12:30:00 |

---

## Messages / Notifications

| Code | English | Arabic | Type | Method |
|------|---------|--------|------|--------|
| MSG-MTV-SUC-01 | Proposal sent for voting successfully. | تم إرسال المقترح للتصويت بنجاح. | Success | In-App |
| MSG-MTV-ERR-01 | Meeting Subject is required. | موضوع الاجتماع مطلوب. | Validation | In-App |
| MSG-MTV-ERR-02 | Please add at least one proposed date and time. | يرجى إضافة موعد مقترح واحد على الأقل. | Validation | In-App |
| MSG-MTV-ERR-03 | Invalid file type. Please upload a PDF file only. | نوع الملف غير صالح. يرجى رفع ملف PDF فقط. | Validation | In-App |
| MSG-MTV-ERR-04 | File upload failed. Please try again. | فشل رفع الملف. يرجى المحاولة مرة أخرى. | Error | In-App |
| MSG-MTV-WRN-01 | Are you sure you want to discard your changes? | هل أنت متأكد من أنك تريد تجاهل التغييرات؟ | Warning | Modal |
| MSG-MTV-NOT-01 | A new vote has started for “[Subject]”. | تم بدء تصويت جديد بخصوص “[الموضوع]”. | Notification | In-App, Email |

---





# User Story 2: Vote on Proposed Meeting Times

## 1. Introduction
This user story describes the process by which Board Members cast their votes on proposed meeting times. After a Legal Counsel or Board Secretary creates a proposal, members receive a notification, review details, and select preferred time slot(s). The system aggregates the input and determines the final time based on fund-specific voting rules.

## 2. Main User Story

| Field              | Description                                                                 |
|--------------------|-----------------------------------------------------------------------------|
| **Name**           | Vote on Proposed Meeting Times                                              |
| **User Story**     | As a Board Member, I want to view the proposed times for a new meeting and cast my vote on my preferred options, so that my availability is considered for scheduling. |
| **Story Points**   | 5                                                                           |
| **User Roles**     | Board Member (Voter)                                                        |
| **Access Requirements** | Authenticated Board Member for the specific fund                         |
| **Trigger**        | User receives notification or navigates to the "Meetings" section           |
| **Frequency**      | Medium                                                                      |
| **Pre-condition**  | A proposal exists in "Under Voting" status; user hasn't voted yet           |
| **Business Rules** | - One vote per user per proposal  
- Voting only during "Under Voting"  
- Result calculated per fund's rule |
| **Post-condition** | Vote saved; if last vote, status updated to "Completed"                     |
| **Risk**           | Users may misinterpret multiple selection. Mitigation: Clear UI guidance.  |
| **Assumptions**    | System identifies eligible members correctly                                |
| **UX/UI**          | To be linked to Figma mockups                                               |

---

## 3. Process Flow

| Step | Action Description                                                                 | Actor         | Message Code     | Notes                                  |
|------|-------------------------------------------------------------------------------------|---------------|------------------|----------------------------------------|
| 1    | Receives notification and opens voting page                                        | Board Member  | N/A              | Entry point                            |
| 2    | Displays meeting details and attachments                                           | System        | N/A              |                                        |
| 3    | Optionally downloads attachments                                                   | Board Member  | N/A              | Optional                                |
| 4    | Displays proposed date/time options                                                | System        | N/A              |                                        |
| 5    | Selects preferred time slot(s)                                                     | Board Member  | MSG-VMT-ERR-01   |                                        |
| 6    | Clicks “Submit Vote”                                                               | Board Member  | N/A              |                                        |
| 7    | Validates at least one selection                                                   | System        | MSG-VMT-ERR-01   |                                        |
| 8    | Saves vote to database                                                             | System        | N/A              |                                        |
| 9    | Checks if all members have voted                                                   | System        | N/A              |                                        |
| 10   | If last vote, updates status to “Completed” and sends notification                 | System        | MSG-VMT-NOT-01   |                                        |
| 11   | Displays success message                                                           | System        | MSG-VMT-SUC-01   |                                        |

---

## 4. Alternative Flows

| Alternative Scenario        | Condition                                     | Action                                                                            | Message Code     | Resolution                                              |
|----------------------------|-----------------------------------------------|-----------------------------------------------------------------------------------|------------------|---------------------------------------------------------|
| Submit Without Selection   | No time slot selected                         | Prevent submission; show error                                                    | MSG-VMT-ERR-01   | User selects a time and resubmits                      |
| Accessing a Completed Vote | Proposal already marked as “Completed”        | Show results only; disable vote controls                                          | MSG-VMT-INF-01   | View-only mode                                          |
| Double Voting Attempt      | User has already voted                        | Show selected options; disable changes                                            | MSG-VMT-INF-02   | Prevent duplicate voting                                |

---

## 5. Acceptance Criteria

| Scenario                      | Given                                                | When                                            | Then                                                                 |
|------------------------------|------------------------------------------------------|-------------------------------------------------|----------------------------------------------------------------------|
| Successfully Cast a Vote     | On voting page for active proposal                   | Select time and click “Submit Vote”             | Vote saved, show “Your vote has been submitted successfully.”       |
| Cast Final Deciding Vote     | Last Board Member to vote                            | Submit vote                                     | Status updated to “Completed”, notify creator                       |
| Attempt to Vote Without Input| On voting page                                       | Click submit without selection                  | Show “Please select at least one option before submitting.”         |
| View Proposal Attachments    | Attachments available                                | Click on document link                          | Document opens/downloads                                            |

---

## 6. Screen Elements

| ID            | Type               | Name (EN)            | Name (AR)             | Required | Validation        | Business Logic                     | Data Entity             | Interaction | Accessibility Notes                      |
|---------------|--------------------|-----------------------|------------------------|----------|--------------------|-------------------------------------|--------------------------|-------------|------------------------------------------|
| ELM-VMT-001   | Label              | Meeting Subject       | موضوع الاجتماع         | N/A      | N/A                | Shows meeting subject               | Meeting_Time_Proposal    | Read        | Should be <h2>                           |
| ELM-VMT-002   | Text Block         | Description           | الوصف                 | N/A      | N/A                | Shows description                   | Meeting_Time_Proposal    | Read        |                                          |
| ELM-VMT-003   | Link List          | Attachments           | المرفقات              | N/A      | N/A                | Download links                      | Proposal_Attachment      | Click       | Use descriptive names                    |
| ELM-VMT-004   | Radio/Checkbox     | Proposed Times        | المواعيد المقترحة     | Required | At least one       | Time selection input                | Proposed_Date            | Select      | Wrap in <fieldset> and <legend>         |
| ELM-VMT-005   | Button             | Submit Vote           | إرسال التصويت         | N/A      | N/A                | Final action                        | Meeting_Time_Vote        | Click       | Primary action                          |
| ELM-VMT-006   | Text Block         | Status                | الحالة                | N/A      | N/A                | Current status                      | Meeting_Time_Proposal    | Read        | High contrast for readability           |

---

## 7. Data Entities

### Entity: Meeting_Time_Vote

| Attribute       | Arabic Name       | Required   | Type          | Length | Default            | Notes                                | Sample EN | Sample AR |
|----------------|-------------------|------------|---------------|--------|---------------------|--------------------------------------|-----------|-----------|
| VoteID         | معرف التصويت      | Yes        | Auto Integer  | N/A    | Primary Key         |                                      | 501       | 501       |
| ProposalID     | معرف المقترح       | Yes        | Foreign Key   | N/A    | Link to Proposal     | Must be an existing proposal         | 101       | 101       |
| UserID         | معرف المستخدم      | Yes        | Foreign Key   | N/A    | Link to Users        | Must be a Board Member               | 75        | 75        |
| ProposedDateID | معرف التاريخ المقترح| Yes        | Foreign Key   | N/A    |                     | Must belong to this proposal         | 201       | 201       |
| VoteTimestamp  | وقت التصويت        | Yes        | DateTime      | N/A    | Current Timestamp    |                                      | 2025-07-25 09:15:00 | 2025-07-25 09:15:00 |

---

## 8. Messages and Notifications

| Code             | English Message                                | Arabic Message                                | Type         | Delivery      |
|------------------|--------------------------------------------------|------------------------------------------------|--------------|----------------|
| MSG-VMT-SUC-01   | Your vote has been submitted successfully.       | تم إرسال تصويتك بنجاح.                         | Success       | In-App         |
| MSG-VMT-ERR-01   | Please select at least one option before submitting.| يرجى اختيار خيار واحد على الأقل قبل الإرسال. | Validation    | In-App         |
| MSG-VMT-NOT-01   | Voting for the meeting "[Subject]" is now complete.| اكتمل التصويت لاجتماع "[الموضوع]".          | Notification | In-App, Email  |
| MSG-VMT-INF-01   | Voting for this proposal is complete.            | التصويت لهذا المقترح قد اكتمل.                 | Info         | In-App         |
| MSG-VMT-INF-02   | You have already voted on this proposal.         | لقد قمت بالتصويت على هذا المقترح مسبقًا.       | Info         | In-App         |




# User Story 3: Schedule a New Board Meeting

This document outlines the user story for scheduling a new board meeting. This function is a core administrative task performed by either the Legal Counsel or the Board Secretary. It allows them to formalize a meeting by defining all its parameters, including the subject, time, location (physical or online), attendees, agenda, and supporting documents.

Once a meeting is successfully created and saved, the system automatically dispatches invitations to all specified attendees, ensuring they are officially informed. The newly scheduled meeting then appears in the relevant users' calendars or meeting lists within the application.

## Target User Personas

- **Board Secretary (أمين سر المجلس)**: The primary user responsible for the administrative creation and coordination of board meetings.
- **Legal Counsel (المسؤول القانوني)**: An authorized user who can also schedule meetings, often to address legal or governance matters.
- **Attendees (Board Members, Fund Manager, etc.)**: The recipients of the meeting invitation who will participate in the scheduled event.

## Main User Story Table

| Field | Description |
|-------|-------------|
| Name | Schedule a New Board Meeting |
| User Story | As a Board Secretary, I want to create a new meeting with a specific date, time, location, agenda, and list of attendees, so that I can formally schedule the meeting and automatically send invitations to all participants. |
| Story Points | 13 |
| User Roles | Legal Counsel, Board Secretary (Creator). |
| Access Requirements | User must be authenticated and have the role of "Legal Counsel" or "Board Secretary" for the fund. |
| Trigger | A meeting time has been agreed upon (either formally or informally), and the user needs to create the official event in the system. |
| Frequency of Use | Medium |
| Pre-condition | The user is logged into the system and has navigated to the meetings section for a specific fund. |
| Business Rules | - Subject is required. <br> - Date must be in future. <br> - End Time > Start Time. <br> - Location specific logic. <br> - At least one agenda item required. |
| Post-condition | Meeting saved with status "Scheduled" or "Upcoming", and invitations sent. |
| Risk | Medium: Failed notifications may result in uninformed attendees. |
| Assumptions | Correct user roles/permissions, attendee list available. |
| UX/UI Design Link | N/A |

## Process Flow Table

| Step | Action Description | Actor | Related Message Codes | Notes |
|------|---------------------|--------|------------------------|-------|
| 1 | Navigate to "Meetings" and click "Schedule New Meeting" | Board Secretary / Legal Counsel | N/A | Initiates workflow |
| 2 | System displays the form | System | N/A | Includes Basic Details, Attendees, Agenda, Attachments |
| 3 | Fill "Basic Details" | Board Secretary / Legal Counsel | MSG-SCM-ERR-01, 02, 03 | Location conditional logic |
| 4 | Review and adjust Attendees | Board Secretary / Legal Counsel | N/A | Core attendees pre-selected |
| 5 | Add Agenda Item | Board Secretary / Legal Counsel | MSG-SCM-ERR-04 | Add multiple items |
| 6 | Add Attachments (optional) | Board Secretary / Legal Counsel | N/A | General supporting files |
| 7 | Click "Schedule Meeting" | Board Secretary / Legal Counsel | N/A | Submit form |
| 8 | System validates | System | MSG-SCM-ERR-01 to 04 | Inline validation |
| 9 | System saves meeting | System | N/A | Database persistence |
| 10 | Invitations sent | System | MSG-SCM-NOT-01 | To all attendees |
| 11 | Show success and redirect | System | MSG-SCM-SUC-01 | Confirm to user |

## Alternative Flow Table

| Scenario | Condition | Action | Related Message Codes | Resolution |
|----------|-----------|--------|------------------------|------------|
| End Time Before Start | Invalid time entered | Block submission | MSG-SCM-ERR-02 | Correct time |
| Past Date | Date in past | Block submission | MSG-SCM-ERR-03 | Use future date |
| Missing Required Fields | Fields empty | Show errors | MSG-SCM-ERR-01, 04 | Fill all fields |
| User Cancels | Clicks "Cancel" | Prompt to confirm | MSG-SCM-WRN-01 | Discard data if confirmed |

## Acceptance Criteria Table

| Scenario | Given | When | Then |
|----------|-------|------|------|
| Online Meeting | On scheduling page | Fill all fields, select Online, add agenda | Meeting created, Zoom link generated, invites sent |
| Physical Meeting | On scheduling page | Fill all fields, select Meeting Room, add agenda | Meeting created, invites with location sent |
| Invalid Time | Enter Start 10 AM, End 9 AM | Click "Schedule" | Error: "End time must be after start time" |
| No Agenda | Fill details, no agenda | Click "Schedule" | Error: "At least one agenda item is required" |

## Screen Elements Table

| Element ID | Type | English | Arabic | Required | Validation | Logic | Data Entity | Interaction | Notes |
|------------|------|---------|--------|----------|------------|-------|-------------|-------------|-------|
| ELM-SCM-001 | Input | Meeting Subject | موضوع الاجتماع | Required | Not empty | N/A | Meeting.Subject | Type | Label present |
| ELM-SCM-002 | Dropdown | Meeting Type | نوع الاجتماع | Required | N/A | e.g., Recurring, Annual | Meeting.Type | Select | N/A |
| ELM-SCM-003 | Date Picker | Date | التاريخ | Required | Future date | N/A | Meeting.Date | Select | Keyboard accessible |
| ELM-SCM-004 | Time Picker | Start Time | موعد البداية | Required | N/A | N/A | Meeting.StartTime | Select | N/A |
| ELM-SCM-005 | Time Picker | End Time | موعد النهاية | Required | > Start Time | N/A | Meeting.EndTime | Select | N/A |
| ELM-SCM-006 | Radio | Location | المكان | Required | N/A | Toggle room/link field | Meeting.LocationType | Select | Use fieldset/legend |
| ELM-SCM-007 | Text | Meeting Room | قاعة الاجتماعات | Conditionally Required | Not empty | If Room selected | Meeting.LocationDetails | Type | N/A |
| ELM-SCM-008 | Label | Meeting Link | لينك الاجتماع | N/A | N/A | If Online selected | Meeting.LocationDetails | Read | N/A |
| ELM-SCM-009 | Checkbox | Attendees | الحضور | Required | N/A | Core roles checked | Meeting_Attendee | Select | Labeled checkboxes |
| ELM-SCM-010 | Dynamic | Agenda | أجندة الاجتماع | Required | ≥ 1 item | Add/remove | Meeting_Agenda_Item | Type | N/A |
| ELM-SCM-011 | Button | Schedule Meeting | إنشاء الاجتماع | N/A | N/A | Submit form | N/A | Click | Primary action |

## Data Entities Table

### Entity: Meeting

| Attribute | Arabic | Mandatory | Type | Length | Integration | Default | Condition | Rules | Arabic Sample | English Sample |
|-----------|--------|-----------|------|--------|-------------|---------|-----------|-------|----------------|----------------|
| MeetingID | معرف الاجتماع | Yes | Auto Int | N/A | DB Primary Key | N/A | N/A | N/A | 45 | 45 |
| FundID | معرف الصندوق | Yes | Relation | N/A | FK to Funds | N/A | N/A | N/A | 333 | 333 |
| Subject | الموضوع | Yes | Text | 255 | DB | N/A | N/A | Not empty | اجتماع مجلس الإدارة الشهري | Monthly Board Meeting |
| MeetingDate | تاريخ الاجتماع | Yes | Date | N/A | DB | N/A | N/A | Future date | 2025-08-15 | 2025-08-15 |
| StartTime | وقت البدء | Yes | Time | N/A | DB | N/A | N/A | N/A | 10:00:00 | 10:00:00 |
| LocationType | نوع المكان | Yes | Text | 50 | DB | N/A | N/A | Online or Room | أونلاين | Online |
| LocationDetails | تفاصيل المكان | Yes | Text | 1024 | DB | N/A | N/A | Zoom link or Room | https://zoom.us/j/12345 | https://zoom.us/j/12345 |
| Status | الحالة | Yes | Text | 50 | DB | Scheduled | N/A | 'Not Started', 'Ongoing' | لم يبدأ بعد | Not Started |

### Entity: Meeting_Agenda_Item

| Attribute | Arabic | Mandatory | Type | Length | Integration | Default | Condition | Rules | Arabic Sample | English Sample |
|-----------|--------|-----------|------|--------|-------------|---------|-----------|-------|----------------|----------------|
| AgendaItemID | معرف بند الأجندة | Yes | Auto Int | N/A | DB Primary Key | N/A | N/A | N/A | 112 | 112 |
| MeetingID | معرف الاجتماع | Yes | Relation | N/A | FK to Meeting | N/A | N/A | N/A | 45 | 45 |
| ItemSubject | موضوع البند | Yes | Text | 255 | DB | N/A | N/A | Not empty | الموافقة على محضر الاجتماع السابق | Approval of Previous MoM |
| ItemDescription | وصف البند | Optional | Text | 1000 | DB | NULL | N/A | N/A | مراجعة وإقرار محضر الاجتماع المنعقد في يوليو. | Review and approve the minutes from the July meeting. |

## Messages / Notifications Table

| Code | English | Arabic | Type | Method |
|------|---------|--------|------|--------|
| MSG-SCM-SUC-01 | Meeting scheduled successfully, invitations sent. | تم إنشاء الاجتماع بنجاح، وتم إرسال الدعوات. | Success | In-App |
| MSG-SCM-ERR-01 | Meeting Subject is required. | موضوع الاجتماع مطلوب. | Validation | In-App |
| MSG-SCM-ERR-02 | End time must be after start time. | يجب أن يكون وقت النهاية بعد وقت البدء. | Validation | In-App |
| MSG-SCM-ERR-03 | Meeting date must be in the future. | يجب أن يكون تاريخ الاجتماع في المستقبل. | Validation | In-App |
| MSG-SCM-ERR-04 | At least one agenda item is required. | مطلوب بند واحد على الأقل في جدول الأعمال. | Validation | In-App |
| MSG-SCM-WRN-01 | Are you sure you want to cancel? | هل أنت متأكد من الإلغاء؟ | Warning | In-App (Modal) |
| MSG-SCM-NOT-01 | Invitation: You are invited to "[Subject]" on [Date] at [Time]. | دعوة: أنتم مدعوون لاجتماع "[الموضوع]" بتاريخ [التاريخ] في تمام الساعة [الوقت]. | Notification | Email, In-App |



# User Story 4: Manage a Live Board Meeting

## 1. Introduction
This document outlines the user story for managing a board meeting in real-time. This functionality is crucial for the official record-keeping and smooth execution of the meeting. The primary responsibility for managing the session falls to the Board Secretary or the Legal Counsel.

During a live meeting, these administrators can formally start and end the session, take attendance, and manage attachments. Concurrently, all participants, including board members and other invitees, can engage by viewing shared documents and contributing to a live discussion by adding and replying to notes.

---

## 2. Target User Personas
| Role                         | Description                                                                           |
|-----------------------------|---------------------------------------------------------------------------------------|
| Board Secretary / Legal Counsel | Administrators who control the meeting's state, manage attendance, and moderate the session. |
| Board Members / Fund Manager     | Active participants who view information and contribute by adding notes and comments.       |
| Other Attendees                 | Participants who can view shared information and engage in discussion.                      |

---

## 3. Main User Story Table

| Field               | Description                                                                                   |
|--------------------|-----------------------------------------------------------------------------------------------|
| **Name**           | Manage a Live Board Meeting                                                                   |
| **User Story**     | As a Board Secretary, I want to manage a live meeting session by starting the meeting, taking attendance, and facilitating discussions through notes and attachments, so that the meeting is conducted efficiently and all actions are properly recorded. |
| **Story Points**   | 8 (Medium complexity)                                                                          |
| **User Roles**     | Board Secretary, Legal Counsel (Admins), Board Members, Fund Manager, Other Attendees         |
| **Access Requirements** | Invited users only; administrative controls restricted to Board Secretary / Legal Counsel     |
| **Trigger**        | Scheduled meeting start time has arrived                                                       |
| **Frequency**      | High – used during every scheduled meeting                                                     |
| **Pre-condition**  | Meeting status is “Upcoming” or “Not Started” and current time is on/after scheduled time      |
| **Post-condition** | - Meeting marked as “Finished” <br> - Attendance saved <br> - Notes and replies recorded      |
| **Risk**           | Low (Mitigation: moderation by admins)                                                         |
| **Assumptions**    | System supports real-time dashboard for all users                                              |
| **UX/UI Link**     | N/A (to be linked to Figma mockups)                                                            |

---

## 4. Process Flow Table

| Step | Action Description                                                                 | Actor                      | Related Message Codes | Notes                                      |
|------|------------------------------------------------------------------------------------|----------------------------|------------------------|--------------------------------------------|
| 1    | Navigates to meeting details and clicks “Start Meeting”                           | Board Secretary / Legal Counsel | N/A                    | Sets status to "In Progress"              |
| 2    | Updates attendance for each board member                                           | Board Secretary / Legal Counsel | MSG-LMM-SUC-01        | Attendance for official record             |
| 3    | Adds note/comment in the “Notes” panel                                             | Any Attendee               | N/A                    | All attendees allowed                      |
| 4    | Reviews note and replies                                                           | Board Secretary / Legal Counsel | N/A                    | Moderated replies                          |
| 5    | Uploads document via "Add Attachment"                                              | Board Secretary / Legal Counsel | MSG-LMM-SUC-02        | Shared with all attendees                  |
| 6    | Clicks “End Meeting”                                                               | Board Secretary / Legal Counsel | MSG-LMM-WRN-01        | Closes session                             |
| 7    | Confirms the action in modal                                                       | Board Secretary / Legal Counsel | N/A                    | Prevents mistakes                          |
| 8    | System updates status to “Finished” and disables live controls                     | System                      | MSG-LMM-SUC-03         | Archives meeting data                      |

---

## 5. Alternative Flow Table

| Alternative Scenario         | Condition                                                      | Action                                                          | Related Message Codes | Resolution                                                 |
|-----------------------------|----------------------------------------------------------------|-----------------------------------------------------------------|------------------------|------------------------------------------------------------|
| Meeting Ended Prematurely   | Admin ends meeting accidentally                                | Meeting closed and controls disabled                            | MSG-LMM-SUC-03         | Future feature: "Reopen Meeting"; otherwise reschedule     |
| Participant Tries Admin Action | Participant tries restricted action                             | System blocks interaction (UI disables buttons)                 | N/A                    | Clear UI role-based control                                |
| Attachment Upload Fails     | Upload fails due to network error                              | Error message shown                                              | MSG-LMM-ERR-01         | Retry option                                                |

---

## 6. Acceptance Criteria Table

| Scenario                          | Given                                              | When                                     | Then                                                                 |
|----------------------------------|----------------------------------------------------|------------------------------------------|----------------------------------------------------------------------|
| Start and End Meeting            | I am Board Secretary on meeting dashboard          | Click “Start Meeting”, then “End Meeting” | Status changes to “In Progress” and then “Finished”                  |
| Take Attendance                  | I am Legal Counsel during meeting                  | Set member status to Present             | Confirmation message and record updated                             |
| Add Note as Board Member         | I am a Board Member during a meeting               | Type and submit a note                   | Note appears in real-time                                            |
| Reply to Note as Admin           | I am an Admin reviewing notes                      | Click "Reply" and submit                 | Reply nested under original note                                    |
| Upload Attachment                | I am Legal Counsel                                 | Upload a PDF file                        | File becomes visible and downloadable                               |

---

## 7. Screen Elements Table

| Element ID      | Type           | English Name           | Arabic Name            | Required | Validation     | Business Logic                                      | Data Entity          | Interaction | Accessibility Notes                                 |
|----------------|----------------|------------------------|------------------------|----------|----------------|----------------------------------------------------|----------------------|-------------|-----------------------------------------------------|
| ELM-LMM-001    | Button         | Start/End Meeting      | بدء الاجتماع / إنهاء الاجتماع | N/A      | N/A            | Toggles status; visible to admins only             | Meeting.Status        | Click       | Screen readers announce button                      |
| ELM-LMM-002    | Label          | Meeting Status         | حالة الاجتماع          | N/A      | N/A            | Shows meeting status                               | Meeting.Status        | Read        | Use ARIA live regions                              |
| ELM-LMM-003    | List           | Attendees              | الحضور                | N/A      | N/A            | Displays invited users                             | Meeting_Attendee      | N/A         | N/A                                                 |
| ELM-LMM-004    | Dropdown       | Attendance Status      | حالة الحضور            | N/A      | N/A            | Set by admin for board members                     | Meeting_Attendee      | Select      | N/A                                                 |
| ELM-LMM-005    | Panel          | Notes / Comments       | الملاحظات              | N/A      | N/A            | Real-time feed                                     | Meeting_Note          | Read        | N/A                                                 |
| ELM-LMM-006    | Text Area      | Add Note               | إضافة ملاحظة          | N/A      | Not Empty      | Add new note                                       | Meeting_Note.Content  | Type        | N/A                                                 |
| ELM-LMM-007    | Panel          | Attachments            | المرفقات              | N/A      | N/A            | Displays attachments                               | Meeting_Attachment    | Read        | N/A                                                 |
| ELM-LMM-008    | Button         | Add Attachment         | إضافة مرفق             | N/A      | N/A            | Upload new file; visible to admins only            | Meeting_Attachment    | Click       | N/A                                                 |

---

## 8. Data Entities Table

### Entity: Meeting (Update)

| Attribute         | Arabic Name | Type     | Required | Rules / Notes                                       | Sample             |
|------------------|-------------|----------|----------|----------------------------------------------------|--------------------|
| Status           | الحالة      | Text     | Yes      | Values: Scheduled, In Progress, Finished, Canceled | In Progress (جاري) |

---

### Entity: Meeting_Attendee (Update)

| Attribute         | Arabic Name   | Type  | Required | Rules / Notes                               | Sample      |
|------------------|---------------|-------|----------|---------------------------------------------|-------------|
| AttendanceStatus | حالة الحضور   | Text  | Optional | Values: Present (حاضر), Absent (غائب)      | Present (حاضر) |

---

### Entity: Meeting_Note

| Attribute       | Arabic Name     | Type           | Required | Notes                                       | Sample                    |
|----------------|------------------|----------------|----------|---------------------------------------------|---------------------------|
| NoteID         | معرف الملاحظة    | Auto Integer   | Yes      | Primary Key                                 | 701                       |
| MeetingID      | معرف الاجتماع    | Relation       | Yes      | Foreign Key to Meeting                      | 45                        |
| UserID         | معرف المستخدم    | Relation       | Yes      | Foreign Key to Users                        | 75                        |
| ParentNoteID   | الملاحظة الأصل   | Relation       | Optional | Reply to another note                       | 701                       |
| NoteContent    | محتوى الملاحظة   | Text (2000)    | Yes      | Not empty                                   | I agree with this point.  |
| Timestamp      | وقت الإنشاء      | DateTime       | Yes      | Auto-generated                              | 2025-08-15 10:30:00       |

---

## 9. Messages / Notifications Table

| Code             | English Message                                          | Arabic Message                                            | Type     | Delivery Method  |
|------------------|----------------------------------------------------------|-----------------------------------------------------------|----------|------------------|
| MSG-LMM-SUC-01   | Attendance for [Member Name] has been updated.           | تم تحديث حالة حضور [اسم العضو].                          | Success  | In-App (Toast)   |
| MSG-LMM-SUC-02   | Attachment uploaded successfully.                        | تم رفع المرفق بنجاح.                                     | Success  | In-App (Toast)   |
| MSG-LMM-SUC-03   | The meeting has been successfully ended.                | تم إنهاء الاجتماع بنجاح.                                 | Success  | In-App (Alert)   |
| MSG-LMM-ERR-01   | Attachment upload failed. Please try again.             | فشل رفع المرفق. يرجى المحاولة مرة أخرى.                 | Error    | In-App (Alert)   |
| MSG-LMM-WRN-01   | Are you sure you want to end the meeting?               | هل أنت متأكد من إنهاء الاجتماع؟ لا يمكن التراجع عن هذا الإجراء. | Warning  | In-App (Modal)   |



# User Story 5: Generate and Circulate Meeting Minutes

## 1. Introduction
This document details the user story for the creation and distribution of meeting minutes (MoM). This critical governance task is performed by the Board Secretary or Legal Counsel after a meeting has concluded.

The process involves drafting the key points and decisions from the meeting into a formal record. Once the draft is complete, it is electronically circulated to all board members who were marked as present during the meeting. This circulation initiates the next phase of the workflow, where attendees are required to review the minutes and provide their electronic signature for approval.

---

## 2. Target User Personas

| Role                         | Description                                                                 |
|------------------------------|-----------------------------------------------------------------------------|
| Board Secretary / Legal Counsel | Responsible for drafting and sending the meeting minutes.                    |
| Board Members                | Required to review and sign the circulated meeting minutes.                |

---

## 3. Main User Story Table

| Field               | Description                                                                                                           |
|--------------------|-----------------------------------------------------------------------------------------------------------------------|
| **Name**           | Generate and Circulate Meeting Minutes                                                                                |
| **User Story**     | As a Board Secretary, after a meeting has finished, I want to draft the meeting minutes by recording the key discussion points and decisions, so that I can circulate the draft to the attendees for their review and electronic signature. |
| **Story Points**   | 8 (Medium complexity)                                                                                                 |
| **User Roles**     | Board Secretary, Legal Counsel                                                                                        |
| **Access Requirements** | Must be "Board Secretary" or "Legal Counsel" and only for meetings with "Finished" status                        |
| **Trigger**        | A board meeting has been formally ended and is marked "Finished"                                                     |
| **Frequency**      | High – used after every board meeting                                                                                 |
| **Pre-condition**  | Meeting status is "Finished" and attendance has been recorded                                                         |
| **Business Rules** | - Minutes only created for "Finished" meetings  <br> - Must be drafted as specific points  <br> - Sent only to attendees who were "Present" |
| **Post-condition** | - Draft is saved  <br> - Status is "Pending Signature" <br> - Notifications sent to attendees                         |
| **Risk**           | Low – Controlled through attendee review and signature                                                               |
| **Assumptions**    | - Electronic signature feature is available  <br> - Attendees identified from recorded attendance                     |
| **UX/UI Link**     | N/A (To be linked to "Draft Meeting Minutes" screen mockup)                                                           |

---

## 4. Process Flow Table

| Step | Action Description                                                                      | Actor                      | Related Message Codes | Notes                                                  |
|------|------------------------------------------------------------------------------------------|----------------------------|------------------------|--------------------------------------------------------|
| 1    | Navigates to details page of a "Finished" meeting                                       | Board Secretary / Legal Counsel | N/A                    | Entry point for post-meeting actions                   |
| 2    | Clicks "Create Meeting Minutes"                                                         | Board Secretary / Legal Counsel | N/A                    | Opens the drafting editor                              |
| 3    | Editor for drafting minutes as specific points is displayed                             | System                     | N/A                    | Editor enforces structure                              |
| 4    | Enters content for each point                                                           | Board Secretary / Legal Counsel | MSG-GMM-ERR-01        | Main drafting step                                     |
| 5    | Clicks "Send for Signature"                                                             | Board Secretary / Legal Counsel | N/A                    | Initiates signature workflow                           |
| 6    | System checks minutes are not empty                                                     | System                     | MSG-GMM-ERR-01        | Prevents empty submissions                             |
| 7    | Identifies list of board members marked as "Present"                                    | System                     | N/A                    | Uses attendance data                                   |
| 8    | Saves draft and sets status to "Pending Signature"                                      | System                     | N/A                    | Record updated                                         |
| 9    | Sends notifications for review and signature                                            | System                     | MSG-GMM-NOT-01        | Email/In-app/Push notifications                        |
| 10   | Displays success confirmation                                                           | System                     | MSG-GMM-SUC-01         | Confirms circulation                                   |

---

## 5. Alternative Flow Table

| Scenario                          | Condition                                              | Action                                               | Message Code        | Resolution                                                                 |
|----------------------------------|--------------------------------------------------------|------------------------------------------------------|---------------------|----------------------------------------------------------------------------|
| Active Meeting                   | User tries to access minutes for a meeting "In Progress" | Button is disabled/hidden                            | N/A                 | Meeting must be ended first                                                |
| Send Empty Minutes               | User clicks "Send for Signature" with empty content    | System prevents and shows error                      | MSG-GMM-ERR-01      | Draft must have at least one point                                         |
| No Attendees Present             | No attendees were marked "Present"                     | System blocks circulation                            | MSG-GMM-ERR-02      | Admin must mark at least one attendee as "Present"                        |

---

## 6. Acceptance Criteria Table

| Scenario                          | Given                                                       | When                                              | Then                                                                 |
|----------------------------------|-------------------------------------------------------------|---------------------------------------------------|----------------------------------------------------------------------|
| Draft and Circulate Minutes      | Legal Counsel viewing "Finished" meeting with attendance    | Clicks "Create Meeting Minutes", adds content, sends for signature | Status becomes "Pending Signature", notifications sent               |
| Prevent Empty Minutes            | Secretary is on draft screen                                | Sends with no content                             | System prevents and shows "Minutes content cannot be empty"          |
| Notify Only Attendees            | Only 3 of 5 members marked "Present"                        | Sends minutes                                      | Notifications sent to only the 3 present members                     |
| Verify Status After Circulation | Minutes sent for signature                                  | Secretary checks status                            | Status is visible as "Pending Signature"                             |

---

## 7. Screen Elements Table

| Element ID      | Type             | English Name         | Arabic Name              | Required | Validation          | Business Logic                                           | Data Entity            | Interaction | Accessibility Notes                          |
|----------------|------------------|----------------------|--------------------------|----------|----------------------|----------------------------------------------------------|------------------------|-------------|----------------------------------------------|
| ELM-GMM-001    | Page Title       | Draft Meeting Minutes| مسودة محضر الاجتماع     | N/A      | N/A                  | N/A                                                      | N/A                    | N/A         | Should be an `<h1>`                          |
| ELM-GMM-002    | Rich Text Editor | Minutes Content      | محتوى المحضر             | Required | Cannot be empty      | Draft minutes as structured points                       | Meeting_Minutes.Content | Type        | Should be accessible with keyboard shortcuts |
| ELM-GMM-003    | Button           | Save as Draft        | حفظ كمسودة               | Optional | N/A                  | Saves as draft only                                      | Meeting_Minutes.Status | Click       | Secondary button                             |
| ELM-GMM-004    | Button           | Send for Signature   | إرسال للتوقيع           | Required | Cannot be empty      | Sends and updates status                                 | Meeting_Minutes.Status | Click       | Primary button                               |
| ELM-GMM-005    | Label            | Circulation List     | قائمة التوزيع            | N/A      | Read-only            | Displays users who will receive the signature request     | Meeting_Attendee        | Read        | N/A                                         |

---

## 8. Data Entities Table

### Entity: Meeting_Minutes

| Attribute         | Arabic Name     | Mandatory | Type              | Length | Default   | Rules                                             | Sample (AR)                              | Sample (EN)                              |
|------------------|------------------|-----------|-------------------|--------|-----------|---------------------------------------------------|-------------------------------------------|------------------------------------------|
| MinutesID         | معرف المحضر      | Yes       | Auto Integer       | N/A    | N/A       | Primary Key                                        | 88                                        | 88                                       |
| MeetingID         | معرف الاجتماع     | Yes       | Relation           | N/A    | N/A       | Must be linked to "Finished" meeting              | 45                                        | 45                                       |
| Content           | المحتوى           | Optional  | Rich Text / JSON   | N/A    | NULL      | Mandatory before sending for signature            | [{"point": "تقرر..."}, ...]              | [{"point": "It was decided..."}, ...]    |
| Status            | الحالة            | Yes       | Text               | 50     | Draft     | Values: Draft, Pending Signature, Completed       | قيد التوقيع                              | Pending Signature                        |
| CreatedByUserID   | معرف المنشئ       | Yes       | Relation           | N/A    | N/A       | Must be Board Secretary / Legal Counsel           | 54                                        | 54                                       |
| CreationDate      | تاريخ الإنشاء     | Yes       | DateTime           | N/A    | Now       | Auto-generated timestamp                          | 2025-08-15 12:00:00                       | 2025-08-15 12:00:00                      |

---

### Entity: Minutes_Signature

| Attribute         | Arabic Name     | Mandatory | Type            | Length | Default | Rules                                | Sample (AR)             | Sample (EN)             |
|------------------|------------------|-----------|-----------------|--------|---------|--------------------------------------|--------------------------|--------------------------|
| SignatureID       | معرف التوقيع     | Yes       | Auto Integer     | N/A    | N/A     | Primary Key                          | 901                      | 901                      |
| MinutesID         | معرف المحضر      | Yes       | Relation         | N/A    | N/A     | Foreign key to Meeting_Minutes       | 88                       | 88                       |
| SignatoryUserID   | معرف الموقع       | Yes       | Relation         | N/A    | N/A     | Must be an attendee                  | 75                       | 75                       |
| SignatureStatus   | حالة التوقيع     | Yes       | Text             | 50     | Pending | Values: Pending, Signed              | قيد الانتظار             | Pending                  |
| SignatureDate     | تاريخ التوقيع     | Optional  | DateTime         | N/A    | NULL    | Filled when user signs               | 2025-08-16 11:00:00      | 2025-08-16 11:00:00     |

---

## 9. Messages / Notifications Table

| Code               | English Message                                                          | Arabic Message                                                           | Type        | Channel                |
|--------------------|---------------------------------------------------------------------------|---------------------------------------------------------------------------|-------------|------------------------|
| MSG-GMM-SUC-01     | The meeting minutes have been successfully sent for signature.            | تم إرسال محضر الاجتماع للتوقيع بنجاح.                                     | Success     | In-App                 |
| MSG-GMM-ERR-01     | Minutes content cannot be empty. Please draft the minutes before sending. | لا يمكن أن يكون محتوى المحضر فارغًا. يرجى صياغة المحضر قبل الإرسال.     | Validation  | In-App                 |
| MSG-GMM-ERR-02     | Cannot circulate minutes as there are no recorded attendees for this meeting. | لا يمكن تعميم المحضر لعدم وجود حاضرين مسجلين لهذا الاجتماع.             | Validation  | In-App                 |
| MSG-GMM-NOT-01     | The minutes for the meeting "[Meeting Subject]" are ready for your review and signature. | محضر اجتماع "[موضوع الاجتماع]" جاهز للمراجعة والتوقيع من قبلكم.     | Notification | Email, In-App, Push    |



# User Story 6: Electronically Sign Meeting Minutes

This document details the user story for the electronic signature of meeting minutes (MoM). This feature is a critical component of the governance workflow, enabling board members to formally approve the official record of a meeting.

## 📌 Summary

After the minutes have been drafted and circulated, attendees who were present receive a notification. They can then access the draft, review its content, and apply their pre-configured electronic signature to signify their approval. The system tracks each signature, and once the final required signature is collected, it automatically generates a consolidated, non-editable PDF document containing the minutes and all associated signatures for archival.

---

## 🎯 Target User Personas

- **Board Member (أعضاء المجلس)**: The primary user who reviews and applies their electronic signature to the minutes.
- **Board Secretary / Legal Counsel**: Administrators who monitor the signature collection progress and access the final, signed PDF document.

---

## 📋 Main User Story Table

| Field               | Description |
|--------------------|-------------|
| **Name**           | Electronically Sign Meeting Minutes |
| **User Story**     | As a Board Member, I want to review the drafted meeting minutes and apply my electronic signature to approve them, so that I can formally validate the record of the meeting in a secure and efficient manner. |
| **Story Points**   | 8 (Medium complexity) |
| **User Roles**     | Board Member (Signatory) |
| **Access Requirements** | Must be an authenticated Board Member who was marked as "Present" at the meeting and has a pending signature request. |
| **Trigger**        | Notification that meeting minutes are ready for signature (via in-app or email). |
| **Frequency of Use** | High – after every meeting. |
| **Pre-condition**  | Meeting minutes are in "Pending Signature" status. User has not signed yet. |
| **Business Rules** | - Only attendees can sign.<br> - Signatures are final and cannot be undone.<br> - Signature is pulled from user's profile.<br> - Final PDF is generated once all signatures are collected. |
| **Post-condition** | - Signature and timestamp recorded.<br> - Status changes to "Completed" once all signatories sign.<br> - Final PDF is archived. |
| **Risk** | Medium – No rejection or change-request workflow defined. **Mitigation**: Add "Request Revision" option. |
| **Assumptions** | Electronic signatures have been pre-configured in user profiles (from Phase 2). |
| **UX/UI Design Link** | N/A – Will be linked to Figma wireframes. |

---

## 🔁 Process Flow Table

| Step | Action Description | Actor | Related Message Codes | Notes |
|------|--------------------|--------|------------------------|-------|
| 1 | Receives a notification and navigates to "Sign Meeting Minutes" page | Board Member | N/A | Entry point |
| 2 | System displays minutes in read-only format | System | N/A | For review |
| 3 | Displays current signature status of all required signatories | System | N/A | Transparency |
| 4 | Clicks "Approve and E-Sign" button | Board Member | N/A | Main action |
| 5 | Applies electronic signature from profile | System | N/A | Signature is added |
| 6 | Updates status to "Signed" and records timestamp | System | N/A | Task complete |
| 7 | Checks if all required attendees have signed | System | N/A | Triggers finalization |
| 8 | Generates final PDF if all signatures collected | System | MSG-ESM-NOT-02 | Tamper-proof archive |
| 9 | Updates minutes status to "Completed" and attaches final PDF | System | N/A | Workflow complete |
| 10 | Displays success message | System | MSG-ESM-SUC-01 | Confirms signature |

---

## 🔄 Alternative Flow Table

| Alternative Scenario | Condition | Action | Message Code | Resolution |
|----------------------|-----------|--------|---------------|------------|
| Unauthorized Access | Non-attendee tries to sign | System blocks access | MSG-ESM-ERR-01 | Redirect to dashboard |
| Duplicate Sign Attempt | Already signed user returns | Button disabled, status shown | MSG-ESM-INF-01 | No action allowed |
| Signature Not Configured | User has no signature profile | Blocks action, prompts to configure | MSG-ESM-ERR-02 | Links to profile setup |

---

## ✅ Acceptance Criteria Table

| Scenario | Given | When | Then |
|----------|-------|------|------|
| **Successfully Sign** | I am a Board Member with signature request | I click "Approve and E-Sign" | Status updates to "Signed", message shown |
| **Final Signatory** | I am last to sign | I click "Approve and E-Sign" | Final PDF generated, status = "Completed", notification sent |
| **View Final Signed PDF** | I view a "Completed" meeting | I click the minutes link | Shows final PDF with all signatures |
| **Unauthorized Access** | I did not attend the meeting | I try to sign | Error message shown, access denied |

---

## 🖼️ Screen Elements Table

| ID | Type | Name (EN) | Name (AR) | Required | Validation | Logic | Data Entity | Interaction | Accessibility |
|----|------|------------|-----------|----------|------------|--------|-------------|-------------|----------------|
| ELM-ESM-001 | Page Title | Review & Sign Meeting Minutes | مراجعة وتوقيع محضر الاجتماع | N/A | N/A | N/A | N/A | Read | Should be `<h1>` |
| ELM-ESM-002 | Read-only Panel | Minutes Content | محتوى المحضر | N/A | N/A | Shows drafted minutes | Meeting_Minutes.Content | Scroll | N/A |
| ELM-ESM-003 | Status List | Signature Status | حالة التوقيعات | N/A | N/A | List of signatories with status | Minutes_Signature | Read | N/A |
| ELM-ESM-004 | Button | Approve and E-Sign | موافقة وتوقيع إلكتروني | N/A | N/A | Updates status, adds signature | Minutes_Signature.Status | Click | Should have focus state |
| ELM-ESM-005 | Link | Download as Draft | تحميل كمسودة | N/A | N/A | Download draft-only version | N/A | Click | N/A |

---

## 🧩 Data Entities Table

### **Entity: Minutes_Signature**

| Attribute (EN) | Attribute (AR) | Mandatory | Type | Length | Source | Default | Rules | Sample (AR) | Sample (EN) |
|----------------|----------------|-----------|------|--------|--------|---------|--------|--------------|--------------|
| SignatureStatus | حالة التوقيع | Yes | Text | 50 | DB | Pending | Updates to 'Signed' | موقع | Signed |
| SignatureDate | تاريخ التوقيع | Yes on sign | DateTime | N/A | DB | NULL | Timestamped on sign | 2025-08-16 11:00:00 | 2025-08-16 11:00:00 |
| SignatureData | بيانات التوقيع | Optional | Text / Image Path | N/A | DB | NULL | Pulled from profile | /signatures/user75.svg | /signatures/user75.svg |

---

### **Entity: Meeting_Minutes**

| Attribute (EN) | Attribute (AR) | Mandatory | Type | Length | Source | Default | Rules | Sample (AR) | Sample (EN) |
|----------------|----------------|-----------|------|--------|--------|---------|--------|--------------|--------------|
| Status | الحالة | Yes | Text | 50 | DB | Pending Signature | Updates to 'Completed' | مكتمل | Completed |
| FinalDocumentPath | مسار الملف النهائي | Optional | Text | 1024 | DB / FS | NULL | Stores signed PDF | /meetings/45/final_mom.pdf | /meetings/45/final_mom.pdf |

---

## 💬 Messages/Notifications Table

| Code | English | Arabic | Type | Method |
|------|---------|--------|------|--------|
| MSG-ESM-SUC-01 | Your signature has been successfully applied. | تم تطبيق توقيعك بنجاح. | Success | In-App |
| MSG-ESM-ERR-01 | You are not authorized to perform this action. | ليس لديك صلاحية للقيام بهذا الإجراء. | Error | In-App |
| MSG-ESM-ERR-02 | Your electronic signature is not configured. Please set it up in your profile before signing. | توقيعك الإلكتروني غير مُعد. يرجى إعداده في ملفك الشخصي قبل التوقيع. | Error | In-App (Alert) |
| MSG-ESM-INF-01 | You have already signed these minutes. No further action is required. | لقد قمت بالتوقيع على هذا المحضر مسبقًا. لا يلزم اتخاذ أي إجراء آخر. | Information | In-App |
| MSG-ESM-NOT-02 | The signing process for the minutes of "[Meeting Subject]" is now complete and the final document is available. | اكتملت عملية التوقيع على محضر اجتماع "[موضوع الاجتماع]" والملف النهائي متاح الآن. | Notification | In-App, Email |

---


# User Story 7: View Scheduled Meetings

## 1. Summary

This document details the user story for viewing the list of scheduled meetings. This is a fundamental feature for all users involved in the fund's governance, providing a centralized and up-to-date overview of all planned, ongoing, and completed meetings.

Users can navigate to a dedicated section to see a list of all meetings associated with a fund. This view displays key information at a glance, such as the meeting title, date, and its current status. It also serves as the primary gateway for users to access more detailed information about a specific meeting or, for authorized administrators, to perform actions like editing or canceling an upcoming meeting.

### Target User Personas
- **Board Member (أعضاء المجلس)**
- **Fund Manager (مدير الصندوق)**
- **Compliance Officer (مسؤول المطابقة والالتزام)**
- **Board Secretary (أمين سر المجلس)**
- **Legal Counsel (المسؤول القانوني)**

---

## 2. Main User Story Table

| Field               | Description                                                                                     |
|--------------------|-------------------------------------------------------------------------------------------------|
| **Name**           | View Scheduled Meetings                                                                         |
| **User Story**     | As a Board Member, I want to view a list of all scheduled meetings with their dates and statuses, so that I can easily keep track of my upcoming commitments and access details of past meetings. |
| **Story Points**   | 3 (Low complexity)                                                                              |
| **User Roles**     | Board Member, Fund Manager, Board Secretary, Legal Counsel, Compliance Officer.                 |
| **Access Requirements** | User must be authenticated and associated with the fund.                                  |
| **Trigger**        | User navigates to the "Meetings" section.                                                       |
| **Frequency of Use** | High                                                                                          |
| **Pre-condition**  | The user is logged into the system.                                                             |
| **Post-condition** | The user sees a clear, filterable list of meetings with access to actions based on role.       |
| **Risk**           | Low: Outdated status due to no refresh. Mitigation: Auto/manual refresh.                       |
| **Assumptions**    | The system can efficiently query and display meetings for the logged-in user.                  |
| **UX/UI Design Link** | N/A – To be linked to Figma.                                                                |

---

## 3. Business Rules

- Meetings display statuses: **"Not Started," "In Progress," "Finished," "Canceled"**.
- **Edit** action: visible only for **Board Secretaries / Legal Counsels**, only when status is **"Not Started"**.
- **Cancel** action: visible only for **Board Secretaries / Legal Counsels** for upcoming meetings.
- **Meeting Minutes**: shown only if status is **"Finished"**.

---

## 4. Process Flow

| Step | Action Description | Actor               | Related Message Codes | Notes                                                |
|------|--------------------|---------------------|------------------------|------------------------------------------------------|
| 1    | Navigate to Meetings | Any authorized user | N/A                    | Entry point                                          |
| 2    | System fetches and displays list | System | MSG-VSM-INF-01 | Sorted by date (newest first)                        |
| 3    | Display Title, Date/Time, Status | System | N/A            | Key information at a glance                          |
| 4    | Show buttons conditionally | System | N/A              | Based on role and meeting status                     |
| 5    | Optionally filter by status | User | N/A               | Helps users focus on specific subsets                |
| 6    | Click “Details” button | User | N/A                   | Navigates to meeting detail page                     |
| 7    | Click “Edit” button (if eligible) | Admins | N/A           | Opens meeting in edit mode                          |

---

## 5. Alternative Flows

| Alternative Scenario         | Condition                                          | Action / Behavior                                                                 | Message Code        | Resolution                                                                                     |
|-----------------------------|----------------------------------------------------|-----------------------------------------------------------------------------------|----------------------|-----------------------------------------------------------------------------------------------|
| No Meetings Scheduled       | User visits meeting list and no meetings exist     | System shows friendly message                                                     | MSG-VSM-INF-01       | If user has permissions, show “Schedule New Meeting” button                                   |
| Unauthorized Action Attempt | User clicks disabled "Edit"                        | No action taken                                                                   | N/A                  | Disabled state conveys that action is not permitted                                            |
| Viewing Canceled Meeting    | Meeting is canceled                                | All actions disabled except “Details”                                             | N/A                  | Canceled meetings kept for historical reference                                               |

---

## 6. Acceptance Criteria

| Scenario                            | Given                                       | When                                  | Then                                                                                      |
|-------------------------------------|--------------------------------------------|----------------------------------------|-------------------------------------------------------------------------------------------|
| View All Meetings                   | I am a Fund Manager                         | I go to the "Meetings" section         | I see a list of meetings with Title, Date, Status                                         |
| Admin Views Upcoming Meeting        | I am a Board Secretary                      | I see a meeting with status "Not Started" | I see **Details, Edit, Cancel** buttons                                                    |
| Board Member Views Upcoming Meeting | I am a Board Member                         | I see a meeting with status "Not Started" | I see **Details**, other actions are hidden or disabled                                   |
| View a Finished Meeting             | Any user                                    | I see a meeting with status "Finished" | I see **Meeting Minutes**, **Edit/Cancel** are disabled                                   |
| No Meetings Exist                   | I am a new user                             | I visit meeting list page              | I see message: "There are no scheduled meetings to display."                              |

---

## 7. Screen Elements

| ID             | Type           | English Name         | Arabic Name            | Required/Optional | Validation | Business Logic | Data Source | Interaction | Accessibility Notes                        |
|----------------|----------------|----------------------|-------------------------|-------------------|------------|----------------|-------------|-------------|---------------------------------------------|
| ELM-VSM-001    | Page Title     | Meetings             | الاجتماعات              | N/A               | N/A        | N/A            | N/A         | N/A         | Should use `<h1>`                           |
| ELM-VSM-002    | Button         | Schedule New Meeting | إنشاء اجتماع جديد      | N/A               | N/A        | Visible to authorized users | N/A   | Click       | N/A                                         |
| ELM-VSM-003    | Filter/Dropdown| Filter by Status     | تصفية حسب الحالة        | Optional          | N/A        | Filters by Meeting.Status | Meeting | Select      | N/A                                         |
| ELM-VSM-004    | Table/List     | Meeting List         | قائمة الاجتماعات        | N/A               | N/A        | List container | Meeting     | N/A         | Use `<th>` for headers                      |
| ELM-VSM-005    | Text           | Meeting Title        | عنوان الاجتماع          | N/A               | N/A        | Show Subject   | Meeting     | Read        | N/A                                         |
| ELM-VSM-006    | Text           | Meeting Date         | تاريخ الاجتماع          | N/A               | N/A        | Date & Time    | Meeting     | Read        | N/A                                         |
| ELM-VSM-007    | Badge/Tag      | Status               | الحالة                  | N/A               | N/A        | Colored text   | Meeting     | Read        | Do not rely on color only                  |
| ELM-VSM-008    | Button Group   | Actions              | الإجراءات                | N/A               | N/A        | Varies by role | N/A         | Click       | N/A                                         |

---

## 8. Data Entities

### Entity: Meeting (Read-only)

| Attribute (English) | Attribute (Arabic) | Mandatory | Type     | Length | Source   | Default | Rules                          | Arabic Sample               | English Sample            |
|---------------------|--------------------|-----------|----------|--------|----------|---------|-------------------------------|-----------------------------|---------------------------|
| MeetingID           | معرف الاجتماع       | Yes       | Integer  | N/A    | DB       | N/A     | N/A                           | 45                          | 45                        |
| Subject             | الموضوع             | Yes       | Text     | 255    | DB       | N/A     | N/A                           | اجتماع مجلس الإدارة الشهري | Monthly Board Meeting     |
| MeetingDate         | تاريخ الاجتماع       | Yes       | Date     | N/A    | DB       | N/A     | N/A                           | 2025-08-15                  | 2025-08-15                |
| StartTime           | وقت البدء           | Yes       | Time     | N/A    | DB       | N/A     | N/A                           | 10:00:00                    | 10:00:00                  |
| Status              | الحالة              | Yes       | Text     | 50     | DB       | N/A     | N/A                           | لم يبدأ بعد                | Not Started               |
| HasMinutes          | يوجد محضر اجتماع    | Boolean   | Boolean  | N/A    | System   | N/A     | True if status is 'Finished'  | نعم                         | True                      |

---

## 9. Messages & Notifications

| Code              | Message (English)                        | Message (Arabic)                             | Type         | Method         |
|------------------|-------------------------------------------|----------------------------------------------|--------------|----------------|
| MSG-VSM-INF-01   | There are no scheduled meetings to display. | لا توجد اجتماعات مجدولة لعرضها.              | Information  | In-App         |





# User Story 8: Modify an Upcoming Meeting

This document outlines the user story for modifying the details of a previously scheduled meeting. This administrative function is reserved for the Board Secretary and the Legal Counsel, allowing them to make necessary changes to an upcoming meeting.

The feature enables these authorized users to reopen the meeting creation form, adjust any of the details—such as the date, time, attendees, or agenda—and save the changes. Upon saving, the system ensures that all invited attendees are automatically notified of the updated meeting information, maintaining clear and consistent communication. This action is only permissible for meetings that have not yet started.

## Target User Personas:

* **Board Secretary (أمين سر المجلس) / Legal Counsel (المسؤول القانوني):** The administrators who have the authority to edit and update scheduled meetings.
* **Attendees (Board Members, etc.):** The recipients of the notification who need to be aware of any changes to the meeting schedule or content.

---

## Main User Story Table

| Field | Description | Content Guidelines |
| :--- | :--- | :--- |
| **Name** | Modify an Upcoming Meeting | Defines the process for editing the details of a meeting that has not yet started. |
| **User Story** | As a Legal Counsel, I want to modify the details of an upcoming meeting, such as changing the time or adding a new agenda item, so that I can update the official record and ensure all attendees are notified of the changes. | |
| **Story Points** | 5 | Low-to-Medium complexity. It reuses the creation form but adds logic for state checking and notifications. |
| **User Roles** | Board Secretary, Legal Counsel (Editor). | |
| **Access Requirements** | User must have the role of "Board Secretary" or "Legal Counsel." The meeting's status must be "Not Started" (لم يبدأ بعد). |
| **Trigger** | The user identifies a need to change the details of a scheduled meeting from the meeting list view. | |
| **Frequency of Use** | Low. This action is performed only when there are necessary changes to a planned meeting. | |
| **Pre-condition** | A meeting exists with the status "Not Started." The user is on the meeting list or meeting details page. | |
| **Business Rules** | - A meeting can only be modified if its status is "Not Started".<br>- Only the Board Secretary or Legal Counsel can perform this action.<br>- Upon saving the modifications, all attendees must be re-notified of the changes.<br>- The same validation rules as meeting creation apply (e.g., end time must be after start time). |
| **Post-condition** | - The meeting's details are updated in the database.<br>- A notification detailing the changes is sent to all previously invited attendees. | |
| **Risk** | **Low:** An attendee might miss the update notification. Mitigation: The notification should have a clear subject line like "UPDATE:" or "CHANGE:" to draw attention. The meeting details in-app should always reflect the most current information. | |
| **Assumptions** | The system has a robust notification service capable of sending updates to all attendees. | |
| **UX/UI Design Link** | N/A - This would reuse the "Create Meeting" Figma wireframes, populated with existing data. | |

---

## Process Flow Table

| Step | Action Description | Actor | Related Message Codes | Notes |
| :--- | :--- | :--- | :--- | :--- |
| 1 | Navigates to the meeting list and locates the upcoming meeting to be modified. | Board Secretary / Legal Counsel | N/A | The user identifies the correct meeting record. |
| 2 | Clicks the "Edit" (تعديل) button for that meeting. | Board Secretary / Legal Counsel | N/A | This action is only enabled for meetings with a "Not Started" status. |
| 3 | The system displays the meeting creation form, pre-populated with all the existing details of the meeting. | System | N/A | The user is presented with the familiar interface for editing. |
| 4 | Modifies the necessary fields, such as changing the date, updating an agenda item, or adding a new attachment. | Board Secretary / Legal Counsel | N/A | The user performs the required changes. |
| 5 | After making all changes, clicks the "Save Changes" or "Update Meeting" button. | Board Secretary / Legal Counsel | N/A | Submits the updated information. |
| 6 | The system validates the modified data against all business rules (e.g., time validation, required fields). | System | N/A | Same validation logic as the creation process. |
| 7 | The system saves the updated meeting details to the database. | System | N/A | The official record is now changed. |
| 8 | The system sends a notification to all invited attendees informing them of the update. | System | MSG-MUM-NOT-01 | This ensures all participants are aware of the new details. |
| 9 | Displays a success message to the user and redirects them to the updated meeting details page. | System | MSG-MUM-SUC-01 | Confirms the successful modification. |

---

## Alternative Flow Table

| Alternative Scenario | Condition | Action | Related Message Codes | Resolution |
| :--- | :--- | :--- | :--- | :--- |
| Attempt to Edit a Started Meeting | A Legal Counsel tries to click the "Edit" button for a meeting whose status is "In Progress" or "Finished." | The "Edit" button is disabled or hidden in the user interface. | N/A | The system enforces the business rule that only upcoming meetings can be modified. The user cannot proceed. |
| Unauthorized User Attempts to Edit | A Board Member views the details of an upcoming meeting. | The "Edit" button is not visible or is disabled for their role. | N/A | Access to the modification function is restricted by user role as per the BRD. |
| Invalid Change Made | The user modifies the meeting time, setting the end time before the start time, and clicks "Save Changes." | The system displays a validation error, preventing the save. | MSG-SCM-ERR-02 | The user must correct the invalid data before the system will accept the changes. |

---

## Acceptance Criteria Table

| Scenario | Given | When | Then |
| :--- | :--- | :--- | :--- |
| Successfully Modify Meeting Time | I am a Board Secretary editing an upcoming meeting | I change the StartTime of the meeting to a new time and click "Save Changes" | The meeting's time should be updated in the system, and all attendees should receive a notification about the new meeting time. |
| Successfully Add an Agenda Item | I am a Legal Counsel editing an upcoming meeting | I add a new item to the agenda and click "Save Changes" | The meeting's agenda should be updated, and the notification sent to attendees should reflect that the agenda has changed. |
| Attempt to Edit a Finished Meeting | I am a Board Secretary viewing the details of a meeting with "Finished" status | I look for the "Edit" button | The "Edit" button should be hidden or disabled, preventing me from modifying the meeting. |
| Unauthorized User Attempt | I am a Board Member viewing the details of an upcoming meeting | I look for the "Edit" button | The "Edit" button should be hidden or disabled for my role. |

---

## Screen Elements Table

*(This user story reuses the same screen elements as the "Schedule a New Board Meeting" user story. The primary difference is that the form fields are pre-populated with existing data.)*

| Element ID | Element Type | Element Name (English) | Element Name (Arabic) | Notes |
| :--- | :--- | :--- | :--- | :--- |
| ELM-SCM-001 | Input Field | Meeting Subject | موضوع الاجتماع | Pre-populated with existing data. |
| ELM-SCM-002 | Dropdown | Meeting Type | نوع الاجتماع | Pre-populated with existing data. |
| ELM-SCM-003 | Date Picker | Date | التاريخ | Pre-populated with existing data. |
| ELM-SCM-009 | Checkbox Group | Attendees | الحضور | Pre-populated with existing data. |
| ELM-MUM-001 | Button | Save Changes | حفظ التغييرات | The primary submission button, replacing "Schedule Meeting". |

---

## Data Entities Table

*(This user story updates the same data entities as the "Schedule a New Board Meeting" user story: Meeting, Meeting_Attendee, Meeting_Agenda_Item, and Meeting_Attachment.)*

**Entity Name: Meeting (Update)**

| Attribute (English) | Attribute (Arabic) | Action | Sample in Arabic | Sample in English |
| :--- | :--- | :--- | :--- | :--- |
| Subject | الموضوع | UPDATE | اجتماع مجلس الإدارة الشهري (مُعدل) | Monthly Board Meeting (Amended) |
| MeetingDate | تاريخ الاجتماع | UPDATE | 2025-08-16 | 2025-08-16 |
| StartTime | وقت البدء | UPDATE | 11:00:00 | 11:00:00 |
| LastModifiedDate | تاريخ آخر تعديل | UPDATE | 2025-07-25 10:00:00 | 2025-07-25 10:00:00 |

---

## Messages/Notifications Table

| Message Code | Message (English) | Message (Arabic) | Message Type | Communication Method |
| :--- | :--- | :--- | :--- | :--- |
| MSG-MUM-SUC-01 | The meeting has been updated successfully. Attendees have been notified of the changes. | تم تحديث الاجتماع بنجاح. تم إشعار الحضور بالتغييرات. | Success | In-App |
| MSG-MUM-NOT-01 | UPDATE: The details for the meeting "[Subject]" have been changed. Please review the updated information. | تحديث: تم تغيير تفاصيل اجتماع "[الموضوع]". يرجى مراجعة المعلومات المحدثة. | Notification | Email, In-App |
| MSG-MUM-ERR-01 | This meeting cannot be modified as it has already started or is finished. | لا يمكن تعديل هذا الاجتماع لأنه قد بدأ بالفعل أو انتهى. | Error | In-App (Alert) |


 
# User Story 9: Cancel an Upcoming Meeting

## 1. Summary
This document outlines the user story for canceling a scheduled meeting. This is a critical administrative action performed by a Board Secretary or Legal Counsel when a planned meeting is no longer required.

Authorized users can select an upcoming meeting and mark it as canceled. The system prompts for confirmation, then updates the meeting’s status and notifies all attendees. This action is only available for meetings that have not yet started.

---

## 2. Target User Personas
- **Board Secretary (أمين سر المجلس)** / **Legal Counsel (المسؤول القانوني)**: Authorized to cancel scheduled meetings.
- **Attendees** (Board Members, etc.): Recipients of the cancellation notification.

---

## 3. Main User Story Table

| Field             | Description                                                                                                                                   |
|------------------|-----------------------------------------------------------------------------------------------------------------------------------------------|
| **Name**         | Cancel an Upcoming Meeting                                                                                                                   |
| **User Story**   | As a Board Secretary, I want to cancel a scheduled meeting that is no longer needed, so that its status is updated and all attendees are notified. |
| **Story Points** | 2 (Low complexity: state change, confirmation, notification)                                                                                 |
| **User Roles**   | Board Secretary, Legal Counsel                                                                                                                |
| **Access**       | Only users with "Board Secretary" or "Legal Counsel" roles                                                                                   |
| **Trigger**      | A decision is made to cancel a scheduled meeting                                                                                              |
| **Frequency**    | Low - Exception-based                                                                                                                         |
| **Pre-condition**| Meeting status is "Not Started" (لم يبدأ بعد), and the meeting has not yet reached its scheduled start time                                 |

---

## 4. Business Rules

- Only the Board Secretary or Legal Counsel can cancel a meeting.
- Cancellation is only allowed before the scheduled start time.
- Upon cancellation, all invited attendees must be notified.
- A canceled meeting remains stored for audit/history but cannot be reactivated or edited.

---

## 5. Post-Condition

- The meeting's status is updated to **"Canceled" (ملغی)**.
- Notification is sent to all attendees.

---

## 6. Risk
- **Low**: If a notification fails, some attendees may show up.
- **Mitigation**: Meeting status should be clearly visible in the app’s meeting list.

---

## 7. Assumptions

- The system can reliably deliver in-app and email notifications to all attendees.

---

## 8. Process Flow Table

| Step | Action Description                                                         | Actor                    | Message Code        | Notes                                                             |
|------|------------------------------------------------------------------------------|--------------------------|---------------------|-------------------------------------------------------------------|
| 1    | Navigate to meeting list and locate upcoming meeting                        | Board Secretary / Legal Counsel | N/A             | Identify correct meeting                                           |
| 2    | Click "Cancel" (إلغاء) button                                               | Board Secretary / Legal Counsel | N/A             | Enabled only for "Not Started" meetings                           |
| 3    | System displays confirmation modal                                          | System                   | MSG-CUM-WRN-01       | Prevents accidental action                                        |
| 4    | Click "Confirm Cancel" in modal                                             | Board Secretary / Legal Counsel | N/A             | Final confirmation                                                 |
| 5    | System updates meeting status to "Canceled" (ملغی)                          | System                   | N/A                  | Official update                                                    |
| 6    | System sends notification to all attendees                                 | System                   | MSG-CUM-NOT-01       | Email and in-app                                                  |
| 7    | System shows success message                                                | System                   | MSG-CUM-SUC-01       | Feedback to administrator                                          |
| 8    | Meeting list refreshes to show updated status                              | System                   | N/A                  | Immediate UI update                                               |

---

## 9. Alternative Flow Table

| Alternative Scenario         | Condition                                                              | Action                                                  | Message Code | Resolution                                                                 |
|-----------------------------|------------------------------------------------------------------------|---------------------------------------------------------|--------------|----------------------------------------------------------------------------|
| Cancel a started meeting     | Meeting status is "In Progress" or "Finished"                          | "Cancel" button hidden or disabled                      | N/A          | Not allowed                                                               |
| Cancel confirmation aborted | User closes modal or clicks "Keep Meeting"                             | Modal closes, no change                                 | N/A          | Meeting remains scheduled                                                 |
| Unauthorized user attempt    | Board Member attempts to cancel meeting                                | "Cancel" button not visible                             | N/A          | Role-based access control enforced                                       |

---

## 10. Acceptance Criteria Table

| Scenario                           | Given                                      | When                                               | Then                                                                 |
|-----------------------------------|--------------------------------------------|----------------------------------------------------|----------------------------------------------------------------------|
| Cancel successfully               | I am a Legal Counsel viewing meeting list  | I click "Cancel" on a "Not Started" meeting        | Status changes to "Canceled", all attendees are notified             |
| View canceled status              | I am a Board Member                        | I view the meeting list                            | I see the status marked "Canceled" (ملغی)                             |
| Attempt to cancel finished meeting| I am a Board Secretary                     | I view a "Finished" meeting                        | "Cancel" button is hidden or disabled                                 |
| Confirm notification content      | A meeting I was invited to is canceled     | I check my email or notifications                 | I receive a message stating the subject and time of cancellation      |

---

## 11. Screen Elements Table

| Element ID      | Type         | English Name         | Arabic Name         | Required | Validation | Business Logic                                                | Data Entity     | User Interaction | Accessibility Notes                   |
|----------------|--------------|----------------------|---------------------|----------|------------|----------------------------------------------------------------|------------------|------------------|----------------------------------------|
| ELM-VSM-008    | Button       | Cancel               | إلغاء               | N/A      | N/A        | Shown only for Admins & upcoming meetings                     | Meeting.Status   | Click             | N/A                                    |
| ELM-CUM-001    | Modal Dialog | Confirm Cancellation | تأكيد الإلغاء       | N/A      | N/A        | Appears when clicking Cancel                                  | N/A              | Modal             | Modal should trap focus                |
| ELM-CUM-002    | Text         | Confirmation Message | رسالة التأكيد       | N/A      | N/A        | Warning to confirm cancellation                               | N/A              | Read              | N/A                                    |
| ELM-CUM-003    | Button       | Confirm Cancel       | تأكيد الإلغاء       | N/A      | N/A        | Executes the cancellation                                     | Meeting.Status   | Click             | Styled destructively (e.g., red)       |
| ELM-CUM-004    | Button       | Keep Meeting         | إبقاء الاجتماع       | N/A      | N/A        | Closes modal, aborts action                                   | N/A              | Click             | N/A                                    |

---

## 12. Data Entities Table

**Entity: Meeting (Update)**

| Attribute         | Arabic Name       | Mandatory | Type      | Length | Default | Rule                                                                 |
|-------------------|------------------|-----------|-----------|--------|---------|----------------------------------------------------------------------|
| Status            | الحالة            | Yes       | Text      | 50     | N/A     | Updated from "Not Started" → "Canceled" (ملغی)                       |
| LastModifiedDate  | تاريخ آخر تعديل  | Yes       | DateTime  | N/A    | N/A     | Updated with current timestamp when cancellation occurs              |

---

## 13. Messages / Notifications Table

| Code            | Message (English)                                                                 | Message (Arabic)                                                              | Type       | Method          |
|------------------|----------------------------------------------------------------------------------|-------------------------------------------------------------------------------|------------|------------------|
| MSG-CUM-SUC-01    | The meeting has been canceled successfully.                                     | تم إلغاء الاجتماع بنجاح.                                                      | Success    | In-App (Toast)   |
| MSG-CUM-WRN-01    | Are you sure you want to cancel this meeting? This action cannot be undone.    | هل أنت متأكد من إلغاء هذا الاجتماع؟ لا يمكن التراجع عن هذا الإجراء.         | Warning    | In-App (Modal)   |
| MSG-CUM-NOT-01    | CANCELLATION: "[Subject]" meeting on [Date] at [Time] has been canceled.       | إلغاء: تم إلغاء اجتماع "[الموضوع]" بتاريخ [التاريخ] في الساعة [الوقت].     | Notification| Email, In-App    |
| MSG-CUM-ERR-01    | This meeting cannot be canceled because it has already started or finished.     | لا يمكن إلغاء هذا الاجتماع لأنه جاري بالفعل أو قد انتهى.                     | Error      | In-App (Alert)   |



# User Story 10: Manage Meeting Attachments

This document outlines the user stories related to managing attachments for a board meeting. This functionality is divided into two key perspectives: the administrative role of adding and removing documents, and the participant role of viewing and downloading them.

Administrators (Board Secretary and Legal Counsel) need the ability to manage the official documents for a meeting at any stage—before or during the session. This ensures that all relevant materials are available and up-to-date. All attendees, in turn, require seamless access to view and download these attachments to prepare for and participate effectively in the meeting.

---

## 🎯 Target User Personas

| Role | Description |
|------|-------------|
| **Board Secretary (أمين سر المجلس)** / **Legal Counsel (المسؤول القانوني)** | Administrators who curate the meeting's documents by adding or removing files. |
| **Board Member (أعضاء المجلس)** / **Fund Manager (مدير الصندوق)** and other attendees | Participants who need to access and review the provided documents. |

---

## 🧾 Main User Story Table

| Field | Description |
|-------|-------------|
| **Name** | Manage Meeting Attachments |
| **User Story** | **Admin**: As a Board Secretary, I want to add and remove attachments for a meeting, so I can ensure all participants have access to the correct and most current documents.<br>**Participant**: As a Board Member, I want to easily view and download all attachments for an upcoming meeting, so I can be fully prepared for the discussion. |
| **Story Points** | 5 |
| **User Roles** | **Admin**: Board Secretary, Legal Counsel<br>**Participant**: Board Members, Fund Manager, other attendees |
| **Access Requirements** | **Admin (Add/Delete)**: Must be Board Secretary or Legal Counsel<br>**Participant (View/Download)**: Must be invited to the meeting |
| **Trigger** | Admin adds/removes documents. Participant reviews documents before a meeting. |
| **Frequency of Use** | High |
| **Pre-condition** | A meeting has been scheduled. User is viewing the meeting details. |
| **Business Rules** | - Only Board Secretaries and Legal Counsels can add or delete<br>- All attendees can view/download<br>- PDF files only |
| **Post-condition** | - **Add**: File visible to all<br>- **Delete**: File removed from all views<br>- **Download**: File saved locally |
| **Risk** | Low – mitigated by delete confirmation dialog |
| **Assumptions** | System supports secure uploads/downloads and storage |
| **UX/UI Design Link** | N/A (To be linked to Figma wireframes) |

---

## 🔄 Process Flow Table

| Step | Action Description | Actor | Message Codes | Notes |
|------|---------------------|-------|----------------|-------|
| 1 | Clicks "Add Attachment" | Board Secretary / Legal Counsel | N/A | Button visible only to admins |
| 2 | Uploads a PDF file | Board Secretary / Legal Counsel | `MSG-MMA-SUC-01`, `MSG-MMA-ERR-01` | File is uploaded to server |
| 3 | New file appears in list | System | N/A | UI updates in real-time |
| 4 | Clicks "Delete" icon | Board Secretary / Legal Counsel | N/A | Only visible to admins |
| 5 | Confirms deletion | Board Secretary / Legal Counsel | `MSG-MMA-WRN-01` | Confirmation modal shown |
| 6 | File is removed | System | `MSG-MMA-SUC-02` | List updates for all |
| 7 | Clicks file name | Any Attendee | N/A | Initiates download |
| 8 | File downloaded to device | System | N/A | Browser handles this |

---

## 🔁 Alternative Flow Table

| Scenario | Condition | Action | Message Code | Resolution |
|----------|-----------|--------|--------------|------------|
| Invalid File Type Upload | Non-PDF file selected | Show error message | `MSG-MMA-ERR-02` | User must upload a PDF |
| Unauthorized Delete Attempt | Non-admin views list | Hide delete button | N/A | No control rendered |
| File Upload Fails | Upload fails due to error | Show generic error | `MSG-MMA-ERR-01` | Retry upload |

---

## ✅ Acceptance Criteria Table

| Scenario | Given | When | Then |
|----------|-------|------|------|
| Administrator Adds an Attachment | I’m Legal Counsel viewing a meeting | I click "Add Attachment" and upload a PDF | File appears in list, with success message |
| Participant Downloads an Attachment | I’m a Board Member viewing a meeting | I click file name | File is downloaded successfully |
| Administrator Deletes an Attachment | I’m Legal Counsel | I click delete and confirm | File is removed, success message shown |
| Participant Verifies Deletion | I’m a Board Member | I refresh page after deletion | File no longer appears |

---

## 🖥️ Screen Elements Table

| Element ID | Element Type | Name (EN) | Name (AR) | Validation | Business Logic | Data Entity | Interaction | Accessibility |
|------------|--------------|-----------|-----------|------------|----------------|-------------|-------------|----------------|
| ELM-MMA-001 | Panel | Attachments | المرفقات | N/A | Main container | N/A | Read | Should be a landmark `<section>` |
| ELM-MMA-002 | Button | Add Attachment | إضافة مرفق | N/A | Only for admins | Meeting_Attachment | Click | File picker opens |
| ELM-MMA-003 | List | Attachment List | قائمة المرفقات | N/A | Display files | Meeting_Attachment | Read | Use `<ul>` or `<table>` |
| ELM-MMA-004 | Link | Attachment Name | اسم المرفق | N/A | Download link | Meeting_Attachment | Click | Should be descriptive |
| ELM-MMA-005 | Icon Button | Delete | حذف | N/A | Only for admins | Meeting_Attachment | Click | Should have `aria-label` |
| ELM-MMA-006 | Modal Dialog | Confirm Deletion | تأكيد الحذف | N/A | Confirm before delete | N/A | N/A | Trap focus inside modal |

---

## 🗃️ Data Entities Table

**Entity Name**: `Meeting_Attachment`

| Attribute (EN) | Attribute (AR) | Type | Required | Notes | Example |
|----------------|----------------|------|----------|-------|---------|
| AttachmentID | معرف المرفق | Auto-increment Integer | Yes | PK | 301 |
| MeetingID | معرف الاجتماع | Relation | Yes | FK to `Meeting` | 45 |
| AttachmentType | نوع المرفق | Text (50) | Yes | General / Agenda | عام |
| AgendaItemID | معرف بند الأجندة | Relation | No | FK if AttachmentType is Agenda | 112 |
| FileName | اسم الملف | Text (255) | Yes | Name shown to user | تقرير_الربع_السنوي.pdf |
| FilePath | مسار الملف | Text (1024) | Yes | Link to file storage | /attachments/qr.pdf |
| UploadedDate | تاريخ الرفع | DateTime | Yes | Auto timestamp | 2025-07-24 10:00:00 |

---

## 📢 Messages & Notifications Table

| Code | English Message | Arabic Message | Type | Method |
|------|------------------|----------------|------|--------|
| `MSG-MMA-SUC-01` | Attachment uploaded successfully. | تم رفع المرفق بنجاح. | Success | In-App (Toast) |
| `MSG-MMA-SUC-02` | Attachment deleted successfully. | تم حذف المرفق بنجاح. | Success | In-App (Toast) |
| `MSG-MMA-ERR-01` | File upload failed. Please try again. | فشل رفع الملف. يرجى المحاولة مرة أخرى. | Error | In-App (Alert) |
| `MSG-MMA-ERR-02` | Invalid file type. Only PDF files are permitted. | نوع الملف غير صالح. يُسمح بملفات PDF فقط. | Validation | In-App (Alert) |
| `MSG-MMA-WRN-01` | Are you sure you want to permanently delete this attachment? | هل أنت متأكد من رغبتك في حذف هذا المرفق نهائيًا؟ | Warning | In-App (Modal) |

---
