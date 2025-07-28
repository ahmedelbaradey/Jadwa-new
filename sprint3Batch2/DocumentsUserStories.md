### **User Story: View and Access Fund Documents**

#### **1. Introduction**

This feature is designed to provide authorized users with seamless access to all essential documents related to an investment fund. The primary business objective is to enhance transparency, ensure good governance, and facilitate easy access to critical information such as terms and conditions, meeting minutes, and official reports. The intended users for this functionality are Fund Managers, Board Secretaries, and Board Members.

#### **2. Main User Story Table**

| Field | Description | Content Guidelines |
| :--- | :--- | :--- |
| **Name** | View and Access Fund Documents | A clear, descriptive title. |
| **User Story** | As a user (Fund Manager, Board Secretary, or Board Member), I want to view and download categorized documents so that I can stay informed and retrieve important information efficiently. | "As a [user], I want [goal] so that [benefit]" |
| **Story Points** | 5 | Fibonacci sequence (1, 2, 3, 5, 8, etc.) |
| **User Roles** | Fund Manager, Board Secretary, Board Members | All applicable user personas. |
| **Access Requirements** | The user must be authenticated and assigned to the specific fund as a manager or member. | Prerequisites for access. |
| **Trigger** | The user navigates to the "Documents" section from the main dashboard of a fund. | User action or system event that initiates the story. |
| **Frequency of Use** | Medium | High/Medium/Low with justification. |
| **Pre-condition** | The user has logged into the system and selected a fund. Documents must have been previously uploaded to the fund. | Required state before the story begins. |
| **Business Rules** | Documents must be displayed under four specific categories: "Terms and Conditions," "Meeting Minutes," "Reports," and "Others". All listed user roles have permission to view and download documents. Document translation is a Phase Two feature and is not included. | Constraints and operational policies. |
| **Post-condition**| The user successfully views the document list and/or downloads a selected document. | Expected end state after successful completion. |
| **Risk** | Low. Potential for unauthorized access is mitigated by strict role-based permissions defined in the system. | Potential risks and mitigation plans. |
| **Assumptions** | Documents are uploaded in standard, readable formats (e.g., PDF). The system provides adequate storage for all fund documents. | What we assume to be true for this story. |
| **UX/UI Design Link** | N/A (Based on BRD) | Wireframes, mockups, design system references. |

#### **3. Process Flow Table**

| Step | Action Description | Actor | Related Message Codes | Notes |
| :--- | :--- | :--- | :--- | :--- |
| 1 | The user logs into the application and navigates to the main screen of a specific fund. | User | N/A | Assumes successful authentication. |
| 2 | The user clicks on the "Documents" (الوثائق) icon or link within the fund's dashboard. | User | N/A | This action triggers navigation to the documents repository. |
| 3 | The system displays the documents screen, with all documents organized into their respective categories. | System | MSG-001 (if empty) | Categories are: Terms & Conditions, Meeting Minutes, Reports, and Others. |
| 4 | The user views the list, where each document entry displays its name, size, and a brief summary. | User | N/A | Provides an at-a-glance overview of each file. |
| 5 | The user clicks on a document's name to open it for viewing. | User | N/A | |
| 6 | The system presents the document's content within an integrated viewer. | System | N/A | The user does not need to leave the application to view the file. |
| 7 | From the viewer, the user clicks either the "Download" or "Print" button. | User | N/A | |
| 8 | The system either initiates the file download to the user's device or opens the browser's print dialog. | System | MSG-002 (on failure) | The action is completed. |

#### **4. Alternative Flow Table**

| Alternative Scenario | Condition | Action | Related Message Codes | Resolution |
| :--- | :--- | :--- | :--- | :--- |
| No documents found | A user navigates to the Documents section for a fund that has no uploaded files. | The system displays an informational message indicating that no documents are available. | MSG-001 | A Fund Manager or Board Secretary must upload documents for them to appear. |
| File download fails | The user clicks the download button, but a network or server error occurs. | The system displays an error message indicating the download has failed. | MSG-002 | The user should retry the download. If the issue persists, it needs to be reported to the system administrator. |
| Corrupted File | The user downloads and attempts to open a file, but it is corrupted and unreadable. | The user cannot view the file content locally. | MSG-003 | The user must report the corrupted file. A Fund Manager or Board Secretary is responsible for uploading a valid version. |

#### **5. Acceptance Criteria Table**

