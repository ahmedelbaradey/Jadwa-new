# User Stories Documentation

## Notifications (JDWA-451)

### [JDWA-454] Receive a Notification

**User Story**
As a system user, I want to receive notifications, So that I can track update on my funds

**Process Flow**
1. User receive a notification
2. System display a push notification in the bottom right
   - **A. If user is navigating any screen**
     - System does below actions:
       - update general notifications counter
   
   - **Alternative 1 - B. if user is navigating fund details screen**
     - system does below actions:
       - update general notifications counter
       - update notification list in left side, and display the received notification in the top (LIFO)

**Acceptance Criteria**
| Scenario | Given | When | Then |
|----------|-------|------|------|
| Successful notification message display | The user is on any screen | The user receives a notification | The notification is displayed and notification counter is updated |
| Successful notification message display | The user is on fund details screen | The user receives a notification | The notification is displayed, and notification counter and notification list are updated |
| Unknown error during displaying data | The user is connected to the internet | The user clicks screen link | An error message is displayed indicating unknown error, and the order remains not done.MSG002 |

---

### [JDWA-453] Navigate fund notification list

**User Story**
As a fund manager/legal counsil/board secretary/board member, I want to investigate fund notification list, So that I can track my funds changes trough notifications

**Process Flow**
1. User press notification list tab in fund details screen, side panel with 2 tabs:
   - Notifications list سجل التنبيهات (default selected)
   - status history سجل الحركات
2. System displays notification list tab which contains all fund notifications sent to the logged-in user displayed as cards, ordered DESC by date (LIFO)

**Alternative 1 (filter notification list)**
1. In fund details screen, user press fund activity notification counter
2. System filters fund notification list

**Acceptance Criteria**
| Scenario | Given | When | Then |
|----------|-------|------|------|
| Successful fund notification list display | The user is on the fund notification list screen | The user clicks screen link | The fund notification list is displayed |
| Unknown error during displaying data | The user is connected to the internet | The user clicks screen link | An error message is displayed indicating unknown error, and the order remains not done.MSG001 |

---

### [JDWA-452] Send a Notification

**User Story**
As a system, I want to send notifications, So that I can notify users with funds last updates

**Process Flow**
1. User take an action where it is includes send a notification
2. System determines notification activity (General, Fund, Resolutions, Members, Meetings, Assessments, Documents)
   - **A. if this notification is related to a specific fund (activity = fund)**, System updates:
     - user general notification counter
     - user fund notification counter

**Alternative 1**
- **B. if this notification is related to a specific fund activity (activity = Resolution, Members, Meetings, Assessments, documents)**, system updates:
  - user general notification counter
  - user fund notification counter
  - user activity counter

**Alternative 2**
- **C. if this notification is not related to a specific fund (activity = general)**, system updates:
  - user general notification counter

**Acceptance Criteria**
| Scenario | Given | When | Then |
|----------|-------|------|------|
| Successful main notification counter update | The user is on any screen | The user receives a notification | The main notification counter is updated |
| Successful fund notification counter update | The user is on funds screen | The user refresh screen after receiving a notification | The fund notification counter is updated |
| Successful fund activity notification counter update | The user is on fund details screen | The user refresh screen after receiving a notification related to a specific activity | The fund activity notification counter is updated |
| Unknown error during sending | The user is connected to the internet | The user do the acquired action | An error message is displayed indicating unknown error, and the order remains not done.MSG002 |

---

## Funds Management (JDWA-180)

### [JDWA-278] Search funds

**User Story**
As a system user, I want to search funds as per my role, So that I can track my funds

**Process Flow**
1. User clicks "funds" link in side menu
2. System displays Funds screen, which display all funds assigned to the logged in user as fund manager, or legal council or board secretary, or board member
   - If there are no funds, system display MSG001
3. User can search funds with fund name
4. System filters displayed funds as per search criteria
5. User can use advanced search by clicking filter icon
6. system display advanced search popup
7. user enter search criteria and press "search" button
8. System displays funds which matches selected criteria (And)
   - If there are no funds match the search criteria, system display MSG001

