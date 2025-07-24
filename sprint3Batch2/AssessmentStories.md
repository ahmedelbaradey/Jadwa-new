
# User Story: Create New Assessment  
**Version**: 1.0  
**Date**: 2025-07-24  
**Author**: Mofaker (Business Analyst)

---

## 1. Introduction

This document outlines the requirements for the **"Create New Assessment"** feature. This functionality allows a **Fund Manager** to create a new assessment, define its content (either as a questionnaire or an attachment for review), and submit it for approval by the **Legal Council** or **Board Secretary**.  
This process is the first step in gathering formal feedback from board members on specific fund-related topics. The requirements are based on the user request and the **"Assessments (التقييمات)"** section of the BRD.

---

## 2. Main User Story

| Field               | Description |
|--------------------|-------------|
| **Name**           | Create New Assessment |
| **User Story**     | As a Fund Manager, I want to create a new assessment with various question types (questionnaire or attachment review) and save it as a draft or submit it for approval, so I can gather structured feedback from board members. |
| **Story Points**   | 8 |
| **User Roles**     | Fund Manager, Legal Council, Board Secretary |
| **Access Requirements** | User must be authenticated and hold an authorized role. |
| **Trigger**        | Clicking the "Create New Assessment" button within the "Assessments" section of a specific fund. |
| **Frequency of Use** | Medium |
| **Pre-condition**  | User is logged in and has navigated to the Assessments page of a specific fund. |
| **Business Rules** | - Assessment must have a title<br>- Must select type: Questionnaire or Attachment<br>- At least one question for Questionnaire<br>- Supported question types: Single choice, Text<br>- Attachment file required for Attachment type<br>- Status: Draft or Waiting for Approval<br>- Submit triggers notifications |
| **Post-condition** | - Draft: saved privately<br>- Submitted: status changes to "Waiting for Approval" and notifications sent |
| **Risk**           | UI for dynamic questions may be complex |
| **Mitigation**     | Clear UI/UX with tooltips and phased feature rollout |
| **Assumptions**    | System identifies relevant board members based on the fund |

---

## 3. Process Flow (Happy Path)

**Scenario**: A Fund Manager creates a new questionnaire-based assessment and submits it.

| Step | Action Description | Actor | Related Message Codes | Notes |
|------|---------------------|--------|------------------------|-------|
| 1 | Navigate to "Assessments" and click "Create New Assessment" | Fund Manager | | |
| 2 | System shows "Create New Assessment" form | System | | |
| 3 | Enter title (e.g., "Q3 Performance Evaluation") | Fund Manager | | |
| 4 | Select "Questionnaire" as Assessment Type | Fund Manager | | Dynamically shows question interface |
| 5 | Add "Single Choice" question with options | Fund Manager | | Repeat as needed |
| 6 | Add "Text" question | Fund Manager | | |
| 7 | Click "Submit for Approval" | Fund Manager | | |
| 8 | System validates required fields | System | VAL-ASM-001, VAL-ASM-002 | |
| 9 | System saves assessment with status "Waiting for Approval" | System | MSG-ASM-001 | Read-only for creator |
| 10 | Notifications sent to Legal Council & Board Secretary | System | MSG-ASM-002 | |

---

## 4. Alternative Flows

| Alternative Scenario | Condition | Action | Related Message Codes | Resolution |
|----------------------|-----------|--------|------------------------|------------|
| Save as Draft | Save without submitting | Click "Save as Draft" | MSG-ASM-003 | Saved as Draft, no notifications |
| Validation Error | Title or questions missing | Validation triggered on submit | VAL-ASM-001, VAL-ASM-002 | User must complete fields |
| Create Attachment Assessment | Select "Attachment" type | Upload document and submit | VAL-ASM-003 | Same flow but with attachment |

---

## 5. Acceptance Criteria

| Scenario | Given | When | Then |
|----------|-------|------|------|
| **Successful Questionnaire Submission** | Fund Manager on "Create New Assessment" page | Enter title, select type, add questions, click submit | Assessment saved, notifications sent |
| **Successful Attachment Submission** | Fund Manager on "Create New Assessment" page | Enter title, upload PDF, click submit | Assessment saved, notifications sent |
| **Save as Draft** | Fund Manager on form | Enter title only, click save | Draft saved, no notification |
| **Submission with Missing Title** | Fund Manager on form | Add questions only, click submit | Error shown, assessment not saved |

---

## 6. Screen Elements