| Scenario | Given | When | Then |
| :--- | :--- | :--- | :--- |
| **1. View Document Categories** | I am an authorized user within a fund's workspace | I navigate to the "Documents" section | I must see documents organized under the four defined categories: "Terms and Conditions," "Meeting Minutes," "Reports," and "Others". |
| **2. View Document Details** | I am viewing the list of documents | I examine a single document entry | I must see the document's file name, file size, and summary description displayed. |
| **3. Download a Document** | I am viewing a document in the application's built-in viewer | I click the "Download" button | The document file must successfully download to my local device. |
| **4. Print a Document** | I am viewing a document in the application's built-in viewer | I click the "Print" button | My web browser's native print dialog must open, showing the document ready for printing. |
| **5. Handle Empty State** | I am an authorized user in a fund workspace where no documents have been uploaded | I navigate to the "Documents" section | I must see a clear message stating that no documents are currently available for this fund. |

#### **6. Screen Elements Table**

| Element ID | Element Type | Element Name (English) | Element Name (Arabic) | Required/Optional | Validation Rules | Business Logic | Related Data Entity | User Interaction | Accessibility Notes |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| ELM-001 | Navigation Link | Documents | الوثائق | Required | N/A | Navigates the user to the documents repository for the current fund. | N/A | Click | Standard link with clear text label. |
| ELM-002 | Header | Category Title (e.g., Reports) | عنوان الفئة (مثل: التقارير) | Required | N/A | Visually groups documents into their respective categories. | Document.Category | View | Use proper heading tags (H2, H3) for semantic structure. |
| ELM-003 | List Item | Document Entry | إدخال وثيقة | Required | N/A | Container displaying information for a single document. | Document | Click | Each item must be focusable and selectable via keyboard. |
| ELM-004 | Component | Document Viewer | عارض الوثائق | Required | N/A | Renders the content of the selected document within the UI. | Document.FileContent | View/Scroll | Must be compatible with screen readers. |
| ELM-005 | Button | Download | تنزيل | Required | N/A | Initiates the download of the currently viewed document file. | Document.FileContent | Click | Clear icon and text label. |
| ELM-006 | Button | Print | طباعة | Required | N/A | Opens the system's print dialog for the current document. | Document.FileContent | Click | Clear icon and text label. |
| ELM-007 | Text | Empty State Message | رسالة الحالة الفارغة | Required | N/A | Informs the user when the document list is empty. | N/A | View | Text must be readable and concise. |

#### **7. Data Entities Table**

**Entity Name:** Document

| Attribute (English) | Attribute (Arabic) | Mandatory/Optional | Attribute Type | Data Length | Integration Requirements | Default Value | Condition (if needed) | Rules (if needed) | Sample in Arabic | Sample in English |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| DocumentID | معرف الوثيقة | Mandatory | Number (Auto-Increment) | N/A | Database Primary Key | N/A | N/A | Unique system-wide identifier. | 101 | 101 |
| FileName | اسم الملف | Mandatory | Text | 255 | Database | N/A | N/A | Cannot be null or empty. | تقرير الربع الأول.pdf | Q1 Report.pdf |
| FileSize | حجم الملف | Mandatory | Text | 50 | System-Generated | N/A | N/A | Automatically calculated upon upload and stored as text (e.g., "2.1 MB"). | 2.1 ميجابايت | 2.1 MB |
| FileType | نوع الملف | Mandatory | Text | 50 | System-Generated | N/A | N/A | Mime type of the file (e.g., 'application/pdf'). | PDF | PDF |
| Category | الفئة | Mandatory | Dropdown | 100 | Database | N/A | N/A | Must be one of the four predefined values. | محاضر الاجتماعات | Meeting Minutes |
| Summary | ملخص | Optional | Text | 500 | Database | N/A | N/A | A brief, user-provided description of the document's content. | محضر اجتماع لمناقشة النتائج المالية. | Minutes for the meeting to discuss financial results. |
| FileContent | محتوى الملف | Mandatory | Binary/File | N/A | File Storage (e.g., S3, Blob) | N/A | N/A | The actual binary data of the document file. | N/A | N/A |
| UploadedDate | تاريخ الرفع | Mandatory | Datetime | N/A | System-Generated | Current Timestamp | N/A | The date and time the document was uploaded. | 2025-07-24 10:00:00 | 2025-07-24 10:00:00 |
| UploadedBy | تم الرفع بواسطة | Mandatory | Relation | N/A | Database Foreign Key (User) | N/A | N/A | A reference to the user who uploaded the document. | أحمد علي | Ahmed Ali |

