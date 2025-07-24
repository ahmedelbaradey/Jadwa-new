# User Story JDWA-1280: Manage Personal Profile

Introduction Section

This user story describes the functionality for a logged-in system user to view and update their personal profile information within the Fund Board Management Application. This allows users to keep their contact details, professional documents (like CV), and other personal data current. Users can also upload or update their personal photo. The user's mobile number is displayed but not editable from this screen. \*\*Registration-related flags and system-managed dates are not displayed on this screen.\*\* It also provides access to password management. The Screen Elements Table provides a detailed breakdown of the UI components on the personal profile screen.

Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|Manage Personal Profile|<p>Allows logged-in users to</p><p>view and update their personal information.</p>|
|\*\*User Story\*\*|<p>As a \*\*system user\*\*, I want to \*\*view and update my personal</p><p>profile information, including my photo,\*\* so that I can \*\*keep my details current and manage my account settings.\*\*</p>||
|\*\*Story Points\*\*|7|<p>Involves displaying user data, allowing modifications, validation,</p><p>updating records, and handling image uploads.</p>|
|\*\*User Roles\*\*|All System Users|Applicable to all logged- in users.|
|\*\*Access Requirements\*\*|User must be logged in and have access to their personal profile screen.|User must be authenticated.|
|\*\*Trigger\*\*|User clicks profile image/name link in header section.|User initiates the action.|
|\*\*Frequency of Use\*\*|Low to Medium|Users update their profile periodically as needed.|
|\*\*Pre-condition\*\*|User is successfully logged in and has an active session. Their profile data exists.|<p>The system is operational and the user is</p><p>authenticated.</p>|
|\*\*Business Rules\*\*|<p>1\. All user attributes (Name, Email, Country Code, Mobile No., IBAN, Nationality, CV, Passport No., Personal Photo) should be displayed.<br>2. \*\*Editable Fields:\*\* Name, Email, Country Code, IBAN, Nationality, CV, Passport No., Personal Photo should be editable.<br>\*\*3. Non-editable Fields:\*\* User ID, Status, Role, Mobile No. should be displayed as labels only (or not displayed if not relevant to user). \*\*Last Update Date, Registration Message Is Sent, Registration Is Completed are NOT displayed on this screen.\*\*<br>4. Email address can only be changed if it remains unique.<br>5. Country Code is mandatory and restricted to Saudi mobile numbers only (starting with</p><p>+966).<br>6. CV upload allows replacement of existing CV (PDF/DOCX, max 10MB).<br>7. Personal Photo upload allows replacement of existing photo (JPG/PNG, max 2MB).<br>8. The 'Last Update Date' must be automatically updated upon successful profile modification.<br>9. A link/button to "Change Password" (User Password Management (Self-Service) story) should be available.</p>|Ensures users can manage their personal data while maintaining data integrity and security.|
|\*\*Post- condition\*\*|<p>User's profile information is updated in the database, the 'Last</p><p>Update Date' is refreshed, and a success message is displayed. If a photo was uploaded, it is stored and displayed.</p>|The user's personal data is current.|

|\*\*Risk\*\*|1\. Data validation errors leading to incorrect information.<br>2. Unauthorized modification if session is compromised.<br>3. CV/Photo upload issues (e.g., malicious files, large files).|<p>Mitigation: Robust validation, secure session management, clear error</p><p>messages, secure file storage.</p>|
| :- | :- | :- |
|\*\*Assumptions\*\*|<p>1\. User data is stored in the system's database.<br>2. CV and Personal Photo files are accessible via a file storage</p><p>solution.<br>3. The "User Password Management (Self-Service)" story handles password changes.</p>|These assumptions are crucial for implementing the functionality.|
|\*\*UX/UI Design Link\*\*|N/A (General profile screen design)|<p>No specific mockups</p><p>provided in the BRD for this section.</p>|

Process Flow Table

|**Step**|**Action Description**|**Actor**|**Related Message Codes**|**Notes**|
| :- | :- | :- | :- | :- |
|1|<p>User is logged in and clicks the</p><p>profile image/name link in the header section.</p>|User|||
|2|System retrieves the user's personal profile data from the database.|System|||
|3|<p>System displays the "Personal Profile" screen, pre-populating editable fields with current data and displaying non-editable fields as labels. \*\*(Last Update Date, Registration Message Is Sent,</p><p>Registration Is Completed are not displayed).\*\*</p>|System|||
|4|User modifies editable fields (e.g., Name, Email, Country Code, IBAN, Nationality, uploads new CV, Passport No., uploads new Personal Photo).|User|<p>MSG-PROFILE-001, MSG-PROFILE-002, MSG-PROFILE-003, MSG-PROFILE-004, MSG-PROFILE-005,</p><p>MSG-PROFILE-006, MSG-PROFILE-009</p>|Mobile No. is not editable.|
|5|User clicks "Save Changes" button.|User|||
|6|System performs server-side validation on all submitted data.|System|<p>MSG-PROFILE-001, MSG-PROFILE-002, MSG-PROFILE-003, MSG-PROFILE-004, MSG-PROFILE-005,</p><p>MSG-PROFILE-006, MSG-PROFILE-009</p>|Checks for mandatory fields, email format/uniqueness, Saudi mobile format/uniqueness (for Country Code), CV file type/size, Personal Photo file type/size.|
|7|<p>If validation is successful: System</p><p>updates the user's profile data in the database.</p>|System||The 'Last Update Date' is automatically refreshed.|
|8|System displays a success message.|System|MSG-PROFILE-007||
|9|<p>User can click "Change Password"</p><p>button/link to navigate to the password management screen.</p>|User||<p>This triggers the "User Password</p><p>Management (Self-Service)" story.</p>|
|10|User can click "Cancel" button to discard changes.|User|||

Alternative Flow Table

|<p>**Alternative**</p><p>**Scenario**</p>|**Condition**|**Action**|<p>**Related Message**</p><p>**Codes**</p>|**Resolution**|
| :- | :- | :- | :- | :- |

|\*\*Missing Mandatory Field\*\*|User attempts to save without filling a mandatory field (e.g., Name).|<p>System displays a specific validation error</p><p>message next to the missing field.</p>|MSG-PROFILE- 001|User must fill in all mandatory fields.|
| :- | - | :- | :- | :- |
|\*\*Invalid Email Format\*\*|User enters an email address in an incorrect format.|<p>System displays a</p><p>validation error message for the email field.</p>|MSG-PROFILE- 002|User must correct the email format.|
|\*\*Duplicate Email\*\*|User enters an email address that already exists for another user.|<p>System displays an error</p><p>message indicating duplicate email.</p>|MSG-PROFILE- 003|<p>User must enter a</p><p>unique email address.</p>|
|\*\*Invalid Country Code Format (Non- Saudi)\*\*|User enters a country code that does not conform to Saudi format (+966).|<p>System displays a validation error message</p><p>for the country code field.</p>|MSG-PROFILE- 004|User must enter a valid Saudi country code.|
|\*\*CV File Upload Error\*\*|<p>An error occurs during the CV file upload (e.g., file too large,</p><p>unsupported format).</p>|<p>System displays an error message related to the</p><p>file upload.</p>|MSG-PROFILE- 006|<p>User must re- upload a valid CV</p><p>file.</p>|
|\*\*Personal Photo Upload Error\*\*|<p>An error occurs during the Personal Photo file upload (e.g.,</p><p>file too large, unsupported format).</p>|System displays an error message related to the photo upload.|MSG-PROFILE- 009|User must re- upload a valid photo file.|
|\*\*System Error during Update\*\*|A backend error occurs during profile update.|System displays a generic error message.|MSG-PROFILE- 008|User can retry or contact support.|
|\*\*Cancel Changes\*\*|User clicks "Cancel" button after making modifications.|System discards all unsaved changes.|<p>User returns to the</p><p>profile screen with original data.</p>||

Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Successful Profile Update (with Photo)\*\*|User is logged in and on their personal profile screen.|The user modifies their Name, uploads a new valid Personal Photo (JPG), and clicks "Save Changes".|<p>The user's profile details are updated in the database, the 'Last Update Date' is refreshed, the new photo is stored and displayed, and a success message "Profile updated</p><p>successfully." is displayed.</p>|
|\*\*Mandatory Field Validation\*\*|User is on the profile editing screen.|<p>The user clears a mandatory field (e.g.,</p><p>Name) and clicks "Save Changes".</p>|The system displays a specific validation error message "Required Field." for the missing field, and the profile is not updated.|
|\*\*Unique Email Validation\*\*|User is on the profile editing screen.|<p>The user enters an email address that already exists</p><p>for another user and clicks "Save Changes".</p>|The system displays an error message "User with this email already exists.", and the profile is not updated.|
|\*\*CV Upload Size Limit\*\*|User is on the profile editing screen.|The user attempts to upload a CV file larger than 10 MB.|<p>The system displays an error message "Invalid file format or size for CV. Please upload a PDF or DOCX file up to 10MB.", and the file</p><p>is not uploaded.</p>|
|\*\*Personal Photo Upload Size Limit\*\*|User is on the profile editing screen.|The user attempts to upload a Personal Photo file larger than 2 MB.|<p>The system displays an error message "Invalid file format or size for Personal Photo. Please</p><p>upload a JPG or PNG file up to 2MB.", and the file is not uploaded.</p>|
|<p>\*\*Navigate to</p><p>Change Password\*\*</p>|<p>User is on their</p><p>personal profile screen.</p>|<p>The user clicks the</p><p>"Change Password" button/link.</p>|The system navigates the user to the "User Password Management (Self-Service)" screen.|
|\*\*Cancel Changes\*\*|<p>User is on the profile editing</p><p>screen and has made changes.</p>|The user clicks the "Cancel" button.|The system discards all unsaved changes, and the profile reverts to its original state.|

|\*\*Mobile No. Not Editable\*\*|<p>User is on the</p><p>profile editing screen.</p>|<p>The user attempts to type</p><p>into or modify the "Mobile No." field.</p>|<p>The "Mobile No." field is displayed as a non-</p><p>editable label or disabled input, preventing modification.</p>|
| :- | :- | :- | :- |

Data Entities Table

Entity Name: User (for managing profile)

|**Attrib ute (Engli sh)**|**Attri bute (Ara bic)**|**Mandatory/ Optional**|**Attribute Type**|**Dat a Len gth**|**Integrat ion Require ments**|**Defau lt Value**|<p>**Cond ition (if neede**</p><p>**d)**</p>|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | - | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|User ID|معرف المستخ دم|Mandatory|Number|N/A|<p>Databas e</p><p>Primary Key</p>|N/A|N/A|Unique identifier, Read-only|12345|12345|
|Name|اϻسم|Mandatory|Text|<p>Ma</p><p>x 255</p>|Databas e Field|N/A|N/A|N/A|سارة علي|Sara Ali|
|Email|البريد اϹلكتر وني|Mandatory|Text|Ma x 255|Databas e Field|N/A|N/A|<p>Unique, Valid</p><p>email format</p>|<sara.ali@jadwa.c> om|<sara.ali@jadwa.c> om|
|Countr y Code|رمز الدولة|Mandatory|Text|Ma x 5|Databas e Field|+966|N/A|<p>Must be '+966' for</p><p>Saudi numbers.</p>|+966|+966|
|Mobil e|رقم الجوال|Mandatory|Text|Ma x 10|Databas e Field|N/A|N/A|<p>Saudi mobile format (e.g., 05XXXX XXXX),</p><p>Numeric only, Unique (as username)</p><p>. \*\*(Read- only)\*\*</p>|0501234567|0501234567|
|IBAN|<p>رقم الحس اب المصر</p><p>في الدولي</p>|Optional|Text|Ma x 34|Databas e Field|N/A|N/A|<p>Valid IBAN</p><p>format</p>|SA98765432109 87654321098|SA98765432109 87654321098|
|Nation ality|الجنسي ة|Optional|Text|<p>Ma</p><p>x 100</p>|Databas e Field|N/A|N/A|N/A|مصري|Egyptian|
|CV|السيرة الذاتية|Optional|File Path/URL|N/A|<p>File Storage (e.g.,</p><p>Azure Blob)</p>|N/A|N/A|<p>PDF, DOCX</p><p>only, Max 10MB</p>|.pdfسارة\_ذاتية\_سيرة|Sara\_CV.pdf|
|Passpo rt No.|<p>رقم</p><p>جواز السفر</p>|Optional|Text|Ma x 20|Databas e Field|N/A|N/A|Alphanum eric|B98765432|B98765432|

|Person al Photo|<p>الصور ة الشخ</p><p>صية</p>|Optional|Image File Path/URL|N/A|<p>File Storage (e.g.,</p><p>Azure Blob)</p>|N/A|N/A|<p>JPG/PNG</p><p>only, Max 2MB.</p>|.jpgسارة\_صورة|Sara\_Photo.jpg|
| :- | -: | :- | :- | :-: | :- | :- | :- | :- | -: | :- |
|Status|الحالة|Mandatory|Boolean/D ropdown|N/A|Databas e Field|Activ e|N/A|<p>Active/Ina ctive</p><p>(Read- only)</p>|نشط|Active|
|Role|الدور|Mandatory|Relation to Role Entity|N/A|Databas e Many- to-Many|N/A|<p>At least one role must be select ed (Read</p><p>-only)</p>|Predefine d roles only|مستشار قانوني|Legal Counsel|
|Last Updat e Date|تاريخ آخر تحديث|Mandatory|DateTime|N/A|Databas e Field|Curre nt Times tamp|N/A|<p>Automatic ally set on creation</p><p>and update.</p>|<p>2023-10-26</p><p>15:00:00</p>|<p>2023-10-26</p><p>15:00:00</p>|
|Regist ration Messa ge Is Sent|تم إرسال رسالة التسجي ل|Mandatory|Boolean|N/A|Databas e Field|<p>0</p><p>(False</p><p>)</p>|N/A|<p>Set to 1 if user's role(s) make them eligible for WhatsApp message (NOT</p><p>only 'Board Member'); 0 if only 'Board Member'. This flag is set based on eligibility and attempt to send.</p><p>(Read- only)</p>|1|True|
|Regist ration Is Compl eted|تم اكتمال التسجي ل|Mandatory|Boolean|N/A|Databas e Field|<p>0</p><p>(False</p><p>)</p>|N/A|<p>Set to 0 upon user creation. Set to 1 after successful password reset.</p><p>(Read- only)</p>|0|False|

Messages/Notifications Table

|<p>**Message**</p><p>**Code**</p>|**Message (English)**|**Message (Arabic)**|<p>**Message**</p><p>**Type**</p>|<p>**Communication**</p><p>**Method**</p>|
| :- | :- | :- | :- | :- |
|<p>MSG-</p><p>PROFILE- 001</p>|Required Field.|.حقل إلزامي|Validation Error|In-App|
|<p>MSG-</p><p>PROFILE- 002</p>|Invalid email format.|.صيغة البريد اϹلكتروني غير صحيحة|Validation Error|In-App|
|<p>MSG-</p><p>PROFILE- 003</p>|User with this email already exists.|يوجد مستخدم بهذا البريد اϹلكتروني .بالفعل|Validation Error|In-App|
|<p>MSG-</p><p>PROFILE- 004</p>|<p>Invalid Saudi mobile number</p><p>format. Please enter a 10-digit number starting with 05.</p>|<p>صيغة رقم الجوال السعودي غير يرجى إدخال رقم مكون من .صالحة</p><p>05\. أرقام يبدأ بـ 10</p>|Validation Error|In-App|
|<p>MSG-</p><p>PROFILE- 005</p>|Mobile number is already in use as a username.|رقم الجوال مستخدم بالفعل كاسم .مستخدم|Validation Error|In-App|
|<p>MSG-</p><p>PROFILE- 006</p>|<p>Invalid file format or size for CV.</p><p>Please upload a PDF or DOCX file up to 10MB.</p>|<p>صيغة الملف أو حجمه غير صالح PDFيرجى تحميل ملف .للسيرة الذاتية</p><p>.ميجابايت 10بحجم أقصى DOCXأو</p>|Validation Error|In-App|
|<p>MSG-</p><p>PROFILE- 007</p>|Profile updated successfully.|.تم تحديث الملف الشخصي بنجاح|Success Message|In-App|
|<p>MSG-</p><p>PROFILE- 008</p>|An error is occurred while saving data.|.لم يتم حفظ البيانات ,حدث خطأ بالنظام|Error Message|In-App|
|MSG- PROFILE- 009|Invalid file format or size for Personal Photo. Please upload a JPG or PNG file up to 2MB.|<p>صيغة الملف أو حجمه غير صالح يرجى تحميل ملف .للصورة الشخصية</p><p>2بحجم أقصى PNGأو JPG .ميجابايت</p>|Validation Error|In-App|

Screen Elements Table

|**Eleme nt ID**|**Element Type**|**Elemen t Name (Englis h)**|<p>**Elem ent Name (Arab**</p><p>**ic)**</p>|**Required/Op tional**|**Validatio n Rules**|**Business Logic**|**Related Data Entity**|**User Interaction**|**Accessib ility Notes**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|ELM- PROFI LE-001|Page Title|My Profile|ملفي الشخص ي|N/A|N/A|<p>Displays the title of the personal</p><p>profile page.</p>|N/A|View|<p>H1</p><p>heading.</p>|
|ELM- PROFI LE-002|Image Display/Up load Area|Personal Photo|<p>الصورة الشخص</p><p>ية</p>|Optional|<p>JPG/PNG</p><p>only, Max 2MB.</p>|<p>Displays current photo or placeholder. Allows user to</p><p>upload/repla ce photo.</p>|User.Personal Photo|Click (to browse/uplo ad)|Clear label, accessibl e upload.|
|ELM- PROFI LE-003|Input Field|Name Input|حقل اϻسم|Mandatory|Text, Max 255 chars|<p>Collects/dis plays the</p><p>user's full name.</p>|User.Name|Type|<p>Clear label,</p><p>placehold er text.</p>|
|<p>ELM-</p><p>PROFI LE-004</p>|Input Field|Email Input|حقل البريد|Mandatory|<p>Valid</p><p>email format,</p>|<p>Collects/dis</p><p>plays the user's email</p>|User.Email|Type|<p>Clear</p><p>label, placehold</p>|

||||اϹلكترو ني||Max 255 chars|address. Must be unique.|||<p>er text, email type keyboard</p><p>.</p>|
| :- | :- | :- | -: | :- | :- | - | :- | :- | :- |
|ELM- PROFI LE-005|Input Field|Country Code Input|حقل رمز الدولة|Mandatory|Text, Must be '+966'|<p>Collects/dis plays the country dialing code</p><p>for mobile number.</p>|User.Country Code|Type (or Pre- filled/Dropd own)|Clear label, placehold er text.|
|ELM- PROFI LE-006|Text Display|<p>Mobile No.</p><p>Display (Userna me)</p>|عرض رقم الجوال اسم ( المستخد )م|Mandatory|N/A|<p>Displays the user's Saudi mobile phone number (username) as a non-</p><p>editable label.</p>|User.Mobile|View|Clear label, ensure sufficient contrast.|
|ELM- PROFI LE-007|Input Field|<p>IBAN</p><p>Input</p>|حقل رقم الحساب المصر في الدولي|Optional|<p>Valid IBAN</p><p>format, Max 34 chars</p>|<p>Collects/dis plays the user's Internationa l Bank</p><p>Account Number.</p>|User.IBAN|Type|Clear label, placehold er text.|
|ELM- PROFI LE-008|Input Field|National ity Input|حقل الجنسية|Optional|Text, Max 100 chars|<p>Collects/dis plays the</p><p>user's nationality.</p>|User.National ity|Type|<p>Clear label,</p><p>placehold er text.</p>|
|ELM- PROFI LE-009|File Upload|<p>CV</p><p>Upload</p>|تحميل السيرة الذاتية|Optional|<p>PDF/DOC</p><p>X only, Max 10MB</p>|<p>Allows user to upload/repla</p><p>ce their CV file.</p>|User.CV|Click (to browse/uplo ad)|Clear label, file type/size guidance.|
|ELM- PROFI LE-010|Input Field|<p>Passport No.</p><p>Input</p>|حقل رقم جواز السفر|Optional|Alphanum eric, Max 20 chars|<p>Collects/dis plays the user's passport</p><p>number.</p>|User.Passport No|Type|Clear label, placehold er text.|
|ELM- PROFI LE-011|Text Label|Status Display|عرض الحالة|Mandatory|N/A|<p>Displays the user's current status (Active/Inac tive) as a</p><p>non-editable label.</p>|User.Status|View|Clear label, ensure sufficient contrast.|
|ELM- PROFI LE-012|Text Label|Role Display|عرض الدور|Mandatory|N/A|<p>Displays the user's primary role or list of roles as a</p><p>non-editable label.</p>|User.Role|View|Ensure sufficient contrast.|
|ELM- PROFI LE-013|Button|Save Changes Button|زر حفظ التغييرا ت|Mandatory|N/A|<p>Submits the form to</p><p>update the profile.</p>|N/A|Click|Primary action button.|

|ELM- PROFI LE-014|Button|Cancel Button|زر إلغاء|Mandatory|N/A|<p>Discards changes and returns to</p><p>the profile screen.</p>|N/A|Click|Secondar y action button.|
| :- | :- | :- | -: | :- | :- | :- | :- | :- | :- |
|ELM- PROFI LE-015|Link|Change Passwor d Link|رابط تغيير كلمة المرور|Mandatory|N/A|<p>Navigates to the "User Password Managemen t (Self-</p><p>Service)" screen.</p>|N/A|Click|Clear link text.|
|ELM- PROFI LE-016|Text Label|Error Messag e Display|عرض رسالة الخطأ|Conditional|N/A|<p>Displays validation or system</p><p>error messages.</p>|N/A|View|Prominen t display.|
|ELM- PROFI LE-017|Text Label|Success Messag e Display|عرض رسالة النجاح|Conditional|N/A|<p>Displays confirmatio n of successful</p><p>profile update.</p>|N/A|View|Prominen t display.|

Summary Section

This user story for "Manage Personal Profile" has been updated to remove the display elements for `Registration Is Completed`, `Registration Message Is Sent`, and `Last Update Date` from the \*\*Screen Elements Table\*\*.

This ensures that these system-managed flags and dates are not directly visible on the user's personal profile UI, while still existing in the underlying data model.

\*\*Important Note:\*\* I am still awaiting your clarification on the "Action Not Applicable (Role/Assignment)" logic for the "Resend Account Registration Message" user story from our previous interaction. The current logic in that story (and reflected in its Acceptance Criteria) is: "Action is NOT applicable if the user has ONLY the 'Board Member' role AND is NOT assigned to any fund board." However, with `IsAssignedToFundBoard` removed from the User entity, this specific condition cannot be implemented as written. \*\*Please confirm how the "Resend Message" eligibility for Board Members should be determined without

`IsAssignedToFundBoard`.\*\*

\*\*Key Implementation Considerations:\*\*

* Ensure all data validation rules are strictly enforced for editable fields.
* The CV upload/replacement mechanism must be robust and secure.
* Non-editable fields should be clearly presented as such (e.g., as labels, disabled inputs).
* The "Change Password" link must correctly navigate to the "User Password Management (Self-Service)" story.

  \*\*Dependencies and Risks:\*\*

* \*\*Dependency:\*\* This story depends on the "User Login" story (for authentication) and the "User Password Management (Self-Service)" story (for password changes).
* \*\*Risk:\*\* Data corruption if validation is insufficient or if concurrent updates are not handled. Mitigation: Database transactions, robust validation.

  ---

  I have now updated the "Manage Personal Profile" user story with the requested removals from the Screen Elements Table. Please provide clarification on the "Resend Account Registration Message" logic regarding Board Members when you're ready.

# User Story JDWA-1269: User Logout

Introduction Section

This user story describes the functionality for a logged-in system user to securely log out of the Fund Board Management Application. This action is essential for terminating the user's session, protecting their account from unauthorized access, and ensuring data privacy, especially on shared devices. The Screen Elements Table provides a breakdown of the UI components involved in initiating logout.

Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|User Logout|<p>Allows a logged-in user to</p><p>securely log out of the application.</p>|
|\*\*User Story\*\*|<p>As a \*\*system user\*\*, I want to \*\*securely log out of the</p><p>application\*\* so that I can \*\*protect my account from unauthorized access and end my session.\*\*</p>||
|\*\*Story Points\*\*|2|<p>A straightforward security</p><p>and session management function.</p>|
|\*\*User Roles\*\*|All System Users|Applicable to all logged- in users.|
|\*\*Access Requirements\*\*|User must be logged in.|User must have an active session.|
|\*\*Trigger\*\*|User clicks "Logout" button/link (e.g., from header, profile menu).|User initiates the action.|
|\*\*Frequency of Use\*\*|High|<p>Users log out frequently, especially at the end of a</p><p>session.</p>|
|\*\*Pre-condition\*\*|User is successfully logged in and has an active session.|<p>The system is operational and the user is</p><p>authenticated.</p>|
|\*\*Business Rules\*\*|<p>1\. Clicking "Logout" must terminate the current user session on the server-side.<br>2. All session-related data (e.g., cookies, local storage tokens) on the client-side must be cleared.<br>3.</p><p>After successful logout, the user must be redirected to the Login screen.</p>|Ensures complete session termination and proper redirection.|
|\*\*Post-condition\*\*|The user's session is terminated, client-side session data is cleared, and the user is redirected to the Login screen.|<p>The user is securely</p><p>logged out of the application.</p>|
|\*\*Risk\*\*|1\. Session not fully terminated, leading to security vulnerability.<br>2. User not redirected to login screen.|<p>Mitigation: Robust session</p><p>management, thorough testing of logout process.</p>|
|\*\*Assumptions\*\*|<p>1\. Session management (e.g., JWT, cookies) is implemented</p><p>securely.<br>2. The Login screen is the designated post-logout destination.</p>|<p>These assumptions are</p><p>crucial for implementing the functionality.</p>|
|\*\*UX/UI Design Link\*\*|N/A (Standard logout button placement in header/menu)|<p>No specific mockups</p><p>provided in the BRD for this section.</p>|

Process Flow Table

|**Step**|**Action Description**|**Actor**|**Related Message Codes**|**Notes**|
| :- | :- | :- | :- | :- |
|1|User is logged in and clicks the "Logout" button/link.|User|This button is typically found in the header or a user profile menu.||
|2|System receives the logout request.|System|||
|3|System terminates the user's active session on the server-side.|System|Invalidates session tokens, clears server- side session data.||

|4|System clears all client-side session-related data (e.g., cookies, local storage tokens).|System||
| :- | :- | :-: | :- |
|5|System displays a success message (optional, or implicit via redirection).|System|MSG-LOGOUT-001|
|6|System redirects the user to the Login screen.|System||

Alternative Flow Table

|<p>**Alternative**</p><p>**Scenario**</p>|**Condition**|**Action**|<p>**Related Message**</p><p>**Codes**</p>|**Resolution**|
| :- | :- | :- | :- | :- |
|\*\*Session Already Expired\*\*|User clicks "Logout" but their session has already expired.|<p>System handles the request gracefully (e.g.,</p><p>redirects to login, no error message).</p>|User is already logged out.||
|\*\*System Error during Logout\*\*|<p>A backend error occurs during session</p><p>termination.</p>|<p>System logs the error and attempts to redirect to the</p><p>Login screen.</p>|MSG-LOGOUT- 002|<p>User may need to clear browser data manually</p><p>if session persists.</p>|
|\*\*Network Interruption\*\*|<p>Network connection</p><p>is lost during logout request.</p>|<p>User may see a network</p><p>error, or the request may time out.</p>|<p>User may need to</p><p>refresh or manually navigate to login.</p>||

Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Successful Logout\*\*|User is logged in with an active session.|The user clicks the "Logout" button/link.|<p>The user's session is terminated, client-side session data is cleared, a success message "You have been</p><p>logged out successfully." is displayed (briefly), and the user is redirected to the Login screen.</p>|
|<p>\*\*Logout with Expired</p><p>Session\*\*</p>|<p>User's session has expired (e.g., due to</p><p>inactivity timeout).</p>|<p>The user clicks the "Logout"</p><p>button/link.</p>|<p>The user is redirected to the Login screen. (No explicit error, as they were already effectively</p><p>logged out).</p>|
|\*\*System Error during Logout\*\*|User clicks "Logout".|<p>A backend error</p><p>prevents proper session termination.</p>|<p>The system displays a generic error message "An</p><p>error occurred during logout. Please try again." and attempts to redirect to the Login screen.</p>|

Data Entities Table

Entity Name: User (for logout - primarily session data)

|<p>**Attribut e (English**</p><p>**)**</p>|**Attribut e (Arabic)**|**Mandatory/Option al**|**Attribut e Type**|**Data Lengt h**|**Integration Requiremen ts**|**Defaul t Value**|**Conditio n (if needed)**|<p>**Rules (if needed**</p><p>**)**</p>|**Sampl e in Arabic**|<p>**Sampl e in Englis**</p><p>**h**</p>|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|Session Token|رمز الجلسة|Mandatory|Text|N/A|<p>Server-side session management, Client-side storage (e.g.,</p><p>cookie, local storage)</p>|N/A|N/A|<p>Unique</p><p>, time- limited.</p>|N/A|N/A|

Messages/Notifications Table

|**Message Code**|**Message (English)**|**Message (Arabic)**|<p>**Message**</p><p>**Type**</p>|<p>**Communication**</p><p>**Method**</p>|
| :- | :- | :- | :- | :- |
|MSG- LOGOUT-001|You have been logged out successfully.|.تم تسجيل خروجك بنجاح|Success Message|In-App (briefly)|
|MSG- LOGOUT-002|An error occurred during logout. Please try again.|.حدث خطأ أثناء تسجيل الخروج .يرجى المحاولة مرة أخرى|Error Message|In-App|

Screen Elements Table

|**Element ID**|**Element Type**|<p>**Element Name**</p><p>**(English)**</p>|<p>**Element Name**</p><p>**(Arabic)**</p>|**Required/Option al**|**Validatio n Rules**|**Busines s Logic**|<p>**Relate d Data**</p><p>**Entity**</p>|<p>**User Interactio**</p><p>**n**</p>|**Accessibilit y Notes**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|ELM- LOGOU T-001|Button/Lin k|Logout Button|رابط /زر تسجيل الخروج|Mandatory|N/A|<p>Initiates the</p><p>logout process.</p>|N/A|Click|<p>Clear label, typically in</p><p>header or user menu.</p>|
|ELM- LOGOU T-002|Text Label|Success/Err or Message Display|<p>عرض رسالة</p><p>الخ/النجاح طأ</p>|Conditional|N/A|<p>Display s the outcom e of the</p><p>logout attempt.</p>|N/A|View|Prominent display (briefly).|

Summary Section

This user story for "User Logout" provides a clear and concise definition for securely ending a user's session. It covers the essential steps of server-side session termination, client-side data clearance, and redirection to the login screen. This ensures a fundamental security and usability aspect of the application is well-documented.

\*\*Important Note:\*\* I am still awaiting your clarification on the "Action Not Applicable (Role/Assignment)" logic for the "Resend Account Registration Message" user story from our previous interaction. The current logic in that story (and reflected in its Acceptance Criteria) is: "Action is NOT applicable if the user has ONLY the 'Board Member' role AND is NOT assigned to any fund board." However, with `IsAssignedToFundBoard` removed from the User entity, this specific condition cannot be implemented as written. \*\*Please confirm how the "Resend Message" eligibility for Board Members should be determined without

`IsAssignedToFundBoard`.\*\*

\*\*Key Implementation Considerations:\*\*