| ID | Type | Name (EN) | Name (AR) | Required | Validation | Business Logic | Entity | Interaction | Accessibility |
|----|------|------------|------------|----------|------------|----------------|--------|-------------|----------------|
| ELM-ASM-001 | Input Field | Assessment Title | عنوان التقييم | Required | Max 255 chars |  | Assessment.Title | Type | `aria-label` |
| ELM-ASM-002 | Radio Group | Assessment Type | نوع التقييم | Required | Must select | Show/hide sections | Assessment.Type | Select | Use `<fieldset>` |
| ELM-ASM-003 | Section | Question Builder | قسم الأسئلة | Conditional | Required if type is "Questionnaire" | Add/Edit/Delete questions | AssessmentQuestion | Interact | |
| ELM-ASM-004 | File Upload | Attachment | المرفق | Conditional | Required if type is "Attachment" | Accepts .pdf/.docx | Assessment.Attachment_URL | Upload | |
| ELM-ASM-005 | Button | Add Question | أضف سؤال | Optional | N/A | Adds question block | N/A | Click | |
| ELM-ASM-006 | Button | Submit for Approval | إرسال للموافقة | Optional | N/A | Triggers validation | N/A | Click | Primary |
| ELM-ASM-007 | Button | Save as Draft | حفظ كمسودة | Optional | N/A | Save without submit | N/A | Click | Secondary |
| ELM-ASM-008 | Button | Cancel | إلغاء | Optional | N/A | Discard and return | N/A | Click | Tertiary |

---

## 7. Data Entities

### Entity: **Assessment**

| Attribute | Arabic | Required | Type | Rules | Sample (AR) | Sample (EN) |
|-----------|--------|----------|------|--------|--------------|--------------|
| AssessmentID | معرف التقييم | Mandatory | Auto Integer | Primary Key | 101 | 101 |
| FundID | معرف الصندوق | Mandatory | FK |  | 22 | 22 |
| Title | العنوان | Mandatory | Text (255) | Not empty | تقييم أداء الربع الثالث | Q3 Performance Review |
| Type | النوع | Mandatory | Enum | 'Questionnaire', 'Attachment' | استبيان | Questionnaire |
| Status | الحالة | Mandatory | Enum | Various status values | مسودة | Draft |
| AttachmentURL | رابط المرفق | Optional | Text (URL) | Required for Attachment | /docs/assessment1.pdf | /docs/assessment1.pdf |
| CreatedBy | تم الإنشاء بواسطة | Mandatory | FK (User) |  | عبدالله عثمان | Abdullah Osman |
| CreationDate | تاريخ الإنشاء | Mandatory | DateTime |  | 24-07-2025 | 2025-07-24 |

### Entity: **AssessmentQuestion**

| Attribute | Arabic | Required | Type | Rules | Sample (AR) | Sample (EN) |
|-----------|--------|----------|------|--------|--------------|--------------|
| QuestionID | معرف السؤال | Mandatory | Auto Integer | Primary Key | 2001 | 2001 |
| AssessmentID | معرف التقييم | Mandatory | FK (Assessment) |  | 101 | 101 |
| QuestionText | نص السؤال | Mandatory | Text (1000) |  | ما هو تقييمك العام للأداء؟ | What is your overall rating of the performance? |
| QuestionType | نوع السؤال | Mandatory | Enum | 'SingleChoice', 'Text' | اختيار واحد | SingleChoice |
| Options | الخيارات | Optional | JSON or FK | Required for SingleChoice | ["ممتاز", "جيد", "ضعيف"] | ["Excellent", "Good", "Poor"] |

---

## 8. Messages & Notifications

| Code | Message (EN) | Message (AR) | Type | Method |
|------|---------------|---------------|------|--------|
| MSG-ASM-001 | Assessment submitted for approval successfully. | تم إرسال التقييم للموافقة بنجاح. | Success | In-App |
| MSG-ASM-002 | A new assessment '[Assessment.Title]' is waiting for your approval. | تقييم جديد بعنوان "[Assessment.Title]" ينتظر موافقتك. | Notification | Email, In-App |
| MSG-ASM-003 | Assessment saved as draft. | تم حفظ التقييم كمسودة. | Success | In-App |
| VAL-ASM-001 | Assessment Title is required. | عنوان التقييم مطلوب. | Validation | On-Screen |
| VAL-ASM-002 | At least one question is required. | مطلوب سؤال واحد على الأقل. | Validation | On-Screen |
| VAL-ASM-003 | Attachment is required. | مطلوب إرفاق ملف. | Validation | On-Screen |






# User Story: Approve or Reject Assessment
**Version:** 1.0  
**Date:** 2025-07-24  
**Author:** Mofaker Business Analyst  

---

## 1. Introduction  
This document outlines the requirements for the "Approve or Reject Assessment" feature. This functionality serves as a critical quality control step in the assessment lifecycle. It allows authorized reviewers (Legal Council, Board Secretary) to examine assessments submitted by Fund Managers. Based on their review, they can either approve the assessment for distribution or reject it, providing feedback for revision. This governance step ensures that all assessments sent to board members are accurate, relevant, and well-formed.

---