#### **8. Messages/Notifications Table**

| Message Code | Message (English) | Message (Arabic) | Message Type | Communication Method |
| :--- | :--- | :--- | :--- | :--- |
| MSG-001 | No documents are available for this fund. | لا توجد وثائق متاحة لهذا الصندوق. | Notification | In-App Message |
| MSG-002 | File download failed. Please check your connection and try again. | فشل تنزيل الملف. يرجى التحقق من اتصالك والمحاولة مرة أخرى. | Error | In-App Alert |
| MSG-003 | This file may be corrupted or is unreadable. Please notify the Fund Manager. | قد يكون هذا الملف تالفًا أو غير قابل للقراءة. يرجى إبلاغ مدير الصندوق. | Warning | In-App Message |


### **User Story: Manage Fund Documents**

#### **1. Introduction**

This feature empowers authorized users to maintain the integrity and completeness of the fund's official records. The primary business objective is to provide a secure and straightforward way for Fund Managers and Board Secretaries to add new official documents and remove outdated or incorrect ones. This ensures all stakeholders are accessing the most current and relevant information.

#### **2. Main User Story Table**

| Field | Description | Content Guidelines |
| :--- | :--- | :--- |
| **Name** | Manage Fund Documents | A clear, descriptive title. |
| **User Story** | As a Fund Manager or Board Secretary, I want to upload new documents and delete obsolete ones, so that I can ensure the fund's document repository is accurate and complete. | "As a [user], I want [goal] so that [benefit]" |
| **Story Points** | 8 | Fibonacci sequence (1, 2, 3, 5, 8, etc.) |
| **User Roles** | Fund Manager, Board Secretary | All applicable user personas. |
| **Access Requirements** | The user must be authenticated and have the role of "Fund Manager" or "Board Secretary" for the specific fund. | Prerequisites for access. |
| **Trigger** | The user clicks the "Add Document" button or the "Delete" icon on the Documents screen. | User action or system event that initiates the story. |
| **Frequency of Use** | Low | High/Medium/Low with justification. |
| **Pre-condition** | The user is logged in and has navigated to the "Documents" section of a fund for which they have management rights. | Required state before the story begins. |
| **Business Rules** | • Only Fund Managers and Board Secretaries can add or delete documents. <br> • Uploaded documents must be assigned to a category. <br> • A confirmation prompt must be shown before any document is permanently deleted. | Constraints and operational policies. |
| **Post-condition**| A new document is added to the list under its chosen category, or an existing document is permanently removed from the list. | Expected end state after successful completion. |
| **Risk** | Medium. The accidental deletion of a critical document is a key risk, which is mitigated by requiring user confirmation. | Potential risks and mitigation plans. |
| **Assumptions** | The system enforces defined restrictions on file size and supported file types to ensure security and performance. | What we assume to be true for this story. |
| **UX/UI Design Link** | N/A (Based on BRD) | Wireframes, mockups, design system references. |

#### **3. Process Flow Table**

| Step | Action Description | Actor | Related Message Codes | Notes |
| :--- | :--- | :--- | :--- | :--- |
| 1 | **(Upload)** From the "Documents" screen, the user clicks the "Add Document" button. | User (Manager/Secretary) | N/A | This action is only available to authorized roles. |
| 2 | The system displays a form/modal for uploading a new document. | System | N/A | |
| 3 | The user selects a file from their local device to upload. | User (Manager/Secretary) | MSG-007, MSG-008 | The system should validate the file type and size on selection. |
| 4 | The user selects a category for the document from a predefined list. | User (Manager/Secretary) | N/A | Categories are "Terms and Conditions," "Meeting Minutes," "Reports," and "Others". |
| 5 | The user clicks the "Upload" button to submit the file. | User (Manager/Secretary) | N/A | |
| 6 | The system saves the file, updates the database, and the document list refreshes to show the new file. | System | MSG-004, MSG-009 | A success message confirms the upload. |
| 7 | **(Delete)** The user clicks the "Delete" icon next to a document they wish to remove. | User (Manager/Secretary) | N/A | |
| 8 | The system displays a confirmation prompt asking the user to confirm the deletion. | System | MSG-006 | This is a critical step to prevent accidental data loss. |
| 9 | The user confirms the deletion. | User (Manager/Secretary) | N/A | |
| 10 | The system permanently removes the document and refreshes the list. | System | MSG-005 | A success message confirms the deletion. |

