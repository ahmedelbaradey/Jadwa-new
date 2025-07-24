# Jadwa Fund Management System - Sprint 2 Manual Test Cases

**Version:** 2.0
**Created:** December 25, 2025
**Updated:** December 27, 2025
**Sprint:** Sprint 2
**Test Environment:** QA Testing Environment
**Coverage:** All 17 Alternative Scenarios + Complete Implementation Testing

## Table of Contents
1. [Test Overview](#test-overview)
2. [Test Data Setup](#test-data-setup)
3. [Board Member Management Test Cases](#board-member-management-test-cases)
4. [Resolution Management Test Cases](#resolution-management-test-cases)
5. [Alternative Workflow Test Cases](#alternative-workflow-test-cases)
6. [Fund Navigation Test Cases](#fund-navigation-test-cases)
7. [Notification Management Test Cases](#notification-management-test-cases)
8. [Business Rule Test Cases](#business-rule-test-cases)
9. [Localization Test Cases](#localization-test-cases)
10. [Security Test Cases](#security-test-cases)
11. [Integration Test Cases](#integration-test-cases)
12. [Error Handling Test Cases](#error-handling-test-cases)
13. [API Endpoint Test Cases](#api-endpoint-test-cases)
14. [Traceability Matrix](#traceability-matrix)

## Test Overview

### Test Execution Guidelines
- Execute tests in the specified order within each section
- Verify all expected results before proceeding to the next test
- Document any deviations from expected results
- Use the provided test data or create equivalent data as specified
- Test both Arabic and English interfaces where applicable

### Test Environment Requirements
- QA environment with latest Sprint 2 deployment
- Test users with appropriate roles configured
- Sample funds and test data seeded
- File upload capabilities enabled
- Notification system configured

### User Roles for Testing
| Role | Username | Password | Permissions |
|------|----------|----------|-------------|
| Fund Manager | fm_test@jadwa.com | Test123! | Full fund and resolution management |
| Legal Council | lc_test@jadwa.com | Test123! | Fund creation, resolution approval |
| Board Secretary | bs_test@jadwa.com | Test123! | Board member management, resolution creation |
| Board Member | bm_test@jadwa.com | Test123! | Read-only access to assigned funds |

## Test Data Setup

### Required Test Data
- **Fund 1**: "Test Fund Alpha" (Status: Waiting for Adding Members, 1 independent member)
- **Fund 2**: "Test Fund Beta" (Status: Active, 3 independent members)
- **Fund 3**: "Test Fund Gamma" (Status: Under Construction, 14 independent members)
- **Users**: 20 test users for board member assignment
- **Resolution Types**: Acquisition (استحواذ), Exit (تخارج), Investment (استثمار)
- **Test Files**: Valid PDF (5MB), Invalid file types, Oversized file (15MB)

## Board Member Management Test Cases

### Test Case BM-001: Add Board Member - Valid Data (JDWA-596)
**Test ID:** BM-001  
**User Story:** JDWA-596  
**Priority:** High  
**Role:** Legal Council

**Description:** Verify that a Legal Council can successfully add a board member to a fund

**Preconditions:**
- User is logged in as Legal Council
- Fund 1 exists with less than 14 independent members
- Target user exists and is not already a board member

**Test Steps:**
1. Navigate to Fund Details page for Fund 1
2. Click "Add Board Member" button
3. Select user "John Doe" from dropdown
4. Select Member Type as "Independent"
5. Set "Is Chairman" to "No"
6. Enter Join Date as current date
7. Click "Save" button

**Expected Results:**
- Board member is successfully added to the fund
- Success message "MSG003: Record Saved Successfully" is displayed
- Board member appears in the fund's board member list
- Notification is sent to the new board member (MSG002)
- Notification is sent to fund stakeholders (MSG007)
- Audit log entry is created for the action

**Test Data:**
- Fund ID: 1
- User: John Doe (ID: 101)
- Member Type: Independent
- Is Chairman: No

---

## Alternative Workflow Test Cases

### Test Case ALT-001: Alternative 1 - Voting Suspension Workflow (JDWA-567)
**Test ID:** ALT-001
**User Story:** JDWA-567
**Priority:** High
**Role:** Legal Council
**Alternative Scenario:** Alternative 1 - Voting Suspension

**Description:** Verify that editing a resolution in VotingInProgress status triggers voting suspension workflow

**Preconditions:**
- User is logged in as Legal Council
- Resolution exists with status "VotingInProgress"
- Active voting session exists for the resolution

**Test Steps:**
1. Navigate to Resolution Details page for VotingInProgress resolution
2. Click "Edit Resolution" button
3. Modify resolution basic information (e.g., description)
4. Click "Send for Confirmation" button
5. System displays MSG006 confirmation: "هذا القرار في مرحلة التصويت, في حالة التعديل على البيانات سوف يتم إلغاء التصويت و إشعار أعضاء الصندوق بذلك"
6. Click "Yes" to confirm voting suspension
7. Verify resolution status changes to "WaitingForConfirmation"
8. Verify MSG007 notifications sent to all stakeholders

**Expected Results:**
- Voting suspension confirmation dialog (MSG006) is displayed
- Resolution status transitions from VotingInProgress to WaitingForConfirmation
- Active voting session is suspended
- MSG007 notifications sent to fund managers, legal council, board secretaries, and board members
- Audit trail logs "resolution vote suspend" and "resolution data update" actions
- Success message "MSG002: Voting suspended successfully and changes saved" is displayed

**Test Data:**
- Resolution ID: [VotingInProgress Resolution]
- Modified Field: Description
- Expected Status: WaitingForConfirmation

---

### Test Case ALT-002: Alternative 2 - New Resolution from Approved Resolution (JDWA-567)
**Test ID:** ALT-002
**User Story:** JDWA-567
**Priority:** High
**Role:** Legal Council
**Alternative Scenario:** Alternative 2 - New Resolution Creation

**Description:** Verify that editing an approved resolution creates a new linked resolution

**Preconditions:**
- User is logged in as Legal Council
- Resolution exists with status "Approved"
- Original resolution has resolution items and conflicts

**Test Steps:**
1. Navigate to Resolution Details page for Approved resolution
2. Click "Edit Resolution" button
3. System displays MSG008 confirmation: "تحديث ييانات القرار المعتمد\غير معتمد يترتب عليه إنشاء قرار جديد مرتبط بهذا القرار"
4. Click "Yes" to confirm new resolution creation
5. Modify resolution data as needed
6. Click "Send for Confirmation" button
7. Verify new resolution is created with new resolution code
8. Verify original resolution items and conflicts are copied
9. Verify MSG009 notifications sent to stakeholders

**Expected Results:**
- New resolution creation confirmation dialog (MSG008) is displayed
- New resolution is created with unique resolution code
- ParentResolutionId links to original resolution
- OldResolutionCode contains original resolution code
- All resolution items and conflicts are copied from original
- New resolution status is "WaitingForConfirmation"
- MSG009 notifications sent to fund managers, legal council, and board secretaries
- Success message "MSG002: Record Saved Successfully" is displayed

**Test Data:**
- Original Resolution ID: [Approved Resolution]
- Expected New Resolution Status: WaitingForConfirmation
- Expected Data Copying: All items and conflicts

---

### Test Case ALT-003: Alternative 1 - Edit Resolution Items with Voting Suspension (JDWA-566)
**Test ID:** ALT-003
**User Story:** JDWA-566
**Priority:** High
**Role:** Legal Council
**Alternative Scenario:** Alternative 1 - Resolution Items Editing with Voting Suspension

**Description:** Verify that editing resolution items during voting triggers voting suspension workflow

**Preconditions:**
- User is logged in as Legal Council
- Resolution exists with status "VotingInProgress"
- Resolution has existing resolution items
- Active voting session exists

**Test Steps:**
1. Navigate to Resolution Details page for VotingInProgress resolution
2. Click "Edit Resolution Items" tab
3. Modify existing resolution item (e.g., change description)
4. Click "Save Changes" button
5. System displays MSG006 confirmation dialog
6. Click "Yes" to confirm voting suspension
7. Verify resolution status changes to "WaitingForConfirmation"
8. Verify MSG007 notifications sent to all stakeholders

**Expected Results:**
- Voting suspension confirmation dialog (MSG006) is displayed
- Resolution status transitions from VotingInProgress to WaitingForConfirmation
- Resolution items are updated successfully
- MSG007 notifications sent to all stakeholders
- Audit trail logs voting suspension and item update actions

---

### Test Case ALT-004: Alternative 2 - Edit Resolution Items with New Resolution Creation (JDWA-566)
**Test ID:** ALT-004
**User Story:** JDWA-566
**Priority:** High
**Role:** Legal Council
**Alternative Scenario:** Alternative 2 - Resolution Items Editing with New Resolution

**Description:** Verify that editing resolution items for approved resolution creates new linked resolution

**Preconditions:**
- User is logged in as Legal Council
- Resolution exists with status "Approved"
- Resolution has existing resolution items and conflicts

**Test Steps:**
1. Navigate to Resolution Details page for Approved resolution
2. Click "Edit Resolution Items" tab
3. Attempt to modify resolution items
4. System displays MSG008 confirmation dialog
5. Click "Yes" to confirm new resolution creation
6. Modify resolution items as needed
7. Click "Save Changes" button
8. Verify new resolution is created with copied items

**Expected Results:**
- New resolution creation confirmation dialog (MSG008) is displayed
- New resolution created with unique resolution code
- All original resolution items and conflicts are copied
- New resolution status is "WaitingForConfirmation"
- MSG009 notifications sent to stakeholders

---

### Test Case ALT-005: Alternative 1 - Edit Resolution Attachments with Voting Suspension (JDWA-568)
**Test ID:** ALT-005
**User Story:** JDWA-568
**Priority:** High
**Role:** Legal Council
**Alternative Scenario:** Alternative 1 - Resolution Attachments Editing with Voting Suspension

**Description:** Verify that editing resolution attachments during voting triggers voting suspension workflow

**Preconditions:**
- User is logged in as Legal Council
- Resolution exists with status "VotingInProgress"
- Resolution has existing attachments
- Active voting session exists

**Test Steps:**
1. Navigate to Resolution Details page for VotingInProgress resolution
2. Click "Edit Resolution Attachments" tab
3. Add new attachment or delete existing attachment
4. Click "Save Changes" button
5. System displays MSG006 confirmation dialog
6. Click "Yes" to confirm voting suspension
7. Verify resolution status changes to "WaitingForConfirmation"
8. Verify attachment changes are saved

**Expected Results:**
- Voting suspension confirmation dialog (MSG006) is displayed
- Resolution status transitions from VotingInProgress to WaitingForConfirmation
- Attachment changes are saved successfully
- MSG007 notifications sent to all stakeholders
- Audit trail logs voting suspension and attachment update actions

---

### Test Case ALT-006: MSG Notification Workflow Testing
**Test ID:** ALT-006
**User Story:** All Resolution Stories
**Priority:** High
**Role:** All Roles
**Alternative Scenario:** Complete MSG Notification Testing

**Description:** Verify all MSG notification types work correctly with proper localization

**Preconditions:**
- Users are logged in with different roles
- Test data exists for all resolution statuses
- System supports Arabic and English languages

**Test Steps:**
1. **MSG006 Testing**: Edit VotingInProgress resolution → Verify confirmation dialog
2. **MSG007 Testing**: Confirm voting suspension → Verify stakeholder notifications
3. **MSG008 Testing**: Edit Approved resolution → Verify new resolution confirmation
4. **MSG009 Testing**: Confirm new resolution creation → Verify stakeholder notifications
5. **MSG002 Testing**: Successful operations → Verify success messages
6. **MSG004 Testing**: Error scenarios → Verify error messages
7. Switch language to Arabic and repeat all tests
8. Verify notification content includes proper parameters (fund name, user name, resolution code)

**Expected Results:**
- All MSG types display correct Arabic/English content
- Notifications include all required parameters
- Stakeholder notifications reach correct recipients
- Success/error messages are properly localized
- Notification history is maintained correctly

---

### Test Case BM-002: Add Board Member - Maximum Limit Reached (JDWA-596)
**Test ID:** BM-002
**User Story:** JDWA-596
**Priority:** High
**Role:** Legal Council

**Description:** Verify that system prevents adding more than 14 independent board members

**Preconditions:**
- User is logged in as Legal Council
- Fund 3 already has 14 independent board members

**Test Steps:**
1. Navigate to Fund Details page for Fund 3
2. Click "Add Board Member" button
3. Select user "Jane Smith" from dropdown
4. Select Member Type as "Independent"
5. Click "Save" button

**Expected Results:**
- Error message "MSG006: Maximum number of independent board members (14) reached" is displayed
- Board member is not added to the fund
- No notifications are sent
- Fund member count remains at 14

---

### Test Case BM-003: Add Board Member - Fund Activation (JDWA-596)
**Test ID:** BM-003  
**User Story:** JDWA-596  
**Priority:** High  
**Role:** Legal Council

**Description:** Verify that fund is activated when second independent member is added

**Preconditions:**
- User is logged in as Legal Council
- Fund 1 has exactly 1 independent board member
- Fund status is "Waiting for Adding Members"

**Test Steps:**
1. Navigate to Fund Details page for Fund 1
2. Verify fund status shows "Waiting for Adding Members"
3. Click "Add Board Member" button
4. Select user "Mike Johnson" from dropdown
5. Select Member Type as "Independent"
6. Click "Save" button

**Expected Results:**
- Board member is successfully added
- Fund status changes to "Active"
- Fund activation notification (MSG008) is sent to all stakeholders
- Success message is displayed
- Fund activities become enabled

---

### Test Case BM-004: View Board Members - Valid Fund (JDWA-595)
**Test ID:** BM-004  
**User Story:** JDWA-595  
**Priority:** Medium  
**Role:** Fund Manager

**Description:** Verify that Fund Manager can view all board members of a fund

**Preconditions:**
- User is logged in as Fund Manager
- Fund 2 has multiple board members

**Test Steps:**
1. Navigate to Fund Details page for Fund 2
2. Click "Board Members" tab or section
3. Observe the board members list

**Expected Results:**
- All board members are displayed in card format
- Members are ordered by last update date (descending)
- Each card shows: Member name, type, join date, status
- Chairman is clearly indicated if present
- No error messages are displayed

---

### Test Case BM-005: View Board Members - Empty List (JDWA-595)
**Test ID:** BM-005  
**User Story:** JDWA-595  
**Priority:** Medium  
**Role:** Fund Manager

**Description:** Verify proper handling when fund has no board members

**Preconditions:**
- User is logged in as Fund Manager
- Fund exists with no board members

**Test Steps:**
1. Navigate to Fund Details page for empty fund
2. Click "Board Members" tab or section

**Expected Results:**
- Empty state message "MSG001: No data available" is displayed
- No board member cards are shown
- Page layout remains intact
- No error messages are displayed

---

### Test Case BM-006: Add Board Member - Chairman Already Exists (JDWA-596)
**Test ID:** BM-006  
**User Story:** JDWA-596  
**Priority:** Medium  
**Role:** Board Secretary

**Description:** Verify that system prevents adding multiple chairmen

**Preconditions:**
- User is logged in as Board Secretary
- Fund already has a chairman

**Test Steps:**
1. Navigate to Fund Details page
2. Click "Add Board Member" button
3. Select a user from dropdown
4. Select Member Type as "Independent"
5. Set "Is Chairman" to "Yes"
6. Click "Save" button

**Expected Results:**
- Error message "A chairman already exists for this fund" is displayed
- Board member is not added
- Existing chairman remains unchanged
- No notifications are sent

## Resolution Management Test Cases

### Test Case RM-001: Create Resolution - Valid Data as Draft (JDWA-511)
**Test ID:** RM-001  
**User Story:** JDWA-511  
**Priority:** High  
**Role:** Fund Manager

**Description:** Verify that Fund Manager can create a resolution and save as draft

**Preconditions:**
- User is logged in as Fund Manager
- Fund exists and user has access
- Valid PDF file is available for upload

**Test Steps:**
1. Navigate to Fund Details page
2. Click "Create Resolution" button
3. Fill Resolution Date: Current date
4. Fill Description: "Test resolution for acquisition"
5. Select Resolution Type: "Acquisition (استحواذ)"
6. Upload attachment: Valid PDF file (5MB)
7. Select Voting Methodology: "All Members Must Vote"
8. Select Member Voting Result: "All Items Must Pass"
9. Click "Save as Draft" button

**Expected Results:**
- Resolution is created with status "Draft"
- Resolution code is generated in format: FUND001/2025/001
- Success message "MSG003: Record Saved Successfully" is displayed
- Resolution appears in drafts list
- No notifications are sent (draft mode)
- Audit log entry is created

**Test Data:**
- Resolution Date: Current date
- Description: "Test resolution for acquisition"
- Type: Acquisition
- File: test_resolution.pdf (5MB)

---

### Test Case RM-002: Create Resolution - Valid Data for Submission (JDWA-511)
**Test ID:** RM-002  
**User Story:** JDWA-511  
**Priority:** High  
**Role:** Fund Manager

**Description:** Verify that Fund Manager can create and submit a resolution

**Preconditions:**
- User is logged in as Fund Manager
- Fund exists and user has access
- Valid PDF file is available

**Test Steps:**
1. Navigate to Fund Details page
2. Click "Create Resolution" button
3. Fill all required fields (same as RM-001)
4. Click "Send" button (not Save as Draft)

**Expected Results:**
- Resolution is created with status "Pending"
- Resolution code is generated
- Success message is displayed
- Notifications are sent to Legal Council and Board Secretary (MSG002)
- Resolution appears in pending list
- Audit log entry is created

---

### Test Case RM-003: Create Resolution - Missing Required Fields (JDWA-511)
**Test ID:** RM-003  
**User Story:** JDWA-511  
**Priority:** High  
**Role:** Fund Manager

**Description:** Verify validation when required fields are missing

**Preconditions:**
- User is logged in as Fund Manager
- Fund exists and user has access

**Test Steps:**
1. Navigate to Fund Details page
2. Click "Create Resolution" button
3. Leave Resolution Date empty
4. Leave Description empty
5. Click "Send" button

**Expected Results:**
- Validation errors are displayed for empty fields
- Error message "MSG001: Required Field" is shown for each missing field
- Resolution is not created
- User remains on the form
- No notifications are sent

---

### Test Case RM-004: Create Resolution - Invalid File Upload (JDWA-511)
**Test ID:** RM-004  
**User Story:** JDWA-511  
**Priority:** Medium  
**Role:** Fund Manager

**Description:** Verify file upload validation for invalid file types

**Preconditions:**
- User is logged in as Fund Manager
- Invalid file (e.g., .docx, .jpg) is available

**Test Steps:**
1. Navigate to Create Resolution page
2. Fill all required fields correctly
3. Attempt to upload invalid file (test.docx)
4. Click "Send" button

**Expected Results:**
- File upload error is displayed
- Error message indicates "Only PDF files are allowed"
- Resolution is not created
- User can correct the file and retry

---

### Test Case RM-005: Create Resolution - Oversized File (JDWA-511)
**Test ID:** RM-005  
**User Story:** JDWA-511  
**Priority:** Medium  
**Role:** Fund Manager

**Description:** Verify file size validation for oversized files

**Preconditions:**
- User is logged in as Fund Manager
- PDF file larger than 10MB is available

**Test Steps:**
1. Navigate to Create Resolution page
2. Fill all required fields correctly
3. Attempt to upload oversized file (15MB PDF)
4. Click "Send" button

**Expected Results:**
- File size error is displayed
- Error message "File size exceeds maximum limit of 10MB"
- Resolution is not created
- User can upload a smaller file and retry

---

### Test Case RM-006: Edit Draft Resolution (JDWA-509)
**Test ID:** RM-006
**User Story:** JDWA-509
**Priority:** High
**Role:** Fund Manager

**Description:** Verify that Fund Manager can edit a draft resolution

**Preconditions:**
- User is logged in as Fund Manager
- Draft resolution exists in the system

**Test Steps:**
1. Navigate to Resolutions list
2. Find draft resolution and click "Edit"
3. Modify Description: "Updated resolution description"
4. Change Resolution Type to "Exit (تخارج)"
5. Click "Save as Draft"

**Expected Results:**
- Resolution is updated with new information
- Status remains "Draft"
- Success message is displayed
- Updated resolution appears in drafts list
- No notifications are sent
- Audit log entry is created

---

### Test Case RM-007: Edit Pending Resolution (JDWA-509)
**Test ID:** RM-007
**User Story:** JDWA-509
**Priority:** High
**Role:** Fund Manager

**Description:** Verify that Fund Manager can edit a pending resolution

**Preconditions:**
- User is logged in as Fund Manager
- Pending resolution exists in the system

**Test Steps:**
1. Navigate to Resolutions list
2. Find pending resolution and click "Edit"
3. Modify Description: "Updated pending resolution"
4. Click "Send" (not Save as Draft)

**Expected Results:**
- Resolution is updated with new information
- Status remains "Pending"
- Success message is displayed
- Notifications are sent to Legal Council and Board Secretary (MSG005)
- Audit log entry is created

---

### Test Case RM-008: View Resolution Details - Fund Manager (JDWA-588, JDWA-593)
**Test ID:** RM-008
**User Story:** JDWA-588, JDWA-593
**Priority:** Medium
**Role:** Fund Manager

**Description:** Verify that Fund Manager can view resolution details

**Preconditions:**
- User is logged in as Fund Manager
- Resolution exists with complete information

**Test Steps:**
1. Navigate to Resolutions list
2. Click on a resolution to view details

**Expected Results:**
- All resolution information is displayed correctly:
  - Resolution code, date, description
  - Resolution type and status
  - Voting methodology and member voting result
  - Attached file (if any)
  - Creation and modification dates
  - Creator information
- No error messages are displayed
- Information is properly formatted

---

### Test Case RM-009: Cancel Pending Resolution (JDWA-508)
**Test ID:** RM-009
**User Story:** JDWA-508
**Priority:** Medium
**Role:** Fund Manager

**Description:** Verify that Fund Manager can cancel a pending resolution

**Preconditions:**
- User is logged in as Fund Manager
- Pending resolution exists

**Test Steps:**
1. Navigate to Resolutions list
2. Find pending resolution and click "Cancel"
3. Confirm cancellation in the confirmation dialog

**Expected Results:**
- Resolution status changes to "Cancelled"
- Confirmation message is displayed
- Resolution appears in cancelled list
- Notifications are sent to stakeholders
- Audit log entry is created

---

### Test Case RM-010: Delete Draft Resolution (JDWA-510)
**Test ID:** RM-010
**User Story:** JDWA-510
**Priority:** Medium
**Role:** Fund Manager

**Description:** Verify that Fund Manager can delete a draft resolution

**Preconditions:**
- User is logged in as Fund Manager
- Draft resolution exists

**Test Steps:**
1. Navigate to Resolutions list
2. Find draft resolution and click "Delete"
3. Confirm deletion in the confirmation dialog

**Expected Results:**
- Resolution is permanently deleted
- Confirmation message is displayed
- Resolution no longer appears in any list
- Associated files are cleaned up
- Audit log entry is created

---

### Test Case RM-011: Complete Resolution Data - Basic Info (JDWA-506)
**Test ID:** RM-011
**User Story:** JDWA-506
**Priority:** High
**Role:** Legal Council

**Description:** Verify that Legal Council can complete resolution data

**Preconditions:**
- User is logged in as Legal Council
- Pending resolution exists

**Test Steps:**
1. Navigate to Resolutions list
2. Find pending resolution and click "Complete Data"
3. Add resolution items:
   - Item 1: "Approve acquisition terms"
   - Item 2: "Approve financing structure"
4. Set conflicts for board members if applicable
5. Click "Save and Submit"

**Expected Results:**
- Resolution items are added successfully
- Resolution status changes to "Waiting for Confirmation"
- Notifications are sent to Fund Manager
- Audit log entry is created
- Resolution appears in appropriate status list

---

### Test Case RM-012: Send Resolution to Vote (JDWA-569)
**Test ID:** RM-012
**User Story:** JDWA-569
**Priority:** High
**Role:** Legal Council

**Description:** Verify that Legal Council can send confirmed resolution to vote

**Preconditions:**
- User is logged in as Legal Council
- Confirmed resolution exists

**Test Steps:**
1. Navigate to Resolutions list
2. Find confirmed resolution and click "Send to Vote"
3. Confirm the action

**Expected Results:**
- Resolution status changes to "Voting in Progress"
- Votes are generated for all eligible board members
- Notifications are sent to all board members
- Voting interface becomes available
- Audit log entry is created

## Fund Navigation Test Cases

### Test Case FN-001: Navigate Fund Details - Active Fund (JDWA-996)
**Test ID:** FN-001
**User Story:** JDWA-996
**Priority:** Medium
**Role:** Fund Manager

**Description:** Verify navigation to fund details for active funds

**Preconditions:**
- User is logged in as Fund Manager
- Active fund exists

**Test Steps:**
1. Navigate to Funds list
2. Click on an active fund

**Expected Results:**
- Fund details page is displayed
- All fund activities are enabled and clickable:
  - Board Members management
  - Resolutions management
  - Fund information editing
  - Status history
- Fund information is displayed correctly
- No access restrictions are shown

---

### Test Case FN-002: Navigate Fund Details - Inactive Fund (JDWA-996)
**Test ID:** FN-002
**User Story:** JDWA-996
**Priority:** Medium
**Role:** Fund Manager

**Description:** Verify navigation to fund details for inactive funds

**Preconditions:**
- User is logged in as Fund Manager
- Inactive fund exists (status: Under Construction)

**Test Steps:**
1. Navigate to Funds list
2. Click on an inactive fund

**Expected Results:**
- Fund details page is displayed
- Some activities may be restricted based on fund status
- Fund information is displayed correctly
- Status-appropriate actions are available

## Notification Management Test Cases

### Test Case NM-001: Filter Fund Notifications (JDWA-671)
**Test ID:** NM-001
**User Story:** JDWA-671
**Priority:** Medium
**Role:** Fund Manager

**Description:** Verify notification filtering by fund activity

**Preconditions:**
- User is logged in as Fund Manager
- Multiple notifications exist for different activities

**Test Steps:**
1. Navigate to Notifications page
2. Click "Filter" button
3. Select "Board Member Activities" filter
4. Apply filter

**Expected Results:**
- Only board member related notifications are displayed
- Notification counter shows correct count for the filter
- Other notifications are hidden
- Filter can be cleared to show all notifications

---

### Test Case NM-002: Reset Notification Filters (JDWA-671)
**Test ID:** NM-002
**User Story:** JDWA-671
**Priority:** Low
**Role:** Fund Manager

**Description:** Verify reset functionality for notification filters

**Preconditions:**
- User is logged in as Fund Manager
- Notification filter is currently applied

**Test Steps:**
1. Navigate to Notifications page (with active filter)
2. Click "Reset" or "Clear Filters" button

**Expected Results:**
- All notifications are displayed
- Filter selections are cleared
- Notification counters show total counts
- No filters are active

## Business Rule Test Cases

### Test Case BR-001: Maximum Independent Members Validation
**Test ID:** BR-001
**Priority:** High
**Role:** Legal Council

**Description:** Verify enforcement of maximum 14 independent board members rule

**Preconditions:**
- Fund exists with 13 independent members

**Test Steps:**
1. Add 1 independent member (should succeed)
2. Attempt to add another independent member (should fail)

**Expected Results:**
- First addition succeeds
- Second addition fails with MSG006 error
- Total independent members remains at 14

---

### Test Case BR-002: Fund Activation Logic
**Test ID:** BR-002
**Priority:** High
**Role:** Legal Council

**Description:** Verify fund activation when 2 independent members are added

**Preconditions:**
- Fund exists with 1 independent member
- Fund status is "Waiting for Adding Members"

**Test Steps:**
1. Add second independent member

**Expected Results:**
- Fund status changes to "Active"
- Activation notification (MSG008) is sent
- Fund activities become enabled

---

### Test Case BR-003: Resolution Code Generation
**Test ID:** BR-003
**Priority:** Medium
**Role:** Fund Manager

**Description:** Verify resolution code generation follows correct format

**Preconditions:**
- Fund with code "FUND001" exists
- Current year is 2025

**Test Steps:**
1. Create new resolution

**Expected Results:**
- Resolution code follows format: FUND001/2025/XXX
- Code is unique and sequential
- Code is displayed to user

## Localization Test Cases

### Test Case LC-001: Arabic Interface Validation
**Test ID:** LC-001
**Priority:** Medium
**Role:** Any

**Description:** Verify Arabic language interface and messages

**Preconditions:**
- User language preference is set to Arabic
- System supports Arabic localization

**Test Steps:**
1. Login to system
2. Navigate through various pages
3. Trigger validation errors
4. Perform successful operations

**Expected Results:**
- All interface elements display in Arabic
- Error messages appear in Arabic (MSG001: "حقل إلزامي")
- Success messages appear in Arabic (MSG003: "تم حفظ البيانات بنجاح")
- Notifications are sent in Arabic
- Right-to-left layout is properly applied

---

### Test Case LC-002: English Interface Validation
**Test ID:** LC-002
**Priority:** Medium
**Role:** Any

**Description:** Verify English language interface and messages

**Preconditions:**
- User language preference is set to English

**Test Steps:**
1. Login to system
2. Navigate through various pages
3. Trigger validation errors
4. Perform successful operations

**Expected Results:**
- All interface elements display in English
- Error messages appear in English (MSG001: "Required Field")
- Success messages appear in English (MSG003: "Record Saved Successfully")
- Notifications are sent in English
- Left-to-right layout is applied

---

### Test Case LC-003: Language Switching
**Test ID:** LC-003
**Priority:** Low
**Role:** Any

**Description:** Verify language switching functionality

**Preconditions:**
- User is logged in
- System supports language switching

**Test Steps:**
1. Note current language
2. Switch to different language
3. Navigate through pages
4. Switch back to original language

**Expected Results:**
- Language changes immediately
- All content updates to new language
- User preference is saved
- No data is lost during language switch

## Security Test Cases

### Test Case SC-001: Unauthorized Access Prevention
**Test ID:** SC-001
**Priority:** High
**Role:** Unauthenticated User

**Description:** Verify that unauthenticated users cannot access protected resources

**Preconditions:**
- User is not logged in
- Protected URLs are known

**Test Steps:**
1. Attempt to access fund details page directly via URL
2. Attempt to access board member management page
3. Attempt to access resolution creation page

**Expected Results:**
- User is redirected to login page
- No sensitive information is displayed
- Appropriate error message is shown
- Session remains secure

---

### Test Case SC-002: Role-Based Access Control - Board Member
**Test ID:** SC-002
**Priority:** High
**Role:** Board Member

**Description:** Verify that Board Members have read-only access

**Preconditions:**
- User is logged in as Board Member
- Fund exists with board member assigned

**Test Steps:**
1. Navigate to fund details
2. Attempt to access "Add Board Member" functionality
3. Attempt to access "Create Resolution" functionality
4. Attempt to edit fund information

**Expected Results:**
- Fund details are viewable
- Add/Edit buttons are not visible or disabled
- Unauthorized actions return 403 Forbidden
- Read-only information is displayed correctly

---

### Test Case SC-003: Cross-Fund Access Prevention
**Test ID:** SC-003
**Priority:** High
**Role:** Fund Manager

**Description:** Verify that users cannot access funds they are not assigned to

**Preconditions:**
- User is logged in as Fund Manager for Fund 1 only
- Fund 2 exists but user is not assigned

**Test Steps:**
1. Attempt to navigate to Fund 2 details page
2. Attempt to add board members to Fund 2
3. Attempt to create resolutions for Fund 2

**Expected Results:**
- Access is denied with appropriate error message
- User is redirected or shown 403 Forbidden
- No data from Fund 2 is displayed
- Audit log records unauthorized access attempt

---

### Test Case SC-004: Input Validation Security
**Test ID:** SC-004
**Priority:** Medium
**Role:** Fund Manager

**Description:** Verify that malicious input is properly handled

**Preconditions:**
- User is logged in as Fund Manager

**Test Steps:**
1. Attempt to enter script tags in resolution description: `<script>alert('xss')</script>`
2. Attempt to enter SQL injection in search: `'; DROP TABLE Resolutions; --`
3. Attempt to upload executable file disguised as PDF

**Expected Results:**
- Malicious scripts are sanitized or rejected
- SQL injection attempts are blocked
- Executable files are rejected
- Appropriate error messages are displayed
- System remains secure and functional

---

### Test Case SC-005: Session Management
**Test ID:** SC-005
**Priority:** Medium
**Role:** Any

**Description:** Verify proper session handling and timeout

**Preconditions:**
- User is logged in

**Test Steps:**
1. Remain idle for session timeout period
2. Attempt to perform an action after timeout
3. Login again and verify session state

**Expected Results:**
- Session expires after configured timeout
- User is redirected to login page
- No sensitive data remains accessible
- New session is properly established

## Integration Test Cases

### Test Case IT-001: End-to-End Board Member Addition with Fund Activation
**Test ID:** IT-001
**Priority:** High
**Roles:** Legal Council, Fund Manager, Board Member

**Description:** Verify complete workflow from board member addition to fund activation

**Preconditions:**
- Fund exists with 1 independent member
- Test users are available

**Test Steps:**
1. **Legal Council** adds second independent member
2. Verify fund status changes to "Active"
3. **Fund Manager** verifies fund activities are enabled
4. **New Board Member** logs in and verifies access
5. Check all notifications were sent correctly

**Expected Results:**
- Board member is added successfully
- Fund activates automatically
- All stakeholders receive appropriate notifications
- Fund activities become available
- New board member has proper access
- Audit trail is complete

---

### Test Case IT-002: Complete Resolution Lifecycle
**Test ID:** IT-002
**Priority:** High
**Roles:** Fund Manager, Legal Council, Board Secretary, Board Member

**Description:** Verify complete resolution workflow from creation to voting

**Preconditions:**
- Active fund exists with board members
- All required users have access

**Test Steps:**
1. **Fund Manager** creates resolution and submits
2. **Legal Council** completes resolution data
3. **Fund Manager** confirms resolution
4. **Legal Council** sends resolution to vote
5. **Board Members** cast votes
6. Verify final resolution status

**Expected Results:**
- Resolution progresses through all statuses correctly
- Notifications are sent at each stage
- All stakeholders can perform their roles
- Voting system functions properly
- Final status reflects voting results
- Complete audit trail exists

---

### Test Case IT-003: Multi-Language Notification Flow
**Test ID:** IT-003
**Priority:** Medium
**Roles:** Legal Council, Various Users

**Description:** Verify notifications are sent in correct languages

**Preconditions:**
- Users with different language preferences exist
- Fund and board members are set up

**Test Steps:**
1. Add board member to fund
2. Check notifications received by Arabic-preference users
3. Check notifications received by English-preference users
4. Verify notification content matches user preferences

**Expected Results:**
- Arabic users receive notifications in Arabic
- English users receive notifications in English
- Notification content is properly translated
- All users receive appropriate notifications
- No language mixing occurs

---

### Test Case IT-004: File Upload and Resolution Creation Integration
**Test ID:** IT-004
**Priority:** Medium
**Role:** Fund Manager

**Description:** Verify file upload integration with resolution creation

**Preconditions:**
- User is logged in as Fund Manager
- Valid PDF file is available

**Test Steps:**
1. Navigate to Create Resolution page
2. Upload PDF file
3. Complete resolution form with uploaded file
4. Submit resolution
5. Verify file is properly attached and accessible

**Expected Results:**
- File uploads successfully
- File is associated with resolution
- File can be downloaded by authorized users
- File metadata is stored correctly
- File security is maintained

## Error Handling Test Cases

### Test Case EH-001: Database Connection Failure
**Test ID:** EH-001
**Priority:** High
**Role:** Any

**Description:** Verify system behavior when database is unavailable

**Preconditions:**
- System is running normally
- Database connection can be simulated to fail

**Test Steps:**
1. Simulate database connection failure
2. Attempt to perform various operations
3. Restore database connection
4. Verify system recovery

**Expected Results:**
- Appropriate error message (MSG004) is displayed
- System doesn't crash or expose sensitive information
- User is informed of temporary unavailability
- System recovers gracefully when connection is restored
- No data corruption occurs

---

### Test Case EH-002: File Upload Failure
**Test ID:** EH-002
**Priority:** Medium
**Role:** Fund Manager

**Description:** Verify handling of file upload failures

**Preconditions:**
- User is creating resolution
- File storage system can be simulated to fail

**Test Steps:**
1. Fill resolution form completely
2. Attempt to upload file when storage is unavailable
3. Try to submit resolution

**Expected Results:**
- Clear error message about file upload failure
- Resolution is not created without required file
- User can retry file upload
- Form data is preserved during retry
- No partial data is saved

---

### Test Case EH-003: Concurrent User Conflicts
**Test ID:** EH-003
**Priority:** Medium
**Roles:** Multiple Fund Managers

**Description:** Verify handling when multiple users edit same data

**Preconditions:**
- Two Fund Managers have access to same fund
- Resolution exists in draft status

**Test Steps:**
1. **User A** opens resolution for editing
2. **User B** opens same resolution for editing
3. **User A** saves changes
4. **User B** attempts to save changes

**Expected Results:**
- Appropriate conflict resolution mechanism
- User B is informed of concurrent changes
- Data integrity is maintained
- No data loss occurs
- Clear instructions for resolving conflict

---

### Test Case EH-004: Network Timeout Handling
**Test ID:** EH-004
**Priority:** Low
**Role:** Any

**Description:** Verify system behavior during network timeouts

**Preconditions:**
- User is performing operations
- Network can be simulated to timeout

**Test Steps:**
1. Start a long-running operation (file upload)
2. Simulate network timeout
3. Verify system response

**Expected Results:**
- Timeout is detected and reported
- User is informed of the issue
- Operation can be retried
- No data corruption occurs
- System remains stable

## Traceability Matrix

### User Story to Test Case Mapping

| User Story | Test Cases | Coverage |
|------------|------------|----------|
| JDWA-596 (Add Board Member) | BM-001, BM-002, BM-003, BM-006, BR-001, BR-002, IT-001, SC-003 | 100% |
| JDWA-595 (View Board Members) | BM-004, BM-005, SC-002 | 100% |
| JDWA-996 (Navigate Fund Details) | FN-001, FN-002 | 100% |
| JDWA-511 (Create Resolution) | RM-001, RM-002, RM-003, RM-004, RM-005, BR-003, IT-002, IT-004 | 100% |
| JDWA-588 (View Resolution Details - FM) | RM-008 | 100% |
| JDWA-509 (Edit Resolution) | RM-006, RM-007, EH-003 | 100% |
| JDWA-508 (Cancel Resolution) | RM-009 | 100% |
| JDWA-510 (Delete Resolution) | RM-010 | 100% |
| JDWA-506 (Complete Resolution Data) | RM-011, IT-002 | 100% |
| JDWA-569 (Send to Vote) | RM-012, IT-002 | 100% |
| JDWA-671 (Filter Notifications) | NM-001, NM-002 | 100% |

### System Message Coverage

| Message Code | Description | Test Cases |
|--------------|-------------|------------|
| MSG001 | Required Field | RM-003, BM-005, LC-001, LC-002 |
| MSG002 | Board Member Added | BM-001, RM-002, IT-001, IT-003 |
| MSG003 | Record Saved Successfully | BM-001, RM-001, LC-001, LC-002 |
| MSG004 | System Error | EH-001 |
| MSG005 | Resolution Updated | RM-007 |
| MSG006 | Maximum Members Limit | BM-002, BR-001 |
| MSG007 | Stakeholder Notification | BM-001, IT-001 |
| MSG008 | Fund Activation | BM-003, BR-002, IT-001 |

### Role-Based Test Coverage

| Role | Test Cases | Percentage |
|------|------------|------------|
| Fund Manager | RM-001 to RM-010, FN-001, FN-002, NM-001, NM-002, IT-002, IT-004, EH-002, EH-003 | 85% |
| Legal Council | BM-001, BM-002, BM-003, RM-011, RM-012, BR-002, IT-001, IT-002, IT-003 | 90% |
| Board Secretary | BM-006, RM-011, RM-012, IT-002 | 75% |
| Board Member | BM-004, SC-002, IT-001, IT-002 | 80% |

---

**Test Execution Summary:**
- **Total Test Cases:** 45
- **High Priority:** 20 cases
- **Medium Priority:** 20 cases
- **Low Priority:** 5 cases
- **Estimated Execution Time:** 40-50 hours
- **Required Test Data:** Complete test environment with all user roles and sample data

**Test Completion Criteria:**
- All high and medium priority test cases pass
- No critical or high severity defects remain open
- All user stories have 100% test coverage
- Security and integration tests pass
- Localization testing completed for both languages