## 2. Main User Story  
| Field | Description |
|-------|-------------|
| **Name** | Approve or Reject Assessment |
| **User Story** | As a Legal Council or Board Secretary, I want to review a submitted assessment, and then approve or reject it, so that I can ensure its quality and relevance before it is sent to board members. |
| **Story Points** | 3 |
| **User Roles** | Legal Council, Board Secretary |
| **Access Requirements** | User must be authenticated with the system and hold the role of "Legal Council" or "Board Secretary". |
| **Trigger** | User receives a notification for an assessment awaiting approval and clicks the link, or filters the assessments list for "Waiting for Approval" status and selects an item. |
| **Frequency of Use** | Medium |
| **Pre-condition** | An assessment exists in the system with a "Waiting for Approval" status. The user is logged in with appropriate review permissions. |
| **Business Rules** | - The reviewer can see all details of the assessment but cannot edit the content.<br>- The reviewer must choose one of two actions: "Approve" or "Reject".<br>- If the assessment is rejected, providing a reason/comment is mandatory.<br>- Approving an assessment changes its status to "Approved".<br>- Rejecting an assessment changes its status to "Rejected".<br>- The original creator (Fund Manager) must be notified of the outcome (approval or rejection). |
| **Post-condition** | - On Approve: The assessment status is updated to "Approved". A notification is sent to the creator. The assessment is now ready for distribution.<br>- On Reject: The assessment status is updated to "Rejected". A notification, including the rejection reason, is sent to the creator. |
| **Risk** | Delays in the approval process can create a bottleneck. Mitigation: Implement a dashboard widget for reviewers showing pending approvals and configure automated reminder notifications for assessments awaiting review for more than 48 hours. |
| **Assumptions** | The first authorized reviewer (either Legal Council or Board Secretary) to act on the assessment determines its outcome. Approval from multiple roles is not required for this version. |
| **UX/UI Design Link** | N/A |

---

## 3. Process Flow (Happy Path)  
**Scenario:** A Legal Council reviews and approves a newly submitted assessment.  

| Step | Action Description | Actor | Related Message Codes | Notes |
|------|---------------------|--------|------------------------|-------|
| 1 | Receives an in-app notification: "A new assessment is waiting for your approval." Clicks on the notification. | Legal Council | MSG-ASM-002 | |
| 2 | The system displays the assessment details in a read-only view. The "Approve" and "Reject" action buttons are visible at the bottom of the page. | System | | All fields, questions, and/or attachments are displayed for review. |
| 3 | Reviews the assessment's title, type, and content (questions or attachment) for clarity and relevance. | Legal Council | | |
| 4 | Clicks the "Approve" button. | Legal Council | | |
| 5 | The system updates the assessment's status in the database from "Waiting for Approval" to "Approved". | System | MSG-ASM-004 | |
| 6 | The system sends a notification to the Fund Manager who created the assessment. | System | MSG-ASM-005 | The notification informs the creator that their assessment has been approved. |
| 7 | The user is redirected back to the assessments list, where the assessment now shows an "Approved" status. | System | |

---

## 4. Alternative Flows  

| Alternative Scenario | Condition | Action | Related Message Codes | Resolution |
|----------------------|-----------|--------|------------------------|------------|
| **Reject Assessment** | The reviewer finds the assessment's questions are ambiguous or the attached document is incorrect. | 1. Clicks the "Reject" button.<br>2. The system displays a dialog box with a mandatory text area for "Rejection Reason".<br>3. The reviewer enters their feedback (e.g., "Please rephrase question 2 for clarity.").<br>4. Clicks "Confirm Rejection". | VAL-ASM-004, MSG-ASM-006 | The system updates the assessment's status to "Rejected" and sends the rejection reason in a notification to the creator. The assessment can then be edited and resubmitted by the creator. |
| **Attempt to Reject Without Reason** | The reviewer clicks "Reject" and then tries to confirm without entering a reason. | The system prevents the action and displays a validation error message. | VAL-ASM-004 | The reviewer must provide a reason to proceed with the rejection. |

---

## 5. Acceptance Criteria  

| Scenario | Given | When | Then |
|----------|-------|------|------|
| **Successful Approval** | I am a Legal Council viewing an assessment with the status "Waiting for Approval". | I review the content and click the "Approve" button. | The assessment's status should change to "Approved", and a notification of the approval should be sent to the Fund Manager who created it. |
| **Successful Rejection** | I am a Board Secretary viewing an assessment with the status "Waiting for Approval". | I click the "Reject" button, enter a mandatory reason like "The attached document is outdated", and confirm the rejection. | The assessment's status should change to "Rejected", and a notification including my reason should be sent to the Fund Manager who created it. |
| **Rejection Requires a Reason** | I am a Legal Council viewing an assessment with the status "Waiting for Approval". | I click the "Reject" button and then try to confirm without entering any text in the "Rejection Reason" field. | The system must prevent the rejection and display an error message indicating that the reason is a required field. |

---

## 6. Screen Elements  