* Ensure robust session invalidation on the server-side to prevent session hijacking.
* Thoroughly clear all client-side session data (cookies, local storage, etc.).
* The redirection to the login screen should be immediate and reliable.

\*\*Dependencies and Risks:\*\*

* \*\*Dependency:\*\* This story is dependent on the "User Login" story (as users must be logged in to log out).
* \*\*Risk:\*\* Incomplete session termination leading to security vulnerabilities. Mitigation: Rigorous testing of session management.

  ---

  I have now written the "User Logout" user story. Please provide clarification on the "Resend Account Registration Message" logic regarding Board Members when you're ready.

# User Story JDWA-1268: User Password Management (Self-Service)

Introduction Section

This user story describes the functionality for a system user to manage their own password. This includes changing their password from their personal profile when already logged in, as well as serving as the mandatory password reset screen for users upon their very first login (when `Registration Is Completed` is 0). Upon successful password reset, the system will update the user's `Registration Is Completed` flag to 1 (if it was 0) and redirect them to their dashboard (if `Registration Is Completed` was 0) or their Manage Profile screen (if

`Registration Is Completed` was 1). The Screen Elements Table provides a detailed breakdown of the UI components involved in this process.

Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|User Password Management (Self-Service)|<p>Allows users to change their</p><p>password from profile or as a mandatory first-login reset.</p>|
|\*\*User Story\*\*|<p>As a \*\*system user\*\*, I want to \*\*manage my password (change it from my profile or reset it on first login)\*\* so that I</p><p>can \*\*maintain my account security and complete my registration if pending.\*\*</p>||
|\*\*Story Points\*\*|8|<p>Combines standard password change with a critical first- login mandatory reset,</p><p>including conditional flag update and redirection.</p>|
|\*\*User Roles\*\*|All System Users|<p>Applicable to all logged-in users (for profile change)</p><p>and users redirected from login (for mandatory reset).</p>|
|\*\*Access Requirements\*\*|User must be logged in (for profile change) OR successfully authenticated and redirected from login (for mandatory reset).|User must be authenticated.|
|\*\*Trigger\*\*|User clicks "Change Password" on profile OR System redirects user from Login (if `Registration Is Completed` = 0).|User initiates or is forced into the action.|
|\*\*Frequency of Use\*\*|Low|<p>Used periodically for security updates or as a one-</p><p>time first-login requirement.</p>|
|\*\*Pre-condition\*\*|User is logged in (for profile change) OR successfully authenticated from login with `Registration Is Completed` = 0.|<p>The system is operational</p><p>and the user's session is active.</p>|
|\*\*Business Rules\*\*|<p>1\. User must provide their current password for verification (if initiated from profile).<br>2. The current password must match the stored password for the logged-in user.<br>3. User must enter a new password that meets predefined complexity requirements.<br>4. The new password must be confirmed by re-entry.<br>5. The new password cannot be the same as the current password.<br>6. The 'Last Update Date' must be automatically updated upon successful password change.<br>7. After any successful password reset from this screen:<br> a. If the user's 'Registration Is Completed' flag was 0 (meaning it was a first-time reset), the system must set</p><p>`Registration Is Completed` to 1, and then redirect the user to their default dashboard/landing page.<br> b. If the user's 'Registration Is Completed' flag was 1 (meaning it was a standard profile change), the user is redirected to their Manage Profile screen.<br>8. If initiated from first-login redirection,</p><p>the user cannot navigate away from this screen until password is changed.</p>|Ensures secure password changes, completes registration for new users, and provides appropriate redirection.|

|\*\*Post- condition\*\*|<p>User's password is changed to the new password, the 'Last Update Date' is refreshed, the 'Registration Is Completed' flag is updated (if applicable), and the user is redirected to their</p><p>\*\*dashboard (if first-time reset) or Manage Profile screen (if standard change).\*\*</p>|The user's account is updated with the new password, and their registration status is finalized.|
| :- | :- | :- |
|\*\*Risk\*\*|1\. Unauthorized password change if current password verification is weak.<br>2. User unable to change password due to complexity requirements or errors.<br>3. User confusion if forced into password change.|<p>Mitigation: Strong password complexity rules, robust current password verification, clear error</p><p>messages, clear messaging on mandatory reset screen.</p>|
|\*\*Assumptions\*\*|<p>1\. Password hashing and storage are handled securely by the</p><p>backend.<br>2. Password complexity rules are defined.<br>3. A "Manage Profile" screen exists as a destination.</p>|<p>These assumptions are</p><p>crucial for implementing the functionality.</p>|
|\*\*UX/UI Design Link\*\*|N/A (Multi-purpose password change form)|<p>No specific mockups provided in the BRD for this</p><p>section.</p>|

Process Flow Table

|**Step**|**Action Description**|**Actor**|<p>**Related Message**</p><p>**Codes**</p>|**Notes**|
| :- | :- | :- | :- | :- |
|1|<p>User initiates password change: either by clicking "Change Password" on profile, OR by being redirected from Login (if</p><p>`Registration Is Completed` = 0).</p>|User / System|||
|2|System displays the password change form, prompting for Current Password (if from profile), New Password, and Confirm New Password.|System||<p>If redirected from Login (`Registration Is Completed` = 0), the "Current Password" field may be hidden/disabled/pre-filled with temporary password, and the user</p><p>cannot navigate away.</p>|
|3|User enters their current password (if applicable), new password, and confirms new password.|User|<p>MSG-PROFILE- PW-001, MSG- PROFILE-PW-002, MSG-PROFILE-</p><p>PW-003, MSG- PROFILE-PW-004</p>||
|4|User clicks "Save Changes" or "Reset Password" button.|User|||
|5|System validates current password against stored credentials (if applicable).|System|MSG-PROFILE- PW-001|If mismatch, display error.|
|6|System validates new password (complexity, match with confirmation, not same as current).|System|<p>MSG-PROFILE- PW-002, MSG- PROFILE-PW-003,</p><p>MSG-PROFILE- PW-004</p>|If invalid, display error.|
|7|If all validations are successful: System updates the user's password in the database with the new password.|System|<p>The 'Last Update Date' is</p><p>automatically refreshed.</p>||
|8|\*\*System checks the user's 'Registration Is Completed' flag (its value \*before\* any update in this process).\*\*|System|<p>This check determines the original state for conditional update</p><p>and redirection.</p>||
|9|<p>\*\*If 'Registration Is Completed' was 0 (based on check in Step 8): System sets</p><p>the user's 'Registration Is Completed' flag to 1.\*\*</p>|System|This updates the flag to reflect completion.||

|10|System displays a success message.|System|MSG-PROFILE- PW-005||
| :- | :- | :-: | :- | :- |
|11|<p>\*\*System redirects the user based on the original 'Registration Is Completed' flag value (from Step 8):\*\*<br> \*\*- If original 'Registration Is Completed' was 0: Redirect to their default dashboard/landing page.\*\*<br> \*\*- If original 'Registration</p><p>Is Completed' was 1: Redirect to their Manage Profile screen.\*\*</p>|System||This step handles the redirection based on the flag's state before the update.|

Alternative Flow Table

|<p>**Alternative**</p><p>**Scenario**</p>|**Condition**|**Action**|<p>**Related Message**</p><p>**Codes**</p>|**Resolution**|
| :- | :- | :- | :- | :- |
|<p>\*\*Incorrect</p><p>Current Password (from Profile)\*\*</p>|<p>User enters an incorrect current</p><p>password (when initiated from profile).</p>|System displays an error message.|MSG-PROFILE- PW-001|<p>User must re-enter</p><p>correct current password.</p>|
|\*\*New Password Complexity Violation\*\*|User enters a new password that does not meet complexity rules.|System displays an error message.|MSG-PROFILE- PW-002|<p>User must enter a new password</p><p>meeting requirements.</p>|
|\*\*New Password Mismatch\*\*|New password and confirm password do not match.|System displays an error message.|MSG-PROFILE- PW-003|User must ensure passwords match.|
|\*\*New Password Same as Current\*\*|<p>User attempts to set the new</p><p>password identical to the current password.</p>|System displays an error message.|MSG-PROFILE- PW-004|<p>User must choose a</p><p>different new password.</p>|
|<p>\*\*System Error</p><p>during Password Change\*\*</p>|A backend error occurs during password update.|<p>System displays a</p><p>generic error message.</p>|MSG-PROFILE- PW-006|User can retry or contact support.|
|\*\*Cancel Password Change (from Profile)\*\*|User decides not to save changes (when initiated from profile).|User clicks "Cancel" button.|<p>System discards changes and</p><p>returns to profile screen.</p>||
|\*\*Mandatory Password Change Bypass Attempt\*\*|<p>User is redirected from Login (`Registration Is Completed` = 0) and attempts to navigate</p><p>away from the password change screen.</p>|<p>System prevents navigation or redirects back to the</p><p>password change screen.</p>|User must complete password change.||

Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Successful Password Change (from Profile)\*\*|<p>User is logged in and on their personal profile screen. User's</p><p>`Registration Is Completed` flag is 1.</p>|The user enters their correct current password, a new valid password (meeting complexity, different from current), confirms it, and clicks "Save Changes".|<p>The user's password is changed, the 'Last Update Date' is updated, the 'Registration Is Completed' flag remains 1, a success message "Password changed successfully." is displayed, and the user is redirected to their Manage</p><p>Profile screen.</p>|
|\*\*Successful Password Reset (Mandatory First Login)\*\*|User is logged in and redirected from Login. User's `Registration Is Completed` flag is 0.|The user enters a new valid password (meeting complexity, different from current), confirms it, and clicks "Save Changes".|<p>The user's password is changed, the 'Last Update Date' is updated, the 'Registration Is Completed' flag is set to 1, a success message "Password</p><p>changed successfully." is displayed, and the user is redirected to their dashboard.</p>|
|<p>\*\*Incorrect</p><p>Current Password (from Profile)\*\*</p>|<p>User is logged in and</p><p>on the password change form.</p>|<p>The user enters an incorrect</p><p>current password, valid new</p><p>password, and clicks "Save Changes".</p>|<p>The system displays an error message</p><p>"Incorrect current password."</p>|

|\*\*New Password Complexity Violation\*\*|User is logged in and on the password change form.|<p>The user enters their correct current password (if applicable), a new password</p><p>that is too short, and clicks "Save Changes".</p>|The system displays an error message "Password does not meet complexity requirements."|
| :- | :- | :- | - |
|\*\*New Password Same as Current\*\*|User is logged in and on the password change form.|<p>The user enters their correct current password (if applicable), and sets the new password identical to</p><p>the current one, then clicks "Save Changes".</p>|The system displays an error message "New password cannot be the same as current password."|
|<p>\*\*Mandatory Password Change</p><p>- Bypass Attempt\*\*</p>|<p>User is redirected from Login (`Registration Is Completed` = 0) to the</p><p>password change screen.</p>|The user attempts to navigate to another page (e.g., dashboard, another menu item).|The system prevents navigation and keeps the user on the password change screen, or redirects them back to it.|

Data Entities Table

Entity Name: User (for self-service password management)

|**Attribute (English)**|<p>**Attribu te (Arabic**</p><p>**)**</p>|**Mandatory/Opti onal**|**Attribu te Type**|**Data Lengt h**|**Integration Requireme nts**|**Default Value**|**Conditi on (if needed)**|**Rules (if needed)**|**Sampl e in Arabic**|<p>**Sampl e in**</p><p>**Englis h**</p>|
| :- | :- | :- | :- | :- | - | :- | :- | :- | :- | :- |
|User ID|معرف المستخدم|Mandatory|Number|N/A|<p>Database</p><p>Primary Key</p>|N/A|N/A|Unique identifier|12345|12345|
|Password Hash|تجزئة كلمة المرور|Mandatory|Text|N/A|Database Field|N/A|N/A|<p>Hashed value of</p><p>user's password.</p>|N/A|N/A|
|<p>Last</p><p>Update Date</p>|<p>تاريخ آخر</p><p>تحديث</p>|Mandatory|<p>DateTi</p><p>me</p>|N/A|<p>Database</p><p>Field</p>|<p>Current</p><p>Timesta mp</p>|N/A|<p>Automatica</p><p>lly set on</p><p>creation and update.</p>|<p>2023-</p><p>10-26</p><p>15:00:</p><p>00</p>|<p>2023-</p><p>10-26</p><p>15:00:</p><p>00</p>|
|Registrati on Is Complete d|تم اكتمال التسجيل|Mandatory|Boolean|N/A|Database Field|0 (False)|N/A|<p>Set to 0 upon user creation. Set to 1 after successful</p><p>password reset.</p>|0|False|

Messages/Notifications Table

|**Message Code**|**Message (English)**|**Message (Arabic)**|<p>**Message**</p><p>**Type**</p>|<p>**Communication**</p><p>**Method**</p>|
| :- | :- | :- | :- | :- |
|MSG-PROFILE- PW-001|Incorrect current password.|.كلمة المرور الحالية غير صحيحة|Error Message|In-App|
|MSG-PROFILE- PW-002|Password does not meet complexity requirements.|كلمة المرور ϻ تفي بمتطلبات .التعقيد|Error Message|In-App|
|MSG-PROFILE- PW-003|Passwords do not match.|.كلمات المرور غير متطابقة|Error Message|In-App|
|MSG-PROFILE- PW-004|New password cannot be the same as current password.|كلمة المرور الجديدة ϻ يمكن أن .تكون مطابقة لكلمة المرور الحالية|Error Message|In-App|
|MSG-PROFILE- PW-005|Password changed successfully.|.تم تغيير كلمة المرور بنجاح|Success Message|In-App|

|MSG-PROFILE- PW-006|An error occurred while changing password.|.حدث خطأ أثناء تغيير كلمة المرور|Error Message|In-App|
| :- | :- | -: | :- | :- |

Screen Elements Table

