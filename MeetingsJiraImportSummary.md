# Meetings User Stories - Jira Import Summary

## Overview
This document provides a quick reference for importing 9 Meetings user stories into Jira under a Meetings Epic.

## Epic Details
- **Epic Name**: Meetings
- **Epic Key**: To be assigned by Jira
- **Total Story Points**: 60
- **Description**: Complete meeting management system for board meetings including scheduling, voting, live management, minutes generation, and electronic signatures.

## User Stories Summary

| Story | Title | Points | Priority | User Roles |
|-------|-------|--------|----------|------------|
| 1 | Propose Meeting Times for Voting | 8 | High | Legal Counsel, Board Secretary |
| 2 | Vote on Proposed Meeting Times | 5 | High | Board Member |
| 3 | Schedule a New Board Meeting | 13 | High | Legal Counsel, Board Secretary |
| 4 | Manage a Live Board Meeting | 8 | Medium | Board Secretary, Legal Counsel, Board Members |
| 5 | Generate and Circulate Meeting Minutes | 8 | Medium | Board Secretary, Legal Counsel |
| 6 | Electronically Sign Meeting Minutes | 8 | Medium | Board Member |
| 7 | View Scheduled Meetings | 3 | High | All Users |
| 8 | Modify an Upcoming Meeting | 5 | Medium | Board Secretary, Legal Counsel |
| 9 | Cancel an Upcoming Meeting | 2 | Low | Board Secretary, Legal Counsel |

## Import Instructions

### Step 1: Create Epic
1. Create new Epic in Jira
2. Set Epic Name: "Meetings"
3. Add description from main document
4. Assign to appropriate project

### Step 2: Import Stories
Import in this recommended order:
1. **Story 7** (View Scheduled Meetings) - Foundation
2. **Story 1** (Propose Meeting Times) - Start of workflow
3. **Story 2** (Vote on Proposed Times) - Voting process
4. **Story 3** (Schedule New Meeting) - Core scheduling
5. **Story 8** (Modify Meeting) - Management
6. **Story 9** (Cancel Meeting) - Management
7. **Story 4** (Manage Live Meeting) - Execution
8. **Story 5** (Generate Minutes) - Documentation
9. **Story 6** (Electronic Signatures) - Final approval

### Step 3: Configure Each Story
For each story, set:
- **Issue Type**: Story
- **Parent**: Link to Meetings Epic
- **Summary**: Use title from summary table
- **Description**: Copy complete content from main document
- **Story Points**: Use values from summary table
- **Priority**: Use suggested priority levels
- **Labels**: meetings, scheduling, voting, minutes, e-signature
- **Components**: Meetings

### Step 4: Validation
After import, verify:
- All 9 stories are linked to the Epic
- Total story points sum to 60
- All stories have complete descriptions
- Acceptance criteria are properly formatted
- Data entities and messages are included

## Key Features Covered

### Meeting Lifecycle
1. **Planning Phase**: Time proposal and voting
2. **Scheduling Phase**: Formal meeting creation
3. **Management Phase**: Modifications and cancellations
4. **Execution Phase**: Live meeting management
5. **Documentation Phase**: Minutes generation and signatures
6. **Viewing Phase**: Meeting list and details

### User Roles Supported
- **Legal Counsel**: Full meeting management capabilities
- **Board Secretary**: Full meeting management capabilities
- **Board Member**: Voting, attendance, and signature capabilities
- **All Users**: Viewing capabilities for associated meetings

### Technical Components
- Meeting time proposal and voting system
- Comprehensive meeting scheduling
- Real-time meeting management
- Rich text minutes editor
- Electronic signature integration
- Notification system
- Audit trail and change logging

## Dependencies and Prerequisites

### System Requirements
- User authentication and role management
- Email notification system
- File upload and storage capabilities
- Electronic signature integration
- Rich text editing capabilities
- PDF generation for final documents

### Database Entities
- Meeting_Time_Proposal
- Proposed_Date
- Proposal_Attachment
- Meeting_Time_Vote
- Meeting
- Meeting_Agenda_Item
- Meeting_Attendee
- Meeting_Minutes
- Minutes_Signature
- Meeting_Change_Log

### Integration Points
- Email system for notifications
- Calendar system for scheduling
- File storage for attachments and PDFs
- Electronic signature service
- Video conferencing (Zoom) for online meetings

## Success Metrics
- Meeting scheduling efficiency
- Voting participation rates
- Minutes completion and signature rates
- User adoption across different roles
- System reliability during live meetings

## Next Steps After Import
1. Review and prioritize stories with product owner
2. Assign stories to development sprints
3. Create technical tasks for each story
4. Set up development and testing environments
5. Begin implementation starting with foundational stories

---

**Ready for Jira Import**
All user stories are documented in `MeetingsUserStoriesForJiraExport.md` with complete specifications ready for development teams.