#### **4. Alternative Flow Table**

| Alternative Scenario | Condition | Action | Related Message Codes | Resolution |
| :--- | :--- | :--- | :--- | :--- |
| Invalid File Type | A user attempts to upload a file with an unsupported format (e.g., an executable file). | The system blocks the upload and displays a validation error message listing the supported file types. | MSG-007 | The user must select a file with a valid and supported format. |
| File Exceeds Size Limit | A user attempts to upload a file that is larger than the system's defined maximum size. | The system blocks the upload and displays a validation error stating the maximum allowed file size. | MSG-008 | The user must select a smaller file or compress the original file before uploading. |
| User Cancels Deletion | After clicking the "Delete" icon, the user chooses "Cancel" on the confirmation prompt. | The system closes the confirmation prompt, and no changes are made. | N/A | The deletion process is terminated, and the document remains in the repository. |
| Upload Fails | A network connection error occurs while the file is being uploaded. | The system cancels the upload and displays an error message. | MSG-009 | The user should check their network connection and try the upload again. |

#### **5. Acceptance Criteria Table**

| Scenario | Given | When | Then |
| :--- | :--- | :--- | :--- |
| **1. Successful Document Upload** | I am a Fund Manager on the Documents screen | I select a valid file, choose a category, and click "Upload" | A success message must be displayed, and the new document must appear in the list under the correct category. |
| **2. Successful Document Deletion** | I am a Board Secretary on the Documents screen | I click the "Delete" icon for a document and confirm the action in the prompt | A success message must be displayed, and the document must be permanently removed from the document list. |
| **3. Attempt to Upload Invalid File** | I am a Fund Manager in the "Add Document" modal | I attempt to upload a file with a disallowed extension (e.g., .exe) | A validation error message must appear, and the upload process must be blocked until a valid file is chosen. |
| **4. Unauthorized Access Attempt** | I am logged in as a Board Member (a non-management role) | I navigate to the Documents screen | I must not see the "Add Document" button or any "Delete" icons. |

#### **6. Screen Elements Table**

| Element ID | Element Type | Element Name (English) | Element Name (Arabic) | Required/Optional | Validation Rules | Business Logic | Related Data Entity | User Interaction | Accessibility Notes |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| ELM-008 | Button | Add Document | إضافة وثيقة | Required | N/A | Opens the upload modal. Visible only to Fund Managers and Board Secretaries. | N/A | Click | Must have clear text and be keyboard accessible. |
| ELM-009 | Button/Icon | Delete Document | حذف وثيقة | Required | N/A | Initiates the deletion process for a specific document. Visible only to authorized roles. | Document | Click | Must have a clear, understandable icon and an accessible name (aria-label). |
| ELM-010 | Modal | Upload Document Form | نموذج رفع وثيقة | Required | N/A | The interface containing fields for uploading a new file. | N/A | View | Must be properly managed for focus control. |
| ELM-011 | Input | File Selector | محدد الملف | Required | File type, size | Allows the user to browse and select a local file for upload. | Document.FileContent | Click/Select | Standard file input should be labeled clearly. |
| ELM-012 | Dropdown | Category | الفئة | Required | Must select one | Assigns the uploaded document to one of the four predefined categories. | Document.Category | Select | Standard dropdown with a label. |
| ELM-013 | Button | Upload | رفع | Required | N/A | Submits the form to add the document to the repository. | N/A | Click | Becomes enabled only after a valid file is selected. |
| ELM-014 | Modal | Deletion Confirmation | تأكيد الحذف | Required | N/A | Prompts the user to confirm their intent to delete a file permanently. | N/A | View/Click | Must contain clear "Confirm" and "Cancel" buttons. |

#### **7. Data Entities Table**

This feature utilizes the same **Document** entity defined in the "View and Access Fund Documents" user story. No new data entities are required.

#### **8. Messages/Notifications Table**