| Element ID | Element Type | Element Name (English) | Element Name (Arabic) | Required/Optional | Validation Rules | Business Logic | Related Data Entity | User Interaction | Accessibility Notes |
|------------|---------------|--------------------------|------------------------|--------------------|------------------|----------------|----------------------|------------------|----------------------|
| ELM-REV-001 | Read-only Text | Assessment Title | عنوان التقييم | N/A | N/A | Displays the assessment's title. | Assessment.Title | View | |
| ELM-REV-002 | Read-only View | Assessment Content | محتوى التقييم | N/A | N/A | Displays the questions or a link to the attachment. | AssessmentQuestion / Assessment.AttachmentURL | View | |
| ELM-REV-003 | Button | Approve | موافقة | N/A | N/A | Triggers the approval workflow. | N/A | Click | Primary action button. |
| ELM-REV-004 | Button | Reject | رفض | N/A | N/A | Triggers the rejection workflow. | N/A | Click | Secondary action button. |
| ELM-REV-005 | Text Area | Rejection Reason | سبب الرفض | Conditional | Required if user clicks "Reject". Not empty. | Appears in a modal upon clicking "Reject". | Assessment.ReviewerComments | Type | aria-label for "Rejection Reason" |

---

## 7. Data Entities  

**Entity Name:** Assessment (Update)  

| Attribute (English) | Attribute (Arabic) | Mandatory/Optional | Attribute Type | Rules (if needed) | Sample in Arabic | Sample in English |
|---------------------|--------------------|---------------------|----------------|--------------------|------------------|--------------------|
| Status | الحالة | Mandatory | Enum | Updated to 'Approved' or 'Rejected'. | موافق عليه | Approved |
| ReviewerComments | ملاحظات المراجع | Optional | Text (2000) | Stores the rejection reason if applicable. | يرجى إعادة صياغة السؤال الثاني ليكون أكثر وضوحًا. | Please rephrase question 2 for more clarity. |
| ReviewedBy | تمت المراجعة بواسطة | Optional | Relation (User) | Stores the ID of the user who approved/rejected. | سارة أحمد | Sara Ahmed |
| ReviewedDate | تاريخ المراجعة | Optional | DateTime | Timestamp of the approval/rejection action. | 24-07-2025 | 2025-07-24 |

---

## 8. Messages/Notifications  

| Message Code | Message (English) | Message (Arabic) | Message Type | Communication Method |
|--------------|-------------------|------------------|----------------|----------------------|
| MSG-ASM-004 | The assessment has been approved. | تمت الموافقة على التقييم. | Success | In-App Message |
| MSG-ASM-005 | Your assessment '[Assessment.Title]' has been approved. | تمت الموافقة على تقييمك "[Assessment.Title]". | Notification | Email, In-App Notification |
| MSG-ASM-006 | Your assessment '[Assessment.Title]' has been rejected. Reason: [Assessment.ReviewerComments] | تم رفض تقييمك "[Assessment.Title]". السبب: [Assessment.ReviewerComments] | Notification | Email, In-App Notification |
| VAL-ASM-004 | Rejection reason is required. | سبب الرفض مطلوب. | Validation | On-Screen |

---

## 9. Summary & Next Steps  
This user story defines the crucial approval gate in the assessment workflow, ensuring proper governance and quality control. Its implementation depends on the completion of the "Create New Assessment" story. Once an assessment is approved, it becomes ready for the next logical step: distribution to the board members.


---

## User Story 3: Distribute Assessment

### 1. Introduction
This user story covers the process of making an approved assessment "live". This action is performed by the Fund Manager and involves changing the assessment's status and sending out notifications to all relevant board members, officially kicking off the feedback and data collection process.

### 2. Main User Story

| Field               | Description |
|--------------------|-------------|
| **Name**           | Distribute Assessment |
| **User Story**     | As a Fund Manager, I want to send an approved assessment to all relevant board members to initiate the feedback process. |
| **Story Points**   | 2 |
| **User Roles**     | Fund Manager |
| **Access Requirements** | User must be authenticated with the system and hold the "Fund Manager" role. |
| **Trigger**        | User navigates to the assessments list, finds an assessment with "Approved" status, and clicks the "Distribute" action button. |
| **Frequency of Use** | Medium. This action is performed once for every assessment that is successfully approved. |
| **Pre-condition**  | An assessment exists in the system with an "Approved" status. Board members are assigned to the corresponding fund. |
| **Business Rules** | - Only assessments with the status "Approved" can be distributed. The "Distribute" action should be disabled for all other statuses.  
- Once distributed, the assessment status changes to "Active".  
- Upon distribution, a notification is sent to every board member associated with the fund.  
- An "Active" assessment can no longer be edited or deleted. |
| **Post-condition** | - The assessment's status is updated to "Active".  
- Notifications are queued and sent to all relevant board members. |
| **Risk**           | Sending to an incorrect list of members. **Mitigation:** The system should automatically populate the recipient list based on the fund's current board membership, with an optional, non-editable review step for the Fund Manager before confirmation. |
| **Assumptions**    | The system knows which board members are associated with which fund and will handle the notification delivery. |
| **UX/UI Design Link** | N/A |

---

### 3. Process Flow (Happy Path)

| Step | Action Description | Actor | Related Message Codes | Notes |
|------|--------------------|--------|------------------------|-------|
| 1 | Navigates to the "Assessments" page and locates an assessment with "Approved" status. | Fund Manager | | |
| 2 | Clicks the "Distribute" action associated with that assessment. | Fund Manager | | |
| 3 | The system displays a confirmation dialog: "Are you sure you want to send this assessment to [X] board members?" | System | MSG-DIST-001 | [X] is the number of members in the fund. |
| 4 | Clicks "Confirm". | Fund Manager | | |
| 5 | The system updates the assessment's status to "Active". | System | | |
| 6 | The system sends a notification to all board members of the fund. | System | MSG-DIST-002 | |
| 7 | The UI on the assessments list page updates to show the assessment's new "Active" status. | System | | The "Distribute" action is no longer available. |