<table><tr><th colspan="1" valign="top"><b>Element ID</b></th><th colspan="1" valign="top"><b>Eleme nt Type</b></th><th colspan="1" valign="top"><p><b>Element Name (English</b></p><p><b>)</b></p></th><th colspan="1" valign="top"><p><b>Eleme nt Name (Arabi</b></p><p><b>c)</b></p></th><th colspan="1" valign="top"><b>Required/Optio nal</b></th><th colspan="1" valign="top"><b>Validati on Rules</b></th><th colspan="1" valign="top"><b>Business Logic</b></th><th colspan="1" valign="top"><b>Related Data Entity</b></th><th colspan="1" valign="top"><b>User Interacti on</b></th><th colspan="1" valign="top"><b>Accessibili ty Notes</b></th></tr>
<tr><td colspan="1" valign="top">ELM-</td><td colspan="1" valign="top">Page</td><td colspan="1" valign="top">Change</td><td colspan="1" rowspan="7" valign="top">تغيير كلمة /المرور إعادة تعيين كلمة المرور</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Displays the</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">View</td><td colspan="1" valign="top">H1</td></tr>
<tr><td colspan="1" valign="top">PROFIL</td><td colspan="1" valign="top">Title</td><td colspan="1" valign="top">Passwor</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">title of the</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">heading.</td></tr>
<tr><td colspan="1" valign="top">E-PW-</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">d / Reset</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">password</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top">001</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Passwor</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">management</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">d</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">form. Dynamic</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">based on</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">context.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"><p>ELM- PROFIL</p><p>E-PW-</p></td><td colspan="1" valign="top">Input Field</td><td colspan="1" valign="top"><p>Current Passwor</p><p>d Input</p></td><td colspan="1" valign="top"><p>حقل كلمة المرور</p><p>الحالية</p></td><td colspan="1" valign="top">Conditional</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top"><p>Collects the user's current</p><p>password for</p></td><td colspan="1" valign="top">User.Passw ord</td><td colspan="1" valign="top">Type</td><td colspan="1" valign="top"><p>Clear label,</p><p>password</p></td></tr>
<tr><td colspan="1" valign="top">002</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">verification.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">type input</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">**Hidden/disab</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">(masked).</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">led if redirected</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">from Login</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">(first-time</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">reset).**</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top">ELM- PROFIL E-PW-</td><td colspan="1" valign="top">Input Field</td><td colspan="1" valign="top">New Passwor d Input</td><td colspan="1" valign="top">حقل كلمة المرور الجديدة</td><td colspan="1" valign="top">Mandatory</td><td colspan="1" valign="top">Meets complexi ty rules</td><td colspan="1" valign="top">Collects the user's new password.</td><td colspan="1" valign="top">User.Passw ord</td><td colspan="1" valign="top">Type</td><td colspan="1" valign="top">Clear label, password</td></tr>
<tr><td colspan="1" valign="top">003</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"><p>type input</p><p>(masked).</p></td></tr>
<tr><td colspan="1" valign="top">ELM- PROFIL E-PW- 004</td><td colspan="1" valign="top">Input Field</td><td colspan="1" valign="top">Confirm New Passwor d Input</td><td colspan="1" valign="top">حقل تأكيد كلمة المرور الجديدة</td><td colspan="1" valign="top">Mandatory</td><td colspan="1" valign="top"><p>Must match New</p><p>Passwor d</p></td><td colspan="1" valign="top">Confirms the new password.</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Type</td><td colspan="1" valign="top"><p>Clear label, password</p><p>type input (masked).</p></td></tr>
<tr><td colspan="1" valign="top">ELM-</td><td colspan="1" valign="top">Button</td><td colspan="1" valign="top">Save</td><td colspan="1" rowspan="5" valign="top"><p>زر حفظ التغييرات</p><p>إعادة / تعيين كلمة المرور</p></td><td colspan="1" valign="top">Mandatory</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Submits the</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Click</td><td colspan="1" valign="top">Primary</td></tr>
<tr><td colspan="1" valign="top">PROFIL</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Changes</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">password</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">action</td></tr>
<tr><td colspan="1" valign="top">E-PW-</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">/ Reset</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">change/reset</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">button.</td></tr>
<tr><td colspan="1" valign="top">005</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Passwor</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">request.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">d Button</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top">ELM-</td><td colspan="1" valign="top">Button</td><td colspan="1" valign="top">Cancel</td><td colspan="1" valign="top">زر إلغاء</td><td colspan="1" valign="top">Conditional</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Discards</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Click</td><td colspan="1" valign="top">Secondary</td></tr>
<tr><td colspan="1" valign="top">PROFIL</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Button</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">changes and</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">action</td></tr>
<tr><td colspan="1" valign="top">E-PW-</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">returns to the</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">button.</td></tr>
<tr><td colspan="1" valign="top">006</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">profile screen.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">**Hidden if</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">redirected from</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Login</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">(mandatory</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">reset).**</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"><p>ELM-</p><p>PROFIL E-PW-</p></td><td colspan="1" valign="top"><p>Text</p><p>Label</p></td><td colspan="1" valign="top"><p>Error</p><p>Message Display</p></td><td colspan="1" valign="top"><p>عرض</p><p>رسالة الخطأ</p></td><td colspan="1" valign="top">Conditional</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top"><p>Displays</p><p>validation or system error</p></td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">View</td><td colspan="1" valign="top"><p>Prominent</p><p>display.</p></td></tr>
<tr><td colspan="1" valign="top">007</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">messages.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top">ELM- PROFIL E-PW-</td><td colspan="1" valign="top">Text Label</td><td colspan="1" valign="top">Success Message Display</td><td colspan="1" valign="top">عرض رسالة النجاح</td><td colspan="1" valign="top">Conditional</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Displays confirmation of successful</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">View</td><td colspan="1" valign="top">Prominent display.</td></tr>
<tr><td colspan="1" valign="top">008</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"><p>password</p><p>change/reset.</p></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
</table>

|<p>ELM- PROFIL E-PW-</p><p>009</p>|Text Label|<p>Mandato ry Reset Instructi</p><p>on</p>|<p>تعليمات إعادة التعيين</p><p>اϹلزامية</p>|Conditional|N/A|<p>Displays instructions like "Please set</p><p>a new</p>|N/A|View|Clear and prominent.|
| :- | :- | - | - | :- | :- | :- | :- | :- | :- |
|||||||password to||||
|||||||complete your||||
|||||||registration."||||
|||||||\*\*Visible only||||
|||||||if redirected||||
|||||||from Login||||
|||||||(mandatory||||
|||||||reset).\*\*||||

Summary Section

This rewritten user story for "User Password Management (Self-Service)" now correctly sequences the steps:

\*\*the `Registration Is Completed` flag is checked (Step 9), then updated (Step 10) if necessary, followed by the display of the success message (Step 11), and finally the conditional redirection (Step 12).\*\* This ensures the user sees the success message before being redirected to the appropriate screen based on their updated registration status.

\*\*Important Note:\*\* I am still awaiting your clarification on the "Action Not Applicable (Role/Assignment)" logic for the "Resend Account Registration Message" user story from our previous interaction. The current logic in that story (and reflected in its Acceptance Criteria) is: "Action is NOT applicable if the user has ONLY the 'Board Member' role AND is NOT assigned to any fund board." However, with `IsAssignedToFundBoard` removed from the User entity, this specific condition cannot be implemented as written. \*\*Please confirm how the "Resend Message" eligibility for Board Members should be determined without

`IsAssignedToFundBoard`.\*\*

\*\*Key Implementation Considerations:\*\*

* \*\*Critical:\*\* Implement the conditional redirection logic based on the user's `Registration Is Completed` flag

\*before\* it is updated in this process.

* Ensure the `Registration Is Completed` flag is correctly updated to 1 upon successful password reset when it was originally 0.
* The redirection to the appropriate screen must be robust and cannot be bypassed.

\*\*Dependencies and Risks:\*\*

* \*\*Dependency:\*\* This story is a critical dependency for the "User Login" story (for the first-time login redirection). It also depends on the "User Profile" screen for standard access.
* \*\*Risk:\*\* User confusion if the redirection logic is not clear or if they expect a different landing page. Mitigation: Clear messaging and thorough testing.

  ---

  I have now updated the "User Password Management (Self-Service)" user story with the corrected step order. Please provide clarification on the "Resend Account Registration Message" logic regarding Board Members when you're ready.

# User Story JDWA-1267: User Login

Introduction Section

This user story describes the process for a user to securely log in to the Fund Board Management Application. It covers authentication via username (Saudi Mobile Number) and password. Upon successful login, the system checks the `Registration Is Completed` flag: if 0, the user is redirected to a mandatory "Reset Password" screen; if 1, they are redirected to their dashboard. This ensures enhanced security for initial logins and a streamlined experience for returning users. For security, an account will be deactivated after 5 consecutive failed login attempts. The Screen Elements Table provides a detailed breakdown of the UI components on the login screen.

Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|User Login|Allows users to securely log in to the application.|
|\*\*User Story\*\*|<p>As a \*\*system user\*\*, I want to \*\*securely log in to the application and be directed appropriately based on my</p><p>registration status\*\* so that I can \*\*access my assigned functionalities and ensure my account security.\*\*</p>||
|\*\*Story Points\*\*|7|<p>Involves authentication, session management, and</p><p>conditional redirection based on registration status.</p>|
|\*\*User Roles\*\*|All System Users|<p>Applicable to all roles (System Admin, Fund</p><p>Manager, Board Member, etc.).</p>|
|\*\*Access Requirements\*\*|Valid username (Mobile No.) and password. Active user status.|User must have a valid, active account.|
|\*\*Trigger\*\*|User navigates to the application URL.|User attempts to access the system.|
|\*\*Frequency of Use\*\*|High|Users log in frequently to access the system.|
|\*\*Pre-condition\*\*|User has a registered account with a username (Mobile No.) and password. The application is running and accessible.|The system is operational and ready for login.|
|\*\*Business Rules\*\*|<p>1\. Username is the registered Saudi Mobile Number (10 digits, starting with 05).<br>2. Password must be validated against stored credentials.<br>3. Failed login attempts: After 5 consecutive failed login attempts, the user's account will be automatically deactivated.<br>4. Only 'Active' users can log in.<br>5. Integration with Active Directory (Phase Two) for authentication.<br>6. Upon successful login, the system checks the 'Registration Is Completed' flag:<br> a. If 'Registration Is Completed' = 0, the user is immediately redirected to a "Reset Password" screen.<br> b. If 'Registration Is Completed' = 1, the user is redirected to their</p><p>default dashboard/landing page.<br>7. Failed login attempts should be logged.</p>|Ensures secure and user- friendly access, with conditional redirection based on registration completion and robust account security.|
|\*\*Post- condition\*\*|<p>User is successfully authenticated, redirected to the appropriate screen (Reset Password or Dashboard), and a</p><p>secure session is established. Or, if lockout occurs, the account is deactivated.</p>|The user gains access to the system, or their account is secured.|
|\*\*Risk\*\*|1\. Unauthorized access due to weak security.<br>2. User lockout due to forgotten password or too many failed attempts.<br>3. Incorrect redirection logic.|<p>Mitigation: Strong password policies, clear password recovery/reset options,</p><p>thorough testing of redirection logic, clear</p>|

|||messaging on account deactivation.|
| :- | :- | :- |
|\*\*Assumptions\*\*|<p>1\. User accounts are managed via "Add New System User" and "Edit Existing System User" stories.<br>2. Password hashing and storage are handled securely by the backend.<br>3. A "Reset User Password" functionality exists (separate story).<br>4. Active Directory integration will be</p><p>handled in a later phase.<br>5. The system's default language is Arabic.</p>|These assumptions are crucial for implementing the functionality.|
|\*\*UX/UI Design Link\*\*|Provided login UI (Page 5 of BRD).|References the initial login screen mockup.|

Process Flow Table

|**Step**|**Action Description**|**Actor**|**Related Message Codes**|**Notes**|
| :- | :- | :- | :- | :- |
|1|User navigates to the application URL.|User|||
|2|<p>System displays the Login screen with</p><p>options for username (Mobile No.) and password.</p>|System||(References BRD Page 5 UI, simplified).|
|3|User enters their registered Mobile No. as username and password.|User|||
|4|User clicks "Login" button.|User|||
|5|<p>System validates username (Mobile</p><p>No.) and password against stored credentials.</p>|System|MSG-LOGIN-001, MSG- LOGIN-002|Checks for valid format, existence, and match.|
|6|<p>\*\*If login fails:\*\* System increments</p><p>the 'Failed Login Attempts' counter for the user.</p>|System|||
|7|<p>\*\*If 'Failed Login Attempts' reaches</p><p>5:\*\* System changes the user's 'Status' to 'Inactive'.</p>|System|MSG-LOGIN-004|This deactivates the account.|
|8|\*\*If login fails (general):\*\* System displays an error message.|System|MSG-LOGIN-001, MSG- LOGIN-003, MSG-LOGIN- 004|<p>Message varies based on specific failure (invalid credentials, inactive, or</p><p>deactivated).</p>|
|9|\*\*If login is successful:\*\* System resets 'Failed Login Attempts' to 0.|System|||
|10|System checks if the user account is 'Active'.|System|MSG-LOGIN-003|<p>If 'Inactive' (e.g., due to previous deactivation or</p><p>manual admin action), login is denied.</p>|
|11|<p>\*\*If login is successful AND user is</p><p>'Active':\*\* System establishes a secure session.</p>|System|||
|12|System checks the 'Registration Is Completed' flag for the user.|System|||
|13|<p>If 'Registration Is Completed' = 0:</p><p>System immediately redirects user to the "Reset Password" screen.</p>|System|<p>This is a mandatory</p><p>redirection for first- time/incomplete registration.</p>||
|14|<p>If 'Registration Is Completed' = 1:</p><p>System redirects user to their default dashboard/landing page.</p>|System|This is for returning users with completed registration.||
|15|System logs the login attempt (success or failure).|System|For security and auditing.||

Alternative Flow Table

|**Alternative Scenario**|**Condition**|**Action**|<p>**Related Message**</p><p>**Codes**</p>|**Resolution**|
| :- | :- | :- | :- | :- |
|\*\*Invalid Credentials\*\*|User enters incorrect Mobile No. or password.|System displays an error message.|MSG- LOGIN-001|User must re-enter correct credentials.|
|<p>\*\*Account</p><p>Deactivated (by failed attempts)\*\*</p>|<p>User attempts to log in after 5</p><p>failed attempts have deactivated their account.</p>|<p>System displays an error</p><p>message indicating the account is inactive.</p>|MSG- LOGIN-004|<p>User must contact</p><p>System Admin for activation.</p>|
|<p>\*\*Account</p><p>Deactivated (by admin)\*\*</p>|<p>User attempts to log in with</p><p>an account previously deactivated by an admin.</p>|<p>System displays an error</p><p>message indicating the account is inactive.</p>|MSG- LOGIN-003|<p>User must contact</p><p>System Admin for activation.</p>|
|\*\*System Error during Login\*\*|<p>A backend error occurs</p><p>during the authentication process.</p>|System displays a generic error message.|MSG- LOGIN-005|User can retry or contact support.|

Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|<p>\*\*Successful Login</p><p>(Registration Not Completed)\*\*</p>|A user account exists with `Registration Is Completed` = 0.|<p>The user enters their correct Mobile No. and</p><p>password, and clicks "Login".</p>|<p>The user is successfully authenticated, immediately redirected to the "Reset</p><p>Password" screen, and a secure session is established.</p>|
|<p>\*\*Successful Login</p><p>(Registration Completed)\*\*</p>|<p>An existing user account exists with</p><p>`Registration Is Completed` = 1.</p>|<p>The user enters their correct Mobile No. and</p><p>password, and clicks "Login".</p>|<p>The user is successfully authenticated, redirected to their default</p><p>dashboard/landing page, and a secure session is established.</p>|
|\*\*Invalid Credentials (Below Lockout)\*\*|An account exists.|<p>The user enters an incorrect password for a valid Mobile No. (e.g.,</p><p>3 times), and clicks "Login".</p>|The system displays an error message "Invalid username or password."|
|\*\*Account Deactivated by Failed Attempts\*\*|An account exists. 'Failed Login Attempts' is 4.|The user enters an incorrect password for a valid Mobile No. (5th consecutive attempt), and clicks "Login".|<p>The system changes the user's 'Status' to 'Inactive', displays an error message "Your account has been deactivated due to too many failed login attempts. Please contact</p><p>support.", and prevents further login attempts.</p>|
|\*\*Inactive User Login\*\*|<p>An 'Inactive' user account exists (e.g., deactivated by admin or previous failed</p><p>attempts).</p>|The user enters their Mobile No. and password, and clicks "Login".|The system displays an error message "Your account is inactive. Please contact support."|
|\*\*System Error during Login\*\*|<p>A backend error occurs</p><p>during the authentication process.</p>|The user attempts to log in.|<p>The system displays a generic error</p><p>message "An unexpected error occurred during login. Please try again."</p>|

Data Entities Table

Entity Name: User (for login)

|<p>**Attribut e (English**</p><p>**)**</p>|<p>**Attrib ute (Arabi**</p><p>**c)**</p>|**Mandatory/Op tional**|**Attribute Type**|**Data Leng th**|<p>**Integratio n Requirem**</p><p>**ents**</p>|<p>**Defa ult Valu**</p><p>**e**</p>|<p>**Condit ion (if needed**</p><p>**)**</p>|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | - | :- | :- | :- | :- | :- | :- |
|User ID|معرف المستخدم|Mandatory|Number|N/A|<p>Database Primary</p><p>Key</p>|N/A|N/A|Unique identifier|12345|12345|
|Mobile|رقم الجوال|Mandatory|Text|Max 10|Database Field|N/A|N/A|<p>Saudi mobile</p><p>format (e.g., 05XXXXXX</p>|<p>0501234</p><p>567</p>|<p>0501234</p><p>567</p>|

|||||||||<p>XX),</p><p>Numeric only, Unique (as username).</p>|||
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|Passwor d Hash|<p>تجزئة</p><p>كلمة المرور</p>|Mandatory|Text|N/A|Database Field|N/A|N/A|<p>Hashed value</p><p>of user's password.</p>|N/A|N/A|
|Status|الحالة|Mandatory|Boolean/Drop down|N/A|Database Field|Activ e|N/A|Active/Inacti ve|نشط|Active|
|Failed Login Attempt s|محاوϻت تسجيل الدخول الفاشلة|Mandatory|Number|N/A|Database Field|0|N/A|<p>Increments on failure, resets on success.</p><p>Resets to 0 on successful login.</p>|0|0|
|Registra tion Is Complet ed|تم اكتمال التسجيل|Mandatory|Boolean|N/A|Database Field|<p>0</p><p>(Fals e)</p>|N/A|<p>Set to 0 upon user creation. Set to 1 after successful</p><p>password reset.</p>|0|False|

Messages/Notifications Table

|<p>**Message**</p><p>**Code**</p>|**Message (English)**|**Message (Arabic)**|<p>**Message**</p><p>**Type**</p>|<p>**Communication**</p><p>**Method**</p>|
| :- | :- | :- | :- | :- |
|<p>MSG- LOGIN-</p><p>001</p>|Invalid username or password.|اسم المستخدم أو كلمة المرور غير .صحيحة|Error Message|In-App|
|<p>MSG-</p><p>LOGIN- 002</p>|Login successful.|.تم تسجيل الدخول بنجاح|Success Message|In-App|
|<p>MSG- LOGIN-</p><p>003</p>|Your account is inactive. Please contact support.|يرجى اϻتصال .حسابك غير نشط .بالدعم|Error Message|In-App|
|<p>MSG-</p><p>LOGIN- 004</p>|<p>Your account has been deactivated</p><p>due to too many failed login attempts. Please contact support.</p>|<p>تم إلغاء تفعيل حسابك بسبب كثرة .محاوϻت تسجيل الدخول الفاشلة</p><p>.يرجى اϻتصال بالدعم</p>|Error Message|In-App|
|<p>MSG-</p><p>LOGIN- 005</p>|An unexpected error occurred during login. Please try again.|حدث خطأ غير متوقع أثناء تسجيل .يرجى المحاولة مرة أخرى .الدخول|Error Message|In-App|

Screen Elements Table

|**Eleme nt ID**|**Eleme nt Type**|**Element Name (English)**|<p>**Eleme nt Name (Arabi**</p><p>**c)**</p>|**Required/Optio nal**|**Validatio n Rules**|**Business Logic**|**Related Data Entity**|**User Interactio n**|**Accessibili ty Notes**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|<p>ELM- LOGIN</p><p>-001</p>|Page Title|Login|تسجيل الدخول|N/A|N/A|<p>Displays the</p><p>title of the login page.</p>|N/A|View|<p>H1</p><p>heading.</p>|
|<p>ELM-</p><p>LOGIN</p><p>-002</p>|<p>Input</p><p>Field</p>|<p>Mobile</p><p>No. (Usernam e)</p>|رقم الجوال اسم ( )المستخدم|Mandatory|<p>Saudi</p><p>mobile format,</p><p>Max 10 digits</p>|<p>Collects the</p><p>user's mobile</p><p>number as username.</p>|User.Mobile|Type|<p>Clear label,</p><p>placeholder</p><p>, numeric keyboard.</p>|

|<p>ELM- LOGIN</p><p>-003</p>|Input Field|Password|كلمة المرور|Mandatory|N/A|Collects the user's password.|User.Passwo rd|Type|<p>Clear label, password</p><p>type input (masked).</p>|
| :- | :- | :- | -: | :- | :- | :- | :- | :- | :- |
|<p>ELM- LOGIN</p><p>-004</p>|Button|Login Button|<p>زر</p><p>تسجيل الدخول</p>|Mandatory|N/A|<p>Submits</p><p>login credentials.</p>|N/A|Click|<p>Primary</p><p>action button.</p>|
|<p>ELM- LOGIN</p><p>-005</p>|Link|<p>Forgot Password</p><p>?</p>|<p>هل نسيت كلمة</p><p>المرور؟</p>|Optional|N/A|<p>Navigates to password</p><p>recovery/res et process.</p>|N/A|Click|Clear link text.|
|<p>ELM- LOGIN</p><p>-006</p>|Text Label|Error Message Display|عرض رسالة الخطأ|Conditional|N/A|<p>Displays login error messages (e.g., invalid credentials, inactive account,</p><p>deactivated account).</p>|N/A|View|Prominent display, clear text.|

Summary Section

This rewritten user story for "User Login" now accurately reflects the removal of `ELM-LOGIN-007` (Welcome Message) from the Screen Elements Table. The login process remains streamlined, with conditional redirection based on the `Registration Is Completed` flag and the account deactivation policy after 5 failed attempts.

\*\*Important Note:\*\* I am still awaiting your clarification on the "Action Not Applicable (Role/Assignment)" logic for the "Resend Account Registration Message" user story from our previous interaction. The current logic in that story (and reflected in its Acceptance Criteria) is: "Action is NOT applicable if the user has ONLY the 'Board Member' role AND is NOT assigned to any fund board." However, with `IsAssignedToFundBoard` removed from the User entity, this specific condition cannot be implemented as written. \*\*Please confirm how the "Resend Message" eligibility for Board Members should be determined without

`IsAssignedToFundBoard`.\*\*

\*\*Key Implementation Considerations:\*\*

* \*\*Critical:\*\* The "Reset User Password" story must be updated to set `Registration Is Completed` to 1 upon successful password reset.
* Ensure the redirection logic is robust and cannot be bypassed.
* The system's default language will be used for the UI.

\*\*Dependencies and Risks:\*\*

* \*\*Dependency:\*\* This story depends heavily on the "Reset User Password" story for the next step in the login flow. It also depends on "Add New System User" for account creation.
* \*\*Risk:\*\* User confusion if login process is not intuitive or error messages are unclear.

\---

I have now updated the "User Login" user story with the removal of `ELM-LOGIN-007`. Please provide clarification on the "Resend Account Registration Message" logic regarding Board Members when you're ready.

# User Story JDWA-1257: Reset User Password

Introduction Section

This user story describes the functionality for System Administrators to reset the password for an existing user account within the Fund Board Management Application. This action is crucial for assisting users who have forgotten their password or for security reasons. The action is strictly conditional, available only for active users whose registration message was sent and whose overall registration is completed. Upon successful reset, the user receives their new temporary password via WhatsApp. The Screen Elements Table provides a detailed breakdown of the UI components involved in this process.

Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|Reset User Password|<p>Allows System Administrators to reset a user's password and</p><p>notify them via WhatsApp.</p>|
|\*\*User Story\*\*|<p>As a \*\*System Administrator\*\*, I want to \*\*reset a user's</p><p>password and send them the new password via WhatsApp\*\* so that I can \*\*help them regain access to their account.\*\*</p>||
|\*\*Story Points\*\*|5|<p>Involves user selection, conditional logic, password generation,</p><p>database update, and external notification.</p>|
|\*\*User Roles\*\*|System Admin|<p>Only System Administrators can</p><p>perform this action.</p>|
|\*\*Access Requirements\*\*|Authenticated and authorized System Admin access. User must be selected from the user list or details page.|User must be logged in as a System Admin.|
|\*\*Trigger\*\*|System Admin clicks "Reset Password" button/link for a specific user from the user list or user details page.|User initiates the action.|
|\*\*Frequency of Use\*\*|Low to Medium|<p>Used when users forget</p><p>passwords or for security reasons.</p>|
|\*\*Pre-condition\*\*|<p>System Admin is logged in. The target user account exists and has a valid Saudi mobile number. \*\*The user must be 'Active', AND</p><p>their 'Registration Is Completed' flag must be 1, AND their 'Registration Message Is Sent' flag must be 1.\*\*</p>|<p>The system is operational, and the user</p><p>meets all eligibility criteria.</p>|
|\*\*Business Rules\*\*|1\. The "Reset Password" action is \*\*ONLY applicable\*\* if the user's Status is 'Active' AND their 'Registration Is Completed' flag is 1 AND their 'Registration Message Is Sent' flag is 1.<br>2. The "Reset Password" button/link must be hidden if the user does not meet these criteria.<br>3. The user must have a valid Saudi mobile number registered.<br>4. The system will automatically generate a new, strong temporary password.<br>5. A confirmation dialog should be presented before resetting the password.<br>6. The new temporary password will be sent to the user via WhatsApp.<br>7. The 'Last Update Date' must be automatically updated upon successful password reset.|Ensures strict conditional availability, secure password generation, and user notification.|
|\*\*Post- condition\*\*|<p>The user's password is reset to a new temporary password, the 'Last Update Date' is refreshed, and the new password is sent to</p><p>the user via WhatsApp.</p>|<p>The system state reflects the updated password,</p><p>and the user is informed.</p>|
|\*\*Risk\*\*|<p>1\. WhatsApp message delivery failure.<br>2. New password</p><p>security concerns (e.g., weak generation, insecure transmission).<br>3. Resetting password for an incorrect user.</p>|<p>Mitigation: Confirmation</p><p>dialog, strong password generation, secure</p>|

|||WhatsApp API, audit trails.|
| :- | :- | :- |
|\*\*Assumptions\*\*|<p>1\. WhatsApp Business API integration is available and configured.<br>2. Default password policy for generated</p><p>passwords is defined (e.g., length, complexity).<br>3. The system handles password hashing internally.</p>|These assumptions are crucial for implementing the functionality.|
|\*\*UX/UI Design Link\*\*|N/A (Refer to general action button/confirmation dialog patterns)|<p>No specific mockups</p><p>provided in the BRD for this section.</p>|

Process Flow Table

|**Step**|**Action Description**|**Actor**|<p>**Related**</p><p>**Message Codes**</p>|**Notes**|
| :- | :- | :- | :- | :- |
|1|System Admin is viewing the "View System Users List" or "View User Details" page.|System Admin|||
|2|<p>System Admin identifies an eligible user and</p><p>clicks the "Reset Password" button/link for that user.</p>|System Admin||<p>This button/link is only</p><p>visible/enabled if the user meets all eligibility criteria.</p>|
|3|<p>System displays a confirmation dialog asking</p><p>the System Admin to confirm the password reset.</p>|System|MSG-RESET- 001|Includes the user's name in the message.|
|4|System Admin reviews the confirmation message and clicks "Confirm" or "Cancel".|System Admin||If "Cancel" is clicked, the process stops.|
|5|System generates a new, strong temporary password for the user.|System|||
|6|<p>System updates the user's password in the</p><p>database with the new generated password (hashing handled internally).</p>|System||The 'Last Update Date' is automatically refreshed.|
|7|<p>System attempts to send a WhatsApp message</p><p>to the user containing their new temporary password and login instructions.</p>|System|<p>MSG-RESET-</p><p>002, MSG- RESET-003</p>|Requires WhatsApp Business API call.|
|8|<p>System displays a success or failure message</p><p>to the System Admin based on the password reset and WhatsApp message sending attempt.</p>|System|<p>MSG-RESET-</p><p>002, MSG- RESET-003</p>|<p>Note: Using MSG-RESET-002</p><p>for success, MSG-RESET-003 for failure.</p>|
|9|System Admin can continue managing users.|System Admin|||

Alternative Flow Table

<table><tr><th colspan="1" valign="top"><b>Alternative Scenario</b></th><th colspan="1" valign="top"><b>Condition</b></th><th colspan="1" valign="top"><b>Action</b></th><th colspan="1" valign="top"><p><b>Related</b></p><p><b>Message Codes</b></p></th><th colspan="1" valign="top"><b>Resolution</b></th></tr>
<tr><td colspan="1" valign="top">**Action Not</td><td colspan="1" valign="top">System Admin attempts to click</td><td colspan="1" valign="top">The "Reset</td><td colspan="1" rowspan="6" valign="top"></td><td colspan="1" valign="top">System Admin</td></tr>
<tr><td colspan="1" valign="top">Applicable</td><td colspan="1" valign="top">"Reset Password" for a user who is</td><td colspan="1" valign="top">Password" button/link</td><td colspan="1" valign="top">cannot perform the</td></tr>
<tr><td colspan="1" valign="top">(Eligibility Not</td><td colspan="1" valign="top">NOT 'Active', OR whose</td><td colspan="1" valign="top">is **hidden**.</td><td colspan="1" valign="top">action.</td></tr>
<tr><td colspan="1" valign="top">Met)**</td><td colspan="1" valign="top">'Registration Is Completed' flag is 0,</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top">OR whose 'Registration Message Is</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top">Sent' flag is 0.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top">**Confirmation</td><td colspan="1" valign="top">System Admin clicks "Reset</td><td colspan="1" valign="top">System closes the</td><td colspan="1" rowspan="2" valign="top"></td><td colspan="1" valign="top">No action</td></tr>
<tr><td colspan="1" valign="top">Canceled**</td><td colspan="1" valign="top"><p>Password" but then clicks "Cancel"</p><p>in the confirmation dialog.</p></td><td colspan="1" valign="top"><p>confirmation dialog</p><p>and no password reset occurs.</p></td><td colspan="1" valign="top">performed.</td></tr>
<tr><td colspan="1" valign="top">**WhatsApp</td><td colspan="1" valign="top">The system successfully resets the</td><td colspan="1" valign="top">System displays a</td><td colspan="1" valign="top">MSG-</td><td colspan="1" valign="top">System Admin may</td></tr>
<tr><td colspan="1" valign="top">Message Failure**</td><td colspan="1" valign="top">password but fails to send the</td><td colspan="1" valign="top">warning message to</td><td colspan="1" valign="top">RESET-</td><td colspan="1" valign="top">need to manually</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top">WhatsApp message.</td><td colspan="1" valign="top">the System Admin</td><td colspan="1" valign="top">003</td><td colspan="1" valign="top">provide the</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">and logs the failure.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"><p>password to the</p><p>user.</p></td></tr>
</table>

|<p>\*\*System Error</p><p>during Password Reset\*\*</p>|A backend error occurs during the password reset operation.|<p>System displays a</p><p>generic error message.</p>|<p>MSG-</p><p>RESET- 004</p>|<p>System Admin can</p><p>retry later or contact support.</p>|
| :- | :- | :- | :- | :- |
|\*\*User Not Found\*\*|The selected user's ID is invalid or the user has been deleted.|<p>System displays an</p><p>error message (e.g., "User not found").</p>|<p>MSG-</p><p>RESET- 005</p>|<p>System Admin can</p><p>return to the user list.</p>|

Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Reset Password - Success (Eligible User)\*\*|A System Admin is logged in. A user is 'Active', their 'Registration Is Completed' flag is 1, and their 'Registration Message Is Sent' flag is 1.|The System Admin clicks "Reset Password" for this user and clicks "Confirm" in the dialog.|<p>The user's password is reset to a new temporary password, the 'Last Update Date' is updated, a WhatsApp message with the new password is sent to the user, and a success message "Password</p><p>reset successfully." is displayed to the System Admin.</p>|
|\*\*Reset Password - Not Applicable (Inactive User)\*\*|A System Admin is logged in. A user is 'Inactive'.|<p>The System Admin attempts to click</p><p>"Reset Password" for this user.</p>|<p>The "Reset Password" button/link is</p><p>\*\*hidden\*\*, and the action cannot be performed.</p>|
|<p>\*\*Reset Password - Not Applicable</p><p>(Registration Not Complete)\*\*</p>|<p>A System Admin is logged in. A user is 'Active', but</p><p>their 'Registration Is Completed' flag is 0.</p>|<p>The System Admin attempts to click</p><p>"Reset Password" for this user.</p>|<p>The "Reset Password" button/link is</p><p>\*\*hidden\*\*, and the action cannot be performed.</p>|
|\*\*Reset Password - Not Applicable (Message Not Sent)\*\*|<p>A System Admin is logged in. A user is 'Active', their 'Registration Is Completed' flag is 1, but their</p><p>'Registration Message Is Sent' flag is 0.</p>|The System Admin attempts to click "Reset Password" for this user.|<p>The "Reset Password" button/link is</p><p>\*\*hidden\*\*, and the action cannot be performed.</p>|
|\*\*Reset Password - Failure (WhatsApp Message Failure)\*\*|A System Admin is logged in. A user is eligible, the password is reset, but the WhatsApp message fails to send.|<p>The System Admin clicks "Reset Password" for this user and clicks</p><p>"Confirm" in the dialog.</p>|The user's password is reset, but a warning message "Failed to send new password via WhatsApp. Please notify the user manually." is displayed to the System Admin.|
|<p>\*\*Reset Password - Confirmation</p><p>Canceled\*\*</p>|<p>A System Admin clicks "Reset Password" for a</p><p>user.</p>|<p>The System Admin clicks "Cancel" in the</p><p>confirmation dialog.</p>|The confirmation dialog closes, and the password is not reset.|
|\*\*System Error during Reset\*\*|A System Admin attempts to reset a password.|<p>A backend error occurs during the</p><p>password reset operation.</p>|<p>The system displays a generic error message "An error is occurred while</p><p>resetting password." and the password remains unchanged.</p>|

Data Entities Table

Entity Name: User (for resetting password)

|<p>**Attribu te (Englis**</p><p>**h)**</p>|<p>**Attrib ute (Arabi**</p><p>**c)**</p>|**Mandatory/O ptional**|**Attribute Type**|**Data Leng th**|<p>**Integratio n Requirem**</p><p>**ents**</p>|**Default Value**|<p>**Condit ion (if needed**</p><p>**)**</p>|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | - | :- | :- | :- | :- | :- | :- |
|User ID|معرف المستخدم|Mandatory|Number|N/A|<p>Database</p><p>Primary Key</p>|N/A|N/A|Unique identifier|12345|12345|
|Mobile|رقم الجوال|Mandatory|Text|Max 10|Database Field|N/A|N/A|<p>Saudi mobile format (e.g.,</p><p>05XXXXX XXX),</p>|<p>0501234</p><p>567</p>|<p>0501234</p><p>567</p>|

|||||||||<p>Numeric only, Unique</p><p>(as username).</p>|||
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|Passwor d|كلمة المرور|Mandatory|Text|N/A|Database Field|N/A|N/A|<p>System- generated, stored securely (hashing</p><p>handled internally).</p>|N/A|N/A|
|Status|الحالة|Mandatory|Boolean/Drop down|N/A|Database Field|Active|N/A|Active/Inacti ve|نشط|Active|
|Registra tion Message Is Sent|تم إرسال رسالة التسجيل|Mandatory|Boolean|N/A|Database Field|<p>0</p><p>(False)</p>|N/A|<p>Set to 1 if user's role(s) make them eligible for WhatsApp message (NOT only 'Board Member'); 0 if only</p><p>'Board Member'.</p>|1|True|
|Registra tion Is Complet ed|تم اكتمال التسجيل|Mandatory|Boolean|N/A|Database Field|<p>0</p><p>(False)</p>|N/A|<p>Set to 0 upon user creation.</p><p>Updated by a separate process (e.g.,</p><p>user's first login).</p>|0|False|
|Last Update Date|تاريخ آخر تحديث|Mandatory|DateTime|N/A|Database Field|Current Timesta mp|N/A|<p>Automaticall y set on</p><p>creation and update.</p>|<p>2023-</p><p>10-26</p><p>15:00:00</p>|<p>2023-</p><p>10-26</p><p>15:00:00</p>|

Messages/Notifications Table

|<p>**Message**</p><p>**Code**</p>|**Message (English)**|**Message (Arabic)**|<p>**Message**</p><p>**Type**</p>|<p>**Communication**</p><p>**Method**</p>|
| :- | :- | :- | :- | :- |
|MSG- RESET- 001|<p>Are you sure you want to reset the password for [User Name]? A new</p><p>temporary password will be sent via WhatsApp.</p>|<p>هل أنت متأكد من إعادة تعيين كلمة ؟ سيتم ]اسم المستخدم[المرور للمستخدم إرسال كلمة مرور مؤقتة جديدة عبر</p><p>.الواتساب</p>|Confirmation Dialog|In-App|
|<p>MSG-</p><p>RESET- 002</p>|<p>Password reset successfully. A new</p><p>temporary password has been sent to the user via WhatsApp.</p>|<p>تم .تم إعادة تعيين كلمة المرور بنجاح إرسال كلمة مرور مؤقتة جديدة للمستخدم</p><p>.عبر الواتساب</p>|Success Message|In-App|
|<p>MSG-</p><p>RESET- 003</p>|<p>Failed to send new password via</p><p>WhatsApp. Please notify the user manually.</p>|فشل إرسال كلمة المرور الجديدة عبر .يرجى إخطار المستخدم يدويا˝ .الواتساب|Warning Message|In-App / System Log|
|<p>MSG-</p><p>RESET- 004</p>|An error is occurred while resetting password.|لم يتم إعادة تعيين ,حدث خطأ بالنظام .كلمة المرور|Error Message|In-App|
|<p>MSG-</p><p>RESET- 005</p>|User not found.|.لم يتم العثور على المستخدم|Error Message|In-App|

|<p>\*\*MSG- RESET-</p><p>006\*\*</p>|\*\*Your password for Jadwa Fund Board Management has been reset. Your new temporary password is: [New Password]. Please log in through this link [login URL] and change your password.\*\*|تمت إعادة تعيين كلمة المرور \*\* الخاصة بك لتطبيق إدارة مجالس كلمة المرور المؤقتة .صناديق جدوى ].كلمة المرور الجديدة[ :الجديدة هي يرجى تسجيل الدخول من خϼل هذا وتغيير ]رابط تسجيل الدخول[الرابط .\*\*كلمة المرور الخاصة بك|\*\*Success Message\*\*|\*\*WhatsApp\*\*|
| :- | :- | -: | :- | :- |

Screen Elements Table

|**Eleme nt ID**|**Element Type**|**Element Name (English)**|<p>**Elemen t Name (Arabic**</p><p>**)**</p>|**Required/Optio nal**|**Validati on Rules**|**Busines s Logic**|<p>**Relate d Data**</p><p>**Entity**</p>|**User Interaction**|**Accessibilit y Notes**|
| :- | :- | :- | - | :- | :- | :- | :- | :- | :- |
|ELM- RESE T-001|Button/Lin k|Reset Password Button|رابط /زر إعادة تعيين كلمة المرور|Conditional|N/A|<p>Initiates the passwor d reset process.</p><p>\*\*Hidde n if user is not eligible.</p><p>\*\*</p>|N/A|Click|Clear label, visible/enabl ed based on conditions.|
|ELM- RESE T-002|Confirmati on Dialog|Reset Password Confirmati on Dialog|نافذة تأكيد إعادة تعيين كلمة المرور|Mandatory|N/A|<p>Prompts System Admin to confirm</p><p>passwor d reset.</p>|N/A|View, Click (Confirm/Canc el)|Clear message, accessible buttons.|
|ELM- RESE T-003|Text Label|Confirmati on Message Text|نص رسالة التأكيد|Mandatory|N/A|<p>Displays "Are you sure you want to reset the passwor d for [User</p><p>Name]?"</p>|N/A|View|Dynamic content for user name.|
|ELM- RESE T-004|Button|Confirm Reset Button|زر تأكيد إعادة التعيين|Mandatory|N/A|<p>Confirm s the passwor</p><p>d reset action.</p>|N/A|Click|Clearly labeled.|
|ELM- RESE T-005|Button|Cancel Reset Button|زر إلغاء إعادة التعيين|Mandatory|N/A|<p>Cancels the passwor</p><p>d reset action.</p>|N/A|Click|Clearly labeled.|
|ELM- RESE T-006|Text Label|Success/Err or Message Display|<p>عرض رسالة</p><p>الخ/النجاح طأ</p>|Conditional|N/A|<p>Displays the outcome of the passwor</p><p>d reset attempt.</p>|N/A|View|Prominent display, clear text.|

Summary Section

This user story for "Reset User Password" provides a comprehensive definition for resetting a user's password. It clearly outlines the strict eligibility criteria (active, registration completed, message sent), the automatic generation of a new password, and the critical step of sending this new password via WhatsApp. The Process Flow, Alternative Flow, Acceptance Criteria, Messages, and Screen Elements have been meticulously detailed to ensure a secure and user-friendly administrative action.

\*\*Important Note:\*\* I am still awaiting your clarification on the "Action Not Applicable (Role/Assignment)" logic for the "Resend Account Registration Message" user story from our previous interaction. The current logic in that story (and reflected in its Acceptance Criteria) is: "Action is NOT applicable if the user has ONLY the 'Board Member' role AND is NOT assigned to any fund board." However, with `IsAssignedToFundBoard` removed from the User entity, this specific condition cannot be implemented as written. \*\*Please confirm how the "Resend Message" eligibility for Board Members should be determined without

`IsAssignedToFundBoard`.\*\*

\*\*Key Implementation Considerations:\*\*

* \*\*Critical:\*\* Implement the precise logic for hiding/showing the "Reset Password" button based on the combined eligibility criteria.
* Ensure the password generation mechanism adheres to strong security standards.
* The WhatsApp message containing the new password must be handled with utmost security and reliability.
* Consider forcing a password change on the user's first login after a reset.

\*\*Dependencies and Risks:\*\*

* \*\*Dependency:\*\* This story depends on the "View System Users List" or "View System User Details" stories for initiating the action. It also depends on the "Add New System User" story for initial user creation and password generation.
* \*\*Risk:\*\* Security breach if the new password is compromised during generation, storage, or transmission. Mitigation: Strong encryption, secure API, and audit trails.
* \*\*Risk:\*\* User lockout if the new password is not received or is incorrect. Mitigation: Clear instructions, manual fallback.

  ---

  I have now generated the user story for "Reset User Password" with all the requested details. Please provide clarification on the "Resend Account Registration Message" logic regarding Board Members when you're ready.

# User Story JDWA-1253: Activate/Deactivate System User

Introduction Section

This user story describes the functionality for System Administrators to change the status of an existing user account between 'Active' and 'Inactive' within the Fund Board Management Application. This action is crucial for controlling user access. For specific "single-holder" roles (Legal Counsel, Finance Controller, Compliance and Legal Managing Director, Head of Real Estate), activating a user may trigger a replacement workflow if another active user holds that role, while deactivating such a user is restricted if no replacement is in place. The user will receive a WhatsApp message notifying them of their new status and appropriate action. The visibility of the 'Activate' or 'Deactivate' button is strictly controlled, appearing only when the user is eligible for that specific action. The Screen Elements Table provides a detailed breakdown of the UI components involved in this status change.

Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|Activate/Deactivate System User|<p>Allows System Administrators to change a user's status between</p><p>active and inactive, with special handling for single-holder roles.</p>|
|\*\*User Story\*\*|<p>As a \*\*System Administrator\*\*, I want to \*\*activate or deactivate a system user and notify them of the status change, managing single-holder role conflicts,\*\* so that I</p><p>can \*\*control their access to the application and ensure critical roles are always filled.\*\*</p>||
|\*\*Story Points\*\*|9|<p>Involves user selection, complex conditional logic, replacement/prevention workflow,</p><p>status update, session termination, and external notification.</p>|
|\*\*User Roles\*\*|System Admin|Only System Administrators can perform this action.|
|\*\*Access Requirements\*\*|Authenticated and authorized System Admin access. User must be selected from the user list or details page.|User must be logged in as a System Admin.|
|\*\*Trigger\*\*|<p>System Admin clicks "Activate" or "Deactivate"</p><p>button/link for a specific user from the user list or user details page.</p>|User initiates the action.|
|\*\*Frequency of Use\*\*|Low to Medium|<p>Used when personnel status</p><p>changes (e.g., leave of absence, termination, return).</p>|
|\*\*Pre-condition\*\*|<p>System Admin is logged in. The target user account</p><p>exists. The user's status must be either 'Active' (for deactivation) or 'Inactive' (for activation).</p>|The system is operational, and the user's current status is known.|
|\*\*Business Rules\*\*|<p>1\. The "Deactivate" action is \*\*ONLY applicable\*\* if the user's current status is 'Active'.<br>2. The "Activate" action is \*\*ONLY applicable\*\* if the user's current status is 'Inactive'.<br>3. The "Deactivate" button/link must be hidden if the user is 'Inactive'.<br>4. The "Activate" button/link must be hidden if the user is 'Active'.<br>5. A confirmation dialog must be presented before changing the user's status.<br>6. Upon deactivation, any active sessions for that user should be terminated.<br>7. The 'Last Update Date' must be automatically updated upon successful status change.<br>\*\*8. User receives a WhatsApp message notification of their new status (Activated/Deactivated) and appropriate action (e.g., "you</p><p>can now log in" or "your account is inactive").\*\*<br>\*\*9. Special Handling for Single-Holder Roles (Legal</p>|Ensures proper access control, user confirmation, and accurate record keeping, with strict button visibility, complex role-based status change logic, and user notification.|

||<p>Counsel, Finance Controller, Compliance and Legal Managing Director, Head of Real Estate):\*\*<br> a.</p><p>\*\*On Activate:\*\* If the user being activated holds \*only\* one of these single-holder roles, the system must check for another \*active\* user currently holding \*only\* that specific role. If found, a confirmation dialog is displayed to replace the existing user. If 'Replace' is chosen, the existing user is deactivated before the current user is activated. If 'Cancel' is chosen, the activation is aborted.<br> b. \*\*On Deactivate:\*\* If the user being deactivated holds \*only\* one of these single-holder roles, the system must check if there is \*another active user\* holding \*only\* that same role. If no other active user holds that role, the deactivation is prevented, and an error</p><p>message is displayed, requiring the admin to add another user first.</p>||
| :- | :- | :- |
|\*\*Post- condition\*\*|<p>The user's status is changed to the new state ('Active' or 'Inactive'), the 'Last Update Date' is refreshed, and a WhatsApp notification is sent to the user. If deactivated, any active sessions are terminated. If a replacement</p><p>occurred during activation, the previous user with that specific role is deactivated.</p>|The system state reflects the updated user status and potentially a deactivated previous role holder, and the user is informed.|
|\*\*Risk\*\*|<p>1\. Accidental deactivation/activation of a critical user.<br>2. User unable to log in due to incorrect activation/deactivation.<br>3. Accidental deactivation of a user during replacement.<br>4. Failure to ensure critical</p><p>roles are always filled.<br>5. WhatsApp message delivery failure.</p>|Mitigation: Clear confirmation dialogs, thorough testing of complex logic, audit trails, robust error messages, WhatsApp API monitoring.|
|\*\*Assumptions\*\*|<p>1\. The system has a mechanism to terminate active user sessions.<br>2. The user's status is a single boolean or dropdown field in the user entity.<br>3. The roles 'Legal Counsel', 'Finance Controller', 'Compliance and Legal Managing Director', 'Head of Real Estate' are considered</p><p>"single-holder" roles for active users.<br>4. WhatsApp Business API integration is available and configured.</p>|These assumptions are crucial for implementing the functionality.|
|\*\*UX/UI Design Link\*\*|N/A (Refer to general action button/confirmation dialog patterns, new role replacement dialog)|No specific mockups provided in the BRD for this section.|

Process Flow Table

|**Step**|**Action Description**|**Actor**|<p>**Related Message**</p><p>**Codes**</p>|**Notes**|
| :- | :- | :- | :- | :- |
|1|<p>System Admin is viewing the "View</p><p>System Users List" or "View User Details" page.</p>|System Admin|||
|2|System Admin identifies a user and clicks the "Deactivate" or "Activate" button/link for that user.|System Admin|<p>The button/link is only visible/enabled if</p><p>the user is eligible for that specific action.</p>||
|3|<p>System displays a confirmation</p><p>dialog asking the System Admin to confirm the status change.</p>|System|<p>MSG-ACTDEACT-</p><p>001, MSG- ACTDEACT-002</p>|Dialog message varies based on action (Activate/Deactivate).|
|4|<p>System Admin reviews the</p><p>confirmation message and clicks "Confirm" or "Cancel".</p>|System Admin|If "Cancel" is clicked, the process stops.||
|5|\*\*If action is "Deactivate":\*\*|System|||
|6|<p>\*\*System checks if the user being deactivated holds ONLY a single-</p><p>holder role (Legal Counsel, Finance Controller, etc.).\*\*</p>|System|||

|7|\*\*If yes (single-holder role) AND no other active user holds that same role:\*\* System displays an error message preventing deactivation.|System|MSG-ACTDEACT- 007|<p>"You can't deactivate this user [username] with role [role], you should add another user with the</p><p>same role first." Deactivation is aborted.</p>|
| :- | :- | :- | :- | :- |
|8|<p>\*\*If no (not single-holder role, or another active user exists):\*\* System</p><p>changes the user's status to 'Inactive' in the database.</p>|System|The 'Last Update Date' is automatically refreshed.||
|9|System terminates any active sessions for the user.|System|||
|10|\*\*If action is "Activate":\*\*|System|||
|11|<p>\*\*System checks if the user being</p><p>activated holds ONLY a single- holder role.\*\*</p>|System|||
|12|<p>\*\*If yes (single-holder role) AND an active user already holds ONLY that specific role (who is NOT the user being activated):\*\* System displays</p><p>a confirmation dialog asking to replace the existing user.</p>|System|MSG-ACTDEACT- 008|Dialog includes existing user's name and role. Buttons: "Replace", "Cancel".|
|13|<p>\*\*If System Admin clicks "Replace"</p><p>in the dialog:\*\* System deactivates the existing user with that role.</p>|<p>System</p><p>Admin / System</p>|<p>The existing user's</p><p>status is changed to 'Inactive'.</p>||
|14|<p>\*\*If System Admin clicks "Cancel" in the dialog:\*\* System aborts the</p><p>activation process and returns to the previous page.</p>|System Admin / System|Activation is aborted.||
|15|<p>\*\*If no (not single-holder role, or no active conflict, or after replacement):\*\* System changes the</p><p>user's status to 'Active' in the database.</p>|System|The 'Last Update Date' is automatically refreshed.||
|16|\*\*System sends a WhatsApp message to the user notifying them of their new status and appropriate action.\*\*|System|<p>MSG-ACTDEACT- 009, MSG- ACTDEACT-010,</p><p>MSG-ACTDEACT- 011</p>|Message content varies based on new status (Activated/Deactivated) and includes login link if activated.|
|17|System displays a success message to the System Admin.|System|<p>MSG-ACTDEACT-</p><p>003, MSG- ACTDEACT-004</p>|Message varies based on action (Activated/Deactivated).|
|18|System updates the user list/details page to reflect the new status.|System|||
|19|System Admin can continue managing users.|System Admin|||

Alternative Flow Table

|**Alternative Scenario**|**Condition**|**Action**|<p>**Related Message**</p><p>**Codes**</p>|**Resolution**|
| :- | :- | :- | :- | :- |
|<p>\*\*Action Not</p><p>Applicable (Button Hidden)\*\*</p>|<p>System Admin attempts to</p><p>click "Activate" for an 'Active' user, or "Deactivate" for an 'Inactive' user.</p>|<p>The respective</p><p>"Activate" or "Deactivate" button/link is</p><p>\*\*hidden\*\*, preventing the action.</p>||<p>System Admin cannot</p><p>perform the action</p>|
|<p>\*\*Deactivate - Single-Holder Role</p><p>Prevention\*\*</p>|<p>System Admin attempts to deactivate a user who</p><p>holds ONLY a single-</p>|System displays an error message|<p>MSG- ACTDEACT-</p><p>007</p>|<p>System Admin must first add another user with that</p><p>role and activate them.</p>|

||holder role, AND no other active user holds that role.|preventing deactivation.|||
| :- | :- | :- | :- | :- |
|\*\*Activate - Role Replacement - Cancelled\*\*|<p>System Admin attempts to activate a user with a single-holder role conflict, and clicks "Cancel" in the</p><p>replacement confirmation dialog.</p>|System closes the dialog and aborts the activation process.|Activation is aborted.||
|\*\*Confirmation Canceled (General)\*\*|<p>System Admin clicks "Deactivate" or "Activate" but then clicks "Cancel" in</p><p>the initial confirmation dialog.</p>|System closes the confirmation dialog and no status change occurs.|No action performed.||
|<p>\*\*System Error</p><p>during Status Change\*\*</p>|A backend error occurs during the status update.|<p>System displays a</p><p>generic error message.</p>|<p>MSG-</p><p>ACTDEACT- 005</p>|System Admin can retry later or contact support.|
|\*\*User Not Found\*\*|<p>The selected user's ID is</p><p>invalid or the user has been deleted.</p>|<p>System displays an</p><p>error message (e.g., "User not found").</p>|<p>MSG-</p><p>ACTDEACT- 006</p>|System Admin can return to the user list.|
|\*\*WhatsApp Message Failure\*\*|The system fails to send the WhatsApp notification message to the user.|System logs the failure.|MSG- ACTDEACT- 011|<p>System Admin may need to manually notify the user or verify mobile number. \*\*Note: This</p><p>failure does NOT prevent the status change.\*\*</p>|

Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Deactivate User - Success (Non-Single Holder Role)\*\*|A System Admin is logged in. An 'Active' user (e.g., Accountant) exists.|The System Admin clicks "Deactivate" for the user and clicks "Confirm" in the dialog.|<p>The user's status is changed to 'Inactive', any active sessions are terminated, a success message "User [User Name] has been deactivated successfully." is displayed, a WhatsApp message is sent to the user notifying them of deactivation, and the</p><p>user list/details page reflects the new status.</p>|
|\*\*Deactivate User - Prevention (Single- Holder Role)\*\*|A System Admin is logged in. An 'Active' user "John Doe" holds ONLY the "Legal Counsel" role, and no other active user holds that role.|The System Admin clicks "Deactivate" for "John Doe" and clicks "Confirm" in the dialog.|<p>The system displays an error message "You can't deactivate this user [John Doe] with role [Legal Counsel], you should add another user with the same role first.", and "John Doe"'s status</p><p>remains 'Active'. No WhatsApp message is sent.</p>|
|\*\*Activate User - Success (No Conflict)\*\*|A System Admin is logged in. An 'Inactive' user (e.g., Accountant) exists.|The System Admin clicks "Activate" for the user and clicks "Confirm" in the dialog.|<p>The user's status is changed to 'Active', a success message "User [User Name] has been activated successfully." is displayed, a WhatsApp message is sent to the user notifying them of activation</p><p>and login, and the user list/details page reflects the new status.</p>|
|\*\*Activate User - Success (Single- Holder Role, With Replacement)\*\*|<p>A System Admin is logged in. An 'Inactive' user "Jane Doe" holds ONLY the "Legal Counsel" role. An 'Active' user "John Smith"</p><p>currently holds ONLY the "Legal Counsel" role.</p>|The System Admin clicks "Activate" for "Jane Doe", clicks "Confirm" in the initial dialog, then clicks "Replace" in the role replacement dialog.|The existing user "John Smith" is deactivated, "Jane Doe"'s status is changed to 'Active', a success message "User [Jane Doe] has been activated successfully." is displayed, a WhatsApp message is sent to "Jane Doe" notifying them of activation and|

||||login, and the user list/details page reflects both status changes.|
| :- | :- | :- | :- |
|\*\*Activate User - Role Replacement - Cancelled\*\*|<p>A System Admin is logged in. An 'Inactive' user "Jane Doe" holds ONLY the "Legal Counsel" role. An 'Active' user "John Smith"</p><p>currently holds ONLY the "Legal Counsel" role.</p>|The System Admin clicks "Activate" for "Jane Doe", clicks "Confirm" in the initial dialog, then clicks "Cancel" in the role replacement dialog.|The role replacement dialog closes, "Jane Doe"'s status remains 'Inactive', "John Smith"'s status remains 'Active', and the activation process is aborted. No WhatsApp message is sent.|
|\*\*Deactivate User - Button Hidden (Already Inactive)\*\*|A System Admin is logged in. An 'Inactive' user exists.|<p>The System Admin views the user</p><p>list/details page for this user.</p>|<p>The "Deactivate" button/link is</p><p>\*\*hidden\*\*.</p>|
|\*\*Activate User - Button Hidden (Already Active)\*\*|A System Admin is logged in. An 'Active' user exists.|<p>The System Admin views the user</p><p>list/details page for this user.</p>|<p>The "Activate" button/link is</p><p>\*\*hidden\*\*.</p>|
|<p>\*\*Status Change - Confirmation</p><p>Canceled (General)\*\*</p>|A System Admin clicks "Deactivate" for a user.|<p>The System Admin clicks "Cancel" in the</p><p>initial confirmation dialog.</p>|The confirmation dialog closes, and the user's status remains unchanged.|
|\*\*System Error during Status Change\*\*|A System Admin attempts to deactivate a user.|A backend error occurs during the status update.|<p>The system displays a generic error message "An error is occurred while changing user status." and the user's</p><p>status remains unchanged. No WhatsApp message is sent.</p>|
|\*\*WhatsApp Message Failure (Status Change Success)\*\*|A System Admin successfully activates a user.|The system attempts to send the WhatsApp notification but it fails.|<p>The user's status is successfully changed, a success message is displayed to the admin, but no WhatsApp message is received by the</p><p>user. The failure is logged.</p>|

Data Entities Table

Entity Name: User (for activate/deactivate)

|<p>**Attrib ute (Engli**</p><p>**sh)**</p>|<p>**Attrib ute (Arabi**</p><p>**c)**</p>|**Mandatory/Op tional**|**Attribute Type**|**Data Leng th**|<p>**Integratio n Requirem**</p><p>**ents**</p>|**Default Value**|<p>**Conditi on (if needed**</p><p>**)**</p>|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | - | :- | :- | :- | :- | :- | :- |
|User ID|معرف المستخدم|Mandatory|Number|N/A|<p>Database</p><p>Primary Key</p>|N/A|N/A|Unique identifier|12345|12345|
|Name|اϻسم|Mandatory|Text|Max 255|Database Field|N/A|N/A|N/A|محمد أحمد|<p>Moham</p><p>med Ahmed</p>|
|Status|الحالة|Mandatory|Boolean/Drop down|N/A|Database Field|Active|N/A|Active/Inacti ve|نشط|Active|
|<p>Last</p><p>Update Date</p>|<p>تاريخ</p><p>آخر تحديث</p>|Mandatory|DateTime|N/A|<p>Database</p><p>Field</p>|<p>Current</p><p>Timesta mp</p>|N/A|<p>Automaticall</p><p>y set on</p><p>creation and update.</p>|<p>2023-</p><p>10-26</p><p>15:00:00</p>|<p>2023-</p><p>10-26</p><p>15:00:00</p>|
|Role|الدور|Mandatory|<p>Relation to</p><p>Role Entity</p>|N/A|<p>Database</p><p>Many-to- Many</p>|N/A|<p>At least</p><p>one role must be selected</p><p>,</p><p>conditio nal</p>|<p>Predefined</p><p>roles only</p>|<p>مدير</p><p>صندوق، عضو مجلس</p>|<p>Fund</p><p>Manager</p><p>, Board Member</p>|

||||||||logic applies||||
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|Mobile|رقم الجوال|Mandatory|Text|Max 10|Database Field|N/A|N/A|<p>Saudi mobile format (e.g., 05XXXXXX XX),</p><p>Numeric only, Unique</p><p>(as username).</p>|<p>0501234</p><p>567</p>|<p>0501234</p><p>567</p>|

Messages/Notifications Table

|**Message Code**|**Message (English)**|**Message (Arabic)**|**Message Type**|<p>**Communication**</p><p>**Method**</p>|
| :- | :- | :- | :- | :- |
|MSG- ACTDEACT- 001|Are you sure you want to deactivate [User Name]? This user will no longer be able to log in.|<p>هل أنت متأكد من إلغاء تفعيل ؟ لن يتمكن هذا ]اسم المستخدم[ المستخدم من تسجيل الدخول بعد</p><p>نϵا.</p>|Confirmation Dialog|In-App|
|<p>MSG-</p><p>ACTDEACT- 002</p>|<p>Are you sure you want to activate</p><p>[User Name]? This user will be able to log in.</p>|<p>اسم [هل أنت متأكد من تفعيل ؟ سيتمكن هذا ]المستخدم</p><p>.المستخدم من تسجيل الدخول</p>|Confirmation Dialog|In-App|
|<p>MSG-</p><p>ACTDEACT- 003</p>|User [User Name] has been deactivated successfully.|اسم [تم إلغاء تفعيل المستخدم .بنجاح ]المستخدم|Success Message|In-App|
|<p>MSG-</p><p>ACTDEACT- 004</p>|User [User Name] has been activated successfully.|اسم [تم تفعيل المستخدم .بنجاح ]المستخدم|Success Message|In-App|
|<p>MSG- ACTDEACT-</p><p>005</p>|An error is occurred while changing user status.|لم يتم ,حدث خطأ بالنظام .تغيير حالة المستخدم|Error Message|In-App|
|<p>MSG-</p><p>ACTDEACT- 006</p>|User not found.|.لم يتم العثور على المستخدم|Error Message|In-App|
|MSG- ACTDEACT- 007|<p>You can't deactivate this user [User Name] with role [Role</p><p>Name]. You should add another user with the same role first.</p>|<p>ϻ يمكنك إلغاء تفعيل المستخدم اسم [بالدور ]اسم المستخدم[ يجب إضافة مستخدم ].الدور</p><p>.آخر بنفس الدور أوϻا</p>|Error Message|In-App|
|MSG- ACTDEACT- 008|<p>There is another active user [Existing User Name] with the</p><p>same role [Role Name]. Do you want to replace him?</p>|<p>اسم [يوجد مستخدم نشط آخر بنفس الدور ]المستخدم الحالي هل تريد</p><p>استبداله؟ ].اسم الدور[</p>|Confirmation Dialog|In-App|
|<p>\*\*MSG- ACTDEACT-</p><p>009\*\*</p>|<p>\*\*Your account for Jadwa Fund Board Management has been activated. You can now log in</p><p>using your registered mobile number.\*\*</p>|<p>تم تفعيل حسابك في تطبيق \*\* .إدارة</p><p>مجالس صناديق جدوى يمكنك اϵن تسجيل الدخول باستخدام رقم جوالك .\*\*المسجل</p>|\*\*Success Message\*\*|\*\*WhatsApp\*\*|
|<p>\*\*MSG- ACTDEACT-</p><p>010\*\*</p>|<p>\*\*Your account for Jadwa Fund Board Management has been</p><p>deactivated. You will no longer be able to log in.\*\*</p>|<p>تم إلغاء تفعيل حسابك في \*\* تطبيق إدارة مجالس صناديق لن تتمكن من</p><p>تسجيل .جدوى .\*\*الدخول بعد اϵن</p>|\*\*Success Message\*\*|\*\*WhatsApp\*\*|
|<p>\*\*MSG- ACTDEACT-</p><p>011\*\*</p>|<p>\*\*Failed to send WhatsApp</p><p>notification for status change. Please notify the user manually.\*\*</p>|<p>فشل إرسال إشعار الواتساب \*\* يرجى إخطار .لتغيير الحالة</p><p>.\*\*المستخدم يدويا</p>|\*\*Warning Message\*\*|\*\*In-App / System Log\*\*|

Screen Elements Table

|**Element ID**|**Element Type**|<p>**Element**</p><p>**Name (English)**</p>|<p>**Eleme**</p><p>**nt Name**</p>|**Required/Opt ional**|<p>**Validat**</p><p>**ion Rules**</p>|**Business Logic**|<p>**Relat**</p><p>**ed Data**</p>|**User Interaction**|**Accessibili ty Notes**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |

||||<p>**(Arabi**</p><p>**c)**</p>||||<p>**Entit**</p><p>**y**</p>|||
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|ELM- ACTDEA CT-001|Button/Li nk|Deactivat e User Button|رابط /زر إلغاء تفعيل المستخدم|Conditional|N/A|<p>Initiates deactivation process.</p><p>\*\*Hidden if user is Inactive or if deactivation is prevented by single-holder role rule.\*\*</p>|N/A|Click|Clear label, visible/ena bled based on conditions.|
|<p>ELM-</p><p>ACTDEA CT-002</p>|<p>Button/Li</p><p>nk</p>|<p>Activate</p><p>User Button</p>|<p>رابط /زر</p><p>تفعيل المستخدم</p>|Conditional|N/A|<p>Initiates</p><p>activation process.</p><p>\*\*Hidden if user is Active.\*\*</p>|N/A|Click|<p>Clear label,</p><p>visible/ena bled based on conditions.</p>|
|ELM- ACTDEA CT-003|Confirmat ion Dialog|<p>Status Change Confirmat</p><p>ion Dialog</p>|نافذة تأكيد تغيير الحالة|Mandatory|N/A|Prompts System Admin to confirm status change.|N/A|View, Click (Confirm/Ca ncel)|Clear message, accessible buttons.|
|ELM- ACTDEA CT-004|Text Label|Confirmat ion Message Text|نص رسالة التأكيد|Mandatory|N/A|<p>Displays "Are you sure you want to [deactivate/acti</p><p>vate] [User Name]?"</p>|N/A|View|Dynamic content for action and user name.|
|<p>ELM- ACTDEA</p><p>CT-005</p>|Button|<p>Confirm Button (in</p><p>Dialog)</p>|زر تأكيد في ( )النافذة|Mandatory|N/A|<p>Confirms the status change</p><p>action.</p>|N/A|Click|Clearly labeled.|
|<p>ELM- ACTDEA</p><p>CT-006</p>|Button|<p>Cancel Button (in</p><p>Dialog)</p>|زر إلغاء في ( )النافذة|Mandatory|N/A|<p>Cancels the status change</p><p>action.</p>|N/A|Click|Clearly labeled.|
|ELM- ACTDEA CT-007|Text Label|<p>Success/E rror</p><p>Message Display</p>|<p>عرض رسالة</p><p>ال/النجاح خطأ</p>|Conditional|N/A|<p>Displays the outcome of the</p><p>status change attempt.</p>|N/A|View|Prominent display, clear text.|
|ELM- ACTDEA CT-008|Confirmat ion Dialog|Role Replacem ent Confirmat ion (Activate)|تأكيد استبدال الدور )تفعيل(|Conditional|N/A|<p>Prompts System Admin to confirm replacing an existing user with a single- holder role</p><p>during activation.</p>|N/A|View, Click (Replace/Ca ncel)|Clear message, accessible buttons.|
|ELM- ACTDEA CT-009|Button|Replace Button (in Dialog)|زر استبدال في ( )النافذة|Mandatory|N/A|<p>Confirms replacement of existing user and proceeds with current</p><p>user's activation.</p>|N/A|Click|Clearly labeled, primary action in dialog.|
|ELM- ACTDEA CT-010|Button|Cancel Button (in Dialog)|زر إلغاء في ( )النافذة|Mandatory|N/A|<p>Cancels the replacement</p><p>and aborts activation.</p>|N/A|Click|Clearly labeled.|

Summary Section

This rewritten user story for "Activate/Deactivate System User" now fully incorporates the requirement for

\*\*sending a WhatsApp message to the user notifying them of their new status and appropriate action.\*\* This is integrated into the Process Flow and new messages have been added to the Messages/Notifications Table. The complex logic for single-holder role conflicts during activation/deactivation remains fully detailed, ensuring a comprehensive and precise definition for managing user status.

\*\*Important Note:\*\* I am still awaiting your clarification on the "Action Not Applicable (Role/Assignment)" logic for the "Resend Account Registration Message" user story from our previous interaction. The current logic in that story (and reflected in its Acceptance Criteria) is: "Action is NOT applicable if the user has ONLY the 'Board Member' role AND is NOT assigned to any fund board." However, with `IsAssignedToFundBoard` removed from the User entity, this specific condition cannot be implemented as written. \*\*Please confirm how the "Resend Message" eligibility for Board Members should be determined without

`IsAssignedToFundBoard`.\*\*

\*\*Key Implementation Considerations:\*\*

* Implement the precise logic for checking single-holder role conflicts during both activation and deactivation.
* Ensure the WhatsApp notification is sent reliably and contains the correct dynamic content based on the status change.
* Handle WhatsApp API failures gracefully for these notifications.

\*\*Dependencies and Risks:\*\*

* \*\*Dependency:\*\* This story depends on the "View System Users List" or "View System User Details" stories for initiating the action. It also impacts the user's ability to log in (Login story) and relies on WhatsApp Business API integration.
* \*\*Risk:\*\* Accidental deactivation/activation or incorrect role assignment due to complex logic. Mitigation: Thorough testing, clear UI feedback.
* \*\*Risk:\*\* WhatsApp message delivery failures leading to user confusion about their status. Mitigation: Robust error handling, logging, and alternative notification options.

  ---

  I have now rewritten the "Activate/Deactivate System User" user story with the new WhatsApp notification requirement. Please provide clarification on the "Resend Account Registration Message" logic regarding Board Members when you're ready.

# User Story JDWA-1251: Edit Existing System User

Introduction Section

This user story describes the functionality for System Administrators to modify the details and roles of an existing user account within the Fund Board Management Application. This ensures that user information remains accurate and their permissions are up-to-date, reflecting any changes in personnel or responsibilities.

\*\*The user's status is displayed as a label and cannot be changed from this screen.\*\* A new validation is introduced for specific roles (Legal Counsel, Finance Controller, Compliance and Legal Managing Director, Head of Real Estate) to check for existing active users with the same selected role, prompting for a replacement if found. The Screen Elements Table provides a detailed breakdown of the UI components on the user editing form.

Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|Edit Existing System User|<p>Allows System Administrators to modify</p><p>details and roles of existing user accounts.</p>|
|\*\*User Story\*\*|<p>As a \*\*System Administrator\*\*, I want to \*\*edit an existing user's details and roles, handling potential role replacements,\*\*</p><p>so that I can \*\*keep user information accurate and manage their system access.\*\*</p>||
|\*\*Story Points\*\*|10|<p>Involves retrieving data, complex modification, validation with</p><p>replacement workflow, and updating.</p>|
|\*\*User Roles\*\*|System Admin|<p>Only System Administrators can edit</p><p>existing users.</p>|
|\*\*Access Requirements\*\*|<p>Authenticated and authorized System Admin access to the User Management module. User must be selected from the user list or</p><p>details page.</p>|User must be logged in as a System Admin.|
|\*\*Trigger\*\*|System Admin clicks "Edit" button/link for a specific user from the user list or user details page.|User initiates the action.|
|\*\*Frequency of Use\*\*|Medium|<p>User data changes periodically due to role</p><p>changes, contact updates, or status changes.</p>|
|\*\*Pre-condition\*\*|System Admin is logged in, has access to the User Management module, and the user to be edited exists.|<p>The system is operational,</p><p>and the selected user's data is available.</p>|
|\*\*Business Rules\*\*|<p>1\. All mandatory fields must be filled.<br>2. Email address can only be changed if it remains unique.<br>3. Country Code and Mobile No. are mandatory and restricted to Saudi mobile numbers only (starting with 05, 9 digits after 05).<br>4. The Mobile No. (username) can be changed, but must remain unique and valid Saudi format.<br>5. At least one role must be assigned to a user.<br>\*\*6. Role Selection Logic:\*\* Multi-select for roles is enabled \*\*ONLY IF\*\* the selected roles are ('Fund Manager' AND 'Board Member') OR ('Associate Fund Manager' AND 'Board Member'). Otherwise, multi-select is disabled, restricting to a single role.<br>7. CV and Passport No. are optional fields.<br>8. The 'Last Update Date' must be automatically updated upon successful modification.<br>9. The 'Registration</p><p>Message Is Sent' flag and 'Registration Is Completed' flag are displayed but not directly editable by the System Admin in this</p>|Ensures data integrity, proper access control, and accurate record keeping during user modification, with a critical role replacement workflow.|

||<p>view.<br>10. User Status is displayed as a label and cannot be modified from this screen.<br>\*\*11. Unique Role Validation with Replacement (on Save):\*\* If a selected role is one of: 'Legal Counsel', 'Finance Controller', 'Compliance and Legal Managing Director', 'Head of Real Estate', the system must check for another \*active\* user (who is \*not\* the user currently being edited) currently holding \*only\* that specific role. If found, a confirmation dialog is displayed to replace the existing user. If 'Replace' is chosen, the existing user is deactivated before the current user's roles are updated. If 'Cancel' is chosen, the System Admin is returned to the form to modify role selection or other</p><p>details.</p>||
| :- | :- | :- |
|\*\*Post- condition\*\*|The user account is updated with the modified details and roles, and the 'Last Update Date' is refreshed. If a replacement occurred, the previous user with that specific role is deactivated.|<p>The system state reflects the successful update</p><p>and/or replacement of a user.</p>|
|\*\*Risk\*\*|1\. Accidental modification of critical user data.<br>2. Incorrect role assignment leading to unintended access changes.<br>3. Data validation errors.<br>4. Accidental deactivation of a user during replacement.|<p>Mitigation: Confirmation prompts, robust input validation, clear UI, audit trails, and careful</p><p>implementation of replacement logic.</p>|
|\*\*Assumptions\*\*|<p>1\. Core Identity in ASP.NET is used for user and permission management.<br>2. System Admin understands the implications of status changes (e.g., deactivation/activation).<br>3. The roles 'Legal Counsel', 'Finance Controller', 'Compliance and Legal</p><p>Managing Director', 'Head of Real Estate' are considered "single- holder" roles for active users.</p>|These assumptions simplify the initial implementation scope.|
|\*\*UX/UI Design Link\*\*|N/A (Refer to general form design principles, confirmation dialog for replacement)|<p>No specific mockups provided in the BRD for</p><p>this section.</p>|

Process Flow Table

|**Step**|**Action Description**|**Actor**|<p>**Related Message**</p><p>**Codes**</p>|**Notes**|
| :- | :- | :- | :- | :- |
|1|<p>System Admin is viewing the "View System Users List" or</p><p>"View User Details" page.</p>|System Admin|||
|2|<p>System Admin identifies a user</p><p>and clicks the "Edit" button/link for that user.</p>|System Admin|||
|3|System retrieves the selected user's current details and pre- populates an editable form.|System|<p>All fields are pre-filled with existing data. Role selection dynamically adjusts based on current roles. User</p><p>Status is displayed as a non-editable label.</p>||
|4|System Admin modifies user details (e.g., Name, Email, Country Code, Mobile No., IBAN, Nationality, CV, Passport No.).|System Admin|<p>MSG-EDIT-001, MSG-EDIT-002, MSG-EDIT-003,</p><p>MSG-EDIT-005, MSG-EDIT-006</p>|Mobile No. is editable and subject to Saudi format/uniqueness validation.|
|5|System Admin modifies user roles (add or remove roles) using the dynamic role selection control.|System Admin|MSG-EDIT-004|<p>\*\*Conditional logic:\*\* Multi-select for roles is enabled \*\*ONLY IF\*\* the selected roles are ('Fund Manager'</p><p>AND 'Board Member') OR ('Associate Fund Manager' AND 'Board</p>|

|||||Member'). Otherwise, multi-select is disabled, restricting to a single role.|
| :- | :- | :- | :- | :- |
|6|<p>System Admin reviews the</p><p>modified information and clicks the "Save Changes" button.</p>|System Admin|||
|vv7|System performs server-side validation on all submitted data.|System|<p>MSG-EDIT-001, MSG-EDIT-002, MSG-EDIT-003, MSG-EDIT-004,</p><p>MSG-EDIT-005, MSG-EDIT-006</p>|Checks for mandatory fields, email format, uniqueness, role selection based on business rules, and Saudi mobile number format/uniqueness.|
|8|<p>\*\*If validation is successful, System checks if any selected role is a "single-holder" role (Legal Counsel, Finance Controller, Compliance and Legal Managing Director, Head of Real Estate) AND if this role is different from</p><p>the user's current single-holder role (if any).\*\*</p>|System||This check excludes self-assignment.|
|9|<p>\*\*If a single-holder role is selected AND an active user (who is NOT the user currently being edited) already holds ONLY that specific role:\*\* System displays a</p><p>confirmation dialog asking to replace the existing user.</p>|System|MSG-EDIT-010|Dialog includes existing user's name and role. Buttons: "Replace", "Cancel".|
|10|<p>\*\*If System Admin clicks "Replace" in the dialog:\*\* System</p><p>deactivates the existing user with that role.</p>|System Admin / System||The existing user's status is changed to 'Inactive'.|
|11|<p>\*\*If System Admin clicks "Cancel" in the dialog:\*\* System returns the System Admin to the user editing form, allowing them to modify role selection or other</p><p>details.</p>|System Admin / System||This allows the admin to choose a non-conflicting role or adjust other inputs.|
|12|<p>\*\*If System Admin proceeds (either no conflict, or after cancelling replacement and modifying input):\*\* System updates the user account in the</p><p>database with the modified details and roles.</p>|System||The 'Last Update Date' is automatically refreshed.|
|13|System displays a success message.|System|MSG-EDIT-007||
|14|<p>System redirects the System Admin back to the "View System Users List" or "View User</p><p>Details" page, with the updated user information visible.</p>|System|||

Alternative Flow Table

|**Alternative Scenario**|**Condition**|**Action**|<p>**Related Message**</p><p>**Codes**</p>|**Resolution**|
| :- | :- | :- | :- | :- |
|<p>\*\*Missing Mandatory</p><p>Field\*\*</p>|<p>System Admin attempts to save without filling a mandatory</p><p>field (e.g., Name, Mobile No.).</p>|System displays a specific validation error message|MSG- EDIT-001|<p>System Admin must fill in all mandatory</p><p>fields.</p>|

|||"Required Field." next to the missing field.|||
| :- | :- | :- | :- | :- |
|\*\*Invalid Email Format\*\*|System Admin enters an email address in an incorrect format.|<p>System displays a validation</p><p>error message "Invalid email format." for the email field.</p>|MSG- EDIT-002|<p>System Admin must</p><p>correct the email format.</p>|
|\*\*Duplicate Email\*\*|System Admin enters an email address that already exists for another user.|<p>System displays an error message "User with this</p><p>email already exists." indicating duplicate email.</p>|MSG- EDIT-003|System Admin must enter a unique email address.|
|<p>\*\*Invalid Mobile No. Format</p><p>(Non-Saudi)\*\*</p>|System Admin enters a mobile number that does not conform to Saudi mobile number format.|<p>System displays a validation error message "Invalid Saudi mobile number format.</p><p>Please enter a 10-digit number starting with 05." for the mobile number field.</p>|MSG- EDIT-005|System Admin must enter a valid Saudi mobile number.|
|\*\*Duplicate Mobile No. (Username)\*\*|System Admin enters a mobile number that already exists as a username for another user.|<p>System displays an error message "Mobile number is already in use as a username." indicating the</p><p>mobile number is already in use.</p>|MSG- EDIT-006|System Admin must enter a unique mobile number.|
|\*\*Invalid Role Selection (Logic Violation)\*\*|<p>System Admin selects roles that violate the conditional logic (e.g., 'Fund Manager' and 'Accountant' simultaneously, or</p><p>more than one role when multi- select is disabled).</p>|System displays a warning/error message "Invalid role selection."|MSG- EDIT-004|System Admin must correct the role selection.|
|\*\*No Roles Remaining\*\*|System Admin attempts to remove all roles from a user.|<p>System displays a warning/error message "Invalid role selection." prompting to select at least</p><p>one role.</p>|MSG- EDIT-004|System Admin must ensure at least one role is assigned.|
|\*\*CV File Upload Error\*\*|An error occurs during the CV file upload (e.g., file too large, unsupported format).|<p>System displays an error message "Invalid file format or size for CV. Please upload a PDF or DOCX file up to</p><p>10MB." related to the file upload.</p>|MSG- EDIT-008|System Admin must re-upload a valid CV file.|
|\*\*Role Replacement - Cancelled\*\*|<p>System Admin selects a single- holder role that has an active user, and clicks "Cancel" in the</p><p>replacement confirmation dialog.</p>|System closes the dialog and returns the System Admin to the user editing form.||System Admin can modify input and re- attempt save.|
|\*\*System Error during Update\*\*|<p>A backend error occurs during user update (e.g., database</p><p>connection issue).</p>|<p>System displays a generic error message "An error is</p><p>occurred while saving data."</p>|MSG- EDIT-009|<p>System Admin can retry the operation</p><p>or contact support.</p>|
|\*\*Cancel Edit\*\*|System Admin decides not to save changes.|System Admin clicks a "Cancel" button.||<p>System discards all unsaved changes and redirects back</p><p>to the previous page (user list or details).</p>|

Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Successful User|A System Admin is|The System Admin modifies the|The user's details are updated|
|Update (No Role|logged in and on the|user's Name, changes their|in the system, the 'Last Update|
|Conflict)\*\*|"Edit User" form for|Mobile No. to a new valid Saudi|Date' is refreshed, a success|
||an existing user.|number, and adds a valid new|message "Record Updated|
|||role (e.g., "Accountant") that|Successfully" is displayed, and|

|||does not conflict, then clicks "Save Changes".|the System Admin is redirected to the user list or details page.|
| :- | :- | :- | :- |
|\*\*Successful User Update (Single- Holder Role, No Existing User)\*\*|A System Admin is logged in and on the "Edit User" form for an existing user.|<p>The System Admin changes the user's role to "Legal Counsel" (a single-holder role) where no other active user currently holds</p><p>that role, and clicks "Save Changes".</p>|The user's role is updated, a success message "Record Updated Successfully" is displayed, and the System Admin is redirected.|
|\*\*Successful User Update (Single- Holder Role, With Replacement)\*\*|<p>A System Admin is logged in and on the "Edit User" form for user "Jane Doe". An active user "John Smith" currently holds</p><p>ONLY the "Legal Counsel" role.</p>|The System Admin changes "Jane Doe"'s role to "Legal Counsel", clicks "Save Changes", and then clicks "Replace" in the confirmation dialog.|The existing user "John Smith" is deactivated, "Jane Doe"'s role is updated to "Legal Counsel", a success message "Record Updated Successfully" is displayed, and the System Admin is redirected.|
|\*\*Role Replacement - Cancelled (Admin Modifies Input)\*\*|A System Admin is on the "Edit User" form for user "Jane Doe". An active user "John Smith" currently holds ONLY the "Legal Counsel" role.|The System Admin changes "Jane Doe"'s role to "Legal Counsel", clicks "Save Changes", then clicks "Cancel" in the confirmation dialog, then changes "Jane Doe"'s role to "Accountant" and clicks "Save Changes" again.|<p>The user "Jane Doe"'s role is updated to "Accountant", the existing user "John Smith" remains active, a success message "Record Updated Successfully" is displayed, and</p><p>the System Admin is redirected.</p>|
|\*\*Mandatory Field Validation\*\*|A System Admin is on the "Edit User" form.|The System Admin clears a mandatory field (e.g., Name) and clicks "Save Changes".|<p>The system displays a specific validation error message "Required Field" for the</p><p>missing field, and the user's data is not updated.</p>|
|\*\*Unique Mobile No. Validation\*\*|A System Admin is on the "Edit User" form.|The System Admin enters a Saudi mobile number that already exists as a username for another user and clicks "Save Changes".|<p>The system displays an error message "Mobile number is already in use as a username.", and the user's data is not</p><p>updated.</p>|
|\*\*Conditional Role Selection - Invalid (Fund Manager & Accountant)\*\*|A System Admin is on the "Edit User" form.|The System Admin attempts to assign "Fund Manager" and "Accountant" roles simultaneously.|<p>The system displays an error message "Invalid role selection." and prevents the</p><p>invalid selection or does not update the user's roles.</p>|
|<p>\*\*Conditional Role Selection - Valid (Associate Fund</p><p>Manager & Board Member)\*\*</p>|A System Admin is on the "Edit User" form.|The System Admin selects "Associate Fund Manager" and "Board Member" roles, and clicks "Save Changes".|The system successfully updates the user with both roles assigned.|
|\*\*CV Upload Size Limit\*\*|A System Admin is on the "Edit User" form.|The System Admin attempts to upload a CV file larger than 10 MB.|<p>The system displays an error message "Invalid file format or size for CV. Please upload a PDF or DOCX file up to</p><p>10MB.", and the file is not uploaded.</p>|
|\*\*System Error during Save\*\*|<p>A System Admin is on the "Edit User" form</p><p>and clicks "Save Changes".</p>|An unexpected backend error occurs during the saving process.|<p>The system displays the message "An error is occurred</p><p>while saving data" and the user's data is not updated.</p>|
|\*\*Cancel Edit\*\*|A System Admin is on the "Edit User" form.|The System Admin clicks the "Cancel" button after making changes.|<p>The system discards all unsaved changes and redirects</p><p>the System Admin back to the previous page.</p>|

Data Entities Table

Entity Name: User (for editing)

|**Attrib ute (Engli sh)**|**Attri bute (Ara bic)**|<p>**Mandatory**</p><p>**/Optional**</p>|**Attribute Type**|**Dat a Len gth**|**Integra tion Requir ements**|**Defau lt Value**|<p>**Cond ition (if neede**</p><p>**d)**</p>|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | - | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|User ID|معرف المستخ دم|Mandatory|Number|N/A|<p>Databas e</p><p>Primary Key</p>|N/A|N/A|<p>Unique identifier,</p><p>Read-only for edit</p>|12345|12345|
|Name|اϻسم|Mandatory|Text|<p>Ma</p><p>x 255</p>|Databas e Field|N/A|N/A|N/A|محمد أحمد|Mohammed Ahmed|
|Email|البريد اϹلكتر وني|Mandatory|Text|Ma x 255|Databas e Field|N/A|N/A|<p>Unique, Valid</p><p>email format</p>|mohammed.ahme <d@jadwa.com>|mohammed.ahme <d@jadwa.com>|
|Countr y Code|رمز الدولة|Mandatory|Text|Ma x 5|Databas e Field|+966|N/A|<p>Must be '+966' for</p><p>Saudi numbers.</p>|+966|+966|
|Mobil e|رقم الجوال|Mandatory|Text|Ma x 10|Databas e Field|N/A|N/A|<p>Saudi mobile format (e.g., 05XXXX XXXX),</p><p>Numeric only, Unique (as username)</p><p>.</p>|0501234567|0501234567|
|IBAN|<p>رقم الحس اب المصر</p><p>في الدولي</p>|Optional|Text|Ma x 34|Databas e Field|N/A|N/A|<p>Valid IBAN</p><p>format</p>|SA987654321098 7654321098|SA987654321098 7654321098|
|Nation ality|الجنس ية|Optional|Text|<p>Ma</p><p>x 100</p>|Databas e Field|N/A|N/A|N/A|سعودي|Saudi|
|CV|السيرة الذاتية|Optional|File Path/URL|N/A|<p>File Storage (e.g.,</p><p>Azure Blob)</p>|N/A|N/A|<p>PDF, DOCX</p><p>only, Max 10MB</p>|.pdfمحمد\_ذاتية\_سيرة|Mohammed\_CV. pdf|
|Passpo rt No.|<p>رقم</p><p>جواز السفر</p>|Optional|Text|Ma x 20|Databas e Field|N/A|N/A|Alphanu meric|A12345678|A12345678|
|Status|الحالة|Mandatory|Boolean/D ropdown|N/A|Databas e Field|Activ e|N/A|Active/In active|نشط|Active|
|Role|الدور|Mandatory|Relation to Role Entity|N/A|Databas e Many- to- Many|N/A|<p>At least one</p><p>role must</p>|Predefine d roles only|مدير صندوق، عضو مجلس|Fund Manager, Board Member|

||||||||<p>be select ed, condit ional logic</p><p>applie s</p>||||
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|Passw ord|كلمة المرور|Mandatory|Text|N/A|Databas e Field|N/A|N/A|<p>System- generated, stored securely (hashing handled internally)</p><p>.</p>|N/A|N/A|
|Last Updat e Date|تاريخ آخر تحديث|Mandatory|DateTime|N/A|Databas e Field|Curre nt Times tamp|N/A|<p>Automati cally set on creation</p><p>and update.</p>|<p>2023-10-26</p><p>15:00:00</p>|<p>2023-10-26</p><p>15:00:00</p>|
|<p>Regist</p><p>ration Messa ge Is Sent</p>|تم إرسال رسالة التسج يل|Mandatory|Boolean|N/A|<p>Databas</p><p>e Field</p>|<p>0</p><p>(False</p><p>)</p>|N/A|<p>Set to 1 if</p><p>user's role(s) make them eligible for WhatsAp p message (NOT</p><p>only 'Board Member'); 0 if only 'Board Member'. This flag is set based on eligibility and</p><p>attempt to send.</p>|1|True|
|<p>Regist</p><p>ration Is Compl eted</p>|تم اكتمال التسج يل|Mandatory|Boolean|N/A|<p>Databas</p><p>e Field</p>|<p>0</p><p>(False</p><p>)</p>|N/A|<p>Set to 0</p><p>upon user creation. Updated by a separate process (e.g.,</p><p>user's first login).</p>|0|False|

Entity Name: Role (for selection)

|**Attribut e**|<p>**Attribut**</p><p>**e (Arabic)**</p>|**Mandatory/Option al**|**Attribut e Type**|<p>**Data**</p><p>**Lengt h**</p>|<p>**Integration**</p><p>**Requiremen ts**</p>|<p>**Defaul**</p><p>**t Value**</p>|<p>**Conditio**</p><p>**n (if needed)**</p>|<p>**Rules**</p><p>**(if needed)**</p>|**Sampl e in**|**Sampl e in**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |

|<p>**(English**</p><p>**)**</p>|||||||||<p>**Arabi**</p><p>**c**</p>|<p>**Englis**</p><p>**h**</p>|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|Role ID|معرف الدور|Mandatory|<p>Number (Auto- generated</p><p>)</p>|N/A|Database Primary Key|N/A|N/A|Unique identifie r|1|1|
|Role Name|اسم الدور|Mandatory|Text|Max 50|Database Field|N/A|N/A|Unique|مدير النظام|System Admin|

Messages/Notifications Table

|<p>**Message**</p><p>**Code**</p>|**Message (English)**|**Message (Arabic)**|**Message Type**|<p>**Communication**</p><p>**Method**</p>|
| :- | :- | :- | :- | :- |
|MSG- EDIT-001|Required Field.|.حقل إلزامي|Validation Error|In-App|
|MSG- EDIT-002|Invalid email format.|صيغة البريد اϹلكتروني غير .صحيحة|Validation Error|In-App|
|MSG- EDIT-003|User with this email already exists.|يوجد مستخدم بهذا البريد اϹلكتروني .بالفعل|Validation Error|In-App|
|MSG- EDIT-004|Invalid role selection.|.اختيار الدور غير صالح|Validation Error|In-App|
|MSG- EDIT-005|<p>Invalid Saudi mobile number</p><p>format. Please enter a 10-digit number starting with 05.</p>|<p>صيغة رقم الجوال السعودي غير يرجى إدخال رقم مكون من .صالحة</p><p>05\.أرقام يبدأ بـ 10</p>|Validation Error|In-App|
|MSG- EDIT-006|Mobile number is already in use as a username.|رقم الجوال مستخدم بالفعل كاسم .مستخدم|Validation Error|In-App|
|MSG- EDIT-007|Record Updated Successfully.|.تم تحديث البيانات بنجاح|Success Message|In-App|
|MSG- EDIT-008|Invalid file format or size for CV. Please upload a PDF or DOCX file up to 10MB.|<p>صيغة الملف أو حجمه غير صالح يرجى تحميل ملف .للسيرة الذاتية</p><p>10بحجم أقصى DOCXأو PDF .ميجابايت</p>|Validation Error|In-App|
|MSG- EDIT-009|An error is occurred while saving data.|لم يتم حفظ ,حدث خطأ بالنظام .البيانات|Error Message|In-App|
|MSG- EDIT-010|<p>There is another active user with the role [Role Name]: [Existing User</p><p>Name]. Do you want to replace him?</p>|<p>اسم [يوجد مستخدم نشط آخر بالدور هل ].اسم المستخدم الحالي[ ]:الدور</p><p>تريد استبداله؟</p>|Confirmation Dialog|In-App|

Screen Elements Table

|**Elem ent ID**|**Element Type**|<p>**Element Name (English**</p><p>**)**</p>|<p>**Elem ent Name**</p><p>**(Ara bic)**</p>|**Required/Op tional**|**Validatio n Rules**|**Business Logic**|**Related Data Entity**|**User Interaction**|**Accessib ility Notes**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|<p>ELM- EDIT</p><p>-001</p>|Page Title|Edit User|<p>تعديل المستخد</p><p>م</p>|N/A|N/A|Displays the title of the user|N/A|View|<p>H1</p><p>heading, clear and</p>|
|||||||<p>editing</p><p>form.</p>|||concise.|
|ELM- EDIT|Input Field|Name Input|حقل اϻسم|Mandatory|Text, Max 255 chars|Collects/dis plays the|User.Name|Type|Clear label,|
|-002||||||user's full|||placehol|
|||||||name.|||der text.|
|<p>ELM- EDIT</p><p>-003</p>|Input Field|Email Input|حقل البريد اϹلكترو ني|Mandatory|<p>Valid email format,</p><p>Max 255 chars</p>|Collects/dis plays the user's email address.|User.Email|Type|<p>Clear label, placehol</p><p>der text, email</p>|

<table><tr><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th><th colspan="1" valign="top">Must be unique.</th><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th><th colspan="1" valign="top"><p>type keyboard</p><p>.</p></th></tr>
<tr><td colspan="1" valign="top"><p>ELM- EDIT</p><p>-004</p></td><td colspan="1" valign="top">Input Field</td><td colspan="1" valign="top">Country Code Input</td><td colspan="1" valign="top">حقل رمز الدولة</td><td colspan="1" valign="top">Mandatory</td><td colspan="1" valign="top">Text, Must be '+966'</td><td colspan="1" valign="top"><p>Collects/dis plays the country dialing code for</p><p>mobile number.</p></td><td colspan="1" valign="top">User.Countr yCode</td><td colspan="1" valign="top">Type (or Pre- filled/Drop down)</td><td colspan="1" valign="top">Clear label, placehol der text.</td></tr>
<tr><td colspan="1" valign="top">ELM-</td><td colspan="1" valign="top">Input Field</td><td colspan="1" valign="top">Mobile</td><td colspan="1" rowspan="10" valign="top"><p>حقل رقم الجوال اسم (</p><p>المستخد )م</p></td><td colspan="1" valign="top">Mandatory</td><td colspan="1" valign="top">Numeric</td><td colspan="1" valign="top">Collects/dis</td><td colspan="1" valign="top">User.Mobile</td><td colspan="1" valign="top">Type</td><td colspan="1" valign="top">Clear</td></tr>
<tr><td colspan="1" valign="top">EDIT</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">No.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">only, 10</td><td colspan="1" valign="top">plays the</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">label,</td></tr>
<tr><td colspan="1" valign="top">-005</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Input</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">digits,</td><td colspan="1" valign="top">user's Saudi</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">placehol</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">(Userna</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">starts with</td><td colspan="1" valign="top">mobile</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">der text,</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">me)</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">'05',</td><td colspan="1" valign="top">phone</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">numeric</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Unique.</td><td colspan="1" valign="top">number,</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">keyboard</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">which</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">.</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">serves as</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">their</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">username.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"><p>ELM- EDIT</p><p>-006</p></td><td colspan="1" valign="top">Input Field</td><td colspan="1" valign="top"><p>IBAN</p><p>Input</p></td><td colspan="1" valign="top"><p>حقل رقم الحساب المصر في</p><p>الدولي</p></td><td colspan="1" valign="top">Optional</td><td colspan="1" valign="top"><p>Valid IBAN</p><p>format, Max 34 chars</p></td><td colspan="1" valign="top"><p>Collects/dis plays the user's Internation</p><p>al Bank</p></td><td colspan="1" valign="top">User.IBAN</td><td colspan="1" valign="top">Type</td><td colspan="1" valign="top">Clear label, placehol der text.</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"><p>Account</p><p>Number.</p></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top">ELM- EDIT</td><td colspan="1" valign="top">Input Field</td><td colspan="1" valign="top">Nationali ty Input</td><td colspan="1" valign="top">حقل الجنسية</td><td colspan="1" valign="top">Optional</td><td colspan="1" valign="top">Text, Max 100 chars</td><td colspan="1" valign="top">Collects/dis plays the</td><td colspan="1" valign="top">User.Nation ality</td><td colspan="1" valign="top">Type</td><td colspan="1" valign="top">Clear label,</td></tr>
<tr><td colspan="1" valign="top">-007</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"><p>user's</p><p>nationality.</p></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"><p>placehol</p><p>der text.</p></td></tr>
<tr><td colspan="1" valign="top"><p>ELM- EDIT</p><p>-008</p></td><td colspan="1" valign="top">File Upload</td><td colspan="1" valign="top"><p>CV</p><p>Upload</p></td><td colspan="1" valign="top">تحميل السيرة الذاتية</td><td colspan="1" valign="top">Optional</td><td colspan="1" valign="top"><p>PDF/DO</p><p>CX only, Max</p></td><td colspan="1" valign="top">Allows user to upload/repl</td><td colspan="1" valign="top">User.CV</td><td colspan="1" valign="top">Click (to browse/upl oad)</td><td colspan="1" valign="top">Clear label, file</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">10MB</td><td colspan="1" valign="top">ace their</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">type/size</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">CV file.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">guidance</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">.</td></tr>
<tr><td colspan="1" valign="top"><p>ELM- EDIT</p><p>-009</p></td><td colspan="1" valign="top">Input Field</td><td colspan="1" valign="top"><p>Passport No.</p><p>Input</p></td><td colspan="1" valign="top"><p>حقل رقم جواز</p><p>السفر</p></td><td colspan="1" valign="top">Optional</td><td colspan="1" valign="top"><p>Alphanu meric,</p><p>Max 20</p></td><td colspan="1" valign="top"><p>Collects/dis plays the</p><p>user's</p></td><td colspan="1" valign="top">User.Passpor tNo</td><td colspan="1" valign="top">Type</td><td colspan="1" valign="top"><p>Clear label,</p><p>placehol</p></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">chars</td><td colspan="1" valign="top"><p>passport</p><p>number.</p></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">der text.</td></tr>
<tr><td colspan="1" valign="top">ELM- EDIT</td><td colspan="1" valign="top">Dropdown/Chec kboxes</td><td colspan="1" valign="top">Role Selection</td><td colspan="1" valign="top">اختيار الدور</td><td colspan="1" valign="top">Mandatory</td><td colspan="1" valign="top">At least one role</td><td colspan="1" valign="top">Allows selection of</td><td colspan="1" valign="top">User.Role</td><td colspan="1" valign="top">Select</td><td colspan="1" valign="top">Clear label,</td></tr>
<tr><td colspan="1" valign="top">-010</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">selected,</td><td colspan="1" valign="top">one or</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">accessibl</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">**conditi</td><td colspan="1" valign="top">more</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">e</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">onal logic</td><td colspan="1" valign="top">predefined</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">options,</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">applies**</td><td colspan="1" valign="top">roles for</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">visual</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">the user.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">feedback</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">**Multi-</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">for</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">select</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">multi-</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">enabled/dis</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">select</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">abled</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">state.</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"><p>dynamicall</p><p>y.**</p></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top">ELM- EDIT</td><td colspan="1" valign="top">Text Display</td><td colspan="1" valign="top">Status Display</td><td colspan="1" valign="top">عرض الحالة</td><td colspan="1" valign="top">Mandatory</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Displays the user's</td><td colspan="1" valign="top">User.Status</td><td colspan="1" valign="top">View</td><td colspan="1" valign="top">Clear label,</td></tr>
<tr><td colspan="1" valign="top">-011</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">current</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">ensure</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">status</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">sufficien</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">(Active/Ina</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
</table>

<table><tr><th colspan="1" rowspan="4" valign="top"></th><th colspan="1" rowspan="4" valign="top"></th><th colspan="1" rowspan="4" valign="top"></th><th colspan="1" rowspan="4" valign="top"></th><th colspan="1" rowspan="4" valign="top"></th><th colspan="1" rowspan="4" valign="top"></th><th colspan="1" valign="top">ctive) as a</th><th colspan="1" rowspan="4" valign="top"></th><th colspan="1" rowspan="4" valign="top"></th><th colspan="1" valign="top">t</th></tr>
<tr><td colspan="1" valign="top">non-</td><td colspan="1" valign="top">contrast.</td></tr>
<tr><td colspan="1" valign="top">editable</td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top">label.</td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"><p>ELM- EDIT</p><p>-012</p></td><td colspan="1" valign="top">Button</td><td colspan="1" valign="top">Save Changes Button</td><td colspan="1" valign="top"><p>زر حفظ التغييرا</p><p>ت</p></td><td colspan="1" valign="top">Mandatory</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Submits the form to update the</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Click</td><td colspan="1" valign="top">Primary action button,</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">user.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"><p>clearly</p><p>labeled.</p></td></tr>
<tr><td colspan="1" valign="top">ELM- EDIT</td><td colspan="1" valign="top">Button</td><td colspan="1" valign="top">Cancel Button</td><td colspan="1" valign="top">زر إلغاء</td><td colspan="1" valign="top">Mandatory</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Discards changes</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Click</td><td colspan="1" valign="top">Secondar y action</td></tr>
<tr><td colspan="1" valign="top">-013</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">and returns</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">button,</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">to the</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">clearly</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">previous</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">labeled.</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">page.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"><p>ELM- EDIT</p><p>-014</p></td><td colspan="1" valign="top">Text Label</td><td colspan="1" valign="top"><p>Validatio n Error</p><p>Message</p></td><td colspan="1" valign="top"><p>رسالة خطأ</p><p>التحقق</p></td><td colspan="1" valign="top">Conditional</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top"><p>Displays specific</p><p>validation</p></td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">View</td><td colspan="1" valign="top"><p>Red text, clear</p><p>indicatio</p></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">errors next</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">n of</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"><p>to relevant</p><p>fields.</p></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">error.</td></tr>
<tr><td colspan="1" valign="top">ELM- EDIT</td><td colspan="1" valign="top">Text Label</td><td colspan="1" valign="top">Success Message</td><td colspan="1" valign="top">رسالة النجاح</td><td colspan="1" valign="top">Conditional</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Displays confirmatio</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">View</td><td colspan="1" valign="top">Green text,</td></tr>
<tr><td colspan="1" valign="top">-015</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">n of</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">promine</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">successful</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">nt</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">user</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">display.</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">update.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"><p>ELM- EDIT</p><p>-016</p></td><td colspan="1" valign="top">Text Label</td><td colspan="1" valign="top">Generic Error Message</td><td colspan="1" valign="top">رسالة خطأ عامة</td><td colspan="1" valign="top">Conditional</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top"><p>Displays unexpected</p><p>system errors.</p></td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">View</td><td colspan="1" valign="top"><p>Red text, promine</p><p>nt display.</p></td></tr>
<tr><td colspan="1" valign="top"><p>ELM- EDIT</p><p>-017</p></td><td colspan="1" valign="top">Confirmation Dialog</td><td colspan="1" valign="top"><p>Role Replace</p><p>ment</p></td><td colspan="1" valign="top"><p>تأكيد استبدال</p><p>الدور</p></td><td colspan="1" valign="top">Conditional</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top"><p>Prompts System</p><p>Admin to</p></td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top"><p>View, Click (Replace/C</p><p>ancel)</p></td><td colspan="1" valign="top"><p>Clear message,</p><p>accessibl</p></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Confirm</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">confirm</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">e</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">ation</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">replacing</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">buttons.</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">an existing</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">user with a</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"><p>single-</p><p>holder role.</p></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"><p>ELM- EDIT</p><p>-018</p></td><td colspan="1" valign="top">Button</td><td colspan="1" valign="top">Replace Button (in Dialog)</td><td colspan="1" valign="top">زر استبدال في ( )النافذة</td><td colspan="1" valign="top">Mandatory</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Confirms replacemen t of existing user and</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Click</td><td colspan="1" valign="top">Clearly labeled, primary action in</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">proceeds</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">dialog.</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">with</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">current</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">user's role</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">update.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"><p>ELM- EDIT</p><p>-019</p></td><td colspan="1" valign="top">Button</td><td colspan="1" valign="top"><p>Cancel Button (in</p><p>Dialog)</p></td><td colspan="1" valign="top"><p>زر إلغاء في (</p><p>)النافذة</p></td><td colspan="1" valign="top">Mandatory</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top"><p>Cancels the replacemen t and</p><p>returns to</p></td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Click</td><td colspan="1" valign="top">Clearly labeled.</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">the editing</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">form.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
</table>

Summary Section

This rewritten user story for "Edit Existing System User" now fully incorporates the updated conditional logic for role selection, allowing multi-select specifically for `(Fund Manager AND Board Member)` or `(Associate

Fund Manager AND Board Member)`. This ensures precise control over role combinations during editing, mirroring the logic from the "Add New System User" story. The complex role replacement workflow also remains fully detailed.

\*\*Key Implementation Considerations:\*\*

* \*\*Critical:\*\* Implement the precise logic for enabling/disabling multi-select based on the new, more complex role combination condition.
* Implement the precise logic to identify "single-holder" roles and check for existing \*active\* users (excluding the user currently being edited) holding \*only\* that role.
* Ensure the replacement confirmation dialog is clear and handles both "Replace" and "Cancel" actions correctly, with "Cancel" returning to the form.

  \*\*Dependencies and Risks:\*\*

* \*\*Dependency:\*\* This story depends on the "View System Users List" or "View System User Details" stories for navigation to the edit form. It also relies on the data structure defined in "Add New System User." It has a dependency on the "Deactivate System User" functionality for the role replacement workflow.
* \*\*Risk:\*\* Increased complexity of role selection UI/logic could lead to bugs or user confusion. Mitigation: Thorough testing of dynamic UI and validation.
* \*\*Risk:\*\* Accidental deactivation of a user if the replacement logic is flawed or misunderstood by the admin. Mitigation: Clear dialog, thorough testing.

  ---

  I have now rewritten the "Edit Existing System User" user story with the updated conditional logic for role selection.

  \*\*Important Note:\*\* I am still awaiting your clarification on the "Action Not Applicable (Role/Assignment)" logic for the "Resend Account Registration Message" user story from our previous interaction. The current logic in that story (and reflected in its Acceptance Criteria) is: "Action is NOT applicable if the user has ONLY the 'Board Member' role AND is NOT assigned to any fund board." However, with `IsAssignedToFundBoard` removed from the User entity, this specific condition cannot be implemented as written. \*\*Please confirm how the "Resend Message" eligibility for Board Members should be determined without

  `IsAssignedToFundBoard`.\*\*

  Would you like me to generate the user stories for "Activate System User" or "Reset User Password" next, or would you prefer to refine this one further, or select another from the original list?

# User Story JDWA-1225: Resend Account Registration Message

Introduction Section

This user story describes the functionality for System Administrators to resend the account registration WhatsApp message to a specific user. This action is conditionally available only for active users whose initial registration message was eligible to be sent (flag = 1) but whose overall registration process is not yet completed (flag = 0). This helps ensure users receive their login credentials if the initial message was missed or failed.

Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|Resend Account Registration Message|<p>Allows System Administrators to resend the registration</p><p>WhatsApp message to a user.</p>|
|\*\*User Story\*\*|<p>As a \*\*System Administrator\*\*, I want to \*\*resend the account registration message to an eligible user\*\* so that I can \*\*ensure</p><p>they receive their login credentials and complete their registration.\*\*</p>||
|\*\*Story Points\*\*|5|<p>Involves user selection, conditional logic,</p><p>external API call, and status update.</p>|
|\*\*User Roles\*\*|System Admin|<p>Only System</p><p>Administrators can perform this action.</p>|
|\*\*Access Requirements\*\*|Authenticated and authorized System Admin access. User must be selected from the user list or details page.|User must be logged in as a System Admin.|
|\*\*Trigger\*\*|System Admin clicks "Resend Message" button/link for a specific user from the user list or user details page.|User initiates the action.|
|\*\*Frequency of Use\*\*|Low to Medium|<p>Used when initial message fails or user</p><p>requests it.</p>|
|\*\*Pre-condition\*\*|<p>System Admin is logged in. The target user account exists and has a valid Saudi mobile number. \*\*The user must be 'Active', AND their</p><p>'Registration Is Completed' flag must be 0, AND their 'Registration Message Is Sent' flag must be 1.\*\*</p>|<p>The system is operational, and the</p><p>user meets all specific eligibility criteria.</p>|
|\*\*Business Rules\*\*|<p>1\. The "Resend Message" action is \*\*ONLY applicable\*\* if the user is \*\*'Active' AND their 'Registration Is Completed' flag is 0 AND their 'Registration Message Is Sent' flag is 1.\*\*<br>2. The user must have a valid Saudi mobile number registered.<br>3. The system will resend the same registration message content, including the temporary password and login link (content from MSG-ADD- 008).<br>4. The 'Registration Message Is Sent' flag for the user should be updated to 1 upon successful resend (if it was 0, but it</p><p>should already be 1 for eligibility).<br>5. A confirmation dialog should be presented before resending the message.</p>|Ensures strict conditional availability, proper message content, and status tracking.|
|\*\*Post- condition\*\*|The WhatsApp message is resent to the user, a success/failure message is displayed to the System Admin, and the 'Registration Message Is Sent' flag is updated if applicable.|<p>The system state reflects the attempt and</p><p>outcome of resending the message.</p>|
|\*\*Risk\*\*|1\. WhatsApp message delivery failure.<br>2. Resending message to an incorrect user.<br>3. Performance issues with WhatsApp API.|<p>Mitigation: Confirmation dialog, robust mobile number</p><p>validation, API monitoring.</p>|

|\*\*Assumptions\*\*|1\. WhatsApp Business API integration is available and configured.<br>2. The system can retrieve the user's temporary password for the message content.|<p>These assumptions are crucial for</p><p>implementing the functionality.</p>|
| :- | :- | :- |
|\*\*UX/UI Design Link\*\*|N/A (Refer to general action button/confirmation dialog patterns)|<p>No specific mockups</p><p>provided in the BRD for this section.</p>|

Process Flow Table

|**Step**|**Action Description**|**Actor**|<p>**Related**</p><p>**Message Codes**</p>|**Notes**|
| :- | :- | :- | :- | :- |
|1|<p>System Admin is viewing the</p><p>"View System Users List" or "View User Details" page.</p>|System Admin|||
|2|System Admin identifies a user and clicks the "Resend Message" button/link for that user.|System Admin||<p>This button/link is only visible/enabled if the user meets \*\*all\*\* eligibility criteria</p><p>(Active, Registration Is Completed = 0, Registration Message Is Sent = 1).</p>|
|3|<p>System displays a confirmation</p><p>dialog asking the System Admin to confirm resending the message.</p>|System|MSG- RESEND-001|Includes the user's name in the message.|
|4|<p>System Admin reviews the</p><p>confirmation message and clicks "\*\*Resend\*\*" or "Cancel".</p>|System Admin||If "Cancel" is clicked, the process stops.|
|5|<p>System retrieves the user's mobile</p><p>number, temporary password, and login URL.</p>|System|||
|6|<p>System attempts to send the WhatsApp registration message to</p><p>the user (content from MSG-ADD- 008).</p>|System|MSG- RESEND-002, MSG-ADD-008||
|7|<p>System displays a success or failure message to the System Admin</p><p>based on the WhatsApp message sending attempt.</p>|System|MSG- RESEND-002|Note: Using MSG-RESEND-002 for both success/failure, message text will differentiate.|
|8|System Admin can continue managing users.|System Admin|||

Alternative Flow Table

|**Alternative Scenario**|**Condition**|**Action**|<p>**Related**</p><p>**Message Codes**</p>|**Resolution**|
| :- | :- | :- | :- | :- |
|\*\*Action Not Applicable (Eligibility Criteria Not Met)\*\*|<p>System Admin attempts to click "Resend Message" for a user who is NOT 'Active', OR whose 'Registration Is Completed' flag is 1, OR whose</p><p>'Registration Message Is Sent' flag is 0.</p>|<p>The "Resend Message" button/link is</p><p>\*\*hidden\*\*.</p>||System Admin cannot perform the action.|
|\*\*Confirmation Canceled\*\*|<p>System Admin clicks "Resend Message" but then clicks</p><p>"Cancel" in the confirmation dialog.</p>|<p>System closes the confirmation dialog</p><p>and no message is sent.</p>||No action performed.|
|\*\*WhatsApp API Error\*\*|An error occurs during the WhatsApp API call (e.g., API down, rate limit exceeded).|System displays a failure message to the System Admin and logs the error.|MSG- RESEND- 002|<p>System Admin can retry later. Note: Using MSG- RESEND-002 for both</p><p>success/failure, message text will differentiate.</p>|

Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|<p>\*\*Resend Message</p><p>- Success (Eligible User)\*\*</p>|A System Admin is logged in. A user is 'Active', their 'Registration Is Completed' flag is 0, and their 'Registration Message Is Sent' flag is 1.|The System Admin clicks "Resend Message" for this user and clicks "Resend" in the confirmation dialog.|<p>The WhatsApp registration message is successfully sent to the user, the 'Registration Message Is Sent' flag for the user remains 1 (as it was already 1), and a success message "Account</p><p>registration message sent successfully." is displayed.</p>|
|<p>\*\*Resend Message</p><p>- Not Applicable (Inactive User)\*\*</p>|A System Admin is logged in. A user is 'Inactive'.|<p>The System Admin attempts to click</p><p>"Resend Message" for this user.</p>|<p>The "Resend Message" button/link is</p><p>\*\*hidden\*\*, and the action cannot be performed.</p>|
|<p>\*\*Resend Message</p><p>- Not Applicable (Registration Completed = 1)\*\*</p>|<p>A System Admin is logged in. A user is 'Active', their 'Registration Is Completed' flag is 1, and their</p><p>'Registration Message Is Sent' flag is 1.</p>|The System Admin attempts to click "Resend Message" for this user.|<p>The "Resend Message" button/link is</p><p>\*\*hidden\*\*, and the action cannot be performed.</p>|
|<p>\*\*Resend Message</p><p>- Not Applicable (Registration Message Is Sent = 0)\*\*</p>|<p>A System Admin is logged in. A user is 'Active', their 'Registration Is Completed' flag is 0, and their 'Registration Message Is</p><p>Sent' flag is 0 (e.g., Board Member only).</p>|The System Admin attempts to click "Resend Message" for this user.|<p>The "Resend Message" button/link is</p><p>\*\*hidden\*\*, and the action cannot be performed.</p>|
|<p>\*\*Resend Message</p><p>- Failure (WhatsApp API Error)\*\*</p>|A System Admin is logged in. A user is eligible for the message, but the WhatsApp API fails to send it.|<p>The System Admin clicks "Resend Message" for this user and clicks "Resend" in</p><p>the confirmation dialog.</p>|A failure message (e.g., "Failed to send account registration message. Please try again.") is displayed, and the 'Registration Message Is Sent' flag remains unchanged (at 1).|
|<p>\*\*Resend Message</p><p>- Confirmation Canceled\*\*</p>|A System Admin clicks "Resend Message" for a user.|<p>The System Admin clicks "Cancel" in the</p><p>confirmation dialog.</p>|The confirmation dialog closes, and no message is sent.|

Data Entities Table

Entity Name: User (for resending message)

|**Attribute (English)**|**Attrib ute (Arabi c)**|**Mandatory/O ptional**|**Attribute Type**|**Data Lengt h**|**Integrati on Require ments**|**Defau lt Value**|<p>**Condit ion (if needed**</p><p>**)**</p>|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|User ID|معرف المستخدم|Mandatory|Number|N/A|<p>Database</p><p>Primary Key</p>|N/A|N/A|Unique identifier|12345|12345|
|Mobile|رقم الجوال|Mandatory|Text|Max 10|Database Field|N/A|N/A|<p>Saudi mobile format (e.g., 05XXXXX XXX),</p><p>Numeric only,</p><p>Unique (as username).</p>|<p>0501234</p><p>567</p>|<p>0501234</p><p>567</p>|
|Password|كلمة المرور|Mandatory|Text|N/A|Database Field|N/A|N/A|<p>System- generated,</p><p>stored securely</p>|N/A|N/A|

|||||||||<p>(hashing</p><p>handled internally).</p>|||
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|Role|الدور|Mandatory|Relation to Role Entity|N/A|Database Many-to- Many|N/A|<p>At least one role must be selecte d, conditi onal</p><p>logic applies</p>|Predefined roles only|<p>مدير</p><p>صندوق، عضو مجلس</p>|Fund Manage r, Board Member|
|Registrati on Message Is Sent|تم إرسال رسالة التسجيل|Mandatory|Boolean|N/A|Database Field|<p>0</p><p>(False</p><p>)</p>|N/A|Set to 1 if user's role(s) make them eligible for WhatsApp message (NOT only 'Board Member'); 0 if only 'Board Member'.|1|True|
|\*\*Registr ation Is Complete d\*\*|<p>تم \*\* اكتمال التسجيل</p><p>\*\*</p>|<p>\*\*Mandatory\*</p><p>\*</p>|\*\*Boolean\*\*|\*\*N/ A\*\*|\*\*Databas e Field\*\*|<p>\*\*0 (False</p><p>)\*\*</p>|<p>\*\*N/A</p><p>\*\*</p>|<p>\*\*Set to 0 upon user creation. Updated by a separate process (e.g., user's</p><p>first login).\*\*</p>|0|False|
|Status|الحالة|Mandatory|Boolean/Dro pdown|N/A|Database Field|Active|N/A|Active/Inacti ve|نشط|Active|

Messages/Notifications Table

|<p>**Message**</p><p>**Code**</p>|**Message (English)**|**Message (Arabic)**|**Message Type**|<p>**Communication**</p><p>**Method**</p>|
| :- | :- | :- | :- | :- |
|<p>MSG- RESEND-</p><p>001</p>|<p>Are you sure you want to resend the account registration message to</p><p>[User Name]?</p>|هل أنت متأكد من إعادة إرسال اسم [رسالة تسجيل الحساب إلى ؟]المستخدم|Confirmation Dialog|In-App|
|MSG- RESEND- 002|<p>Account registration message sent successfully. / Failed to send</p><p>account registration message. Please try again.</p>|<p>تم إرسال رسالة تسجيل الحساب فشل إرسال رسالة تسجيل / .بنجاح يرجى المحاولة مرة .الحساب</p><p>.أخرى</p>|Success/Error Message|In-App|
|MSG- ADD-008|<p>Your registration for Jadwa Fund Board Management is complete. Your temporary password is:</p><p>[Password]. Please log in through this link [login URL].</p>|<p>تم تسجيلك في تطبيق إدارة مجالس كلمة</p><p>المرور .صناديق جدوى يرجى ].كلمة المرور[ :المؤقتة هي تسجيل الدخول من خϼل هذا الرابط</p><p>].رابط تسجيل الدخول[</p>|Success Message|WhatsApp|

Screen Elements Table

|**Elemen**|**Element**|**Element**|**Elemen**|**Required/Optio**|**Validati**|**Business**|**Relate**|**User**|**Accessibilit**|
| :- | :- | :- | :-: | :- | :-: | :- | :- | :- | :- |
|**t ID**|**Type**|**Name**|**t Name**|**nal**|**on Rules**|**Logic**|**d**|**Interaction**|**y Notes**|
|||**(English)**|<p>**(Arabic**</p><p>**)**</p>||||**Data Entity**|||

|ELM- RESEN D-001|Button/Lin k|Resend Message Button|<p>رابط /زر إعادة</p><p>إرسال الرسالة</p>|Conditional|N/A|<p>Initiates the</p><p>resend process.</p>|N/A|Click|<p>Clear label, visible/enab</p><p>led based on conditions.</p>|
| :- | :- | :- | -: | :- | :- | :- | :- | :- | :- |
|ELM- RESEN D-002|Confirmati on Dialog|Resend Confirmati on Dialog|نافذة تأكيد إعادة اϹرسال|Mandatory|N/A|<p>Prompts System Admin to confirm</p><p>resend action.</p>|N/A|View, Click (Confirm/Can cel)|Clear message, accessible buttons.|
|ELM- RESEN D-003|Text Label|Confirmati on Message Text|نص رسالة التأكيد|Mandatory|N/A|<p>Displays "Are you sure you want to resend the account registrati on message to [User</p><p>Name]?"</p>|N/A|View|Dynamic content for user name.|
|ELM- RESEN D-004|Button|Resend Button (in Dialog)|<p>زر إعادة اϹرسال</p><p>في ( )النافذة</p>|Mandatory|N/A|<p>Confirms the</p><p>resend action.</p>|N/A|Click|Clearly labeled.|
|ELM- RESEN D-005|Button|Cancel Resend Button|زر إلغاء إعادة اϹرسال|Mandatory|N/A|<p>Cancels the</p><p>resend action.</p>|N/A|Click|Clearly labeled.|
|ELM- RESEN D-006|Text Label|Success/Er ror Message Display|<p>عرض رسالة</p><p>الخ/النجاح طأ</p>|Conditional|N/A|<p>Displays the outcome of the</p><p>resend attempt.</p>|N/A|View|Prominent display, clear text.|

Summary Section

This user story for "Resend Account Registration Message" has been updated with the precise eligibility criteria: the action is \*\*only applicable for active users where 'Registration Is Completed' is 0 AND 'Registration Message Is Sent' is 1.\*\* This refined logic is now reflected in the Business Rules, Pre-conditions, Alternative Flows, and Acceptance Criteria, ensuring strict control over when this action is available.

\*\*Key Implementation Considerations:\*\*

* \*\*Critical:\*\* Implement the precise logic for \*\*hiding\*\* the "Resend Message" button/link based on the combined conditions: `(User.Status = 'Inactive' OR User.RegistrationIsCompleted = 1 OR User.RegistrationMessageIsSent = 0)`.
* Ensure the single `MSG-RESEND-002` can dynamically display both success and failure messages based on the WhatsApp API response.
* Handle WhatsApp API failures gracefully, logging errors and informing the System Admin.

\*\*Dependencies and Risks:\*\*

* \*\*Dependency:\*\* This story depends on the "View System Users List" or "View User Details" stories for initiating the action. It also depends on the "Add New System User" story for initial user creation and password generation, and the correct setting of `Registration Is Completed` and `Registration Message Is Sent` flags.

* \*\*Risk:\*\* Misinterpretation of the complex eligibility conditions leading to incorrect button visibility. Mitigation: Thorough testing of the combined conditional logic.
* \*\*Risk:\*\* WhatsApp message not reaching the user due to external factors. Mitigation: Provide System Admin with clear feedback and alternative notification options.

  ---

  I have now updated the "Resend Account Registration Message" user story with the precise eligibility criteria.

  Would you like me to generate the user stories for "Activate System User" or "Delete System User" next, or would you prefer to refine this one further, or select another from the original list?

# User Story JDWA-1223: Add New System User

Introduction Section

This user story details the functionality enabling System Administrators to create new user accounts within the Fund Board Management Application. This process involves capturing essential user information, where the Saudi Mobile Number serves the account username, assigning appropriate roles with specific conditional logic (affecting multi-select behavior), setting the initial active status, generating a default password, and conditionally notifying the user via WhatsApp. A new validation is introduced for specific roles (Legal Counsel, Finance Controller, Compliance and Legal Managing Director, Head of Real Estate) to check for existing active users with the same selected role, prompting for a replacement if found. If the replacement is cancelled, the admin can proceed with creating the new user by selecting a non-conflicting role. A flag tracks WhatsApp message eligibility, and another tracks overall registration completion. The Screen Elements Table provides a detailed breakdown of the UI components on the user creation form.

Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|Add New System User|Enables System Administrators to create new user accounts.|
|\*\*User Story\*\*|<p>As a \*\*System Administrator\*\*, I need to \*\*add new user accounts with specific role assignments, handling potential role replacements, and track registration</p><p>status\*\* so that I can \*\*onboard new personnel and grant them appropriate system access.\*\*</p>||
|\*\*Story Points\*\*|13|<p>This involves complex data entry, dynamic UI for role selection, unique role validation with replacement workflow, password generation, conditional external notification, and</p><p>status tracking.</p>|
|\*\*User Roles\*\*|System Admin|Only System Administrators can add new users.|
|\*\*Access Requirements\*\*|Authenticated and authorized System Admin access to the User Management module.|User must be logged in as a System Admin.|
|\*\*Trigger\*\*|System Admin clicks "Add New User" button from the user list page.|User initiates the action from the system's administrative interface.|
|\*\*Frequency of Use\*\*|Medium|<p>New users are added periodically as</p><p>personnel changes or new funds are introduced.</p>|
|\*\*Pre-condition\*\*|System Admin is logged in and has access to the User Management module.|The system is operational, and the admin has the necessary permissions.|
|\*\*Business Rules\*\*|<p>1\. Each user must have a unique email address.<br>2. All mandatory fields must be filled.<br>3. At least one role must be assigned to a new user.<br>4. CV and Passport No. are optional fields.<br>5. New users default to 'Active' status.<br>\*\*6. Role Selection Logic:\*\* Multi-select for roles is enabled \*\*ONLY IF\*\* the selected roles are ('Fund Manager' AND 'Board Member') OR ('Associate Fund Manager' AND 'Board Member'). Otherwise, multi-select is disabled, restricting to a single role.<br>7. System automatically generates a default password for new users.<br>8. Country Code and Mobile No. are mandatory and restricted to Saudi mobile numbers only (starting with 05, 9 digits after 05).<br>9. The Saudi Mobile No. will serve as the user's account</p><p>username.<br>10. A WhatsApp message is sent to the user upon successful registration confirmation,</p>|Ensures data integrity, proper access control, dynamic UI behavior, and user notification eligibility/completion tracking during creation, with a critical role replacement workflow.|

||<p>EXCEPT if the added user has ONLY the 'Board Member' role.<br>11. The 'Registration Message Is Sent' flag is set to 1 if the user's assigned role(s) make them eligible for a WhatsApp message (i.e., NOT only 'Board Member'). Otherwise, it's set to 0 (for users with only 'Board Member' role). This flag reflects eligibility for sending, not delivery success.<br>12. The 'Registration Is Completed' flag is set to 0 (False) by default upon user creation.<br>13. Unique Role Validation with Replacement: If a selected role is one of: 'Legal Counsel', 'Finance Controller', 'Compliance and Legal Managing Director', 'Head of Real Estate', the system must check for another \*active\* user (who is \*not\* the user currently being edited) currently holding</p><p>\*only\* that specific role. If found, a confirmation dialog is displayed to replace the existing user. If 'Replace' is chosen, the existing user is deactivated before the new user is added. If 'Cancel' is chosen, the</p><p>System Admin is returned to the form to modify role selection or other details.</p>||
| :- | :- | :- |
|<p>\*\*Post-</p><p>condition\*\*</p>|<p>A new user account is successfully created with an</p><p>active status, default password, assigned roles, the user appears in the system's user list, a conditional WhatsApp confirmation is sent, and flags are set. If a replacement occurred, the previous user with that specific role is deactivated.</p>|<p>The system state reflects the</p><p>successful creation and/or replacement of a user.</p>|
|\*\*Risk\*\*|<p>1\. Data entry errors leading to incorrect user information.<br>2. Incorrect role assignment leading to unauthorized access.<br>3. WhatsApp message delivery failure.<br>4. Password security</p><p>concerns.<br>5. Incorrect mobile number validation.<br>6. Complex UI logic for role selection.</p>|<p>Mitigation: Robust input validation, clear role descriptions, WhatsApp API monitoring, secure password generation and initial delivery, clear</p><p>mobile number format guidance, thorough testing of dynamic UI.</p>|
|\*\*Assumptions\*\*|<p>1\. Core Identity in ASP.NET is used for user and permission management.<br>2. System Admin has full understanding of role implications.<br>3. WhatsApp Business API integration is available and configured.<br>4. Default password policy is defined (e.g., length, complexity).<br>5. The system handles password hashing internally.<br>6. The 'Registration Is Completed' flag will be updated by a separate process (e.g., user's first login).<br>7. The roles 'Legal Counsel', 'Finance Controller', 'Compliance and</p><p>Legal Managing Director', 'Head of Real Estate' are considered "single-holder" roles for active users.</p>|These assumptions simplify the initial implementation scope.|
|\*\*UX/UI Design Link\*\*|N/A (General form design principles, dynamic multi- select UI, confirmation dialog for replacement)|No specific mockups provided in the BRD for this section.|

Process Flow Table

|**Step**|**Action Description**|**Actor**|<p>**Related**</p><p>**Message Codes**</p>|**Notes**|
| :- | :- | :- | :- | :- |
|1|<p>System Admin navigates to the "User Management" section and clicks the "Add</p><p>New User" button.</p>|System Admin|||
|2|System displays a form for new user data entry.|System||<p>All fields are initially empty. Role</p><p>selection defaults to single-select mode.</p>|

|3|System Admin enters required and optional user details: Name, Email, Country Code, Mobile No., IBAN, Nationality, CV (upload), Passport No.|System Admin|<p>MSG-ADD- 001, MSG- ADD-002, MSG-ADD-</p><p>003, MSG- ADD-010</p>|Country Code and Mobile No. are mandatory and validated as Saudi numbers.|
| :- | :- | :- | :- | - |
|4|System Admin selects one or more roles for the new user from a predefined list of available roles.|System Admin||<p>\*\*Conditional logic:\*\* Multi-select for roles is enabled \*\*ONLY IF\*\* the selected roles are ('Fund Manager' AND 'Board Member') OR ('Associate Fund Manager' AND 'Board Member').</p><p>Otherwise, multi-select is disabled, restricting to a single role.</p>|
|5|<p>System Admin reviews the entered</p><p>information and clicks the "\*\*Create User\*\*" button.</p>|System Admin|||
|6|System performs server-side validation on all submitted data.|System|<p>MSG-ADD- 001, MSG- ADD-002, MSG-ADD- 003, MSG- ADD-010, MSG-ADD-</p><p>011</p>|Checks for mandatory fields, email format, uniqueness, role selection based on business rules, and Saudi mobile number format/uniqueness.|
|7|<p>\*\*If validation is successful, System</p><p>checks if any selected role is a "single- holder" role (Legal Counsel, Finance Controller, Compliance and Legal</p><p>Managing Director, Head of Real Estate).\*\*</p>|System|||
|8|<p>\*\*If a single-holder role is selected AND an active user already holds ONLY that specific role:\*\* System displays a</p><p>confirmation dialog asking to replace the existing user.</p>|System|MSG-ADD- 012|Dialog includes existing user's name and role. Buttons: "Replace", "Cancel".|
|9|<p>\*\*If System Admin clicks "Replace" in</p><p>the dialog:\*\* System deactivates the existing user with that role.</p>|<p>System</p><p>Admin / System</p>||The existing user's status is changed to 'Inactive'.|
|10|<p>\*\*If System Admin clicks "Cancel" in the dialog:\*\* System returns the System Admin to the user creation form, allowing</p><p>them to modify role selection or other details.</p>|System Admin / System||This allows the admin to choose a non- conflicting role or adjust other inputs.|
|11|<p>\*\*If System Admin proceeds (either no conflict, or after cancelling replacement and modifying input):\*\* System proceeds to create the new user account in the</p><p>database with \*\*active status\*\*.</p>|System||This step is reached if no conflict, or if conflict was cancelled and admin re- submits successfully.|
|12|System \*\*generates a user default password\*\* based on predefined policy.|System||This password will be used for the user's first login.|
|13|System stores the generated password securely (hashing handled internally).|System|||
|14|<p>\*\*System sets the 'Registration Message Is Sent' flag:\*\*<br> - To \*\*1\*\* if the user's assigned role(s) make them eligible for a WhatsApp message (i.e., NOT only 'Board Member').<br> - To \*\*0\*\* if the user's assigned role(s) make them</p><p>ineligible for a WhatsApp message (i.e., ONLY 'Board Member').</p>|System||This flag tracks the user's eligibility for the WhatsApp notification based on their role(s).|

|15|\*\*System sets the 'Registration Is Completed' flag to 0 (False).\*\*|System||This flag defaults to 0 upon creation.|
| :- | :- | :- | :- | :- |
|16|<p>\*\*Conditional:\*\* If the added user has</p><p>\*\*NOT\*\* only the 'Board Member' role, System \*\*attempts to send a WhatsApp message to the user to confirm their</p><p>registration\*\*, including initial login instructions and a link to the login page.</p>|System|MSG-ADD- 008, MSG- ADD-009|Requires WhatsApp Business API integration. The outcome of this step does NOT affect the 'Registration Message Is Sent' flag.|
|17|System displays a success message.|System|MSG-ADD- 005|This flag defaults to 0 upon creation.|
|18|<p>System redirects the System Admin back</p><p>to the "View System Users List" page, with the newly added user visible.</p>|System|||

Alternative Flow Table

|**Alternative Scenario**|**Condition**|**Action**|<p>**Related Message**</p><p>**Codes**</p>|**Resolution**|
| :- | :- | :- | :- | :- |
|\*\*Missing Mandatory Field\*\*|<p>System Admin attempts to save without filling a mandatory field (e.g.,</p><p>Name, Email, Country Code, Mobile No.).</p>|System displays a specific validation error message next to the missing field.|MSG- ADD-001|System Admin must fill in all mandatory fields.|
|\*\*Invalid Email Format\*\*|<p>System Admin enters an</p><p>email address in an incorrect format.</p>|<p>System displays a</p><p>validation error message for the email field.</p>|MSG- ADD-002|System Admin must correct the email format.|
|\*\*Duplicate Email\*\*|<p>System Admin enters an</p><p>email address that already exists in the system.</p>|<p>System displays an error</p><p>message indicating duplicate email.</p>|MSG- ADD-003|System Admin must enter a unique email address.|
|\*\*Invalid Mobile No. Format (Non- Saudi)\*\*|<p>System Admin enters a mobile number that does not conform to Saudi mobile number format (e.g., not</p><p>starting with 05, incorrect length).</p>|System displays a validation error message for the mobile number field.|MSG- ADD-010|System Admin must enter a valid Saudi mobile number.|
|\*\*Duplicate Mobile No. (Username)\*\*|<p>System Admin enters a mobile number that already exists as a username in the</p><p>system.</p>|<p>System displays an error message indicating the mobile number is already</p><p>in use.</p>|MSG- ADD-011|System Admin must enter a unique mobile number.|
|<p>\*\*Invalid Role</p><p>Selection (Logic Violation)\*\*</p>|<p>System Admin selects roles</p><p>that violate the conditional logic (e.g., 'Fund Manager' and 'Accountant' simultaneously, or more</p><p>than one role when multi- select is disabled).</p>|<p>System displays a</p><p>warning/error message (e.g., "Required Field." as MSG-ADD-001).</p>|<p>MSG-</p><p>ADD-001</p>|<p>System Admin must correct</p><p>the role selection.</p>|
|\*\*No Roles Selected\*\*|System Admin attempts to save a new user without assigning any roles.|<p>System displays a warning/error message prompting to select at</p><p>least one role.</p>|MSG- ADD-001|System Admin must select at least one role for the user.|
|\*\*CV File Upload Error\*\*|<p>An error occurs during the</p><p>CV file upload (e.g., file too large, unsupported format).</p>|<p>System displays an error</p><p>message related to the file upload.</p>|MSG- ADD-006|System Admin must re- upload a valid CV file.|
|\*\*Role Replacement - Cancelled\*\*|<p>System Admin selects a single-holder role that has an active user, and clicks</p><p>"Cancel" in the replacement confirmation dialog.</p>|System closes the dialog and returns the System Admin to the user creation form.||System Admin can modify input and re-attempt creation.|

|\*\*WhatsApp Message Failure (Non-Board Member Only)\*\*|The system fails to send the WhatsApp confirmation message for a user who is NOT only a Board Member.|<p>System logs the failure and displays a warning to the System Admin.</p><p>\*\*The 'Registration Message Is Sent' flag</p><p>remains set based on role eligibility (Step 14).\*\*</p>|MSG- ADD-009|System Admin may need to manually notify the user or verify mobile number.|
| :- | :- | :- | :- | - |
|\*\*System Error during Creation\*\*|A backend error occurs during user creation (e.g., database connection issue).|System displays a generic error message.|MSG- ADD-007|<p>System Admin can retry the operation or contact support. \*\*The 'Registration Message Is Sent' flag is set to 0\*\* (as user creation itself failed),</p><p>\*\*and 'Registration Is Completed' flag remains 0.\*\*</p>|
|<p>\*\*Cancel</p><p>Creation\*\*</p>|<p>System Admin decides not</p><p>to create the user.</p>|<p>System Admin clicks a</p><p>"Cancel" button.</p>||<p>System discards all entered</p><p>data and redirects back to</p><p>the "View System Users List".</p>|

Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Successful User Creation (Non- Board Member Only) - WhatsApp Attempted\*\*|A System Admin is logged in and on the "Add New User" form.|The System Admin fills all mandatory fields (including valid Saudi Mobile No.), selects valid roles (e.g., only "Fund Manager"), and clicks "Create User".|<p>The new user account is created with active status, the Mobile No. is set as username, a default password is generated and stored, a success message "Record Saved Successfully" is displayed, a WhatsApp confirmation message is attempted to be sent to the user including the login link, \*\*the 'Registration Message Is Sent' flag is set to 1\*\*, \*\*the 'Registration Is Completed' flag is set to 0\*\*, and the</p><p>System Admin is redirected to the user list.</p>|
|\*\*Successful User Creation (Board Member Only) - No WhatsApp Attempt\*\*|A System Admin is logged in and on the "Add New User" form.|The System Admin fills all mandatory fields (including valid Saudi Mobile No.), selects \*\*only\*\* the "Board Member" role, and clicks "Create User".|<p>The new user account is created with active status, the Mobile No. is set as username, a default password is generated and stored, a success message "Record Saved Successfully" is displayed, \*\*NO WhatsApp confirmation message is attempted to be sent\*\*, \*\*the 'Registration Message Is Sent' flag is set to 0\*\* (as per role eligibility), \*\*the 'Registration Is Completed' flag is set to 0\*\*, and the</p><p>System Admin is redirected to the user list.</p>|
|\*\*Successful User Creation (Single- Holder Role, With Replacement)\*\*|<p>A System Admin is logged in and on the "Add New User" form. An active user "John Doe" currently holds ONLY the</p><p>"Legal Counsel" role.</p>|The System Admin fills all mandatory fields, selects "Legal Counsel" for the new user, clicks "Create User", and then clicks "Replace" in the confirmation dialog.|The existing user "John Doe" is deactivated, the new user account is created with active status and "Legal Counsel" role, a success message "Record Saved Successfully" is displayed, and the System Admin is redirected to the user list.|

|<p>\*\*Role Replacement</p><p>- Cancelled (Admin Modifies Input)\*\*</p>|A System Admin is on the "Add New User" form. An active user "John Doe" currently holds ONLY the "Legal Counsel" role.|<p>The System Admin fills all mandatory fields, selects "Legal Counsel" for the new user, clicks "Create User", then clicks "Cancel" in the confirmation dialog, then changes the role to</p><p>"Accountant" and clicks "Create User" again.</p>|The new user account is created with the "Accountant" role, the existing user "John Doe" remains active, a success message "Record Saved Successfully" is displayed, and the System Admin is redirected to the user list.|
| :- | :- | :- | :- |
|\*\*Mandatory Field Validation\*\*|A System Admin is on the "Add New User" form.|The System Admin leaves the "Mobile No." field empty and clicks "Create User".|<p>The system displays a specific validation error message "Required</p><p>Field" for the "Mobile No." field, and the user account is not created.</p>|
|\*\*Invalid Mobile No. Format\*\*|A System Admin is on the "Add New User" form.|The System Admin enters "0123456789" (non-Saudi format) into the Mobile No. field and clicks "Create User".|<p>The system displays a validation error message "Invalid Saudi mobile number format. Please enter a 10-digit number starting with 05." for the mobile number</p><p>field, and the user account is not created.</p>|
|\*\*Duplicate Mobile No. (Username)\*\*|A System Admin is on the "Add New User" form.|<p>The System Admin enters a Saudi mobile number that already exists as a username</p><p>in the system and clicks "Create User".</p>|The system displays an error message "Mobile number is already in use as a username.", and the user account is not created.|
|<p>\*\*Conditional Role Selection - Valid</p><p>(Fund Manager & Board Member)\*\*</p>|A System Admin is on the "Add New User" form.|<p>The System Admin selects "Fund Manager" and "Board</p><p>Member" roles, and clicks "Create User".</p>|The system successfully creates the user with both roles assigned.|
|<p>\*\*Conditional Role Selection - Valid (Associate Fund</p><p>Manager & Board Member)\*\*</p>|A System Admin is on the "Add New User" form.|The System Admin selects "Associate Fund Manager" and "Board Member" roles, and clicks "Create User".|The system successfully creates the user with both roles assigned.|
|<p>\*\*Conditional Role</p><p>Selection - Invalid (Multi-select Disabled)\*\*</p>|<p>A System Admin is</p><p>on the "Add New User" form.</p>|<p>The System Admin selects</p><p>"Legal Counsel" and then attempts to select "Accountant" (when multi- select is disabled).</p>|<p>The system prevents the selection of the</p><p>second role, or displays a generic validation error message "Required Field" if the selection is invalid, and the user account is not created.</p>|
|\*\*WhatsApp Failure (Flag Remains Based on Eligibility)\*\*|A new user is created with the role "Fund Manager", but WhatsApp message sending fails.|The system completes the user creation process.|<p>A warning message "Failed to send WhatsApp confirmation message. Please notify the user manually." is displayed, and the 'Registration Message Is Sent' flag remains set to 1 (because the user was eligible to receive</p><p>it), and the 'Registration Is Completed' flag is set to 0.</p>|
|\*\*System Error during Save (Flags Set to 0)\*\*|A System Admin is on the "Add New User" form and clicks "Create User".|An unexpected backend error occurs during the saving process.|<p>The system displays the message "An error is occurred while saving data" and the user account is not created, and both the 'Registration Message Is Sent' flag</p><p>and 'Registration Is Completed' flag are set to 0.</p>|
|\*\*Cancel User Creation\*\*|<p>A System Admin is</p><p>on the "Add New User" form.</p>|The System Admin clicks the "Cancel" button.|<p>The system discards all entered data and</p><p>redirects the System Admin back to the "View System Users List" page.</p>|

Data Entities Table

Entity Name: User (for adding)

|**Attrib ute (Engli sh)**|**Attri bute (Ara bic)**|**Mandatory/ Optional**|**Attribute Type**|**Dat a Len gth**|**Integra tion Require ments**|**Defau lt Value**|<p>**Condi tion (if neede**</p><p>**d)**</p>|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | - | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|User ID|معرف المستخ دم|Mandatory|Number (Auto- generated)|N/A|<p>Databas e</p><p>Primary Key</p>|N/A|N/A|Unique identifier|12345|12345|
|Name|اϻسم|Mandatory|Text|<p>Ma</p><p>x 255</p>|Databas e Field|N/A|N/A|N/A|سارة علي|Sara Ali|
|Email|البريد اϹلكتر وني|Mandatory|Text|Ma x 255|Databas e Field|N/A|N/A|<p>Unique, Valid</p><p>email format</p>|<sara.ali@jadwa.c> om|<sara.ali@jadwa.c> om|
|Countr y Code|رمز الدولة|Mandatory|Text|Ma x 5|Databas e Field|+966|N/A|<p>Must be '+966' for</p><p>Saudi numbers.</p>|+966|+966|
|<p>Mobil</p><p>e</p>|<p>رقم</p><p>الجوال</p>|Mandatory|Text|<p>Ma</p><p>x 10</p>|<p>Databas</p><p>e Field</p>|N/A|N/A|<p>Saudi</p><p>mobile format (e.g., 05XXXX XXXX),</p><p>Numeric only, Unique (as username)</p><p>.</p>|0501234567|0501234567|
|IBAN|<p>رقم الحس اب المصر</p><p>في الدولي</p>|Optional|Text|Ma x 34|Databas e Field|N/A|N/A|<p>Valid IBAN</p><p>format</p>|SA98765432109 87654321098|SA98765432109 87654321098|
|Nation ality|الجنسي ة|Optional|Text|<p>Ma</p><p>x 100</p>|Databas e Field|N/A|N/A|N/A|مصري|Egyptian|
|CV|السيرة الذاتية|Optional|File Upload|N/A|<p>File Storage (e.g.,</p><p>Azure Blob)</p>|N/A|N/A|<p>PDF, DOCX</p><p>only, Max 10MB</p>|.pdfسارة\_ذاتية\_سيرة|Sara\_CV.pdf|
|Passpo rt No.|<p>رقم</p><p>جواز السفر</p>|Optional|Text|Ma x 20|Databas e Field|N/A|N/A|Alphanum eric|B98765432|B98765432|
|Status|الحالة|Mandatory|Boolean/D ropdown|N/A|Databas e Field|Activ e|N/A|Active/Ina ctive|نشط|Active|
|Role|الدور|Mandatory|Relation to Role Entity|N/A|Databas e Many- to-Many|N/A|<p>At least one role must be select</p><p>ed, condit</p>|Predefine d roles only|مستشار قانوني|Legal Counsel|

||||||||<p>ional logic</p><p>applie s</p>||||
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|Passw ord|كلمة المرور|Mandatory|Text|N/A|Databas e Field|N/A|N/A|<p>System- generated, stored securely (hashing handled internally)</p><p>.</p>|N/A|N/A|
|Last Updat e Date|تاريخ آخر تحديث|Mandatory|DateTime|N/A|Databas e Field|Curre nt Times tamp|N/A|<p>Automatic ally set on creation</p><p>and update.</p>|<p>2023-10-26</p><p>15:00:00</p>|<p>2023-10-26</p><p>15:00:00</p>|
|Regist ration Messa ge Is Sent|تم إرسال رسالة التسجي ل|Mandatory|Boolean|N/A|Databas e Field|<p>0</p><p>(False</p><p>)</p>|N/A|<p>Set to 1 if user's role(s) make them eligible for WhatsApp message (NOT</p><p>only 'Board Member'); 0 if only 'Board Member'. This flag is set based on eligibility and</p><p>attempt to send.</p>|1|True|
|Regist ration Is Compl eted|تم اكتمال التسجي ل|Mandatory|Boolean|N/A|Databas e Field|<p>0</p><p>(False</p><p>)</p>|N/A|<p>Set to 0 upon user creation. Updated by a separate process (e.g.,</p><p>user's first login).</p>|0|False|

Entity Name: Role (for selection)

|<p>**Attribut e (English**</p><p>**)**</p>|**Attribut e (Arabic)**|**Mandatory/Option al**|**Attribut e Type**|**Data Lengt h**|**Integration Requiremen ts**|**Defaul t Value**|**Conditio n (if needed)**|**Rules (if needed)**|<p>**Sampl e in Arabi**</p><p>**c**</p>|<p>**Sampl e in Englis**</p><p>**h**</p>|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|Role ID|معرف الدور|Mandatory|<p>Number (Auto- generated</p><p>)</p>|N/A|Database Primary Key|N/A|N/A|Unique identifie r|1|1|

|Role Name|اسم الدور|Mandatory|Text|Max 50|Database Field|N/A|N/A|Unique|مدير النظام|System Admin|
| :- | -: | :- | :- | :- | :- | :- | :- | :- | -: | :- |

Messages/Notifications Table

|<p>**Message**</p><p>**Code**</p>|**Message (English)**|**Message (Arabic)**|<p>**Message**</p><p>**Type**</p>|<p>**Communication**</p><p>**Method**</p>|
| :- | :- | :- | :- | :- |
|MSG- ADD-001|Required Field.|.حقل إلزامي|Validation Error|In-App|
|MSG- ADD-002|Invalid email format.|صيغة البريد اϹلكتروني غير .صحيحة|Validation Error|In-App|
|MSG- ADD-003|User with this email already exists.|يوجد مستخدم بهذا البريد اϹلكتروني .بالفعل|Validation Error|In-App|
|MSG- ADD-005|Record Saved Successfully.|.تم حفظ البيانات بنجاح|Success Message|In-App|
|MSG- ADD-006|Invalid file format or size for CV. Please upload a PDF or DOCX file up to 10MB.|<p>صيغة الملف أو حجمه غير صالح يرجى تحميل ملف .للسيرة الذاتية 10بحجم أقصى DOCXأو PDF</p><p>.ميجابايت</p>|Validation Error|In-App|
|MSG- ADD-007|An error is occurred while saving data.|لم يتم حفظ ,حدث خطأ بالنظام .البيانات|Error Message|In-App|
|MSG- ADD-008|<p>Your registration for Jadwa Fund Board Management is complete. Your temporary password is:</p><p>[Password]. Please log in through this link [login URL].</p>|<p>تم تسجيلك في تطبيق إدارة مجالس كلمة المرور المؤقتة .صناديق جدوى يرجى تسجيل</p><p>].كلمة المرور[ :هي</p><p>رابط [الدخول من خϼل هذا الرابط ].تسجيل الدخول</p>|Success Message|WhatsApp|
|<p>MSG-</p><p>ADD-009</p>|<p>Failed to send WhatsApp</p><p>confirmation message. Please notify the user manually.</p>|<p>.فشل إرسال رسالة تأكيد الواتساب .يرجى إخطار</p><p>المستخدم يدويا</p>|<p>Warning</p><p>Message</p>|<p>In-App / System</p><p>Log</p>|
|MSG- ADD-010|<p>Invalid Saudi mobile number format. Please enter a 10-digit number</p><p>starting with 05.</p>|<p>صيغة رقم الجوال السعودي غير يرجى إدخال رقم مكون من .صالحة</p><p>05\.أرقام يبدأ بـ 10</p>|Validation Error|In-App|
|MSG- ADD-011|Mobile number is already in use as a username.|رقم الجوال مستخدم بالفعل كاسم .مستخدم|Validation Error|In-App|
|MSG- ADD-012|<p>There is another active user with the role [Role Name]: [Existing User</p><p>Name]. Do you want to replace him?</p>|<p>اسم [يوجد مستخدم نشط آخر بالدور هل ].اسم المستخدم الحالي[ ]:الدور</p><p>تريد استبداله؟</p>|Confirmation Dialog|In-App|

Screen Elements Table

|**Elem ent ID**|**Element Type**|<p>**Element Name (English**</p><p>**)**</p>|<p>**Elem ent Name**</p><p>**(Ara bic)**</p>|**Required/Op tional**|**Validatio n Rules**|**Business Logic**|**Related Data Entity**|**User Interaction**|**Accessib ility Notes**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|<p>ELM- ADD-</p><p>001</p>|Page Title|<p>Add New</p><p>User</p>|<p>إضافة مستخدم</p><p>جديد</p>|N/A|N/A|<p>Displays the title of</p><p>the user</p>|N/A|View|<p>H1</p><p>heading, clear and</p>|
|||||||<p>creation</p><p>form.</p>|||concise.|
|ELM- ADD-|Input Field|Name Input|حقل اϻسم|Mandatory|Text, Max 255 chars|Collects the user's|User.Name|Type|Clear label,|
|002||||||full name.|||placehol|
||||||||||der text.|
|ELM- ADD- 003|Input Field|Email Input|حقل البريد اϹلكترو ني|Mandatory|Valid email format, Max 255|Collects the user's email address.|User.Email|Type|Clear label, placehol der text,|
||||||chars|<p>Must be</p><p>unique.</p>|||<p>email</p><p>type</p>|

<table><tr><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th><th colspan="1" valign="top"><p>keyboard</p><p>.</p></th></tr>
<tr><td colspan="1" valign="top">ELM- ADD- 004</td><td colspan="1" valign="top">Input Field</td><td colspan="1" valign="top">Country Code Input</td><td colspan="1" valign="top">حقل رمز الدولة</td><td colspan="1" valign="top">Mandatory</td><td colspan="1" valign="top">Text, Must be '+966'</td><td colspan="1" valign="top"><p>Collects the country dialing code for Saudi</p><p>mobile number.</p></td><td colspan="1" valign="top">User.Countr yCode</td><td colspan="1" valign="top">Type (or Pre- filled/Drop down)</td><td colspan="1" valign="top">Clear label, placehol der text.</td></tr>
<tr><td colspan="1" valign="top">ELM-</td><td colspan="1" valign="top">Input Field</td><td colspan="1" valign="top">Mobile</td><td colspan="1" rowspan="10" valign="top"><p>حقل رقم الجوال اسم (</p><p>المستخد )م</p></td><td colspan="1" valign="top">Mandatory</td><td colspan="1" valign="top">Numeric</td><td colspan="1" valign="top">Collects</td><td colspan="1" valign="top">User.Mobile</td><td colspan="1" valign="top">Type</td><td colspan="1" valign="top">Clear</td></tr>
<tr><td colspan="1" valign="top">ADD-</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">No.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">only, 10</td><td colspan="1" valign="top">the user's</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">label,</td></tr>
<tr><td colspan="1" valign="top">005</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Input</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">digits,</td><td colspan="1" valign="top">Saudi</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">placehol</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">(Userna</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">starts with</td><td colspan="1" valign="top">mobile</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">der text,</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">me)</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">'05',</td><td colspan="1" valign="top">phone</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">numeric</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Unique.</td><td colspan="1" valign="top">number,</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">keyboard</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">which</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">.</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">serves as</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">their</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">username.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top">ELM- ADD- 006</td><td colspan="1" valign="top">Input Field</td><td colspan="1" valign="top"><p>IBAN</p><p>Input</p></td><td colspan="1" valign="top">حقل رقم الحساب المصر في الدولي</td><td colspan="1" valign="top">Optional</td><td colspan="1" valign="top"><p>Valid IBAN</p><p>format, Max 34 chars</p></td><td colspan="1" valign="top"><p>Collects the user's Internation al Bank</p><p>Account</p></td><td colspan="1" valign="top">User.IBAN</td><td colspan="1" valign="top">Type</td><td colspan="1" valign="top">Clear label, placehol der text.</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Number.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top">ELM- ADD-</td><td colspan="1" valign="top">Input Field</td><td colspan="1" valign="top">Nationali ty Input</td><td colspan="1" valign="top">حقل الجنسية</td><td colspan="1" valign="top">Optional</td><td colspan="1" valign="top">Text, Max 100 chars</td><td colspan="1" valign="top">Collects the user's</td><td colspan="1" valign="top">User.Nation ality</td><td colspan="1" valign="top">Type</td><td colspan="1" valign="top">Clear label,</td></tr>
<tr><td colspan="1" valign="top">007</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">nationality.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">placehol</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">der text.</td></tr>
<tr><td colspan="1" valign="top">ELM- ADD- 008</td><td colspan="1" valign="top">File Upload</td><td colspan="1" valign="top"><p>CV</p><p>Upload</p></td><td colspan="1" valign="top">تحميل السيرة الذاتية</td><td colspan="1" valign="top">Optional</td><td colspan="1" valign="top"><p>PDF/DO</p><p>CX only, Max</p></td><td colspan="1" valign="top">Allows user to upload</td><td colspan="1" valign="top">User.CV</td><td colspan="1" valign="top">Click (to browse/upl oad)</td><td colspan="1" valign="top">Clear label, file</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">10MB</td><td colspan="1" valign="top">their CV</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">type/size</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">file.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">guidance</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">.</td></tr>
<tr><td colspan="1" valign="top"><p>ELM- ADD-</p><p>009</p></td><td colspan="1" valign="top">Input Field</td><td colspan="1" valign="top"><p>Passport No.</p><p>Input</p></td><td colspan="1" valign="top">حقل رقم جواز السفر</td><td colspan="1" valign="top">Optional</td><td colspan="1" valign="top"><p>Alphanu meric,</p><p>Max 20</p></td><td colspan="1" valign="top"><p>Collects the user's</p><p>passport</p></td><td colspan="1" valign="top">User.Passpor tNo</td><td colspan="1" valign="top">Type</td><td colspan="1" valign="top"><p>Clear label,</p><p>placehol</p></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">chars</td><td colspan="1" valign="top">number.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">der text.</td></tr>
<tr><td colspan="1" valign="top">ELM- ADD-</td><td colspan="1" valign="top">Dropdown/Chec kboxes</td><td colspan="1" valign="top">Role Selection</td><td colspan="1" valign="top">اختيار الدور</td><td colspan="1" valign="top">Mandatory</td><td colspan="1" valign="top">At least one role</td><td colspan="1" valign="top">Allows selection of</td><td colspan="1" valign="top">User.Role</td><td colspan="1" valign="top">Select</td><td colspan="1" valign="top">Clear label,</td></tr>
<tr><td colspan="1" valign="top">010</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">selected,</td><td colspan="1" valign="top">one or</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">accessibl</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">**conditi</td><td colspan="1" valign="top">more</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">e</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">onal logic</td><td colspan="1" valign="top">predefined</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">options,</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">applies**</td><td colspan="1" valign="top">roles for</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">visual</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">the user.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">feedback</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">**Multi-</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">for</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">select</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">multi-</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">enabled/dis</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">select</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">abled</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">state.</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"><p>dynamicall</p><p>y.**</p></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top">ELM- ADD- 011</td><td colspan="1" valign="top">Button</td><td colspan="1" valign="top">Create User Button</td><td colspan="1" valign="top">زر إنشاء المستخد م</td><td colspan="1" valign="top">Mandatory</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Submits the form to create the</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Click</td><td colspan="1" valign="top">Primary action button,</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">new user.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"><p>clearly</p><p>labeled.</p></td></tr>
<tr><td colspan="1" valign="top"><p>ELM- ADD-</p><p>012</p></td><td colspan="1" valign="top">Button</td><td colspan="1" valign="top">Cancel Button</td><td colspan="1" valign="top">زر إلغاء</td><td colspan="1" valign="top">Mandatory</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top"><p>Discards changes</p><p>and returns</p></td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Click</td><td colspan="1" valign="top"><p>Secondar y action</p><p>button,</p></td></tr>
</table>

|||||||to the user list.|||clearly labeled.|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|<p>ELM- ADD-</p><p>013</p>|Text Label|<p>Validatio n Error</p><p>Message</p>|<p>رسالة خطأ</p><p>التحقق</p>|Conditional|N/A|<p>Displays specific</p><p>validation</p>|N/A|View|<p>Red text, clear</p><p>indicatio</p>|
|||||||errors next|||n of|
|||||||<p>to relevant</p><p>fields.</p>|||error.|
|ELM- ADD-|Text Label|Success Message|رسالة النجاح|Conditional|N/A|Displays confirmatio|N/A|View|Green text,|
|014||||||n of|||promine|
|||||||successful|||nt|
|||||||user|||display.|
|||||||creation.||||
|ELM- ADD- 015|Text Label|Generic Error Message|رسالة خطأ عامة|Conditional|N/A|<p>Displays unexpected</p><p>system errors.</p>|N/A|View|<p>Red text, promine</p><p>nt display.</p>|
|<p>ELM- ADD-</p><p>016</p>|Confirmation Dialog|<p>Role Replace</p><p>ment</p>|<p>تأكيد استبدال</p><p>الدور</p>|Conditional|N/A|<p>Prompts System</p><p>Admin to</p>|N/A|<p>View, Click (Replace/C</p><p>ancel)</p>|<p>Clear message,</p><p>accessibl</p>|
|||Confirm||||confirm|||e|
|||ation||||replacing|||buttons.|
|||||||an existing||||
|||||||user with a||||
|||||||<p>single-</p><p>holder role.</p>||||
|ELM- ADD- 017|Button|<p>Replace Button (in</p><p>Dialog)</p>|<p>زر استبدال في (</p><p>)النافذة</p>|Mandatory|N/A|<p>Confirms replacemen t of</p><p>existing</p>|N/A|Click|<p>Clearly labeled, primary</p><p>action in</p>|
|||||||user and|||dialog.|
|||||||proceeds||||
|||||||with new||||
|||||||<p>user</p><p>creation.</p>||||
|ELM- ADD- 018|Button|<p>Cancel Button (in</p><p>Dialog)</p>|<p>زر إلغاء في (</p><p>)النافذة</p>|Mandatory|N/A|<p>Cancels the replacemen t and aborts</p><p>new user</p>|N/A|Click|Clearly labeled.|
|||||||creation.||||

Summary Section

This rewritten user story for "Add New System User" now includes the updated conditional logic for role selection, allowing multi-select specifically for `(Fund Manager AND Board Member)` or `(Associate Fund Manager AND Board Member)`. This ensures precise control over role combinations.

\*\*Key Implementation Considerations:\*\*

* \*\*Critical:\*\* Implement the precise logic for enabling/disabling multi-select based on the new, more complex role combination condition.
* The `Registration Message Is Sent` flag logic remains as last defined (set to 1 if eligible, 0 if only Board Member, regardless of delivery success).
* Maintain robust validation for Saudi mobile numbers (format and uniqueness) as this is the username.

\*\*Dependencies and Risks:\*\*

* \*\*Dependency:\*\* This story depends on the "View System Users List" story for navigation. It also depends on the availability and configuration of the WhatsApp Business API.

* \*\*Risk:\*\* Increased complexity of role selection UI/logic could lead to bugs or user confusion. Mitigation: Thorough testing of dynamic UI and validation.

  ---

  I have now updated the "Add New System User" user story with the refined conditional logic for role selection.

  \*\*Important Note:\*\* I am still awaiting your clarification on the "Action Not Applicable (Role/Assignment)" logic for the "Resend Account Registration Message" user story from our previous interaction. The current logic in that story (and reflected in its Acceptance Criteria) is: "Action is NOT applicable if the user has ONLY the 'Board Member' role AND is NOT assigned to any fund board." However, with `IsAssignedToFundBoard` removed from the User entity, this specific condition cannot be implemented as written. \*\*Please confirm how the "Resend Message" eligibility for Board Members should be determined without

  `IsAssignedToFundBoard`.\*\*

  Would you like me to generate the user stories for "Activate System User" or "Reset User Password" next, or would you prefer to refine this one further, or select another from the original list?

User Story JDWA−1217: Filter System Users List

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
| :- | :- | - |
|\*\*Pre−condition\*\*|System Admin is logged in and viewing the "View System Users List" (which now defaults to active users).|The user list is displayed and contains data.|
|\*\*Business Rules\*\*|1\. \*\*Default view of the user list is 'Active' users only.\*\*<br>2. Mobile No. filter applies immediately as typed.<br>3. Other filters (Name, Status, Role) are applied via a filter popup.<br>4. Multiple filters can be applied simultaneously (AND logic).<br>5. Text−based filters (Name, Mobile No.) should support partial matches.|Ensures efficient and flexible filtering capabilities with a focused default view and hybrid interaction.|
|\*\*Post− condition\*\*|The displayed user list is updated to show only users matching the applied filter criteria.|The system state reflects the filtered view of user data.|
|\*\*Risk\*\*|1\. Performance degradation with complex filters on large datasets.<br>2. Inaccurate filtering results due to incorrect logic.|Mitigation: Optimized database queries, thorough testing of filter logic.|
|\*\*Assumptions\*\*|1\. The underlying data structure supports efficient filtering by specified attributes.<br>2. The UI provides intuitive filter controls.|These assumptions are necessary for effective filtering.|
|\*\*UX/UI Design Link\*\*|N/A (Refer to general filtering UI patterns)|Specific UI mockups not provided in BRD for this section.|

Process Flow Table

|**Step**|**Action Description**|**Actor**|**Related Message Codes**|**Notes**|
| :- | :- | :- | :- | :- |
|1|System Admin is viewing the "View System Users List" (which defaults to active users).|System Admin|||

|2|System Admin interacts with the "Mobile No." filter control (types into the search box).|System Admin|This is an inline, immediate filter.||
| :- | :- | :- | :- | :- |
|3|System applies the "Mobile No." filter criteria to the displayed user list in real−time.|System|||
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

<table><tr><th colspan="1" valign="top"><b>Alternativ e Scenario</b></th><th colspan="1" valign="top"><b>Condition</b></th><th colspan="1" valign="top"><b>Action</b></th><th colspan="1" valign="top"><p><b>Related Message</b></p><p><b>Codes</b></p></th><th colspan="1" valign="top"><b>Resolution</b></th></tr>
<tr><td colspan="1" valign="top">**No Matching</td><td colspan="1" valign="top">System Admin</td><td colspan="1" valign="top">System displays a "No</td><td colspan="1" valign="top">MSG−FILT−001</td><td colspan="1" valign="top">System Admin</td></tr>
<tr><td colspan="1" valign="top">Users**</td><td colspan="1" valign="top">applies filters, but</td><td colspan="1" valign="top">records exist to</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">can modify or</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top">no users match the</td><td colspan="1" valign="top">display" message.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">clear filters.</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top">criteria.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top">**Invalid Filter</td><td colspan="1" valign="top">System Admin</td><td colspan="1" valign="top">System may ignore</td><td colspan="1" valign="top">MSG−FILT−002</td><td colspan="1" valign="top">System Admin</td></tr>
<tr><td colspan="1" valign="top">Input**</td><td colspan="1" valign="top">enters invalid</td><td colspan="1" valign="top">invalid characters or</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">corrects input.</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top">characters into a</td><td colspan="1" valign="top">display a warning.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top">text filter (e.g., non−</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top">numeric in Mobile</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top">No. field).</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top">**Performance</td><td colspan="1" valign="top">Applying complex</td><td colspan="1" valign="top">System may display a</td><td colspan="1" valign="top">Optimization of</td><td colspan="1" rowspan="4" valign="top"></td></tr>
<tr><td colspan="1" valign="top">Lag**</td><td colspan="1" valign="top">filters on large</td><td colspan="1" valign="top">loading indicator.</td><td colspan="1" valign="top">database</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top">datasets causes a</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">queries and</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top">noticeable delay.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">indexing.</td></tr>
<tr><td colspan="1" valign="top">**System Error</td><td colspan="1" valign="top">A backend error</td><td colspan="1" valign="top">System displays a</td><td colspan="1" valign="top">MSG−FILT−003</td><td colspan="1" valign="top">System Admin</td></tr>
<tr><td colspan="1" valign="top">during Filtering**</td><td colspan="1" valign="top">occurs during the</td><td colspan="1" valign="top">generic error</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">can refresh the</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top">filtering operation.</td><td colspan="1" valign="top">message.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">page or try</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">filtering again.</td></tr>
<tr><td colspan="1" valign="top">**Filter Popup</td><td colspan="1" valign="top">System Admin</td><td colspan="1" valign="top">System closes the</td><td colspan="1" valign="top">System Admin</td><td colspan="1" rowspan="6" valign="top"></td></tr>
<tr><td colspan="1" valign="top">Closed Without</td><td colspan="1" valign="top">opens the filter</td><td colspan="1" valign="top">popup and the user</td><td colspan="1" valign="top">can reopen the</td></tr>
<tr><td colspan="1" valign="top">Applying**</td><td colspan="1" valign="top">popup but closes it</td><td colspan="1" valign="top">list remains</td><td colspan="1" valign="top">popup later.</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top">without clicking</td><td colspan="1" valign="top">unchanged (only</td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top">"Apply Filters".</td><td colspan="1" valign="top">Mobile No. filter, if</td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">applied, persists).</td><td colspan="1" valign="top"></td></tr>
</table>
Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Filter by Name − Success\*\*|A System Admin is viewing the user list.|The System Admin opens the filter popup, types "Moham" into the Name filter, and clicks "Apply Filters".|The system displays only "Mohammed Ahmed" in the user list.|

|\*\*Filter by Role − Success\*\*|A System Admin is viewing the user list.|The System Admin opens the filter popup, selects "Legal Counsel" from the Role filter dropdown, and clicks "Apply Filters".|The system displays only users assigned the "Legal Counsel" role.|
| :- | :- | :- | - |
|\*\*Filter by Status − Success\*\*|A System Admin is viewing the user list which defaults to active users.|The System Admin opens the filter popup, selects "Inactive" from the Status filter, and clicks "Apply Filters".|The system displays only inactive users.|
|\*\*Filter by Mobile No. − Success (Inline)\*\*|A System Admin is viewing the user list containing users with various mobile numbers.|The System Admin types "50123" into the Mobile No. filter search box (inline).|The system immediately displays only users whose mobile number contains "50123".|
|\*\*Multiple Filters (Hybrid) − Success\*\*|A System Admin is viewing the user list.|The System Admin types "50123" into the Mobile No. filter, then opens the filter popup, types "Sara" into the Name filter, and clicks "Apply Filters".|The system displays only active users whose mobile number contains "50123" AND whose name contains "Sara".|
|\*\*No Results Found\*\*|A System Admin is viewing the user list.|The System Admin applies a filter combination (e.g., Name: "XYZ", Role: "Admin") for which no users exist.|The system displays the message "No records exist to display".|
|\*\*Clear Filters\*\*|A System Admin has applied one or more filters to the user list (including Mobile No. and/or popup filters).|The System Admin opens the filter popup and clicks the "Clear Filters" button.|The user list reverts to its default state, displaying only active users, and all filter inputs are cleared.|
|\*\*System Error during Filtering\*\*|A System Admin attempts to apply a filter.|An unexpected error occurs during the filtering process.|<p>The system displays the message "An error is occurred, can’t display data"</p><p>and the filter may not apply</p>|

||||or the list becomes unresponsive.|
| :- | :- | :- | :- |

Data Entities Table

Entity Name: User (for filtering)

|<p>**Attr i bute (En g**</p><p>**lish)**</p>|<p>**Attr i bute (Ar a**</p><p>**bic)**</p>|**Mandator y/Optional**|**Attribute Type**|**Da t a Le n gth**|**Integr a tion Requir ement s**|**Def aul t Val ue**|**Con ditio n (if need ed)**|<p>**Rules (if needed**</p><p>**)**</p>|**Sample in Arabic**|**Sample in English**|
| :- | :- | :- | :- | :- | :- | :- | - | :- | :- | :- |
|Nam e|اϻسم|Mandatory|Text|<p>Ma x 25</p><p>5</p>|Databa se Field|N/A|N/A|N/A|محمد أحمد|Mohammed Ahmed|
|Ema il|البريد اϹلك ترون ي|Mandatory|Text|<p>Ma x 25</p><p>5</p>|Databa se Field|N/A|N/A|<p>Unique</p><p>, Valid email format</p>|mohammed.ah <med@jadwa.co> m|mohammed.ah <med@jadwa.co> m|
|Stat us|الحالة|Mandatory|Boolean/ Dropdow n|N/ A|Databa se Field|Acti ve|N/A|Active/I nactive|نشط|Active|
|Role|الدور|Mandatory|Relation to Role Entity|N/ A|Databa se Many− to− Many|N/A|At least one role must be selec ted|Predefi ned roles only|مدير صندوق، عضو مجلس|Fund Manager, Board Member|
|Mob ile|رقم الجوا ل|Optional|Text|Ma x 15|Databa se Field|N/A|N/A|Numeri c only|501234567|501234567|

Messages/Notifications Table

|**Message Code**|**Message (English)**|**Message (Arabic)**|**Message Type**|**Communication Method**|
| :- | :- | :- | :- | :- |
|MSG−FILT− 001|No records exist to display.|عفوا, ϻ توجد بيانات مسجلة .لعرضها|Information|In−App|
|MSG−FILT− 002|Invalid filter input. Please check your entry.|إدخال غير صالح في الفلتر. .يرجى التحقق من إدخالك|Validation Error|In−App|
|MSG−FILT− 003|<p>An error is occurred,</p><p>can’t display data.</p>|حدث خطأ بالنظام , لم يتمكن .من عرض البيانات|Error Message|In−App|

Screen Elements Table

<table><tr><th colspan="1" valign="top"><b>Eleme nt ID</b></th><th colspan="1" valign="top"><b>Elemen t Type</b></th><th colspan="1" valign="top"><p><b>Elemen t Name (Englis h</b></p><p><b>)</b></p></th><th colspan="1" valign="top"><b>Eleme nt Name (Arab i c)</b></th><th colspan="1" valign="top"><b>Required/Opt i onal</b></th><th colspan="1" valign="top"><b>Validation Rules</b></th><th colspan="1" valign="top"><b>Busin e ss Logic</b></th><th colspan="1" valign="top"><b>Related Data Entity</b></th><th colspan="1" valign="top"><b>User Interact i on</b></th><th colspan="1" valign="top"><b>Accessibili ty Notes</b></th></tr>
<tr><td colspan="1" valign="top">ELM−</td><td colspan="1" valign="top">Input Field</td><td colspan="1" valign="top">Mobile</td><td colspan="1" rowspan="12" valign="top"><p>حقل تصفية رقم الجوال</p><p>(مباشر)</p></td><td colspan="1" valign="top">Optional</td><td colspan="1" valign="top">Numeric</td><td colspan="1" valign="top">Filters</td><td colspan="1" valign="top">User.Mo</td><td colspan="1" valign="top">Type</td><td colspan="1" valign="top">Clear</td></tr>
<tr><td colspan="1" valign="top">FILT−</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">No.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">only</td><td colspan="1" valign="top">the</td><td colspan="1" valign="top">bile</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">label,</td></tr>
<tr><td colspan="1" valign="top">001</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Filter</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">user</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">placehold</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Input</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">list by</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">er text.</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">(Inline)</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">user</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">mobile</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">numbe</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">r</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">(partial</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">match)</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">in real−</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">time.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top">ELM− FILT− 002</td><td colspan="1" valign="top">Button/Ico n</td><td colspan="1" valign="top">Filter Button (Opens Popup)</td><td colspan="1" valign="top"><p>زر/أيقو نة التصفية (يفتح نافذة</p><p>منبثقة)</p></td><td colspan="1" valign="top">Mandatory</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top"><p>Opens the filter popup</p><p>for</p></td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Click</td><td colspan="1" valign="top"><p>Clear icon (e.g., funnel), accessible</p><p>label.</p></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Name,</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Status,</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">and</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
</table>

<table><tr><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th><th colspan="1" valign="top">Role filters.</th><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th><th colspan="1" valign="top"></th></tr>
<tr><td colspan="1" valign="top">ELM− FILT− 003</td><td colspan="1" valign="top">Modal/Po pup</td><td colspan="1" valign="top">Filter Popup</td><td colspan="1" valign="top">نافذة/من بثقة التصفية</td><td colspan="1" valign="top">Mandatory</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Contai ns filter control s for Name, Status, and Role.</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">View</td><td colspan="1" valign="top">Accessible modal, focus managem ent.</td></tr>
<tr><td colspan="1" valign="top">ELM− FILT− 004</td><td colspan="1" valign="top">Input Field</td><td colspan="1" valign="top">Name Filter Input (in Popup)</td><td colspan="1" valign="top"><p>حقل تصفية اϻسم (في النافذة</p><p>المنبثقة)</p></td><td colspan="1" valign="top">Optional</td><td colspan="1" valign="top">Text, Alphanum eric</td><td colspan="1" valign="top"><p>Filters the user list by user name (partial match)</p><p>.</p></td><td colspan="1" valign="top">User.Na me</td><td colspan="1" valign="top">Type</td><td colspan="1" valign="top">Clear label, placehold er text.</td></tr>
<tr><td colspan="1" valign="top">ELM−</td><td colspan="1" valign="top">Dropdown</td><td colspan="1" valign="top">Status</td><td colspan="1" rowspan="6" valign="top"><p>قائمة تصفية الحالة (في النافذة</p><p>المنبثقة)</p></td><td colspan="1" valign="top">Optional</td><td colspan="1" valign="top">Predefine</td><td colspan="1" valign="top">Filters</td><td colspan="1" valign="top">User.Stat</td><td colspan="1" valign="top">Select</td><td colspan="1" valign="top">Clear</td></tr>
<tr><td colspan="1" valign="top">FILT−</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Filter</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">d values:</td><td colspan="1" valign="top">the</td><td colspan="1" valign="top">us</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">label,</td></tr>
<tr><td colspan="1" valign="top">005</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Dropdo</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Active,</td><td colspan="1" valign="top">user</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">accessible</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">wn (in</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Inactive,</td><td colspan="1" valign="top">list by</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">options.</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Popup)</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">All</td><td colspan="1" valign="top">user</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">status.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top">ELM−</td><td colspan="1" valign="top">Dropdown</td><td colspan="1" valign="top">Role</td><td colspan="1" rowspan="7" valign="top"><p>قائمة تصفية الدور (في النافذة</p><p>المنبثقة)</p></td><td colspan="1" valign="top">Optional</td><td colspan="1" valign="top">Predefine</td><td colspan="1" valign="top">Filters</td><td colspan="1" valign="top">User.Rol</td><td colspan="1" valign="top">Select</td><td colspan="1" valign="top">Clear</td></tr>
<tr><td colspan="1" valign="top">FILT−</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Filter</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">d values:</td><td colspan="1" valign="top">the</td><td colspan="1" valign="top">e</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">label,</td></tr>
<tr><td colspan="1" valign="top">006</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Dropdo</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">System</td><td colspan="1" valign="top">user</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">accessible</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">wn (in</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Admin,</td><td colspan="1" valign="top">list by</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">options.</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Popup)</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Fund</td><td colspan="1" valign="top">assigne</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Manager,</td><td colspan="1" valign="top">d role.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">etc.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
</table>

<table><tr><th colspan="1" valign="top">ELM−</th><th colspan="1" valign="top">Button</th><th colspan="1" valign="top">Apply</th><th colspan="1" rowspan="14" valign="top"><p>زر تطبيق الفϼتر (في النافذة</p><p>المنبثقة)</p></th><th colspan="1" valign="top">Mandatory</th><th colspan="1" valign="top">N/A</th><th colspan="1" valign="top">Applies</th><th colspan="1" valign="top">N/A</th><th colspan="1" valign="top">Click</th><th colspan="1" valign="top">Clearly</th></tr>
<tr><td colspan="1" valign="top">FILT−</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Filters</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">all</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">labeled,</td></tr>
<tr><td colspan="1" valign="top">007</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Button</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">selecte</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">primary</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">(in</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">d filter</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">action</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Popup)</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">criteria</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">within</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">from</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">popup.</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">the</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">popup</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">to the</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">user</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">list and</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">closes</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">the</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">popup.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top">ELM−</td><td colspan="1" valign="top">Button</td><td colspan="1" valign="top">Clear</td><td colspan="1" rowspan="14" valign="top"><p>زر مسح الفϼتر (في النافذة</p><p>المنبثقة)</p></td><td colspan="1" valign="top">Optional</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Resets</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Click</td><td colspan="1" valign="top">Clearly</td></tr>
<tr><td colspan="1" valign="top">FILT−</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Filters</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">all</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">labeled.</td></tr>
<tr><td colspan="1" valign="top">008</td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Button</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">filter</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">(in</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">criteria</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Popup)</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">within</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">the</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">popup</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">and</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">clears</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">the</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">inline</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">Mobile</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">No.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">filter.</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top">ELM− FILT− 009</td><td colspan="1" valign="top">Button/Ico n</td><td colspan="1" valign="top">Close Filter Popup Button</td><td colspan="1" valign="top">زر/أيقو نة إغϼق نافذة التصفية</td><td colspan="1" valign="top">Mandatory</td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top"><p>Closes the filter popup</p><p>withou</p></td><td colspan="1" valign="top">N/A</td><td colspan="1" valign="top">Click</td><td colspan="1" valign="top">Standard close icon (e.g., 'X').</td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">t</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">applyin</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
<tr><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top">g</td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td><td colspan="1" valign="top"></td></tr>
</table>

|||||||change s.||||
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|<p>ELM−</p><p>FILT− 010</p>|Text Label|<p>Filter</p><p>Results Messag</p>|رسالة نتائج الفلتر|Conditional|N/A|<p>Display</p><p>s messag</p>|N/A|View|<p>Clear and</p><p>informativ e.</p>|
|||e||||es like||||
|||||||"No||||
|||||||records||||
|||||||exist to||||
|||||||display||||
|||||||" when||||
|||||||filters||||
|||||||yield||||
|||||||no||||
|||||||results.||||
Summary Section

This user story for "Filter System Users List" has been significantly updated to implement a hybrid filtering approach. The "Mobile No." filter now operates inline and in real−time, while other filters (Name, Status, Role) are managed through a dedicated filter popup. The process flow and screen elements have been meticulously re−sequenced and detailed to reflect this new interaction model, ensuring a precise understanding of this complex functionality.

\*\*Key Implementation Considerations:\*\*

* Ensure the inline "Mobile No." filter provides immediate feedback to the user.
* The "Clear Filters" button in the popup should clear \*all\* filters, including the inline Mobile No. filter, and revert the list to its default (active users only) state.
* Ensure the filter popup is a modal and handles focus correctly for accessibility.
* Implement efficient database queries for filtering, especially with large datasets and partial matches.

  \*\*Dependencies and Risks:\*\*

* \*\*Dependency:\*\* This story depends on the "View System Users List" story to provide the base list to be filtered.
* \*\*Risk:\*\* User confusion due to two different filtering mechanisms. Mitigation: Clear UI design, intuitive placement of filter controls, and consistent behavior.

−−−

I have now updated the "Filter System Users List" user story with the revised Process Flow and Screen Elements Table, incorporating the hybrid filtering mechanism.

Would you like me to generate the user stories for "Activate System User" or "Delete System User" next, or would you prefer to refine this one further, or select another from the original list?

User Story JDWA-1213: View System Users List

Introduction Section

This user story outlines the capability for System Administrators to access and review a comprehensive list of all registered users within the Fund Board Management Application. This list serves as the primary entry point for initiating various user management operations, such as viewing detailed profiles, editing user information, managing user status, \*\*resetting passwords, and resending registration messages.\*\* The Screen Elements Table provides a detailed breakdown of the UI components on this list view.

Main User Story Table

|**Field**|**Description**|**Content Guidelines**|
| :- | :- | :- |
|\*\*Name\*\*|View System Users List|<p>Enables System Administrators to view a list of users and navigate to</p><p>specific management actions.</p>|
|\*\*User Story\*\*|<p>As a \*\*System Administrator\*\*, I need to \*\*see a list of all system users with options to manage them\*\* so that I can</p><p>\*\*efficiently oversee and access individual user administration tasks.\*\*</p>||
|\*\*Story Points\*\*|4|<p>Focuses on display and</p><p>navigation, now with more complex action button visibility.</p>|
|\*\*User Roles\*\*|System Admin|<p>Exclusive access for</p><p>System Administrators.</p>|
|<p>\*\*Access</p><p>Requirements\*\*</p>|Authenticated and authorized System Admin access.|<p>User must be logged in with</p><p>System Admin privileges.</p>|
|\*\*Trigger\*\*|<p>System Admin selects "User Management" from the main</p><p>navigation.</p>|<p>User initiates navigation to</p><p>the user list.</p>|
|\*\*Frequency of Use\*\*|High|<p>Regular access for user</p><p>oversight and task initiation.</p>|
|\*\*Pre-condition\*\*|System Admin is logged in and the User Management module is accessible.|<p>The system is operational and the admin has</p><p>necessary permissions.</p>|
|\*\*Business Rules\*\*|<p>1\. The list must display both active and inactive users by default.<br>2. Each user entry must include direct links or buttons for "View", "Edit", "Deactivate", "Activate", "Reset Password", and "Resend Message" actions.<br>3. The "Last Update Date" column must be sortable in ascending and descending order.<br>\*\*4. Button Visibility/Enablement for Actions:\*\*<br> a. \*\*View, Edit:\*\* Always visible and enabled.<br> b. \*\*Deactivate:\*\* Visible and enabled only if the user's current Status is 'Active'.<br> c. \*\*Activate:\*\* Visible and enabled only if the user's current Status is 'Inactive'.<br> d. \*\*Reset Password:\*\* Visible and enabled only if the user's Status is 'Active' AND their 'Registration Is Completed' flag is 1 AND their 'Registration Message Is Sent' flag is 1.<br> e. \*\*Resend Message:\*\* Visible and enabled only if the user is 'Active', AND their 'Registration Is Completed' flag is 0, AND their 'Registration Message Is Sent'</p><p>flag is 1.</p>|Ensures comprehensive visibility, direct access to management functions, data organization, and precise action button visibility.|
|\*\*Post- condition\*\*|A paginated list of all system users is displayed, with actionable navigation controls for each user, and the list can be sorted by "Last Update Date".|<p>The system presents the user data with interactive management options and</p><p>sorting capability.</p>|

|\*\*Risk\*\*|1\. Performance degradation with large user datasets.<br>2. Ambiguous navigation leading to user errors.<br>3. Incorrect button visibility logic.|<p>Mitigation: Implement efficient pagination and clear UI/UX for action buttons, thorough testing of</p><p>visibility conditions.</p>|
| :- | :- | :- |
|\*\*Assumptions\*\*|<p>1\. User data is consistently stored and retrievable.<br>2. Dedicated user stories exist for each linked action (View, Edit,</p><p>Deactivate, Activate, Reset Password, Resend Message).</p>|<p>Fundamental assumptions for list display and action</p><p>linking.</p>|
|\*\*UX/UI Design Link\*\*|N/A (General list/table patterns with action columns)|<p>No specific mockups provided in the BRD for</p><p>this section.</p>|

Process Flow Table

|**Step**|**Action Description**|**Actor**|**Related Message Codes**|**Notes**|
| :- | :- | :- | :- | :- |
|1|System Admin logs into the application.|System Admin|||
|2|<p>System Admin navigates to the "User</p><p>Management" section.</p>|System Admin|||
|3|<p>System retrieves all user accounts from</p><p>the database.</p>|System|||
|4|<p>System displays a paginated table listing users by Name, Email, Status, assigned Role, Last Update Date, and an "Actions"</p><p>column.</p>|System||Default sorting (e.g., by Name A-Z) is applied.|
|5|<p>System Admin reviews the user list and</p><p>can navigate through pages.</p>|System Admin|||
|6|<p>System Admin clicks on the "Last</p><p>Update Date" column header.</p>|System Admin|||
|7|<p>System re-sorts the user list by "Last</p><p>Update Date" in ascending or descending order (toggles on click).</p>|System|||
|8|<p>System Admin can click on any of the actionable buttons/links in the "Actions" column (View, Edit, Deactivate, Activate, Reset Password, Resend Message) to initiate a specific user</p><p>management task.</p>|System Admin||The specific actions and their eligibility are detailed in the "Actionable Buttons Table" below.|

Alternative Flow Table

|**Alternative Scenario**|**Condition**|**Action**|**Related Message Codes**|**Resolution**|
| :- | :- | :- | :- | :- |
|<p>\*\*No Users</p><p>Registered\*\*</p>|<p>The system database</p><p>contains no user accounts.</p>|<p>System displays a</p><p>message indicating no records exist.</p>|MSG-LIST-001|<p>System Admin can</p><p>proceed to add new users.</p>|
|\*\*Unknown Display Error\*\*|<p>An unexpected error occurs during the rendering or display of user data after</p><p>retrieval.</p>|System displays a generic error message.|MSG-LIST-002|System Admin can refresh the page or contact support.|
|\*\*Action Button Hidden\*\*|<p>A specific action button (e.g., "Reset Password") is not applicable for a user</p><p>(e.g., user is inactive).</p>|<p>The corresponding action button/link is</p><p>\*\*hidden\*\*, preventing the action.</p>|<p>System Admin understands the action is not</p><p>currently available.</p>||
|\*\*Sorting Error\*\*|An error occurs during the sorting operation.|System displays a generic error message.|MSG-LIST-002|<p>System Admin can</p><p>refresh the page or try sorting again.</p>|
||||||

Acceptance Criteria Table

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|\*\*Display All Users\*\*|A System Admin is logged in and accesses the "User Management" section.|The system has a mix of active and inactive users registered.|<p>The system displays a paginated list showing all registered users, including their Name, Email, Status, assigned Role, and Last Update Date, with an "Actions" column containing clickable options as defined in the</p><p>"Actionable Buttons Table".</p>|
|\*\*Action Button Visibility (General)\*\*|A System Admin is viewing the user list.|A user row is displayed.|<p>The "Actions" column for that user row correctly displays enabled/disabled/hidden buttons for "View", "Edit", "Deactivate", "Activate", "Reset Password", and "Resend Message" based on the user's status and</p><p>eligibility criteria.</p>|
|\*\*Action Button Visibility (Reset Password Eligible)\*\*|<p>A System Admin is viewing the user list. A user is 'Active', 'Registration Is Completed' is 1, and 'Registration Message Is</p><p>Sent' is 1.</p>|The user row is displayed.|The "Reset Password" button is visible and enabled for this user.|
|\*\*Action Button Visibility (Resend Message Eligible)\*\*|<p>A System Admin is viewing the user list. A user is 'Active', 'Registration Is Completed' is 0, and 'Registration Message Is</p><p>Sent' is 1.</p>|The user row is displayed.|The "Resend Message" button is visible and enabled for this user.|
|\*\*Unknown Display Error\*\*|A System Admin is logged in and attempts to access the "User Management" section.|<p>An unexpected error occurs during the display of user data (e.g., UI rendering</p><p>issue, data parsing error).</p>|The system displays the message "An error is occurred, can’t display data" and the user list is not displayed or is incomplete.|
|\*\*Sort by Last Update Date - Ascending\*\*|A System Admin is viewing the user list with multiple users having different "Last Update Dates".|<p>The System Admin clicks the "Last Update Date" column header for the first time (or to</p><p>set ascending sort).</p>|The user list is re-sorted to display users with the oldest "Last Update Date" first.|

Data Entities Table

Entity Name: User (for listing and navigation)

|**Attrib ute (Engli sh)**|**Attri bute (Ara bic)**|**Mandatory/ Optional**|**Attribute Type**|**Dat a Len gth**|**Integra tion Require ments**|**Defau lt Value**|**Condi tion (if neede d)**|**Rules (if needed)**|**Sample in Arabic**|**Sample in English**|
| :- | - | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|User ID|معرف المستخ دم|Mandatory|Number (Auto- generated)|N/A|<p>Databas e Primary</p><p>Key</p>|N/A|N/A|Unique identifie r|12345|12345|
|Name|اϻسم|Mandatory|Text|<p>Ma x</p><p>255</p>|Databas e Field|N/A|N/A|N/A|محمد أحمد|Mohammed Ahmed|
|Email|<p>البريد</p><p>اϹلكتر وني</p>|Mandatory|Text|<p>Ma</p><p>x 255</p>|Databas e Field|N/A|N/A|Unique, Valid|mohammed.ahme <d@jadwa.com>|mohammed.ahme <d@jadwa.com>|

|||||||||<p>email</p><p>format</p>|||
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|Status|الحالة|Mandatory|<p>Boolean/D</p><p>ropdown</p>|N/A|<p>Databas</p><p>e Field</p>|<p>Activ</p><p>e</p>|N/A|<p>Active/I</p><p>nactive</p>|نشط|Active|
|Role|الدور|Mandatory|Relation to Role Entity|N/A|Databas e Many- to-Many|N/A|<p>At least one role must be select ed, condit ional logic applie</p><p>s</p>|Predefin ed roles only|مدير صندوق، عضو مجلس|Fund Manager, Board Member|
|Last Updat e Date|تاريخ آخر تحديث|Mandatory|DateTime|N/A|Databas e Field|Curre nt Times tamp|N/A|<p>Automat ically set on creation and</p><p>update.</p>|<p>2023-10-26</p><p>15:00:00</p>|<p>2023-10-26</p><p>15:00:00</p>|
|Regist ration Messa ge Is Sent|تم إرسال رسالة التسجي ل|Mandatory|Boolean|N/A|Databas e Field|<p>0</p><p>(False</p><p>)</p>|N/A|<p>Set to 1 if user's role(s) make them eligible for WhatsA pp message (NOT</p><p>only 'Board Member'</p><p>); 0 if only 'Board Member'</p><p>.</p>|1|True|
|Regist ration Is Compl eted|تم اكتمال التسجي ل|Mandatory|Boolean|N/A|Databas e Field|<p>0</p><p>(False</p><p>)</p>|N/A|<p>Set to 0 upon user creation. Updated by a separate process (e.g., user's first</p><p>login).</p>|0|False|

Entity Name: Role (for listing purposes)

|<p>**Attribut e (English**</p><p>**)**</p>|**Attribut e (Arabic)**|**Mandatory/Option al**|**Attribut e Type**|**Data Lengt h**|**Integration Requiremen ts**|**Defaul t Value**|**Conditio n (if needed)**|**Rules (if needed)**|<p>**Sampl e in Arabi**</p><p>**c**</p>|<p>**Sampl e in Englis**</p><p>**h**</p>|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |

|Role ID|معرف الدور|Mandatory|<p>Number (Auto- generated</p><p>)</p>|N/A|Database Primary Key|N/A|N/A|Unique identifie r|1|1|
| :- | -: | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|<p>Role</p><p>Name</p>|اسم الدور|Mandatory|Text|<p>Max</p><p>50</p>|<p>Database</p><p>Field</p>|N/A|N/A|Unique|<p>مدير</p><p>النظام</p>|<p>System</p><p>Admin</p>|

Messages/Notifications Table

|**Message Code**|**Message (English)**|**Message (Arabic)**|**Message Type**|**Communication Method**|
| :- | :- | :- | :- | :- |
|<p>MSG-LIST-</p><p>001</p>|No records exist to display.|<p>ϻ توجد بيانات مسجلة ,عفوا</p><p>.لعرضها</p>|Information|In-App|
|<p>MSG-LIST-</p><p>002</p>|<p>An error is occurred, can’t</p><p>display data.</p>|<p>لم يتمكن من ,حدث خطأ بالنظام</p><p>.عرض البيانات</p>|<p>Error</p><p>Message</p>|In-App|

Screen Elements Table

|**Elem ent ID**|**Element Type**|**Element Name (English)**|<p>**Eleme nt Name**</p><p>**(Arab ic)**</p>|**Required/Opt ional**|**Validat ion Rules**|**Business Logic**|**Related Data Entity**|**User Interacti on**|**Accessibili ty Notes**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|ELM- LIST- 001|Page Title|User Manage ment|إدارة المستخد مين|N/A|N/A|Displays the title of the page.|N/A|View|<p>H1</p><p>heading, clear and concise.</p>|
|ELM- LIST- 002|Button|Add New User|إضافة مستخدم جديد|Optional|N/A|Navigates to the "Add New System User" form.|N/A|Click|<p>Primary action button, clearly</p><p>labeled.</p>|
|ELM- LIST- 003|Table|User List Table|جدول قائمة المستخد مين|Mandatory|N/A|Displays a paginated list of system users.|User|View, Scroll, Click (on rows/acti ons)|<p>Accessible table structure, clear column</p><p>headers.</p>|
|<p>ELM- LIST-</p><p>004</p>|Table Header|Name|اϻسم|Mandatory|N/A|<p>Column header for</p><p>user's name.</p>|User.Name|Click (for sorting)|<p>Screen reader</p><p>friendly.</p>|
|<p>ELM- LIST-</p><p>005</p>|Table Header|Email|<p>البريد اϹلكترو</p><p>ني</p>|Mandatory|N/A|<p>Column header for</p><p>user's email.</p>|User.Email|Click (for sorting)|<p>Screen reader</p><p>friendly.</p>|
|ELM- LIST- 006|Table Header|Status|الحالة|Mandatory|N/A|<p>Column header for user's status (Active/Inact</p><p>ive).</p>|User.Status|Click (for sorting)|Screen reader friendly.|
|ELM- LIST- 007|Table Header|Role|الدور|Mandatory|N/A|<p>Column header for user's assigned</p><p>role(s).</p>|User.Role|Click (for sorting)|Screen reader friendly.|
|ELM- LIST- 008|Table Header|Last Update Date|تاريخ آخر تحديث|Mandatory|N/A|<p>Column header for the last modification date of the</p><p>user record.</p>|User.LastUpdat eDate|Click (for sorting)|Screen reader friendly, indicates sortability.|

|ELM- LIST- 009|Table Header|Actions|اϹجراءا ت|Mandatory|N/A|<p>Column header for action buttons/links</p><p>.</p>|N/A|N/A|Screen reader friendly.|
| - | :- | :- | -: | :- | :- | :- | :- | :- | :- |
|ELM- LIST- 010|Text Label|User Name (in row)|<p>اسم المستخد في (م</p><p>)الصف</p>|Mandatory|N/A|<p>Displays the name of each user in</p><p>the list.</p>|User.Name|View|Ensure sufficient contrast.|
|ELM- LIST- 011|Text Label|User Email (in row)|<p>بريد المستخد م اϹلكترو في (ني</p><p>)الصف</p>|Mandatory|N/A|Displays the email of each user in the list.|User.Email|View|Ensure sufficient contrast.|
|ELM- LIST- 012|Text Label|User Status (in row)|حالة المستخد في (م )الصف|Mandatory|N/A|Displays the status (Active/Inact ive) of each user.|User.Status|View|<p>Use clear visual indicators (e.g., color, icon) for</p><p>status.</p>|
|ELM- LIST- 013|Text Label|User Role (in row)|دور المستخد في (م )الصف|Mandatory|N/A|<p>Displays the primary role or a list of roles for</p><p>each user.</p>|User.Role|View|Ensure sufficient contrast.|
|ELM- LIST- 014|Text Label|Last Update Date (in row)|تاريخ آخر تحديث في ( )الصف|Mandatory|N/A|Displays the last date and time the user record was modified.|User.LastUpdat eDate|View|<p>Ensure sufficient contrast, consistent date/time</p><p>format.</p>|
|ELM- LIST- 015|Button/Link|View (in row)|عرض في ( )الصف|Mandatory|N/A|<p>Navigates to the "View System User Details"</p><p>page.</p>|N/A|Click|Clear icon/text, accessible label.|
|ELM- LIST- 016|Button/Link|Edit (in row)|<p>تعديل</p><p>في ( )الصف</p>|Mandatory|N/A|<p>Navigates to the "Edit Existing System</p><p>User" form.</p>|N/A|Click|Clear icon/text, accessible label.|
|ELM- LIST- 017|Button/Link|Deactivat e (in row)|إلغاء التفعيل في ( )الصف|Conditional|N/A|<p>Initiates deactivation process.</p><p>\*\*Hidden if user is Inactive or if deactivation is prevented by single- holder role rule.\*\*</p>|N/A|Click|Clear icon/text, accessible label, disabled if user is inactive.|
|ELM- LIST- 018|Button/Link|Activate (in row)|<p>تفعيل</p><p>في ( )الصف</p>|Conditional|N/A|<p>Initiates activation process.</p><p>\*\*Hidden if</p><p>user is Active.\*\*</p>|N/A|Click|<p>Clear icon/text, accessible label, visible/ena</p><p>bled based</p>|

||||||||||<p>on</p><p>conditions.</p>|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|<p>\*\*EL M- LIST-</p><p>019\*\*</p>|\*\*Button/Li nk\*\*|\*\*Reset Password (in row)\*\*|<p>إعادة\*\* تعيين كلمة المرور في ( )الصف</p><p>\*\*</p>|<p>\*\*Conditional</p><p>\*\*</p>|<p>\*\*N/A\*</p><p>\*</p>|\*\*Initiates the "Reset User Password" process.\*\*|\*\*N/A\*\*|<p>\*\*Click\*</p><p>\*</p>|<p>\*\*Clear label, hidden if user is not eligible (Status = Inactive OR</p><p>Registratio n Is Completed</p><p>= 0 OR</p><p>Registratio n Message</p><p>Is Sent = 0).\*\*</p>|
|<p>\*\*EL M- LIST-</p><p>020\*\*</p>|\*\*Button/Li nk\*\*|\*\*Resen d Message (in row)\*\*|<p>إعادة\*\* إرسال الرسالة في ( )الصف</p><p>\*\*</p>|<p>\*\*Conditional</p><p>\*\*</p>|<p>\*\*N/A\*</p><p>\*</p>|\*\*Initiates the "Resend Account Registration Message" process.\*\*|\*\*N/A\*\*|<p>\*\*Click\*</p><p>\*</p>|<p>\*\*Clear label, hidden if user is not eligible (Status = Inactive OR</p><p>Registratio n Is Completed</p><p>= 1 OR</p><p>Registratio n Message Is Sent = 0).\*\*</p>|
|<p>ELM-</p><p>LIST- 021</p>|<p>Pagination</p><p>Controls</p>|<p>عناصر</p><p>التحكم بالترقيم</p>|N/A|Mandatory|N/A|<p>Allows</p><p>navigation through multiple pages of</p><p>users.</p>|N/A|Click|<p>Clear</p><p>navigation, current page indicator.</p>|
|ELM- LIST- 022|Text Label|"No records exist to display" message|<p>رسالة ,عفوا" ϻ توجد بيانات مسجلة لعرضها</p><p>"</p>|Conditional|N/A|Displays when the user list is empty.|N/A|View|Clear and informativ e.|
|ELM- LIST- 023|Text Label|Error Message Display|عرض رسالة الخطأ|Conditional|N/A|<p>Displays generic error messages (e.g., for data retrieval or display</p><p>errors).</p>|N/A|View|Prominent display, clear text.|

Actionable Buttons Table

|**Button/Link Name (English)**|<p>**Button/Link Name**</p><p>**(Arabic)**</p>|**Description**|**Target User Story**|**Visibility/Enablement Condition**|
| :- | :- | :- | :- | :- |

|\*\*View\*\*|عرض|<p>Navigates to the detailed profile page of the</p><p>selected user.</p>|View System User Details|Always visible and enabled for all users.|
| :- | -: | :- | :- | :- |
|\*\*Edit\*\*|تعديل|<p>Navigates to the editable form for the selected user's</p><p>details.</p>|Edit Existing System User|Always visible and enabled for all users.|
|\*\*Deactivate\*\*|إلغاء التفعيل|Initiates deactivation process.|Activate/Deactivate System User|<p>Visible and enabled only if the user's current Status is 'Active' AND deactivation is NOT prevented by</p><p>single-holder role rule.</p>|
|\*\*Activate\*\*|تفعيل|<p>Initiates activation</p><p>process.</p>|Activate/Deactivate System User|Visible and enabled only if the user's current Status is 'Inactive'.|
|\*\*Reset Password\*\*|إعادة تعيين كلمة المرور|Initiates the password reset process.|Reset User Password|<p>Visible and enabled only if the user's Status is 'Active' AND their 'Registration Is Completed' flag is 1 AND their 'Registration Message Is</p><p>Sent' flag is 1.</p>|
|\*\*Resend Message\*\*|إعادة إرسال الرسالة|Initiates the resend account registration message process.|Resend Account Registration Message|<p>Visible and enabled only if the user is 'Active', AND their 'Registration Is Completed' flag is 0, AND their 'Registration Message Is Sent' flag is</p><p>1\.</p>|

Summary Section

This rewritten user story for "View System Users List" now fully incorporates the "Reset Password" and "Resend Message" buttons into its "Actions" column. Their precise display conditions, as defined in their respective user stories, are explicitly stated in the Business Rules, Acceptance Criteria, and Screen Elements Table. This ensures that the list view provides accurate and context-sensitive access to all user management actions.

\*\*Important Note:\*\* I am still awaiting your clarification on the "Action Not Applicable (Role/Assignment)" logic for the "Resend Account Registration Message" user story from our previous interaction. The current logic in that story (and reflected in its Acceptance Criteria) is: "Action is NOT applicable if the user has ONLY the 'Board Member' role AND is NOT assigned to any fund board." However, with `IsAssignedToFundBoard` removed from the User entity, this specific condition cannot be implemented as written. \*\*Please confirm how the "Resend Message" eligibility for Board Members should be determined without

`IsAssignedToFundBoard`.\*\* This clarification is essential for the "Resend Account Registration Message" story's button visibility logic, and consequently for the "Resend Message" button's visibility in this "View System Users List" story.

\*\*Key Implementation Considerations:\*\*

* \*\*Critical:\*\* Implement the complex combined visibility logic for all action buttons in the "Actions" column.
* Ensure clicking these buttons correctly navigates to or triggers the corresponding user story's process.
* Maintain efficient data retrieval and rendering for the list, especially with multiple conditional columns.

\*\*Dependencies and Risks:\*\*

* \*\*Dependency:\*\* This story depends on all other user management stories (Add, Edit, View Details, Activate/Deactivate, Reset Password, Resend Message) for the functionality of its action buttons.
* \*\*Risk:\*\* Incorrect button visibility leading to user frustration or security vulnerabilities. Mitigation: Rigorous testing of all visibility conditions.

  ---

I have now rewritten the "View System Users List" user story with the inclusion and conditional display of "Reset Password" and "Resend Message" buttons. Please provide clarification on the "Resend Account Registration Message" logic regarding Board Members when you're ready.
