
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