---

### 4. Alternative Flows

| Alternative Scenario | Condition | Action | Related Message Codes | Resolution |
|----------------------|-----------|--------|------------------------|------------|
| Action Not Available | The Fund Manager views an assessment with a "Draft" or "Rejected" status. | The "Distribute" action button is disabled or not visible for that row in the assessments list. | N/A | User cannot perform the action, which is the correct behavior. |
| User Cancels Distribution | The Fund Manager clicks "Distribute" but then clicks "Cancel" in the confirmation dialog. | The system closes the dialog and no changes are made. | N/A | The assessment remains in the "Approved" state. |

---

### 5. Acceptance Criteria

| Scenario | Given | When | Then |
|----------|-------|------|------|
| Successful Distribution | I am a Fund Manager and there is an "Approved" assessment. | I click the "Distribute" action and confirm. | The assessment's status must change to "Active", and all board members of that fund must receive a notification to complete the assessment. |
| Action Disabled for Draft | I am a Fund Manager viewing an assessment with a "Draft" status. | I look for a "Distribute" action. | The "Distribute" action must not be available for me to click. |
| Action Disabled for Active | I am a Fund Manager viewing an assessment that is already "Active". | I look for a "Distribute" action. | The "Distribute" action must not be available for me to click. |

---

### 6. Screen Elements

| Element ID | Element Type | Element Name (English) | Element Name (Arabic) |
|------------|--------------|-------------------------|------------------------|
| ELM-DIST-001 | Button/Icon | Distribute | توزيع / إرسال |
| ELM-DIST-002 | Modal Dialog | Confirmation Dialog | مربع حوار للتأكيد |
| ELM-DIST-003 | Button | Confirm | تأكيد |
| ELM-DIST-004 | Status Badge | Active | نشط |

---

### 7. Data Entities

- **Entity Name:** Assessment (Update)  
  - **Status:** Updated from Approved to Active.

- **Entity Name:** AssessmentResponse (New records created)  
  - A new record is created for each board member of the fund with a default Status of Pending.

---

### 8. Messages/Notifications

| Message Code | Message (English) | Message (Arabic) | Message Type | Communication Method |
|--------------|-------------------|------------------|---------------|----------------------|
| MSG-DIST-001 | Are you sure you want to send this assessment to [X] board members? It cannot be recalled. | هل أنت متأكد من إرسال هذا التقييم إلى [X] من أعضاء المجلس؟ لا يمكن التراجع عن هذا الإجراء. | Confirmation | In-App Dialog |
| MSG-DIST-002 | A new assessment, '[Assessment.Title]', is ready for your review. Please submit your response. | تقييم جديد بعنوان "[Assessment.Title]" جاهز لمراجعتك. يرجى تقديم إجابتك. | Notification | Email, In-App Notification |


# User Story 4: Respond to Assessment

## 1. Introduction

This user story details the core interaction for board members. It covers how a board member accesses an active assessment, provides answers to the defined questions or reviews the attached material, and submits their feedback to the system. This process is the primary mechanism for data collection within the feature.

---

## 2. Main User Story

| Field               | Description                                                                 |
|--------------------|-----------------------------------------------------------------------------|
| **Name**           | Respond to Assessment                                                       |
| **User Story**     | As a Board Member, I want to access an assessment, provide my answers and comments, and submit my feedback for recording. |
| **Story Points**   | 5                                                                           |
| **User Roles**     | Board Member                                                                |
| **Access Requirements** | User must be authenticated and have the role of "Board Member".        |
| **Trigger**        | User clicks on a notification link for a new assessment or selects an assessment with a "Pending Response" status from their dashboard/list. |
| **Frequency of Use** | Medium (depends on how often assessments are distributed)                 |
| **Pre-condition**  | Assessment is "Active". User is a board member of the relevant fund and has "Pending" response status. |
| **Business Rules** | - A member can only respond once.  
- For "Questionnaire" types, mandatory questions must be answered.  
- For "Attachment" types, comment is optional.  
- Submission locks response.  
- System saves response and timestamp. |
| **Post-condition** | - AssessmentResponse is saved or updated with status = "Completed".  
- Response is locked from further edits. |
| **Risk**           | Low response rate. _Mitigation_: Add auto-reminders.                        |
| **Assumptions**    | No mandatory deadline in current version (could be added later).            |
| **UX/UI Design Link** | N/A                                                                     |

---

## 3. Process Flow (Happy Path)