**Acceptance Criteria**
| Scenario | Given | When | Then |
|----------|-------|------|------|
| Successful funds search display | The user is on the funds screen | The user clicks filter | The fund list matches the filter is displayed |
| Unknown error during displaying data | The user is connected to the internet | The user clicks screen link | An error message is displayed indicating unknown error, and the order remains not done.MSG002 |

---

### [JDWA-276] Edit fund Exit date -legal council

**User Story**
As user, I want edit fund exit date, So that I can manage my fund.

**Process Flow**
1. From funds screen user press one of fund cards with status, any status except "under construction/تحت الإنشاء"
2. User press "details" button
3. System displays fund details screen
   - If status "لا يوجد أعضاء" && user (legal council && board secretary && fund manager), system display a message MSG001
   - All fund activities are disabled, except members
4. User press "edit Exit date"
5. System display pop up includes exit date's date picker
6. User enter exit date and press save
7. System do below actions:
   - save Exit date
   - send notification to fund manager(s) && && board secretary(s), board members attached to the fund MSG004, notification activity (fund)
   - and then display a success message MSG002

**Acceptance Criteria**
| Scenario | Given | When | Then |
|----------|-------|------|------|
| Successful edit Exit date submission | The user is on the edit exit date popup | The user fills in the required fields and clicks "Save" | The fund exit date is saved and a confirmation message is displayed.MSG002 |
| Unknown error during submission | The user is connected to the internet | The user clicks "save" | An error message is displayed indicating Unknown error, and the order remains unsaved.MSG003 |

**System Messages**
| Arabic Message | English Message | Subject | Type | MSGID |
|----------------|-----------------|---------|------|-------|
| يجب إضافة أعضاء الصندوق حتى تتمكن من التعامل مع أنشطة الصندوق , على الأقل 2 عضو مستقل | You should add board members to be able to perform fund activities, at least 2 independent members. | | Notification | MSG001 |
| تم حفظ البيانات بنجاح | Record Saved Successfully | | Notification | MSG002 |
| حدث خطأ بالنظام , لم يتم حفظ البيانات | An error is occurred while saving data | | Error | MSG003 |
| تم تعديل تاريخ التخارج للصندوق "اسم الصندوق" ليصبح "تاريخ التخارج" بواسطة "اسم المستخدم" | Data is completed for the fund "fund name", by "user name" | تغيير تاريخ التخارج/ change exit date | Notification | MSG004 |

---

### [JDWA-274] Navigate Fund details

**User Story**
As user, I want navigate the fund details, So that I can manage my fund.

**Process Flow**
1. From funds screen user press one of fund cards with status, any status except "under construction/تحت الإنشاء"
2. User press "details" button
3. System displays fund details screen
   - If status "لا يوجد أعضاء" && user (legal council && board secretary && fund manager), system display a notification message MSG001
   - All fund activities are disabled, except members
4. Fund details include resolutions, assessments, documents, meetings, members, displayed as clickable cards

**Acceptance Criteria**
| Scenario | Given | When | Then |
|----------|-------|------|------|
| Successful fund details display | The user is on the fund details screen | The user clicks screen link | The fund details is displayed |
| Unknown error during displaying data | The user is connected to the internet | The user clicks screen link | An error message is displayed indicating unknown error, and the order remains not done.MSG002 |

**System Messages**
| Arabic Message | English Message | Type | MSGID |
|----------------|-----------------|------|-------|
| يجب إضافة أعضاء الصندوق حتى تتمكن من التعامل مع أنشطة الصندوق , على الأقل 2 عضو مستقل | You should add board members to be able to perform fund activities, at least 2 independent members. | Notification | MSG001 |
| حدث خطأ بالنظام , لم يتمكن من عرض البيانات | An error is occurred, can't display data | Error | MSG002 |

---

## Settings (JDWA-310)

### [JDWA-184] Add fund strategy

