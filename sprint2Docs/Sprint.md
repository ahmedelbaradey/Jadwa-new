|<p>[**Board members**](https://arabdt.atlassian.net/browse/JDWA-594) ([JDWA-594](https://arabdt.atlassian.net/browse/JDWA-594)) </p><p><h3>![ref1]**[JDWA-596] [Add fund's board member- legal council/board secretart](https://arabdt.atlassian.net/browse/JDWA-596)** </h3></p>||
| :- | :- |
|||

|` `**Description**  | |
| :-: | :- |

|||
| :- | :- |

|<p>As a legal council/board secretary, I want to add a board member to a fund, So that I can manage board members within my funds.</p><p></p>|
| :- |

|||
| :- | :- |
|**Process flow:**|<p>1. From members screen user press “add member”</p><p>2. System display “new member” screen</p><p>1. User fill Basic info required data,</p><p>1. A. User press “add ” button</p><p>1. System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. system does below actions</p><p>a.      if new member independent && no of active independent board members= 14, System display an error message **MSG006**</p><p>b.      else, save member with status “active”</p><p>c.       if no. of active independent users== 2, system changes fund status to “Active”</p><p>d.      send notification to</p><p>`                                                              `i.      the new board member attached to the fund **MSG002**, notification activity (members)</p><p>`                                                             `ii.      fund manager/legal council/board secretary attached to the fund **MSG007**, notification activity (members)</p><p>`                                                           `iii.      fund manager/legal council/board secretary/board members attached to the fund, **MSG008,** notification activity (fund)</p><p>e.      display success message **MSG003**</p><p><h3>**Alternative**</h3></p><p>NA</p>|
|**Acceptance criteria:**||

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Missing required fields|The user has not filled in all mandatory fields|The user clicks “ Save”|An error message is displayed indicating which fields are required. **MSG001**|
|Successful board member submission|The user is on the add board member screen|The user fills in the required fields and clicks “Save”|The new board member is attached to the fund and a confirmation message is displayed.**MSG003,**|
|Unknown error during submission|The user is connected to the internet|The user clicks “save”|An error message is displayed indicating Unknown error, and the order remains unsaved.**MSG004**|

|||
| :- | :- |
|**Data field validation:**||

|**Field Title**|**Type**|**R**|**NU**|**DC**|**SC**|**SP**|**Min/Max**|**Sample**|||||||||||||||||||
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|**Screen elements**|||||||||||||||||||||||||||
|<p>اسم العضو</p><p>Member name</p>|<p>DDL</p><p>One select</p>|Y| | | | |` `display all users except roles (legal council/board secretary/finance controller/compliance and legal/Managing director)|<p>Source (system)</p><p>Searchable list</p>|||||||||||||||||||
|<p>نوع العضو</p><p>Type</p>|Radio button| | | | | |<p>- independent</p><p>- not independent</p>| |||||||||||||||||||
|<p>هل العضو رئيس مجلس إدارة</p><p>Is board chairman</p>|Toggle| | | | | |Each board has max 1 chairman|` `disabled if board already has a chairman|||||||||||||||||||
|**Screen actions**|||||||||||||||||||||||||||
|<p>أضف</p><p>add</p>|button| | | | | || |||||||||||||||||||
|<p>الغاء</p><p>cancel</p>|button| | | | | ||Back to members list screen|||||||||||||||||||

||<p></p><p></p>|
| :- | :- |
|**System messages:**||

|**Arabic Message**|**English Message**|**Subject**|**Type**|**MSGID**|
| :- | :- | :- | :- | :- |
|حقل إلزامي|Required Field| |Error|MSG001|
|تم حفظ البيانات بنجاح|Record Saved Successfully| |Success|MSG003|
|تم إضافتك كعضو "نوع العضو" في مجلس الصندوق "اسم الصندوق"<br>بواسطة "دور المستخدم" "اسم  المستخدم"|You were added as a board member “member type” to the fund “fund name” by “user name”|إضافة عضو لمجلس الصندوق|Notification|MSG002|
|حدث خطأ بالنظام , لم يتم حفظ البيانات|An error is occurred while saving data| |Error|MSG004|
|لقد وصلت للحد الأقصى لعدد الأعضاء المستقلين للصندوق|You have reached the maximum number of independent board members of this fund| |Notification|MSG006|
|تم إضافة "اسم العضو "كعضو جديد “نوع العضو”في مجلس الصندوق "اسم الصندوق"<br>بواسطة "دور المستخدم" "اسم  المستخدم|A new “member type” member “member name” is added to the fund “fund name” by “role name” “user name”|إضافة عضو لمجلس الصندوق|notification|MSG007|
|تم تفعيل الصندوق "اسم الصندوق" بنجاح و ذلك باتمام اضافة 2 عضو مستقل|“fund name” is successfully activated, as 2 independent members are attached to it|تفعيل الصندوق|Notification|MSG008|

|||
| :- | :- |







|<p>[**Board members**](https://arabdt.atlassian.net/browse/JDWA-594) ([JDWA-594](https://arabdt.atlassian.net/browse/JDWA-594)) </p><p><h3>- **[JDWA-595] [View fund board members- fund manager/ legal council/board secretary**](https://arabdt.atlassian.net/browse/JDWA-595)**</h3></p><p></p>||
| :- | :- |

|` `**Description**  | |
| :-: | :- |

|||
| :- | :- |

|<p><h3>**User Story**</h3></p><p>As a fund manager/legal council/board secretary, I want to investigate fund board member’s So that I can manage fund members</p><p></p>|
| :- |

|<h3></h3>||
| :- | :- |
|**Process flow:**|<p>1. In “fund details” screen User clicks “members”</p><p>1. System displays board members screen, which display all board members with all status, attached to this fund as cards</p><p>a.      If there are no members, system display **MSG001**</p><p>1. System display list of board members cards ordered DESC by last update date</p><p>1. Each board members data is displayed in a separated card</p><p> </p><p><h3>**Alternative 1**</h3></p><p>NA</p>|
|**Acceptance criteria:**||

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Successful board members display|The user is on the members screen|The user clicks screen link|The board members list is displayed|
|Unknown error during displaying data|The user is connected to the internet|The user clicks screen link|An error message is displayed indicating unknown error, and the order remains not done.**MSG002**|

|

**Fund items/for each activity**

activity name

Label
|

Notification icon/No. of notifications for this activity for the logged in user

 |

button

 |

Y

 |  |  |  |  |  |

 Filters fund notification list

 |
|

No. of items for this activity

 |

Label

 |

Y

 |  |  |  |  |  |  |
|||
| :- | :- |




|<p>[**Funds Management**](https://arabdt.atlassian.net/browse/JDWA-180) ([JDWA-180](https://arabdt.atlassian.net/browse/JDWA-180)) </p><p><h3>![ref1]**[JDWA-996] [Navigate Fund details- Active](https://arabdt.atlassian.net/browse/JDWA-996)** </h3></p>|
| :- |


|` `**Description**  | |
| :-: | :- |

|<p>As user, I want navigate the fund details where its status is active, So that I can manage my fund activities.</p><p></p>|
| :- |

|**Process flow:**|<p>1. From funds screen user press one of fund cards with status, active</p><p>1. User press “details” button</p><p>1. System displays fund details screen</p><p>a.      If fund status is active</p><p>b.      All fund activities are enabled</p><p>1. Fund details include resolutions, assessments, documents, meetings, members, displayed as clickable cards</p>|
| :- | :- |
|**Acceptance criteria:**||

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Successful fund details display|The user is on the fund details screen|The user clicks screen link|The fund details is displayed|
|Unknown error during displaying data|The user is connected to the internet|The user clicks screen link|An error message is displayed indicating unknown error, and the order remains not done.**MSG002**|
| | | | |

|||
| :- | :- |
|**Data field validation:**||
|**System messages:**||

|**Arabic Message**|**English Message**|**Type**|**MSGID**|
| :- | :- | :- | :- |
|حدث خطأ بالنظام , لم يتمكن من عرض البيانات|An error is occurred, can’t display data|Error|MSG002|

|||
| :- | :- |



|<p>[**Resolutions**](https://arabdt.atlassian.net/browse/JDWA-504) ([JDWA-504](https://arabdt.atlassian.net/browse/JDWA-504)) </p><p><h3>![ref1]**[JDWA-511] [Create a resolution- fund manager** ](https://arabdt.atlassian.net/browse/JDWA-511)** </h3></p>|||
| :- | :- | :- |
|` `**Description**  | ||

|<p><h3>**User Story**</h3></p><p>As a fund manager, I want create a new resolution, So that I can manage resolutions within my funds.</p><p><h3>**Process Flow**</h3></p><p>1. From resolutions screen user press add new resolution</p><p>1. System display “new resolution” screen</p><p>1. User fill Basic info required data,</p><p>1. A. User press “Save as draft” button</p><p>1. System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. system does below actions</p><p>a.      generate resolution code, format (fund code/resolution year/resolution no.)</p><p>b.      save resolution with status “draft”</p><p>c.       display success message **MSG003**</p><p><h3>**Alternative**</h3></p><p>1. B. User press “send” button</p><p>1. System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. system does below actions</p><p>a.      generate resolution code (serial no), format (fund code/resolution year/resolution no.)</p><p>b.      save resolution with status “pending”</p><p>c.       log resolution Action details, action name (resolution creation), date, user (user name), role</p><p>d.      send notification to</p><p>`                                                              `i.      legal council && board secretary (if exist) attached to the fund **MSG002**, notification activity (resolution)</p><p>e.      display success message **MSG003**</p><p><h3>**Acceptance Criteria**</h3></p>|
| :- |

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Missing required fields|The user has not filled in all mandatory fields|The user clicks “ send”/save as draft|An error message is displayed indicating which fields are required. **MSG001**|
|Successful resolution submission|The user is on the add resolution screen|The user fills in the required fields and clicks “send”/save as|The new resolution is saved and a confirmation message is displayed.**MSG003,**|
|Unknown error during submission|The user is connected to the internet|The user clicks “send/save as draft”|An error message is displayed indicating Unknown error, and the order remains unsaved.**MSG004**|

|<h3>**Data Field Validation Criteria**</h3>|
| :- |

|**Field Title**|**Title En**|**Type**|**R**|**NU**|**DC**|**SC**|**SP**|**Min**|**Max**|**Condition**|**Sample Data Ar**|**Sample Data En**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|**Screen elements**|||||||||||||
|تاريخ القرار|Resolution date|Date |Y|N|N|N|N|Min (fund initiation date)|Max (today)||Gregorian/Hijri||
|وصف القرار|Description|Text|N|N|N|Y|Y||Max (500)char||||
|نوع القرار|Type|DDL ,One select|Y||||||||استحواذ – تخارج- بيع – توزيع أرباح – تمديد مدة الصندوق – تعديل شروط واحكام الصندوق – الموافقة على القوائم المالية – تعيين مقدمي خدمات – موافقة على شروط واحكام الصندوق – أخرى||
|نوع القرار المضاف|New type|Text|Y|N|N|N|Y|||Appears in case user select (اخرى)|||
|ملف القرار|Resolution file|File|Y||||||1 file ,Size up to (10 Mb)|File type (PDF)|||
|الية التصويت  للقرار|Voting  methodology|Option|Y|||||||editable , Display value as fund voting type|جميع الأعضاء -  أغلبية الأعضاء|All members - Majority|
|احتساب نتيجة التصويت  للاعضاء|members Voting result|Option|N||||||||جميع البنود - أغلبية البنود|All items -  Majority of items (default selection)|
|**Screen actions**|||||||||||||
|Save as draft||Button|||||||||||
|send||button|||||||||||
|cancel||button|||||||||Back to resolutions list screen ||

|<p><h3></h3></p><p><h3><a name="systemmessage"></a>**System message**</h3></p>|
| :- |

|**Arabic Message**|**English Message**|**Subject**|**Type**|**MSGID**|
| :- | :- | :- | :- | :- |
|حقل إلزامي|Required Field| |Error|MSG001|
|تم حفظ البيانات بنجاح|Record Saved Successfully| |Success|MSG003|
|تم إضافة قرار جديد ضمن فعاليات صندوق "اسم الصندوق" بواسطة مدير الصندوق "اسم المستخدم” يرجى استكمال بيانات القرار|A new resolution is added attached to fund "fund name " by fund manager “user name”, kindly complete resolution info.|استكمال بيانات القرار/ completing resolution data|Notification|MSG002|
|حدث خطأ بالنظام , لم يتم حفظ البيانات|An error is occurred while saving data| |Error|MSG004|
||||||

||
| :- |














|<p>[**Resolutions**](https://arabdt.atlassian.net/browse/JDWA-504) ([JDWA-504](https://arabdt.atlassian.net/browse/JDWA-504)) </p><p><h3>![ref1]**[JDWA-588] [view draft/pending/cancelled resolution details- fund manager](https://arabdt.atlassian.net/browse/JDWA-588)** </h3></p>|||
| :- | :- | :- |
|` `**Description**  | ||

|<p><h3>**User Story**</h3></p><p>As a fund manager, I want to be able to view draft/pending/cancelled resolution details , So that I can track my resolution details.</p><p><h3>**Process Flow**</h3></p><p>1. From resolutions screen user press details link for one of displayed resolutions with status “draft/pending/cancelled”</p><p>1. System display “details” screen</p><p>1. User can review resolution basic info</p><p><h3>**Acceptance Criteria**</h3></p>|
| :- |

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Successful resolution display|The user is on the resolution details screen|The user clicks “details”|The resolution details are displayed|
|Unknown error during submission|The user is connected to the internet|The user clicks “details”|An error message is displayed indicating Unknown error, and the order remains unsaved.**MSG001**|

|<h3>**System messages**</h3>|
| :- |

|**Arabic Message**|**English Message**|**Subject**|**Type**|**MSGID**|
| :- | :- | :- | :- | :- |
|حدث خطأ بالنظام , لم يتمكن من عرض البيانات|An error is occurred while displaying data| |Error|MSG001|

||
| :- |










|<p>[**Resolutions**](https://arabdt.atlassian.net/browse/JDWA-504) ([JDWA-504](https://arabdt.atlassian.net/browse/JDWA-504)) </p><p><h3>![ref1]**[JDWA-509] [Edit a draft/pending resolution- fund manager** ](https://arabdt.atlassian.net/browse/JDWA-509)** </h3></p>|||
| :- | :- | :- |
|` `**Description**  | ||

|<p><h3>**User Story**</h3></p><p>As a fund manager, I want to edit a draft/pending resolution, So that I can manage resolutions within my funds.</p><p><h3>**Process Flow**</h3></p><p>1. From resolutions screen user press edit button for one of displayed resolutions with status “draft/pending”, or press edit button in details screen</p><p>1. System display “edit resolution” screen</p><p>1. User fill Basic info required data,</p><p>1. A. in case of draft, User press “Save as draft” button</p><p>1. System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. system does below actions</p><p>a.      save resolution with the same resolution code</p><p>b.      save resolution with status “draft”</p><p>c.       display success message **MSG003**</p><p><h3>**Alternative 1**</h3></p><p>1. B. in case of Draft, User press “send” button</p><p>1. System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. system does below actions</p><p>a.      save resolution with the same resolution code</p><p>a.      save resolution with status “pending”</p><p>b.      log Action details, action name (resolution creation), date, user (user name), role</p><p>c.       send notification to</p><p>`                                                              `i.      legal council && board secretary (if exist) attached to the fund **MSG002**, notification activity (resolution)</p><p>d.      display success message **MSG003**</p><p><h3>**Alternative 2**</h3></p><p>1. C. in case of pending, User press “send” button</p><p>1. System validates entering required data</p><p>b.      If any violation is existed, system display and error message **MSG001**</p><p>1. system does below actions</p><p>a.      save resolution with the same resolution code</p><p>b.      save resolution with status “pending” as it</p><p>c.       log resolution Action details, action name (resolution edit), date, user (user name), role</p><p>d.      send notification to</p><p>a.      legal council && board secretary (if exist) attached to the fund **MSG005**, notification activity (resolution)</p><p>e.      display success message **MSG003**</p><p><h3>**Acceptance Criteria**</h3></p>|
| :- |

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Missing required fields|The user has not filled in all mandatory fields|The user clicks “ send”/save as draft|An error message is displayed indicating which fields are required. **MSG001**|
|Successful resolution submission|The user is on the add resolution screen|The user fills in the required fields and clicks “send”/save as|The new resolution is saved and a confirmation message is displayed.**MSG003,**|
|Unknown error during submission|The user is connected to the internet|The user clicks “send/save as draft”|An error message is displayed indicating Unknown error, and the order remains unsaved.**MSG004**|

|<h3>**Data Field Validation Criteria**</h3>|
| :- |

|**Field Title**|**EN Title**|**Type**|**R**|**NU**|**DC**|**SC**|**SP**|**Min**|**Max**|**Condition**|**Sample**|**Sample Data Ar**|**Sample Data En**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|**Screen elements**||||||||||||||
|كود القرار|Resolution code|Label|Y||||||||Autogenerated ,format (fund code/resolution year/resolution no.)|||
|تاريخ القرار|Resolution date|Date |Y|N|N|N|N|Min (fund initiation date)|Max (today)||Gregorian/Hijri|||
|وصف القرار|Description|Text|N|N|N|Y|Y|||||||
|نوع القرار|Type|DDL,One select|Y||||||||استحواذ – تخارج- بيع – توزيع أرباح – تمديد مدة الصندوق – تعديل شروط واحكام الصندوق – الموافقة على القوائم المالية – تعيين مقدمي خدمات – موافقة على شروط واحكام الصندوق – أخرى|استحواذ – تخارج- بيع – توزيع أرباح – تمديد مدة الصندوق – تعديل شروط واحكام الصندوق – الموافقة على القوائم المالية – تعيين مقدمي خدمات – موافقة على شروط واحكام الصندوق – أخرى||
|نوع القرار المضاف|New type|Text|Y|N|N|N|Y|||Appears in case user select (اخرى)||||
|ملف القرار|Resolution file|File|Y|||||1 file|1 file ,File type (PDF),Size up to (10 Mb)|||||
|الية التصويت  للقرار|Voting methodology|Option|Y|||||||||جميع الأعضاء - أغلبية الأعضاء|All members -Majority|
|احتساب نتيجة التصويت  للاعضاء|members Voting result|Option|Y|||||||||جميع البنود - أغلبية البنود|All items -  Majority of items|
|الحالة|status|label||||||||||||
|**Screen actions**||||||||||||||
|Save as draft||Button|||||||||Appears in case of draft|||
|send||button||||||||||||
|cancel||button|||||||||Back to resolutions list screen|||

|<h3>**System messages**</h3>|
| :- |

|**Arabic Message**|**English Message**|**Subject**|**Type**|**MSGID**|
| :- | :- | :- | :- | :- |
|حقل إلزامي|Required Field| |Error|MSG001|
|تم حفظ البيانات بنجاح|Record Saved Successfully| |Success|MSG003|
|تم إضافة قرار جديد ضمن فعاليات صندوق "اسم الصندوق" بواسطة مدير الصندوق "اسم المستخدم” يرجى استكمال بيانات القرار|A new resolution is added attached to fund "fund name " by fund manager “user name”, kindly complete resolution info.|استكمال بيانات القرار/ completing resolution data|Notification|MSG002|
|تم تعديل بيانات القرار الجديد "كود القرار" ضمن فعاليات صندوق "اسم الصندوق" بواسطة مدير الصندوق "اسم المستخدم” يرجى استكمال بيانات القرار|A new resolution  with “resolution code” is updated which attached to fund "fund name " by fund manager “user name”, kindly complete resolution info.|استكمال بيانات القرار/ completing resolution data|Notification|MSG005|
|حدث خطأ بالنظام , لم يتم حفظ البيانات|An error is occurred while saving data| |Error|MSG004|

||
| :- |













|<p>[**Resolutions**](https://arabdt.atlassian.net/browse/JDWA-504) ([JDWA-504](https://arabdt.atlassian.net/browse/JDWA-504)) </p><p><h3>![ref1]**[JDWA-508] [Cancel a pending resolution- fund manager** ](https://arabdt.atlassian.net/browse/JDWA-508)** </h3></p>|||
| :- | :- | :- |
|` `**Description**  | ||

|<p><h3>**User Story**</h3></p><p>As a fund manager, I want to Cancel a pending resolution, So that I can manage resolutions within my funds.</p><p><h3>**Process Flow**</h3></p><p>1. From resolutions screen user press cancel button for one of displayed resolutions with status “pending”</p><p>1. System display a confirmation message **MSG001**</p><p>1. If user press “cancel the resolution” button, system does below actions</p><p>a.      changes resolution status to canceled.</p><p>b.      log resolution Action details, action name (resolution cancelation), date, user (user name), role</p><p>c.       display message to user **MSG003**</p><p>d.      send notification to</p><p>`                                                              `i.      legal council && board secretary (if exist) attached to the fund **MSG004**, notification activity (resolution)</p><p>1. If user press “No” button, system keep the resolution status as it</p><p><h3>**Acceptance Criteria**</h3></p>|
| :- |

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Successful resolution cancel|The user is on the resolution list screen|The user clicks cancel button|A confirmation message is displayed **MSG001**|
|Unknown error during updating|The user is connected to the internet|The user clicks “cancel”|An error message is displayed indicating Unknown error, and the order remains unsaved.**MSG002**|

|<p><h3></h3></p><p><h3>**System messages**</h3></p>|
| :- |

|**Arabic Message**|**English Message**|**Subject**|**Type**|**MSGID**|
| :- | :- | :- | :- | :- |
|هل انت متاكد أنك تريد الغاء هذا القرار (رجوع\الغاء القرار)|Are you sure you want to cancel this resolution? (back/cancel the resolution) buttons| |Confirmation|MSG001|
|حدث خطأ بالنظام , لم يتم تحديث البيانات|An error is occurred while updating data| |Error|MSG002|
|تم حفظ البيانات بنجاح|Record Saved Successfully| |Success|MSG003|
|تم الغاء القرار الجديد "كود القرار" ضمن فعاليات صندوق "اسم الصندوق" بواسطة مدير الصندوق "اسم المستخدم”|A new resolution  with “resolution code” is cancelled which attached to fund "fund name " by fund manager “user name”|الغاء القرار|Notification|MSG004|

||
| :- |































|<p>[**Resolutions**](https://arabdt.atlassian.net/browse/JDWA-504) ([JDWA-504](https://arabdt.atlassian.net/browse/JDWA-504)) </p><p><h3>![ref1]**[JDWA-510] [Delete a draft resolution- fund manager** ](https://arabdt.atlassian.net/browse/JDWA-510)** </h3></p>|||
| :- | :- | :- |
|` `**Description**  | ||

|<p><h3>**User Story**</h3></p><p>As a fund manager, I want delete a draft resolution, So that I can manage resolutions within my funds.</p><p><h3>**Process Flow**</h3></p><p>1. From resolutions screen user press delete button for one of displayed resolutions with status “draft”</p><p>1. System display a confirmation message **MSG001**</p><p>1. If user press “delete the resolution” button, system does below actions and display a success message MSG003</p><p>1. If user press “back” button, system keep the resolution data</p><p><h3>**Acceptance Criteria**</h3></p>|
| :- |

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Successful resolution deletes|The user is on the resolution screen|The user clicks delete button|A confirmation message is displayed **MSG001**|
|Unknown error during delete|The user is connected to the internet|The user clicks “delete”|An error message is displayed indicating Unknown error, and the order remains unsaved.**MSG002**|

|<h3><a name="%c2%a0"></a>**System messages**</h3>|
| :- |

|**Arabic Message**|**English Message**|**Subject**|**Type**|**MSGID**|
| :- | :- | :- | :- | :- |
|هل انت متاكد أنك تريد حذف هذا العنصر (رجوع\حذف القرار)|Are you sure you want to delete this item? (Back/delete the resolution) buttons| |Confirmation|MSG001|
|حدث خطأ بالنظام , لم يتم حذف البيانات|An error is occurred while deleting data| |Error|MSG002|
|تم حذف العنصر بنجاح|Item is deleted successfully| | |MSG003|

||
| :- |



|<p>[**Resolutions**](https://arabdt.atlassian.net/browse/JDWA-504) ([JDWA-504](https://arabdt.atlassian.net/browse/JDWA-504)) </p><p><h3>![ref1]**[JDWA-584] [view pending&cancelled resolution details- legal council/board secretary](https://arabdt.atlassian.net/browse/JDWA-584)** </h3></p>|||
| :- | :- | :- |
|` `**Description**  | ||

|<p><h3>**User Story**</h3></p><p>As a legal council/board secretary, I want to be able to view pending&cancelled resolution details , So that I can track my resolution details.</p><p><h3>**Process Flow**</h3></p><p>1. From resolutions screen user press details link for one of displayed resolutions with status “pending/Cancelled”</p><p>1. System display “details” screen</p><p>1. User can review resolution basic info</p><p><h3>**Acceptance Criteria**</h3></p>|
| :- |

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Successful resolution display|The user is on the resolution details screen|The user clicks “details”|The resolution details are displayed|
|Unknown error during submission|The user is connected to the internet|The user clicks “confirm”/reject|An error message is displayed indicating Unknown error, and the order remains unsaved.**MSG001**|

|<p><h3>**Data Field Validation Criteria**</h3></p><p><h3>**System messages**</h3></p>|
| :- |

|**Arabic Message**|**English Message**|**Subject**|**Type**|**MSGID**|
| :- | :- | :- | :- | :- |
|حدث خطأ بالنظام , لم يتمكن من عرض البيانات|An error is occurred while displaying data| |Error|MSG001|

||
| :- |







|<p>[**Resolutions**](https://arabdt.atlassian.net/browse/JDWA-504) ([JDWA-504](https://arabdt.atlassian.net/browse/JDWA-504)) </p><p><h3>![ref1]**[JDWA-506] [Complete resolution data- Basic info -Legal council / board secretary](https://arabdt.atlassian.net/browse/JDWA-506)** </h3></p>|||
| :- | :- | :- |
|` `**Description**  | ||

|<p><h3>**User Story**</h3></p><p>As a legal council / board secretary, I want to complete the new resolution data, So that I can go</p><p>` `forward in new resolution process.</p><p><h3>**Process Flow**</h3></p><p>1. From resolutions screen user press ”complete” button for  one of displayed resolutions with status “pending/completing data”</p><p>1. System display “complete resolution data” screen</p><p>1. User fill Basic info required data,</p><p>1. A. User press “send” button</p><p>1. System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. System validates if no items were added.</p><p>a.      If no items added, system display a confirmation message **MSG006**</p><p>1. system does below actions</p><p>a.      save resolution with status “waiting for confirmation”</p><p>b.      log resolution Action details, action name (resolution data complete), date, user (user name), role</p><p>c.       send notification</p><p>`                                                              `i.      notify fund manager(s), attached to the fund **MSG003**, notification activity (resolution),</p><p>`                                                             `ii.      legal council, if editor is board secretary **MSG003**, </p><p>notification activity (resolution),</p><p>`                                                           `iii.      board secretary(s), attached to the fund, if editor is legal council **MSG003**, notification activity (resolution),</p><p>d.      display success message **MSG002**</p><p><h3>**Alternative**</h3></p><p>1. B. user press “save” button</p><p>1. System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. system does below actions</p><p>a.      save resolution with status “completing data”</p><p>b.      display success message **MSG002**</p><p><h3>**Acceptance Criteria**</h3></p>|
| :- |

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Missing required fields|The user has not filled in all mandatory fields|The user clicks “send for confirmation”|An error message is displayed indicating which fields are required. **MSG001**|
|Successful complete resolution data submission|The user is on the complete resolution data screen|The user fills in the required fields and clicks “send”|The fund data is saved and a confirmation message is displayed.**MSG002,**|
|Unknown error during submission|The user is connected to the internet|The user clicks “send/save”|An error message is displayed indicating Unknown error, and the order remains unsaved.**MSG004**|

|<h3>**Data Field Validation Criteria**</h3>|
| :- |

|**Field Title**|**Ttile En**|**Type**|**R**|**NU**|**DC**|**SC**|**SP**|**Min/Max**|**Sample**|||||||||
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|**Resolution items**||||||||||||||||||
|إضافة بند|Add item|button||||||||||||||||
|**items list**||||||||||||||||||
|عنوان البند|Item title.|label||||||||||||||||
|الوصف|Description||||||||Maxb(200)char|||||||||
|يوجد تعارض|Conflict|Yes/no|||||||disabled|||||||||
|الاعضاء|members|Link (counter)|||||||appears if conflict is exist|||||||||
|تعديل|Edit|button||||||||||||||||
|حذف|Delete|button||||||||||||||||
|**Field Title**|**Title En**|**Type**|**R**|**NU**|**DC**|**SC**|**SP**|**Min/Max**|**Sample**|||||||||
|**Add/Edit item pop up**||||||||||||||||||
|عنوان البند|Item title.|Label|R||||||autogenerated ,Item1 to item N|||||||||
|الوصف|Description|Text|N|N|N|Y|Y|Max (500 char)||||||||||
|تعارض المصالح مع الاعضاء|Conflict of interest with some members|Check box|N||||||Not selected by default|||||||||
|الاعضاء|Members|DDL Multi select|N||||||Enabled && mandatory if conflict is checked|||||||||
|||Multi select||||||||||||||||
|**Actions**||||||||||||||||||
|أضف|add|||||||||||||||||
|الغاء|cancel||||||||Return to resolution details screen|||||||||
|**Conflict members pop up**||||||||||||||||||
|رقم|Serial|Label||||||||||||||||
|||||||||||||||||||
|||||||||||||||||||
|اسم العضو|Member name|Label||||||||||||||||
|إغلاق|close|Button|||||||Return to resolution details screen|||||||||
|||||||||||||||||||
|**Screen actions**||||||||||||||||||
|حفظ|Save|button||||||||||||||||

|<h3>**System messages**</h3>|
| :- |

|**Arabic Message**|**English Message**|**Subject**|**Type**|**MSGID**|
| :- | :- | :- | :- | :- |
|حقل إلزامي|Required Field|\*|Error|MSG001|
|تم حفظ البيانات بنجاح|Record Saved Successfully|\*|Success|MSG002|
|تم استكمال بيانات القرار رقم "كود القرار" في الصندوق "اسم الصندوق" بواسطة "اسم المستخدم"|Data is completed for the resolution code “resolution code attached to fund “fund name”, by “user name”|استكمال بيانات القرار/ resolution data completion|Notification|MSG003|
|حدث خطأ بالنظام , لم يتم حفظ البيانات|An error is occurred while saving data| |Error|MSG004|
|الخروج من الصفحة يعنى عدم حفظ جميع البيانات التي قمت بإضافتها (نعم\لا)|Exit from screen will not saving all data(yes/no)| |Confirmation|MSG005|
|في حالة عدم إضافة بنود القرار سيتم التصويت على القرار ككل (نعم\لا)|If the resolution items are not added, the resolution as a whole will be voted on (yes/no)| |Confirmation|MSG006|

||
| :- |


|<p>[**Resolutions**](https://arabdt.atlassian.net/browse/JDWA-504) ([JDWA-504](https://arabdt.atlassian.net/browse/JDWA-504)) </p><p><h3>![ref1]**[JDWA-567] [Edit resolution data- Basic info -Legal council / board secretary](https://arabdt.atlassian.net/browse/JDWA-567)** </h3></p>||
| :- | :- |
|||

|` `**Description**  | |
| :- | :- |

|||
| :- | :- |

|<p><h3>**User Story**</h3></p><p>As a legal council / board secretary, I want to Edit the resolution data, So that I update the resolution and return back in resolution creation process.</p><p></p>|
| :- |

|<p></p><p>**Data field validation:**</p>||
| :- | :- |

|**Title Ar**|**Title En**|**Type**|**Rrequired**|**NULLABLE**|**Decimals**|**Special Chars**|**Allow Spaces**|**Min**|**Max**|**Condition**|**Sample**|**Sample Data Ar**|**Sample Data En**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|**Screen elements**||||||||||||||
|كود القرار|Resolution code|Label|Y||||||||Autogenerated ,format (fund code/resolution year/resolution no.)|||
|تاريخ القرار|Resolution date|Date |Y|N|N|N|N|Min (fund initiation date)|Max (today)||Gregorian/Hijri|||
|وصف القرار|Description|Text|N|N|N|Y|Y|||||||
|نوع القرار|Type|DDL,One select|Y||||||||استحواذ – تخارج- بيع – توزيع أرباح – تمديد مدة الصندوق – تعديل شروط واحكام الصندوق – الموافقة على القوائم المالية – تعيين مقدمي خدمات – موافقة على شروط واحكام الصندوق – أخرى|استحواذ – تخارج- بيع – توزيع أرباح – تمديد مدة الصندوق – تعديل شروط واحكام الصندوق – الموافقة على القوائم المالية – تعيين مقدمي خدمات – موافقة على شروط واحكام الصندوق – أخرى||
|نوع القرار المضاف|New type|Text|Y|N|N|N|Y|||Appears in case user select (اخرى)||||
|ملف القرار|Resolution file|File|Y|||||1 file|1 file ,File type (PDF),Size up to (10 Mb)|||||
|الية التصويت  للقرار|Voting methodology|Option|Y|||||||||جميع الأعضاء - أغلبية الأعضاء|All members -Majority|
|احتساب نتيجة التصويت  للاعضاء|members Voting result|Option|Y|||||||||جميع البنود - أغلبية البنود|All items -  Majority of items|
|الحالة|status|label||||||||||||
|**Screen actions**||||||||||||||
|Save as draft||Button||||||||Appears in case of draft||||
|send||button||||||||||||
|cancel||button||||||||Back to resolutions list screen||||
|Download||button||||||||Download resolution file||||
|Delete||button|button|||||||Delete resolution file from screen||||

|||
| :- | :- |
|**Process flow:**|<p><h3>**Process Flow**</h3></p><p>1. From resolutions screen user press edit button for one of displayed resolutions with status “waiting for confirmation/confirmed/ rejected /voting in progress/Approved/Not Approved” or press edit button from resolution details screen</p><p>1. System display “Edit resolution data” screen</p><p>1. User edit Basic info required data,</p><p>1. User press “send for confirmation” button</p><p>1. A.  If resolution status is “waiting for confirmation/confirmed/rejected”, System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. System validates if no items were added.</p><p>a.      If no items added, system display a confirmation message **MSG010**</p><p>1. system does below actions</p><p>a.      save resolution with status “waiting for confirmation”</p><p>b.      log resolution Action details, action name (resolution data update), date, user (user name), role</p><p>c.       send notification</p><p>`                                                              `i.      notify fund manager(s)/legal council/other board secretary (if editor is one of board secretary), attached to the fund **MSG003**, notification activity (resolution),</p><p>`                                                              `ii.  notify fund manager(s)/board secretary (if editor is legal council), attached to the fund **MSG003**, notification activity (resolution),</p><p>d.      display success message **MSG002**</p><p><h3>**Alternative 1**</h3></p><p>1. B   If resolution status is “Voting in progress”, System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. system display a confirmation message **MSG006**, yes/no buttons</p><p>1. If user press yes, system does below actions</p><p>a.      Suspend active voting</p><p>b.      save resolution with status “waiting for confirmation”</p><p>c.       log resolution Action details, action name (resolution vote suspend), date, user (user name), role</p><p>d.      log resolution Action details, action name (resolution data update), date, user (user name), role</p><p>e.      send notification</p><p>`                                                              `i.      notify fund manager(s) / legal council & board secretary/ board members, attached to the fund **MSG007**, notification activity (resolution),</p><p>f.        display success message **MSG002**</p><p><h3>**Alternative 2**</h3></p><p>1. From resolutions screen user press edit button for one of displayed resolutions with status “Approved/not approved” or press edit button from resolution details screen</p><p>1. System display a confirmation message **MSG008**, yes/No</p><p>1. If user press yes, System display “Edit resolution data” screen  filled with resolution data, except fund code</p><p>1. User edit Basic info required data,</p><p>1. User press “send for confirmation” button</p><p>1. System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. system does below actions</p><p>a.      generates new resolution code</p><p>b.      relate between old resolution and new one</p><p>c.       save resolution with status “waiting for confirmation”</p><p>d.      log resolution Action details, action name (referral resolution creation), date, user (user name), role</p><p>e.      send notification</p><p>`                                                              `i.      notify fund manager(s)/legal council/board secretary, attached to the fund **MSG009**, notification activity (resolution),</p><p>f.        display success message **MSG002**</p>|
|**Acceptance criteria:**||

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Missing required fields|The user has not filled in all mandatory fields|The user clicks “send for confirmation”|An error message is displayed indicating which fields are required. **MSG001**|
|Successful edit resolution data submission|The user is on the edit resolution screen|The user fills in the required fields and clicks “send for confirmation”|The fund data is saved and a confirmation message is displayed.**MSG002,**|
|Unknown error during submission|The user is connected to the internet|The user clicks “send for confirmation”|An error message is displayed indicating Unknown error, and the order remains unsaved.**MSG004**|

|||
| :- | :- |
|||
|**System messages:**||

|**Arabic Message**|**English Message**|**Subject**|**Type**|**MSGID**|
| :- | :- | :- | :- | :- |
|حقل إلزامي|Required Field|\*|Error|MSG001|
|تم حفظ البيانات بنجاح|Record Saved Successfully|\*|Success|MSG002|
|تم تحديث بيانات القرار رقم "كود القرار" في الصندوق "اسم الصندوق" بواسطة ”الدور” "اسم المستخدم"|Data is updated for the resolution code “resolution code attached to fund “fund name”, by “role” “user name”|تحديث بيانات القرار/ resolution data update|Notification|MSG003|
|حدث خطأ بالنظام , لم يتم حفظ البيانات|An error is occurred while saving data| |Error|MSG004|
|الخروج من الصفحة يعنى عدم حفظ جميع البيانات التي قمت بإضافتها (نعم\لا)|Exit from screen will not saving all data(yes/no)| |Confirmation|MSG005|
|هذا القرار في مرحلة التصويت, في حالة التعديل على البيانات سوف يتم إلغاء التصويت و إشعار أعضاء الصندوق بذلك|This resolution voting is in progress, updating its data will cancel the vote, and system will notify board members| |Confirmation|MSG006|
|تم تحديث بيانات القرار رقم "كود القرار" في الصندوق "اسم الصندوق" بواسطة “الدور” "اسم المستخدم"  مما يترتب عليه إلغاء التصويت الجارى على القرار|Data is updated for the resolution code “resolution code attached to fund “fund name”, by “role”“user name”, which impacts resolution voting cancelation|تحديث بيانات القرار/ resolution data update|Notification|MSG007|
|تحديث ييانات القرار المعتمد\غير معتمد يترتب عليه إنشاء قرار جديد مرتبط بهذا القرار|Updating approved/not approved resolutions initiates a new resolution related to this one  | |Confirmation|MSG008|
|تم إضافة قرار جديد ضمن فعاليات صندوق "اسم الصندوق" بواسطة "دور المستخدم" "اسم المستخدم” كتحديث على القرار "رقم القرار"|A new resolution is added to fund "fund name " by  “user role” “user name”, as an update on resolution no. “resolution no.”|إضافة قرار/ adding a resolution|Notification|MSG009|
|في حالة عدم إضافة بنود القرار سيتم التصويت على القرار ككل (نعم\لا)|If the resolution items are not added, the resolution as a whole will be voted on (yes/no)| |Confirmation|MSG010|

|||
| :- | :- |


|<p>[**Resolutions**](https://arabdt.atlassian.net/browse/JDWA-504) ([JDWA-504](https://arabdt.atlassian.net/browse/JDWA-504)) </p><p><h3>![ref1]**[JDWA-566] [Edit resolution data- resolution items and conflicts -Legal council / board secretary](https://arabdt.atlassian.net/browse/JDWA-566)** </h3></p><p></p>||
| :- | :- |

|` `**Description**  | |
| :-: | :- |

|||
| :- | :- |

|<p>As a legal council / board secretary, I want to complete the new resolution data; attachments, So that I can go forward in new resolution process.</p><p></p>|
| :- |

|<h3></h3>||
| :- | :- |
|**Process flow:**|<p>1. In resolution details screen, user press “add item” button</p><p>1. System displays “Add item" pop up</p><p>1. User enter required data, then press “Add” button</p><p>1. System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. System add item to screen</p><p>1. User press “send for confirmation” button</p><p>1. A.  If resolution status is “waiting for confirmation/confirmed/rejected”, System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. System validates if no items were added.</p><p>a.      If no items added, system display a confirmation message **MSG010**</p><p>1. system does below actions</p><p>a.      save resolution with status “waiting for confirmation”</p><p>b.      log resolution Action details, action name (resolution data update), date, user (user name), role</p><p>c.       send notification</p><p>`                                                              `i.     notify fund manager(s)/legal council/other board secretary (if editor is one of board secretary), attached to the fund **MSG003**, notification activity (resolution),</p><p>`                                                              `ii.  notify fund manager(s)/board secretary (if editor is legal council), attached to the fund **MSG003**, notification activity (resolution),</p><p>d.      display success message **MSG002**</p><p><h3>**Alternative 1**</h3></p><p>1. B   If resolution status is “Voting in progress”, System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. system display a confirmation message **MSG006**, yes/no buttons</p><p>10\.  If user press yes, system does below actions</p><p>a.      Suspend active voting</p><p>b.      save resolution with status “waiting for confirmation”</p><p>c.       log resolution Action details, action name (resolution vote suspend), date, user (user name), role</p><p>d.      log resolution Action details, action name (resolution data update), date, user (user name), role[[MR1]|] </p><p>e.      send notification</p><p>`                                                              `i.      notify fund manager(s) / legal council & board secretary/ board members, attached to the fund **MSG007**, notification activity (resolution),</p><p>f.        display success message **MSG002**</p><p><h3>**Alternative 2**</h3></p><p>1. From resolutions screen user press edit button for one of displayed resolutions with status “Approved/not approved” or press edit button from resolution details screen</p><p>1. System display a confirmation message **MSG008**, yes/No</p><p>1. If user press yes, System display “Edit resolution data” screen  filled with resolution data, except fund code</p><p>1. User can view resolution items and members who have conflict with item</p><p>1. user press “add item” button</p><p>1. System displays “Add item" pop up</p><p>1. User enter required data, then press “Add” button</p><p>1. System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. System add item to screen</p><p>10\.  User press “send for confirmation” button</p><p>11\.  System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>12\.  system does below actions</p><p>a.      generates new resolution code</p><p>b.      relate between old resolution and new one</p><p>c.       save resolution with status “waiting for confirmation”</p><p>d.      log resolution Action details, action name (resolution creation), date, user (user name), role</p><p>e.      send notification</p><p>`                                                              `i.      notify fund manager(s)/legal council/board secretary, attached to the fund **MSG009**, notification activity (resolution),</p><p>f.        display success message **MSG002**</p><p><h3><a name="alternative3%28deleteitem%29"></a>**Alternative 3 (delete item)**</h3></p><p>1. user press delete button for one of displayed item</p><p>1. system delete item from the screen and re-order items no. to correct the count (if we have 5 items, and user deletes item 3, the item 4 title will be changed to item 3 and item 5 title will be changed to item 4 and so on.)</p><p>1. return to step (6)</p><p><h3><a name="alternative4%28viewconflictmembersforani"></a>**Alternative 4 (view conflict members for an item)**</h3></p><p>1-      user press members link</p><p>2-      system display conflict of interest members pop up</p><p>3-      user close the pop up</p>|
|**Acceptance criteria:**||

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Missing required fields|The user has not filled in all mandatory fields|The user clicks “send for confirmation”|An error message is displayed indicating which fields are required. **MSG001**|
|Successful complete resolution data submission|The user is on the edit resolution screen|The user fills in the required fields and clicks “send for confirmation”|The fund data is saved and a confirmation message is displayed.**MSG002,**|
|Unknown error during submission|The user is connected to the internet|The user clicks “save”|An error message is displayed indicating Unknown error, and the order remains unsaved.**MSG004**|

|||
| :- | :- |

|<p>**User Story**</p><p>As a legal council / board secretary, I want to complete the new resolution data; attachments, So that I can go forward in new resolution process.</p><p>**Process Flow**</p><p>1. In resolution details screen, where resolution status is “pending/completing data”,</p><p>1. user press “add item” button</p><p>1. System displays “Add item" pop up</p><p>1. User enter required data, then press “Add” button</p><p>1. System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. System add item to screen</p><p>1. A.User press “send” button</p><p>1. System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. System validates if no items were added.</p><p>a.      If no items added, system display a confirmation message **MSG006**</p><p>10\.  system does below actions</p><p>a.      save resolution with status “waiting for confirmation”</p><p>b.      log Action details, action name (resolution data complete), date, user (user name), role</p><p>c.       send notification</p><p>`                                                              `i.      notify fund manager(s), attached to the fund **MSG003**, notification activity (resolution),</p><p>`                                                             `ii.      legal council, if editor is board secretary **MSG003**, notification activity (resolution),</p><p>`                                                           `iii.      board secretary(s), attached to the fund, if editor is legal council **MSG003**, notification activity (resolution),</p><p>d.      display success message **MSG002**</p><p>**Alternative1 (delete item)**</p><p>1. user press delete button for one of displayed item</p><p>1. system delete item from the screen and re-order items no. to correct the count (if we have 5 items, and user deletes item 3, the item 4 title will be changed to item 3 and item 5 title will be changed to item 4 and so on.)</p><p>1. return to step (6)</p><p>**Alternative 2(view conflict members for an item)**</p><p>1-      user press members link</p><p>2-      system display conflict of interest members pop up</p><p>3-      user close the pop up</p><p> </p><p>**Alternative 3**</p><p>1. B. user press “save” button</p><p>1. System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. system does below actions</p><p>a.      save resolution with status “completing data”</p><p>b.      display success message **MSG002**</p><p> </p><p>**Acceptance Criteria**</p>|
| :- |

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Missing required fields|The user has not filled in all mandatory fields|The user clicks “send”|An error message is displayed indicating which fields are required. **MSG001**|
|Successful complete resolution data submission|The user is on the edit resolution screen|The user fills in the required fields and clicks “send ”|The fund data is saved and a confirmation message is displayed.**MSG002,**|
|Unknown error during submission|The user is connected to the internet|The user clicks “send/save”|An error message is displayed indicating Unknown error, and the order remains unsaved.**MSG004**|

|<p></p><p>**Data Field Validation Criteria**</p>|
| :- |

|**Field Title**|**Title En**|**Type**|**R**|**NU**|**DC**|**SC**|**SP**|**Min/Max**|**Sample**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|**Resolution items**||||||||||
|إضافة بند|Add item|button||||||||
|**items list**||||||||||
|عنوان البند|Item title.|label||||||||
|الوصف|Description||||||||Maxb(200)char|
|يوجد تعارض|Conflict|Yes/no|||||||disabled|
|الاعضاء|members|Link (counter)|||||||appears if conflict is exist|
|تعديل|Edit|button||||||||
|حذف|Delete|button||||||||
|**Add/Edit item pop up**||||||||||
|عنوان البند|Item title.|Label|R||||||autogenerated,Item1 to item N|
|الوصف|Description|Text|N|N|N|Y|Y|Max (500 char)||
|تعارض المصالح مع الاعضاء|Conflict of interest with some members|Check box|N||||||Not selected by default|
|الاعضاء|Members|DDL ,Multi select|N||||||Enabled && mandatory if conflict is checked|
|**Actions**||||||||||
|أضف|add|||||||||
|الغاء|cancel||||||||Return to resolution details screen|
|**Conflict members pop up**||||||||||
|رقم|Serial|Label||||||||
|اسم العضو|Member name|Label||||||||
|إغلاق|close|Button|||||||Return to resolution details screen|
|**Screen actions**||||||||||
|حفظ|Save|button||||||||
|إرسال|Send|button||||||||
|cancel||button|||||||Back to resolutions list screen **MSG005**|

|<p></p><p> </p><p>**System messages**</p>|
| :- |

|**Arabic Message**|**English Message**|**Subject**|**Type**|**MSGID**|
| :- | :- | :- | :- | :- |
|حقل إلزامي|Required Field|\*|Error|MSG001|
|تم حفظ البيانات بنجاح|Record Saved Successfully|\*|Success|MSG002|
|تم استكمال بيانات القرار رقم "كود القرار" في الصندوق "اسم الصندوق" بواسطة "اسم المستخدم"|Data is completed for the resolution code “resolution code attached to fund “fund name”, by “user name”|استكمال بيانات القرار/ resolution data completion|Notification|MSG003|
|حدث خطأ بالنظام , لم يتم حفظ البيانات|An error is occurred while saving data| |Error|MSG004|
|الخروج من الصفحة يعنى عدم حفظ جميع البيانات التي قمت بإضافتها (نعم\لا)|Exit from screen will not saving all data(yes/no)| |Confirmation|MSG005|
|في حالة عدم إضافة بنود القرار سيتم التصويت على القرار ككل (نعم\لا)|If the resolution items are not added, the resolution as a whole will be voted on (yes/no)| |Confirmation|MSG006|

||
| :- |



|<p>[**Resolutions**](https://arabdt.atlassian.net/browse/JDWA-504) ([JDWA-504](https://arabdt.atlassian.net/browse/JDWA-504)) </p><p><h3>![ref1]**[JDWA-507] [Complete resolution data- resolution items and conflicts -Legal council / board secretary](https://arabdt.atlassian.net/browse/JDWA-507)** </h3></p>|||
| :- | :- | :- |
|` `**Description**  | ||

|<p>**User Story**</p><p>As a legal council / board secretary, I want to complete the new resolution data; attachments, So that I can go forward in new resolution process.</p><p>**Process Flow**</p><p>2. In resolution details screen, where resolution status is “pending/completing data”,</p><p>2. user press “add item” button</p><p>2. System displays “Add item" pop up</p><p>2. User enter required data, then press “Add” button</p><p>2. System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>2. System add item to screen</p><p>2. A.User press “send” button</p><p>2. System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>2. System validates if no items were added.</p><p>a.      If no items added, system display a confirmation message **MSG006**</p><p>10\.  system does below actions</p><p>a.      save resolution with status “waiting for confirmation”</p><p>b.      log Action details, action name (resolution data complete), date, user (user name), role</p><p>c.       send notification</p><p>`                                                              `i.      notify fund manager(s), attached to the fund **MSG003**, notification activity (resolution),</p><p>`                                                             `ii.      legal council, if editor is board secretary **MSG003**, notification activity (resolution),</p><p>`                                                           `iii.      board secretary(s), attached to the fund, if editor is legal council **MSG003**, notification activity (resolution),</p><p>d.      display success message **MSG002**</p><p><a name="alternative1%28deleteitem%29"></a>**Alternative1 (delete item)**</p><p>2. user press delete button for one of displayed item</p><p>2. system delete item from the screen and re-order items no. to correct the count (if we have 5 items, and user deletes item 3, the item 4 title will be changed to item 3 and item 5 title will be changed to item 4 and so on.)</p><p>2. return to step (6)</p><p><a name="alternative2%28viewconflictmembersforani"></a>**Alternative 2(view conflict members for an item)**</p><p>1-      user press members link</p><p>2-      system display conflict of interest members pop up</p><p>3-      user close the pop up</p><p> </p><p><a name="alternative3"></a>**Alternative 3**</p><p>2. B. user press “save” button</p><p>2. System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>2. system does below actions</p><p>a.      save resolution with status “completing data”</p><p>b.      display success message **MSG002**</p><p> </p><p>**Acceptance Criteria**</p>|
| :- |

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Missing required fields|The user has not filled in all mandatory fields|The user clicks “send”|An error message is displayed indicating which fields are required. **MSG001**|
|Successful complete resolution data submission|The user is on the edit resolution screen|The user fills in the required fields and clicks “send ”|The fund data is saved and a confirmation message is displayed.**MSG002,**|
|Unknown error during submission|The user is connected to the internet|The user clicks “send/save”|An error message is displayed indicating Unknown error, and the order remains unsaved.**MSG004**|

|<p></p><p>**Data Field Validation Criteria**</p>|
| :- |

|**Field Title**|**Title En**|**Type**|**R**|**NU**|**DC**|**SC**|**SP**|**Min/Max**|**Sample**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|**Resolution items**||||||||||
|إضافة بند|Add item|button||||||||
|**items list**||||||||||
|عنوان البند|Item title.|label||||||||
|الوصف|Description||||||||Maxb(200)char|
|يوجد تعارض|Conflict|Yes/no|||||||disabled|
|الاعضاء|members|Link (counter)|||||||appears if conflict is exist|
|تعديل|Edit|button||||||||
|حذف|Delete|button||||||||
|**Add/Edit item pop up**||||||||||
|عنوان البند|Item title.|Label|R||||||autogenerated,Item1 to item N|
|الوصف|Description|Text|N|N|N|Y|Y|Max (500 char)||
|تعارض المصالح مع الاعضاء|Conflict of interest with some members|Check box|N||||||Not selected by default|
|الاعضاء|Members|DDL ,Multi select|N||||||Enabled && mandatory if conflict is checked|
|**Actions**||||||||||
|أضف|add|||||||||
|الغاء|cancel||||||||Return to resolution details screen|
|**Conflict members pop up**||||||||||
|رقم|Serial|Label||||||||
|اسم العضو|Member name|Label||||||||
|إغلاق|close|Button|||||||Return to resolution details screen|
|**Screen actions**||||||||||
|حفظ|Save|button||||||||
|إرسال|Send|button||||||||
|cancel||button|||||||Back to resolutions list screen **MSG005**|

|<p></p><p> </p><p>**System messages**</p>|
| :- |

|**Arabic Message**|**English Message**|**Subject**|**Type**|**MSGID**|
| :- | :- | :- | :- | :- |
|حقل إلزامي|Required Field|\*|Error|MSG001|
|تم حفظ البيانات بنجاح|Record Saved Successfully|\*|Success|MSG002|
|تم استكمال بيانات القرار رقم "كود القرار" في الصندوق "اسم الصندوق" بواسطة "اسم المستخدم"|Data is completed for the resolution code “resolution code attached to fund “fund name”, by “user name”|استكمال بيانات القرار/ resolution data completion|Notification|MSG003|
|حدث خطأ بالنظام , لم يتم حفظ البيانات|An error is occurred while saving data| |Error|MSG004|
|الخروج من الصفحة يعنى عدم حفظ جميع البيانات التي قمت بإضافتها (نعم\لا)|Exit from screen will not saving all data(yes/no)| |Confirmation|MSG005|
|في حالة عدم إضافة بنود القرار سيتم التصويت على القرار ككل (نعم\لا)|If the resolution items are not added, the resolution as a whole will be voted on (yes/no)| |Confirmation|MSG006|

||
| :- |

<a name="alternative2"></a>

|<p>[**Resolutions**](https://arabdt.atlassian.net/browse/JDWA-504) ([JDWA-504](https://arabdt.atlassian.net/browse/JDWA-504)) </p><p><h3>- **[JDWA-568] [Edit resolution data- attachments -Legal council / board secretary](https://arabdt.atlassian.net/browse/JDWA-568)** </h3></p>||
| :- | :- |

|` `**Description**  | |
| :-: | :- |

|||
| :- | :- |

|<p><h3>**User Story**</h3></p><p>As a legal council / board secretary, I want to Edit the resolution data, So that I update the resolution and return back in resolution creation process..</p><p></p>|
| :- |

|<h3></h3>||
| :- | :- |
|**Process flow:**|<p><h3><a name="processflow"></a>**Process Flow**</h3></p><p>1. In “Edit resolution data” screen, user can view attachments</p><p>1. user press “add attachment” button</p><p>1. System displays open file window</p><p>1. User select one of displayed files to open</p><p>1. System does below actions</p><p>a.      add selected file to the attachments list in the screen</p><p>b.      increment attachments counter +1</p><p>1. User press “send for confirmation” button</p><p>1. A.  If resolution status is “waiting for confirmation/confirmed/rejected”, System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. System validates if no items were added.</p><p>a.      If no items added, system display a confirmation message **MSG010**</p><p>1. system does below actions</p><p>a.      save resolution with status “waiting for confirmation”</p><p>b.      log resolution Action details, action name (resolution data update), date, user (user name), role</p><p>c.       send notification</p><p>`                                                              `i.     notify fund manager(s)/legal council/other board secretary (if editor is one of board secretary), attached to the fund **MSG003**, notification activity (resolution),</p><p>`                                                              `ii.  notify fund manager(s)/board secretary (if editor is legal council), attached to the fund **MSG003**, notification activity (resolution),</p><p>d.      display success message **MSG002**</p><p><h3>**Alternative 1**</h3></p><p>1. B   If resolution status is “Voting in progress”, System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. system display a confirmation message **MSG006**, yes/no buttons</p><p>1. If user press yes, system does below actions</p><p>a.      Suspend active voting</p><p>b.      save resolution with status “waiting for confirmation”</p><p>c.       log resolution Action details, action name (resolution vote suspend), date, user (user name), role</p><p>d.      log resolution Action details, action name (resolution data update), date, user (user name), role[[MR1]|] </p><p>e.      send notification</p><p>`                                                              `i.      notify fund manager(s) / legal council & board secretary/ board members, attached to the fund **MSG007**, notification activity (resolution),</p><p>f.        display success message **MSG002**</p><p><h3>**Alternative 2**</h3></p><p>1. From resolutions screen user press edit button for one of displayed resolutions with status “Approved/not approved” or press edit button from resolution details screen</p><p>1. System display a confirmation message **MSG008**, yes/No</p><p>1. If user press yes, System display “Edit resolution data” screen  filled with resolution data, except fund code</p><p>1. user can view attachments</p><p>1. user press “add attachment” button</p><p>1. System displays open file window</p><p>1. User select one of displayed files to open</p><p>1. System does below actions</p><p>a.      add selected file to the attachments list in the screen</p><p>b.      increment attachments counter +1</p><p>1. User press “send for confirmation” button</p><p>10\.  System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>11\.  system does below actions</p><p>a.      generates new resolution code</p><p>b.      relate between old resolution and new one</p><p>c.       save resolution with status “waiting for confirmation”</p><p>d.      log resolution Action details, action name (resolution creation), date, user (user name), role</p><p>e.      send notification</p><p>`                                                              `i.      notify fund manager(s)/legal council/board secretary, attached to the fund **MSG009**, notification activity (resolution),</p><p>f.        display success message **MSG002**</p><p><h3><a name="alternative3%28deleteattachment%29"></a>**Alternative 3 (delete attachment)**</h3></p><p>1. user press delete button for one of displayed attachments</p><p>1. system delete attachment from the screen</p><p>1. return to step (5)</p>|
|**Acceptance criteria:**||

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Missing required fields|The user has not filled in all mandatory fields|The user clicks “send for confirmation”|An error message is displayed indicating which fields are required. **MSG001**|
|Successful complete resolution data submission|The user is on the edit resolution screen|The user fills in the required fields and clicks “send for confirmation”|The fund data is saved and a confirmation message is displayed.**MSG002,**|
|Unknown error during submission|The user is connected to the internet|The user clicks “save”|An error message is displayed indicating Unknown error, and the order remains unsaved.**MSG004**|

|||
| :- | :- |
|**Data field validation:**||

|**Title Ar**|**Title En**|**Type**|**Req**|**NULLABLE**|**Dec**|**Sp Chars**|**Allow S**|**Min**|**Max**|**Condition**|**Sample**|**Sample Data Ar**|**Sample Data En**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|**Attachments**||||||||||||||
||Attachments counter|Label|||||||||Start value =0|||
||Add attachment|button||||||||||||
|**Attachments list**||||||||||||||
||File name|||||||||||||
||File size|||||||||||||
|**Actions**||||||||||||||
|delete||button||||||||||||
|Download file||button||||||||||||
|إرسال|Send|button||||||||||||
|الغاء|cancel|button|||||||||Back to resolutions list screen **MSG005** |||

|||
| :- | :- |
|**System messages:**||

|**Arabic Message**|** |**English Message**|**Subject**|**Type**|**MSGID**|||
| :- | :- | :- | :- | :- | :- | :- | :- |
|حقل إلزامي|Required Field||Error|MSG001||||
|||||||||
|تم حفظ البيانات بنجاح|Record Saved Successfully||Success|MSG002||||
|||||||||
|تم استكمال بيانات القرار رقم "كود القرار" في الصندوق "اسم الصندوق" بواسطة ““الدور”"اسم المستخدم"|Data is completed for the resolution code “resolution code attached to fund “fund name”, by “role” “user name”|استكمال بيانات القرار/ resolution data completion|Notification|MSG003||||
|حدث خطأ بالنظام , لم يتم حفظ البيانات|An error is occurred while saving data||Error|MSG004||||
|الخروج من الصفحة يعنى عدم حفظ جميع البيانات التي قمت بإضافتها (نعم\لا)|Exit from screen will not saving all data(yes/no)||Confirmation|MSG005||||
|هذا القرار في مرحلة التصويت, في حالة التعديل على البيانات سوف يتم إلغاء التصويت و إشعار أعضاء الصندوق بذلك|This resolution voting is in progress, updating its data will cancel the vote, and system will notify board members||Confirmation|MSG006||||
|تم تحديث بيانات القرار رقم "كود القرار" في الصندوق "اسم الصندوق" بواسطة “الدور” "اسم المستخدم"  مما يترتب عليه إلغاء التصويت الجارى على القرار|Data is updated for the resolution code “resolution code attached to fund “fund name”, by “role” “user name”, which impacts resolution voting cancelation|تحديث بيانات القرار/ resolution data update|Notification|MSG007||||
|تحديث ييانات القرار المعتمد\غير معتمد يترتب عليه إنشاء قرار جديد مرتبط بهذا القرار|Updating approved/not approved resolutions initiates a new resolution related to this one ||Confirmation|MSG008||||
|تم إضافة قرار جديد ضمن فعاليات صندوق "اسم الصندوق" بواسطة "دور المستخدم" "اسم المستخدم” كتحديث على القرار "رقم القرار"|A new resolution is added to fund "fund name " by  “user role” “user name”, as an update on resolution no. “resolution no.”|إضافة قرار/ adding a resolution|Notification|MSG009||||
|في حالة عدم إضافة بنود القرار سيتم التصويت على القرار ككل (نعم\لا)|If the resolution items are not added, the resolution as a whole will be voted on (yes/no)||Confirmation|MSG010 ||||

|||
| :- | :- |







|<p>[**Resolutions**](https://arabdt.atlassian.net/browse/JDWA-504) ([JDWA-504](https://arabdt.atlassian.net/browse/JDWA-504)) </p><p><h3>- **[JDWA-505] [Complete resolution data- attachments -Legal council / board secretary**](https://arabdt.atlassian.net/browse/JDWA-505)**</h3></p>||
| :- | :- |

|` `**Description**  | |
| :-: | :- |

|||
| :- | :- |

|<p><h3>**User Story**</h3></p><p>As a legal council / board secretary, I want to complete the new resolution data; attachments, So that I can go forward in new resolution process.</p><p></p>|
| :- |

|<p><h3>** </h3></p><p><h3></h3></p>||
| :- | :- |
|**Process flow:**|<p>1. In resolution details screen, where resolution status is “pending/completing data”,</p><p>1. user press “add attachment” button</p><p>1. System displays open file window</p><p>1. User select one of displayed files to open</p><p>1. System does below actions</p><p>a.      add selected file to the attachments list in the screen</p><p>b.      increment attachments counter +1</p><p>1. A. User press “send” button</p><p>1. System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. System validates if no items were added.</p><p>a.      If no items added, system display a confirmation message **MSG006**</p><p>1. system does below actions</p><p>a.      save resolution with status “waiting for confirmation”</p><p>b.      log resolution Action details, action name (resolution data complete), date, user (user name), role</p><p>c.       send notification</p><p>`                                                              `i.      notify fund manager(s), attached to the fund **MSG003**, notification activity (resolution),</p><p>`                                                             `ii.      legal council, if editor is board secretary **MSG003**, notification activity (resolution),</p><p>`                                                           `iii.      board secretary(s), attached to the fund, if editor is legal council **MSG003**, notification activity (resolution),</p><p>d.      display success message **MSG002**</p><p><h3><a name="alternative1%28deleteattachment%29"></a>**Alternative 1(delete attachment)**</h3></p><p>1. user press delete button for one of displayed attachments</p><p>1. system delete attachment from the screen</p><p>1. decrease attachments counter -1</p><p>1. return to step (5)</p><p><h3>**Alternative 2**</h3></p><p>1. B. user press “save” button</p><p>1. System validates entering required data</p><p>a.      If any violation is existed, system display and error message **MSG001**</p><p>1. system does below actions</p><p>a.      save resolution with status “completing data”</p><p>b.      display success message **MSG002**</p>|
|**Acceptance criteria:**||

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Missing required fields|The user has not filled in all mandatory fields|The user clicks “send”|An error message is displayed indicating which fields are required. **MSG001**|
|Successful complete resolution data submission|The user is on the edit resolution screen|The user fills in the required fields and clicks “send”|The fund data is saved and a confirmation message is displayed.**MSG002,**|
|Unknown error during submission|The user is connected to the internet|The user clicks “send/save”|An error message is displayed indicating Unknown error, and the order remains unsaved.**MSG004**|

|||
| :- | :- |
|**Data field validation:**||

|**Title Ar**|**Title En**|**Type**|**Req**|**NULLABLE**|**Dec**|**Sp Chars**|**Allow S**|**Min**|**Max**|**Condition**|**Sample**|**Sample Data Ar**|**Sample Data En**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|**Attachments**||||||||||||||
|عداد المرفقات|Attachments counter|Label|||||||||Start value =0|||
|إضافة مرفق|Add attachment|button|||||||max 10 files|||||
|**Attachments list**||||||||||||||
|اسم الملف|File name|||||||||||||
|حجم الملف|File size|||||||||||||
|**Actions**||||||||||||||
|تنزيل الملف|Download|button|||||||||Download resolution file|||
|حذف|Delete|button|||||||||Delete resolution file from screen|||
|فتح|Open|button|||||||||open resolution file|||
|حفظ|Save|button||||||||||||
|إرسال|Send|button||||||||||||
|cancel|الغاء|button|||||||||Back to resolutions list screen **MSG005** |||

|||
| :- | :- |
|**System messages:**||

|**Arabic Message**|**English Message**|**Subject**|**Type**|**MSGID**|
| :- | :- | :- | :- | :- |
|حقل إلزامي|Required Field|\*|Error|MSG001|
|تم حفظ البيانات بنجاح|Record Saved Successfully|\*|Success|MSG002|
|تم استكمال بيانات القرار رقم "كود القرار" في الصندوق "اسم الصندوق" بواسطة "اسم المستخدم"|Data is completed for the resolution code “resolution code attached to fund “fund name”, by “user name”|استكمال بيانات القرار/ resolution data completion|Notification|MSG003|
|حدث خطأ بالنظام , لم يتم حفظ البيانات|An error is occurred while saving data| |Error|MSG004|
|الخروج من الصفحة يعنى عدم حفظ جميع البيانات التي قمت بإضافتها (نعم\لا)|Exit from screen will not saving all data(yes/no)| |Confirmation|MSG005|
|في حالة عدم إضافة بنود القرار سيتم التصويت على القرار ككل (نعم\لا)|If the resolution items are not added, the resolution as a whole will be voted on (yes/no)| |Confirmation|MSG006|

|||
| :- | :- |



|<p>[**Resolutions**](https://arabdt.atlassian.net/browse/JDWA-504) ([JDWA-504](https://arabdt.atlassian.net/browse/JDWA-504)) </p><p><h3>![ref1]**[JDWA-570] [Confirm & Reject a waiting for confirmation resolution/view details- fund manager](https://arabdt.atlassian.net/browse/JDWA-570)** </h3></p>|||
| :- | :- | :- |
|` `**Description**  | ||

|<p><h3>**User Story**</h3></p><p>As a fund manager, I want to be able to confirm/reject a resolution after legal council/board secretary completing resolution info, So that I can go forward in new resolution process</p><p></p>|
| :- |


|**Process flow:**|<p>1. From resolutions screen user press details link for one of displayed resolutions with status “Waiting for confirmation”</p><p>1. System display “details” screen</p><p>1. User press “confirm” button</p><p>1. system does below actions</p><p>a.      save resolution with status “Confirmed”</p><p>b.      log resolution Action details, action name (resolution confirmed), date, user (user name), role</p><p>c.       send notification</p><p>`                                                              `i.      legal council/ Board secretary(s) if exist, attached to the fund **MSG002**, notification activity (resolution),</p><p>d.      display success message **MSG001**</p><p><h3>**Alternative**</h3></p><p>1. B. user press “Reject” button</p><p>1. system display a rejection reason pop up</p><p>1. user enter rejection reason and press ok button</p><p>1. system does below actions</p><p>a.      save resolution with status “rejected ”</p><p>b.      log resolution Action details, action name (resolution rejected), date, user (user name), role</p><p>c.       send notification</p><p>`                                                              `i.      notify fund manager(s), attached to the fund **MSG004**, notification activity (resolution),</p><p>`                                                             `ii.      legal council/ Board secretary(s) if exist, attached to the fund **MSG004**, notification activity (resolution),</p><p>d.      display success message **MSG001**</p>|
| :- | :- |
|**Acceptance criteria:**||

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Successful resolution confirmation submission|The user is on the confirm/reject a resolution screen|The user clicks “confirm”|The fund status is changed and a confirmation message is displayed.**MSG001,**|
|Successful resolution rejection submission|The user is on the confirm/reject a resolution screen|The user clicks “reject”|The fund status is changed and a confirmation message is displayed.**MSG001,**|
|Unknown error during submission|The user is connected to the internet|The user clicks “confirm”/reject|An error message is displayed indicating Unknown error, and the order remains unsaved.**MSG003**|

|||
| :- | :- |
|**Data field validation:**||

|**Title Ar**|**Title En**|**Type**|**Req**|**NULLABLE**|**Dec**|**Sp Chars**|**Allow S**|**Min**|**Max**|**Condition**|**Sample**|**Sample Data Ar**|**Sample Data En**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|**Screen actions**||||||||||||||
|تأكيد|confirm |button ||||||-       ||Appears in case of waiting for confirmation||||
|رفض|Reject |button | |||||-       ||Appears in case of waiting for confirmation||||
|الغاء|Cancel |Button ||||||-       ||Back to resolutions screen||||
|**Screen elements**||||||||||||||
|**Basic info**||||||||||||||
|كود القرار|Resolution code|Label |Y |||||-       |||Autogenerated , format (fund code/resolution year/resolution no.)|||
|القرار الأب|Parent resolution |Label |Y |||||-       |||Appears in case of has referred resolution|||
|كود القرار القديم|Old resolution code|Label|N|N|N||||Max (today)|||||
|تاريخ القرار|Resolution date|Label|Y|N|N|N|N||||Gregorian/Hijri|||
|وصف القرار|Description|Text|N|N|N|Y|Y|||||||
|نوع القرار|Type|Label|Y|||||-       ||||||
|نوع القرار المضاف|New type|Label|Y|N|N|N|Y|-       ||Appears in case user select (اخرى)||||
|ملف القرار|Resolution file|File |Y |||||-      1 file|||File type (PDF) , Size up to (10 Mb)|||
|الية التصويت  للقرار|Voting methodology|Option|Y|||||||||جميع الأعضاء - أغلبية الأعضاء|All members -Majority|
|احتساب نتيجة التصويت  للاعضاء|members Voting result|Option|Y|||||||||جميع البنود - أغلبية البنود|All items -  Majority of items|
|الحالة|status|label||||||-       ||||||
|actions||||||||||||||
|Download ||button||||||-       |||Download resolution file|||
|Open||button||||||-       |||open resolution file|||
|**Attachments**||||||||||||||
|Attachments counter||Label||||||-       |||Start value =0|||
|**Attachments list**||||||||||||||
||File name|||||||-       ||||||
||File size|||||||-       ||||||
|**Actions**||||||||||||||
||Download file|button||||||-       ||||||
|**Resolution items**||||||||||||||
|**items list**||||||||||||||
||Item title.|label||||||-       ||||||
|||||||||||||||
||Description|Label|N|N|N|Y|Y|-      Max (500 char)||||||
|**Conflict members pop up**||||||||||||||
|Member name||Label ||||||-       ||||||
|رجوع|back |Button ||||||-       ||Return to resolution details screen||||
|**Resolution history**||||||||||||||
|Action name||||||||-       ||||||
|role||||||||-       ||||||
|User name||||||||-       ||||||
|Date time||||||||-       ||||||
|**Rejection reason** ||||||||||||||
|||||||||-       ||||||

|||
| :- | :- |
|**System messages:**||

|**Arabic Message**|**English Message**|**Subject**|**Type**|**MSGID**|
| :- | :- | :- | :- | :- |
|تم حفظ البيانات بنجاح|Record Saved Successfully|\*|Success|MSG001|
|تم تأكيد القرار رقم "كود القرار" في الصندوق "اسم الصندوق" بواسطة  مدير الصندوق "اسم المستخدم"|the resolution code “resolution code" attached to fund “fund name”, is confirmed by fund manager “user name”|تأكيد القرار/ resolution confirmation|Notification|MSG002|
|تم رفض القرار رقم "كود القرار" في الصندوق "اسم الصندوق" بواسطة "اسم المستخدم"|the resolution code “resolution code" attached to fund “fund name”, is rejected by “user name”|ر فض القرار/ resolution rejection|Notification|MSG004|
|حدث خطأ بالنظام , لم يتم حفظ البيانات|An error is occurred while saving data| |Error|MSG003|

|||
| :- | :- |




|<p>[**Resolutions**](https://arabdt.atlassian.net/browse/JDWA-504) ([JDWA-504](https://arabdt.atlassian.net/browse/JDWA-504)) </p><p><h3>![ref1]**[JDWA-582] [Navigate Resolutions](https://arabdt.atlassian.net/browse/JDWA-582)** </h3></p>|||
| :- | :- | :- |
|` `**Description**  | ||

|<p><h3>**User Story**</h3></p><p>As a system user, I want to investigate resolutions attached to my funds, So that I can manage resolutions and track their status.</p><p><h3>**Process Flow**</h3></p><p>1. In “fund details” screen User clicks “resolution”</p><p>1. System displays resolutions screen, which display all resolutions attached to this fund</p><p>a.      If there are no resolutions, system display **MSG001**</p><p>1. System display list of resolutions ordered DESC by last update date</p><p>a.      For fund manager, system display all resolutions with all status</p><p>b.      For legal council/board secretary, system display all resolutions with all status except draft</p><p>c.       For board member, system display all resolutions with status voting in progress, approved/not approved</p><p>1. Each resolution is displayed in a separated record</p><p></p><p></p><p></p><p><h3>**Acceptance Criteria**</h3></p>|
| :- |

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Successful resolutions display|The user is on the resolutions screen|The user clicks screen link|The resolutions list is displayed|
|Unknown error during displaying data|The user is connected to the internet|The user clicks screen link|An error message is displayed indicating unknown error, and the order remains not done.**MSG002**|

|<h3></h3>|
| :- |



|<p>[**Resolutions**](https://arabdt.atlassian.net/browse/JDWA-504) ([JDWA-504](https://arabdt.atlassian.net/browse/JDWA-504)) </p><p><h3>![ref1]**[JDWA-593] [view details completing data & waiting for confirmation & confirmed & rejected resolution- fund manager**](https://arabdt.atlassian.net/browse/JDWA-593)**</h3></p>|
| :- |

|` `**Description**  | |
| :-: | :- |

|<p><h3><a name="asafundmanager%2cviewresolutiondetailsof"></a>**As a fund manager, view resolution details of completing data & waiting for & confirmation & confirmed & rejected resolution, So that I can go forward in new resolution process.**</h3></p><p><h3>**Data Field Validation:** </h3></p>|
| :- |

|**Title Ar**|**Title En**|**Type**|**Req**|**NULLABLE**|**Dec**|**Sp Chars**|**Allow S**|**Min**|**Max**|**Condition**|**Sample**|**Sample Data Ar**|**Sample Data En**|
| :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- | :- |
|**Screen actions**||||||||||||||
|تأكيد|confirm |button ||||||-       ||Appears in case of waiting for confirmation||||
|رفض|Reject |button | |||||-       ||Appears in case of waiting for confirmation||||
|الغاء|Cancel |Button ||||||-       ||Back to resolutions screen||||
|**Screen elements**||||||||||||||
|**Basic info**||||||||||||||
|كود القرار|Resolution code|Label |Y |||||-       |||Autogenerated , format (fund code/resolution year/resolution no.)|||
|القرار الأب|Parent resolution |Label |Y |||||-       |||Appears in case of has referred resolution|||
|كود القرار القديم|Old resolution code|Label|N|N|N||||Max (today)|||||
|تاريخ القرار|Resolution date|Label|Y|N|N|N|N||||Gregorian/Hijri|||
|وصف القرار|Description|Text|N|N|N|Y|Y|||||||
|نوع القرار|Type|Label|Y|||||-       ||||||
|نوع القرار المضاف|New type|Label|Y|N|N|N|Y|-       ||Appears in case user select (اخرى)||||
|ملف القرار|Resolution file|File |Y |||||-      1 file|||File type (PDF) , Size up to (10 Mb)|||
|<a name="range!a16"></a>الية التصويت  للقرار|Voting methodology|Option|Y|||||||||جميع الأعضاء - أغلبية الأعضاء|All members -Majority|
|احتساب نتيجة التصويت  للاعضاء|members Voting result|Option|Y|||||||||جميع البنود - أغلبية البنود|All items -  Majority of items|
|الحالة|status|label||||||-       ||||||
|actions||||||||||||||
|Download ||button||||||-       |||Download resolution file|||
|Open||button||||||-       |||open resolution file|||
|**Attachments**||||||||||||||
|Attachments counter||Label||||||-       |||Start value =0|||
|**Attachments list**||||||||||||||
||File name|||||||-       ||||||
||File size|||||||-       ||||||
|**Actions**||||||||||||||
||Download file|button||||||-       ||||||
|**Resolution items**||||||||||||||
|**items list**||||||||||||||
||Item title.|label||||||-       ||||||
|||||||||||||||
||Description|Label|N|N|N|Y|Y|-      Max (500 char)||||||
|**Conflict members pop up**||||||||||||||
|Member name||Label ||||||-       ||||||
|رجوع|back |Button ||||||-       ||Return to resolution details screen||||
|**Resolution history**||||||||||||||
|Action name||||||||-       ||||||
|role||||||||-       ||||||
|User name||||||||-       ||||||
|Date time||||||||-       ||||||
|**Rejection reason** ||||||||||||||
|||||||||-       ||||||

|<p><h3></h3></p><p></p>|
| :- |

|**Process flow:**|<p>1. From resolutions screen user press details link for one of displayed resolutions with status “completing data/waiting for confirmation/confirmed/rejected”</p><p>1. System display “details” screen</p><p>1. User can review resolution basic info, attachments, download files, view resolution items and conflict with members, resolution history</p><p><h3>**Alternative**</h3></p><p>NA</p>|
| :- | :- |
|**Acceptance criteria:**||

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Successful resolution display|The user is on the resolution details screen|The user clicks “details”|The resolution details are displayed|
|Unknown error during submission|The user is connected to the internet|The user clicks “details”|An error message is displayed indicating Unknown error, and the order remains unsaved.**MSG001**|

|||
| :- | :- |
|**Data field validation:**||
|**System messages:**||

|**Arabic Message**|**English Message**|**Subject**|**Type**|**MSGID**|
| :- | :- | :- | :- | :- |
|حدث خطأ بالنظام , لم يتمكن من عرض البيانات|An error is occurred while displaying data| |Error|MSG001|

|||
| :- | :- |




|<p>[**Resolutions**](https://arabdt.atlassian.net/browse/JDWA-504) ([JDWA-504](https://arabdt.atlassian.net/browse/JDWA-504)) </p><p><h3>![ref1]**[JDWA-589] [view details a completing data& waiting for confirmation &confirmed &rejected resolution – legal council/board secretary](https://arabdt.atlassian.net/browse/JDWA-589)** </h3></p>|
| :- |

|` `**Description**  | |
| :-: | :- |

|<p><h3>**User Story**</h3></p><p>As a council/board secretary, I want to be able to view a completing data & waiting for confirmation &  confirmed & Rejected resolution details, So that I can go forward in new resolution process.</p><p></p>|
| :- |

|**Process flow:**|<p>1. From resolutions screen user press details link for one of displayed resolutions with status “completing data &Waiting for confirmation&confirmed&Rejected”</p><p>1. System display “details” screen</p><p>1. User can review resolution basic info, attachments, items, resolution history</p><p> </p><p><h3>**Alternative**</h3></p><p>NA</p>|
| :- | :- |
|**Acceptance criteria:**||

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Successful resolution display|The user is on the resolution details screen|The user clicks “details”|The resolution details are displayed|
|Unknown error during submission|The user is connected to the internet|The user clicks “details”|An error message is displayed indicating Unknown error, and the order remains unsaved.**MSG001**|

|||
| :- | :- |
|**System messages:**||

|**Arabic Message**|**English Message**|**Subject**|**Type**|**MSGID**|
| :- | :- | :- | :- | :- |
|حدث خطأ بالنظام , لم يتمكن من عرض البيانات|An error is occurred while displaying data| |Error|MSG001|

|||
| :- | :- |






|<p>[**Resolutions**](https://arabdt.atlassian.net/browse/JDWA-504) ([JDWA-504](https://arabdt.atlassian.net/browse/JDWA-504)) </p><p><h3>![ref1]**[JDWA-569] [Send a confirmed resolution to vote - legal council/board secretary](https://arabdt.atlassian.net/browse/JDWA-569)** </h3></p>|
| :- |

|**Description**  | |
| :- | :- |

|<p><h3>**User Story**</h3></p><p>As a legal council/board secretary, I want to be able to send a resolution to vote after fund manager confirmation , So that I can start voting process.</p><p></p>|
| :- |

|**Process flow:**|<p>1. From resolutions screen user press details link for one of displayed resolutions with status “confirmed”</p><p>1. System display “details” screen</p><p>1. User press “send to vote” button</p><p>1. System display a confirmation message</p><p>1. If user press yes, system does below actions</p><p>a.      save resolution with status “Voting in progress”</p><p>b.      generate a vote with status “active”</p><p>c.       log resolution Action details, action name (start resolution voting), date, user (user name), role</p><p>d.      send notification</p><p>`                                                              `i.      fund manager/board members/ legal council/ Board secretary(s) if exist, attached to the fund **MSG002**, notification activity (resolution),</p><p>e.      display success message **MSG001**</p><p><h3>**Alternative**</h3></p><p>a.      NA</p>|
| :- | :- |
|**Acceptance criteria:**||

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Successful resolution send to vote submission|The user is on the send to vote resolution screen|The user clicks “send to vote”|The fund status is changed and a confirmation message is displayed.**MSG001,**|
|Unknown error during submission|The user is connected to the internet|The user clicks ”send to vote”|An error message is displayed indicating Unknown error, and the order remains unsaved.**MSG003**|

|||
| :- | :- |
|**Data field validation:**|check attachment|
|**System messages:**||

|**Arabic Message**|**English Message**|**Subject**|**Type**|**MSGID**|
| :- | :- | :- | :- | :- |
|تم حفظ البيانات بنجاح|Record Saved Successfully|\*|Success|MSG001|
|تم إرسال القرار رقم "كود القرار" للتصويت من قبل الأعضاء في الصندوق "اسم الصندوق" بواسطة ”الدور” "اسم المستخدم"|the resolution code “resolution code" attached to fund “fund name”, is sent to vote by “user name”|التصويت على القرار/ resolution confirmation|Notification|MSG002|
|حدث خطأ بالنظام , لم يتم حفظ البيانات|An error is occurred while saving data| |Error|MSG003|

|||
| :- | :- |
















|<p>[**Resolutions**](https://arabdt.atlassian.net/browse/JDWA-504) ([JDWA-504](https://arabdt.atlassian.net/browse/JDWA-504)) </p><p><h3>![ref1]**[JDWA-583] [Search resolutions](https://arabdt.atlassian.net/browse/JDWA-583)** </h3></p>|||
| :- | :- | :- |
|` `**Description**  | ||

|<p><h3>**User Story**</h3></p><p>As a system user, I want to search resolutions as per my role, So that I can track resolutions through different funds</p><p><h3>**Process Flow**</h3></p><p>1. In “fund details” screen User clicks “resolution”</p><p>1. System displays resolutions screen, which display all resolutions attached to this fund</p><p>a.      If there are no resolutions, system display **MSG001**</p><p>1. User can search resolution with resolution code,</p><p>1. System filters displayed resolutions as per search criteria</p><p>1. User can use advanced search by clicking filter icon</p><p>1. system display advanced search popup</p><p>1. user enter search criteria and press “search” button</p><p>1. System displays resolutions which matches selected criteria (And)</p><p>a. If there are no resolutions match the search criteria, system display **MSG001**</p><p><h3>**Acceptance Criteria**</h3></p>|
| :- |

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Successful resolutions search display|The user is on the resolutions screen|The user clicks filter  |The resolutions list matches the filter is displayed|
|Unknown error during displaying data|The user is connected to the internet|The user clicks screen link|An error message is displayed indicating unknown error, and the order remains not done.**MSG002**|

|<h3></h3>|
| :- |





|<p>[**Notifications**](https://arabdt.atlassian.net/browse/JDWA-451) ([JDWA-451](https://arabdt.atlassian.net/browse/JDWA-451)) </p><p><h3>![ref1]**[JDWA-671] [Filter fund notification list** ](https://arabdt.atlassian.net/browse/JDWA-671)** </h3></p>||
| :- | :- |
|||

|` `**Description**  | |
| :-: | :- |

|||
| :- | :- |

|<p>As a fund manager/legal council/board secretary/board member, I want to filter fund notification list, So that I can track my funds changes trough notifications specific to a certain fund activity</p><p></p>|
| :- |

|||
| :- | :- |
|<p></p><p>**Process flow:**</p>|<p>1. User press notification list tab in fund details screen, side panel with 2 tabs</p><p>a.      Notifications list سجل التنبيهات (default selected)</p><p>b.       status history سجل الحركات</p><p>1. System displays notification list tab which contains all fund notifications sent to the logged-in user displayed as cards, ordered DESC by date (LIFO)</p><p>1. if no of independent members >= 2, system activate fund activities</p><p>2. User press any of fund activity notification counter, displayed on activity card</p><p>1. System filters fund notification list as per to selected fund activity </p><p>&emsp;1. If there are no notification, system display **MSG002**</p><p><h3><a name="alternative1%28resetfundnotificationlist"></a>**Alternative 1 (reset fund notification list)**</h3></p><p>1. user press reset button in fund notification list</p><p>2. system display all fund notifications sent to the logged-in user displayed as cards, ordered DESC by date (LIFO)<br>   a.      If there are no notification, system display **MSG002**</p>|
|**Acceptance criteria:**||

|**Scenario**|**Given**|**When**|**Then**|
| :- | :- | :- | :- |
|Successful fund notification list filtration|The user is on the fund details screen|The user clicks the activity counter|The fund notification list is filtered|
|Unknown error during displaying data|The user is connected to the internet|The user clicks screen link|An error message is displayed indicating unknown error, and the order remains not done.**MSG001**|
| | | | |

|||
| :- | :- |
|**System messages:**||

|عفوا, لا توجد  بيانات مسجلة لعرضها|No records exist to display|notification|MSG002|
| :- | :- | :- | :- |

|||
| :- | :- |

[ref1]: https://arabdt.atlassian.net/images/icons/link_out_bot.gif