| Step | Action Description                                                                 | Actor         | Related Message Codes | Notes                                   |
|------|-------------------------------------------------------------------------------------|---------------|------------------------|-----------------------------------------|
| 1    | Clicks on link from notification or selects assessment from list                   | Board Member  | MSG-DIST-002           |                                         |
| 2    | System shows questions or attachment                                                | System        |                        | Fields are editable for new submission  |
| 3    | Enters answers or selects options                                                   | Board Member  |                        |                                         |
| 4    | Clicks "Submit Response"                                                            | Board Member  |                        |                                         |
| 5    | System validates required answers                                                   | System        | VAL-RESP-001           | If successful, proceed to next step     |
| 6    | System saves data and marks status as "Completed"                                   | System        | MSG-RESP-001           |                                         |
| 7    | Displays success message and switches to read-only mode                             | System        | MSG-RESP-001           | Submit button disabled                  |

---

## 4. Alternative Flows

| Alternative Scenario              | Condition                                                 | Action                                                                 | Message Codes     | Resolution                                          |
|----------------------------------|-----------------------------------------------------------|------------------------------------------------------------------------|-------------------|-----------------------------------------------------|
| Incomplete Submission            | Skips a required question and clicks "Submit"             | System prevents submission and highlights missing fields                | VAL-RESP-001      | Complete required fields before proceeding          |
| Viewing an Already Completed Assessment | Clicks an already submitted assessment link          | System shows the response in read-only mode                             | N/A               | Prevents duplicate/duplicate editing                |

---

## 5. Acceptance Criteria

| Scenario                      | Given                                                  | When                             | Then                                                                 |
|------------------------------|--------------------------------------------------------|----------------------------------|----------------------------------------------------------------------|
| Successful Questionnaire      | I am viewing an active questionnaire assessment        | I submit complete answers        | Answers saved, status = "Completed", shows read-only view           |
| Successful Attachment         | I view an active attachment assessment                 | I submit optional comment        | Comment saved, status = "Completed"                                 |
| Incomplete Response           | I skip a required question in a questionnaire          | I click submit                   | System displays validation error for missing answers                |

---

## 6. Screen Elements

| Element ID      | Element Type         | English Name         | Arabic Name           |
|------------------|----------------------|-----------------------|------------------------|
| ELM-RESP-001    | Read-only Text       | Question Text         | نص السؤال              |
| ELM-RESP-002    | Input/Radio Group    | Answer Input          | حقل الإجابة            |
| ELM-RESP-003    | Text Area            | General Comments      | تعليقات عامة           |
| ELM-RESP-004    | Button               | Submit Response       | إرسال الإجابة          |

---

## 7. Data Entities

### Entity: AssessmentResponse (Update)

Represents the submission from a board member.

| Attribute (English)   | Arabic Name         | Mandatory | Type                 | Rules                          | Sample (AR)            | Sample (EN)         |
|-----------------------|---------------------|-----------|----------------------|--------------------------------|-------------------------|---------------------|
| ResponseID            | معرف الاستجابة      | Yes       | Auto-increment Int   | Primary Key                    | 501                     | 501                 |
| AssessmentID          | معرف التقييم         | Yes       | FK (Assessment)      |                                | 101                     | 101                 |
| UserID                | معرف المستخدم        | Yes       | FK (User)            | Must be board member           | 998                     | 998                 |
| Status                | الحالة               | Yes       | Enum                 | 'Pending' → 'Completed'        | مكتمل                  | Completed           |
| SubmissionDate        | تاريخ الإرسال         | Optional  | DateTime             | On final submit                | 2025-07-24              | 2025-07-24          |
| GeneralComments       | تعليقات عامة          | Optional  | Text                 | Free-text comment              | لا توجد تعليقات إضافية. | No additional comments |

---

### Entity: Answer (New)

Stores each question's answer.

| Attribute (English)   | Arabic Name        | Mandatory | Type                 | Rules                          | Sample (AR)  | Sample (EN)     |
|-----------------------|--------------------|-----------|----------------------|--------------------------------|--------------|-----------------|
| AnswerID              | معرف الإجابة       | Yes       | Auto-increment Int   | Primary Key                    | 9001         | 9001            |
| ResponseID            | معرف الاستجابة     | Yes       | FK (AssessmentResponse) |                              | 501          | 501             |
| QuestionID            | معرف السؤال        | Yes       | FK (AssessmentQuestion) |                             | 2001         | 2001            |
| AnswerValue           | قيمة الإجابة       | Optional  | Text (4000)          | Text or selected option        | ممتاز        | Excellent        |
| CreatedDate           | تاريخ الإنشاء       | Yes       | DateTime             | Timestamp on save              | 2025-07-24    | 2025-07-24       |

---

## 8. Messages / Notifications

| Code           | Message (English)                                  | Message (Arabic)                                      | Type       | Channel         |
|----------------|----------------------------------------------------|-------------------------------------------------------|------------|-----------------|
| MSG-RESP-001   | Your response has been submitted successfully.     | تم استلام إجابتك بنجاح. شكراً لك.                     | Success    | In-App          |
| VAL-RESP-001   | Please answer all required questions before submitting. | يرجى الإجابة على جميع الأسئلة المطلوبة قبل الإرسال. | Validation | On-Screen       |



