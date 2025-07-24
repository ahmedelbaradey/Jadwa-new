## User Story JDWA-1258: Add Fund's Board Member (with Conditional Registration WhatsApp Notification)
### Introduction Section
This user story describes the process for a Legal Counsel or Board Secretary to add a new board member to a specific fund within the Fund Board Management Application. This includes capturing basic member information, performing validations, and ensuring that the new board member receives an automated notification upon successful attachment to the fund. Crucially, if the added member is a user who previously had ONLY the 'Board Member' role (and thus did not receive the initial registration WhatsApp message), this process will now trigger the sending of that message, making them eligible to use the system. The `Registration Message Is Sent` flag will be updated to 1 upon the \*attempt\* to send this message. The Screen Elements Table provides a detailed breakdown of the UI components on the "Add New Member" screen.
### Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|Add Fund's Board Member (with Conditional Registration WhatsApp Notification)|Allows Legal Counsel/Board Secretary to add a board member to a fund, sending conditional registration WhatsApp and other notifications.|
|\*\*User Story\*\*|As a \*\*legal council/board secretary\*\*, I want to \*\*add a board member to a fund and ensure they receive their system access message if eligible\*\* so that I can \*\*manage board members within my funds and onboard them to the system.\*\*||
|\*\*Story Points\*\*|9|Involves data entry, multiple validations, conditional fund status change, multiple notifications, and conditional WhatsApp send/flag update.|
|\*\*User Roles\*\*|Legal Counsel, Board Secretary|Only these roles can add board members.|
|\*\*Access Requirements\*\*|Authenticated and authorized access to the "Members" screen within a specific fund.|User must be logged in and have appropriate permissions.|
|\*\*Trigger\*\*|User presses "add member" from the members screen.|User initiates the action.|
|\*\*Frequency of Use\*\*|Low to Medium|Board members are added periodically.|
|\*\*Pre-condition\*\*|User is logged in as Legal Counsel/Board Secretary. The fund exists and is accessible. The user being added as a member must exist in the system.|The system is operational and the user has necessary permissions.|
|\*\*Business Rules\*\*|1\. Maximum number of active board members per fund is 15.<br>2. Maximum number of active independent board members per fund is 14.<br>3. Minimum of 2 active independent board members required to change fund status to "Active".<br>4. New members are saved with status "active".<br>5. An in-app notification (MSG002) is sent to the new board member upon successful attachment to the fund.<br>6. Notifications (MSG007, MSG008) are sent to Fund Manager/Legal Counsel/Board Secretary/other Board Members attached to the fund.<br>7. Basic info data is required.<br>8. Only one board chairman per fund.<br>\*\*9. Conditional WhatsApp Registration Message:\*\* If the user being added as a board member has their 'Registration Message Is Sent' flag = 0 (indicating they were initially an "ONLY Board Member" and didn't receive the WhatsApp message), the system will attempt to send the WhatsApp registration message (content from MSG-ADD-008). \*\*Upon this attempt, the user's 'Registration Message Is Sent' flag is updated to 1, regardless of delivery success.\*\*|Ensures data integrity, fund status management, comprehensive notification, and conditional WhatsApp onboarding for Board Members.|
|\*\*Post-condition\*\*|The new board member is attached to the fund, notifications are sent, and a success message is displayed. Fund status may change to "Active". If applicable, the WhatsApp registration message is attempted to be sent to the new board member and their 'Registration Message Is Sent' flag is updated to 1.|The system state reflects the new member's addition and related updates, including their onboarding status.|
|\*\*Risk\*\*|1\. Data validation errors.<br>2. Notification delivery failure.<br>3. Incorrect fund status change.<br>4. WhatsApp message delivery failure.|Mitigation: Robust validation, notification monitoring, clear logic for fund status, WhatsApp API monitoring.|
|\*\*Assumptions\*\*|1\. Fund and user data are correctly managed elsewhere.<br>2. Notification mechanisms (e.g., WhatsApp API) are functional.<br>3. "Member type" (Independent/Not Independent) is a selectable field.<br>4. The 'Registration Message Is Sent' flag is correctly set during initial user creation (0 for ONLY Board Member, 1 otherwise).|These assumptions are crucial for implementing the functionality.|
|\*\*UX/UI Design Link\*\*|N/A (General form design, confirmation messages)|No specific mockups provided beyond basic screen elements.|
### Process Flow Table

|**Step**|**Action Description**|**Actor**|**Related Message Codes**|**Notes**|
| :- | :- | :- | :- | :- |
|1|From members screen, User presses "add member".|Legal Counsel/Board Secretary|||
|2|System checks if the number of active board members is 15.|System|MSG005|If yes, display error and abort.|
|3|System displays "new member" screen.|System|||
|4|User fills in basic info required data (Member Name, Type, Is board chairman).|Legal Counsel/Board Secretary|Member Name is a selection of an existing user.||
|5|User presses "add" button.|Legal Counsel/Board Secretary|||
|6|System validates entered required data.|System|MSG001, MSG009|If any violation, display error and abort.|
|7|System checks if new member is independent AND if number of active independent board members is 14.|System|MSG006|If yes, display error and abort.|
|8|System saves the new member with status "active" and links them to the fund.|System|||
|9|System checks if number of active independent users is 2.|System|If yes, proceed to change fund status.||
|10|If condition in Step 9 is met: System changes fund status to "Active".|System|MSG008|Notification (fund) is sent.|
|11|System sends in-app notification to the new board member attached to the fund.|System|MSG002|Notification activity (members).|
|12|System sends notification to fund manager/legal counsel/board secretary attached to the fund.|System|MSG007|Notification activity (members).|
|13|System sends notification to fund manager/legal counsel/board secretary/board members attached to the fund.|System|MSG008|Notification activity (fund).|
|14|\*\*System checks if the new board member's 'Registration Message Is Sent' flag is 0.\*\*|System|This identifies "ONLY Board Member" users who haven't received the WhatsApp message yet.||
|15|\*\*If condition in Step 14 is met:\*\* System attempts to send the WhatsApp registration message (content from MSG-ADD-008) to the new board member.|System|MSG-ADD-008, MSG-ADD-009|Requires WhatsApp Business API call.|
|16|\*\*Upon attempt to send WhatsApp message (from Step 15):\*\* System updates the new board member's 'Registration Message Is Sent' flag to 1.|System|\*\*This update happens regardless of delivery success.\*\*||
|17|System displays success message.|System|MSG003||
### Alternative Flow Table

|**Alternative Scenario**|**Condition**|**Action**|**Related Message Codes**|**Resolution**|
| :- | :- | :- | :- | :- |
|\*\*Max Board Members Reached\*\*|User attempts to add a member when active board members = 15.|System displays an error message.|MSG005|User cannot add more members.|
|\*\*Missing Required Fields\*\*|User attempts to add member without filling all mandatory fields.|System displays an error message indicating required fields.|MSG001|User must fill all mandatory fields.|
|\*\*Max Independent Board Members Reached\*\*|New member is independent AND active independent board members = 14.|System displays an error message.|MSG006|User cannot add more independent members.|
|\*\*Board Chairman Conflict\*\*|User attempts to add a member as chairman when another chairman already exists.|System displays an error message.|MSG009|User must uncheck "Is board chairman" or select another member.|
|\*\*Unknown Error during Submission\*\*|An unexpected error occurs during data saving.|System displays an error message.|MSG004|User can retry or contact support.|
|\*\*WhatsApp Registration Message Failure\*\*|System attempts to send the WhatsApp registration message (Step 15) but fails.|System logs the failure.|MSG-ADD-009|System Admin may need to manually notify the user or verify mobile number. \*\*The 'Registration Message Is Sent' flag is already set to 1 (due to the attempt).\*\*|
### Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Successful Board Member Submission (WhatsApp Attempted)\*\*|The user is on the "add board member" screen. The fund has 1 active independent member. The selected user is an existing user with ONLY the 'Board Member' role, and their 'Registration Message Is Sent' flag is 0.|The user fills in the required fields for this new independent member and clicks "add".|The new board member is attached to the fund, the fund status changes to "Active", MSG002 (in-app) is sent to the new board member, MSG007 and MSG008 are sent to relevant parties, the WhatsApp registration message (MSG-ADD-008) is attempted to be sent to the new board member, their 'Registration Message Is Sent' flag is updated to 1 (due to the attempt), and a confirmation message (MSG003) is displayed.|
|\*\*Successful Board Member Submission (No WhatsApp Sent - Already Sent)\*\*|The user is on the "add board member" screen. The selected user is an existing user with the 'Fund Manager' role (meaning their 'Registration Message Is Sent' flag is already 1).|The user fills in the required fields for this new member and clicks "add".|The new board member is attached to the fund, MSG002 (in-app) is sent, MSG007 and MSG008 are sent, \*\*NO WhatsApp registration message is attempted to be sent\*\*, and a confirmation message (MSG003) is displayed.|
|\*\*Missing Required Fields\*\*|The user has not filled in all mandatory fields on the "add board member" screen.|The user clicks "add".|An error message (MSG001) is displayed indicating which fields are required.|
|\*\*Max Board Members Reached\*\*|The fund already has 15 active board members.|The user clicks "add member" from the members screen.|An error message (MSG005) is displayed, preventing the "new member" screen from appearing.|
|\*\*WhatsApp Registration Message Failure (Flag Still 1)\*\*|A new board member is added, they are an "ONLY Board Member" with `Registration Message Is Sent` = 0, but the WhatsApp message fails to send.|The system attempts to send the WhatsApp message.|The WhatsApp message is not sent, the failure is logged (MSG-ADD-009), and the new board member's 'Registration Message Is Sent' flag is updated to 1 (due to the attempt).|
### Data Entities Table
Entity Name: Board Member (for adding)

|**Attribute (English)**|**Attribute (Arabic)**|**Mandatory/Optional**|**Attribute Type**|**Data Length**|**Integration Requirements**|**Default Value**|**Condition (if needed)**|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|Member Name (User ID)|اسم العضو (معرف المستخدم)|Mandatory|Number|N/A|Relation to User Entity|N/A|N/A|Must select from predefined list.|محمد علي|Mohammed Ali|
|Member Type|نوع العضو|Mandatory|Radio Button|N/A|N/A|Independent|N/A|Independent / Not Independent|مستقل|Independent|
|Is Board Chairman|هل العضو رئيس مجلس إدارة|Optional|Checkbox|N/A|N/A|Default: Not selected|N/A|Only one chairman per board.|نعم|Yes|
|Status|الحالة|Mandatory|Text|N/A|N/A|Active|N/A|Active/Inactive|نشط|Active|
|Fund ID|معرف الصندوق|Mandatory|Number|N/A|Relation to Fund Entity|N/A|N/A|Must be linked to an existing fund.|F123|F123|

Entity Name: User (relevant attributes for Board Member)

|**Attribute (English)**|**Attribute (Arabic)**|**Mandatory/Optional**|**Attribute Type**|**Data Length**|**Integration Requirements**|**Default Value**|**Condition (if needed)**|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|User ID|معرف المستخدم|Mandatory|Number|N/A|Database Primary Key|N/A|N/A|Unique identifier|12345|12345|
|Mobile|رقم الجوال|Mandatory|Text|Max 10|Database Field|N/A|N/A|Saudi mobile format (e.g., 05XXXXXXXX), Numeric only.|0501234567|0501234567|
|Password|كلمة المرور|Mandatory|Text|N/A|Database Field|N/A|N/A|System-generated, stored securely (hashing handled internally).|N/A|N/A|
|Role|الدور|Mandatory|Relation to Role Entity|N/A|Database Many-to-Many|N/A|At least one role must be selected.|Predefined roles only|عضو مجلس|Board Member|
|\*\*Registration Message Is Sent\*\*|\*\*تم إرسال رسالة التسجيل\*\*|\*\*Mandatory\*\*|\*\*Boolean\*\*|\*\*N/A\*\*|\*\*Database Field\*\*|\*\*0 (False)\*\*|\*\*N/A\*\*|\*\*Set to 1 if user's role(s) make them eligible for WhatsApp message (NOT only 'Board Member'); 0 if only 'Board Member'. This flag is set based on eligibility and attempt to send.\*\*|0|False|
|Status|الحالة|Mandatory|Boolean/Dropdown|N/A|Database Field|Active|N/A|Active/Inactive|نشط|Active|

Entity Name: Fund (relevant attributes)

|**Attribute (English)**|**Attribute (Arabic)**|**Mandatory/Optional**|**Attribute Type**|**Data Length**|**Integration Requirements**|**Default Value**|**Condition (if needed)**|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|Fund ID|معرف الصندوق|Mandatory|Number|N/A|Database Primary Key|N/A|N/A|Unique identifier|F123|F123|
|Fund Status|حالة الصندوق|Mandatory|Text|N/A|N/A|N/A|N/A|Active/Inactive/etc.|نشط|Active|
|Active Board Members Count|عدد أعضاء المجلس النشطين|Derived|Number|N/A|Derived from Board Member entity|0|N/A|Count of active members linked to fund.|5|5|
|Active Independent Board Members Count|عدد أعضاء المجلس المستقلين النشطين|Derived|Number|N/A|Derived from Board Member entity|0|N/A|Count of active independent members.|2|2|
### Messages/Notifications Table

|**Message Code**|**Message (English)**|**Message (Arabic)**|**Message Type**|**Communication Method**|
| :- | :- | :- | :- | :- |
|MSG001|Required Field.|حقل إلزامي.|Error|In-App|
|MSG002|You were added as a board member [member type] to the fund [fund name] by [role name] [user name].|تم إضافتك كعضو "[نوع العضو]" في مجلس الصندوق "[اسم الصندوق]" بواسطة "[اسم الدور]" "[اسم المستخدم]".|Notification|In-App|
|MSG003|Record Saved Successfully.|تم حفظ البيانات بنجاح.|Success|In-App|
|MSG004|An error is occurred while saving data.|حدث خطأ بالنظام , لم يتم حفظ البيانات.|Error|In-App|
|MSG005|You have reached the maximum number of board members of this fund.|لقد وصلت للحد الأقصى لعدد أعضاء مجلس الصندوق.|Notification/Error|In-App|
|MSG006|You have reached the maximum number of independent board members of this fund.|لقد وصلت للحد الأقصى لعدد الأعضاء المستقلين للصندوق.|Notification/Error|In-App|
|MSG007|A new "[member type]" member "[member name]" is added to the fund "[fund name]" by "[role name]" "[user name]".|تم إضافة عضو جديد "[نوع العضو]" "[اسم العضو]" في مجلس الصندوق "[اسم الصندوق]" بواسطة "[اسم الدور]" "[اسم المستخدم]".|Notification|System (Implicitly WhatsApp/SMS/Email based on system setup)|
|MSG008|"fund name" is successfully activated, as 2 independent members are attached to it.|تم تفعيل الصندوق "[اسم الصندوق]" بنجاح، وذلك بإتمام إضافة 2 عضو مستقل.|Notification|System (Implicitly WhatsApp/SMS/Email based on system setup)|
|MSG009|Board members include only one chairman.|مجلس أعضاء الصندوق يحوي رئيس مجلس إدارة واحد فقط.|Error|In-App|
|MSG-ADD-008|Your registration for Jadwa Fund Board Management is complete. Your temporary password is: [Password]. Please log in through this link [login URL].|تم تسجيلك في تطبيق إدارة مجالس صناديق جدوى. كلمة المرور المؤقتة هي: [كلمة المرور]. يرجى تسجيل الدخول من خلال هذا الرابط [رابط تسجيل الدخول].|Success Message|WhatsApp|
|MSG-ADD-009 (from Add User)|Failed to send WhatsApp confirmation message. Please notify the user manually.|فشل إرسال رسالة تأكيد الواتساب. يرجى إخطار المستخدم يدوياً.|Warning Message|In-App / System Log|
### Screen Elements Table

|**Element ID**|**Element Type**|**Element Name (English)**|**Element Name (Arabic)**|**Required/Optional**|**Validation Rules**|**Business Logic**|**Related Data Entity**|**User Interaction**|**Accessibility Notes**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|ELM-ADDMB-001|Button|Add Member Button|زر إضافة عضو|Mandatory|N/A|Initiates the process to add a new board member.|N/A|Click|Clearly labeled.|
|ELM-ADDMB-002|Page Title|New Member Screen|شاشة عضو جديد|N/A|N/A|Displays the title of the new member form.|N/A|View|H1 heading.|
|ELM-ADDMB-003|Dropdown/Searchable List|Member Name|اسم العضو|Mandatory|Must select from system source (existing users).|Selects an existing user to be added as a board member.|Board Member.MemberName (User.UserID)|Select|Clear label, searchable.|
|ELM-ADDMB-004|Radio Button Group|Member Type|نوع العضو|Mandatory|One selection only.|Selects if the member is Independent or Not Independent.|Board Member.MemberType|Select|Clear labels for options.|
|ELM-ADDMB-005|Checkbox|Is Board Chairman|هل العضو رئيس مجلس إدارة|Optional|Only one chairman per fund.|Designates the member as the board chairman.|Board Member.IsBoardChairman|Check/Uncheck|Clear label.|
|ELM-ADDMB-006|Button|Add Button|زر أضف|Mandatory|N/A|Submits the form to add the new member.|N/A|Click|Primary action button.|
|ELM-ADDMB-007|Button|Cancel Button|زر إلغاء|Mandatory|N/A|Discards changes and returns to members list screen.|N/A|Click|Secondary action button.|
|ELM-ADDMB-008|Text Label|Error Message Display|عرض رسالة الخطأ|Conditional|N/A|Displays validation or system error messages (e.g., MSG001, MSG004, MSG005, MSG006, MSG009, MSG-ADD-009).|N/A|View|Prominent display.|
|ELM-ADDMB-009|Text Label|Success Message Display|عرض رسالة النجاح|Conditional|N/A|Displays confirmation of successful member addition (MSG003).|N/A|View|Prominent display.|
### Summary Section
This rewritten user story for "Add Fund's Board Member (with Conditional Registration WhatsApp Notification)" now consistently applies the logic for the `Registration Message Is Sent` flag: it is set to `1` upon the \*attempt\* to send the WhatsApp message, regardless of delivery success. This ensures alignment with the updated definition of this flag across all user management stories. The conditional sending of the WhatsApp registration message (MSG-ADD-008) to "ONLY Board Member" users when they are added to a fund remains a key part of this story.

\*\*Key Implementation Considerations:\*\*

\*   Implement the precise conditional logic for sending the WhatsApp message (Step 14-16 in Process Flow).

\*   Ensure secure retrieval of the user's temporary password and login URL for MSG-ADD-008.

\*   Handle WhatsApp API failures gracefully and log them.

\*   The `Registration Message Is Sent` flag must be updated transactionally with the WhatsApp send attempt.

\*\*Dependencies and Risks:\*\*

\*   \*\*Dependency:\*\* This story depends on the "Add New System User" story (for initial user creation and setting of `Registration Message Is Sent` flag). It also depends on the WhatsApp Business API.

\*   \*\*Risk:\*\* Users not receiving their initial login credentials if the WhatsApp send fails and is not manually followed up. Mitigation: Robust error logging and admin notification (MSG-ADD-009).

\*   \*\*Risk:\*\* Confusion if the user receives multiple messages or no message due to incorrect flag logic.

\---
## User Story JDWA-1257: Reset User Password
### Introduction Section
This user story describes the functionality for System Administrators to reset the password for an existing user account within the Fund Board Management Application. This action is crucial for assisting users who have forgotten their password or for security reasons. The action is strictly conditional, available only for active users whose registration message was sent and whose overall registration is completed. Upon successful reset, the user receives their new temporary password via WhatsApp. The Screen Elements Table provides a detailed breakdown of the UI components involved in this process.
### Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|Reset User Password|Allows System Administrators to reset a user's password and notify them via WhatsApp.|
|\*\*User Story\*\*|As a \*\*System Administrator\*\*, I want to \*\*reset a user's password and send them the new password via WhatsApp\*\* so that I can \*\*help them regain access to their account.\*\*||
|\*\*Story Points\*\*|5|Involves user selection, conditional logic, password generation, database update, and external notification.|
|\*\*User Roles\*\*|System Admin|Only System Administrators can perform this action.|
|\*\*Access Requirements\*\*|Authenticated and authorized System Admin access. User must be selected from the user list or details page.|User must be logged in as a System Admin.|
|\*\*Trigger\*\*|System Admin clicks "Reset Password" button/link for a specific user from the user list or user details page.|User initiates the action.|
|\*\*Frequency of Use\*\*|Low to Medium|Used when users forget passwords or for security reasons.|
|\*\*Pre-condition\*\*|System Admin is logged in. The target user account exists and has a valid Saudi mobile number. \*\*The user must be 'Active', AND their 'Registration Is Completed' flag must be 1, AND their 'Registration Message Is Sent' flag must be 1.\*\*|The system is operational, and the user meets all eligibility criteria.|
|\*\*Business Rules\*\*|1\. The "Reset Password" action is \*\*ONLY applicable\*\* if the user's Status is 'Active' AND their 'Registration Is Completed' flag is 1 AND their 'Registration Message Is Sent' flag is 1.<br>2. The "Reset Password" button/link must be hidden if the user does not meet these criteria.<br>3. The user must have a valid Saudi mobile number registered.<br>4. The system will automatically generate a new, strong temporary password.<br>5. A confirmation dialog should be presented before resetting the password.<br>6. The new temporary password will be sent to the user via WhatsApp.<br>7. The 'Last Update Date' must be automatically updated upon successful password reset.|Ensures strict conditional availability, secure password generation, and user notification.|
|\*\*Post-condition\*\*|The user's password is reset to a new temporary password, the 'Last Update Date' is refreshed, and the new password is sent to the user via WhatsApp.|The system state reflects the updated password, and the user is informed.|
|\*\*Risk\*\*|1\. WhatsApp message delivery failure.<br>2. New password security concerns (e.g., weak generation, insecure transmission).<br>3. Resetting password for an incorrect user.|Mitigation: Confirmation dialog, strong password generation, secure WhatsApp API, audit trails.|
|\*\*Assumptions\*\*|1\. WhatsApp Business API integration is available and configured.<br>2. Default password policy for generated passwords is defined (e.g., length, complexity).<br>3. The system handles password hashing internally.|These assumptions are crucial for implementing the functionality.|
|\*\*UX/UI Design Link\*\*|N/A (Refer to general action button/confirmation dialog patterns)|No specific mockups provided in the BRD for this section.|
### Process Flow Table

|**Step**|**Action Description**|**Actor**|**Related Message Codes**|**Notes**|
| :- | :- | :- | :- | :- |
|1|System Admin is viewing the "View System Users List" or "View User Details" page.|System Admin|||
|2|System Admin identifies an eligible user and clicks the "Reset Password" button/link for that user.|System Admin|This button/link is only visible/enabled if the user meets all eligibility criteria.||
|3|System displays a confirmation dialog asking the System Admin to confirm the password reset.|System|MSG-RESET-001|Includes the user's name in the message.|
|4|System Admin reviews the confirmation message and clicks "Confirm" or "Cancel".|System Admin|If "Cancel" is clicked, the process stops.||
|5|System generates a new, strong temporary password for the user.|System|||
|6|System updates the user's password in the database with the new generated password (hashing handled internally).|System|The 'Last Update Date' is automatically refreshed.||
|7|System attempts to send a WhatsApp message to the user containing their new temporary password and login instructions.|System|MSG-RESET-002, MSG-RESET-003|Requires WhatsApp Business API call.|
|8|System displays a success or failure message to the System Admin based on the password reset and WhatsApp message sending attempt.|System|MSG-RESET-002, MSG-RESET-003|Note: Using MSG-RESET-002 for success, MSG-RESET-003 for failure.|
|9|System Admin can continue managing users.|System Admin|||
### Alternative Flow Table

|**Alternative Scenario**|**Condition**|**Action**|**Related Message Codes**|**Resolution**|
| :- | :- | :- | :- | :- |
|\*\*Action Not Applicable (Eligibility Not Met)\*\*|System Admin attempts to click "Reset Password" for a user who is NOT 'Active', OR whose 'Registration Is Completed' flag is 0, OR whose 'Registration Message Is Sent' flag is 0.|The "Reset Password" button/link is \*\*hidden\*\*.|System Admin cannot perform the action.||
|\*\*Confirmation Canceled\*\*|System Admin clicks "Reset Password" but then clicks "Cancel" in the confirmation dialog.|System closes the confirmation dialog and no password reset occurs.|No action performed.||
|\*\*WhatsApp Message Failure\*\*|The system successfully resets the password but fails to send the WhatsApp message.|System displays a warning message to the System Admin and logs the failure.|MSG-RESET-003|System Admin may need to manually provide the password to the user.|
|\*\*System Error during Password Reset\*\*|A backend error occurs during the password reset operation.|System displays a generic error message.|MSG-RESET-004|System Admin can retry later or contact support.|
|\*\*User Not Found\*\*|The selected user's ID is invalid or the user has been deleted.|System displays an error message (e.g., "User not found").|MSG-RESET-005|System Admin can return to the user list.|
### Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Reset Password - Success (Eligible User)\*\*|A System Admin is logged in. A user is 'Active', their 'Registration Is Completed' flag is 1, and their 'Registration Message Is Sent' flag is 1.|The System Admin clicks "Reset Password" for this user and clicks "Confirm" in the dialog.|The user's password is reset to a new temporary password, the 'Last Update Date' is updated, a WhatsApp message with the new password is sent to the user, and a success message "Password reset successfully." is displayed to the System Admin.|
|\*\*Reset Password - Not Applicable (Inactive User)\*\*|A System Admin is logged in. A user is 'Inactive'.|The System Admin attempts to click "Reset Password" for this user.|The "Reset Password" button/link is \*\*hidden\*\*, and the action cannot be performed.|
|\*\*Reset Password - Not Applicable (Registration Not Complete)\*\*|A System Admin is logged in. A user is 'Active', but their 'Registration Is Completed' flag is 0.|The System Admin attempts to click "Reset Password" for this user.|The "Reset Password" button/link is \*\*hidden\*\*, and the action cannot be performed.|
|\*\*Reset Password - Not Applicable (Message Not Sent)\*\*|A System Admin is logged in. A user is 'Active', their 'Registration Is Completed' flag is 1, but their 'Registration Message Is Sent' flag is 0.|The System Admin attempts to click "Reset Password" for this user.|The "Reset Password" button/link is \*\*hidden\*\*, and the action cannot be performed.|
|\*\*Reset Password - Failure (WhatsApp Message Failure)\*\*|A System Admin is logged in. A user is eligible, the password is reset, but the WhatsApp message fails to send.|The System Admin clicks "Reset Password" for this user and clicks "Confirm" in the dialog.|The user's password is reset, but a warning message "Failed to send new password via WhatsApp. Please notify the user manually." is displayed to the System Admin.|
|\*\*Reset Password - Confirmation Canceled\*\*|A System Admin clicks "Reset Password" for a user.|The System Admin clicks "Cancel" in the confirmation dialog.|The confirmation dialog closes, and the password is not reset.|
|\*\*System Error during Reset\*\*|A System Admin attempts to reset a password.|A backend error occurs during the password reset operation.|The system displays a generic error message "An error is occurred while resetting password." and the password remains unchanged.|
### Data Entities Table
Entity Name: User (for resetting password)

|**Attribute (English)**|**Attribute (Arabic)**|**Mandatory/Optional**|**Attribute Type**|**Data Length**|**Integration Requirements**|**Default Value**|**Condition (if needed)**|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|User ID|معرف المستخدم|Mandatory|Number|N/A|Database Primary Key|N/A|N/A|Unique identifier|12345|12345|
|Mobile|رقم الجوال|Mandatory|Text|Max 10|Database Field|N/A|N/A|Saudi mobile format (e.g., 05XXXXXXXX), Numeric only, Unique (as username).|0501234567|0501234567|
|Password|كلمة المرور|Mandatory|Text|N/A|Database Field|N/A|N/A|System-generated, stored securely (hashing handled internally).|N/A|N/A|
|Status|الحالة|Mandatory|Boolean/Dropdown|N/A|Database Field|Active|N/A|Active/Inactive|نشط|Active|
|Registration Message Is Sent|تم إرسال رسالة التسجيل|Mandatory|Boolean|N/A|Database Field|0 (False)|N/A|Set to 1 if user's role(s) make them eligible for WhatsApp message (NOT only 'Board Member'); 0 if only 'Board Member'.|1|True|
|Registration Is Completed|تم اكتمال التسجيل|Mandatory|Boolean|N/A|Database Field|0 (False)|N/A|Set to 0 upon user creation. Updated by a separate process (e.g., user's first login).|0|False|
|Last Update Date|تاريخ آخر تحديث|Mandatory|DateTime|N/A|Database Field|Current Timestamp|N/A|Automatically set on creation and update.|2023-10-26 15:00:00|2023-10-26 15:00:00|
### Messages/Notifications Table

|**Message Code**|**Message (English)**|**Message (Arabic)**|**Message Type**|**Communication Method**|
| :- | :- | :- | :- | :- |
|MSG-RESET-001|Are you sure you want to reset the password for [User Name]? A new temporary password will be sent via WhatsApp.|هل أنت متأكد من إعادة تعيين كلمة المرور للمستخدم [اسم المستخدم]؟ سيتم إرسال كلمة مرور مؤقتة جديدة عبر الواتساب.|Confirmation Dialog|In-App|
|MSG-RESET-002|Password reset successfully. A new temporary password has been sent to the user via WhatsApp.|تم إعادة تعيين كلمة المرور بنجاح. تم إرسال كلمة مرور مؤقتة جديدة للمستخدم عبر الواتساب.|Success Message|In-App|
|MSG-RESET-003|Failed to send new password via WhatsApp. Please notify the user manually.|فشل إرسال كلمة المرور الجديدة عبر الواتساب. يرجى إخطار المستخدم يدوياً.|Warning Message|In-App / System Log|
|MSG-RESET-004|An error is occurred while resetting password.|حدث خطأ بالنظام , لم يتم إعادة تعيين كلمة المرور.|Error Message|In-App|
|MSG-RESET-005|User not found.|لم يتم العثور على المستخدم.|Error Message|In-App|
|\*\*MSG-RESET-006\*\*|\*\*Your password for Jadwa Fund Board Management has been reset. Your new temporary password is: [New Password]. Please log in through this link [login URL] and change your password.\*\*|\*\*تمت إعادة تعيين كلمة المرور الخاصة بك لتطبيق إدارة مجالس صناديق جدوى. كلمة المرور المؤقتة الجديدة هي: [كلمة المرور الجديدة]. يرجى تسجيل الدخول من خلال هذا الرابط [رابط تسجيل الدخول] وتغيير كلمة المرور الخاصة بك.\*\*|\*\*Success Message\*\*|\*\*WhatsApp\*\*|
### Screen Elements Table

|**Element ID**|**Element Type**|**Element Name (English)**|**Element Name (Arabic)**|**Required/Optional**|**Validation Rules**|**Business Logic**|**Related Data Entity**|**User Interaction**|**Accessibility Notes**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|ELM-RESET-001|Button|Reset Password |` `إعادة تعيين كلمة المرور|Conditional|N/A|Initiates the password reset process. \*\*Hidden if user is not eligible.\*\*|N/A|Click|Clear label, visible/enabled based on conditions.|
|ELM-RESET-002|Confirmation Dialog|Reset Password Confirmation Dialog|نافذة تأكيد إعادة تعيين كلمة المرور|Mandatory|N/A|Prompts System Admin to confirm password reset.|N/A|View, Click (Confirm/Cancel)|Clear message, accessible buttons.|
|ELM-RESET-003|Text Label|Confirmation Message Text|نص رسالة التأكيد|Mandatory|N/A|Displays "Are you sure you want to reset the password for [User Name]?"|N/A|View|Dynamic content for user name.|
|ELM-RESET-004|Button|Confirm Reset |تأكيد إعادة التعيين|Mandatory|N/A|Confirms the password reset action.|N/A|Click|Clearly labeled.|
|ELM-RESET-005|Button|Cancel Reset |إلغاء إعادة التعيين|Mandatory|N/A|Cancels the password reset action.|N/A|Click|Clearly labeled.|
|ELM-RESET-006|Text Label|Success/Error Message Display|عرض رسالة النجاح/الخطأ|Conditional|N/A|Displays the outcome of the password reset attempt.|N/A|View|Prominent display, clear text.|
### Summary Section
This user story for "Reset User Password" provides a comprehensive definition for resetting a user's password. It clearly outlines the strict eligibility criteria (active, registration completed, message sent), the automatic generation of a new password, and the critical step of sending this new password via WhatsApp. The Process Flow, Alternative Flow, Acceptance Criteria, Messages, and Screen Elements have been meticulously detailed to ensure a secure and user-friendly administrative action.

\*\*Important Note:\*\* I am still awaiting your clarification on the "Action Not Applicable (Role/Assignment)" logic for the "Resend Account Registration Message" user story from our previous interaction. The current logic in that story (and reflected in its Acceptance Criteria) is: "Action is NOT applicable if the user has ONLY the 'Board Member' role AND is NOT assigned to any fund board." However, with `IsAssignedToFundBoard` removed from the User entity, this specific condition cannot be implemented as written. \*\*Please confirm how the "Resend Message" eligibility for Board Members should be determined without `IsAssignedToFundBoard`.\*\*

\*\*Key Implementation Considerations:\*\*

\*   \*\*Critical:\*\* Implement the precise logic for hiding/showing the "Reset Password" button based on the combined eligibility criteria.

\*   Ensure the password generation mechanism adheres to strong security standards.

\*   The WhatsApp message containing the new password must be handled with utmost security and reliability.

\*   Consider forcing a password change on the user's first login after a reset.

\*\*Dependencies and Risks:\*\*

\*   \*\*Dependency:\*\* This story depends on the "View System Users List" or "View System User Details" stories for initiating the action. It also depends on the "Add New System User" story for initial user creation and password generation.

\*   \*\*Risk:\*\* Security breach if the new password is compromised during generation, storage, or transmission. Mitigation: Strong encryption, secure API, and audit trails.

\*   \*\*Risk:\*\* User lockout if the new password is not received or is incorrect. Mitigation: Clear instructions, manual fallback.

\---

I have now generated the user story for "Reset User Password" with all the requested details. Please provide clarification on the "Resend Account Registration Message" logic regarding Board Members when you're ready.
## User Story JDWA-1253: Activate/Deactivate System User
### Introduction Section
This user story describes the functionality for System Administrators to change the status of an existing user account between 'Active' and 'Inactive' within the Fund Board Management Application. This action is crucial for controlling user access. \*\*Deactivation is now restricted for Independent Board Members if their removal would drop a fund's independent board member count under 2, for Fund Managers/Associate Fund Managers if they are the sole manager for any assigned fund, and for any other single-holder role if no other active user holds that role.\*\* Activating a user with a "single-holder" role may trigger a replacement workflow. The user receives a WhatsApp message notifying them of their new status and appropriate action. The visibility of the 'Activate' or 'Deactivate' button is strictly controlled, appearing only when the user is eligible for that specific action. The Screen Elements Table provides a detailed breakdown of the UI components involved in this status change.
### Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|Activate/Deactivate System User|Allows System Administrators to change a user's status between active and inactive, with complex deactivation restrictions and activation replacement handling.|
|\*\*User Story\*\*|As a \*\*System Administrator\*\*, I want to \*\*activate or deactivate a system user, managing critical role deactivation restrictions and activation conflicts,\*\* so that I can \*\*control their access to the application and ensure critical roles are always filled.\*\*||
|\*\*Story Points\*\*|11|Involves user selection, complex conditional logic, replacement/prevention workflow, status update, session termination, and external notification.|
|\*\*User Roles\*\*|System Admin|Only System Administrators can perform this action.|
|\*\*Access Requirements\*\*|Authenticated and authorized System Admin access. User must be selected from the user list or details page.|User must be logged in as a System Admin.|
|\*\*Trigger\*\*|System Admin clicks "Activate" or "Deactivate" button/link for a specific user from the user list or user details page.|User initiates the action.|
|\*\*Frequency of Use\*\*|Low to Medium|Used when personnel status changes (e.g., leave of absence, termination, return).|
|\*\*Pre-condition\*\*|System Admin is logged in. The target user account exists. The user's status must be either 'Active' (for deactivation) or 'Inactive' (for activation).|The system is operational, and the user's current status is known.|
|\*\*Business Rules\*\*|1\. The "Deactivate" action is \*\*ONLY applicable\*\* if the user's current status is 'Active'.<br>2. The "Activate" action is \*\*ONLY applicable\*\* if the user's current status is 'Inactive'.<br>3. The "Deactivate" button/link must be hidden if the user is 'Inactive'.<br>4. The "Activate" button/link must be hidden if the user is 'Active'.<br>5. A confirmation dialog must be presented before changing the user's status.<br>6. Upon deactivation, any active sessions for that user should be terminated.<br>7. The 'Last Update Date' must be automatically updated upon successful status change.<br>8. User receives a WhatsApp message notification of their new status and appropriate action.<br>\*\*9. Deactivation Restriction for Independent Board Members:\*\* If the action is 'deactivate' AND the user has a 'Board Member' role \*\*AND\*\* is assigned to one or more funds \*\*as an independent board member\*\*, \*\*AND\*\* for any of these funds, the number of independent board members would drop \*\*under 2\*\* if this user is deactivated, the deactivation is prevented, and an error message is displayed.<br>\*\*10. Deactivation Restriction for Sole Fund Manager/Associate Fund Manager:\*\* If the action is 'deactivate' AND the user has a 'Fund Manager' role \*\*OR\*\* 'Associate Fund Manager' role \*\*AND\*\* is assigned to one or more funds, \*\*AND\*\* for any of those assigned funds, they are the \*only\* Fund Manager \*\*OR\*\* Associate Fund Manager assigned to that specific fund, the deactivation is prevented, and an error message is displayed.<br>\*\*11. Deactivation Restriction for Other Single-Holder Roles:\*\* If the action is 'deactivate' AND the user holds \*only\* a single-holder role (Legal Counsel, Finance Controller, Compliance and Legal Managing Director, Head of Real Estate) AND no other active user holds that same role, the deactivation is prevented, and an error message is displayed, requiring the admin to add another user first.<br>12. \*\*Special Handling for Single-Holder Roles (Legal Counsel, Finance Controller, Compliance and Legal Managing Director, Head of Real Estate - on Activate):\*\* If the user being activated holds \*only\* one of these single-holder roles, the system must check for another \*active\* user currently holding \*only\* that specific role. If found, a confirmation dialog is displayed to replace the existing user. If 'Replace' is chosen, the existing user is deactivated before the current user is activated. If 'Cancel' is chosen, the activation is aborted.|Ensures proper access control, user confirmation, and accurate record keeping, with strict button visibility, complex role-based status change logic, and user notification.|
|\*\*Post-condition\*\*|The user's status is changed to the new state ('Active' or 'Inactive'), the 'Last Update Date' is refreshed, and a WhatsApp notification is sent to the user. If deactivated, any active sessions are terminated. If a replacement occurred during activation, the previous user with that specific role is deactivated.|The system state reflects the updated user status and potentially a deactivated previous role holder, and the user is informed.|
|\*\*Risk\*\*|1\. Accidental deactivation/activation of a critical user.<br>2. User unable to log in due to incorrect activation/deactivation.<br>3. Accidental deactivation of a user during replacement.<br>4. Failure to ensure critical roles are always filled.<br>5. WhatsApp message delivery failure.|Mitigation: Clear confirmation dialogs, thorough testing of complex logic, audit trails, robust error messages, WhatsApp API monitoring.|
|\*\*Assumptions\*\*|1\. The system has a mechanism to terminate active user sessions.<br>2. The user's status is a single boolean or dropdown field in the user entity.<br>3. The roles 'Legal Counsel', 'Finance Controller', 'Compliance and Legal Managing Director', 'Head of Real Estate' are considered "single-holder" roles for active users.<br>4. The system can determine if a 'Board Member' is assigned to one or more funds and count independent board members per fund.<br>5. The system can determine if a 'Fund Manager' or 'Associate Fund Manager' is the \*only\* manager of that type assigned to a specific fund.<br>6. WhatsApp Business API integration is available and configured.|These assumptions are crucial for implementing the functionality.|
|\*\*UX/UI Design Link\*\*|N/A (Refer to general action button/confirmation dialog patterns, new role replacement dialog)|No specific mockups provided in the BRD for this section.|
### Process Flow Table

|**Step**|**Action Description**|**Actor**|**Related Message Codes**|**Notes**|
| :- | :- | :- | :- | :- |
|1|System Admin is viewing the "View System Users List" or "View User Details" page.|System Admin|||
|2|System Admin identifies a user and clicks the "Deactivate" or "Activate" button/link for that user.|System Admin|The button/link is only visible/enabled if the user is eligible for that specific action.||
|3|System displays a confirmation dialog asking the System Admin to confirm the status change.|System|MSG-ACTDEACT-001, MSG-ACTDEACT-002|Dialog message varies based on action (Activate/Deactivate).|
|4|System Admin reviews the confirmation message and clicks "Confirm" or "Cancel".|System Admin|If "Cancel" is clicked, the process stops.||
|5|\*\*If action is "Deactivate":\*\*|System|||
|6|\*\*System checks if the user has a 'Board Member' role AND is assigned to one or more funds as an independent board member AND one or more of these funds' independent board member count would drop under 2 if this user is deactivated.\*\*|System|||
|7|\*\*If yes (Independent Board Member deactivation restricted):\*\* System displays an error message preventing deactivation.|System|MSG-ACTDEACT-009|"You cannot deactivate this Board Member as it would drop the independent member count below 2 for an assigned fund." Deactivation is aborted.|
|8|\*\*System checks if the user has a 'Fund Manager' role OR 'Associate Fund Manager' role AND is assigned to one or more funds, AND for any of those assigned funds, they are the \*only\* Fund Manager OR Associate Fund Manager assigned to that specific fund.\*\*|System|||
|9|\*\*If yes (Fund Manager/Associate Fund Manager deactivation restricted):\*\* System displays an error message preventing deactivation.|System|MSG-ACTDEACT-010|"You cannot deactivate this [Role Name] as they are the only [Role Name] for an assigned fund." Deactivation is aborted.|
|10|\*\*System checks if the user being deactivated holds ONLY a single-holder role (Legal Counsel, Finance Controller, etc.).\*\*|System|This is the newly added check.||
|11|\*\*If yes (single-holder role) AND no other active user holds that same role:\*\* System displays an error message preventing deactivation.|System|MSG-ACTDEACT-007|"You can't deactivate this user [username] with role [role], you should add another user with the same role first." Deactivation is aborted.|
|12|\*\*If deactivation is allowed (no restrictions met):\*\* System changes the user's status to 'Inactive' in the database.|System|The 'Last Update Date' is automatically refreshed.||
|13|System terminates any active sessions for the user.|System|||
|14|\*\*If action is "Activate":\*\*|System|||
|15|\*\*System checks if the user being activated holds ONLY a single-holder role (Legal Counsel, Finance Controller, etc.).\*\*|System|||
|16|\*\*If yes (single-holder role) AND an active user already holds ONLY that specific role (who is NOT the user being activated):\*\* System displays a confirmation dialog asking to replace the existing user.|System|MSG-ACTDEACT-008|Dialog includes existing user's name and role. Buttons: "Replace", "Cancel".|
|17|\*\*If System Admin clicks "Replace" in the dialog:\*\* System deactivates the existing user with that role.|System Admin / System|The existing user's status is changed to 'Inactive'.||
|18|\*\*If System Admin clicks "Cancel" in the dialog:\*\* System aborts the activation process and returns to the previous page.|System Admin / System|Activation is aborted.||
|19|\*\*If activation is allowed (no conflict, or after replacement):\*\* System changes the user's status to 'Active' in the database.|System|The 'Last Update Date' is automatically refreshed.||
|20|System sends a WhatsApp message to the user notifying them of their new status and appropriate action.|System|MSG-ACTDEACT-009, MSG-ACTDEACT-010, MSG-ACTDEACT-011|Message content varies based on new status (Activated/Deactivated) and includes login link if activated.|
|21|System displays a success message to the System Admin.|System|MSG-ACTDEACT-003, MSG-ACTDEACT-004|Message varies based on action (Activated/Deactivated).|
|22|System updates the user list/details page to reflect the new status.|System|||
|23|System Admin can continue managing users.|System Admin|||
### Alternative Flow Table

|**Alternative Scenario**|**Condition**|**Action**|**Related Message Codes**|**Resolution**|
| :- | :- | :- | :- | :- |
|\*\*Action Not Applicable (Button Hidden)\*\*|System Admin attempts to click "Activate" for an 'Active' user, or "Deactivate" for an 'Inactive' user.|The respective "Activate" or "Deactivate" button/link is \*\*hidden\*\*, preventing the action.|System Admin cannot perform the action.||
|\*\*Deactivate - Independent Board Member Restriction\*\*|System Admin attempts to deactivate an Independent Board Member who is assigned to funds, AND their deactivation would drop an assigned fund's independent board member count under 2.|System displays an error message preventing deactivation.|MSG-ACTDEACT-009|System Admin must ensure another independent board member is added to the affected fund(s) first.|
|\*\*Deactivate - Sole Fund Manager/Associate Fund Manager Restriction\*\*|System Admin attempts to deactivate a Fund Manager or Associate Fund Manager who is the \*only\* manager of that type for an assigned fund.|System displays an error message preventing deactivation.|MSG-ACTDEACT-010|System Admin must add another Fund Manager/Associate Fund Manager to the affected fund(s) first.|
|\*\*Deactivate - Other Single-Holder Role Restriction\*\*|System Admin attempts to deactivate a user who holds ONLY a single-holder role (Legal Counsel, Finance Controller, etc.), AND no other active user holds that same role.|System displays an error message preventing deactivation.|MSG-ACTDEACT-007|System Admin must first add another user with that role and activate them.|
|\*\*Activate - Role Replacement - Cancelled\*\*|System Admin attempts to activate a user with a single-holder role conflict, and clicks "Cancel" in the replacement confirmation dialog.|System closes the dialog and aborts the activation process.|Activation is aborted.||
|\*\*Confirmation Canceled (General)\*\*|System Admin clicks "Deactivate" or "Activate" but then clicks "Cancel" in the initial confirmation dialog.|System closes the confirmation dialog and no status change occurs.|No action performed.||
|\*\*System Error during Status Change\*\*|A backend error occurs during the status update.|System displays a generic error message.|MSG-ACTDEACT-005|System Admin can retry later or contact support.|
|\*\*User Not Found\*\*|The selected user's ID is invalid or the user has been deleted.|System displays an error message (e.g., "User not found").|MSG-ACTDEACT-006|System Admin can return to the user list.|
|\*\*WhatsApp Message Failure\*\*|The system fails to send the WhatsApp notification message to the user.|System logs the failure.|MSG-ACTDEACT-011|System Admin may need to manually notify the user or verify mobile number. \*\*Note: This failure does NOT prevent the status change.\*\*|
### Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Deactivate User - Success (Non-Restricted)\*\*|A System Admin is logged in. An 'Active' user (e.g., Accountant) exists.|The System Admin clicks "Deactivate" for the user and clicks "Confirm" in the dialog.|The user's status is changed to 'Inactive', any active sessions are terminated, a success message "User [User Name] has been deactivated successfully." is displayed, a WhatsApp message is sent to the user notifying them of deactivation, and the user list/details page reflects the new status.|
|\*\*Deactivate User - Prevention (Independent Board Member Restriction)\*\*|A System Admin is logged in. An 'Active' user "Alice" is an \*\*Independent Board Member\*\* assigned to Fund X, and Fund X has only 2 independent board members (including Alice).|The System Admin clicks "Deactivate" for "Alice" and clicks "Confirm" in the dialog.|The system displays an error message "You cannot deactivate this Board Member as it would drop the independent member count below 2 for an assigned fund.", and "Alice"'s status remains 'Active'. No WhatsApp message is sent.|
|\*\*Deactivate User - Prevention (Sole Fund Manager/Associate Fund Manager)\*\*|A System Admin is logged in. An 'Active' user "Bob" is a Fund Manager assigned to Fund Y, and "Bob" is the \*only\* Fund Manager assigned to Fund Y.|The System Admin clicks "Deactivate" for "Bob" and clicks "Confirm" in the dialog.|The system displays an error message "You cannot deactivate this [Fund Manager] as they are the only [Fund Manager] for an assigned fund.", and "Bob"'s status remains 'Active'. No WhatsApp message is sent.|
|\*\*Deactivate User - Prevention (Other Single-Holder Role)\*\*|A System Admin is logged in. An 'Active' user "Charlie" holds ONLY the "Legal Counsel" role, and no other active user holds that role.|The System Admin clicks "Deactivate" for "Charlie" and clicks "Confirm" in the dialog.|The system displays an error message "You can't deactivate this user [Charlie] with role [Legal Counsel], you should add another user with the same role first.", and "Charlie"'s status remains 'Active'. No WhatsApp message is sent.|
|\*\*Activate User - Success (No Conflict)\*\*|A System Admin is logged in. An 'Inactive' user (e.g., Accountant) exists.|The System Admin clicks "Activate" for the user and clicks "Confirm" in the dialog.|The user's status is changed to 'Active', a success message "User [User Name] has been activated successfully." is displayed, a WhatsApp message is sent to the user notifying them of activation and login, and the user list/details page reflects the new status.|
|\*\*Activate User - Success (Single-Holder Role, With Replacement)\*\*|A System Admin is logged in. An 'Inactive' user "Jane Doe" holds ONLY the "Legal Counsel" role. An 'Active' user "John Smith" currently holds ONLY the "Legal Counsel" role.|The System Admin clicks "Activate" for "Jane Doe", clicks "Confirm" in the initial dialog, then clicks "Replace" in the role replacement dialog.|The existing user "John Smith" is deactivated, "Jane Doe"'s status is changed to 'Active', a success message "User [Jane Doe] has been activated successfully." is displayed, a WhatsApp message is sent to "Jane Doe" notifying them of activation and login, and the user list/details page reflects both status changes.|
|\*\*Activate User - Role Replacement - Cancelled\*\*|A System Admin is logged in. An 'Inactive' user "Jane Doe" holds ONLY the "Legal Counsel" role. An 'Active' user "John Smith" currently holds ONLY the "Legal Counsel" role.|The System Admin clicks "Activate" for "Jane Doe", clicks "Confirm" in the initial dialog, then clicks "Cancel" in the role replacement dialog.|The role replacement dialog closes, "Jane Doe"'s status remains 'Inactive', "John Smith"'s status remains 'Active', and the activation process is aborted. No WhatsApp message is sent.|
|\*\*Deactivate User - Button Hidden (Already Inactive)\*\*|A System Admin is logged in. An 'Inactive' user exists.|The System Admin views the user list/details page for this user.|The "Deactivate" button/link is \*\*hidden\*\*.|
|\*\*Activate User - Button Hidden (Already Active)\*\*|A System Admin is logged in. An 'Active' user exists.|The System Admin views the user list/details page for this user.|The "Activate" button/link is \*\*hidden\*\*.|
|\*\*Status Change - Confirmation Canceled (General)\*\*|A System Admin clicks "Deactivate" for a user.|The System Admin clicks "Cancel" in the initial confirmation dialog.|The confirmation dialog closes, and the user's status remains unchanged.|
|\*\*WhatsApp Message Failure (Status Change Success)\*\*|A System Admin successfully activates a user.|The system attempts to send the WhatsApp notification but it fails.|The user's status is successfully changed, a success message is displayed to the admin, but no WhatsApp message is received by the user. The failure is logged.|
### Data Entities Table
Entity Name: User (for activate/deactivate)

|**Attribute (English)**|**Attribute (Arabic)**|**Mandatory/Optional**|**Attribute Type**|**Data Length**|**Integration Requirements**|**Default Value**|**Condition (if needed)**|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|User ID|معرف المستخدم|Mandatory|Number|N/A|Database Primary Key|N/A|N/A|Unique identifier|12345|12345|
|Name|الاسم|Mandatory|Text|Max 255|Database Field|N/A|N/A|N/A|محمد أحمد|Mohammed Ahmed|
|Status|الحالة|Mandatory|Boolean/Dropdown|N/A|Database Field|Active|N/A|Active/Inactive|نشط|Active|
|Last Update Date|تاريخ آخر تحديث|Mandatory|DateTime|N/A|Database Field|Current Timestamp|N/A|Automatically set on creation and update.|2023-10-26 15:00:00|2023-10-26 15:00:00|
|Role|الدور|Mandatory|Relation to Role Entity|N/A|Database Many-to-Many|N/A|At least one role must be selected, conditional logic applies|Predefined roles only|مدير صندوق، عضو مجلس|Fund Manager, Board Member|
|Mobile|رقم الجوال|Mandatory|Text|Max 10|Database Field|N/A|N/A|Saudi mobile format (e.g., 05XXXXXXXX), Numeric only, Unique (as username).|0501234567|0501234567|
|IsAssignedToFund|هل تم تعيينه لصندوق|Derived/Boolean|N/A|N/A|Derived from FundBoardMember assignments.|False|N/A|True if user is linked to any active fund board.|1|True|
|IsSoleFundManagerOfAnyFund|هل هو المدير الوحيد لأي صندوق|Derived/Boolean|N/A|N/A|True if user is a Fund Manager and is the only Fund Manager for any assigned fund.|False|N/A|Derived from Fund assignments.|1|True|
|IsSoleAssociateFundManagerOfAnyFund|هل هو المدير المساعد الوحيد لأي صندوق|Derived/Boolean|N/A|N/A|True if user is an Associate Fund Manager and is the only Associate Fund Manager for any assigned fund.|False|N/A|Derived from Fund assignments.|1|True|
|AssignedFundsIndependentBoardMemberCount|عدد أعضاء المجلس المستقلين للصناديق المعينة|Derived|Number|N/A|Derived from FundBoardMember assignments.|N/A|N/A|For each fund assigned to this user, count of active independent board members.|2|2|
### Messages/Notifications Table

|**Message Code**|**Message (English)**|**Message (Arabic)**|**Message Type**|**Communication Method**|
| :- | :- | :- | :- | :- |
|MSG-ACTDEACT-001|Are you sure you want to deactivate [User Name]? This user will no longer be able to log in.|هل أنت متأكد من إلغاء تفعيل [اسم المستخدم]؟ لن يتمكن هذا المستخدم من تسجيل الدخول بعد الآن.|Confirmation Dialog|In-App|
|MSG-ACTDEACT-002|Are you sure you want to activate [User Name]? This user will be able to log in.|هل أنت متأكد من تفعيل [اسم المستخدم]؟ سيتمكن هذا المستخدم من تسجيل الدخول.|Confirmation Dialog|In-App|
|MSG-ACTDEACT-003|User [User Name] has been deactivated successfully.|تم إلغاء تفعيل المستخدم [اسم المستخدم] بنجاح.|Success Message|In-App|
|MSG-ACTDEACT-004|User [User Name] has been activated successfully.|تم تفعيل المستخدم [اسم المستخدم] بنجاح.|Success Message|In-App|
|MSG-ACTDEACT-005|An error is occurred while changing user status.|حدث خطأ بالنظام , لم يتم تغيير حالة المستخدم.|Error Message|In-App|
|MSG-ACTDEACT-006|User not found.|لم يتم العثور على المستخدم.|Error Message|In-App|
|MSG-ACTDEACT-007|You can't deactivate this user [User Name] with role [Role Name], you should add another user with the same role first.|لا يمكنك إلغاء تفعيل المستخدم [اسم المستخدم] بالدور [اسم الدور]. يجب إضافة مستخدم آخر بنفس الدور أولاً.|Error Message|In-App|
|MSG-ACTDEACT-008|There is another active user [Existing User Name] with the same role [Role Name]. Do you want to replace him?|يوجد مستخدم نشط آخر [اسم المستخدم الحالي] بنفس الدور [اسم الدور]. هل تريد استبداله؟|Confirmation Dialog|In-App|
|MSG-ACTDEACT-009|Your account for Jadwa Fund Board Management has been activated. You can now log in using your registered mobile number.|تم تفعيل حسابك في تطبيق إدارة مجالس صناديق جدوى. يمكنك الآن تسجيل الدخول باستخدام رقم جوالك المسجل.|Success Message|WhatsApp|
|MSG-ACTDEACT-010|Your account for Jadwa Fund Board Management has been deactivated. You will no longer be able to log in.|تم إلغاء تفعيل حسابك في تطبيق إدارة مجالس صناديق جدوى. لن تتمكن من تسجيل الدخول بعد الآن.|Success Message|WhatsApp|
|MSG-ACTDEACT-011|Failed to send WhatsApp notification for status change. Please notify the user manually.|فشل إرسال إشعار الواتساب لتغيير الحالة. يرجى إخطار المستخدم يدوياً.|Warning Message|In-App / System Log|
|MSG-ACTDEACT-012|You cannot deactivate this [Role Name] as they are the only [Role Name] for an assigned fund.|لا يمكنك إلغاء تفعيل هذا الدور "[اسم الدور]" حيث أنه المدير الوحيد للصندوق المعين.|Error Message|In-App|
|\*\*MSG-ACTDEACT-013\*\*|\*\*You cannot deactivate this Board Member as it would drop the independent member count below 2 for an assigned fund.\*\*|\*\*لا يمكنك إلغاء تفعيل عضو مجلس الإدارة هذا لأنه سيؤدي إلى انخفاض عدد الأعضاء المستقلين إلى أقل من 2 في أحد الصناديق المعينة.\*\*|\*\*Error Message\*\*|\*\*In-App\*\*|
### Screen Elements Table

|**Element ID**|**Element Type**|**Element Name (English)**|**Element Name (Arabic)**|**Required/Optional**|**Validation Rules**|**Business Logic**|**Related Data Entity**|**User Interaction**|**Accessibility Notes**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|ELM-ACTDEACT-001|Button|Deactivate User|إلغاء تفعيل المستخدم|Conditional|N/A|Initiates deactivation process. \*\*Hidden if user is Inactive or if deactivation is prevented by single-holder role rule.\*\*|N/A|Click|Clear label, visible/enabled based on conditions.|
|ELM-ACTDEACT-002|Button|Activate User |تفعيل المستخدم|Conditional|N/A|Initiates activation process. \*\*Hidden if user is Active.\*\*|N/A|Click|Clear label, visible/enabled based on conditions.|
|ELM-ACTDEACT-003|Confirmation Dialog|Status Change Confirmation Dialog|نافذة تأكيد تغيير الحالة|Mandatory|N/A|Prompts System Admin to confirm status change.|N/A|View, Click (Confirm/Cancel)|Clear message, accessible buttons.|
|ELM-ACTDEACT-004|Text Label|Confirmation Message Text|نص رسالة التأكيد|Mandatory|N/A|Displays "Are you sure you want to [deactivate/activate] [User Name]?"|N/A|View|Dynamic content for action and user name.|
|ELM-ACTDEACT-005|Button|Confirm (in Dialog)|تأكيد (في النافذة)|Mandatory|N/A|Confirms the status change action.|N/A|Click|Clearly labeled.|
|ELM-ACTDEACT-006|Button|Cancel (in Dialog)|إلغاء (في النافذة)|Mandatory|N/A|Cancels the status change action.|N/A|Click|Clearly labeled.|
|ELM-ACTDEACT-007|Text Label|Success/Error Message Display|عرض رسالة النجاح/الخطأ|Conditional|N/A|Displays the outcome of the status change attempt.|N/A|View|Prominent display, clear text.|
|ELM-ACTDEACT-008|Confirmation Dialog|Role Replacement Confirmation (Activate)|تأكيد استبدال الدور (تفعيل)|Conditional|N/A|Prompts System Admin to confirm replacing an existing user with a single-holder role during activation.|N/A|View, Click (Replace/Cancel)|Clear message, accessible buttons.|
|ELM-ACTDEACT-009|Button|Replace (in Dialog)|استبدال (في النافذة)|Mandatory|N/A|Confirms replacement of existing user and proceeds with current user's activation.|N/A|Click|Clearly labeled, primary action in dialog.|
|ELM-ACTDEACT-010|Button|Cancel (in Dialog)|إلغاء (في النافذة)|Mandatory|N/A|Cancels the replacement and aborts activation.|N/A|Click|Clearly labeled.|
### Summary Section
This rewritten user story for "Activate/Deactivate System User" now precisely implements the complex deactivation restriction for \*\*Independent Board Members\*\*: deactivation is prevented if it would drop an assigned fund's independent board member count \*\*under 2\*\*. This rule is integrated into the Business Rules, Process Flow, Alternative Flow, and Acceptance Criteria, along with a new error message (`MSG-ACTDEACT-013`). This ensures robust control over user status changes for critical roles.

\*\*Important Note:\*\* I am still awaiting your clarification on the "Action Not Applicable (Role/Assignment)" logic for the "Resend Account Registration Message" user story from our previous interaction. The current logic in that story (and reflected in its Acceptance Criteria) is: "Action is NOT applicable if the user has ONLY the 'Board Member' role AND is NOT assigned to any fund board." However, with `IsAssignedToFundBoard` removed from the User entity, this specific condition cannot be implemented as written. \*\*Please confirm how the "Resend Message" eligibility for Board Members should be determined without `IsAssignedToFundBoard`.\*\*

\*\*Key Implementation Considerations:\*\*

\*   \*\*Critical:\*\* Implement the precise logic for checking Board Member independent count (per fund) and Sole Fund Manager/Associate Fund Manager status (per fund) during deactivation. This will require complex queries.

\*   Ensure the WhatsApp notification is sent reliably and contains the correct dynamic content based on the status change.

\*   Handle WhatsApp API failures gracefully for these notifications.

\*\*Dependencies and Risks:\*\*

\*   \*\*Dependency:\*\* This story depends on the "View System Users List" or "View System User Details" stories for initiating the action. It also impacts the user's ability to log in (Login story) and relies on WhatsApp Business API integration. It has new dependencies on fund assignment data and role counts per fund.

\*   \*\*Risk:\*\* Accidental deactivation/activation or incorrect role assignment due to complex logic. Mitigation: Thorough testing, clear UI feedback.

\*   \*\*Risk:\*\* Failure to ensure critical roles are always filled if the prevention logic is flawed.

\---
## User Story JDWA-1251: Edit Existing System User
### Introduction Section
This user story describes the functionality for System Administrators to modify the details and roles of an existing user account within the Fund Board Management Application. This ensures that user information remains accurate and their permissions are up-to-date. The user's Country Code and Mobile Number are displayed but not editable from this screen. A new validation is introduced for specific roles (Legal Counsel, Finance Controller, Compliance and Legal Managing Director, Head of Real Estate) to check for existing active users with the same selected role, prompting for a replacement if found. Crucially, changing the role of a Board Member who is assigned to a fund is restricted. Additionally, if a user with only the 'Board Member' role is assigned a 'Fund Manager' or 'Associate Fund Manager' role, and their registration message hasn't been sent, the system will send it via WhatsApp. \*\*New notifications are sent to users if their 'Fund Manager' or 'Associate Fund Manager' role is removed, or if their role changes to/from a single-holder role.\*\* The Screen Elements Table provides a detailed breakdown of the UI components on the user editing form.
### Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|Edit Existing System User|Allows System Administrators to modify details and roles of existing user accounts, with complex role change validations and conditional WhatsApp sends.|
|\*\*User Story\*\*|As a \*\*System Administrator\*\*, I want to \*\*edit an existing user's details and roles, handling potential role replacements and conditional registration messages,\*\* so that I can \*\*keep user information accurate and manage their system access.\*\*||
|\*\*Story Points\*\*|16|Involves retrieving data, complex modification, multiple layers of role validation, replacement workflow, conditional WhatsApp send, and updating.|
|\*\*User Roles\*\*|System Admin|Only System Administrators can edit existing users.|
|\*\*Access Requirements\*\*|Authenticated and authorized System Admin access to the User Management module. User must be selected from the user list or details page.|User must be logged in as a System Admin.|
|\*\*Trigger\*\*|System Admin clicks "Edit" button/link for a specific user from the user list or user details page.|User initiates the action.|
|\*\*Frequency of Use\*\*|Medium|User data changes periodically due to role changes, contact updates, or status changes.|
|\*\*Pre-condition\*\*|System Admin is logged in, has access to the User Management module, and the user to be edited exists.|The system is operational, and the selected user's data is available.|
|\*\*Business Rules\*\*|1\. All mandatory fields must be filled.<br>2. Email address can only be changed if it remains unique.<br>3. Country Code and Mobile No. are displayed as labels and cannot be modified from this screen.<br>4. At least one role must be assigned to a user.<br>5. \*\*Role Selection Logic:\*\* Multi-select for roles is enabled \*\*ONLY IF\*\* the selected roles are ('Fund Manager' AND 'Board Member') OR ('Associate Fund Manager' AND 'Board Member'). Otherwise, multi-select is disabled, restricting to a single role.<br>6. CV and Passport No. are optional fields.<br>7. The 'Last Update Date' must be automatically updated upon successful modification.<br>8. The 'Registration Message Is Sent' flag and 'Registration Is Completed' flag are displayed but not directly editable by the System Admin in this view.<br>9. User Status is displayed as a label and cannot be modified from this screen.<br>10. \*\*Role Change Restriction for Assigned Board Members:\*\* If the user has a 'Board Member' role \*\*AND\*\* is assigned to one or more funds, \*\*AND\*\* the 'Board Member' role is being \*\*removed\*\* during the edit, their role(s) \*\*cannot be changed\*\* from this screen. An error message must be displayed.<br>11. \*\*Role Change Restriction for Sole Fund Manager/Associate Fund Manager:\*\* If the user has a 'Fund Manager' role \*\*OR\*\* 'Associate Fund Manager' role \*\*AND\*\* is assigned to one or more funds, \*\*AND\*\* for any of those assigned funds, they are the \*only\* Fund Manager \*\*OR\*\* Associate Fund Manager assigned to that specific fund, \*\*AND\*\* roles are being changed (specifically, the Fund Manager/Associate Fund Manager role is being removed or changed), then their role(s) \*\*cannot be changed\*\* from this screen. An error message must be displayed.<br>12. \*\*Unique Role Validation with Replacement (on Save):\*\* If a selected role is one of: 'Legal Counsel', 'Finance Controller', 'Compliance and Legal Managing Director', 'Head of Real Estate', the system must check for another \*active\* user (who is \*not\* the user currently being edited) currently holding \*only\* that specific role. If found, a confirmation dialog is displayed to replace the existing user. If 'Replace' is chosen, the existing user is deactivated before the current user's roles are updated. If 'Cancel' is chosen, the System Admin is returned to the form to modify role selection or other details.<br>13. \*\*Conditional WhatsApp Registration Message (on Role Change):\*\* If the user being edited has \*\*ONLY\*\* the 'Board Member' role, and a new role (either 'Fund Manager' or 'Associate Fund Manager') is assigned to them, \*\*AND\*\* their 'Registration Message Is Sent' flag is 0, the system will attempt to send the WhatsApp registration message (content from MSG-ADD-008) and update their 'Registration Message Is Sent' flag to 1 upon this attempt.<br>14. \*\*Relieve of Duties Notification:\*\* If the user's 'Fund Manager' role OR 'Associate Fund Manager' role was present before the edit and is now being removed, the system sends an in-app notification to the user: "تم اعفاءك من مهامك ك [اسم الدور] و ذلك على كافة الصناديق المنضم اليها " with title "إعفاء من مهام/ relieve of duties ".<br>\*\*15. Role Update Notification:\*\* If the user's roles are changed to/from a single-holder role (Legal Counsel, Finance Controller, Compliance and Legal Managing Director, Head of Real Estate), OR if a user with a single-holder role is assigned a new single-holder role, the system sends an in-app notification to the user: "تم اعفاءك من مهامك ك [اسم الدور] و قد تم تعيينك ك [اسم الدور]" with title "تعديل مهام/ Update duties ".|Ensures data integrity, proper access control, and accurate record keeping during user modification, with critical role change and replacement workflows, and conditional WhatsApp onboarding.|
|\*\*Post-condition\*\*|The user account is updated with the modified details and roles, and the 'Last Update Date' is refreshed. If a replacement occurred, the previous user with that specific role is deactivated. If applicable, the WhatsApp registration message is attempted to be sent and the 'Registration Message Is Sent' flag is updated. If 'Fund Manager' or 'Associate Fund Manager' role was removed, a notification is sent to the user. If roles are changed to/from a single-holder role, a notification is sent to the user.|The system state reflects the successful update and/or replacement of a user, and conditional onboarding/notification.|
|\*\*Risk\*\*|1\. Accidental modification of critical user data.<br>2. Incorrect role assignment leading to unintended access changes.<br>3. Data validation errors.<br>4. Accidental deactivation of a user during replacement.<br>5. WhatsApp message delivery failure.<br>6. Inability to change roles due to complex restrictions.|Mitigation: Confirmation prompts, robust input validation, clear UI, audit trails, careful implementation of complex logic, WhatsApp API monitoring.|
|\*\*Assumptions\*\*|1\. Core Identity in ASP.NET is used for user and permission management.<br>2. System Admin understands the implications of status changes (e.g., deactivation/activation).<br>3. The roles 'Legal Counsel', 'Finance Controller', 'Compliance and Legal Managing Director', 'Head of Real Estate' are considered "single-holder" roles for active users.<br>4. The system can determine if a 'Board Member' is assigned to one or more funds (e.g., by checking `FundBoardMember` relationships).<br>5. The system can determine if a 'Fund Manager' or 'Associate Fund Manager' is the \*only\* manager of that type assigned to a specific fund.<br>6. WhatsApp Business API integration is available and configured.|These assumptions are crucial for implementing the functionality.|
|\*\*UX/UI Design Link\*\*|N/A (Refer to general form design principles, confirmation dialogs)|No specific mockups provided in the BRD for this section.|
### Process Flow Table

|**Step**|**Action Description**|**Actor**|**Related Message Codes**|**Notes**|
| :- | :- | :- | :- | :- |
|1|System Admin is viewing the "View System Users List" or "View User Details" page.|System Admin|||
|2|System Admin identifies a user and clicks the "Edit" button/link for that user.|System Admin|||
|3|System retrieves the selected user's current details and pre-populates an editable form.|System|All fields are pre-filled with existing data. Role selection dynamically adjusts based on current roles. User Status, Country Code, and Mobile No. are displayed as non-editable labels.||
|4|User modifies editable fields (e.g., Name, Email, IBAN, Nationality, uploads new CV, Passport No.).|User|MSG-EDIT-001, MSG-EDIT-002, MSG-EDIT-003, MSG-EDIT-008||
|5|System Admin modifies user roles (add or remove roles) using the dynamic role selection control.|System Admin|MSG-EDIT-004|\*\*Conditional logic:\*\* Multi-select for roles is enabled \*\*ONLY IF\*\* the selected roles are ('Fund Manager' AND 'Board Member') OR ('Associate Fund Manager' AND 'Board Member'). Otherwise, multi-select is disabled, restricting to a single role.|
|6|System Admin reviews the modified information and clicks the "Save Changes" button.|System Admin|||
|7|System performs server-side validation on all submitted data.|System|MSG-EDIT-001, MSG-EDIT-002, MSG-EDIT-003, MSG-EDIT-004, MSG-EDIT-008|Checks for mandatory fields, email format/uniqueness, role selection based on business rules, CV file type/size.|
|8|\*\*If the user has a 'Board Member' role AND is assigned to one or more funds AND the 'Board Member' role is being removed:\*\* System displays an error message preventing the role change.|System|MSG-EDIT-011|\*\*Role change is aborted.\*\*|
|9|\*\*If the user has a 'Fund Manager' role OR 'Associate Fund Manager' role AND is assigned to one or more funds, AND for any of those assigned funds, they are the \*only\* Fund Manager OR Associate Fund Manager assigned to that specific fund, AND roles are being changed (specifically, the Fund Manager/Associate Fund Manager role is being removed or changed):\*\* System displays an error message preventing the role change.|System|MSG-EDIT-012|\*\*Role change is aborted.\*\*|
|10|\*\*If validation is successful (and no Board Member/Fund Manager/Associate Fund Manager role change restriction): System checks if any selected role is a "single-holder" role (Legal Counsel, Finance Controller, Compliance and Legal Managing Director, Head of Real Estate) AND if this role is different from the user's current single-holder role (if any).\*\*|System|This check excludes self-assignment.||
|11|\*\*If a single-holder role is selected AND an active user (who is NOT the user currently being edited) already holds ONLY that specific role:\*\* System displays a confirmation dialog asking to replace the existing user.|System|MSG-EDIT-010|Dialog includes existing user's name and role. Buttons: "Replace", "Cancel".|
|12|\*\*If System Admin clicks "Replace" in the dialog:\*\* System deactivates the existing user with that role.|System Admin / System|The existing user's status is changed to 'Inactive'.||
|13|\*\*If System Admin clicks "Cancel" in the dialog:\*\* System returns the System Admin to the user editing form, allowing them to modify role selection or other details.|System Admin / System|This allows the admin to choose a non-conflicting role or adjust other inputs.||
|14|\*\*If System Admin proceeds (either no conflict, or after cancelling replacement and modifying input):\*\* System updates the user account in the database with the modified details and roles.|System|The 'Last Update Date' is automatically refreshed.||
|15|\*\*Conditional WhatsApp Registration Message (on Role Change):\*\* If the user being edited has \*\*ONLY\*\* the 'Board Member' role, and a new role (either 'Fund Manager' or 'Associate Fund Manager') is assigned to them, \*\*AND\*\* their 'Registration Message Is Sent' flag is 0, System \*\*attempts to send the WhatsApp registration message\*\* (content from MSG-ADD-008) to the user.|System|MSG-ADD-008, MSG-ADD-009|Requires WhatsApp Business API call. Upon this attempt, the user's 'Registration Message Is Sent' flag is updated to 1.|
|16|\*\*Conditional Relieve of Duties Notification:\*\* If the user's 'Fund Manager' role OR 'Associate Fund Manager' role was present before the edit and is now being removed, System sends an in-app notification to the user.|System|MSG-EDIT-013|Notification activity "general", notification title "إعفاء من مهام/ relieve of duties ".|
|17|\*\*Conditional Role Update Notification:\*\* If the user's roles are changed to one of the single-holder roles (Legal Counsel, Finance Controller, Compliance and Legal Managing Director, Head of Real Estate) OR if a user with a single-holder role is assigned a new single-holder role, System sends an in-app notification to the user.|System|MSG-EDIT-014|Notification activity "general", notification title "تعديل مهام/ Update duties ".|
|18|System displays a success message.|System|MSG-EDIT-007||
|19|System redirects the System Admin back to the "View System Users List" or "View User Details" page, with the updated user information visible.|System|||
### Alternative Flow Table

|**Alternative Scenario**|**Condition**|**Action**|**Related Message Codes**|**Resolution**|
| :- | :- | :- | :- | :- |
|\*\*Missing Mandatory Field\*\*|System Admin attempts to save without filling a mandatory field (e.g., Name).|System displays a specific validation error message "Required Field." next to the missing field.|MSG-EDIT-001|User must fill in all mandatory fields.|
|\*\*Invalid Email Format\*\*|System Admin enters an email address in an incorrect format.|System displays a validation error message "Invalid email format." for the email field.|MSG-EDIT-002|User must correct the email format.|
|\*\*Duplicate Email\*\*|User enters an email address that already exists for another user.|System displays an error message "User with this email already exists." indicating duplicate email.|MSG-EDIT-003|User must enter a unique email address.|
|\*\*Invalid Role Selection (Logic Violation)\*\*|System Admin selects roles that violate the conditional logic (e.g., 'Fund Manager' and 'Accountant' simultaneously, or more than one role when multi-select is disabled).|System displays a warning/error message "Invalid role selection."|MSG-EDIT-004|System Admin must correct the role selection.|
|\*\*No Roles Remaining\*\*|System Admin attempts to remove all roles from a user.|System displays a warning/error message "Invalid role selection." prompting to select at least one role.|MSG-EDIT-004|System Admin must ensure at least one role is assigned.|
|\*\*CV File Upload Error\*\*|An error occurs during the CV file upload (e.g., file too large, unsupported format).|System displays an error message "Invalid file format or size for CV. Please upload a PDF or DOCX file up to 10MB." related to the file upload.|MSG-EDIT-008|System Admin must re-upload a valid CV file.|
|\*\*Role Change Restricted (Assigned Board Member - Role Removed)\*\*|The user being edited has a 'Board Member' role AND is assigned to one or more funds, AND the 'Board Member' role is being removed.|System displays an error message "You cannot change the role of this Board Member as they are assigned to a fund. Please unassign them first."|MSG-EDIT-011|System Admin must unassign the board member from all funds before changing their role.|
|\*\*Role Change Restricted (Sole Fund Manager/Associate Fund Manager)\*\*|The user being edited has a 'Fund Manager' role OR 'Associate Fund Manager' role AND is assigned to one or more funds, AND for any of those assigned funds, they are the \*only\* Fund Manager OR Associate Fund Manager assigned to that specific fund, AND roles are being changed.|System displays an error message "You cannot change the role [Role Name] as they are assigned to one of the investment funds."|MSG-EDIT-012|System Admin must add another Fund Manager/Associate Fund Manager to the affected fund(s) first.|
|\*\*Role Replacement - Cancelled\*\*|System Admin selects a single-holder role that has an active user, and clicks "Cancel" in the replacement confirmation dialog.|System closes the dialog and returns the System Admin to the user editing form.|System Admin can modify input and re-attempt save.||
|\*\*WhatsApp Message Failure (on Role Change)\*\*|A user with ONLY 'Board Member' role is assigned 'Fund Manager' or 'Associate Fund Manager', and their 'Registration Message Is Sent' flag is 0, but the WhatsApp message fails to send.|System logs the failure and displays a warning to the System Admin. \*\*The 'Registration Message Is Sent' flag is still updated to 1 (due to the attempt).\*\*|MSG-ADD-009|System Admin may need to manually notify the user or verify mobile number.|
|\*\*System Error during Update\*\*|A backend error occurs during user update (e.g., database connection issue).|System displays a generic error message "An error is occurred while saving data."|MSG-EDIT-009|System Admin can retry the operation or contact support.|
|\*\*Cancel Edit\*\*|System Admin decides not to save changes.|System Admin clicks a "Cancel" button.|System discards all unsaved changes and redirects back to the previous page.||
### Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Successful User Update (No Role Conflict)\*\*|A System Admin is logged in and on the "Edit User" form for an existing user.|The System Admin modifies the user's Name, updates their Email to a new valid unique email, and adds a valid new role (e.g., "Accountant") that does not conflict, then clicks "Save Changes".|The user's details are updated in the system, the 'Last Update Date' is refreshed, a success message "Record Updated Successfully" is displayed, and the System Admin is redirected to the user list or details page.|
|\*\*Successful User Update (Single-Holder Role, No Existing User)\*\*|A System Admin is logged in and on the "Edit User" form for an existing user.|The System Admin changes the user's role to "Legal Counsel" (a single-holder role) where no other active user currently holds that role, and clicks "Save Changes".|The user's role is updated, a success message "Record Updated Successfully" is displayed, and the System Admin is redirected.|
|\*\*Successful User Update (Single-Holder Role, With Replacement)\*\*|A System Admin is logged in and on the "Edit User" form for user "Jane Doe". An active user "John Smith" currently holds ONLY the "Legal Counsel" role.|The System Admin changes "Jane Doe"'s role to "Legal Counsel", clicks "Save Changes", and then clicks "Replace" in the confirmation dialog.|The existing user "John Smith" is deactivated, "Jane Doe"'s role is updated to "Legal Counsel", a success message "Record Updated Successfully" is displayed, and the System Admin is redirected.|
|\*\*Role Replacement - Cancelled (Admin Modifies Input)\*\*|A System Admin is on the "Edit User" form for user "Jane Doe". An active user "John Smith" currently holds ONLY the "Legal Counsel" role.|The System Admin changes "Jane Doe"'s role to "Legal Counsel", clicks "Save Changes", then clicks "Cancel" in the confirmation dialog, then changes "Jane Doe"'s role to "Accountant" and clicks "Save Changes" again.|The user "Jane Doe"'s role is updated to "Accountant", the existing user "John Smith" remains active, a success message "Record Updated Successfully" is displayed, and the System Admin is redirected.|
|\*\*Role Change Restricted (Assigned Board Member - Role Removed)\*\*|A System Admin is logged in and on the "Edit User" form for user "Alice" who has the "Board Member" role and is assigned to Fund X.|The System Admin attempts to remove the "Board Member" role from "Alice" and clicks "Save Changes".|The system displays an error message "You cannot change the role of this Board Member as they are assigned to a fund. Please unassign them first.", and "Alice"'s role remains unchanged.|
|\*\*Role Change Restricted (Sole Fund Manager/Associate Fund Manager)\*\*|A System Admin is logged in and on the "Edit User" form for user "Bob" who has the "Fund Manager" role and is the \*only\* Fund Manager assigned to Fund Y.|The System Admin attempts to change "Bob"'s role to "Accountant" and clicks "Save Changes".|The system displays an error message "You cannot change the role [Fund Manager] as they are assigned to one of the investment funds.", and "Bob"'s role remains unchanged.|
|\*\*Conditional WhatsApp Send - Success (Board Member to Fund Manager)\*\*|A System Admin is logged in. User "Bob" has ONLY the "Board Member" role, `Registration Message Is Sent` = 0.|The System Admin changes "Bob"'s role to "Fund Manager" (adding this role), and clicks "Save Changes".|"Bob"'s roles are updated, the WhatsApp registration message (MSG-ADD-008) is sent to "Bob", and "Bob"'s 'Registration Message Is Sent' flag is updated to 1. A success message is displayed.|
|\*\*Conditional WhatsApp Send - Failure (Board Member to Fund Manager)\*\*|A System Admin is logged in. User "Charlie" has ONLY the "Board Member" role, `Registration Message Is Sent` = 0. The WhatsApp API fails.|The System Admin changes "Charlie"'s role to "Associate Fund Manager", and clicks "Save Changes".|"Charlie"'s roles are updated, a warning message "Failed to send WhatsApp confirmation message. Please notify the user manually." is displayed, and "Charlie"'s 'Registration Message Is Sent' flag is updated to 1.|
|\*\*Relieve of Duties Notification Sent\*\*|A System Admin is logged in. User "David" has the "Fund Manager" role and is assigned to Fund Z.|The System Admin removes the "Fund Manager" role from "David" (while ensuring another Fund Manager exists for Fund Z) and clicks "Save Changes".|"David"'s roles are updated, and an in-app notification "تم اعفاءك من مهامك ك [مدير الصندوق] و ذلك على كافة الصناديق المنضم اليها " is sent to "David" with title "إعفاء من مهام/ relieve of duties ".|
|\*\*Role Update Notification Sent (Single-Holder Role Change)\*\*|A System Admin is logged in. User "Eve" has the "Accountant" role.|The System Admin changes "Eve"'s role to "Legal Counsel" and clicks "Save Changes".|"Eve"'s roles are updated, and an in-app notification "تم اعفاءك من مهامك ك [محاسب] و قد تم تعيينك ك [مستشار قانوني]" is sent to "Eve" with title "تعديل مهام/ Update duties ".|
|\*\*Mandatory Field Validation\*\*|A System Admin is on the "Edit User" form.|The System Admin clears a mandatory field (e.g., Name) and clicks "Save Changes".|The system displays a specific validation error message "Required Field." for the missing field, and the user's data is not updated.|
|\*\*Unique Email Validation\*\*|A System Admin is on the "Edit User" form.|The System Admin enters an email address that already exists for another user and clicks "Save Changes".|The system displays an error message "User with this email already exists.", and the user's data is not updated.|
|\*\*Conditional Role Selection - Invalid\*\*|A System Admin is on the "Edit User" form.|The System Admin attempts to assign an invalid role combination (e.g., "Fund Manager" and "Accountant") and clicks "Save Changes".|The system displays an error message "Invalid role selection." and prevents the invalid selection or does not update the user's roles.|
|\*\*CV Upload Size Limit\*\*|A System Admin is on the "Edit User" form.|The System Admin attempts to upload a CV file larger than 10 MB.|The system displays an error message "Invalid file format or size for CV. Please upload a PDF or DOCX file up to 10MB.", and the file is not uploaded.|
|\*\*System Error during Save\*\*|A backend error occurs during the saving process.|The system displays the message "An error is occurred while saving data" and the user's data is not updated.|The system displays the message "An error is occurred while saving data" and the user's data is not updated.|
|\*\*Cancel Edit\*\*|A System Admin is on the "Edit User" form.|The System Admin clicks the "Cancel" button after making changes.|The system discards all unsaved changes and redirects back to the previous page.|
### Data Entities Table
Entity Name: User (for editing)

|**Attribute (English)**|**Attribute (Arabic)**|**Mandatory/Optional**|**Attribute Type**|**Data Length**|**Integration Requirements**|**Default Value**|**Condition (if needed)**|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|User ID|معرف المستخدم|Mandatory|Number|N/A|Database Primary Key|N/A|N/A|Unique identifier, Read-only for edit|12345|12345|
|Name|الاسم|Mandatory|Text|Max 255|Database Field|N/A|N/A|N/A|محمد أحمد|Mohammed Ahmed|
|Email|البريد الإلكتروني|Mandatory|Text|Max 255|Database Field|N/A|N/A|Unique, Valid email format|mohammed.ahmed@jadwa.com|mohammed.ahmed@jadwa.com|
|Country Code|رمز الدولة|Mandatory|Text|Max 5|Database Field|+966|N/A|Must be '+966' for Saudi numbers. \*\*(Read-only)\*\*|+966|+966|
|Mobile|رقم الجوال|Mandatory|Text|Max 10|Database Field|N/A|N/A|Saudi mobile format (e.g., 05XXXXXXXX), Numeric only, Unique (as username). \*\*(Read-only)\*\*|0501234567|0501234567|
|IBAN|رقم الحساب المصرفي الدولي|Optional|Text|Max 34|Database Field|N/A|N/A|Valid IBAN format|SA9876543210987654321098|SA9876543210987654321098|
|Nationality|الجنسية|Optional|Text|Max 100|Database Field|N/A|N/A|N/A|سعودي|Saudi|
|CV|السيرة الذاتية|Optional|File Path/URL|N/A|File Storage (e.g., Azure Blob)|N/A|N/A|PDF, DOCX only, Max 10MB|سيرة\_ذاتية\_محمد.pdf|Mohammed\_CV.pdf|
|Passport No.|رقم جواز السفر|Optional|Text|Max 20|Database Field|N/A|N/A|Alphanumeric|A12345678|A12345678|
|Status|الحالة|Mandatory|Boolean/Dropdown|N/A|Database Field|Active|N/A|Active/Inactive (Read-only)|نشط|Active|
|Role|الدور|Mandatory|Relation to Role Entity|N/A|Database Many-to-Many|N/A|At least one role must be selected, conditional logic applies|Predefined roles only|مدير صندوق، عضو مجلس|Fund Manager, Board Member|
|Password|كلمة المرور|Mandatory|Text|N/A|Database Field|N/A|N/A|System-generated, stored securely (hashing handled internally).|N/A|N/A|
|Last Update Date|تاريخ آخر تحديث|Mandatory|DateTime|N/A|Database Field|Current Timestamp|N/A|Automatically set on creation and update.|2023-10-26 15:00:00|2023-10-26 15:00:00|
|Registration Message Is Sent|تم إرسال رسالة التسجيل|Mandatory|Boolean|N/A|Database Field|0 (False)|N/A|Set to 1 if user's role(s) make them eligible for WhatsApp message (NOT only 'Board Member'); 0 if only 'Board Member'. This flag is set based on eligibility and attempt to send.|1|True|
|Registration Is Completed|تم اكتمال التسجيل|Mandatory|Boolean|N/A|Database Field|0 (False)|N/A|Set to 0 upon user creation. Updated by a separate process (e.g., user's first login).|0|False|
|IsAssignedToFund|هل تم تعيينه لصندوق|Derived/Boolean|N/A|N/A|Derived from FundBoardMember assignments.|False|N/A|True if user is linked to any active fund board.|1|True|
|IsSoleFundManagerOfAnyFund|هل هو المدير الوحيد لأي صندوق|Derived/Boolean|N/A|N/A|True if user is a Fund Manager and is the only Fund Manager for any assigned fund.|False|N/A|Derived from Fund assignments.|1|True|
### Messages/Notifications Table

|**Message Code**|**Message (English)**|**Message (Arabic)**|**Message Type**|**Communication Method**|
| :- | :- | :- | :- | :- |
|MSG-EDIT-001|Required Field.|حقل إلزامي.|Validation Error|In-App|
|MSG-EDIT-002|Invalid email format.|صيغة البريد الإلكتروني غير صحيحة.|Validation Error|In-App|
|MSG-EDIT-003|User with this email already exists.|يوجد مستخدم بهذا البريد الإلكتروني بالفعل.|Validation Error|In-App|
|MSG-EDIT-004|Invalid role selection.|اختيار الدور غير صالح.|Validation Error|In-App|
|MSG-EDIT-005|Invalid Saudi mobile number format. Please enter a 10-digit number starting with 05.|صيغة رقم الجوال السعودي غير صالحة. يرجى إدخال رقم مكون من 10 أرقام يبدأ بـ 05.|Validation Error|In-App|
|MSG-EDIT-006|Mobile number is already in use as a username.|رقم الجوال مستخدم بالفعل كاسم مستخدم.|Validation Error|In-App|
|MSG-EDIT-007|Record Updated Successfully.|تم تحديث البيانات بنجاح.|Success Message|In-App|
|MSG-EDIT-008|Invalid file format or size for CV. Please upload a PDF or DOCX file up to 10MB.|صيغة الملف أو حجمه غير صالح للسيرة الذاتية. يرجى تحميل ملف PDF أو DOCX بحجم أقصى 10 ميجابايت.|Validation Error|In-App|
|MSG-EDIT-009|An error is occurred while saving data.|حدث خطأ بالنظام , لم يتم حفظ البيانات.|Error Message|In-App|
|MSG-EDIT-010|There is another active user with the role [Role Name]: [Existing User Name]. Do you want to replace him?|يوجد مستخدم نشط آخر بالدور [اسم الدور]: [اسم المستخدم الحالي]. هل تريد استبداله؟|Confirmation Dialog|In-App|
|MSG-EDIT-011|You cannot change the role of this Board Member as they are assigned to a fund. Please unassign them first.|لا يمكنك تغيير دور عضو مجلس الإدارة هذا لأنه معين لصندوق. يرجى إلغاء تعيينه أولاً.|Error Message|In-App|
|MSG-EDIT-012|You cannot change the role [Role Name] as they are assigned to one of the investment funds.|لا يمكنك تغيير الدور "[اسم الدور]" حيث انه مضاف لاحد الصناديق الاستثمارية.|Error Message|In-App|
|MSG-EDIT-013|ou have been relieved of your duties as [Role Name] for all assigned funds.|تم اعفاءك من مهامك ك [اسم الدور] و ذلك على كافة الصناديق المنضم اليها|Notification|In-App|
|\*\*MSG-EDIT-014\*\*|Your duties have been updated: you have been relieved of your duties as [Old Role Name] and assigned as [New Role Name].|\*\*تم اعفاءك من مهامك ك [اسم الدور] و قد تم تعيينك ك [اسم الدور]\*\*|\*\*Notification\*\*|\*\*In-App\*\*|
|MSG-ADD-008|Your registration for Jadwa Fund Board Management is complete. Your temporary password is: [Password]. Please log in through this link [login URL].|تم تسجيلك في تطبيق إدارة مجالس صناديق جدوى. كلمة المرور المؤقتة هي: [كلمة المرور]. يرجى تسجيل الدخول من خلال هذا الرابط [رابط تسجيل الدخول].|Success Message|WhatsApp|
|MSG-ADD-009|Failed to send WhatsApp confirmation message. Please notify the user manually.|فشل إرسال رسالة تأكيد الواتساب. يرجى إخطار المستخدم يدوياً.|Warning Message|In-App / System Log|
### Screen Elements Table

|**Element ID**|**Element Type**|**Element Name (English)**|**Element Name (Arabic)**|**Required/Optional**|**Validation Rules**|**Business Logic**|**Related Data Entity**|**User Interaction**|**Accessibility Notes**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|ELM-EDIT-001|Page Title|Edit User|تعديل المستخدم|N/A|N/A|Displays the title of the user editing form.|N/A|View|H1 heading, clear and concise.|
|ELM-EDIT-002|Input Field|Name Input|حقل الاسم|Mandatory|Text, Max 255 chars|Collects/displays the user's full name.|User.Name|Type|Clear label, placeholder text.|
|ELM-EDIT-003|Input Field|Email Input|حقل البريد الإلكتروني|Mandatory|Valid email format, Max 255 chars|Collects/displays the user's email address. Must be unique.|User.Email|Type|Clear label, placeholder text, email type keyboard.|
|ELM-EDIT-004|Text Display|Country Code |حقل رمز الدولة|Mandatory|N/A|displays the country dialing code for mobile number.|User.CountryCode|View|Clear label, placeholder text.|
|ELM-EDIT-005|Text Display|Mobile No. Display (Username)|عرض رقم الجوال (اسم المستخدم)|Mandatory|N/A|Displays the user's Saudi mobile phone number (username) as a non-editable label.|User.Mobile|View|Clear label, placeholder text, numeric keyboard.|
|ELM-EDIT-006|Input Field|IBAN Input|حقل رقم الحساب المصرفي الدولي|Optional|Valid IBAN format, Max 34 chars|Collects/displays the user's International Bank Account Number.|User.IBAN|Type|Clear label, placeholder text.|
|ELM-EDIT-007|Input Field|Nationality Input|حقل الجنسية|Optional|Text, Max 100 chars|Collects/displays the user's nationality.|User.Nationality|Type|Clear label, placeholder text.|
|ELM-EDIT-008|File Upload|CV Upload|تحميل السيرة الذاتية|Optional|PDF/DOCX only, Max 10MB|Allows user to upload/replace their CV file.|User.CV|Click (to browse/upload)|Clear label, file type/size guidance.|
|ELM-EDIT-009|Input Field|Passport No. Input|حقل رقم جواز السفر|Optional|Alphanumeric, Max 20 chars|Collects/displays the user's passport number.|User.PassportNo|Type|Clear label, placeholder text.|
|ELM-EDIT-010|Dropdown|Role Selection|اختيار الدور|Mandatory|At least one role selected, \*\*conditional logic applies\*\*|Allows selection of one or more predefined roles for the user. \*\*Multi-select enabled/disabled dynamically.\*\*|User.Role|Select|Clear label, accessible options, visual feedback for multi-select state.|
|ELM-EDIT-011|Text Display|Status Display|عرض الحالة|Mandatory|N/A|Displays the user's current status (Active/Inactive) as a non-editable label.|User.Status|View|Clear label, ensure sufficient contrast.|
|ELM-EDIT-012|Button|Save Changes |حفظ التغييرات|Mandatory|N/A|Submits the form to update the user.|N/A|Click|Primary action button, clearly labeled.|
|ELM-EDIT-013|Button|Cancel |إلغاء|Mandatory|N/A|Discards changes and returns to the previous page.|N/A|Click|Secondary action button, clearly labeled.|
|ELM-EDIT-014|Text Label|Validation Error Message|رسالة خطأ التحقق|Conditional|N/A|Displays specific validation errors next to relevant fields.|N/A|View|Red text, clear indication of error.|
|ELM-EDIT-015|Text Label|Success Message|رسالة النجاح|Conditional|N/A|Displays confirmation of successful user update.|N/A|View|Green text, prominent display.|
|ELM-EDIT-016|Text Label|Generic Error Message|رسالة خطأ عامة|Conditional|N/A|Displays unexpected system errors.|N/A|View|Red text, prominent display.|
|ELM-EDIT-017|Confirmation Dialog|Role Replacement Confirmation|تأكيد استبدال الدور|Conditional|N/A|Prompts System Admin to confirm replacing an existing user with a single-holder role.|N/A|View, Click (Replace/Cancel)|Clear message, accessible buttons.|
|ELM-EDIT-018|Button|Replace (in Dialog)|استبدال (في النافذة)|Mandatory|N/A|Confirms replacement of existing user and proceeds with current user's role update.|N/A|Click|Clearly labeled, primary action in dialog.|
|ELM-EDIT-019|Button|Cancel (in Dialog)|إلغاء (في النافذة)|Mandatory|N/A|Cancels the replacement and returns to the editing form.|N/A|Click|Clearly labeled.|

## User Story JDWA-1228: View System User Details
### Introduction Section
This user story describes the functionality for System Administrators to view the complete profile and detailed information of a specific user within the Fund Board Management Application. This provides a comprehensive overview of a user's data, including all attributes captured during creation and any additional details, serving as a reference for administrative tasks. The Screen Elements Table provides a detailed breakdown of the UI components on the user details page.
### Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|View System User Details|Allows System Administrators to view the complete profile of a specific user.|
|\*\*User Story\*\*|As a \*\*System Administrator\*\*, I want to \*\*view the detailed profile of a specific user\*\* so that I can \*\*access all their information for administrative purposes.\*\*||
|\*\*Story Points\*\*|3|This is a display-only functionality for a single record.|
|\*\*User Roles\*\*|System Admin|Only System Administrators can view detailed user profiles.|
|\*\*Access Requirements\*\*|Authenticated and authorized System Admin access. User must be selected from the user list.|User must be logged in as a System Admin.|
|\*\*Trigger\*\*|System Admin clicks the "View" link/button for a user from the "View System Users List".|User initiates the action from the user list.|
|\*\*Frequency of Use\*\*|Medium|Admins view details as needed for specific inquiries or before performing other actions.|
|\*\*Pre-condition\*\*|System Admin is logged in, has access to the User Management module, and the selected user exists.|The system is operational, and the selected user's data is available.|
|\*\*Business Rules\*\*|1\. All user attributes, including optional ones, should be displayed.<br>2. CV should be viewable/downloadable.<br>3. Roles should be clearly listed.<br>4. The 'Registration Message Is Sent' flag should be displayed.<br>5. The 'Registration Is Completed' flag should be displayed.<br>6. The "Delete User" action is removed from this view.<br>\*\*7. The "Reset Password" action is applicable only if the user's Status is 'Active' AND their 'Registration Is Completed' flag is 1 AND their 'Registration Message Is Sent' flag is 1.\*\*<br>8. The "Resend Message" action is only applicable if the user is 'Active', AND their 'Registration Is Completed' flag is 0, AND their 'Registration Message Is Sent' flag is 1.|Ensures complete visibility of user data and appropriate actions based on updated logic.|
|\*\*Post-condition\*\*|The system displays a dedicated page with all details of the selected user.|The system state reflects the successful display of detailed user information.|
|\*\*Risk\*\*|1\. Displaying outdated information if data is not refreshed.<br>2. Performance issues if CV download is slow.|Mitigation: Real-time data retrieval, optimized file serving.|
|\*\*Assumptions\*\*|1\. User data is stored in the system's database.<br>2. CV files are accessible via a file storage solution.<br>3. A separate user story for "Reset User Password" will define its specific process.|These assumptions are necessary for displaying comprehensive user data and managing actions.|
|\*\*UX/UI Design Link\*\*|N/A (Refer to general detail page design principles)|No specific mockups provided in the BRD for this section.|
### Process Flow Table

|**Step**|**Action Description**|**Actor**|**Related Message Codes**|**Notes**|
| :- | :- | :- | :- | :- |
|1|System Admin is viewing the "View System Users List".|System Admin|||
|2|System Admin clicks the "View" link/button for a specific user.|System Admin|||
|3|System retrieves all detailed information for the selected user from the database.|System|||
|4|System displays a dedicated "User Details" page, showing all attributes: Name, Email, Country Code, Mobile No., IBAN, Nationality, CV (link/download), Passport No., Status, Role(s), Last Update Date, Registration Message Is Sent flag, and Registration Is Completed flag.|System|CV should be a clickable link for viewing/downloading.||
|5|System Admin can review the user's details.|System Admin|||
|6|System Admin can click on action buttons/links like "Edit", "Resend Message", "Deactivate", "Activate", or "Reset Password" from this page.|System Admin|These actions navigate to their respective user stories. \*\*"Reset Password" is only visible/enabled if user is active AND Registration Is Completed = 1 AND Registration Message Is Sent = 1.\*\* "Resend Message" is visible/enabled based on its specific eligibility criteria.||
|7|System Admin can navigate back to the user list.|System Admin|||
### Alternative Flow Table

|**Alternative Scenario**|**Condition**|**Action**|**Related Message Codes**|**Resolution**|
| :- | :- | :- | :- | :- |
|\*\*User Not Found\*\*|The selected user's ID is invalid or the user has been deleted.|System displays an error message (e.g., "User not found").|MSG-VIEW-001|System Admin can return to the user list.|
|\*\*Data Retrieval Error\*\*|An error occurs during retrieval of user details.|System displays a generic error message.|MSG-VIEW-002|System Admin can retry or contact support.|
|\*\*CV File Not Found\*\*|The CV file linked to the user is missing or corrupted.|System displays a "File not found" message or disables the CV link.|MSG-VIEW-003|System Admin can contact support or re-upload CV via Edit.|
|\*\*Reset Password Not Applicable (Eligibility Not Met)\*\*|The user's Status is NOT 'Active', OR their 'Registration Is Completed' flag is 0, OR their 'Registration Message Is Sent' flag is 0.|The "Reset Password" button is \*\*hidden\*\* or disabled.|System Admin cannot perform the action.||
|\*\*Resend Message Not Applicable (Eligibility Not Met)\*\*|The user does not meet the eligibility criteria for "Resend Message" (e.g., Inactive, Registration Is Completed = 1, or Registration Message Is Sent = 0).|The "Resend Message" button is \*\*hidden\*\* or disabled.|System Admin cannot perform the action.||
### Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*View User Details - Success\*\*|A System Admin is logged in and on the "View System Users List".|The System Admin clicks the "View" link/button for an existing user.|The system navigates to a dedicated "User Details" page displaying all the user's information accurately, including their Name, Email, Status, Role, Last Update Date, Registration Message Is Sent flag, Registration Is Completed flag, and optional fields like CV and Passport No.|
|\*\*View User Details - CV Download\*\*|A System Admin is viewing a user's details page.|The System Admin clicks on the CV link/button.|The user's CV file is downloaded or opened in a new tab/window.|
|\*\*View User Details - User Not Found\*\*|A System Admin attempts to view details for a non-existent user (e.g., by direct URL manipulation).|The system receives a request for a user ID that does not exist.|The system displays an error message indicating "User not found".|
|\*\*Display Registration Message Is Sent Flag\*\*|A System Admin is viewing a user's details page.|The 'Registration Message Is Sent' flag is displayed.|The flag accurately reflects whether the user was eligible for a WhatsApp message based on their role(s) (1 for eligible, 0 for only Board Member).|
|\*\*Display Registration Is Completed Flag\*\*|A System Admin is viewing a user's details page.|The 'Registration Is Completed' flag is displayed.|The flag accurately reflects the overall completion status of the user's registration (0 for not completed, 1 for completed).|
|\*\*Reset Password Button Visibility (Eligible User)\*\*|A System Admin is viewing a user's details page where the user is 'Active', 'Registration Is Completed' is 1, and 'Registration Message Is Sent' is 1.|The page loads.|The "Reset Password" button is visible and enabled.|
|\*\*Reset Password Button Visibility (Inactive User)\*\*|A System Admin is viewing an 'Inactive' user's details page.|The page loads.|The "Reset Password" button is \*\*hidden\*\* or disabled.|
|\*\*Reset Password Button Visibility (Registration Not Complete)\*\*|A System Admin is viewing a user's details page where the user is 'Active', 'Registration Is Completed' is 0, and 'Registration Message Is Sent' is 1.|The page loads.|The "Reset Password" button is \*\*hidden\*\* or disabled.|
|\*\*Reset Password Button Visibility (Message Not Sent)\*\*|A System Admin is viewing a user's details page where the user is 'Active', 'Registration Is Completed' is 1, and 'Registration Message Is Sent' is 0.|The page loads.|The "Reset Password" button is \*\*hidden\*\* or disabled.|
|\*\*Resend Message Button Visibility (Eligible User)\*\*|A System Admin is viewing a user's details page where the user is 'Active', 'Registration Is Completed' is 0, and 'Registration Message Is Sent' is 1.|The page loads.|The "Resend Message" button is visible and enabled.|
|\*\*Resend Message Button Visibility (Ineligible User)\*\*|A System Admin is viewing a user's details page where the user is 'Inactive', OR 'Registration Is Completed' is 1, OR 'Registration Message Is Sent' is 0.|The page loads.|The "Resend Message" button is \*\*hidden\*\* or disabled.|
### Data Entities Table
Entity Name: User (for viewing details)

|**Attribute (English)**|**Attribute (Arabic)**|**Mandatory/Optional**|**Attribute Type**|**Data Length**|**Integration Requirements**|**Default Value**|**Condition (if needed)**|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|User ID|معرف المستخدم|Mandatory|Number|N/A|Database Primary Key|N/A|N/A|Unique identifier|12345|12345|
|Name|الاسم|Mandatory|Text|Max 255|Database Field|N/A|N/A|N/A|محمد أحمد|Mohammed Ahmed|
|Email|البريد الإلكتروني|Mandatory|Text|Max 255|Database Field|N/A|N/A|Unique, Valid email format|mohammed.ahmed@jadwa.com|mohammed.ahmed@jadwa.com|
|Country Code|رمز الدولة|Mandatory|Text|Max 5|Database Field|+966|N/A|Must be '+966' for Saudi numbers.|+966|+966|
|Mobile|رقم الجوال|Mandatory|Text|Max 10|Database Field|N/A|N/A|Saudi mobile format (e.g., 05XXXXXXXX), Numeric only, Unique (as username).|0501234567|0501234567|
|IBAN|رقم الحساب المصرفي الدولي|Optional|Text|Max 34|Database Field|N/A|N/A|Valid IBAN format|SA1234567890123456789012|SA1234567890123456789012|
|Nationality|الجنسية|Optional|Text|Max 100|Database Field|N/A|N/A|N/A|سعودي|Saudi|
|CV|السيرة الذاتية|Optional|File Path/URL|N/A|File Storage (e.g., Azure Blob)|N/A|N/A|PDF, DOCX only, Max 10MB|سيرة\_ذاتية\_محمد.pdf|Mohammed\_CV.pdf|
|Passport No.|رقم جواز السفر|Optional|Text|Max 20|Database Field|N/A|N/A|Alphanumeric|A12345678|A12345678|
|Status|الحالة|Mandatory|Boolean/Dropdown|N/A|Database Field|Active|N/A|Active/Inactive|نشط|Active|
|Role|الدور|Mandatory|Relation to Role Entity|N/A|Database Many-to-Many|N/A|At least one role must be selected, conditional logic applies|Predefined roles only|مدير صندوق، عضو مجلس|Fund Manager, Board Member|
|Password|كلمة المرور|Mandatory|Text|N/A|Database Field|N/A|N/A|System-generated, stored securely (hashing handled internally).|N/A|N/A|
|Last Update Date|تاريخ آخر تحديث|Mandatory|DateTime|N/A|Database Field|Current Timestamp|N/A|Automatically set on creation and update.|2023-10-26 15:00:00|2023-10-26 15:00:00|
|Registration Message Is Sent|تم إرسال رسالة التسجيل|Mandatory|Boolean|N/A|Database Field|0 (False)|N/A|Set to 1 if user's role(s) make them eligible for WhatsApp message (NOT only 'Board Member'); 0 if only 'Board Member'.|1|True|
|Registration Is Completed|تم اكتمال التسجيل|Mandatory|Boolean|N/A|Database Field|0 (False)|N/A|Set to 0 upon user creation. Updated by a separate process (e.g., user's first login).|0|False|
### Messages/Notifications Table

|**Message Code**|**Message (English)**|**Message (Arabic)**|**Message Type**|**Communication Method**|
| :- | :- | :- | :- | :- |
|MSG-VIEW-001|User not found.|لم يتم العثور على المستخدم.|Error Message|In-App|
|MSG-VIEW-002|An error occurred while retrieving user details. Please try again.|حدث خطأ أثناء استرداد تفاصيل المستخدم. يرجى المحاولة مرة أخرى.|Error Message|In-App|
|MSG-VIEW-003|CV file not found or is corrupted.|ملف السيرة الذاتية غير موجود أو تالف.|Warning Message|In-App|
### Screen Elements Table

|**Element ID**|**Element Type**|**Element Name (English)**|**Element Name (Arabic)**|**Required/Optional**|**Validation Rules**|**Business Logic**|**Related Data Entity**|**User Interaction**|**Accessibility Notes**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|ELM-VIEW-001|Page Title|User Details|تفاصيل المستخدم|N/A|N/A|Displays the title of the user details page.|N/A|View|H1 heading, clear and concise.|
|ELM-VIEW-002|Text Label|Name Label|تسمية الاسم|Mandatory|N/A|Label for the user's name.|N/A|View|Clear label.|
|ELM-VIEW-003|Text Display|User Name Display|عرض اسم المستخدم|Mandatory|N/A|Displays the user's full name.|User.Name|View|Ensure sufficient contrast.|
|ELM-VIEW-004|Text Label|Email Label|تسمية البريد الإلكتروني|Mandatory|N/A|Label for the user's email.|N/A|View|Clear label.|
|ELM-VIEW-005|Text Display|User Email Display|عرض بريد المستخدم الإلكتروني|Mandatory|N/A|Displays the user's email address.|User.Email|View|Ensure sufficient contrast.|
|ELM-VIEW-006|Text Label|Country Code Label|تسمية رمز الدولة|Mandatory|N/A|Label for the user's country code.|N/A|View|Clear label.|
|ELM-VIEW-007|Text Display|User Country Code Display|عرض رمز الدولة للمستخدم|Mandatory|N/A|Displays the user's country code.|User.CountryCode|View|Ensure sufficient contrast.|
|ELM-VIEW-008|Text Label|Mobile No. Label|تسمية رقم الجوال|Mandatory|N/A|Label for the user's mobile number.|N/A|View|Clear label.|
|ELM-VIEW-009|Text Display|User Mobile No. Display|عرض رقم الجوال للمستخدم|Mandatory|N/A|Displays the user's mobile number (username).|User.Mobile|View|Ensure sufficient contrast.|
|ELM-VIEW-010|Text Label|IBAN Label|تسمية رقم الحساب المصرفي الدولي|Optional|N/A|Label for the user's IBAN.|N/A|View|Clear label.|
|ELM-VIEW-011|Text Display|User IBAN Display|عرض رقم الحساب المصرفي الدولي للمستخدم|Optional|N/A|Displays the user's IBAN.|User.IBAN|View|Ensure sufficient contrast.|
|ELM-VIEW-012|Text Label|Nationality Label|تسمية الجنسية|Optional|N/A|Label for the user's nationality.|N/A|View|Clear label.|
|ELM-VIEW-013|Text Display|User Nationality Display|عرض جنسية المستخدم|Optional|N/A|Displays the user's nationality.|User.Nationality|View|Ensure sufficient contrast.|
|ELM-VIEW-014|Link/Button|CV Link/Download|رابط/زر تحميل السيرة الذاتية|Optional|N/A|Allows viewing/downloading of the user's CV.|User.CV|Click|Clear label, accessible.|
|ELM-VIEW-015|Text Label|Passport No. Label|تسمية رقم جواز السفر|Optional|N/A|Label for the user's passport number.|N/A|View|Clear label.|
|ELM-VIEW-016|Text Display|User Passport No. Display|عرض رقم جواز السفر للمستخدم|Optional|N/A|Displays the user's passport number.|User.PassportNo|View|Ensure sufficient contrast.|
|ELM-VIEW-017|Text Label|Status Label|تسمية الحالة|Mandatory|N/A|Label for the user's status.|N/A|View|Clear label.|
|ELM-VIEW-018|Text Display|User Status Display|عرض حالة المستخدم|Mandatory|N/A|Displays the user's status (Active/Inactive).|User.Status|View|Use clear visual indicators.|
|ELM-VIEW-019|Text Label|Role Label|تسمية الدور|Mandatory|N/A|Label for the user's assigned role(s).|N/A|View|Clear label.|
|ELM-VIEW-020|Text Display|User Role Display|عرض دور المستخدم|Mandatory|N/A|Displays the user's primary role or list of roles.|User.Role|View|Ensure sufficient contrast.|
|ELM-VIEW-021|Text Label|Last Update Date Label|تسمية تاريخ آخر تحديث|Mandatory|N/A|Label for the last update date.|N/A|View|Clear label.|
|ELM-VIEW-022|Text Display|Last Update Date Display|عرض تاريخ آخر تحديث|Mandatory|N/A|Displays the last date and time the user record was modified.|User.LastUpdateDate|View|Consistent date/time format.|
|ELM-VIEW-023|Text Label|Registration Message Is Sent Label|تسمية تم إرسال رسالة التسجيل|Mandatory|N/A|Label for the registration message sent flag.|N/A|View|Clear label.|
|ELM-VIEW-024|Text Display|Registration Message Is Sent Display|عرض تم إرسال رسالة التسجيل|Mandatory|N/A|Displays the status of the 'Registration Message Is Sent' flag (True/False or 1/0).|User.RegistrationMessageIsSent|View|Ensure sufficient contrast.|
|ELM-VIEW-025|Text Label|Registration Is Completed Label|تسمية تم اكتمال التسجيل|Mandatory|N/A|Label for the registration is completed flag.|N/A|View|Clear label.|
|ELM-VIEW-026|Text Display|Registration Is Completed Display|عرض تم اكتمال التسجيل|Mandatory|N/A|Displays the status of the 'Registration Is Completed' flag (True/False or 1/0).|User.RegistrationIsCompleted|View|Ensure sufficient contrast.|
|ELM-VIEW-027|Button|Edit User Button|زر تعديل المستخدم|Optional|N/A|Navigates to the "Edit Existing System User" form for this user.|N/A|Click|Clearly labeled.|
|ELM-VIEW-028|Button|Resend Message Button|زر إعادة إرسال الرسالة|Conditional|N/A|Initiates the "Resend Account Registration Message" process for this user.|N/A|Click|Clearly labeled, visible/enabled based on conditions.|
|ELM-VIEW-029|Button|Deactivate User Button|زر إلغاء تفعيل المستخدم|Conditional|N/A|Initiates the "Deactivate System User" process for this user.|N/A|Click|Clearly labeled, visible/enabled if user is active.|
|ELM-VIEW-030|Button|Activate User Button|زر تفعيل المستخدم|Conditional|N/A|Initiates the "Activate System User" process for this user.|N/A|Click|Clearly labeled, visible/enabled if user is inactive.|
|ELM-VIEW-031|Button|Reset Password Button|زر إعادة تعيين كلمة المرور|Conditional|N/A|Initiates the "Reset User Password" process for this user.|N/A|Click|Clearly labeled, visible/enabled \*\*only if user is active AND Registration Is Completed = 1 AND Registration Message Is Sent = 1.\*\*|
|ELM-VIEW-032|Button|Back to List Button|زر العودة إلى القائمة|Mandatory|N/A|Navigates back to the "View System Users List".|N/A|Click|Clearly labeled.|
### Summary Section
This user story for "View System User Details" has been comprehensively updated to reflect all the latest changes and interdependencies with the "Add New System User" and "Resend Account Registration Message" stories. This includes:

\*   Displaying both the `Registration Message Is Sent` and `Registration Is Completed` flags.

\*   Updating the visibility logic for the "Resend Message" button based on its new eligibility criteria.

\*   Updating the visibility logic for the "Reset Password" button to be `(Status = Active AND Registration Is Completed = 1 AND Registration Message Is Sent = 1)`.

\*   The removal of `IsAssignedToFundBoard` is also maintained.

\*\*Important Note:\*\* I am still awaiting your clarification on the "Action Not Applicable (Role/Assignment)" logic for the "Resend Account Registration Message" user story from our previous interaction. The current logic in that story (and reflected in its Acceptance Criteria) is: "Action is NOT applicable if the user has ONLY the 'Board Member' role AND is NOT assigned to any fund board." However, with `IsAssignedToFundBoard` removed from the User entity, this specific condition cannot be implemented as written. \*\*Please confirm how the "Resend Message" eligibility for Board Members should be determined without `IsAssignedToFundBoard`.\*\*

\*\*Key Implementation Considerations:\*\*

\*   Ensure the "Reset Password" button's visibility/enablement strictly adheres to its new, complex eligibility criteria.

\*   Ensure the "Resend Message" button's visibility/enablement strictly adheres to its complex eligibility criteria.

\*   \*\*A new user story for "Reset User Password" will be required\*\* to define its specific process.

\*\*Dependencies and Risks:\*\*

\*   \*\*Dependency:\*\* This story depends on the "View System Users List" story for navigation to the details page. It also depends on the "Add New System User" and "Edit Existing System User" stories for the data displayed, and the correct setting of `Registration Message Is Sent` and `Registration Is Completed` flags.

\*   \*\*Risk:\*\* Misinterpretation of the complex eligibility conditions for action buttons leading to incorrect visibility. Mitigation: Thorough testing of the combined conditional logic.

\---

I have now updated the "View System User Details" user story with the precise eligibility criteria for the "Reset Password" button. Please provide clarification on the "Resend Account Registration Message" logic regarding Board Members when you're ready.

## User Story JDWA-1225: Resend Account Registration Message
### Introduction Section
This user story describes the functionality for System Administrators to resend the account registration WhatsApp message to a specific user. This is particularly useful if the initial message failed to deliver or was not received by the user. This action is conditionally available only for active users whose initial registration message was eligible to be sent (flag = 1) but whose overall registration process is not yet completed (flag = 0). This helps ensure users receive their login credentials if the initial message was missed or failed.
### Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|Resend Account Registration Message|Allows System Administrators to resend the registration WhatsApp message to a user.|
|\*\*User Story\*\*|As a \*\*System Administrator\*\*, I want to \*\*resend the account registration message to an eligible user\*\* so that I can \*\*ensure they receive their login credentials and complete their registration.\*\*||
|\*\*Story Points\*\*|5|Involves user selection, conditional logic, external API call, and status update.|
|\*\*User Roles\*\*|System Admin|Only System Administrators can perform this action.|
|\*\*Access Requirements\*\*|Authenticated and authorized System Admin access. User must be selected from the user list or details page.|User must be logged in as a System Admin.|
|\*\*Trigger\*\*|System Admin clicks "Resend Message" button/link for a specific user from the user list or user details page.|User initiates the action.|
|\*\*Frequency of Use\*\*|Low to Medium|Used when initial message fails or user requests it.|
|\*\*Pre-condition\*\*|System Admin is logged in. The target user account exists and has a valid Saudi mobile number. \*\*The user must be 'Active', AND their 'Registration Is Completed' flag is 0, AND their 'Registration Message Is Sent' flag is 1.\*\*|The system is operational, and the user meets all specific eligibility criteria.|
|\*\*Business Rules\*\*|1\. The "Resend Message" action is \*\*ONLY applicable\*\* if the user's Status is 'Active' AND their 'Registration Is Completed' flag is 0 AND their 'Registration Message Is Sent' flag is 1.<br>2. The "Resend Message" button/link must be hidden if the user does not meet these criteria.<br>3. The user must have a valid Saudi mobile number registered.<br>4. The system will resend the same registration message content, including the temporary password and login link (content from MSG-ADD-008).<br>5. The 'Registration Message Is Sent' flag for the user should be updated to 1 upon successful resend (if it was 0, but it should already be 1 for eligibility).<br>6. A confirmation dialog should be presented before resending the message.|Ensures strict conditional availability, proper message content, and status tracking.|
|\*\*Post-condition\*\*|The WhatsApp message is resent to the user, a success/failure message is displayed to the System Admin, and the 'Registration Message Is Sent' flag is updated if applicable.|The system state reflects the attempt and outcome of resending the message.|
|\*\*Risk\*\*|1\. WhatsApp message delivery failure.<br>2. Resending message to an incorrect user.<br>3. Performance issues with WhatsApp API.|Mitigation: Confirmation dialog, robust mobile number validation, API monitoring.|
|\*\*Assumptions\*\*|1\. WhatsApp Business API integration is available and configured.<br>2. The system can retrieve the user's temporary password for the message content.|These assumptions are crucial for implementing the functionality.|
|\*\*UX/UI Design Link\*\*|N/A (Refer to general action button/confirmation dialog patterns)|No specific mockups provided in the BRD for this section.|
### Process Flow Table

|**Step**|**Action Description**|**Actor**|**Related Message Codes**|**Notes**|
| :- | :- | :- | :- | :- |
|1|System Admin is viewing the "View System Users List" or "View User Details" page.|System Admin|||
|2|System Admin identifies a user and clicks the "Resend Message" button/link for that user.|System Admin|This button/link is only visible/enabled if the user meets \*\*all\*\* eligibility criteria (Active, Registration Is Completed = 0, Registration Message Is Sent = 1).||
|3|System displays a confirmation dialog asking the System Admin to confirm resending the message.|System|MSG-RESEND-001|Includes the user's name in the message.|
|4|System Admin reviews the confirmation message and clicks "\*\*Resend\*\*" or "Cancel".|System Admin|If "Cancel" is clicked, the process stops.||
|5|System retrieves the user's mobile number, temporary password, and login URL.|System|||
|6|System attempts to send the WhatsApp registration message to the user (content from MSG-ADD-008).|System|MSG-RESEND-002, MSG-ADD-008||
|7|System displays a success or failure message to the System Admin based on the WhatsApp message sending attempt.|System|MSG-RESEND-002|Note: Using MSG-RESEND-002 for both success/failure, message text will differentiate.|
|8|System Admin can continue managing users.|System Admin|||
### Alternative Flow Table

|**Alternative Scenario**|**Condition**|**Action**|**Related Message Codes**|**Resolution**|
| :- | :- | :- | :- | :- |
|\*\*Action Not Applicable (Eligibility Not Met)\*\*|System Admin attempts to click "Resend Message" for a user who is NOT 'Active', OR whose 'Registration Is Completed' flag is 1, OR whose 'Registration Message Is Sent' flag is 0.|The "Resend Message" button/link is \*\*hidden\*\*.|System Admin cannot perform the action.||
|\*\*Confirmation Canceled\*\*|System Admin clicks "Resend Message" but then clicks "Cancel" in the confirmation dialog.|System closes the confirmation dialog and no message is sent.|No action performed.||
|\*\*WhatsApp API Error\*\*|An error occurs during the WhatsApp API call (e.g., API down, rate limit exceeded).|System displays a failure message to the System Admin and logs the error.|MSG-RESEND-002|System Admin can retry later. Note: Using MSG-RESEND-002 for both success/failure, message text will differentiate.|
### Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Resend Message - Success (Eligible User)\*\*|A System Admin is logged in. A user is 'Active', their 'Registration Is Completed' flag is 0, and their 'Registration Message Is Sent' flag is 1.|The System Admin clicks "Resend Message" for this user and clicks "Resend" in the confirmation dialog.|The WhatsApp registration message is successfully sent to the user, the 'Registration Message Is Sent' flag for the user remains 1 (as it was already 1), and a success message "Account registration message sent successfully." is displayed.|
|\*\*Resend Message - Not Applicable (Inactive User)\*\*|A System Admin is logged in. A user is 'Inactive'.|The System Admin attempts to click "Resend Message" for this user.|The "Resend Message" button/link is \*\*hidden\*\*, and the action cannot be performed.|
|\*\*Resend Message - Not Applicable (Registration Completed = 1)\*\*|A System Admin is logged in. A user is 'Active', their 'Registration Is Completed' flag is 1, and their 'Registration Message Is Sent' flag is 1.|The System Admin attempts to click "Resend Message" for this user.|The "Resend Message" button/link is \*\*hidden\*\*, and the action cannot be performed.|
|\*\*Resend Message - Not Applicable (Registration Message Is Sent = 0)\*\*|A System Admin is logged in. A user is 'Active', their 'Registration Is Completed' flag is 0, and their 'Registration Message Is Sent' flag is 0 (e.g., Board Member only).|The System Admin attempts to click "Resend Message" for this user.|The "Resend Message" button/link is \*\*hidden\*\*, and the action cannot be performed.|
|\*\*Resend Message - Failure (WhatsApp API Error)\*\*|A System Admin is logged in. A user is eligible for the message, but the WhatsApp API fails to send it.|The System Admin clicks "Resend Message" for this user and clicks "Resend" in the confirmation dialog.|A failure message (e.g., "Failed to send account registration message. Please try again.") is displayed, and the 'Registration Message Is Sent' flag remains unchanged (at 1).|
|\*\*Resend Message - Confirmation Canceled\*\*|A System Admin clicks "Resend Message" for a user.|The System Admin clicks "Cancel" in the confirmation dialog.|The confirmation dialog closes, and no message is sent.|
### Data Entities Table
Entity Name: User (for resending message)

|**Attribute (English)**|**Attribute (Arabic)**|**Mandatory/Optional**|**Attribute Type**|**Data Length**|**Integration Requirements**|**Default Value**|**Condition (if needed)**|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|User ID|معرف المستخدم|Mandatory|Number|N/A|Database Primary Key|N/A|N/A|Unique identifier|12345|12345|
|Mobile|رقم الجوال|Mandatory|Text|Max 10|Database Field|N/A|N/A|Saudi mobile format (e.g., 05XXXXXXXX), Numeric only, Unique (as username).|0501234567|0501234567|
|Password|كلمة المرور|Mandatory|Text|N/A|Database Field|N/A|N/A|System-generated, stored securely (hashing handled internally).|N/A|N/A|
|Role|الدور|Mandatory|Relation to Role Entity|N/A|Database Many-to-Many|N/A|At least one role must be selected, conditional logic applies|Predefined roles only|مدير صندوق، عضو مجلس|Fund Manager, Board Member|
|Registration Message Is Sent|تم إرسال رسالة التسجيل|Mandatory|Boolean|N/A|Database Field|0 (False)|N/A|Set to 1 if user's role(s) make them eligible for WhatsApp message (NOT only 'Board Member'); 0 if only 'Board Member'. This flag is set based on eligibility and attempt to send.|1|True|
|Registration Is Completed|تم اكتمال التسجيل|Mandatory|Boolean|N/A|Database Field|0 (False)|N/A|Set to 0 upon user creation. Updated by a separate process (e.g., user's first login).|0|False|
|Status|الحالة|Mandatory|Boolean/Dropdown|N/A|Database Field|Active|N/A|Active/Inactive|نشط|Active|
### Messages/Notifications Table

|**Message Code**|**Message (English)**|**Message (Arabic)**|**Message Type**|**Communication Method**|
| :- | :- | :- | :- | :- |
|MSG-RESEND-001|Are you sure you want to resend the account registration message to [User Name]?|هل أنت متأكد من إعادة إرسال رسالة تسجيل الحساب إلى [اسم المستخدم]؟|Confirmation Dialog|In-App|
|MSG-RESEND-002|Account registration message sent successfully. / Failed to send account registration message. Please try again.|تم إرسال رسالة تسجيل الحساب بنجاح. / فشل إرسال رسالة تسجيل الحساب. يرجى المحاولة مرة أخرى.|Success/Error Message|In-App|
|MSG-ADD-008|Your registration for Jadwa Fund Board Management is complete. Your temporary password is: [Password]. Please log in through this link [login URL].|تم تسجيلك في تطبيق إدارة مجالس صناديق جدوى. كلمة المرور المؤقتة هي: [كلمة المرور]. يرجى تسجيل الدخول من خلال هذا الرابط [رابط تسجيل الدخول].|Success Message|WhatsApp|
### Screen Elements Table

|**Element ID**|**Element Type**|**Element Name (English)**|**Element Name (Arabic)**|**Required/Optional**|**Validation Rules**|**Business Logic**|**Related Data Entity**|**User Interaction**|**Accessibility Notes**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|ELM-RESEND-001|Button|Resend Message|إعادة إرسال الرسالة|Conditional|N/A|Initiates the resend process. \*\*Hidden if user is not eligible.\*\*|N/A|Click|Clear label, visible/enabled based on conditions.|
|ELM-RESEND-002|Confirmation Dialog|Resend Confirmation Dialog|نافذة تأكيد إعادة الإرسال|Mandatory|N/A|Prompts System Admin to confirm resend action.|N/A|View, Click (Confirm/Cancel)|Clear message, accessible controls.|
|ELM-RESEND-003|Text Label|Confirmation Message Text|نص رسالة التأكيد|Mandatory|N/A|Displays "Are you sure you want to resend the account registration message to [User Name]?"|N/A|View|Dynamic content for user name.|
|ELM-RESEND-004|Button|Resend (in Dialog)|إعادة الإرسال (في النافذة)|Mandatory|N/A|Confirms the resend action.|N/A|Click|Clearly labeled.|
|ELM-RESEND-005|Button|Cancel Resend (in Dialog)|إلغاء إعادة الإرسال (في النافذة)|Mandatory|N/A|Cancels the resend action.|N/A|Click|Clearly labeled.|
|ELM-RESEND-006|Text Label|Success/Error Message Display|عرض رسالة النجاح/الخطأ|Conditional|N/A|Displays the outcome of the resend attempt.|N/A|View|Prominent display, clear text.|
### Summary Section
This rewritten user story for "Resend Account Registration Message" now uses "Button" as the `Element Type` and removes the word "زر" from the Arabic "Element Name" in the Screen Elements Table, providing a more concise and consistent naming convention for UI elements.

\*\*Important Note:\*\* I am still awaiting your clarification on the "Action Not Applicable (Role/Assignment)" logic for the "Resend Account Registration Message" user story from our previous interaction. The current logic in that story (and reflected in its Acceptance Criteria) is: "Action is NOT applicable if the user has ONLY the 'Board Member' role AND is NOT assigned to any fund board." However, with `IsAssignedToFundBoard` removed from the User entity, this specific condition cannot be implemented as written. \*\*Please confirm how the "Resend Message" eligibility for Board Members should be determined without `IsAssignedToFundBoard`.\*\*

\*\*Key Implementation Considerations:\*\*

\*   \*\*Critical:\*\* Implement the precise logic for \*\*hiding\*\* the "Resend Message" button: `(User.Status = 'Inactive' OR User.RegistrationIsCompleted = 1 OR User.RegistrationMessageIsSent = 0)`.

\*   Ensure the single `MSG-RESEND-002` can dynamically display both success and failure messages based on the WhatsApp API response.

\*   Handle WhatsApp API failures gracefully, logging errors and informing the System Admin.

\*\*Dependencies and Risks:\*\*

\*   \*\*Dependency:\*\* This story depends on the "View System Users List" or "View User Details" stories for initiating the action. It also depends on the "Add New System User" story for initial user creation and password generation.

\*   \*\*Risk:\*\* Misinterpretation of the "not applicable" conditions leading to incorrect button visibility. Mitigation: Thorough testing of the combined conditional logic.

\---

I have now updated the "Resend Account Registration Message" user story with the requested changes to the Screen Elements Table. Please provide clarification on the "Resend Account Registration Message" logic regarding Board Members when you're ready.
## User Story JDWA-1223: Add New System User
### Introduction Section
This user story details the functionality enabling System Administrators to create new user accounts within the Fund Board Management Application. This process involves capturing essential user information, where the Saudi Mobile Number serves as the account username, assigning appropriate roles with specific conditional logic (affecting multi-select behavior), setting the initial active status, generating a default password, and conditionally notifying the user via WhatsApp. A new validation is introduced for specific roles (Legal Counsel, Finance Controller, Compliance and Legal Managing Director, Head of Real Estate) to check for existing active users with the same selected role, prompting for a replacement if found. If the replacement is cancelled, the admin can proceed with creating the new user by selecting a non-conflicting role. A flag tracks WhatsApp message eligibility, and another tracks overall registration completion. The Screen Elements Table provides a detailed breakdown of the UI components on the user creation form.
### Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|Add New System User|Enables System Administrators to create new user accounts.|
|\*\*User Story\*\*|As a \*\*System Administrator\*\*, I need to \*\*add new user accounts with specific role assignments, handling potential role replacements, and track registration status\*\* so that I can \*\*onboard new personnel and grant them appropriate system access.\*\*||
|\*\*Story Points\*\*|13|This involves complex data entry, dynamic UI for role selection, unique role validation with replacement workflow, password generation, conditional external notification, and status tracking.|
|\*\*User Roles\*\*|System Admin|Only System Administrators can add new users.|
|\*\*Access Requirements\*\*|Authenticated and authorized System Admin access to the User Management module.|User must be logged in as a System Admin.|
|\*\*Trigger\*\*|System Admin clicks "Add New User" button from the user list page.|User initiates the action from the system's administrative interface.|
|\*\*Frequency of Use\*\*|Medium|New users are added periodically as personnel changes or new funds are introduced.|
|\*\*Pre-condition\*\*|System Admin is logged in and has access to the User Management module.|The system is operational, and the admin has the necessary permissions.|
|\*\*Business Rules\*\*|1\. Each user must have a unique email address.<br>2. All mandatory fields must be filled.<br>3. At least one role must be assigned to a new user.<br>4. CV and Passport No. are optional fields.<br>5. New users default to 'Active' status.<br>6. \*\*Role Selection Logic:\*\* Multi-select for roles is enabled \*\*ONLY IF\*\* the selected roles are ('Fund Manager' AND 'Board Member') OR ('Associate Fund Manager' AND 'Board Member'). Otherwise, multi-select is disabled, restricting to a single role.<br>7. System automatically generates a default password for new users.<br>8. Country Code and Mobile No. are mandatory and restricted to Saudi mobile numbers only (starting with 05, 9 digits after 05).<br>9. The Saudi Mobile No. will serve as the user's account username.<br>10. A WhatsApp message is sent to the user upon successful registration confirmation, EXCEPT if the added user has ONLY the 'Board Member' role.<br>11. The 'Registration Message Is Sent' flag is set to 1 if the user's assigned role(s) make them eligible for a WhatsApp message (i.e., NOT only 'Board Member'). Otherwise, it's set to 0 (for users with only 'Board Member' role). This flag reflects eligibility for sending, not delivery success.<br>12. The 'Registration Is Completed' flag is set to 0 (False) by default upon user creation.<br>13. Unique Role Validation with Replacement: If a selected role is one of: 'Legal Counsel', 'Finance Controller', 'Compliance and Legal Managing Director', 'Head of Real Estate', the system must check for another \*active\* user (who is \*not\* the user currently being edited) currently holding \*only\* that specific role. If found, a confirmation dialog is displayed to replace the existing user. If 'Replace' is chosen, the existing user is deactivated before the new user is added. If 'Cancel' is chosen, the System Admin is returned to the form to modify role selection or other details.|Ensures data integrity, proper access control, dynamic UI behavior, and user notification eligibility/completion tracking during creation, with a critical role replacement workflow.|
|\*\*Post-condition\*\*|A new user account is successfully created with an active status, default password, assigned roles, the user appears in the system's user list, a conditional WhatsApp confirmation is sent, and flags are set. If a replacement occurred, the previous user with that specific role is deactivated.|The system state reflects the successful creation and/or replacement of a user.|
|\*\*Risk\*\*|1\. Data entry errors leading to incorrect user information.<br>2. Incorrect role assignment leading to unauthorized access.<br>3. WhatsApp message delivery failure.<br>4. Password security concerns.<br>5. Incorrect mobile number validation.<br>6. Complex UI logic for role selection.|Mitigation: Robust input validation, clear role descriptions, WhatsApp API monitoring, secure password generation and initial delivery, clear mobile number format guidance, thorough testing of dynamic UI.|
|\*\*Assumptions\*\*|1\. Core Identity in ASP.NET is used for user and permission management.<br>2. System Admin has full understanding of role implications.<br>3. WhatsApp Business API integration is available and configured.<br>4. Default password policy is defined (e.g., length, complexity).<br>5. The system handles password hashing internally.<br>6. The 'Registration Is Completed' flag will be updated by a separate process (e.g., user's first login).<br>7. The roles 'Legal Counsel', 'Finance Controller', 'Compliance and Legal Managing Director', 'Head of Real Estate' are considered "single-holder" roles for active users.|These assumptions simplify the initial implementation scope.|
|\*\*UX/UI Design Link\*\*|N/A (General form design principles, dynamic multi-select UI, confirmation dialog for replacement)|No specific mockups provided in the BRD for this section.|
### Process Flow Table

|**Step**|**Action Description**|**Actor**|**Related Message Codes**|**Notes**|
| :- | :- | :- | :- | :- |
|1|System Admin navigates to the "User Management" section and clicks the "Add New User" button.|System Admin|||
|2|System displays a form for new user data entry.|System|All fields are initially empty. Role selection defaults to single-select mode.||
|3|System Admin enters required and optional user details: Name, Email, Country Code, Mobile No., IBAN, Nationality, CV (upload), Passport No.|System Admin|MSG-ADD-001, MSG-ADD-002, MSG-ADD-003, MSG-ADD-010|Country Code and Mobile No. are mandatory and validated as Saudi numbers.|
|4|System Admin selects one or more roles for the new user from a predefined list of available roles.|System Admin|\*\*Conditional logic:\*\* Multi-select for roles is enabled \*\*ONLY IF\*\* the selected roles are ('Fund Manager' AND 'Board Member') OR ('Associate Fund Manager' AND 'Board Member'). Otherwise, multi-select is disabled, restricting to a single role.||
|5|System Admin reviews the entered information and clicks the "\*\*Create User\*\*" button.|System Admin|||
|6|System performs server-side validation on all submitted data.|System|MSG-ADD-001, MSG-ADD-002, MSG-ADD-003, MSG-ADD-010, MSG-ADD-011|Checks for mandatory fields, email format, uniqueness, role selection based on business rules, and Saudi mobile number format/uniqueness.|
|7|\*\*If validation is successful, System checks if any selected role is a "single-holder" role (Legal Counsel, Finance Controller, Compliance and Legal Managing Director, Head of Real Estate).\*\*|System|||
|8|\*\*If a single-holder role is selected AND an active user already holds ONLY that specific role:\*\* System displays a confirmation dialog asking to replace the existing user.|System|MSG-ADD-012|Dialog includes existing user's name and role. Buttons: "Replace", "Cancel".|
|9|\*\*If System Admin clicks "Replace" in the dialog:\*\* System deactivates the existing user with that role.|System Admin / System|The existing user's status is changed to 'Inactive'.||
|10|\*\*If System Admin clicks "Cancel" in the dialog:\*\* System returns the System Admin to the user creation form, allowing them to modify role selection or other details.|System Admin / System|This allows the admin to choose a non-conflicting role or adjust other inputs.||
|11|\*\*If System Admin proceeds (either no conflict, or after cancelling replacement and modifying input):\*\* System proceeds to create the new user account in the database with \*\*active status\*\*.|System|This step is reached if no conflict, or if conflict was cancelled and admin re-submits successfully.||
|12|System \*\*generates a user default password\*\* based on predefined policy.|System|This password will be used for the user's first login.||
|13|System stores the generated password securely (hashing handled internally).|System|||
|14|\*\*System sets the 'Registration Message Is Sent' flag:\*\*<br>   - To \*\*1\*\* if the user's assigned role(s) make them eligible for a WhatsApp message (i.e., NOT only 'Board Member').<br>   - To \*\*0\*\* if the user's assigned role(s) make them ineligible for a WhatsApp message (i.e., ONLY 'Board Member').|System|This flag tracks the user's eligibility for the WhatsApp notification based on their role(s).||
|15|\*\*System sets the 'Registration Is Completed' flag to 0 (False).\*\*|System|This flag defaults to 0 upon creation.||
|16|\*\*Conditional:\*\* If the added user has \*\*NOT\*\* only the 'Board Member' role, System \*\*attempts to send a WhatsApp message to the user to confirm their registration\*\*, including initial login instructions and a link to the login page.|System|MSG-ADD-008, MSG-ADD-009|Requires WhatsApp Business API integration. The outcome of this step does NOT affect the 'Registration Message Is Sent' flag.|
|17|System displays a success message.|System|MSG-ADD-005||
|18|System redirects the System Admin back to the "View System Users List" page, with the newly added user visible.|System|||
### Alternative Flow Table

|**Alternative Scenario**|**Condition**|**Action**|**Related Message Codes**|**Resolution**|
| :- | :- | :- | :- | :- |
|\*\*Missing Mandatory Field\*\*|System Admin attempts to save without filling a mandatory field (e.g., Name, Email, Country Code, Mobile No.).|System displays a specific validation error message next to the missing field.|MSG-ADD-001|System Admin must fill in all mandatory fields.|
|\*\*Invalid Email Format\*\*|System Admin enters an email address in an incorrect format.|System displays a validation error message for the email field.|MSG-ADD-002|System Admin must correct the email format.|
|\*\*Duplicate Email\*\*|System Admin enters an email address that already exists in the system.|System displays an error message indicating duplicate email.|MSG-ADD-003|System Admin must enter a unique email address.|
|\*\*Invalid Mobile No. Format (Non-Saudi)\*\*|System Admin enters a mobile number that does not conform to Saudi mobile number format (e.g., not starting with 05, incorrect length).|System displays a validation error message for the mobile number field.|MSG-ADD-010|System Admin must enter a valid Saudi mobile number.|
|\*\*Duplicate Mobile No. (Username)\*\*|System Admin enters a mobile number that already exists as a username in the system.|System displays an error message indicating the mobile number is already in use.|MSG-ADD-011|System Admin must enter a unique mobile number.|
|\*\*Invalid Role Selection (Logic Violation)\*\*|System Admin selects roles that violate the conditional logic (e.g., 'Fund Manager' and 'Accountant' simultaneously, or more than one role when multi-select is disabled).|System displays a warning/error message (e.g., "Required Field." as MSG-ADD-001).|MSG-ADD-001|System Admin must correct the role selection.|
|\*\*No Roles Selected\*\*|System Admin attempts to save a new user without assigning any roles.|System displays a warning/error message prompting to select at least one role.|MSG-ADD-001|System Admin must select at least one role for the user.|
|\*\*CV File Upload Error\*\*|An error occurs during the CV file upload (e.g., file too large, unsupported format).|System displays an error message related to the file upload.|MSG-ADD-006|System Admin must re-upload a valid CV file.|
|\*\*Role Replacement - Cancelled\*\*|System Admin selects a single-holder role that has an active user, and clicks "Cancel" in the replacement confirmation dialog.|System closes the dialog and returns the System Admin to the user creation form.|System Admin can modify input and re-attempt creation.||
|\*\*WhatsApp Message Failure (Non-Board Member Only)\*\*|The system fails to send the WhatsApp confirmation message for a user who is NOT only a Board Member.|System logs the failure and displays a warning to the System Admin. \*\*The 'Registration Message Is Sent' flag remains set based on role eligibility (Step 14).\*\*|MSG-ADD-009|System Admin may need to manually notify the user or verify mobile number.|
|\*\*System Error during Creation\*\*|A backend error occurs during user creation (e.g., database connection issue).|System displays a generic error message.|MSG-ADD-007|System Admin can retry the operation or contact support. \*\*The 'Registration Message Is Sent' flag is set to 0\*\* (as user creation itself failed), \*\*and 'Registration Is Completed' flag remains 0.\*\*|
|\*\*Cancel Creation\*\*|System Admin decides not to create the user.|System Admin clicks a "Cancel" button.|System discards all entered data and redirects back to the "View System Users List".||
### Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Successful User Creation (Non-Board Member Only) - WhatsApp Attempted\*\*|A System Admin is logged in and on the "Add New User" form.|The System Admin fills all mandatory fields (including valid Saudi Mobile No.), selects valid roles (e.g., only "Fund Manager"), and clicks "Create User".|The new user account is created with active status, the Mobile No. is set as username, a default password is generated and stored, a success message "Record Saved Successfully" is displayed, a WhatsApp confirmation message is attempted to be sent to the user including the login link, \*\*the 'Registration Message Is Sent' flag is set to 1\*\*, \*\*the 'Registration Is Completed' flag is set to 0\*\*, and the System Admin is redirected to the user list.|
|\*\*Successful User Creation (Board Member Only) - No WhatsApp Attempt\*\*|A System Admin is logged in and on the "Add New User" form.|The System Admin fills all mandatory fields (including valid Saudi Mobile No.), selects \*\*only\*\* the "Board Member" role, and clicks "Create User".|The new user account is created with active status, the Mobile No. is set as username, a default password is generated and stored, a success message "Record Saved Successfully" is displayed, \*\*NO WhatsApp confirmation message is attempted to be sent\*\*, \*\*the 'Registration Message Is Sent' flag is set to 0\*\* (as per role eligibility), \*\*the 'Registration Is Completed' flag is set to 0\*\*, and the System Admin is redirected to the user list.|
|\*\*Successful User Creation (Single-Holder Role, With Replacement)\*\*|A System Admin is logged in and on the "Add New User" form. An active user "John Doe" currently holds ONLY the "Legal Counsel" role.|The System Admin fills all mandatory fields, selects "Legal Counsel" for the new user, clicks "Create User", and then clicks "Replace" in the confirmation dialog.|The existing user "John Doe" is deactivated, the new user account is created with active status and "Legal Counsel" role, a success message "Record Saved Successfully" is displayed, and the System Admin is redirected to the user list.|
|\*\*Role Replacement - Cancelled (Admin Modifies Input)\*\*|A System Admin is on the "Add New User" form. An active user "John Doe" currently holds ONLY the "Legal Counsel" role.|The System Admin fills all mandatory fields, selects "Legal Counsel" for the new user, clicks "Create User", then clicks "Cancel" in the confirmation dialog, then changes the role to "Accountant" and clicks "Create User" again.|The new user account is created with the "Accountant" role, the existing user "John Doe" remains active, a success message "Record Saved Successfully" is displayed, and the System Admin is redirected to the user list.|
|\*\*Mandatory Field Validation\*\*|A System Admin is on the "Add New User" form.|The System Admin leaves the "Mobile No." field empty and clicks "Create User".|The system displays a specific validation error message "Required Field" for the "Mobile No." field, and the user account is not created.|
|\*\*Invalid Mobile No. Format\*\*|A System Admin is on the "Add New User" form.|The System Admin enters "0123456789" (non-Saudi format) into the Mobile No. field and clicks "Create User".|The system displays a validation error message "Invalid Saudi mobile number format. Please enter a 10-digit number starting with 05." for the mobile number field, and the user account is not created.|
|\*\*Duplicate Mobile No. (Username)\*\*|A System Admin is on the "Add New User" form.|The System Admin enters a Saudi mobile number that already exists as a username in the system and clicks "Create User".|The system displays an error message "Mobile number is already in use as a username.", and the user account is not created.|
|\*\*Conditional Role Selection - Valid (Fund Manager & Board Member)\*\*|A System Admin is on the "Add New User" form.|The System Admin selects "Fund Manager" and "Board Member" roles, and clicks "Create User".|The system successfully creates the user with both roles assigned.|
|\*\*Conditional Role Selection - Valid (Associate Fund Manager & Board Member)\*\*|A System Admin is on the "Add New User" form.|The System Admin selects "Associate Fund Manager" and "Board Member" roles, and clicks "Create User".|The system successfully creates the user with both roles assigned.|
|\*\*Conditional Role Selection - Invalid (Multi-select Disabled)\*\*|A System Admin is on the "Add New User" form.|The System Admin selects "Legal Counsel" and then attempts to select "Accountant" (when multi-select is disabled).|The system prevents the selection of the second role, or displays a generic validation error message "Required Field" if the selection is invalid, and the user account is not created.|
|\*\*WhatsApp Failure (Flag Remains Based on Eligibility)\*\*|A new user is created with the role "Fund Manager", but WhatsApp message sending fails.|The system completes the user creation process.|A warning message "Failed to send WhatsApp confirmation message. Please notify the user manually." is displayed, and the 'Registration Message Is Sent' flag remains set to 1 (because the user was eligible to receive it), and the 'Registration Is Completed' flag is set to 0.|
|\*\*System Error during Save (Flags Set to 0)\*\*|A System Admin is on the "Add New User" form and clicks "Create User".|An unexpected backend error occurs during the saving process.|The system displays the message "An error is occurred while saving data" and the user account is not created, and both the 'Registration Message Is Sent' flag and 'Registration Is Completed' flag are set to 0.|
|\*\*Cancel User Creation\*\*|A System Admin is on the "Add New User" form.|The System Admin clicks the "Cancel" button.|The system discards all entered data and redirects the System Admin back to the "View System Users List" page.|
### Data Entities Table
Entity Name: User (for adding)

|**Attribute (English)**|**Attribute (Arabic)**|**Mandatory/Optional**|**Attribute Type**|**Data Length**|**Integration Requirements**|**Default Value**|**Condition (if needed)**|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|User ID|معرف المستخدم|Mandatory|Number (Auto-generated)|N/A|Database Primary Key|N/A|N/A|Unique identifier|12345|12345|
|Name|الاسم|Mandatory|Text|Max 255|Database Field|N/A|N/A|N/A|سارة علي|Sara Ali|
|Email|البريد الإلكتروني|Mandatory|Text|Max 255|Database Field|N/A|N/A|Unique, Valid email format|sara.ali@jadwa.com|sara.ali@jadwa.com|
|Country Code|رمز الدولة|Mandatory|Text|Max 5|Database Field|+966|N/A|Must be '+966' for Saudi numbers.|+966|+966|
|Mobile|رقم الجوال|Mandatory|Text|Max 10|Database Field|N/A|N/A|Saudi mobile format (e.g., 05XXXXXXXX), Numeric only, Unique (as username).|0501234567|0501234567|
|IBAN|رقم الحساب المصرفي الدولي|Optional|Text|Max 34|Database Field|N/A|N/A|Valid IBAN format|SA9876543210987654321098|SA9876543210987654321098|
|Nationality|الجنسية|Optional|Text|Max 100|Database Field|N/A|N/A|N/A|مصري|Egyptian|
|CV|السيرة الذاتية|Optional|File Upload|N/A|File Storage (e.g., Azure Blob)|N/A|N/A|PDF, DOCX only, Max 10MB|سيرة\_ذاتية\_سارة.pdf|Sara\_CV.pdf|
|Passport No.|رقم جواز السفر|Optional|Text|Max 20|Database Field|N/A|N/A|Alphanumeric|B98765432|B98765432|
|Status|الحالة|Mandatory|Boolean/Dropdown|N/A|Database Field|Active|N/A|Active/Inactive|نشط|Active|
|Role|الدور|Mandatory|Relation to Role Entity|N/A|Database Many-to-Many|N/A|At least one role must be selected, conditional logic applies|Predefined roles only|مستشار قانوني|Legal Counsel|
|Password|كلمة المرور|Mandatory|Text|N/A|Database Field|N/A|N/A|System-generated, stored securely (hashing handled internally).|N/A|N/A|
|Last Update Date|تاريخ آخر تحديث|Mandatory|DateTime|N/A|Database Field|Current Timestamp|N/A|Automatically set on creation and update.|2023-10-26 15:00:00|2023-10-26 15:00:00|
|Registration Message Is Sent|تم إرسال رسالة التسجيل|Mandatory|Boolean|N/A|Database Field|0 (False)|N/A|Set to 1 if user's role(s) make them eligible for WhatsApp message (NOT only 'Board Member'); 0 if only 'Board Member'. This flag is set based on eligibility and attempt to send.|1|True|
|Registration Is Completed|تم اكتمال التسجيل|Mandatory|Boolean|N/A|Database Field|0 (False)|N/A|Set to 0 upon user creation. Updated by a separate process (e.g., user's first login).|0|False|

Entity Name: Role (for selection)

|**Attribute (English)**|**Attribute (Arabic)**|**Mandatory/Optional**|**Attribute Type**|**Data Length**|**Integration Requirements**|**Default Value**|**Condition (if needed)**|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|Role ID|معرف الدور|Mandatory|Number (Auto-generated)|N/A|Database Primary Key|N/A|N/A|Unique identifier|1|1|
|Role Name|اسم الدور|Mandatory|Text|Max 50|Database Field|N/A|N/A|Unique|مدير النظام|System Admin|
### Messages/Notifications Table

|**Message Code**|**Message (English)**|**Message (Arabic)**|**Message Type**|**Communication Method**|
| :- | :- | :- | :- | :- |
|MSG-ADD-001|Required Field.|حقل إلزامي.|Validation Error|In-App|
|MSG-ADD-002|Invalid email format.|صيغة البريد الإلكتروني غير صحيحة.|Validation Error|In-App|
|MSG-ADD-003|User with this email already exists.|يوجد مستخدم بهذا البريد الإلكتروني بالفعل.|Validation Error|In-App|
|MSG-ADD-005|Record Saved Successfully.|تم حفظ البيانات بنجاح.|Success Message|In-App|
|MSG-ADD-006|Invalid file format or size for CV. Please upload a PDF or DOCX file up to 10MB.|صيغة الملف أو حجمه غير صالح للسيرة الذاتية. يرجى تحميل ملف PDF أو DOCX بحجم أقصى 10 ميجابايت.|Validation Error|In-App|
|MSG-ADD-007|An error is occurred while saving data.|حدث خطأ بالنظام , لم يتم حفظ البيانات.|Error Message|In-App|
|MSG-ADD-008|Your registration for Jadwa Fund Board Management is complete. Your temporary password is: [Password]. Please log in through this link [login URL].|تم تسجيلك في تطبيق إدارة مجالس صناديق جدوى. كلمة المرور المؤقتة هي: [كلمة المرور]. يرجى تسجيل الدخول من خلال هذا الرابط [رابط تسجيل الدخول].|Success Message|WhatsApp|
|MSG-ADD-009|Failed to send WhatsApp confirmation message. Please notify the user manually.|فشل إرسال رسالة تأكيد الواتساب. يرجى إخطار المستخدم يدوياً.|Warning Message|In-App / System Log|
|MSG-ADD-010|Invalid Saudi mobile number format. Please enter a 10-digit number starting with 05.|صيغة رقم الجوال السعودي غير صالحة. يرجى إدخال رقم مكون من 10 أرقام يبدأ بـ 05.|Validation Error|In-App|
|MSG-ADD-011|Mobile number is already in use as a username.|رقم الجوال مستخدم بالفعل كاسم مستخدم.|Validation Error|In-App|
|MSG-ADD-012|There is another active user with the role [Role Name]: [Existing User Name]. Do you want to replace him?|يوجد مستخدم نشط آخر بالدور [اسم الدور]: [اسم المستخدم الحالي]. هل تريد استبداله؟|Confirmation Dialog|In-App|
### Screen Elements Table

|**Element ID**|**Element Type**|**Element Name (English)**|**Element Name (Arabic)**|**Required/Optional**|**Validation Rules**|**Business Logic**|**Related Data Entity**|**User Interaction**|**Accessibility Notes**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|ELM-ADD-001|Page Title|Add New User|إضافة مستخدم جديد|N/A|N/A|Displays the title of the user creation form.|N/A|View|H1 heading, clear and concise.|
|ELM-ADD-002|Input Field|Name|حقل الاسم|Mandatory|Text, Max 255 chars|Collects the user's full name.|User.Name|Type|Clear label, placeholder text.|
|ELM-ADD-003|Input Field|Email|حقل البريد الإلكتروني|Mandatory|Valid email format, Max 255 chars|Collects the user's email address. Must be unique.|User.Email|Type|Clear label, placeholder text, email type keyboard.|
|ELM-ADD-004|Input Field|Country Code|حقل رمز الدولة|Mandatory|Text, Must be '+966'|Collects the country dialing code for Saudi mobile number.|User.CountryCode|Type (or Pre-filled/Dropdown)|Clear label, placeholder text.|
|ELM-ADD-005|Input Field|Mobile No. (Username)|حقل رقم الجوال (اسم المستخدم)|Mandatory|Numeric only, 10 digits, starts with '05', Unique.|Collects the user's Saudi mobile phone number, which serves as their username.|User.Mobile|Type|Clear label, placeholder text, numeric keyboard.|
|ELM-ADD-006|Input Field|IBAN|حقل رقم الحساب المصرفي الدولي|Optional|Valid IBAN format, Max 34 chars|Collects the user's International Bank Account Number.|User.IBAN|Type|Clear label, placeholder text.|
|ELM-ADD-007|Input Field|Nationality|حقل الجنسية|Optional|Text, Max 100 chars|Collects the user's nationality.|User.Nationality|Type|Clear label, placeholder text.|
|ELM-ADD-008|File Upload|CV Upload|تحميل السيرة الذاتية|Optional|PDF/DOCX only, Max 10MB|Allows user to upload their CV file.|User.CV|Click (to browse/upload)|Clear label, file type/size guidance.|
|ELM-ADD-009|Input Field|Passport No.|حقل رقم جواز السفر|Optional|Alphanumeric, Max 20 chars|Collects the user's passport number.|User.PassportNo|Type|Clear label, placeholder text.|
|ELM-ADD-010|Dropdown/Checkboxes|Role Selection|اختيار الدور|Mandatory|At least one role selected, \*\*conditional logic applies\*\*|Allows selection of one or more predefined roles for the user. \*\*Multi-select enabled/disabled dynamically.\*\*|User.Role|Select|Clear label, accessible options, visual feedback for multi-select state.|
|ELM-ADD-011|Button|Create User Button|زر إنشاء المستخدم|Mandatory|N/A|Submits the form to create the new user.|N/A|Click|Primary action button, clearly labeled.|
|ELM-ADD-012|Button|Cancel Button|زر إلغاء|Mandatory|N/A|Discards changes and returns to the user list.|N/A|Click|Secondary action button, clearly labeled.|
|ELM-ADD-013|Text Label|Validation Error Message|رسالة خطأ التحقق|Conditional|N/A|Displays specific validation errors next to relevant fields.|N/A|View|Red text, clear indication of error.|
|ELM-ADD-014|Text Label|Success Message|رسالة النجاح|Conditional|N/A|Displays confirmation of successful user creation.|N/A|View|Green text, prominent display.|
|ELM-ADD-015|Text Label|Generic Error Message|رسالة خطأ عامة|Conditional|N/A|Displays unexpected system errors.|N/A|View|Red text, prominent display.|
|ELM-ADD-016|Confirmation Dialog|Role Replacement Confirmation|تأكيد استبدال الدور|Conditional|N/A|Prompts System Admin to confirm replacing an existing user with a single-holder role.|N/A|View, Click (Replace/Cancel)|Clear message, accessible buttons.|
|ELM-ADD-017|Button|Replace Button (in Dialog)|زر استبدال (في النافذة)|Mandatory|N/A|Confirms replacement of existing user and proceeds with new user creation.|N/A|Click|Clearly labeled, primary action in dialog.|
|ELM-ADD-018|Button|Cancel Button (in Dialog)|زر إلغاء (في النافذة)|Mandatory|N/A|Cancels the replacement and aborts new user creation.|N/A|Click|Clearly labeled.|
### Summary Section
This rewritten user story for "Add New System User" now has the word "Input" removed from the "Element Name (English)" column in the Screen Elements Table, providing a more concise and consistent naming convention for UI elements.

\*\*Key Implementation Considerations:\*\*

\*   \*\*Critical:\*\* Implement the precise logic for enabling/disabling multi-select based on the new, more complex role combination condition.

\*   The `Registration Message Is Sent` flag logic remains as last defined (set to 1 if eligible, 0 if only Board Member, regardless of delivery success).

\*   Maintain robust validation for Saudi mobile numbers (format and uniqueness) as this is the username.

\*\*Dependencies and Risks:\*\*

\*   \*\*Dependency:\*\* This story depends on the "View System Users List" story for navigation. It also depends on the availability and configuration of the WhatsApp Business API.

\*   \*\*Risk:\*\* Increased complexity of role selection UI/logic could lead to bugs or user confusion. Mitigation: Thorough testing of dynamic UI and validation.

\---

I have now updated the "Add New System User" user story with the requested changes to the Screen Elements Table.

\*\*Important Note:\*\* I am still awaiting your clarification on the "Action Not Applicable (Role/Assignment)" logic for the "Resend Account Registration Message" user story from our previous interaction. The current logic in that story (and reflected in its Acceptance Criteria) is: "Action is NOT applicable if the user has ONLY the 'Board Member' role AND is NOT assigned to any fund board." However, with `IsAssignedToFundBoard` removed from the User entity, this specific condition cannot be implemented as written. \*\*Please confirm how the "Resend Message" eligibility for Board Members should be determined without `IsAssignedToFundBoard`.\*\*

Would you like me to generate the user stories for "Activate System User" or "Reset User Password" next, or would you prefer to refine this one further, or select another from the original list?
# User Story JDWA-1217: Filter System Users List
Introduction Section

This user story describes the functionality for System Administrators to apply filters to the displayed list of system users. This allows administrators to narrow down the list based on specific criteria such as Name, Status, or Mobile No., making it easier to locate and manage particular user accounts within the Fund Board Management Application. The default view of the user list will now be active users only.

Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|Filter System Users List|Allows System Administrators to filter the list of system users based on various criteria.|
|\*\*User Story\*\*|As a \*\*System Administrator\*\*, I want to \*\*filter the list of system users by various criteria\*\* so that I can \*\*quickly find and focus on specific user accounts.\*\*||
|\*\*Story Points\*\*|3|Involves applying filter logic to an existing list.|
|\*\*User Roles\*\*|System Admin|Only System Administrators can filter the user list.|
|\*\*Access Requirements\*\*|System Admin authentication and authorization.|User must be logged in as a System Admin and viewing the user list.|
|\*\*Trigger\*\*|System Admin interacts with filter controls (e.g., types in a search box, selects from a dropdown).|User initiates the action from the system's administrative interface.|
|\*\*Frequency of Use\*\*|High|Admins frequently use filters to manage large user bases.|
|\*\*Pre-condition\*\*|System Admin is logged in and viewing the "View System Users List" (which now defaults to active users).|The user list is displayed and contains data.|
|\*\*Business Rules\*\*|1\. \*\*Default view of the user list is 'Active' users only.\*\*<br>2. Mobile No. filter applies with press enter button.<br>3. Other filters (Name, Status, Role) are applied via a filter popup.<br>4. Multiple filters can be applied simultaneously (AND logic).<br>5. Text-based filters (Name, Mobile No.) should support partial matches.|Ensures efficient and flexible filtering capabilities with a focused default view and hybrid interaction.|
|\*\*Post-condition\*\*|The displayed user list is updated to show only users matching the applied filter criteria.|The system state reflects the filtered view of user data.|
|\*\*Risk\*\*|1\. Performance degradation with complex filters on large datasets.<br>2. Inaccurate filtering results due to incorrect logic.|Mitigation: Optimized database queries, thorough testing of filter logic.|
|\*\*Assumptions\*\*|1\. The underlying data structure supports efficient filtering by specified attributes.<br>2. The UI provides intuitive filter controls.|These assumptions are necessary for effective filtering.|
|\*\*UX/UI Design Link\*\*|N/A (Refer to general filtering UI patterns)|Specific UI mockups not provided in BRD for this section.|

Process Flow Table

|**Step**|**Action Description**|**Actor**|**Related Message Codes**|**Notes**|
| :- | :- | :- | :- | :- |
|1|System Admin is viewing the "View System Users List" (which defaults to active users).|System Admin|||
|2|System Admin interacts with the "Mobile No." filter control (types into the search box), then press enter button.|System Admin|||
|3|System applies the "Mobile No." filter criteria to the displayed user list.|System|||
|4|System updates the displayed list to show only users matching the "Mobile No." filter criteria.|System|If no users match, display "No records exist to display".||
|5|System Admin can now click on a "Filter" button or icon on the user list page to access other filter options.|System Admin|This action triggers the filter popup for Name, Status, Role.||
|6|System displays a filter popup/modal containing filter controls for Name, Status, and Role.|System|||
|7|System Admin interacts with filter controls within the popup (e.g., types text into "Name" search box, selects "Inactive" from "Status" dropdown, selects "Fund Manager" from "Role" dropdown).|System Admin|||
|8|System Admin clicks an "Apply Filters" button within the popup.|System Admin|||
|9|System closes the filter popup.|System|||
|10|System applies the combined filter criteria (Mobile No. + popup filters) to the displayed user list.|System|||
|11|System updates the displayed list to show only users matching the combined filter criteria.|System|If no users match, display "No records exist to display".||
|12|System Admin can click on the "Filter" button again to modify existing filters or apply additional filters.|System Admin|This loops back to Step 6.||
|13|System Admin clicks a "Clear Filters" button within the popup (if available) or removes individual filter selections.|System Admin|||
|14|System reverts the user list to its default state (showing only active users).|System|This clears all filters, including Mobile No.||
Alternative Flow Table

|**Alternative Scenario**|**Condition**|**Action**|**Related Message Codes**|**Resolution**|
| :- | :- | :- | :- | :- |
|\*\*No Matching Users\*\*|System Admin applies filters, but no users match the criteria.|System displays a "No records exist to display" message.|MSG-FILT-001|System Admin can modify or clear filters.|
|\*\*Invalid Filter Input\*\*|System Admin enters invalid characters into a text filter (e.g., non-numeric in Mobile No. field).|System may ignore invalid characters or display a warning.|MSG-FILT-002|System Admin corrects input.|
|\*\*Performance Lag\*\*|Applying complex filters on large datasets causes a noticeable delay.|System may display a loading indicator.|Optimization of database queries and indexing.||
|\*\*System Error during Filtering\*\*|A backend error occurs during the filtering operation.|System displays a generic error message.|MSG-FILT-003|System Admin can refresh the page or try filtering again.|
|\*\*Filter Popup Closed Without Applying\*\*|System Admin opens the filter popup but closes it without clicking "Apply Filters".|System closes the popup and the user list remains unchanged (only Mobile No. filter, if applied, persists).|System Admin can reopen the popup later.||
Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Filter by Name - Success\*\*|A System Admin is viewing the user list.|The System Admin opens the filter popup, types "Moham" into the Name filter, and clicks "Apply Filters".|The system displays only "Mohammed Ahmed" in the user list.|
|\*\*Filter by Role - Success\*\*|A System Admin is viewing the user list.|The System Admin opens the filter popup, selects "Legal Counsel" from the Role filter dropdown, and clicks "Apply Filters".|The system displays only users assigned the "Legal Counsel" role.|
|\*\*Filter by Status - Success\*\*|A System Admin is viewing the user list which defaults to active users.|The System Admin opens the filter popup, selects "Inactive" from the Status filter, and clicks "Apply Filters".|The system displays only inactive users.|
|\*\*Filter by Mobile No. - Success\*\*|A System Admin is viewing the user list containing users with various mobile numbers.|The System Admin types "50123" into the Mobile No. filter search box .|The system immediately displays only users whose mobile number contains "50123".|
|\*\*Multiple Filters (Hybrid) - Success\*\*|A System Admin is viewing the user list.|The System Admin types "50123" into the Mobile No. filter, then opens the filter popup, types "Sara" into the Name filter, and clicks "Apply Filters".|The system displays only active users whose mobile number contains "50123" AND whose name contains "Sara".|
|\*\*No Results Found\*\*|A System Admin is viewing the user list.|The System Admin applies a filter combination (e.g., Name: "XYZ", Role: "Admin") for which no users exist.|The system displays the message "No records exist to display".|
|\*\*Clear Filters\*\*|A System Admin has applied one or more filters to the user list (including Mobile No. and/or popup filters).|The System Admin opens the filter popup and clicks the "Clear Filters" button.|The user list reverts to its default state, displaying only active users, and all filter inputs are cleared.|
|\*\*System Error during Filtering\*\*|A System Admin attempts to apply a filter.|An unexpected error occurs during the filtering process.|The system displays the message "An error is occurred, can’t display data" and the filter may not apply or the list becomes unresponsive.|

Data Entities Table

Entity Name: User (for filtering)

|**Attribute (English)**|**Attribute (Arabic)**|**Mandatory/Optional**|**Attribute Type**|**Data Length**|**Integration Requirements**|**Default Value**|**Condition (if needed)**|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|Name|الاسم|Mandatory|Text|Max 255|Database Field|N/A|N/A|N/A|محمد أحمد|Mohammed Ahmed|
|Email|البريد الإلكتروني|Mandatory|Text|Max 255|Database Field|N/A|N/A|Unique, Valid email format|mohammed.ahmed@jadwa.com|mohammed.ahmed@jadwa.com|
|Status|الحالة|Mandatory|Boolean/Dropdown|N/A|Database Field|Active|N/A|Active/Inactive|نشط|Active|
|Role|الدور|Mandatory|Relation to Role Entity|N/A|Database Many-to-Many|N/A|At least one role must be selected|Predefined roles only|مدير صندوق، عضو مجلس|Fund Manager, Board Member|
|Mobile|رقم الجوال|Optional|Text|Max 15|Database Field|N/A|N/A|Numeric only|501234567|501234567|

Messages/Notifications Table

|**Message Code**|**Message (English)**|**Message (Arabic)**|**Message Type**|**Communication Method**|
| :- | :- | :- | :- | :- |
|MSG-FILT-001|No records exist to display.|عفوا, لا توجد بيانات مسجلة لعرضها.|Information|In-App|
|MSG-FILT-002|Invalid filter input. Please check your entry.|إدخال غير صالح في الفلتر. يرجى التحقق من إدخالك.|Validation Error|In-App|
|MSG-FILT-003|An error is occurred, can’t display data.|حدث خطأ بالنظام , لم يتمكن من عرض البيانات.|Error Message|In-App|

Screen Elements Table

|**Element ID**|**Element Type**|**Element Name (English)**|**Element Name (Arabic)**|**Required/Optional**|**Validation Rules**|**Business Logic**|**Related Data Entity**|**User Interaction**|**Accessibility Notes**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|ELM-FILT-001|Input Field|Mobile No. Filter Input|حقل تصفية رقم الجوال (مباشر)|Optional|Numeric only|Filters the user list by user mobile number (partial match)|User.Mobile|Type|Clear label, placeholder text.|
|ELM-FILT-002|Button/Icon|Filter Button (Opens Popup)|زر/أيقونة التصفية (يفتح نافذة منبثقة)|Mandatory|N/A|Opens the filter popup for Name, Status, and Role filters.|N/A|Click|Clear icon (e.g., funnel), accessible label.|
|ELM-FILT-003|Modal/Popup|Filter Popup|نافذة/منبثقة التصفية|Mandatory|N/A|Contains filter controls for Name, Status, and Role.|N/A|View|Accessible modal, focus management.|
|ELM-FILT-004|Input Field|Name Filter Input (in Popup)|حقل تصفية الاسم (في النافذة المنبثقة)|Optional|Text, Alphanumeric|Filters the user list by user name (partial match).|User.Name|Type|Clear label, placeholder text.|
|ELM-FILT-005|Dropdown|Status Filter Dropdown (in Popup)|قائمة تصفية الحالة (في النافذة المنبثقة)|Optional|Predefined values: Active, Inactive, All|Filters the user list by user status.|User.Status|Select|Clear label, accessible options.|
|ELM-FILT-006|Dropdown|Role Filter Dropdown (in Popup)|قائمة تصفية الدور (في النافذة المنبثقة)|Optional|Predefined values: System Admin, Fund Manager, etc.|Filters the user list by assigned role.|User.Role|Select|Clear label, accessible options.|
|ELM-FILT-007|Button|Apply Filters Button (in Popup)|زر تطبيق الفلاتر (في النافذة المنبثقة)|Mandatory|N/A|Applies all selected filter criteria from the popup to the user list and closes the popup.|N/A|Click|Clearly labeled, primary action within popup.|
|ELM-FILT-008|Button|Clear Filters Button (in Popup)|زر مسح الفلاتر (في النافذة المنبثقة)|Optional|N/A|Resets all filter criteria within the popup and clears the Mobile No. filter.|N/A|Click|Clearly labeled.|
|ELM-FILT-009|Button/Icon|Close Filter Popup Button|زر/أيقونة إغلاق نافذة التصفية|Mandatory|N/A|Closes the filter popup without applying changes.|N/A|Click|Standard close icon (e.g., 'X').|
|ELM-FILT-010|Text Label|Filter Results Message|رسالة نتائج الفلتر|Conditional|N/A|Displays messages like "No records exist to display" when filters yield no results.|N/A|View|Clear and informative.|

Summary Section

This user story for "Filter System Users List" has been significantly updated to implement a hybrid filtering approach. The "Mobile No." filter now operates in real-time, while other filters (Name, Status, Role) are managed through a dedicated filter popup. The process flow and screen elements have been meticulously re-sequenced and detailed to reflect this new interaction model, ensuring a precise understanding of this complex functionality.

\*\*Key Implementation Considerations:\*\*

\*   The "Clear Filters" button in the popup should clear \*all\* filters, including the Mobile No. filter, and revert the list to its default (active users only) state.

\*   Ensure the filter popup is a modal and handles focus correctly for accessibility.

\*   Implement efficient database queries for filtering, especially with large datasets and partial matches.

\*\*Dependencies and Risks:\*\*

\*   \*\*Dependency:\*\* This story depends on the "View System Users List" story to provide the base list to be filtered.

\*   \*\*Risk:\*\* User confusion due to two different filtering mechanisms. Mitigation: Clear UI design, intuitive placement of filter controls, and consistent behavior.

\---

I have now updated the "Filter System Users List" user story with the revised Process Flow and Screen Elements Table, incorporating the hybrid filtering mechanism.

Would you like me to generate the user stories for "Activate System User" or "Delete System User" next, or would you prefer to refine this one further, or select another from the original list?

## User Story JDWA-1213: View System Users List
### Introduction Section
This user story outlines the capability for System Administrators to access and review a comprehensive list of all registered users within the Fund Board Management Application. This list serves as the primary entry point for initiating various user management operations, such as viewing detailed profiles, editing user information, managing user status, \*\*resetting passwords, and resending registration messages.\*\* The Screen Elements Table provides a detailed breakdown of the UI components on this list view.
### Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|View System Users List|Enables System Administrators to view a list of users and navigate to specific management actions.|
|\*\*User Story\*\*|As a \*\*System Administrator\*\*, I need to \*\*see a list of all system users with options to manage them\*\* so that I can \*\*efficiently oversee and access individual user administration tasks.\*\*||
|\*\*Story Points\*\*|4|Focuses on display and navigation, now with more complex action button visibility.|
|\*\*User Roles\*\*|System Admin|Exclusive access for System Administrators.|
|\*\*Access Requirements\*\*|Authenticated and authorized System Admin access.|User must be logged in with System Admin privileges.|
|\*\*Trigger\*\*|System Admin selects "User Management" from the main navigation.|User initiates navigation to the user list.|
|\*\*Frequency of Use\*\*|High|Regular access for user oversight and task initiation.|
|\*\*Pre-condition\*\*|System Admin is logged in and the User Management module is accessible.|The system is operational and the admin has necessary permissions.|
|\*\*Business Rules\*\*|1\. The list must display both active and inactive users by default.<br>2. Each user entry must include direct links or buttons for "View", "Edit", "Deactivate", "Activate", "Reset Password", and "Resend Message" actions.<br>3. The "Last Update Date" column must be sortable in ascending and descending order.<br>\*\*4. Button Visibility/Enablement for Actions:\*\*<br>   a. \*\*View, Edit:\*\* Always visible and enabled.<br>   b. \*\*Deactivate:\*\* Visible and enabled only if the user's current Status is 'Active'.<br>   c. \*\*Activate:\*\* Visible and enabled only if the user's current Status is 'Inactive'.<br>   d. \*\*Reset Password:\*\* Visible and enabled only if the user's Status is 'Active' AND their 'Registration Is Completed' flag is 1 AND their 'Registration Message Is Sent' flag is 1.<br>   e. \*\*Resend Message:\*\* Visible and enabled only if the user is 'Active', AND their 'Registration Is Completed' flag is 0, AND their 'Registration Message Is Sent' flag is 1.|Ensures comprehensive visibility, direct access to management functions, data organization, and precise action button visibility.|
|\*\*Post-condition\*\*|A paginated list of all system users is displayed, with actionable navigation controls for each user, and the list can be sorted by "Last Update Date".|The system presents the user data with interactive management options and sorting capability.|
|\*\*Risk\*\*|1\. Performance degradation with large user datasets.<br>2. Ambiguous navigation leading to user errors.<br>3. Incorrect button visibility logic.|Mitigation: Implement efficient pagination and clear UI/UX for action buttons, thorough testing of visibility conditions.|
|\*\*Assumptions\*\*|1\. User data is consistently stored and retrievable.<br>2. Dedicated user stories exist for each linked action (View, Edit, Deactivate, Activate, Reset Password, Resend Message).|Fundamental assumptions for list display and action linking.|
|\*\*UX/UI Design Link\*\*|N/A (General list/table patterns with action columns)|No specific mockups provided in the BRD for this section.|
### Process Flow Table

|**Step**|**Action Description**|**Actor**|**Related Message Codes**|**Notes**|
| :- | :- | :- | :- | :- |
|1|System Admin logs into the application.|System Admin|||
|2|System Admin navigates to the "User Management" section.|System Admin|||
|3|System retrieves all active user accounts from the database not including system admin data.|System|||
|4|System displays a paginated table listing users by Name, Status, assigned Role, Last Update Date, and an "Actions" column.|System|Default sorting (e.g., by Last Update date) is applied.||
|5|System Admin reviews the user list and can navigate through pages.|System Admin|||
|6|System Admin clicks on the "Last Update Date" column header.|System Admin|||
|7|System re-sorts the user list by "Last Update Date" in ascending or descending order (toggles on click).|System|||
|8|System Admin can click on any of the actionable buttons/links in the "Actions" column (View, Edit, Deactivate, Activate, Reset Password, Resend Message) to initiate a specific user management task.|System Admin|The specific actions and their eligibility are detailed in the "Actionable Buttons Table" below.||
### Alternative Flow Table

|**Alternative Scenario**|**Condition**|**Action**|**Related Message Codes**|**Resolution**|
| :- | :- | :- | :- | :- |
|\*\*No Users Registered\*\*|The system database contains no user accounts.|System displays a message indicating no records exist.|MSG-LIST-001|System Admin can proceed to add new users.|
|\*\*Unknown Display Error\*\*|An unexpected error occurs during the rendering or display of user data after retrieval.|System displays a generic error message.|MSG-LIST-002|System Admin can refresh the page or contact support.|
|\*\*Action Button Hidden\*\*|A specific action button (e.g., "Reset Password") is not applicable for a user (e.g., user is inactive).|The corresponding action button/link is \*\*hidden\*\*, preventing the action.|System Admin understands the action is not currently available.||
|\*\*Sorting Error\*\*|An error occurs during the sorting operation.|System displays a generic error message.|MSG-LIST-002|System Admin can refresh the page or try sorting again.|
### Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Display All Users\*\*|A System Admin is logged in and accesses the "User Management" section.|The system has a mix of active and inactive users registered.|The system displays a paginated list showing all registered users, including their Name, Status, assigned Role, and Last Update Date, with an "Actions" column containing clickable options as defined in the "Actionable Buttons Table".|
|\*\*Action Button Visibility (General)\*\*|A System Admin is viewing the user list.|A user row is displayed.|The "Actions" column for that user row correctly displays enabled/disabled/hidden buttons for "View", "Edit", "Deactivate", "Activate", "Reset Password", and "Resend Message" based on the user's status and eligibility criteria.|
|\*\*Action Button Visibility (Reset Password Eligible)\*\*|A System Admin is viewing the user list. A user is 'Active', 'Registration Is Completed' is 1, and 'Registration Message Is Sent' is 1.|The user row is displayed.|The "Reset Password" button is visible and enabled for this user.|
|\*\*Action Button Visibility (Resend Message Eligible)\*\*|A System Admin is viewing the user list. A user is 'Active', 'Registration Is Completed' is 0, and 'Registration Message Is Sent' is 1.|The user row is displayed.|The "Resend Message" button is visible and enabled for this user.|
|\*\*Unknown Display Error\*\*|A System Admin is logged in and attempts to access the "User Management" section.|An unexpected error occurs during the display of user data (e.g., UI rendering issue, data parsing error).|The system displays the message "An error is occurred, can’t display data" and the user list is not displayed or is incomplete.|
|\*\*Sort by Last Update Date - Ascending\*\*|A System Admin is viewing the user list with multiple users having different "Last Update Dates".|The System Admin clicks the "Last Update Date" column header for the first time (or to set ascending sort).|The user list is re-sorted to display users with the oldest "Last Update Date" first.|
### Data Entities Table
Entity Name: User (for listing and navigation)

|**Attribute (English)**|**Attribute (Arabic)**|**Mandatory/Optional**|**Attribute Type**|**Data Length**|**Integration Requirements**|**Default Value**|**Condition (if needed)**|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|User ID|معرف المستخدم|Mandatory|Number (Auto-generated)|N/A|Database Primary Key|N/A|N/A|Unique identifier|12345|12345|
|Name|الاسم|Mandatory|Text|Max 255|Database Field|N/A|N/A|N/A|محمد أحمد|Mohammed Ahmed|
|Status|الحالة|Mandatory|Boolean/Dropdown|N/A|Database Field|Active|N/A|Active/Inactive|نشط|Active|
|Role|الدور|Mandatory|Relation to Role Entity|N/A|Database Many-to-Many|N/A|At least one role must be selected, conditional logic applies|Predefined roles only|مدير صندوق، عضو مجلس|Fund Manager, Board Member|
|Last Update Date|تاريخ آخر تحديث|Mandatory|DateTime|N/A|Database Field|Current Timestamp|N/A|Automatically set on creation and update.|2023-10-26 15:00:00|2023-10-26 15:00:00|
|Registration Message Is Sent|تم إرسال رسالة التسجيل|Mandatory|Boolean|N/A|Database Field|0 (False)|N/A|Set to 1 if user's role(s) make them eligible for WhatsApp message (NOT only 'Board Member'); 0 if only 'Board Member'.|1|True|
|Registration Is Completed|تم اكتمال التسجيل|Mandatory|Boolean|N/A|Database Field|0 (False)|N/A|Set to 0 upon user creation. Updated by a separate process (e.g., user's first login).|0|False|

Entity Name: Role (for listing purposes)

|**Attribute (English)**|**Attribute (Arabic)**|**Mandatory/Optional**|**Attribute Type**|**Data Length**|**Integration Requirements**|**Default Value**|**Condition (if needed)**|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|Role ID|معرف الدور|Mandatory|Number (Auto-generated)|N/A|Database Primary Key|N/A|N/A|Unique identifier|1|1|
|Role Name|اسم الدور|Mandatory|Text|Max 50|Database Field|N/A|N/A|Unique|مدير النظام|System Admin|
### Messages/Notifications Table

|**Message Code**|**Message (English)**|**Message (Arabic)**|**Message Type**|**Communication Method**|
| :- | :- | :- | :- | :- |
|MSG-LIST-001|No records exist to display.|عفوا, لا توجد بيانات مسجلة لعرضها.|Information|In-App|
|MSG-LIST-002|An error is occurred, can’t display data.|حدث خطأ بالنظام , لم يتمكن من عرض البيانات.|Error Message|In-App|
### Screen Elements Table

|**Element ID**|**Element Type**|**Element Name (English)**|**Element Name (Arabic)**|**Required/Optional**|**Validation Rules**|**Business Logic**|**Related Data Entity**|**User Interaction**|**Accessibility Notes**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|ELM-LIST-001|Page Title|User Management|إدارة المستخدمين|N/A|N/A|Displays the title of the page.|N/A|View|H1 heading, clear and concise.|
|ELM-LIST-002|Button|Add New User|إضافة مستخدم جديد|Optional|N/A|Navigates to the "Add New System User" form.|N/A|Click|Primary action button, clearly labeled.|
|ELM-LIST-003|Table|User List Table|جدول قائمة المستخدمين|Mandatory|N/A|Displays a paginated list of system users.|User|View, Scroll, Click (on rows/actions)|Accessible table structure, clear column headers.|
|ELM-LIST-004|Table Header|Name|الاسم|Mandatory|N/A|Column header for user's name.|User.Name|Click (for sorting)|Screen reader friendly.|
|ELM-LIST-005|Table Header|Status|الحالة|Mandatory|N/A|Column header for user's status (Active/Inactive).|User.Status|Click (for sorting)|Screen reader friendly.|
|ELM-LIST-006|Table Header|Role|الدور|Mandatory|N/A|Column header for user's assigned role(s).|User.Role|Click (for sorting)|Screen reader friendly.|
|ELM-LIST-007|Table Header|Last Update Date|تاريخ آخر تحديث|Mandatory|N/A|Column header for the last modification date of the user record.|User.LastUpdateDate|Click (for sorting)|Screen reader friendly, indicates sortability.|
|ELM-LIST-008|Table Header|Actions|الإجراءات|Mandatory|N/A|Column header for action buttons/links.|N/A|N/A|Screen reader friendly.|
|ELM-LIST-009|Text Label|User Name (in row)|اسم المستخدم (في الصف)|Mandatory|N/A|Displays the name of each user in the list.|User.Name|View|Ensure sufficient contrast.|
|ELM-LIST-010|Text Label|User Status (in row)|حالة المستخدم (في الصف)|Mandatory|N/A|Displays the status (Active/Inactive) of each user.|User.Status|View|Use clear visual indicators (e.g., color, icon) for status.|
|ELM-LIST-011|Text Label|User Role (in row)|دور المستخدم (في الصف)|Mandatory|N/A|Displays the primary role or a list of roles for each user.|User.Role|View|Ensure sufficient contrast.|
|ELM-LIST-012|Text Label|Last Update Date (in row)|تاريخ آخر تحديث (في الصف)|Mandatory|N/A|Displays the last date and time the user record was modified.|User.LastUpdateDate|View|Ensure sufficient contrast, consistent date/time format.|
|ELM-LIST-013|Button/Link|View (in row)|عرض (في الصف)|Mandatory|N/A|Navigates to the "View System User Details" page.|N/A|Click|Clear icon/text, accessible label.|
|ELM-LIST-014|Button/Link|Edit (in row)|تعديل (في الصف)|Mandatory|N/A|Navigates to the "Edit Existing System User" form.|N/A|Click|Clear icon/text, accessible label.|
|ELM-LIST-015|Button/Link|Deactivate (in row)|إلغاء التفعيل (في الصف)|Conditional|N/A|Initiates deactivation process. \*\*Hidden if user is Inactive or if deactivation is prevented by single-holder role rule.\*\*|N/A|Click|Clear icon/text, accessible label, disabled if user is inactive.|
|ELM-LIST-016|Button/Link|Activate (in row)|تفعيل (في الصف)|Conditional|N/A|Initiates activation process. \*\*Hidden if user is Active.\*\*|N/A|Click|Clear icon/text, accessible label, visible/enabled based on conditions.|
|\*\*ELM-LIST-017\*\*|\*\*Button/Link\*\*|\*\*Reset Password (in row)\*\*|\*\*إعادة تعيين كلمة المرور (في الصف)\*\*|\*\*Conditional\*\*|\*\*N/A\*\*|\*\*Initiates the "Reset User Password" process.\*\*|\*\*N/A\*\*|\*\*Click\*\*|\*\*Clear label, hidden if user is not eligible (Status = Inactive OR Registration Is Completed = 0 OR Registration Message Is Sent = 0).\*\*|
|\*\*ELM-LIST-018\*\*|\*\*Button/Link\*\*|\*\*Resend Message (in row)\*\*|\*\*إعادة إرسال الرسالة (في الصف)\*\*|\*\*Conditional\*\*|\*\*N/A\*\*|\*\*Initiates the "Resend Account Registration Message" process.\*\*|\*\*N/A\*\*|\*\*Click\*\*|\*\*Clear label, hidden if user is not eligible (Status = Inactive OR Registration Is Completed = 1 OR Registration Message Is Sent = 0).\*\*|
|ELM-LIST-019|Pagination Controls|عناصر التحكم بالترقيم|N/A|Mandatory|N/A|Allows navigation through multiple pages of users.|N/A|Click|Clear navigation, current page indicator.|
|ELM-LIST-020|Text Label|"No records exist to display" message|رسالة "عفوا, لا توجد بيانات مسجلة لعرضها"|Conditional|N/A|Displays when the user list is empty.|N/A|View|Clear and informative.|
|ELM-LIST-021|Text Label|Error Message Display|عرض رسالة الخطأ|Conditional|N/A|Displays generic error messages (e.g., for data retrieval or display errors).|N/A|View|Prominent display, clear text.|
### Actionable Buttons Table

|**Button/Link Name (English)**|**Button/Link Name (Arabic)**|**Description**|**Target User Story**|**Visibility/Enablement Condition**|
| :- | :- | :- | :- | :- |
|\*\*View\*\*|عرض|Navigates to the detailed profile page of the selected user.|View System User Details|Always visible and enabled for all users.|
|\*\*Edit\*\*|تعديل|Navigates to the editable form for the selected user's details.|Edit Existing System User|Always visible and enabled for all users.|
|\*\*Deactivate\*\*|إلغاء التفعيل|Initiates deactivation process.|Activate/Deactivate System User|Visible and enabled only if the user's current Status is 'Active' AND deactivation is NOT prevented by single-holder role rule.|
|\*\*Activate\*\*|تفعيل|Initiates activation process.|Activate/Deactivate System User|Visible and enabled only if the user's current Status is 'Inactive'.|
|\*\*Reset Password\*\*|إعادة تعيين كلمة المرور|Initiates the password reset process.|Reset User Password|Visible and enabled only if the user's Status is 'Active' AND their 'Registration Is Completed' flag is 1 AND their 'Registration Message Is Sent' flag is 1.|
|\*\*Resend Message\*\*|إعادة إرسال الرسالة|Initiates the resend account registration message process.|Resend Account Registration Message|Visible and enabled only if the user is 'Active', AND their 'Registration Is Completed' flag is 0, AND their 'Registration Message Is Sent' flag is 1.|
### Summary Section
This rewritten user story for "View System Users List" now fully incorporates the "Reset Password" and "Resend Message" buttons into its "Actions" column. Their precise display conditions, as defined in their respective user stories, are explicitly stated in the Business Rules, Acceptance Criteria, and Screen Elements Table. This ensures that the list view provides accurate and context-sensitive access to all user management actions.

\*\*Important Note:\*\* I am still awaiting your clarification on the "Action Not Applicable (Role/Assignment)" logic for the "Resend Account Registration Message" user story from our previous interaction. The current logic in that story (and reflected in its Acceptance Criteria) is: "Action is NOT applicable if the user has ONLY the 'Board Member' role AND is NOT assigned to any fund board." However, with `IsAssignedToFundBoard` removed from the User entity, this specific condition cannot be implemented as written. \*\*Please confirm how the "Resend Message" eligibility for Board Members should be determined without `IsAssignedToFundBoard`.\*\* This clarification is essential for the "Resend Account Registration Message" story's button visibility logic, and consequently for the "Resend Message" button's visibility in this "View System Users List" story.

\*\*Key Implementation Considerations:\*\*

\*   \*\*Critical:\*\* Implement the complex combined visibility logic for all action buttons in the "Actions" column.

\*   Ensure clicking these buttons correctly navigates to or triggers the corresponding user story's process.

\*   Maintain efficient data retrieval and rendering for the list, especially with multiple conditional columns.

\*\*Dependencies and Risks:\*\*

\*   \*\*Dependency:\*\* This story depends on all other user management stories (Add, Edit, View Details, Activate/Deactivate, Reset Password, Resend Message) for the functionality of its action buttons.

\*   \*\*Risk:\*\* Incorrect button visibility leading to user frustration or security vulnerabilities. Mitigation: Rigorous testing of all visibility conditions.

\---

I have now rewritten the "View System Users List" user story with the inclusion and conditional display of "Reset Password" and "Resend Message" buttons. Please provide clarification on the "Resend Account Registration Message" logic regarding Board Members when you're ready.
## User Story JDWA-1267: User Login
### Introduction Section
This user story describes the process for a user to securely log in to the Fund Board Management Application. It covers authentication via username (Saudi Mobile Number) and password. Upon successful login, the system checks the `Registration Is Completed` flag: if 0, the user is redirected to a mandatory "Reset Password" screen; if 1, they are redirected to their dashboard. This ensures enhanced security for initial logins and a streamlined experience for returning users. The Screen Elements Table provides a detailed breakdown of the UI components on the login screen.
### Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|User Login|Allows users to securely log in to the application.|
|\*\*User Story\*\*|As a \*\*system user\*\*, I want to \*\*securely log in to the application and be directed appropriately based on my registration status\*\* so that I can \*\*access my assigned functionalities and ensure my account security.\*\*||
|\*\*Story Points\*\*|7|Involves authentication, session management, and conditional redirection based on registration status.|
|\*\*User Roles\*\*|All System Users|Applicable to all roles (System Admin, Fund Manager, Board Member, etc.).|
|\*\*Access Requirements\*\*|Valid username (Mobile No.) and password. Active user status.|User must have a valid, active account.|
|\*\*Trigger\*\*|User navigates to the application URL.|User attempts to access the system.|
|\*\*Frequency of Use\*\*|High|Users log in frequently to access the system.|
|\*\*Pre-condition\*\*|User has a registered account with a username (Mobile No.) and password. The application is running and accessible.|The system is operational and ready for login.|
|\*\*Business Rules\*\*|1\. Username is the registered Saudi Mobile Number (10 digits, starting with 05).<br>2. Password must be validated against stored credentials.<br>3. Only 'Active' users can log in.<br>4. Integration with Active Directory (Phase Two) for authentication.<br>5. Upon successful login, the system checks the 'Registration Is Completed' flag:<br>   a. If 'Registration Is Completed' = 0, the user is immediately redirected to a "Reset Password" screen.<br>   b. If 'Registration Is Completed' = 1, the user is redirected to their default dashboard/landing page.<br>|Ensures secure and user-friendly access, with conditional redirection based on registration completion and robust account security.|
|\*\*Post-condition\*\*|User is successfully authenticated, redirected to the appropriate screen (Reset Password or Dashboard), and a secure session is established. Or, if lockout occurs, the account is deactivated.|The user gains access to the system, or their account is secured.|
|\*\*Risk\*\*|1\. Unauthorized access due to weak security.<br>2. Incorrect redirection logic.|Mitigation: Strong password policies, clear password recovery/reset options, thorough testing of redirection logic, clear messaging on account deactivation.|
|\*\*Assumptions\*\*|1\. User accounts are managed via "Add New System User" and "Edit Existing System User" stories.<br>2. Password hashing and storage are handled securely by the backend.<br>3. A "Reset User Password" functionality exists (separate story).<br>4. Active Directory integration will be handled in a later phase.<br>5. The system's default language is Arabic.|These assumptions are crucial for implementing the functionality.|
|\*\*UX/UI Design Link\*\*|Provided login UI (Page 5 of BRD).|References the initial login screen mockup.|
### Process Flow Table

|**Step**|**Action Description**|**Actor**|**Related Message Codes**|**Notes**|
| :- | :- | :- | :- | :- |
|1|User navigates to the application URL.|User|||
|2|System displays the Login screen with options for username (Mobile No.) and password.|System|(References BRD Page 5 UI, simplified).||
|3|User enters their registered Mobile No. as username and password.|User|||
|4|User clicks "Login" button.|User|||
|5|System validates username (Mobile No.) and password against stored credentials.|System|MSG-LOGIN-001, MSG-LOGIN-002|Checks for valid format, existence, and match.|
|10|If login is successful: System checks if the user account is 'Active'.|System|MSG-LOGIN-003|If 'Inactive' (e.g., due to previous deactivation or manual admin action), login is denied.|
|11|\*\*If login is successful AND user is 'Active':\*\* System establishes a secure session.|System|||
|12|System checks the 'Registration Is Completed' flag for the user.|System|||
|13|If 'Registration Is Completed' = 0: System immediately redirects user to the "Reset Password" screen.|System|This is a mandatory redirection for first-time/incomplete registration.||
|14|If 'Registration Is Completed' = 1: System redirects user to their default dashboard/landing page.|System|This is for returning users with completed registration.||
|15|System logs the login attempt (success or failure).|System|For security and auditing.||
### Alternative Flow Table

|**Alternative Scenario**|**Condition**|**Action**|**Related Message Codes**|**Resolution**|
| :- | :- | :- | :- | :- |
|\*\*Invalid Credentials\*\*|User enters incorrect Mobile No. or password.|System displays an error message.|MSG-LOGIN-001|User must re-enter correct credentials.|
|\*\*Account Deactivated (by admin)\*\*|User attempts to log in with an account previously deactivated by an admin.|System displays an error message indicating the account is inactive.|MSG-LOGIN-003|User must contact System Admin for activation.|
|\*\*System Error during Login\*\*|A backend error occurs during the authentication process.|System displays a generic error message.|MSG-LOGIN-004|User can retry or contact support.|
### Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Successful Login (Registration Not Completed)\*\*|A user account exists with `Registration Is Completed` = 0.|The user enters their correct Mobile No. and password, and clicks "Login".|The user is successfully authenticated, immediately redirected to the "Reset Password" screen, and a secure session is established.|
|\*\*Successful Login (Registration Completed)\*\*|An existing user account exists with `Registration Is Completed` = 1.|The user enters their correct Mobile No. and password, and clicks "Login".|The user is successfully authenticated, redirected to their default dashboard/landing page, and a secure session is established.|
|\*\*Invalid Credentials (Below Lockout)\*\*|An account exists.|The user enters an incorrect password for a valid Mobile No. (e.g., 3 times), and clicks "Login".|The system displays an error message "Invalid username or password."|
|\*\*Inactive User Login\*\*|An 'Inactive' user account exists (e.g., deactivated by admin).|The user enters their Mobile No. and password, and clicks "Login".|The system displays an error message "Your account is inactive. Please contact support."|
|\*\*System Error during Login\*\*|A backend error occurs during the authentication process.|The user attempts to log in.|The system displays a generic error message "An unexpected error occurred during login. Please try again."|
### Data Entities Table
Entity Name: User (for login)

|**Attribute (English)**|**Attribute (Arabic)**|**Mandatory/Optional**|**Attribute Type**|**Data Length**|**Integration Requirements**|**Default Value**|**Condition (if needed)**|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|User ID|معرف المستخدم|Mandatory|Number|N/A|Database Primary Key|N/A|N/A|Unique identifier|12345|12345|
|Mobile|رقم الجوال|Mandatory|Text|Max 10|Database Field|N/A|N/A|Saudi mobile format (e.g., 05XXXXXXXX), Numeric only, Unique (as username).|0501234567|0501234567|
|Password Hash|تجزئة كلمة المرور|Mandatory|Text|N/A|Database Field|N/A|N/A|Hashed value of user's password.|N/A|N/A|
|Status|الحالة|Mandatory|Boolean/Dropdown|N/A|Database Field|Active|N/A|Active/Inactive|نشط|Active|
|Registration Is Completed|تم اكتمال التسجيل|Mandatory|Boolean|N/A|Database Field|0 (False)|N/A|Set to 0 upon user creation. Set to 1 after successful password reset.|0|False|
### Messages/Notifications Table

|**Message Code**|**Message (English)**|**Message (Arabic)**|**Message Type**|**Communication Method**|
| :- | :- | :- | :- | :- |
|MSG-LOGIN-001|Invalid username or password.|اسم المستخدم أو كلمة المرور غير صحيحة.|Error Message|In-App|
|MSG-LOGIN-002|Login successful.|تم تسجيل الدخول بنجاح.|Success Message|In-App|
|MSG-LOGIN-003|Your account is inactive. Please contact support.|حسابك غير نشط. يرجى الاتصال بالدعم.|Error Message|In-App|
|MSG-LOGIN-004|An unexpected error occurred during login. Please try again.|حدث خطأ غير متوقع أثناء تسجيل الدخول. يرجى المحاولة مرة أخرى.|Error Message|In-App|
### Screen Elements Table

|**Element ID**|**Element Type**|**Element Name (English)**|**Element Name (Arabic)**|**Required/Optional**|**Validation Rules**|**Business Logic**|**Related Data Entity**|**User Interaction**|**Accessibility Notes**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|ELM-LOGIN-001|Page Title|Login|تسجيل الدخول|N/A|N/A|Displays the title of the login page.|N/A|View|H1 heading.|
|ELM-LOGIN-002|Input Field|Mobile No. (Username)|رقم الجوال (اسم المستخدم)|Mandatory|Saudi mobile format, Max 10 digits|Collects the user's mobile number as username.|User.Mobile|Type|Clear label, placeholder, numeric keyboard.|
|ELM-LOGIN-003|Input Field|Password|كلمة المرور|Mandatory|N/A|Collects the user's password.|User.Password|Type|Clear label, password type input (masked).|
|ELM-LOGIN-004|Button|Login Button|زر تسجيل الدخول|Mandatory|N/A|Submits login credentials.|N/A|Click|Primary action button.|
|ELM-LOGIN-005|Link|Forgot Password?|هل نسيت كلمة المرور؟|Optional|N/A|Navigates to password recovery/reset process.|N/A|Click|Clear link text.|
|ELM-LOGIN-006|Text Label|Error Message Display|عرض رسالة الخطأ|Conditional|N/A|Displays login error messages (e.g., invalid credentials, inactive account, deactivated account).|N/A|View|Prominent display, clear text.|
### Summary Section
This rewritten user story for "User Login" now accurately reflects the removal of `ELM-LOGIN-007` (Welcome Message) from the Screen Elements Table. The login process remains streamlined, with conditional redirection based on the `Registration Is Completed` flag.

\*\*Important Note:\*\* I am still awaiting your clarification on the "Action Not Applicable (Role/Assignment)" logic for the "Resend Account Registration Message" user story from our previous interaction. The current logic in that story (and reflected in its Acceptance Criteria) is: "Action is NOT applicable if the user has ONLY the 'Board Member' role AND is NOT assigned to any fund board." However, with `IsAssignedToFundBoard` removed from the User entity, this specific condition cannot be implemented as written. \*\*Please confirm how the "Resend Message" eligibility for Board Members should be determined without `IsAssignedToFundBoard`.\*\*

\*\*Key Implementation Considerations:\*\*

\*   \*\*Critical:\*\* The "Reset User Password" story must be updated to set `Registration Is Completed` to 1 upon successful password reset.

\*   Ensure the redirection logic is robust and cannot be bypassed.

\*   The system's default language will be used for the UI.

\*\*Dependencies and Risks:\*\*

\*   \*\*Dependency:\*\* This story depends heavily on the "Reset User Password" story for the next step in the login flow. It also depends on "Add New System User" for account creation.

\*   \*\*Risk:\*\* User confusion if login process is not intuitive or error messages are unclear.

\---

I have now updated the "User Login" user story with the removal of `ELM-LOGIN-007`. Please provide clarification on the "Resend Account Registration Message" logic regarding Board Members when you're ready.
## User Story JDWA-1268: User Password Management (Self-Service)
### Introduction Section
This user story describes the functionality for a system user to manage their own password. This includes changing their password from their personal profile when already logged in, as well as serving as the mandatory password reset screen for users upon their very first login (when `Registration Is Completed` is 0). Upon successful password reset, the system will update the user's `Registration Is Completed` flag to 1 (if it was 0) and redirect them to their dashboard (if `Registration Is Completed` was 0) or their Manage Profile screen (if `Registration Is Completed` was 1). \*\*A display label will explicitly list the detailed password complexity rules in both English and Arabic.\*\* The Screen Elements Table provides a detailed breakdown of the UI components involved in this process.
### Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|User Password Management (Self-Service)|Allows users to change their password from profile or as a mandatory first-login reset.|
|\*\*User Story\*\*|As a \*\*system user\*\*, I want to \*\*manage my password (change it from my profile or reset it on first login)\*\* so that I can \*\*maintain my account security and complete my registration if pending.\*\*||
|\*\*Story Points\*\*|8|Combines standard password change with a critical first-login mandatory reset, including conditional flag update and redirection.|
|\*\*User Roles\*\*|All System Users|Applicable to all logged-in users (for profile change) and users redirected from login (for mandatory reset).|
|\*\*Access Requirements\*\*|User must be logged in (for profile change) OR successfully authenticated and redirected from login (for mandatory reset).|User must be authenticated.|
|\*\*Trigger\*\*|User clicks "Change Password" on profile OR System redirects user from Login (if `Registration Is Completed` = 0).|User initiates or is forced into the action.|
|\*\*Frequency of Use\*\*|Low|Used periodically for security updates or as a one-time first-login requirement.|
|\*\*Pre-condition\*\*|User is logged in (for profile change) OR successfully authenticated from login with `Registration Is Completed` = 0.|The system is operational and the user's session is active.|
|\*\*Business Rules\*\*|1\. User must provide their current password for verification (if initiated from profile).<br>2. The current password must match the stored password for the logged-in user.<br>3. New password must meet the following complexity requirements:<br>   a. At least one digit (0-9).<br>   b. At least one lowercase letter (a-z).<br>   c. At least one non-alphanumeric character (e.g., !, @, #, $, %, ^, &, \*, (, )).<br>   d. At least one uppercase letter (A-Z).<br>   e. Minimum length of 6 characters.<br>   f. At least 1 unique character (this rule is typically superseded by other complexity rules, but included as specified).<br>4. The new password must be confirmed by re-entry.<br>5. The new password cannot be the same as the current password.<br>6. The 'Last Update Date' must be automatically updated upon successful password change.<br>7. After any successful password reset from this screen:<br>   a. If the user's 'Registration Is Completed' flag was 0 (meaning it was a first-time reset), the system must set `Registration Is Completed` to 1, and then redirect the user to their default dashboard/landing page.<br>   b. If the user's 'Registration Is Completed' flag was 1 (meaning it was a standard profile change), the user is redirected to their Manage Profile screen.<br>8. If initiated from first-login redirection, the user cannot navigate away from this screen until password is changed.<br>9. Password complexity rules must be displayed to the user on the password change form.|Ensures secure password changes, completes registration for new users, and provides appropriate redirection.|
|\*\*Post-condition\*\*|User's password is changed to the new password, the 'Last Update Date' is refreshed, the 'Registration Is Completed' flag is updated (if applicable), and the user is redirected to their \*\*dashboard (if first-time reset) or Manage Profile screen (if standard change).\*\*|The user's account is updated with the new password, and their registration status is finalized.|
|\*\*Risk\*\*|1\. Unauthorized password change if current password verification is weak.<br>2. User unable to change password due to complexity requirements or errors.<br>3. User confusion if forced into password change.|Mitigation: Strong password complexity rules, robust current password verification, clear error messages, clear messaging on mandatory reset screen.|
|\*\*Assumptions\*\*|1\. Password hashing and storage are handled securely by the backend.<br>2. Password complexity rules are defined.<br>3. A "Manage Profile" screen exists as a destination.|These assumptions are crucial for implementing the functionality.|
|\*\*UX/UI Design Link\*\*|N/A (Multi-purpose password change form)|No specific mockups provided in the BRD for this section.|
### Process Flow Table

|**Step**|**Action Description**|**Actor**|**Related Message Codes**|**Notes**|
| :- | :- | :- | :- | :- |
|1|User initiates password change: either by clicking "Change Password" on profile, OR by being redirected from Login (if `Registration Is Completed` = 0).|User / System|||
|2|System displays the password change form, prompting for Current Password (if from profile), New Password, and Confirm New Password. \*\*System also displays password rules.\*\*|System|If redirected from Login (`Registration Is Completed` = 0), the "Current Password" field may be hidden/disabled/pre-filled with temporary password, and the user cannot navigate away.||
|3|User enters their current password (if applicable), new password, and confirms new password.|User|MSG-PROFILE-PW-001, MSG-PROFILE-PW-002, MSG-PROFILE-PW-003, MSG-PROFILE-PW-004||
|4|User clicks "Save Changes" or "Reset Password" button.|User|||
|5|System validates current password against stored credentials (if applicable).|System|MSG-PROFILE-PW-001|If mismatch, display error.|
|6|System validates new password (complexity, match with confirmation, not same as current).|System|MSG-PROFILE-PW-002, MSG-PROFILE-PW-003, MSG-PROFILE-PW-004|If invalid, display error.|
|7|If all validations are successful: System updates the user's password in the database with the new password.|System|The 'Last Update Date' is automatically refreshed.||
|8|System displays a success message.|System|MSG-PROFILE-PW-005||
|9|System checks the user's 'Registration Is Completed' flag (its value \*before\* any update in this process).|System|This check determines the original state for conditional update and redirection.||
|10|If 'Registration Is Completed' was 0 (based on check in Step 9): System sets the user's 'Registration Is Completed' flag to 1.|System|This updates the flag to reflect completion.||
|11|System redirects the user based on the original 'Registration Is Completed' flag value (from Step 9):<br>   - If original 'Registration Is Completed' was 0: Redirect to their default dashboard/landing page.<br>   - If original 'Registration Is Completed' was 1: Redirect to their Manage Profile screen.|System|This step handles the redirection based on the flag's state before the update.||
### Alternative Flow Table

|**Alternative Scenario**|**Condition**|**Action**|**Related Message Codes**|**Resolution**|
| :- | :- | :- | :- | :- |
|\*\*Incorrect Current Password (from Profile)\*\*|User enters an incorrect current password (when initiated from profile).|System displays an error message.|MSG-PROFILE-PW-001|User must re-enter correct current password.|
|\*\*New Password Complexity Violation\*\*|User enters a new password that does not meet complexity rules.|System displays an error message.|MSG-PROFILE-PW-002|User must enter a new password meeting requirements.|
|\*\*New Password Mismatch\*\*|New password and confirm password do not match.|System displays an error message.|MSG-PROFILE-PW-003|User must ensure passwords match.|
|\*\*New Password Same as Current\*\*|User attempts to set the new password identical to the current password.|System displays an error message.|MSG-PROFILE-PW-004|User must choose a different new password.|
|\*\*System Error during Password Change\*\*|A backend error occurs during password update.|System displays a generic error message.|MSG-PROFILE-PW-006|User can retry or contact support.|
|\*\*Cancel Password Change (from Profile)\*\*|User decides not to save changes (when initiated from profile).|User clicks "Cancel" button.|System discards changes and returns to profile screen.||
|\*\*Mandatory Password Change Bypass Attempt\*\*|User is redirected from Login (`Registration Is Completed` = 0) and attempts to navigate away from the password change screen.|System prevents navigation or redirects back to the password change screen.|User must complete password change.||
### Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Successful Password Change (from Profile)\*\*|User is logged in and on their personal profile screen. User's `Registration Is Completed` flag is 1.|The user enters their correct current password, a new valid password (meeting complexity, different from current), confirms it, and clicks "Save Changes".|The user's password is changed, the 'Last Update Date' is updated, the 'Registration Is Completed' flag remains 1, a success message "Password changed successfully." is displayed, and the user is redirected to their Manage Profile screen.|
|\*\*Successful Password Reset (Mandatory First Login)\*\*|User is logged in and redirected from Login. User's `Registration Is Completed` flag is 0.|The user enters a new valid password (meeting complexity, different from current), confirms it, and clicks "Save Changes".|The user's password is changed, the 'Last Update Date' is updated, the 'Registration Is Completed' flag is set to 1, a success message "Password changed successfully." is displayed, and the user is redirected to their dashboard.|
|\*\*Incorrect Current Password (from Profile)\*\*|User is logged in and on the password change form.|The user enters an incorrect current password, valid new password, and clicks "Save Changes".|The system displays an error message "Incorrect current password."|
|\*\*New Password Complexity Violation (Missing Digit)\*\*|User is logged in and on the password change form.|The user enters their correct current password (if applicable), a new password like "Abc!ef" (no digit), and clicks "Save Changes".|The system displays an error message "Password does not meet complexity requirements."|
|\*\*New Password Complexity Violation (Too Short)\*\*|User is logged in and on the password change form.|The user enters their correct current password (if applicable), a new password like "Abc!1" (length 5), and clicks "Save Changes".|The system displays an error message "Password does not meet complexity requirements."|
|\*\*New Password Same as Current\*\*|User is logged in and on the password change form.|The user enters their correct current password (if applicable), and sets the new password identical to the current one, then clicks "Save Changes".|The system displays an error message "New password cannot be the same as current password."|
|\*\*Mandatory Password Change - Bypass Attempt\*\*|User is redirected from Login (`Registration Is Completed` = 0) to the password change screen.|The user attempts to navigate to another page (e.g., dashboard, another menu item).|The system prevents navigation and keeps the user on the password change screen, or redirects them back to it.|
### Data Entities Table
Entity Name: User (for self-service password management)

|**Attribute (English)**|**Attribute (Arabic)**|**Mandatory/Optional**|**Attribute Type**|**Data Length**|**Integration Requirements**|**Default Value**|**Condition (if needed)**|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|User ID|معرف المستخدم|Mandatory|Number|N/A|Database Primary Key|N/A|N/A|Unique identifier|12345|12345|
|Password Hash|تجزئة كلمة المرور|Mandatory|Text|N/A|Database Field|N/A|N/A|Hashed value of user's password.|N/A|N/A|
|Last Update Date|تاريخ آخر تحديث|Mandatory|DateTime|N/A|Database Field|Current Timestamp|N/A|Automatically set on creation and update.|2023-10-26 15:00:00|2023-10-26 15:00:00|
|Registration Is Completed|تم اكتمال التسجيل|Mandatory|Boolean|N/A|Database Field|0 (False)|N/A|Set to 0 upon user creation. Set to 1 after successful password reset.|0|False|
### Messages/Notifications Table

|**Message Code**|**Message (English)**|**Message (Arabic)**|**Message Type**|**Communication Method**|
| :- | :- | :- | :- | :- |
|MSG-PROFILE-PW-001|Incorrect current password.|كلمة المرور الحالية غير صحيحة.|Error Message|In-App|
|MSG-PROFILE-PW-002|Password does not meet complexity requirements.|كلمة المرور لا تفي بمتطلبات التعقيد.|Error Message|In-App|
|MSG-PROFILE-PW-003|Passwords do not match.|كلمات المرور غير متطابقة.|Error Message|In-App|
|MSG-PROFILE-PW-004|New password cannot be the same as current password.|كلمة المرور الجديدة لا يمكن أن تكون مطابقة لكلمة المرور الحالية.|Error Message|In-App|
|MSG-PROFILE-PW-005|Password changed successfully.|تم تغيير كلمة المرور بنجاح.|Success Message|In-App|
|MSG-PROFILE-PW-006|An error occurred while changing password.|حدث خطأ أثناء تغيير كلمة المرور.|Error Message|In-App|
### Screen Elements Table

|**Element ID**|**Element Type**|**Element Name (English)**|**Element Name (Arabic)**|**Required/Optional**|**Validation Rules**|**Business Logic**|**Related Data Entity**|**User Interaction**|**Accessibility Notes**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|ELM-PROFILE-PW-001|Page Title|Change Password / Reset Password|تغيير كلمة المرور / إعادة تعيين كلمة المرور|N/A|N/A|Displays the title of the password management form. Dynamic based on context.|N/A|View|H1 heading.|
|ELM-PROFILE-PW-002|Input Field|Current Password|حقل كلمة المرور الحالية|Conditional|N/A|Collects the user's current password for verification. \*\*Hidden/disabled if redirected from Login (first-time reset).\*\*|User.Password|Type|Clear label, password type input (masked).|
|ELM-PROFILE-PW-003|Input Field|New Password|حقل كلمة المرور الجديدة|Mandatory|\*\*At least 1 digit, 1 lowercase, 1 non-alphanumeric, 1 uppercase, Min length 6, 1 unique char.\*\*|Collects the user's new password.|User.Password|Type|Clear label, password type input (masked).|
|\*\*ELM-PROFILE-PW-004\*\*|\*\*Text Label\*\*|\*\*Password Rules Display\*\*|\*\*عرض قواعد كلمة المرور\*\*|\*\*Mandatory\*\*|\*\*N/A\*\*|\*\*Displays the password complexity requirements to the user:\*\*<br>\*\*a. At least one digit (0-9).\*\*<br>\*\*b. At least one lowercase letter (a-z).\*\*<br>\*\*c. At least one non-alphanumeric character (e.g., !, @, #, $, %, ^, &, \*, (, )).\*\*<br>\*\*d. At least one uppercase letter (A-Z).\*\*<br>\*\*e. Minimum length of 6 characters.\*\*<br>\*\*f. At least 1 unique character.\*\*<br>\*\*Arabic:\*\*<br>\*\*أ. رقم واحد على الأقل (0-9).\*\*<br>\*\*ب. حرف صغير واحد على الأقل (أ-ي).\*\*<br>\*\*ج. حرف غير أبجدي رقمي واحد على الأقل (مثل !, @, #, $, %, ^, &, \*, (, )).\*\*<br>\*\*د. حرف كبير واحد على الأقل (أ-ي).\*\*<br>\*\*هـ. طول لا يقل عن 6 أحرف.\*\*<br>\*\*و. حرف فريد واحد على الأقل.\*\*|\*\*N/A\*\*|\*\*View\*\*|\*\*Clear and concise text, visible near password fields.\*\*|
|ELM-PROFILE-PW-005|Input Field|Confirm New Password|حقل تأكيد كلمة المرور الجديدة|Mandatory|Must match New Password|Confirms the new password.|N/A|Type|Clear label, password type input (masked).|
|ELM-PROFILE-PW-006|Button|Save Changes / Reset Password|زر حفظ التغييرات / إعادة تعيين كلمة المرور|Mandatory|N/A|Submits the password change/reset request.|N/A|Click|Primary action button.|
|ELM-PROFILE-PW-007|Button|Cancel|زر إلغاء|Conditional|N/A|Discards changes and returns to the profile screen. \*\*Hidden if redirected from Login (mandatory reset).\*\*|N/A|Click|Secondary action button.|
|ELM-PROFILE-PW-008|Text Label|Error Message Display|عرض رسالة الخطأ|Conditional|N/A|Displays validation or system error messages.|N/A|View|Prominent display.|
|ELM-PROFILE-PW-009|Text Label|Success Message Display|عرض رسالة النجاح|Conditional|N/A|Displays confirmation of successful password change/reset.|N/A|View|Prominent display.|
|ELM-PROFILE-PW-010|Text Label|Mandatory Reset Instruction|تعليمات إعادة التعيين الإلزامية|Conditional|N/A|Displays instructions like "Please set a new password to complete your registration." \*\*Visible only if redirected from Login (mandatory reset).\*\*|N/A|View|Clear and prominent.|
### Summary Section
This rewritten user story for "User Password Management (Self-Service)" now includes the detailed password complexity rules in both English and Arabic within the "Password Rules Display" label (`ELM-PROFILE-PW-004`) in the Screen Elements Table. This ensures users receive clear, bilingual guidance on password requirements. The logic for conditional redirection and `Registration Is Completed` flag update remains precise.

\*\*Important Note:\*\* I am still awaiting your clarification on the "Action Not Applicable (Role/Assignment)" logic for the "Resend Account Registration Message" user story from our previous interaction. The current logic in that story (and reflected in its Acceptance Criteria) is: "Action is NOT applicable if the user has ONLY the 'Board Member' role AND is NOT assigned to any fund board." However, with `IsAssignedToFundBoard` removed from the User entity, this specific condition cannot be implemented as written. \*\*Please confirm how the "Resend Message" eligibility for Board Members should be determined without `IsAssignedToFundBoard`.\*\*

\*\*Key Implementation Considerations:\*\*

\*   \*\*Critical:\*\* Ensure the "Password Rules Display" label accurately reflects all specified complexity rules in both languages.

\*   Implement the conditional redirection logic based on the user's `Registration Is Completed` flag \*before\* it is updated in this process.

\*   Ensure the `Registration Is Completed` flag is correctly updated to 1 upon successful password reset when it was originally 0.

\*\*Dependencies and Risks:\*\*

\*   \*\*Dependency:\*\* This story is a critical dependency for the "User Login" story (for the first-time login redirection). It also depends on the "User Profile" screen for standard access.

\*   \*\*Risk:\*\* User frustration if password complexity rules are not clearly communicated or are too restrictive. Mitigation: Clear inline help text, real-time validation feedback.

\---
## User Story JDWA-1269: User Logout
### Introduction Section
This user story describes the functionality for a logged-in system user to securely log out of the Fund Board Management Application. This action is essential for terminating the user's session, protecting their account from unauthorized access, and ensuring data privacy, especially on shared devices. The Screen Elements Table provides a breakdown of the UI components involved in initiating logout.
### Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|User Logout|Allows a logged-in user to securely log out of the application.|
|\*\*User Story\*\*|As a \*\*system user\*\*, I want to \*\*securely log out of the application\*\* so that I can \*\*protect my account from unauthorized access and end my session.\*\*||
|\*\*Story Points\*\*|2|A straightforward security and session management function.|
|\*\*User Roles\*\*|All System Users|Applicable to all logged-in users.|
|\*\*Access Requirements\*\*|User must be logged in.|User must have an active session.|
|\*\*Trigger\*\*|User clicks "Logout" button/link (e.g., from header, profile menu).|User initiates the action.|
|\*\*Frequency of Use\*\*|High|Users log out frequently, especially at the end of a session.|
|\*\*Pre-condition\*\*|User is successfully logged in and has an active session.|The system is operational and the user is authenticated.|
|\*\*Business Rules\*\*|1\. Clicking "Logout" must terminate the current user session on the server-side.<br>2. All session-related data (e.g., cookies, local storage tokens) on the client-side must be cleared.<br>3. After successful logout, the user must be redirected to the Login screen.|Ensures complete session termination and proper redirection.|
|\*\*Post-condition\*\*|The user's session is terminated, client-side session data is cleared, and the user is redirected to the Login screen.|The user is securely logged out of the application.|
|\*\*Risk\*\*|1\. Session not fully terminated, leading to security vulnerability.<br>2. User not redirected to login screen.|Mitigation: Robust session management, thorough testing of logout process.|
|\*\*Assumptions\*\*|1\. Session management (e.g., JWT, cookies) is implemented securely.<br>2. The Login screen is the designated post-logout destination.|These assumptions are crucial for implementing the functionality.|
|\*\*UX/UI Design Link\*\*|N/A (Standard logout button placement in header/menu)|No specific mockups provided in the BRD for this section.|
### Process Flow Table

|**Step**|**Action Description**|**Actor**|**Related Message Codes**|**Notes**|
| :- | :- | :- | :- | :- |
|1|User is logged in and clicks the "Logout" button/link.|User|This button is typically found in the header or a user profile menu.||
|2|System receives the logout request.|System|||
|3|System terminates the user's active session on the server-side.|System|Invalidates session tokens, clears server-side session data.||
|4|System clears all client-side session-related data (e.g., cookies, local storage tokens).|System|||
|5|System displays a success message (optional, or implicit via redirection).|System|MSG-LOGOUT-001||
|6|System redirects the user to the Login screen.|System|||
### Alternative Flow Table

|**Alternative Scenario**|**Condition**|**Action**|**Related Message Codes**|**Resolution**|
| :- | :- | :- | :- | :- |
|\*\*Session Already Expired\*\*|User clicks "Logout" but their session has already expired.|System handles the request gracefully (e.g., redirects to login, no error message).|User is already logged out.||
|\*\*System Error during Logout\*\*|A backend error occurs during session termination.|System logs the error and attempts to redirect to the Login screen.|MSG-LOGOUT-002|User may need to clear browser data manually if session persists.|
|\*\*Network Interruption\*\*|Network connection is lost during logout request.|User may see a network error, or the request may time out.|User may need to refresh or manually navigate to login.||
### Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Successful Logout\*\*|User is logged in with an active session.|The user clicks the "Logout" button/link.|The user's session is terminated, client-side session data is cleared, a success message "You have been logged out successfully." is displayed (briefly), and the user is redirected to the Login screen.|
|\*\*Logout with Expired Session\*\*|User's session has expired (e.g., due to inactivity timeout).|The user clicks the "Logout" button/link.|The user is redirected to the Login screen. (No explicit error, as they were already effectively logged out).|
|\*\*System Error during Logout\*\*|User clicks "Logout".|A backend error prevents proper session termination.|The system displays a generic error message "An error occurred during logout. Please try again." and attempts to redirect to the Login screen.|
### Data Entities Table
Entity Name: User (for logout - primarily session data)

|**Attribute (English)**|**Attribute (Arabic)**|**Mandatory/Optional**|**Attribute Type**|**Data Length**|**Integration Requirements**|**Default Value**|**Condition (if needed)**|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|Session Token|رمز الجلسة|Mandatory|Text|N/A|Server-side session management, Client-side storage (e.g., cookie, local storage)|N/A|N/A|Unique, time-limited.|N/A|N/A|
### Messages/Notifications Table

|**Message Code**|**Message (English)**|**Message (Arabic)**|**Message Type**|**Communication Method**|
| :- | :- | :- | :- | :- |
|MSG-LOGOUT-001|You have been logged out successfully.|تم تسجيل خروجك بنجاح.|Success Message|In-App (briefly)|
|MSG-LOGOUT-002|An error occurred during logout. Please try again.|حدث خطأ أثناء تسجيل الخروج. يرجى المحاولة مرة أخرى.|Error Message|In-App|
### Screen Elements Table

|**Element ID**|**Element Type**|**Element Name (English)**|**Element Name (Arabic)**|**Required/Optional**|**Validation Rules**|**Business Logic**|**Related Data Entity**|**User Interaction**|**Accessibility Notes**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|ELM-LOGOUT-001|Button/Link|Logout Button|زر/رابط تسجيل الخروج|Mandatory|N/A|Initiates the logout process.|N/A|Click|Clear label, typically in header or user menu.|
|ELM-LOGOUT-002|Text Label|Success/Error Message Display|عرض رسالة النجاح/الخطأ|Conditional|N/A|Displays the outcome of the logout attempt.|N/A|View|Prominent display (briefly).|
### Summary Section
This user story for "User Logout" provides a clear and concise definition for securely ending a user's session. It covers the essential steps of server-side session termination, client-side data clearance, and redirection to the login screen. This ensures a fundamental security and usability aspect of the application is well-documented.

\*\*Important Note:\*\* I am still awaiting your clarification on the "Action Not Applicable (Role/Assignment)" logic for the "Resend Account Registration Message" user story from our previous interaction. The current logic in that story (and reflected in its Acceptance Criteria) is: "Action is NOT applicable if the user has ONLY the 'Board Member' role AND is NOT assigned to any fund board." However, with `IsAssignedToFundBoard` removed from the User entity, this specific condition cannot be implemented as written. \*\*Please confirm how the "Resend Message" eligibility for Board Members should be determined without `IsAssignedToFundBoard`.\*\*

\*\*Key Implementation Considerations:\*\*

\*   Ensure robust session invalidation on the server-side to prevent session hijacking.

\*   Thoroughly clear all client-side session data (cookies, local storage, etc.).

\*   The redirection to the login screen should be immediate and reliable.

\*\*Dependencies and Risks:\*\*

\*   \*\*Dependency:\*\* This story is dependent on the "User Login" story (as users must be logged in to log out).

\*   \*\*Risk:\*\* Incomplete session termination leading to security vulnerabilities. Mitigation: Rigorous testing of session management.

\---

I have now written the "User Logout" user story. Please provide clarification on the "Resend Account Registration Message" logic regarding Board Members when you're ready.
## User Story JDWA-1280: Manage Personal Profile
### Introduction Section
This user story describes the functionality for a logged-in system user to view and update their personal profile information within the Fund Board Management Application. This allows users to keep their contact details, professional documents (like CV), and other personal data current. Users can also upload or update their personal photo. The user's mobile number is displayed but not editable from this screen. \*\*Registration-related flags and system-managed dates are not displayed on this screen.\*\* It also provides access to password management. The Screen Elements Table provides a detailed breakdown of the UI components on the personal profile screen.
### Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|Manage Personal Profile|Allows logged-in users to view and update their personal information.|
|\*\*User Story\*\*|As a \*\*system user\*\*, I want to \*\*view and update my personal profile information, including my photo,\*\* so that I can \*\*keep my details current and manage my account settings.\*\*||
|\*\*Story Points\*\*|7|Involves displaying user data, allowing modifications, validation, updating records, and handling image uploads.|
|\*\*User Roles\*\*|All System Users|Applicable to all logged-in users.|
|\*\*Access Requirements\*\*|User must be logged in and have access to their personal profile screen.|User must be authenticated.|
|\*\*Trigger\*\*|User clicks profile image/name link in header section.|User initiates the action.|
|\*\*Frequency of Use\*\*|Low to Medium|Users update their profile periodically as needed.|
|\*\*Pre-condition\*\*|User is successfully logged in and has an active session. Their profile data exists.|The system is operational and the user is authenticated.|
|\*\*Business Rules\*\*|1\. All user attributes (Name, Email, Country Code, Mobile No., IBAN, Nationality, CV, Passport No., Personal Photo) should be displayed.<br>2. \*\*Editable Fields:\*\* Name, Email, IBAN, Nationality, CV, Passport No., Personal Photo should be editable.<br>\*\*3. Non-editable Fields:\*\* User ID, Status, Role, country code, Mobile No. should be displayed as labels only (or not displayed if not relevant to user). \*\*Last Update Date, Registration Message Is Sent, Registration Is Completed are NOT displayed on this screen.\*\*<br>4. Email address can only be changed if it remains unique.<br>5. CV upload allows replacement of existing CV (PDF/DOCX, max 10MB).<br>6. Personal Photo upload allows replacement of existing photo (JPG/PNG, max 2MB).<br>7. The 'Last Update Date' must be automatically updated upon successful profile modification.<br>8. A link/button to "Change Password" (User Password Management (Self-Service) story) should be available.|Ensures users can manage their personal data while maintaining data integrity and security.|
|\*\*Post-condition\*\*|User's profile information is updated in the database, the 'Last Update Date' is refreshed, and a success message is displayed. If a photo was uploaded, it is stored and displayed.|The user's personal data is current.|
|\*\*Risk\*\*|1\. Data validation errors leading to incorrect information.<br>2. Unauthorized modification if session is compromised.<br>3. CV/Photo upload issues (e.g., malicious files, large files).|Mitigation: Robust validation, secure session management, clear error messages, secure file storage.|
|\*\*Assumptions\*\*|1\. User data is stored in the system's database.<br>2. CV and Personal Photo files are accessible via a file storage solution.<br>3. The "User Password Management (Self-Service)" story handles password changes.|These assumptions are crucial for implementing the functionality.|
|\*\*UX/UI Design Link\*\*|N/A (General profile screen design)|No specific mockups provided in the BRD for this section.|
### Process Flow Table

|**Step**|**Action Description**|**Actor**|**Related Message Codes**|**Notes**|
| :- | :- | :- | :- | :- |
|1|User is logged in and clicks the profile image/name link in the header section.|User|||
|2|System retrieves the user's personal profile data from the database.|System|||
|3|System displays the "Personal Profile" screen, pre-populating editable fields with current data and displaying non-editable fields as labels. \*\*(Last Update Date, Registration Message Is Sent, Registration Is Completed are not displayed).\*\*|System|||
|4|User modifies editable fields (e.g., Name, Email, IBAN, Nationality, uploads new CV, Passport No., uploads new Personal Photo).|User|MSG-PROFILE-001, MSG-PROFILE-002, MSG-PROFILE-003, , MSG-PROFILE-006, MSG-PROFILE-009|Mobile No. is not editable.|
|5|User clicks "Save Changes" button.|User|||
|6|System performs server-side validation on all submitted data.|System|MSG-PROFILE-001, MSG-PROFILE-002, MSG-PROFILE-003, MSG- PROFILE-006, MSG-PROFILE-009|Checks for mandatory fields, email format/uniqueness, CV file type/size, Personal Photo file type/size.|
|7|If validation is successful: System updates the user's profile data in the database.|System|The 'Last Update Date' is automatically refreshed.||
|8|System displays a success message.|System|MSG-PROFILE-007||
|9|User can click "Change Password" button/link to navigate to the password management screen.|User|This triggers the "User Password Management (Self-Service)" story.||
|10|User can click "Cancel" button to discard changes.|User|||
### Alternative Flow Table

|**Alternative Scenario**|**Condition**|**Action**|**Related Message Codes**|**Resolution**|
| :- | :- | :- | :- | :- |
|\*\*Missing Mandatory Field\*\*|User attempts to save without filling a mandatory field (e.g., Name).|System displays a specific validation error message next to the missing field.|MSG-PROFILE-001|User must fill in all mandatory fields.|
|\*\*Invalid Email Format\*\*|User enters an email address in an incorrect format.|System displays a validation error message for the email field.|MSG-PROFILE-002|User must correct the email format.|
|\*\*Duplicate Email\*\*|User enters an email address that already exists for another user.|System displays an error message indicating duplicate email.|MSG-PROFILE-003|User must enter a unique email address.|
|\*\*CV File Upload Error\*\*|An error occurs during the CV file upload (e.g., file too large, unsupported format).|System displays an error message related to the file upload.|MSG-PROFILE-006|User must re-upload a valid CV file.|
|\*\*Personal Photo Upload Error\*\*|An error occurs during the Personal Photo file upload (e.g., file too large, unsupported format).|System displays an error message related to the photo upload.|MSG-PROFILE-009|User must re-upload a valid photo file.|
|\*\*System Error during Update\*\*|A backend error occurs during profile update.|System displays a generic error message.|MSG-PROFILE-008|User can retry or contact support.|
|\*\*Cancel Changes\*\*|User clicks "Cancel" button after making modifications.|System discards all unsaved changes.|User returns to the profile screen with original data.||
### Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Successful Profile Update (with Photo)\*\*|User is logged in and on their personal profile screen.|The user modifies their Name, uploads a new valid Personal Photo (JPG), and clicks "Save Changes".|The user's profile details are updated in the database, the 'Last Update Date' is refreshed, the new photo is stored and displayed, and a success message "Profile updated successfully." is displayed.|
|\*\*Mandatory Field Validation\*\*|User is on the profile editing screen.|The user clears a mandatory field (e.g., Name) and clicks "Save Changes".|The system displays a specific validation error message "Required Field." for the missing field, and the profile is not updated.|
|\*\*Unique Email Validation\*\*|User is on the profile editing screen.|The user enters an email address that already exists for another user and clicks "Save Changes".|The system displays an error message "User with this email already exists.", and the profile is not updated.|
|\*\*CV Upload Size Limit\*\*|User is on the profile editing screen.|The user attempts to upload a CV file larger than 10 MB.|The system displays an error message "Invalid file format or size for CV. Please upload a PDF or DOCX file up to 10MB.", and the file is not uploaded.|
|\*\*Personal Photo Upload Size Limit\*\*|User is on the profile editing screen.|The user attempts to upload a Personal Photo file larger than 2 MB.|The system displays an error message "Invalid file format or size for Personal Photo. Please upload a JPG or PNG file up to 2MB.", and the file is not uploaded.|
|\*\*Navigate to Change Password\*\*|User is on their personal profile screen.|The user clicks the "Change Password" button/link.|The system navigates the user to the "User Password Management (Self-Service)" screen.|
|\*\*Cancel Changes\*\*|User is on the profile editing screen and has made changes.|The user clicks the "Cancel" button.|The system discards all unsaved changes, and the profile reverts to its original state.|
|\*\*Mobile No. Not Editable\*\*|User is on the profile editing screen.|The user attempts to type into or modify the "Mobile No." field.|The "Mobile No." field is displayed as a non-editable label or disabled input, preventing modification.|
### Data Entities Table
Entity Name: User (for managing profile)

|**Attribute (English)**|**Attribute (Arabic)**|**Mandatory/Optional**|**Attribute Type**|**Data Length**|**Integration Requirements**|**Default Value**|**Condition (if needed)**|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|User ID|معرف المستخدم|Mandatory|Number|N/A|Database Primary Key|N/A|N/A|Unique identifier, Read-only|12345|12345|
|Name|الاسم|Mandatory|Text|Max 255|Database Field|N/A|N/A|N/A|سارة علي|Sara Ali|
|Email|البريد الإلكتروني|Mandatory|Text|Max 255|Database Field|N/A|N/A|Unique, Valid email format|sara.ali@jadwa.com|sara.ali@jadwa.com|
|Country Code|رمز الدولة|Mandatory|Text|Max 5|Database Field|+966|N/A|Must be '+966' for Saudi numbers.|+966|+966|
|Mobile|رقم الجوال|Mandatory|Text|Max 10|Database Field|N/A|N/A|Saudi mobile format (e.g., 05XXXXXXXX), Numeric only, Unique (as username). \*\*(Read-only)\*\*|0501234567|0501234567|
|IBAN|رقم الحساب المصرفي الدولي|Optional|Text|Max 34|Database Field|N/A|N/A|Valid IBAN format|SA9876543210987654321098|SA9876543210987654321098|
|Nationality|الجنسية|Optional|Text|Max 100|Database Field|N/A|N/A|N/A|مصري|Egyptian|
|CV|السيرة الذاتية|Optional|File Path/URL|N/A|File Storage (e.g., Azure Blob)|N/A|N/A|PDF, DOCX only, Max 10MB|سيرة\_ذاتية\_سارة.pdf|Sara\_CV.pdf|
|Passport No.|رقم جواز السفر|Optional|Text|Max 20|Database Field|N/A|N/A|Alphanumeric|B98765432|B98765432|
|Personal Photo|الصورة الشخصية|Optional|Image File Path/URL|N/A|File Storage (e.g., Azure Blob)|N/A|N/A|JPG/PNG only, Max 2MB.|صورة\_سارة.jpg|Sara\_Photo.jpg|
|Status|الحالة|Mandatory|Boolean/Dropdown|N/A|Database Field|Active|N/A|Active/Inactive (Read-only)|نشط|Active|
|Role|الدور|Mandatory|Relation to Role Entity|N/A|Database Many-to-Many|N/A|At least one role must be selected (Read-only)|Predefined roles only|مستشار قانوني|Legal Counsel|
|Last Update Date|تاريخ آخر تحديث|Mandatory|DateTime|N/A|Database Field|Current Timestamp|N/A|Automatically set on creation and update.|2023-10-26 15:00:00|2023-10-26 15:00:00|
|Registration Message Is Sent|تم إرسال رسالة التسجيل|Mandatory|Boolean|N/A|Database Field|0 (False)|N/A|Set to 1 if user's role(s) make them eligible for WhatsApp message (NOT only 'Board Member'); 0 if only 'Board Member'. This flag is set based on eligibility and attempt to send. (Read-only)|1|True|
|Registration Is Completed|تم اكتمال التسجيل|Mandatory|Boolean|N/A|Database Field|0 (False)|N/A|Set to 0 upon user creation. Set to 1 after successful password reset. (Read-only)|0|False|
### Messages/Notifications Table

|**Message Code**|**Message (English)**|**Message (Arabic)**|**Message Type**|**Communication Method**|
| :- | :- | :- | :- | :- |
|MSG-PROFILE-001|Required Field.|حقل إلزامي.|Validation Error|In-App|
|MSG-PROFILE-002|Invalid email format.|صيغة البريد الإلكتروني غير صحيحة.|Validation Error|In-App|
|MSG-PROFILE-003|User with this email already exists.|يوجد مستخدم بهذا البريد الإلكتروني بالفعل.|Validation Error|In-App|
|MSG-PROFILE-006|Invalid file format or size for CV. Please upload a PDF or DOCX file up to 10MB.|صيغة الملف أو حجمه غير صالح للسيرة الذاتية. يرجى تحميل ملف PDF أو DOCX بحجم أقصى 10 ميجابايت.|Validation Error|In-App|
|MSG-PROFILE-007|Profile updated successfully.|تم تحديث الملف الشخصي بنجاح.|Success Message|In-App|
|MSG-PROFILE-008|An error is occurred while saving data.|حدث خطأ بالنظام , لم يتم حفظ البيانات.|Error Message|In-App|
|MSG-PROFILE-009|Invalid file format or size for Personal Photo. Please upload a JPG or PNG file up to 2MB.|صيغة الملف أو حجمه غير صالح للصورة الشخصية. يرجى تحميل ملف JPG أو PNG بحجم أقصى 2 ميجابايت.|Validation Error|In-App|
### Screen Elements Table

|**Element ID**|**Element Type**|**Element Name (English)**|**Element Name (Arabic)**|**Required/Optional**|**Validation Rules**|**Business Logic**|**Related Data Entity**|**User Interaction**|**Accessibility Notes**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|ELM-PROFILE-001|Page Title|My Profile|ملفي الشخصي|N/A|N/A|Displays the title of the personal profile page.|N/A|View|H1 heading.|
|ELM-PROFILE-002|Image Display/Upload Area|Personal Photo|الصورة الشخصية|Optional|JPG/PNG only, Max 2MB.|Displays current photo or placeholder. Allows user to upload/replace photo.|User.PersonalPhoto|Click (to browse/upload)|Clear label, accessible upload.|
|ELM-PROFILE-003|Input Field|Name Input|حقل الاسم|Mandatory|Text, Max 255 chars|Collects/displays the user's full name.|User.Name|Type|Clear label, placeholder text.|
|ELM-PROFILE-004|Input Field|Email Input|حقل البريد الإلكتروني|Mandatory|Valid email format, Max 255 chars|Collects/displays the user's email address. Must be unique.|User.Email|Type|Clear label, placeholder text, email type keyboard.|
|ELM-PROFILE-005|Text Display|Country Code |حقل رمز الدولة|Mandatory|N/A|displays the country dialing code for mobile number.|User.CountryCode|View|Clear label, placeholder text.|
|ELM-PROFILE-006|Text Display|Mobile No. Display (Username)|عرض رقم الجوال (اسم المستخدم)|Mandatory|N/A|Displays the user's Saudi mobile phone number (username) as a non-editable label.|User.Mobile|View|Clear label, ensure sufficient contrast.|
|ELM-PROFILE-007|Input Field|IBAN Input|حقل رقم الحساب المصرفي الدولي|Optional|Valid IBAN format, Max 34 chars|Collects/displays the user's International Bank Account Number.|User.IBAN|Type|Clear label, placeholder text.|
|ELM-PROFILE-008|Input Field|Nationality Input|حقل الجنسية|Optional|Text, Max 100 chars|Collects/displays the user's nationality.|User.Nationality|Type|Clear label, placeholder text.|
|ELM-PROFILE-009|File Upload|CV Upload|تحميل السيرة الذاتية|Optional|PDF/DOCX only, Max 10MB|Allows user to upload/replace their CV file.|User.CV|Click (to browse/upload)|Clear label, file type/size guidance.|
|ELM-PROFILE-010|Input Field|Passport No. Input|حقل رقم جواز السفر|Optional|Alphanumeric, Max 20 chars|Collects/displays the user's passport number.|User.PassportNo|Type|Clear label, placeholder text.|
|ELM-PROFILE-011|Text Label|Status Display|عرض الحالة|Mandatory|N/A|Displays the user's current status (Active/Inactive) as a non-editable label.|User.Status|View|Clear label, ensure sufficient contrast.|
|ELM-PROFILE-012|Text Label|Role Display|عرض الدور|Mandatory|N/A|Displays the user's primary role or list of roles as a non-editable label.|User.Role|View|Ensure sufficient contrast.|
|ELM-PROFILE-013|Button|Save Changes Button|زر حفظ التغييرات|Mandatory|N/A|Submits the form to update the profile.|N/A|Click|Primary action button.|
|ELM-PROFILE-014|Button|Cancel Button|زر إلغاء|Mandatory|N/A|Discards changes and returns to the profile screen.|N/A|Click|Secondary action button.|
|ELM-PROFILE-015|Link|Change Password Link|رابط تغيير كلمة المرور|Mandatory|N/A|Navigates to the "User Password Management (Self-Service)" screen.|N/A|Click|Clear link text.|
|ELM-PROFILE-016|Text Label|Error Message Display|عرض رسالة الخطأ|Conditional|N/A|Displays validation or system error messages.|N/A|View|Prominent display.|
|ELM-PROFILE-017|Text Label|Success Message Display|عرض رسالة النجاح|Conditional|N/A|Displays confirmation of successful profile update.|N/A|View|Prominent display.|
### Summary Section
This user story for "Manage Personal Profile" has been updated to remove the display elements for `Registration Is Completed`, `Registration Message Is Sent`, and `Last Update Date` from the \*\*Screen Elements Table\*\*. This ensures that these system-managed flags and dates are not directly visible on the user's personal profile UI, while still existing in the underlying data model.

\*\*Important Note:\*\* I am still awaiting your clarification on the "Action Not Applicable (Role/Assignment)" logic for the "Resend Account Registration Message" user story from our previous interaction. The current logic in that story (and reflected in its Acceptance Criteria) is: "Action is NOT applicable if the user has ONLY the 'Board Member' role AND is NOT assigned to any fund board." However, with `IsAssignedToFundBoard` removed from the User entity, this specific condition cannot be implemented as written. \*\*Please confirm how the "Resend Message" eligibility for Board Members should be determined without `IsAssignedToFundBoard`.\*\*

\*\*Key Implementation Considerations:\*\*

\*   Ensure all data validation rules are strictly enforced for editable fields.

\*   The CV upload/replacement mechanism must be robust and secure.

\*   Non-editable fields should be clearly presented as such (e.g., as labels, disabled inputs).

\*   The "Change Password" link must correctly navigate to the "User Password Management (Self-Service)" story.

\*\*Dependencies and Risks:\*\*

\*   \*\*Dependency:\*\* This story depends on the "User Login" story (for authentication) and the "User Password Management (Self-Service)" story (for password changes).

\*   \*\*Risk:\*\* Data corruption if validation is insufficient or if concurrent updates are not handled. Mitigation: Database transactions, robust validation.

\---

I have now updated the "Manage Personal Profile" user story with the requested removals from the Screen Elements Table. Please provide clarification on the "Resend Account Registration Message" logic regarding Board Members when you're ready.