**User Story**
As an system admin, I want to add a fund strategy, So that I can manage fund strategies to help users to create funds with appropriate strategy

**Process Flow**
1. User clicks "add fund strategy" button in fund strategies screen
2. System display add fund strategy popup
3. User fill Basic info required data
4. User press "Save" button
5. System validates entering required data
   - If any violation is existed, system display and error message MSG003
6. System does below actions:
   - saves fund strategy data, and creation date
   - display success message MSG001

**Acceptance Criteria**
| Scenario | Given | When | Then |
|----------|-------|------|------|
| Successful new fund strategy submission | The user is on the add fund strategy screen | The user fills in the required fields and clicks "Save" | The fund strategy is saved and a confirmation message is displayed.MSG001 |
| Missing required fields | The user has not filled in all mandatory fields | The user clicks "Save" | An error message is displayed indicating which fields are required. MSG003 |
| Unknown error during submission | The user is connected to the internet | The user clicks "save" | An error message is displayed indicating unknown error, and the order remains unsaved.MSG002 |

**System Messages**
| Arabic Message | English Message | Type | MSGID |
|----------------|-----------------|------|-------|
| تم إضافة البيانات بنجاح | Record Saved Successfully | Success | MSG001 |
| حدث خطأ بالنظام , لم يتم حفظ البيانات | An error is occurred while saving data | Error | MSG002 |
| حقل إلزامي | Required Field | Error | MSG003 |

---

### [JDWA-183] Edit fund strategy

**User Story**
As a system admin, I want to edit a fund strategy, So that I can fund strategies to help users to create funds with appropriate strategy

**Process Flow**
1. For one of displayed fund strategies, User clicks "Edit fund strategy" button in fund strategies screen
2. System display edit fund strategy popup
3. User edits fund strategy data
4. User press "Save" button
5. System validates entering required data
   - If any violation is existed, system display and error message MSG003
6. system does below actions:
   - saves fund strategy data and update date
   - display success message MSG001

**Acceptance Criteria**
| Scenario | Given | When | Then |
|----------|-------|------|------|
| Successful edit fund strategy data | The user is on the edit fund strategy screen | The user fills in the required fields and clicks "Save" | The fund strategy data is edited and a confirmation message is displayed.MSG001 |
| Missing required fields | The user has not filled in all mandatory fields | The user clicks "Save" | An error message is displayed indicating which fields are required. MSG003 |
| Unknown error during submission | The user is connected to the internet | The user clicks "save" | An error message is displayed indicating unknown error, and the order remains unsaved.MSG002 |

**System Messages**
| Arabic Message | English Message | Type | MSGID |
|----------------|-----------------|------|-------|
| تم تعديل البيانات بنجاح | Record Saved Successfully | Success | MSG001 |
| حدث خطأ بالنظام , لم يتم حفظ البيانات | An error is occurred while saving data | Error | MSG002 |
| حقل إلزامي | Required Field | Error | MSG003 |

---

### [JDWA-182] Navigate fund strategies

**User Story**
As a system admin, I want to investigate fund strategies, So that I can manage fund strategies to help users to create funds with appropriate strategy

**Process Flow**
1. User clicks "fund strategies" link in settings screen
2. System displays fund strategies screen which contains all fund strategies, ordered by updated date
   - If there are no fund strategies, system display MSG001
3. User can sort data alphabetically by Arabic name or English name, or DESC/ASC update date

**Acceptance Criteria**
| Scenario | Given | When | Then |
|----------|-------|------|------|
| Successful fund strategies display | The user is on the fund strategies' screen | The user clicks screen link | The fund strategies list is displayed |
| Unknown error during displaying data | The user is connected to the internet | The user clicks screen link | An error message is displayed indicating unknown error, and the order remains not done.MSG002 |

---

### [JDWA-181] Navigate funds

**User Story**
As a system user, I want to investigate funds as per my role, So that I can manage funds I had been assigned to and perform my activities.