# User Story 5: View Compiled Assessment Results

## 1. Introduction

This user story focuses on delivering actionable insights to management. After responses have been collected, Fund Managers and other authorized roles need a way to view and analyze the data. This feature provides a results dashboard with aggregated data visualizations for quantitative questions and a clear list of all qualitative feedback.

---

## 2. Main User Story

| Field               | Description                                                                 |
|--------------------|-----------------------------------------------------------------------------|
| **Name**           | View Compiled Assessment Results                                            |
| **User Story**     | As a Fund Manager or Board Secretary, I want to view the compiled results and individual feedback for an assessment to analyze the outcomes. |
| **Story Points**   | 5                                                                           |
| **User Roles**     | Fund Manager, Board Secretary, Legal Council                                |
| **Access Requirements** | User must be authenticated and have one of the authorized management roles. |
| **Trigger**        | User navigates to the assessments list and clicks a "View Results" action on an "Active" or "Completed" assessment. |
| **Frequency of Use** | High                                                                      |
| **Pre-condition**  | Assessment is "Active" or "Completed" and has at least one response submitted. |
| **Business Rules** | - Show a summary with completion rate  
- Aggregate and visualize single-choice answers  
- List all text-based responses  
- Show who submitted each response  
- Update in real-time for active assessments |
| **Post-condition** | N/A (This is a data-viewing feature)                                       |
| **Risk**           | Misinterpretation of data. _Mitigation_: Label charts, use tooltips, allow export |
| **Assumptions**    | Assessment is "Completed" when all members respond                         |
| **UX/UI Design Link** | N/A                                                                     |

---

## 3. Process Flow (Happy Path)

| Step | Action Description                                                               | Actor         | Related Message Codes | Notes                         |
|------|-----------------------------------------------------------------------------------|---------------|------------------------|-------------------------------|
| 1    | Navigates to the assessments list and finds the relevant assessment              | Fund Manager  |                        |                               |
| 2    | Clicks the "View Results" action for the assessment                              | Fund Manager  |                        |                               |
| 3    | System displays the Results Dashboard                                             | System        |                        |                               |
| 4    | Sees the summary with completion rate                                             | Fund Manager  |                        | e.g., "7 out of 10 responded" |
| 5    | Views aggregated results (pie chart, bar chart) per question                     | Fund Manager  |                        |                               |
| 6    | Views text-based answers with responding member names                            | Fund Manager  |                        |                               |
| 7    | Clicks "View Individual Responses" tab to see complete submissions               | Fund Manager  |                        |                               |

---

## 4. Acceptance Criteria

| Scenario                      | Given                                                  | When                             | Then                                                                 |
|------------------------------|--------------------------------------------------------|----------------------------------|----------------------------------------------------------------------|
| View Aggregated Results      | I am viewing assessment with multiple-choice questions | I navigate to results page       | I see charts showing answer percentages and counts                   |
| View Text Responses          | I am viewing assessment with text-based questions      | I navigate to results page       | I see list of all responses with member names                        |
| View Completion Rate         | 5 of 10 members have responded                         | I navigate to results page       | I see summary showing 50% completion rate                            |
| Drill Down to Individual     | I am on results page                                   | I click a respondent’s name      | I see full response of that member                                   |

---

## 5. Screen Elements

| Element ID      | Element Type        | Element Name (English)      | Element Name (Arabic)       |
|------------------|----------------------|------------------------------|------------------------------|
| ELM-RES-001     | Data Display        | Completion Rate              | نسبة الاكتمال               |
| ELM-RES-002     | Chart (Pie/Bar)     | Aggregated Results Chart     | رسم بياني للنتائج المجمعة   |
| ELM-RES-003     | List                | List of Text Responses       | قائمة بالإجابات النصية       |
| ELM-RES-004     | Table               | Individual Responses         | الإجابات الفردية             |

---

## 6. Data Entities

This feature reads data from the following tables:

- **Assessment**
- **AssessmentQuestion**
- **AssessmentResponse**

> No new data entities are created.


# User Story 6: View Personal Assessment Details

## 1. Introduction

This user story provides board members with transparency and a personal record of their activity. It allows them to view a list of all assessments they have been assigned, see which ones are pending their response, and review the specific answers they submitted for completed assessments.

---

## 2. Main User Story

| Field               | Description                                                                 |
|--------------------|-----------------------------------------------------------------------------|
| **Name**           | View Personal Assessment Details                                            |
| **User Story**     | As a Board Member, I want to view the assessments assigned to me and my submitted responses for my records and to take proper action. |
| **Story Points**   | 2                                                                           |
| **User Roles**     | Board Member                                                                |
| **Access Requirements** | User must be authenticated and have the "Board Member" role.           |
| **Trigger**        | User logs in and navigates to a "My Assessments" or "Dashboard" page.       |
| **Frequency of Use** | Medium. Used to track pending tasks and review past activity.             |
| **Pre-condition**  | The user is a board member and has been assigned at least one assessment.   |
| **Business Rules** | - The view must list all assessments assigned to the logged-in member.  
- The list must clearly display the status for each assessment from the member's perspective (e.g., "Pending Response", "Completed").  
- The member must be able to click on a completed assessment to view their own submission in a read-only format. |
| **Post-condition** | N/A                                                                         |
| **Risk**           | N/A                                                                         |
| **Assumptions**    | N/A                                                                         |
| **UX/UI Design Link** | N/A                                                                      |

