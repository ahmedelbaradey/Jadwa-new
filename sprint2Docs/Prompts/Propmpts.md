=======Init Prompt=======
- I want you  to write detailed architecture document (High level , Low level , components , service ) in md format with mermaid chart  and put that document under docs folder 

=======Specific User Story Prompt=======

Based on your comprehensive understanding of the current Jadwa Fund Management System codebase, the Clean Architecture patterns documented in `docs/architecture.md`, and the implementation guidelines in 
`docs/CleanArchitectureSteps.md`,implement the following user story for fund search functionality:

**Implementation Requirements:**
1. **Follow Existing Patterns**: Use the instruction in `docs/CleanArchitectureSteps.md` as a reference template
2. **Clean Architecture Compliance**: Implement using the Generic Repository Pattern with proper CQRS command/query separation
3. **Localization**: Ensure all messages, validation errors, and user-facing text support Arabic/English localization using the existing `_localizer[SharedResourcesKey.{Key}]` pattern
4. **Validation**: Use FluentValidation with localized error messages, following the ValidationBehavior pattern already registered in the DI container
5. **Database Integration**: Integrate with the existing `IRepositoryManager` and `AppDbContext` patterns
6. **API Endpoints**: Create RESTful controllers following the existing controller patterns with proper authentication and authorization
7. **Stories.md Compliance**: Ensure the implementation matches exactly with any specifications in `docs/Stories.md` for fund search functionality
8. **Notification Integration**: If the user story involves notifications, integrate with the existing `FundNotificationService` and Firebase Cloud Messaging system
9. **Build Verification**: Ensure the implementation compiles successfully with the existing codebase without breaking changes
10. **Documentation**: Update relevant documentation files and provide implementation summary

**Context Awareness:**

- The system uses Entity Framework with audit interceptors (no manual audit field setting required)
- Firebase Cloud Messaging is already implemented and integrated for push notifications
- The project follows Clean Architecture with Domain, Application, Infrastructure, and Presentation layers
- All fund-related operations should integrate with the existing Fund entity and its state management patterns
- Use AutoMapper for entity-DTO mapping and maintain the existing DTO inheritance patterns

Please implement the user story with these requirements in mind, ensuring seamless integration with the existing Jadwa API architecture and patterns.

=======Specific Task Prompt=======

- Please review the current implementation status in the file located at `docs/TASK.md` and complete all remaining uncompleted tasks. 
For each uncompleted task:
1. First, read and understand the task requirements from TASK.md
2. Check the current implementation status and identify what work remains
3. Follow the CleanArchitectureSteps.md implementation guide strictly, completing each phase before proceeding to the next
4. Implement the required functionality following existing patterns structed in 
`docs/CleanArchitectureSteps.md`
5. Ensure all handler messages are localized using the _localizer[SharedResourcesKey.{Key}] pattern
6. Use AutoMapper for entity-DTO mapping and FluentValidation for validation
7. Follow the Generic Repository Pattern and facade pattern with IRepositoryManager
8. Update the task status in `docs/TASK.md` to mark completed tasks

Please work through the tasks systematically, respecting any developer assignments and dependency order specified in the task list. 
Focus on implementing the core functionality first before adding advanced features like notifications or complex business logic.

=======Specific Sprint Planning Prompt=======

You are implementing the Jadwa API fund management system following Clean Architecture principles. Based on your understanding of @CleanArchitectureSteps.md as the mandatory implementation guide, execute the following systematic approach:

**Phase 0 Requirements (MUST COMPLETE FIRST):**

1. Review and analyze @CleanArchitectureSteps.md implementation steps
2. Review current codebase structure and existing patterns
3. Analyze `docs/Stories.md` for all fund-related requirements
4. Review `docs/TASK.md` for current task assignments
5. DO NOT proceed to implementation until Phase 0 is completely finished

**Implementation Instructions:**

- Assume developer role: Omnia (highest priority developer)
- Follow CleanArchitectureSteps.md phases strictly (Phase 0 → Phase 1 → Phase 2 → etc.)
- Implement only tasks assigned to Omnia
- Use existing patterns from Categories implementation as reference
- Maintain Generic Repository Pattern and Clean Architecture compliance

**Task Management Requirements:**

1. **Time Tracking**: Provide detailed time consumption report for each task:
   - Task name and description
   - Time spent (hours and minutes)
   - Cumulative total time
   - Phase completion status

2. **Task Reassignment Logic**:
   - Uncheck all uncompleted fund-related tasks
   - Uncheck "Document Management Service Integration" task
   - Reassign tasks based on Stories.md requirements
   - Group ALL tasks from one story to ONE developer (avoid blocking/intersections)
   - Ignore "Fund Code Generation Service" (using auto-increment ID column)
   - Priority order: Omnia > Moustafa > AbdElsalam > Ahmed
   - Order tasks by dependency (prerequisite tasks first)

3. **Update @c:\Workspace\Jadwa-api/docs\TASK.md**:

   - Mark completed tasks as [x]
   - Update task assignments with developer names
   - Reorder tasks by dependency chain
   - Ensure story-based task grouping per developer
**Deliverables:**
1. Complete Phase 0 analysis report
2. Detailed time tracking report
3. Updated TASK.md with new assignments and completion status
4. Implementation progress report for Omnia's assigned tasks

Start with Phase 0 analysis and do not proceed to implementation until explicitly confirmed that Phase 0 is complete.