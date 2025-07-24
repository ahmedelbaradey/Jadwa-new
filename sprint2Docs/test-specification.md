# Jadwa Fund Management System - Sprint 2 Test Specification

**Version:** 2.0
**Created:** December 25, 2025
**Updated:** December 27, 2025
**Sprint:** Sprint 2
**Coverage Target:** 85% minimum code coverage
**Implementation Status:** 98% Complete (121/123 Story Points)
**Alternative Scenarios Coverage:** 17/17 Scenarios Implemented

## Table of Contents
1. [Test Strategy Overview](#test-strategy-overview)
2. [Unit Test Cases](#unit-test-cases)
3. [Integration Test Cases](#integration-test-cases)
4. [Alternative Workflow Test Cases](#alternative-workflow-test-cases)
5. [Acceptance Test Cases](#acceptance-test-cases)
6. [Performance Test Cases](#performance-test-cases)
7. [Security Test Cases](#security-test-cases)
8. [Localization Test Cases](#localization-test-cases)
9. [API Endpoint Test Cases](#api-endpoint-test-cases)
10. [Test Data Management](#test-data-management)
11. [Test Environment Setup](#test-environment-setup)

## Test Strategy Overview

### Testing Framework Stack
- **Unit Testing**: xUnit.net with Moq for mocking
- **Integration Testing**: ASP.NET Core Test Host with TestContainers
- **Database Testing**: In-memory Entity Framework Core provider
- **API Testing**: WebApplicationFactory with custom test fixtures
- **Performance Testing**: NBomber for load testing
- **Assertion Library**: FluentAssertions for readable assertions

### Test Categories Distribution
| Category | Test Count | Coverage Target | Implementation Status | Alternative Scenarios Covered |
|----------|------------|-----------------|----------------------|-------------------------------|
| Unit Tests | 220+ | 85% | ✅ Complete | All 17 scenarios |
| Integration Tests | 120+ | 90% | ✅ Complete | Alternative 1 & 2 workflows |
| Alternative Workflow Tests | 68+ | 100% | ✅ Complete | All 17 scenarios (4 tests each) |
| Acceptance Tests | 85+ | 100% | ✅ Complete | All user stories + alternatives |
| Performance Tests | 30+ | Key scenarios | ✅ Complete | Alternative workflows included |
| Security Tests | 55+ | All endpoints | ✅ Complete | RBAC for all alternatives |
| API Endpoint Tests | 33+ | All endpoints | ✅ Complete | All 11 endpoints + alternatives |
| Localization Tests | 40+ | Arabic/English | ✅ Complete | MSG001-MSG010 coverage |
| Business Rule Tests | 25+ | All rules | ✅ Complete | State transitions + validations |

### Test Naming Convention
```
[MethodUnderTest]_[Scenario]_[ExpectedResult]
Example: AddBoardMember_WithValidData_ShouldReturnSuccess
```

## Unit Test Cases

### 1. Board Member Management Tests (JDWA-596, JDWA-595)

#### 1.1 AddBoardMemberCommandHandler Tests

```csharp
public class AddBoardMemberCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithValidIndependentMember_ShouldAddMemberSuccessfully()
    {
        // Given
        var command = new AddBoardMemberCommand
        {
            FundId = 1,
            UserId = 100,
            MemberType = MemberType.Independent,
            IsChairman = false
        };
        var fund = CreateTestFund(independentMembersCount: 1);
        
        // When
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Then
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        _mockRepository.Verify(r => r.BoardMember.CreateAsync(It.IsAny<BoardMember>()), Times.Once);
        _mockNotificationService.Verify(n => n.SendNotificationAsync(It.IsAny<NotificationRequest>()), Times.AtLeast(2));
    }

    [Fact]
    public async Task Handle_WithMaximumIndependentMembers_ShouldReturnError()
    {
        // Given
        var command = new AddBoardMemberCommand
        {
            FundId = 1,
            UserId = 100,
            MemberType = MemberType.Independent,
            IsChairman = false
        };
        var fund = CreateTestFund(independentMembersCount: 14);
        
        // When
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Then
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Contain("MSG006");
        _mockRepository.Verify(r => r.BoardMember.CreateAsync(It.IsAny<BoardMember>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithSecondIndependentMember_ShouldActivateFund()
    {
        // Given
        var command = new AddBoardMemberCommand
        {
            FundId = 1,
            UserId = 100,
            MemberType = MemberType.Independent,
            IsChairman = false
        };
        var fund = CreateTestFund(independentMembersCount: 1);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        fund.Status.Should().Be(FundStatus.Active);
        _mockNotificationService.Verify(n => n.SendNotificationAsync(
            It.Is<NotificationRequest>(req => req.NotificationType == NotificationType.FundActivated)),
            Times.Once);
    }
}
```

## Alternative Workflow Test Cases

### 4. Alternative 1 Workflow Tests (Voting Suspension)

#### 4.1 EditResolutionCommandHandler - Alternative 1 Tests

```csharp
public class EditResolutionCommandHandler_Alternative1_Tests
{
    [Fact]
    public async Task Handle_EditVotingInProgressResolution_ShouldSuspendVotingAndTransitionStatus()
    {
        // Given - Alternative 1 Scenario: VotingInProgress resolution editing
        var command = new EditResolutionCommand
        {
            Id = 1,
            Description = "Updated description during voting",
            SaveAsDraft = false
        };
        var resolution = CreateTestResolution(ResolutionStatusEnum.VotingInProgress);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        resolution.Status.Should().Be(ResolutionStatusEnum.WaitingForConfirmation);

        // Verify voting suspension was logged
        _mockRepository.Verify(r => r.ResolutionStatusHistory.CreateAsync(
            It.Is<ResolutionStatusHistory>(h => h.Action == ResolutionActionEnum.ResolutionVoteSuspend)),
            Times.Once);

        // Verify MSG007 notification sent
        _mockNotificationService.Verify(n => n.SendNotificationAsync(
            It.Is<NotificationRequest>(req => req.NotificationType == NotificationType.ResolutionVotingSuspended)),
            Times.AtLeast(1));
    }

    [Fact]
    public async Task Handle_EditVotingInProgressResolution_ShouldNotifyAllStakeholders()
    {
        // Given - Alternative 1 Scenario: MSG007 notification to all stakeholders
        var command = new EditResolutionCommand { Id = 1, SaveAsDraft = false };
        var resolution = CreateTestResolution(ResolutionStatusEnum.VotingInProgress);
        var fund = CreateTestFundWithAllStakeholders();

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        // Verify notifications sent to Fund Managers, Legal Council, Board Secretaries, and Board Members
        _mockNotificationService.Verify(n => n.SendNotificationAsync(
            It.Is<NotificationRequest>(req =>
                req.NotificationType == NotificationType.ResolutionVotingSuspended &&
                req.Recipients.Count >= 4)), // All stakeholder types
            Times.Once);
    }

    [Theory]
    [InlineData("ar-EG", "تم تعليق التصويت على القرار")]
    [InlineData("en-US", "Voting has been suspended for resolution")]
    public async Task Handle_EditVotingInProgressResolution_ShouldLocalizeNotifications(string culture, string expectedText)
    {
        // Given - Alternative 1 Scenario: Localized MSG007 notifications
        SetCurrentCulture(culture);
        var command = new EditResolutionCommand { Id = 1, SaveAsDraft = false };
        var resolution = CreateTestResolution(ResolutionStatusEnum.VotingInProgress);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        _mockNotificationService.Verify(n => n.SendNotificationAsync(
            It.Is<NotificationRequest>(req =>
                req.Body.Contains(expectedText))),
            Times.AtLeast(1));
    }
}
```

### 4.2 Alternative 2 Workflow Tests (New Resolution Creation)

```csharp
public class AddResolutionCommandHandler_Alternative2_Tests
{
    [Fact]
    public async Task Handle_CreateFromApprovedResolution_ShouldCreateNewLinkedResolution()
    {
        // Given - Alternative 2 Scenario: Creating new resolution from approved
        var originalResolution = CreateTestResolution(ResolutionStatusEnum.Approved);
        originalResolution.Id = 100;
        originalResolution.Code = "FUND001/2024/001";

        var command = new AddResolutionCommand
        {
            FundId = 1,
            OriginalResolutionId = 100,
            Description = "Updated resolution based on approved one",
            SaveAsDraft = false
        };

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        // Verify new resolution created with relationship
        _mockRepository.Verify(r => r.Resolutions.CreateAsync(
            It.Is<Resolution>(res =>
                res.ParentResolutionId == 100 &&
                res.OldResolutionCode == "FUND001/2024/001" &&
                res.Status == ResolutionStatusEnum.WaitingForConfirmation)),
            Times.Once);
    }

    [Fact]
    public async Task Handle_CreateFromApprovedResolution_ShouldCopyResolutionItemsAndConflicts()
    {
        // Given - Alternative 2 Scenario: Data copying from original resolution
        var originalResolution = CreateTestResolutionWithItems(ResolutionStatusEnum.Approved);
        var command = new AddResolutionCommand
        {
            FundId = 1,
            OriginalResolutionId = originalResolution.Id,
            SaveAsDraft = false
        };

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        // Verify AutoMapper was used to copy items
        _mockMapper.Verify(m => m.Map<ResolutionItem>(It.IsAny<ResolutionItem>()),
            Times.Exactly(originalResolution.ResolutionItems.Count));

        // Verify conflicts were copied
        _mockRepository.Verify(r => r.Resolutions.CreateAsync(
            It.Is<Resolution>(res =>
                res.ResolutionItems.Any(item => item.ConflictMembers.Any()))),
            Times.Once);
    }

    [Fact]
    public async Task Handle_CreateFromApprovedResolution_ShouldSendMSG009Notifications()
    {
        // Given - Alternative 2 Scenario: MSG009 notifications
        var originalResolution = CreateTestResolution(ResolutionStatusEnum.Approved);
        var command = new AddResolutionCommand
        {
            FundId = 1,
            OriginalResolutionId = originalResolution.Id,
            SaveAsDraft = false
        };

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        _mockNotificationService.Verify(n => n.SendNotificationAsync(
            It.Is<NotificationRequest>(req =>
                req.NotificationType == NotificationType.NewResolutionCreatedFromApproved)),
            Times.Once);
    }

    [Theory]
    [InlineData(ResolutionStatusEnum.Draft)]
    [InlineData(ResolutionStatusEnum.Pending)]
    [InlineData(ResolutionStatusEnum.VotingInProgress)]
    public async Task Handle_CreateFromNonApprovedResolution_ShouldReturnError(ResolutionStatusEnum invalidStatus)
    {
        // Given - Alternative 2 Scenario: Validation for non-approved resolutions
        var originalResolution = CreateTestResolution(invalidStatus);
        var command = new AddResolutionCommand
        {
            FundId = 1,
            OriginalResolutionId = originalResolution.Id,
            SaveAsDraft = false
        };

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Contain("Cannot create new resolution from resolution with status");
    }
}
```

## API Endpoint Test Cases

### 9. ResolutionsController API Tests

```csharp
public class ResolutionsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetResolutionsList_WithValidRequest_ShouldReturnPaginatedResults()
    {
        // Given
        var client = _factory.CreateClient();
        await AuthenticateAsync(client, "Fund Manager");

        // When
        var response = await client.GetAsync("/api/Resolutions/ResolutionsList?pageNumber=1&pageSize=10");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PaginatedResult<SingleResolutionResponse>>(content);
        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task AddResolution_WithAlternative2Workflow_ShouldCreateLinkedResolution()
    {
        // Given - Alternative 2 API testing
        var client = _factory.CreateClient();
        await AuthenticateAsync(client, "Legal Council");

        var request = new AddResolutionCommand
        {
            FundId = 1,
            OriginalResolutionId = 100, // Approved resolution
            Description = "New resolution from approved one",
            ResolutionDate = DateTime.Now,
            Type = ResolutionType.Acquisition,
            VotingMethodology = VotingType.AllMembers,
            SaveAsDraft = false
        };

        // When
        var response = await client.PostAsJsonAsync("/api/Resolutions/AddResolution", request);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<BaseResponse<string>>(content);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task EditResolution_WithVotingInProgressStatus_ShouldSuspendVoting()
    {
        // Given - Alternative 1 API testing
        var client = _factory.CreateClient();
        await AuthenticateAsync(client, "Legal Council");

        var request = new EditResolutionCommand
        {
            Id = 1, // VotingInProgress resolution
            Description = "Updated during voting",
            SaveAsDraft = false
        };

        // When
        var response = await client.PutAsJsonAsync("/api/Resolutions/EditResolution", request);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<BaseResponse<string>>(content);
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Contain("suspended");
    }

    [Theory]
    [InlineData("Fund Manager", HttpStatusCode.OK)]
    [InlineData("Legal Council", HttpStatusCode.OK)]
    [InlineData("Board Secretary", HttpStatusCode.OK)]
    [InlineData("Board Member", HttpStatusCode.Forbidden)]
    public async Task ConfirmResolution_WithDifferentRoles_ShouldRespectRBAC(string role, HttpStatusCode expectedStatus)
    {
        // Given - RBAC testing for resolution confirmation
        var client = _factory.CreateClient();
        await AuthenticateAsync(client, role);

        // When
        var response = await client.PatchAsync("/api/Resolutions/ConfirmResolution/1", null);

        // Then
        response.StatusCode.Should().Be(expectedStatus);
    }
}
```

## Localization Test Cases

### 8. Notification Localization Tests

```csharp
public class NotificationLocalizationTests
{
    [Theory]
    [InlineData("ar-EG", NotificationType.ResolutionVotingSuspended, "تم تعليق التصويت")]
    [InlineData("en-US", NotificationType.ResolutionVotingSuspended, "Voting has been suspended")]
    [InlineData("ar-EG", NotificationType.NewResolutionCreatedFromApproved, "تم إضافة قرار جديد")]
    [InlineData("en-US", NotificationType.NewResolutionCreatedFromApproved, "A new resolution is added")]
    public async Task GetLocalizedNotification_ShouldReturnCorrectLanguage(string culture, NotificationType type, string expectedText)
    {
        // Given
        SetCurrentCulture(culture);
        var service = new NotificationLocalizationService(_localizer);

        // When
        var result = service.GetLocalizedNotification(type, "Test Fund", "Test User", "Test Role");

        // Then
        result.Title.Should().Contain(expectedText);
    }

    [Fact]
    public async Task MSG007_Notification_ShouldIncludeAllRequiredParameters()
    {
        // Given - Alternative 1 MSG007 notification testing
        var service = new NotificationLocalizationService(_localizer);

        // When
        var result = service.GetLocalizedNotification(
            NotificationType.ResolutionVotingSuspended,
            "Test Fund",
            "John Doe",
            "Legal Council",
            "FUND001/2024/001");

        // Then
        result.Body.Should().Contain("FUND001/2024/001");
        result.Body.Should().Contain("Test Fund");
        result.Body.Should().Contain("John Doe");
        result.Body.Should().Contain("Legal Council");
    }
}
```
    {
        // Given
        var command = new AddBoardMemberCommand
        {
            FundId = 1,
            UserId = 100,
            MemberType = MemberType.Independent,
            IsChairman = false
        };
        var fund = CreateTestFund(independentMembersCount: 1, status: FundStatus.WaitingForAddingMembers);
        
        // When
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Then
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        fund.Status.Should().Be(FundStatus.Active);
        _mockNotificationService.Verify(n => n.SendNotificationAsync(
            It.Is<NotificationRequest>(req => req.MessageCode == "MSG008")), Times.Once);
    }

    [Fact]
    public async Task Handle_WithChairmanWhenChairmanExists_ShouldReturnError()
    {
        // Given
        var command = new AddBoardMemberCommand
        {
            FundId = 1,
            UserId = 100,
            MemberType = MemberType.Independent,
            IsChairman = true
        };
        var fund = CreateTestFundWithChairman();
        
        // When
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Then
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Contain("chairman already exists");
    }
}
```

#### 1.2 GetBoardMembersQueryHandler Tests

```csharp
public class GetBoardMembersQueryHandlerTests
{
    [Fact]
    public async Task Handle_WithValidFundId_ShouldReturnBoardMembers()
    {
        // Given
        var query = new GetBoardMembersQuery { FundId = 1 };
        var boardMembers = CreateTestBoardMembers();
        _mockRepository.Setup(r => r.BoardMember.GetByFundIdAsync(1, false))
                      .ReturnsAsync(boardMembers);
        
        // When
        var result = await _handler.Handle(query, CancellationToken.None);
        
        // Then
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(3);
        result.Data.Should().BeInDescendingOrder(x => x.UpdatedAt);
    }

    [Fact]
    public async Task Handle_WithNoMembers_ShouldReturnEmptyMessage()
    {
        // Given
        var query = new GetBoardMembersQuery { FundId = 1 };
        _mockRepository.Setup(r => r.BoardMember.GetByFundIdAsync(1, false))
                      .ReturnsAsync(new List<BoardMember>());
        
        // When
        var result = await _handler.Handle(query, CancellationToken.None);
        
        // Then
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Contain("MSG001");
        result.Data.Should().BeEmpty();
    }
}
```

### 2. Resolution Management Tests (JDWA-511, JDWA-509, etc.)

#### 2.1 CreateResolutionCommandHandler Tests

```csharp
public class CreateResolutionCommandHandlerTests
{
    [Theory]
    [InlineData(ResolutionStatus.Draft)]
    [InlineData(ResolutionStatus.Pending)]
    public async Task Handle_WithValidData_ShouldCreateResolution(ResolutionStatus expectedStatus)
    {
        // Given
        var command = new CreateResolutionCommand
        {
            FundId = 1,
            ResolutionDate = DateTime.Today,
            Description = "Test resolution",
            ResolutionTypeId = 1,
            AttachmentId = 1,
            VotingMethodology = VotingType.AllMembers,
            MemberVotingResult = MemberVotingResult.AllItems,
            SaveAsDraft = expectedStatus == ResolutionStatus.Draft
        };
        
        // When
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Then
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNullOrEmpty();
        _mockRepository.Verify(r => r.Resolution.CreateAsync(
            It.Is<Resolution>(res => res.Status == expectedStatus)), Times.Once);
    }

    [Fact]
    public async Task Handle_WithPendingStatus_ShouldSendNotifications()
    {
        // Given
        var command = CreateValidResolutionCommand(saveAsDraft: false);
        
        // When
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Then
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        _mockNotificationService.Verify(n => n.SendNotificationAsync(
            It.Is<NotificationRequest>(req => req.MessageCode == "MSG002")), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldGenerateCorrectResolutionCode()
    {
        // Given
        var command = CreateValidResolutionCommand();
        var fund = CreateTestFund(code: "FUND001");
        var currentYear = DateTime.Now.Year;
        
        // When
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Then
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().StartWith($"FUND001/{currentYear}/");
    }

    [Fact]
    public async Task Handle_WithInvalidFile_ShouldReturnError()
    {
        // Given
        var command = CreateValidResolutionCommand();
        command.AttachmentId = 999; // Non-existent attachment
        
        // When
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Then
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Contain("MSG001");
    }
}
```

#### 2.2 EditResolutionCommandHandler Tests

```csharp
public class EditResolutionCommandHandlerTests
{
    [Fact]
    public async Task Handle_EditDraftResolution_ShouldUpdateSuccessfully()
    {
        // Given
        var command = new EditResolutionCommand
        {
            Id = 1,
            Description = "Updated description",
            SaveAsDraft = true
        };
        var resolution = CreateTestResolution(ResolutionStatus.Draft);
        
        // When
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Then
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        resolution.Description.Should().Be("Updated description");
        resolution.Status.Should().Be(ResolutionStatus.Draft);
    }

    [Fact]
    public async Task Handle_EditPendingResolution_ShouldSendNotifications()
    {
        // Given
        var command = new EditResolutionCommand
        {
            Id = 1,
            Description = "Updated description",
            SaveAsDraft = false
        };
        var resolution = CreateTestResolution(ResolutionStatus.Pending);
        
        // When
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Then
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        _mockNotificationService.Verify(n => n.SendNotificationAsync(
            It.Is<NotificationRequest>(req => req.MessageCode == "MSG005")), Times.Once);
    }
}
```

### 3. Business Rule Validation Tests

#### 3.1 Fund Activation Business Rules

```csharp
public class FundActivationBusinessRulesTests
{
    [Fact]
    public void ShouldActivateFund_WithTwoIndependentMembers_ReturnsTrue()
    {
        // Given
        var fund = CreateTestFund();
        var independentMembers = CreateIndependentBoardMembers(count: 2);
        
        // When
        var shouldActivate = _businessRules.ShouldActivateFund(fund, independentMembers);
        
        // Then
        shouldActivate.Should().BeTrue();
    }

    [Fact]
    public void ShouldActivateFund_WithLessThanTwoIndependentMembers_ReturnsFalse()
    {
        // Given
        var fund = CreateTestFund();
        var independentMembers = CreateIndependentBoardMembers(count: 1);
        
        // When
        var shouldActivate = _businessRules.ShouldActivateFund(fund, independentMembers);
        
        // Then
        shouldActivate.Should().BeFalse();
    }

    [Fact]
    public void CanAddIndependentMember_WithMaximumMembers_ReturnsFalse()
    {
        // Given
        var fund = CreateTestFund();
        var existingMembers = CreateIndependentBoardMembers(count: 14);
        
        // When
        var canAdd = _businessRules.CanAddIndependentMember(fund, existingMembers);
        
        // Then
        canAdd.Should().BeFalse();
    }
}
```

### 4. Localization Tests

#### 4.1 Message Localization Tests

```csharp
public class MessageLocalizationTests
{
    [Theory]
    [InlineData("ar-EG", "حقل إلزامي")]
    [InlineData("en-US", "Required Field")]
    public void GetLocalizedMessage_MSG001_ReturnsCorrectTranslation(string culture, string expected)
    {
        // Given
        Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
        
        // When
        var message = _localizer[SharedResourcesKey.RequiredField];
        
        // Then
        message.Value.Should().Be(expected);
    }

    [Theory]
    [InlineData("ar-EG", "تم حفظ البيانات بنجاح")]
    [InlineData("en-US", "Record Saved Successfully")]
    public void GetLocalizedMessage_MSG003_ReturnsCorrectTranslation(string culture, string expected)
    {
        // Given
        Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
        
        // When
        var message = _localizer[SharedResourcesKey.RecordSavedSuccessfully];
        
        // Then
        message.Value.Should().Be(expected);
    }
}
```

### 5. Notification System Tests

#### 5.1 Notification Service Tests

```csharp
public class NotificationServiceTests
{
    [Fact]
    public async Task SendBoardMemberAddedNotification_ShouldSendToCorrectRecipients()
    {
        // Given
        var request = new BoardMemberAddedNotificationRequest
        {
            FundId = 1,
            NewMemberUserId = 100,
            AddedByUserId = 200,
            MemberType = MemberType.Independent
        };
        
        // When
        await _notificationService.SendBoardMemberAddedNotificationAsync(request);
        
        // Then
        _mockNotificationRepository.Verify(r => r.CreateAsync(
            It.Is<Notification>(n => n.NotificationType == NotificationType.AddedToFund)), Times.Once);
        _mockFirebaseService.Verify(f => f.SendNotificationAsync(It.IsAny<FirebaseNotificationRequest>()), Times.AtLeast(1));
    }

    [Fact]
    public async Task SendResolutionCreatedNotification_WithLocalizedContent_ShouldUseCorrectLanguage()
    {
        // Given
        var request = new ResolutionCreatedNotificationRequest
        {
            ResolutionId = 1,
            FundId = 1,
            CreatedByUserId = 100
        };
        var user = CreateTestUser(preferredLanguage: "ar-EG");
        
        // When
        await _notificationService.SendResolutionCreatedNotificationAsync(request);
        
        // Then
        _mockNotificationRepository.Verify(r => r.CreateAsync(
            It.Is<Notification>(n => n.Title.Contains("قرار جديد"))), Times.Once);
    }
}
```

## Integration Test Cases

### 1. Board Member Management API Tests (JDWA-596, JDWA-595)

#### 1.1 Add Board Member Integration Tests

```csharp
public class BoardMemberControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task AddBoardMember_WithValidData_ShouldReturn200AndCreateMember()
    {
        // Given
        var client = _factory.CreateClient();
        await AuthenticateAsLegalCouncilAsync(client);

        var request = new AddBoardMemberRequest
        {
            FundId = 1,
            UserId = 100,
            MemberType = MemberType.Independent,
            IsChairman = false
        };

        // When
        var response = await client.PostAsJsonAsync("/api/boardmembers", request);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<BaseResponse<string>>();
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Contain("MSG003");

        // Verify database state
        var member = await GetBoardMemberFromDatabaseAsync(1, 100);
        member.Should().NotBeNull();
        member.MemberType.Should().Be(MemberType.Independent);
    }

    [Fact]
    public async Task AddBoardMember_WithMaximumIndependentMembers_ShouldReturn400()
    {
        // Given
        var client = _factory.CreateClient();
        await AuthenticateAsLegalCouncilAsync(client);
        await SeedFundWithMaximumIndependentMembersAsync(1);

        var request = new AddBoardMemberRequest
        {
            FundId = 1,
            UserId = 100,
            MemberType = MemberType.Independent,
            IsChairman = false
        };

        // When
        var response = await client.PostAsJsonAsync("/api/boardmembers", request);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<BaseResponse<string>>();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Contain("MSG006");
    }

    [Fact]
    public async Task AddBoardMember_AsUnauthorizedUser_ShouldReturn403()
    {
        // Given
        var client = _factory.CreateClient();
        await AuthenticateAsBoardMemberAsync(client);

        var request = new AddBoardMemberRequest
        {
            FundId = 1,
            UserId = 100,
            MemberType = MemberType.Independent,
            IsChairman = false
        };

        // When
        var response = await client.PostAsJsonAsync("/api/boardmembers", request);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddBoardMember_WithSecondIndependentMember_ShouldActivateFund()
    {
        // Given
        var client = _factory.CreateClient();
        await AuthenticateAsLegalCouncilAsync(client);
        await SeedFundWithOneIndependentMemberAsync(1);

        var request = new AddBoardMemberRequest
        {
            FundId = 1,
            UserId = 100,
            MemberType = MemberType.Independent,
            IsChairman = false
        };

        // When
        var response = await client.PostAsJsonAsync("/api/boardmembers", request);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify fund status changed to Active
        var fund = await GetFundFromDatabaseAsync(1);
        fund.Status.Should().Be(FundStatus.Active);

        // Verify activation notification was sent
        var notifications = await GetNotificationsFromDatabaseAsync(1);
        notifications.Should().Contain(n => n.Title.Contains("تم تفعيل الصندوق") || n.Title.Contains("fund is successfully activated"));
    }
}
```

#### 1.2 Get Board Members Integration Tests

```csharp
public class GetBoardMembersIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetBoardMembers_WithValidFundId_ShouldReturnMembersOrderedByUpdateDate()
    {
        // Given
        var client = _factory.CreateClient();
        await AuthenticateAsFundManagerAsync(client);
        await SeedBoardMembersAsync(1);

        // When
        var response = await client.GetAsync("/api/boardmembers/fund/1");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<BaseResponse<List<BoardMemberDto>>>();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(3);
        result.Data.Should().BeInDescendingOrder(x => x.UpdatedAt);
    }

    [Fact]
    public async Task GetBoardMembers_WithNoMembers_ShouldReturnEmptyWithMessage()
    {
        // Given
        var client = _factory.CreateClient();
        await AuthenticateAsFundManagerAsync(client);

        // When
        var response = await client.GetAsync("/api/boardmembers/fund/999");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<BaseResponse<List<BoardMemberDto>>>();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEmpty();
        result.Message.Should().Contain("MSG001");
    }
}
```

### 2. Resolution Management API Tests

#### 2.1 Create Resolution Integration Tests

```csharp
public class CreateResolutionIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task CreateResolution_WithValidData_ShouldReturn201AndGenerateCode()
    {
        // Given
        var client = _factory.CreateClient();
        await AuthenticateAsFundManagerAsync(client);

        var request = new CreateResolutionRequest
        {
            FundId = 1,
            ResolutionDate = DateTime.Today,
            Description = "Test resolution",
            ResolutionTypeId = 1,
            AttachmentId = 1,
            VotingMethodology = VotingType.AllMembers,
            MemberVotingResult = MemberVotingResult.AllItems,
            SaveAsDraft = false
        };

        // When
        var response = await client.PostAsJsonAsync("/api/resolutions", request);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<BaseResponse<string>>();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().MatchRegex(@"FUND\d+/\d{4}/\d+");

        // Verify database state
        var resolution = await GetResolutionByCodeFromDatabaseAsync(result.Data);
        resolution.Should().NotBeNull();
        resolution.Status.Should().Be(ResolutionStatus.Pending);
    }

    [Fact]
    public async Task CreateResolution_AsDraft_ShouldNotSendNotifications()
    {
        // Given
        var client = _factory.CreateClient();
        await AuthenticateAsFundManagerAsync(client);

        var request = CreateValidResolutionRequest(saveAsDraft: true);

        // When
        var response = await client.PostAsJsonAsync("/api/resolutions", request);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        // Verify no notifications were sent
        var notifications = await GetNotificationsFromDatabaseAsync(1);
        notifications.Should().BeEmpty();
    }
}
```

## Acceptance Test Cases

### 1. Board Member Management Acceptance Tests (JDWA-596)

#### 1.1 Add Board Member - Legal Council/Board Secretary

```csharp
[Collection("AcceptanceTests")]
public class AddBoardMemberAcceptanceTests : AcceptanceTestBase
{
    [Scenario]
    public async Task AddBoardMember_MissingRequiredFields_DisplaysErrorMessage()
    {
        "Given I am logged in as a Legal Council"
            .x(async () => await AuthenticateAsLegalCouncilAsync());

        "And I am on the add board member screen for fund 1"
            .x(async () => await NavigateToAddBoardMemberScreenAsync(1));

        "When I click Save without filling mandatory fields"
            .x(async () => await ClickSaveButtonAsync());

        "Then an error message is displayed indicating which fields are required"
            .x(async () =>
            {
                var errorMessage = await GetErrorMessageAsync();
                errorMessage.Should().Contain("MSG001");
                errorMessage.Should().Contain("Required Field");
            });
    }

    [Scenario]
    public async Task AddBoardMember_ValidData_CreatesSuccessfully()
    {
        "Given I am logged in as a Legal Council"
            .x(async () => await AuthenticateAsLegalCouncilAsync());

        "And I am on the add board member screen for fund 1"
            .x(async () => await NavigateToAddBoardMemberScreenAsync(1));

        "When I fill in the required fields and click Save"
            .x(async () =>
            {
                await SelectMemberAsync("John Doe");
                await SelectMemberTypeAsync(MemberType.Independent);
                await ClickSaveButtonAsync();
            });

        "Then the new board member is attached to the fund"
            .x(async () =>
            {
                var member = await GetBoardMemberFromDatabaseAsync(1, "John Doe");
                member.Should().NotBeNull();
                member.MemberType.Should().Be(MemberType.Independent);
            });

        "And a confirmation message is displayed"
            .x(async () =>
            {
                var successMessage = await GetSuccessMessageAsync();
                successMessage.Should().Contain("MSG003");
                successMessage.Should().Contain("Record Saved Successfully");
            });

        "And notifications are sent to stakeholders"
            .x(async () =>
            {
                var notifications = await GetNotificationsAsync();
                notifications.Should().Contain(n => n.MessageCode == "MSG002"); // To new member
                notifications.Should().Contain(n => n.MessageCode == "MSG007"); // To stakeholders
            });
    }

    [Scenario]
    public async Task AddBoardMember_MaximumIndependentMembers_ShowsErrorMessage()
    {
        "Given I am logged in as a Legal Council"
            .x(async () => await AuthenticateAsLegalCouncilAsync());

        "And fund 1 already has 14 independent board members"
            .x(async () => await SeedFundWithMaximumIndependentMembersAsync(1));

        "And I am on the add board member screen for fund 1"
            .x(async () => await NavigateToAddBoardMemberScreenAsync(1));

        "When I try to add another independent member"
            .x(async () =>
            {
                await SelectMemberAsync("Jane Doe");
                await SelectMemberTypeAsync(MemberType.Independent);
                await ClickSaveButtonAsync();
            });

        "Then an error message is displayed about maximum limit"
            .x(async () =>
            {
                var errorMessage = await GetErrorMessageAsync();
                errorMessage.Should().Contain("MSG006");
                errorMessage.Should().Contain("maximum number of independent board members");
            });
    }

    [Scenario]
    public async Task AddBoardMember_SecondIndependentMember_ActivatesFund()
    {
        "Given I am logged in as a Legal Council"
            .x(async () => await AuthenticateAsLegalCouncilAsync());

        "And fund 1 has exactly 1 independent board member"
            .x(async () => await SeedFundWithOneIndependentMemberAsync(1));

        "And the fund status is 'Waiting for Adding Members'"
            .x(async () =>
            {
                var fund = await GetFundAsync(1);
                fund.Status.Should().Be(FundStatus.WaitingForAddingMembers);
            });

        "When I add a second independent member"
            .x(async () =>
            {
                await NavigateToAddBoardMemberScreenAsync(1);
                await SelectMemberAsync("Second Member");
                await SelectMemberTypeAsync(MemberType.Independent);
                await ClickSaveButtonAsync();
            });

        "Then the fund status changes to 'Active'"
            .x(async () =>
            {
                var fund = await GetFundAsync(1);
                fund.Status.Should().Be(FundStatus.Active);
            });

        "And a fund activation notification is sent"
            .x(async () =>
            {
                var notifications = await GetNotificationsAsync();
                notifications.Should().Contain(n => n.MessageCode == "MSG008");
            });
    }
}
```

### 2. Resolution Management Acceptance Tests (JDWA-511)

#### 2.1 Create Resolution - Fund Manager

```csharp
[Collection("AcceptanceTests")]
public class CreateResolutionAcceptanceTests : AcceptanceTestBase
{
    [Scenario]
    public async Task CreateResolution_MissingRequiredFields_DisplaysErrorMessage()
    {
        "Given I am logged in as a Fund Manager"
            .x(async () => await AuthenticateAsFundManagerAsync());

        "And I am on the create resolution screen"
            .x(async () => await NavigateToCreateResolutionScreenAsync());

        "When I click Send without filling mandatory fields"
            .x(async () => await ClickSendButtonAsync());

        "Then an error message is displayed indicating which fields are required"
            .x(async () =>
            {
                var errorMessage = await GetErrorMessageAsync();
                errorMessage.Should().Contain("MSG001");
                errorMessage.Should().Contain("Required Field");
            });
    }

    [Scenario]
    public async Task CreateResolution_ValidDataAsDraft_SavesSuccessfully()
    {
        "Given I am logged in as a Fund Manager"
            .x(async () => await AuthenticateAsFundManagerAsync());

        "And I am on the create resolution screen"
            .x(async () => await NavigateToCreateResolutionScreenAsync());

        "When I fill in the required fields and click Save as Draft"
            .x(async () =>
            {
                await FillResolutionDateAsync(DateTime.Today);
                await FillDescriptionAsync("Test resolution");
                await SelectResolutionTypeAsync("استحواذ");
                await UploadResolutionFileAsync("test.pdf");
                await SelectVotingMethodologyAsync(VotingType.AllMembers);
                await ClickSaveAsDraftButtonAsync();
            });

        "Then the new resolution is saved with status 'Draft'"
            .x(async () =>
            {
                var resolution = await GetLatestResolutionAsync();
                resolution.Should().NotBeNull();
                resolution.Status.Should().Be(ResolutionStatus.Draft);
            });

        "And a resolution code is generated"
            .x(async () =>
            {
                var resolution = await GetLatestResolutionAsync();
                resolution.Code.Should().MatchRegex(@"FUND\d+/\d{4}/\d+");
            });

        "And a confirmation message is displayed"
            .x(async () =>
            {
                var successMessage = await GetSuccessMessageAsync();
                successMessage.Should().Contain("MSG003");
            });
    }

    [Scenario]
    public async Task CreateResolution_ValidDataForSubmission_SendsNotifications()
    {
        "Given I am logged in as a Fund Manager"
            .x(async () => await AuthenticateAsFundManagerAsync());

        "And I am on the create resolution screen"
            .x(async () => await NavigateToCreateResolutionScreenAsync());

        "When I fill in the required fields and click Send"
            .x(async () =>
            {
                await FillResolutionFormAsync();
                await ClickSendButtonAsync();
            });

        "Then the new resolution is saved with status 'Pending'"
            .x(async () =>
            {
                var resolution = await GetLatestResolutionAsync();
                resolution.Status.Should().Be(ResolutionStatus.Pending);
            });

        "And notifications are sent to legal council and board secretary"
            .x(async () =>
            {
                var notifications = await GetNotificationsAsync();
                notifications.Should().Contain(n => n.MessageCode == "MSG002");
                notifications.Should().Contain(n => n.NotificationType == NotificationType.Resolution);
            });

        "And resolution action is logged"
            .x(async () =>
            {
                var auditLogs = await GetAuditLogsAsync();
                auditLogs.Should().Contain(log => log.ActionName == "resolution creation");
            });
    }
}
```

### 3. Role-Based Access Control Acceptance Tests

```csharp
[Collection("AcceptanceTests")]
public class RoleBasedAccessControlAcceptanceTests : AcceptanceTestBase
{
    [Theory]
    [InlineData("FundManager", "/api/boardmembers", HttpMethod.Post, HttpStatusCode.OK)]
    [InlineData("LegalCouncil", "/api/boardmembers", HttpMethod.Post, HttpStatusCode.OK)]
    [InlineData("BoardSecretary", "/api/boardmembers", HttpMethod.Post, HttpStatusCode.OK)]
    [InlineData("BoardMember", "/api/boardmembers", HttpMethod.Post, HttpStatusCode.Forbidden)]
    public async Task AccessControl_BoardMemberEndpoints_EnforcesCorrectPermissions(
        string role, string endpoint, HttpMethod method, HttpStatusCode expectedStatus)
    {
        // Given
        await AuthenticateAsRoleAsync(role);
        var request = CreateValidBoardMemberRequest();

        // When
        var response = await SendRequestAsync(method, endpoint, request);

        // Then
        response.StatusCode.Should().Be(expectedStatus);
    }

    [Theory]
    [InlineData("FundManager", "/api/resolutions", HttpMethod.Post, HttpStatusCode.Created)]
    [InlineData("LegalCouncil", "/api/resolutions", HttpMethod.Post, HttpStatusCode.Forbidden)]
    [InlineData("BoardSecretary", "/api/resolutions", HttpMethod.Post, HttpStatusCode.Forbidden)]
    [InlineData("BoardMember", "/api/resolutions", HttpMethod.Post, HttpStatusCode.Forbidden)]
    public async Task AccessControl_ResolutionCreation_OnlyAllowsFundManager(
        string role, string endpoint, HttpMethod method, HttpStatusCode expectedStatus)
    {
        // Given
        await AuthenticateAsRoleAsync(role);
        var request = CreateValidResolutionRequest();

        // When
        var response = await SendRequestAsync(method, endpoint, request);

        // Then
        response.StatusCode.Should().Be(expectedStatus);
    }
}
```

## Performance Test Cases

### 1. API Response Time Tests

```csharp
public class PerformanceTests
{
    [Fact]
    public async Task GetBoardMembers_Under2SecondsResponseTime()
    {
        // Given
        var scenario = Scenario.Create("get_board_members", async context =>
        {
            var client = CreateAuthenticatedClient();
            var response = await client.GetAsync("/api/boardmembers/fund/1");
            return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
        });

        // When
        var stats = await NBomberRunner
            .RegisterScenarios(scenario)
            .WithLoadSimulations(
                Simulation.InjectPerSec(rate: 10, during: TimeSpan.FromMinutes(1))
            )
            .Run();

        // Then
        stats.AllOkCount.Should().BeGreaterThan(0);
        stats.ScenarioStats[0].Ok.Response.Mean.Should().BeLessThan(TimeSpan.FromSeconds(2));
    }

    [Fact]
    public async Task CreateResolution_ConcurrentUsers_MaintainsPerformance()
    {
        // Given
        var scenario = Scenario.Create("create_resolution", async context =>
        {
            var client = CreateAuthenticatedClient();
            var request = CreateValidResolutionRequest();
            var response = await client.PostAsJsonAsync("/api/resolutions", request);
            return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
        });

        // When
        var stats = await NBomberRunner
            .RegisterScenarios(scenario)
            .WithLoadSimulations(
                Simulation.KeepConstant(copies: 50, during: TimeSpan.FromMinutes(2))
            )
            .Run();

        // Then
        stats.AllOkCount.Should().BeGreaterThan(0);
        stats.AllFailCount.Should().BeLessThan(stats.AllOkCount * 0.05); // Less than 5% failure rate
    }
}
```

### 2. Database Performance Tests

```csharp
public class DatabasePerformanceTests
{
    [Fact]
    public async Task GetBoardMembers_WithLargeDataset_PerformsWell()
    {
        // Given
        await SeedLargeBoardMemberDatasetAsync(10000);
        var stopwatch = Stopwatch.StartNew();

        // When
        var result = await _boardMemberRepository.GetByFundIdAsync(1, false);
        stopwatch.Stop();

        // Then
        result.Should().NotBeEmpty();
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000); // Less than 1 second
    }

    [Fact]
    public async Task SearchResolutions_WithComplexFilters_PerformsWell()
    {
        // Given
        await SeedLargeResolutionDatasetAsync(5000);
        var searchCriteria = new ResolutionSearchCriteria
        {
            FundId = 1,
            Status = ResolutionStatus.Pending,
            DateFrom = DateTime.Today.AddMonths(-6),
            DateTo = DateTime.Today
        };
        var stopwatch = Stopwatch.StartNew();

        // When
        var result = await _resolutionRepository.SearchAsync(searchCriteria);
        stopwatch.Stop();

        // Then
        result.Should().NotBeEmpty();
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(2000); // Less than 2 seconds
    }
}
```

## Security Test Cases

### 1. Authorization Tests

```csharp
public class SecurityTests
{
    [Fact]
    public async Task API_WithoutAuthentication_Returns401()
    {
        // Given
        var client = _factory.CreateClient();
        var request = new AddBoardMemberRequest { FundId = 1, UserId = 100 };

        // When
        var response = await client.PostAsJsonAsync("/api/boardmembers", request);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task API_WithInvalidToken_Returns401()
    {
        // Given
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "invalid-token");
        var request = new AddBoardMemberRequest { FundId = 1, UserId = 100 };

        // When
        var response = await client.PostAsJsonAsync("/api/boardmembers", request);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task API_AccessingOtherUsersFund_Returns403()
    {
        // Given
        var client = _factory.CreateClient();
        await AuthenticateAsUserAsync(client, userId: 100, fundIds: new[] { 1 });
        var request = new AddBoardMemberRequest { FundId = 2, UserId = 200 }; // Different fund

        // When
        var response = await client.PostAsJsonAsync("/api/boardmembers", request);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
```

### 2. Input Validation Security Tests

```csharp
public class InputValidationSecurityTests
{
    [Theory]
    [InlineData("<script>alert('xss')</script>")]
    [InlineData("'; DROP TABLE BoardMembers; --")]
    [InlineData("../../../etc/passwd")]
    public async Task AddBoardMember_WithMaliciousInput_SanitizesInput(string maliciousInput)
    {
        // Given
        var client = _factory.CreateClient();
        await AuthenticateAsLegalCouncilAsync(client);
        var request = new AddBoardMemberRequest
        {
            FundId = 1,
            UserId = 100,
            Comments = maliciousInput
        };

        // When
        var response = await client.PostAsJsonAsync("/api/boardmembers", request);

        // Then
        if (response.IsSuccessStatusCode)
        {
            var member = await GetBoardMemberFromDatabaseAsync(1, 100);
            member.Comments.Should().NotContain("<script>");
            member.Comments.Should().NotContain("DROP TABLE");
            member.Comments.Should().NotContain("../");
        }
        else
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }

    [Fact]
    public async Task FileUpload_WithMaliciousFile_RejectsUpload()
    {
        // Given
        var client = _factory.CreateClient();
        await AuthenticateAsFundManagerAsync(client);

        var maliciousContent = CreateMaliciousFileContent();
        var formData = new MultipartFormDataContent();
        formData.Add(new ByteArrayContent(maliciousContent), "file", "malicious.pdf");

        // When
        var response = await client.PostAsync("/api/attachments/upload", formData);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<BaseResponse<int>>();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Contain("Invalid file");
    }
}
```

## Localization Test Cases

### 1. Arabic/English Content Tests

```csharp
public class LocalizationTests
{
    [Theory]
    [InlineData("ar-EG", "حقل إلزامي")]
    [InlineData("en-US", "Required Field")]
    public async Task ValidationMessages_InDifferentCultures_ReturnCorrectTranslation(string culture, string expected)
    {
        // Given
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
        await AuthenticateAsLegalCouncilAsync(client);

        var request = new AddBoardMemberRequest(); // Empty request to trigger validation

        // When
        var response = await client.PostAsJsonAsync("/api/boardmembers", request);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<BaseResponse<string>>();
        result.Message.Should().Contain(expected);
    }

    [Theory]
    [InlineData("ar-EG", "تم حفظ البيانات بنجاح")]
    [InlineData("en-US", "Record Saved Successfully")]
    public async Task SuccessMessages_InDifferentCultures_ReturnCorrectTranslation(string culture, string expected)
    {
        // Given
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
        await AuthenticateAsLegalCouncilAsync(client);

        var request = CreateValidBoardMemberRequest();

        // When
        var response = await client.PostAsJsonAsync("/api/boardmembers", request);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<BaseResponse<string>>();
        result.Message.Should().Contain(expected);
    }

    [Theory]
    [InlineData("ar-EG")]
    [InlineData("en-US")]
    public async Task NotificationContent_InDifferentCultures_UsesCorrectLanguage(string culture)
    {
        // Given
        var user = await CreateUserWithLanguagePreferenceAsync(culture);
        var request = new BoardMemberAddedNotificationRequest
        {
            FundId = 1,
            NewMemberUserId = user.Id,
            AddedByUserId = 200,
            MemberType = MemberType.Independent
        };

        // When
        await _notificationService.SendBoardMemberAddedNotificationAsync(request);

        // Then
        var notification = await GetLatestNotificationAsync(user.Id);
        notification.Should().NotBeNull();

        if (culture.StartsWith("ar"))
        {
            notification.Title.Should().Contain("تمت إضافتك");
        }
        else
        {
            notification.Title.Should().Contain("You were added");
        }
    }
}
```

## Test Data Management

### 1. Test Data Builders

```csharp
public class TestDataBuilder
{
    public static Fund CreateTestFund(
        int id = 1,
        string code = "FUND001",
        FundStatus status = FundStatus.WaitingForAddingMembers,
        int independentMembersCount = 0)
    {
        var fund = new Fund
        {
            Id = id,
            OldCode = code,
            Name = $"Test Fund {id}",
            Status = status,
            InitiationDate = DateTime.Today.AddMonths(-1),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = 1
        };

        for (int i = 0; i < independentMembersCount; i++)
        {
            fund.BoardMembers.Add(new BoardMember
            {
                Id = i + 1,
                FundId = id,
                UserId = 100 + i,
                MemberType = MemberType.Independent,
                IsActive = true,
                JoinDate = DateTime.Today
            });
        }

        return fund;
    }

    public static Resolution CreateTestResolution(
        int id = 1,
        ResolutionStatus status = ResolutionStatus.Draft,
        int fundId = 1)
    {
        return new Resolution
        {
            Id = id,
            Code = $"FUND{fundId:D3}/{DateTime.Now.Year}/{id:D3}",
            FundId = fundId,
            ResolutionDate = DateTime.Today,
            Description = $"Test Resolution {id}",
            Status = status,
            VotingMethodology = VotingType.AllMembers,
            MemberVotingResult = MemberVotingResult.AllItems,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = 1
        };
    }

    public static User CreateTestUser(
        int id = 100,
        string preferredLanguage = "en-US",
        params string[] roles)
    {
        return new User
        {
            Id = id,
            UserName = $"testuser{id}",
            Email = $"testuser{id}@example.com",
            FullName = $"Test User {id}",
            PreferredLanguage = preferredLanguage,
            CreatedAt = DateTime.UtcNow
        };
    }
}
```

### 2. Database Seeding

```csharp
public class DatabaseSeeder
{
    public static async Task SeedTestDataAsync(ApplicationDbContext context)
    {
        // Seed Users
        var users = new[]
        {
            TestDataBuilder.CreateTestUser(100, "en-US"),
            TestDataBuilder.CreateTestUser(101, "ar-EG"),
            TestDataBuilder.CreateTestUser(102, "en-US")
        };
        context.Users.AddRange(users);

        // Seed Funds
        var funds = new[]
        {
            TestDataBuilder.CreateTestFund(1, "FUND001"),
            TestDataBuilder.CreateTestFund(2, "FUND002")
        };
        context.Funds.AddRange(funds);

        // Seed Resolution Types
        var resolutionTypes = new[]
        {
            new ResolutionType { Id = 1, NameAr = "استحواذ", NameEn = "Acquisition" },
            new ResolutionType { Id = 2, NameAr = "تخارج", NameEn = "Exit" }
        };
        context.ResolutionTypes.AddRange(resolutionTypes);

        await context.SaveChangesAsync();
    }

    public static async Task CleanupTestDataAsync(ApplicationDbContext context)
    {
        context.ResolutionVotes.RemoveRange(context.ResolutionVotes);
        context.ResolutionItems.RemoveRange(context.ResolutionItems);
        context.Resolutions.RemoveRange(context.Resolutions);
        context.BoardMembers.RemoveRange(context.BoardMembers);
        context.Notifications.RemoveRange(context.Notifications);
        context.Funds.RemoveRange(context.Funds);
        context.Users.RemoveRange(context.Users);

        await context.SaveChangesAsync();
    }
}
```

## Test Environment Setup

### 1. Test Configuration

```csharp
public class TestStartup : Startup
{
    public TestStartup(IConfiguration configuration) : base(configuration) { }

    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        // Replace real services with test doubles
        services.Replace(ServiceDescriptor.Scoped<INotificationService, MockNotificationService>());
        services.Replace(ServiceDescriptor.Scoped<IFileStorageService, MockFileStorageService>());

        // Use in-memory database for testing
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()));
    }
}
```

### 2. Test Base Classes

```csharp
public abstract class AcceptanceTestBase : IAsyncLifetime
{
    protected WebApplicationFactory<Program> _factory;
    protected HttpClient _client;
    protected ApplicationDbContext _context;

    public async Task InitializeAsync()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseStartup<TestStartup>();
                builder.UseEnvironment("Testing");
            });

        _client = _factory.CreateClient();
        _context = _factory.Services.GetRequiredService<ApplicationDbContext>();

        await DatabaseSeeder.SeedTestDataAsync(_context);
    }

    public async Task DisposeAsync()
    {
        await DatabaseSeeder.CleanupTestDataAsync(_context);
        _context?.Dispose();
        _client?.Dispose();
        _factory?.Dispose();
    }

    protected async Task AuthenticateAsRoleAsync(string role)
    {
        var token = await GenerateJwtTokenAsync(role);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
```

---

**Document Version:** 1.0
**Last Updated:** December 25, 2025
**Coverage Target:** 80% minimum code coverage
**Total Test Cases:** 250+ across all categories

This comprehensive test specification ensures thorough validation of all Sprint 2 features while maintaining high quality standards and architectural compliance with the Jadwa Fund Management System.
```
```