**Process Flow**
1. User clicks "funds" link in side menu
2. System displays Funds screen, which display all funds assigned to the logged in user as fund manager, or legal council or board secretary, or board member
   - If there are no funds, system display MSG001
3. If the user is assigned to any funds, system display list of funds categorized by fund strategy, strategies are ordered DESC by no. of funds
4. Each fund strategy is displayed as a separate panel
   - 1st panel only is expanded
   - User can collapse/expand panels, only one panel can be expanded at a time
   - Funds are displayed as clickable cards, ordered by last action under the same category

**Acceptance Criteria**
| Scenario | Given | When | Then |
|----------|-------|------|------|
| Successful funds display | The user is on the funds screen | The user clicks screen link | The fund list is displayed |
| Unknown error during displaying data | The user is connected to the internet | The user clicks screen link | An error message is displayed indicating unknown error, and the order remains not done.MSG002 |

---

### [JDWA-275] Navigate fund status history

**User Story**
As a fund manager/legal counsil/board secretary, I want to investigate fund status history, So that I can track my funds status changes

**Process Flow**
1. User press status history tab in fund details screen, side panel with 2 tabs:
   - notifications سجل التنبيهات (default selected)
   - status history سجل الحركات
2. System displays fund status history tab which contains all fund status history displayed as cards, ordered DESC by date

**Acceptance Criteria**
| Scenario | Given | When | Then |
|----------|-------|------|------|
| Successful fund status history display | The user is on the fund status history screen | The user clicks screen link | The fund status history list is displayed |
| Unknown error during displaying data | The user is connected to the internet | The user clicks screen link | An error message is displayed indicating unknown error, and the order remains not done.MSG001 |

---

### [JDWA-188] Complete fund data- Legal council

**User Story**
As a legal council, I want complete the new fund data, So that I can go forward in fund activities.

**Process Flow**
1. From funds screen user press one of fund cards with status "under construction/تحت الإنشاء"
2. System display "Edit fund" screen
3. User fill Basic info required data
4. User press "Save" button
5. System validates entering required data
   - If any violation is existed, system display and error message MSG001
6. system does below actions:
   - save fund with status "waiting for adding members/ لا يوجد أعضاء" with the same fund code
   - if TNC file is changed, save TNC file in documents under TNC category (replace the current file)
   - log Action details, action name (fund data completion), date, user (user name), role
   - send notification:
     - in case of adding a new fund manager(s) && board secretary(s) attached to the fund MSG005, notification activity (fund)
     - in case of deleting one of fund manager(s) && board secretary (s) attached to the fund MSG003, notification activity (fund)
     - else notify exist fund manager(s) && board secretary(s) attached to the fund MSG006, notification activity (fund)
   - display success message MSG002

**Acceptance Criteria**
| Scenario | Given | When | Then |
|----------|-------|------|------|
| Missing required fields | The user has not filled in all mandatory fields | The user clicks "Save" | An error message is displayed indicating which fields are required. MSG001 |
| Successful complete fund data submission | The user is on the edit fund screen | The user fills in the required fields and clicks "Save" | The fund data is saved and a confirmation message is displayed.MSG002 |
| Unknown error during submission | The user is connected to the internet | The user clicks "save" | An error message is displayed indicating Unknown error, and the order remains unsaved.MSG004 |

**System Messages**
| Arabic Message | English Message | Subject | Type | MSGID |
|----------------|-----------------|---------|------|-------|
| حقل إلزامي | Required Field | * | Error | MSG001 |
| تم حفظ البيانات بنجاح يجب إضافة أعضاء الصندوق حتى تتمكن من التعامل مع أنشطة الصندوق , على الأقل 2 عضو مستقل | Record Saved Successfully You should add board members to be able to perform fund activities, at least 2 independent members | * | Success | MSG002 |
| تم إعفاؤك من دور "الدور" في صندوق "اسم الصندوق" بواسطة "اسم المستخدم" | You have been relieved of your role as "role name" in fund "fund name" by "username" | إعفاء من مهام/ relieve of duties | Notification | MSG003 |
| حدث خطأ بالنظام , لم يتم حفظ البيانات | An error is occurred while saving data | | Error | MSG004 |
| تم تعيينك ك"الدور" في صندوق "اسم الصندوق" بواسطة "اسم المستخدم" | you are assigned as "Role" to fund "fund name " by "user name" | إضافة مهام/ adding duties | Notification | MSG005 |
| تم استكمال بيانات الصندوق "اسم الصندوق" بواسطة "اسم المستخدم" | Data is completed for the fund "fund name", by "user name" | استكمال بيانات الصندوق/ fund data completion | Notification | MSG006 |