| Message Code | Message (English) | Message (Arabic) | Message Type | Communication Method |
| :--- | :--- | :--- | :--- | :--- |
| MSG-004 | Document uploaded successfully. | تم رفع الوثيقة بنجاح. | Success | In-App Notification |
| MSG-005 | Document has been deleted successfully. | تم حذف الوثيقة بنجاح. | Success | In-App Notification |
| MSG-006 | Are you sure you want to permanently delete this document? This action cannot be undone. | هل أنت متأكد من رغبتك في حذف هذه الوثيقة بشكل دائم؟ لا يمكن التراجع عن هذا الإجراء. | Confirmation | In-App Prompt |
| MSG-007 | Invalid file type. Please upload one of the supported formats. | نوع الملف غير صالح. يرجى رفع ملف بأحد التنسيقات المدعومة. | Validation Error | In-App Message |
| MSG-008 | File is too large. The maximum allowed file size is [X] MB. | حجم الملف كبير جدًا. الحد الأقصى لحجم الملف المسموح به هو [X] ميجابايت. | Validation Error | In-App Message |
| MSG-009 | The document upload failed. Please check your connection and try again. | فشل رفع الوثيقة. يرجى التحقق من اتصالك والمحاولة مرة أخرى. | Error | In-App Alert |



### **User Story: Delete a Document**

#### **1. Introduction**

This feature provides an essential data management capability for Fund Managers and Board Secretaries. The primary business objective is to ensure the document repository remains accurate and up-to-date by allowing authorized users to permanently remove obsolete, incorrect, or superseded files. This maintains the integrity of the fund's official records.

#### **2. Main User Story Table**

| Field | Description | Content Guidelines |
| :--- | :--- | :--- |
| **Name** | Delete a Document | A clear, descriptive title. |
| **User Story** | [cite_start]As a Fund Manager or Board Secretary, I want to delete a document from a fund so that I can manage and remove outdated or incorrect files. [cite: 785] | "As a [user], I want [goal] so that [benefit]" |
| **Story Points** | 3 | Fibonacci sequence (1, 2, 3, 5, 8, etc.) |
| **User Roles** | [cite_start]Fund Manager, Board Secretary [cite: 785] | All applicable user personas. |
| **Access Requirements** | The user must be authenticated and have the role of "Fund Manager" or "Board Secretary" for the specific fund. | Prerequisites for access. |
| **Trigger** | The user clicks the "Delete" icon or button associated with a specific document in the list. | User action or system event that initiates the story. |
| **Frequency of Use** | Low | High/Medium/Low with justification. |
| **Pre-condition** | The user is logged in and is viewing the "Documents" section of a fund they are authorized to manage. | Required state before the story begins. |
| **Business Rules** | [cite_start]• Only Fund Managers and Board Secretaries are permitted to delete documents. [cite: 785] <br> • The system must display a confirmation prompt before the deletion is executed. <br> • Deletion is a permanent and irreversible action. | Constraints and operational policies. |
| **Post-condition**| The selected document is permanently removed from the system's storage and is no longer visible in the fund's document list. | Expected end state after successful completion. |
| **Risk** | Medium. Accidental deletion of a critical document. This risk is mitigated by the mandatory confirmation prompt. | Potential risks and mitigation plans. |
| **Assumptions** | Deleting the document record from the database also triggers the deletion of the associated file from the physical/cloud storage. | What we assume to be true for this story. |
| **UX/UI Design Link**| N/A (Based on BRD) | Wireframes, mockups, design system references. |

#### **3. Process Flow Table**

| Step | Action Description | Actor | Related Message Codes | Notes |
| :--- | :--- | :--- | :--- | :--- |
| 1 | The user navigates to the "Documents" screen and identifies the file to be removed. | User (Manager/Secretary) | N/A | The user must have the appropriate permissions. |
| 2 | The user clicks the "Delete" icon/button for the specific document. | User (Manager/Secretary) | N/A | This initiates the deletion workflow. |
| 3 | The system displays a confirmation prompt, asking the user to verify the action. | System | MSG-006 | This step is crucial to prevent accidental deletions. |
| 4 | The user clicks the "Confirm" button within the prompt. | User (Manager/Secretary) | N/A | This authorizes the system to proceed. |
| 5 | The system permanently deletes the document record from the database and the associated file from storage. | System | MSG-010 (on failure) | This is the core backend process. |
| 6 | The document list on the UI refreshes, the deleted item is gone, and a success message is displayed. | System | MSG-005 | This provides clear feedback to the user. |

#### **4. Alternative Flow Table**

| Alternative Scenario | Condition | Action | Related Message Codes | Resolution |
| :--- | :--- | :--- | :--- | :--- |
| User Cancels Deletion | After the confirmation prompt appears, the user clicks "Cancel" or closes the dialog. | The system closes the confirmation prompt. No deletion action is performed. | N/A | The workflow is terminated, and the document remains unchanged. |
| Deletion Fails | A backend error occurs (e.g., database connection lost, file storage permissions issue) during the deletion process. | The system aborts the deletion and displays an error message to the user. | MSG-010 | The user can retry the action. If the error persists, it must be escalated to a system administrator. |