---

## 3. Process Flow (Happy Path)

| Step | Action Description                                                                 | Actor         | Related Message Codes | Notes                         |
|------|-------------------------------------------------------------------------------------|---------------|------------------------|-------------------------------|
| 1    | Logs into the system and navigates to the "My Assessments" page.                   | Board Member  |                        |                               |
| 2    | The system displays a list of all assessments assigned to this member. Each item shows the title, fund name, and status ("Pending Response" or "Completed"). | System        |                        |                               |
| 3    | Clicks on an assessment with the status "Completed".                               | Board Member  |                        |                               |
| 4    | The system displays a read-only view of the assessment, showing the questions and the specific answers the member submitted. | System        |                        | No action buttons are present.|

---

## 4. Acceptance Criteria

| Scenario                 | Given                                                   | When                            | Then                                                                 |
|--------------------------|----------------------------------------------------------|----------------------------------|----------------------------------------------------------------------|
| View Assessment List     | I am a Board Member who has been assigned two assessments, one of which I have completed. | I navigate to the "My Assessments" page. | I must see a list with two items, one marked "Completed" and the other marked "Pending Response". |
| View Completed Submission| I am a Board Member on the "My Assessments" page.        | I click on an assessment marked "Completed". | I must be shown a read-only page displaying the questions from that assessment and the exact answers I submitted. |
| Action on Pending Assessment | I am a Board Member on the "My Assessments" page.     | I click on an assessment marked "Pending Response". | I must be taken to the assessment response form where I can fill out and submit my answers (as per User Story 4). |

---

## 5. Screen Elements

| Element ID      | Element Type    | Element Name (English)       | Element Name (Arabic)         |
|------------------|------------------|-------------------------------|-------------------------------|
| ELM-MYA-001     | List/Table       | My Assessments List           | قائمة تقييماتي               |
| ELM-MYA-002     | Status Badge     | Status (Pending/Completed)    | الحالة (معلق/مكتمل)          |
| ELM-MYA-003     | Read-only View   | My Submitted Response         | إجاباتي المقدمة              |

---

## 6. Data Entities (View Mode)

This feature **reads data** from the following tables:

- **Assessment**
- **AssessmentResponse**

Filtered by the `UserID` of the logged-in Board Member.

---

## 7. Data Entities (Submission/Update Mode)

This workflow updates the `AssessmentResponse` entity and creates multiple records in the new, related `Answer` entity. This normalized structure provides better clarity and scalability for storing response data.

### Entity: AssessmentResponse (Update)

| Attribute (English) | Attribute (Arabic) | Mandatory/Optional | Attribute Type         | Rules                      | Sample (Arabic)           | Sample (English)      |
|---------------------|--------------------|---------------------|--------------------------|----------------------------|----------------------------|-----------------------|
| ResponseID          | معرف الاستجابة      | Mandatory           | Auto-increment Integer   | Primary Key                | 501                        | 501                   |
| AssessmentID        | معرف التقييم        | Mandatory           | Relation (Assessment)    | Foreign Key                | 101                        | 101                   |
| UserID              | معرف المستخدم       | Mandatory           | Relation (User)          | Foreign Key                | 998                        | 998                   |
| Status              | الحالة              | Mandatory           | Enum                     | 'Pending' → 'Completed'    | مكتمل                     | Completed             |
| SubmissionDate      | تاريخ الإرسال        | Optional            | DateTime                 | Timestamp on final submit  | 2025-07-24                 | 2025-07-24            |
| GeneralComments     | تعليقات عامة         | Optional            | Text                     | Free-form text             | لا توجد تعليقات إضافية.     | No additional comments. |

---

### Entity: Answer (New)

| Attribute (English) | Attribute (Arabic) | Mandatory/Optional | Attribute Type           | Rules                      | Sample (Arabic)           | Sample (English)      |
|---------------------|--------------------|---------------------|---------------------------|----------------------------|----------------------------|-----------------------|
| AnswerID            | معرف الإجابة        | Mandatory           | Auto-increment Integer    | Primary Key                | 9001                       | 9001                  |
| ResponseID          | معرف الاستجابة      | Mandatory           | Relation (AssessmentResponse) | Foreign Key           | 501                        | 501                   |
| QuestionID          | معرف السؤال          | Mandatory           | Relation (AssessmentQuestion) | Foreign Key           | 2001                       | 2001                  |
| AnswerValue         | قيمة الإجابة         | Optional            | Text (max 4000 chars)     | Text or selected choice    | ممتاز                      | Excellent             |
| CreatedDate         | تاريخ الإنشاء        | Mandatory           | DateTime                  | Answer save timestamp      | 2025-07-24                 | 2025-07-24            |