---

### [JDWA-187] Create a fund- fund manager

**User Story**
As a fund manager, I want create a new fund, So that I can manage funds which I can manage.

**Process Flow**
1. From funds screen user press add new fund
2. System display "new fund" screen
3. User fill Basic info required data
4. User press "Save" button
5. System validates entering required data
   - If any violation is existed, system display and error message MSG001
6. system does below actions:
   - generate fund code (serial no)
   - save fund with status "under construction/تحت الإنشاء"
   - save TNC file in documents under TNC category
   - log Action details, action name (fund creation), date, user (user name), role
   - send notification to:
     - other fund manager(s) (if >1) attached to the fund MSG005, notification activity (fund)
     - legal council && board secretary (if exist) attached to the fund MSG002, notification activity (fund)
   - display success message MSG003

**Acceptance Criteria**
| Scenario | Given | When | Then |
|----------|-------|------|------|
| Missing required fields | The user has not filled in all mandatory fields | The user clicks "Save" | An error message is displayed indicating which fields are required. MSG001 |
| Successful fund submission | The user is on the add fund screen | The user fills in the required fields and clicks "Save" | The new fund is saved and a confirmation message is displayed.MSG003 |
| Unknown error during submission | The user is connected to the internet | The user clicks "save" | An error message is displayed indicating Unknown error, and the order remains unsaved.MSG004 |

**System Messages**
| Arabic Message | English Message | Subject | Type | MSGID |
|----------------|-----------------|---------|------|-------|
| حقل إلزامي | Required Field | | Error | MSG001 |
| تم إضافة صندوق جديد باسم "اسم الصندوق" بواسطة "اسم المستخدم" و تم تعيينك ك"الدور" يرجى استكمال بيانات الصندوق | A new fund is added with name "fund name " by "user name", you are assigned as "Role", kindly complete find info. | استكمال بيانات صندوق/ completing fund data | Notification | MSG002 |
| تم حفظ البيانات بنجاح | Record Saved Successfully | | Success | MSG003 |
| حدث خطأ بالنظام , لم يتم حفظ البيانات | An error is occurred while saving data | | Error | MSG004 |
| تم إضافة صندوق جديد باسم "اسم الصندوق" بواسطة "اسم المستخدم" و تم تعيينك ك"الدور" | A new fund is added with name "fund name " by "user name", you are assigned as "Role" | إنشاء صندوق / Adding a fund | Notification | MSG005 |

---

### [JDWA-186] Create a fund- Legal council

**User Story**
As a legal council, I want create a new fund, So that I can manage funds which I can manage.

**Process Flow**
1. From funds screen user press add new fund
2. System display "new fund" screen
3. User fill Basic info required data
4. User press "Save" button
5. System validates entering required data
   - If any violation is existed, system display and error message MSG001
6. system does below actions:
   - generate fund code (serial no)
   - save fund with status "waiting for adding members/لا يوجد أعضاء"
   - save TNC file in documents under TNC category
   - log Action details, action name (fund creation), date, user (user name), role
   - send notification to fund manager(s) && board secretary(s) (if exist) attached to the fund MSG005, notification activity (fund)
   - display success message MSG002