#### **5. Acceptance Criteria Table**

| Scenario | Given | When | Then |
| :--- | :--- | :--- | :--- |
| **1. Successful Deletion** | I am a Fund Manager viewing the Documents list | I click the "Delete" icon for a document and then click "Confirm" on the pop-up prompt | The document must be removed from the list, and a "Document deleted successfully" message must be displayed. |
| **2. Cancellation of Deletion** | I am a Board Secretary and the deletion confirmation prompt is displayed | I click the "Cancel" button | The prompt must close, and the document must remain in the list, unchanged. |
| **3. Unauthorized Deletion Attempt**| I am logged in as a Board Member (a non-management role) | I navigate to the Documents screen and view the list of documents | I must not see any "Delete" icons or buttons, and I should be unable to trigger the deletion process. |

#### **6. Screen Elements Table**

| Element ID | Element Type | Element Name (English) | Element Name (Arabic) | Required/Optional | Validation Rules | Business Logic | Related Data Entity | User Interaction | Accessibility Notes |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| ELM-009 | Button/Icon | Delete Document | حذف وثيقة | Required | N/A | Initiates the deletion process. Must only be visible to Fund Managers and Board Secretaries. | Document | Click | Must have a clear, understandable icon and an accessible name (e.g., aria-label="Delete document"). |
| ELM-014 | Modal | Deletion Confirmation | تأكيد الحذف | Required | N/A | Prompts the user to confirm their intent to delete a file permanently. | N/A | View/Click | Modal must be properly managed for focus control and contain clear "Confirm" and "Cancel" buttons. |

#### **7. Data Entities Table**

This feature operates on the existing **Document** entity. No new data entities or attributes are required for this user story. The action performs a "delete" operation on an existing record.

#### **8. Messages/Notifications Table**

| Message Code | Message (English) | Message (Arabic) | Message Type | Communication Method |
| :--- | :--- | :--- | :--- | :--- |
| MSG-005 | Document has been deleted successfully. | تم حذف الوثيقة بنجاح. | Success | In-App Notification |
| MSG-006 | Are you sure you want to permanently delete this document? This action cannot be undone. | هل أنت متأكد من رغبتك في حذف هذه الوثيقة بشكل دائم؟ لا يمكن التراجع عن هذا الإجراء. | Confirmation | In-App Prompt |
| MSG-010 | Deletion failed. The document could not be removed. Please try again. | فشل الحذف. لم يتم حذف الوثيقة. يرجى المحاولة مرة أخرى. | Error | In-App Alert |

#### **9. Document Notifications Table**

| Message Code | Message (English)                                                                                  | Message (Arabic)                                                                                     | Message Type  | Communication Method         |
|--------------|----------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------|---------------|------------------------------|
| MSG-004      | Document uploaded successfully.                                                                    | تم رفع الوثيقة بنجاح.                                                                                 | Success       | In-App Message               |
| MSG-005      | Document has been deleted successfully.                                                            | تم حذف الوثيقة بنجاح.                                                                                 | Success       | In-App Message               |
| MSG-006      | Are you sure you want to permanently delete this document? This action cannot be undone.          | هل أنت متأكد من رغبتك في حذف هذه الوثيقة بشكل دائم؟ لا يمكن التراجع عن هذا الإجراء.                  | Confirmation  | In-App Prompt                |
| MSG-011      | Document details updated successfully.                                                             | تم تحديث تفاصيل الوثيقة بنجاح.                                                                       | Success       | In-App Message               |
| MSG-N-01     | [ActorName] added the document "[TargetEntityName]" to the [FundName] fund.                       | قام [ActorName] بإضافة الوثيقة "[TargetEntityName]" إلى صندوق [FundName].                          | Notification  | In-App Notification Center   |
| MSG-N-02     | [ActorName] updated the details for the document "[TargetEntityName]" in the [FundName] fund.     | قام [ActorName] بتحديث تفاصيل الوثيقة "[TargetEntityName]" في صندوق [FundName].                   | Notification  | In-App Notification Center   |
| MSG-N-03     | [ActorName] deleted the document "[TargetEntityName]" from the [FundName] fund.                   | قام [ActorName] بحذف الوثيقة "[TargetEntityName]" من صندوق [FundName].                            | Notification  | In-App Notification Center   |