**Acceptance Criteria**
| Scenario | Given | When | Then |
|----------|-------|------|------|
| Missing required fields | The user has not filled in all mandatory fields | The user clicks "Save" | An error message is displayed indicating which fields are required. MSG001 |
| Successful fund submission | The user is on the add fund screen | The user fills in the required fields and clicks "Save" | The new fund is saved and a confirmation message is displayed.MSG002 |
| Unknown error during submission | The user is connected to the internet | The user clicks "save" | An error message is displayed indicating Unknown error, and the order remains unsaved.MSG004 |

**System Messages**
| Arabic Message | English Message | Subject | Type | MSGID |
|----------------|-----------------|---------|------|-------|
| حقل إلزامي | Required Field | | Error | MSG001 |
| تم حفظ البيانات بنجاح يجب إضافة أعضاء الصندوق حتى تتمكن من التعامل مع أنشطة الصندوق , على الأقل 2 عضو مستقل | Record Saved Successfully You should add board members to be able to perform fund activities, at least 2 independent members. | | Success | MSG002 |
| حدث خطأ بالنظام , لم يتم حفظ البيانات | An error is occurred while saving data | | Error | MSG004 |
| تم إضافة صندوق جديد باسم "اسم الصندوق" بواسطة "اسم المستخدم" و تم تعيينك ك"الدور" | A new fund is added with name "fund name " by "user name", you are assigned as "Role" | إنشاء صندوق / Adding a fund | Notification | MSG005 |

---

### [JDWA-185] Edit fund data- Legal council

**User Story**
As a legal council, I want edit the fund data, So that I can manage my funds.

**Process Flow**
1. From funds screen user press one of fund cards with status, any status except "under construction/تحت الإنشاء"
2. User press "details" button
3. System displays fund details screen
4. User press "edit details" button
5. System display "Edit fund" screen
6. User fill Basic info required data
7. User press "Save" button
8. System validates entering required data
   - If any violation is existed, system display and error message MSG001
9. system does below actions:
   - save fund with the same status and the same fund code
   - if TNC file is changed, save TNC file in documents under TNC category (replace the current file)
   - send notification:
     - in case of adding a new fund manager(s) && board secretary(s) attached to the fund MSG005, notification activity (fund)
     - in case of deleting one of fund manager(s) && board secretary (s) attached to the fund MSG003, notification activity (fund)
     - else notify exist fund manager(s) && legal council (if he is not the editor) && board secretary(s) attached to the fund MSG006, notification activity (fund)
   - display success message MSG002

**Acceptance Criteria**
| Scenario | Given | When | Then |
|----------|-------|------|------|
| Missing required fields | The user has not filled in all mandatory fields | The user clicks "Save" | An error message is displayed indicating which fields are required. MSG001 |
| Successful edit fund data submission | The user is on the edit fund screen | The user fills in the required fields and clicks "Save" | The fund data is saved and a confirmation message is displayed.MSG002 |
| Unknown error during submission | The user is connected to the internet | The user clicks "save" | An error message is displayed indicating Unknown error, and the order remains unsaved.MSG004 |

**System Messages**
| Arabic Message | English Message | Subject | Type | MSGID |
|----------------|-----------------|---------|------|-------|
| حقل إلزامي | Required Field | | Error | MSG001 |
| تم حفظ البيانات بنجاح | Record Saved Successfully | | Success | MSG002 |
| تم إعفاؤك من دور "الدور" في صندوق "اسم الصندوق" بواسطة "اسم المستخدم" | You have been relieved of your role as "role name" in fund "fund name" by "username" | إعفاء من مهام/ relieve of duties | Notification | MSG003 |
| حدث خطأ بالنظام , لم يتم حفظ البيانات | An error is occurred while saving data | | Error | MSG004 |
| تم تعيينك ك"الدور" في صندوق "اسم الصندوق" بواسطة "اسم المستخدم" | you are assigned as "Role" to fund "fund name " by "user name" | إضافة مهام/ adding duties | Notification | MSG005 |
| تم تعديل بيانات الصندوق "اسم الصندوق" بواسطة "اسم المستخدم" | Data is completed for the fund "fund name", by "user name" | تعديل بيانات الصندوق/ edit fund data | Notification | MSG006 |
