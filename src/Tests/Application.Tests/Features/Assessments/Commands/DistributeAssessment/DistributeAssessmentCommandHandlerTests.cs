using Application.Features.Assessments.Commands.DistributeAssessment;
using Abstraction.Contracts.Repository;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;
using Domain.Entities.AssessmentManagement;
using Moq;
using Xunit;
using FluentAssertions;
using Abstraction.Contract.Repository.AssessmentManagement;

namespace Application.Tests.Features.Assessments.Commands.DistributeAssessment
{
    /// <summary>
    /// Unit tests for DistributeAssessmentCommandHandler
    /// Tests the business logic for distributing approved assessments to board members
    /// Based on User Story 3: Distribute Assessment from AssessmentStories.md
    /// </summary>
    public class DistributeAssessmentCommandHandlerTests
    {
        private readonly Mock<IRepositoryManager> _mockRepository;
        private readonly Mock<IStringLocalizer<SharedResources>> _mockLocalizer;
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;
        private readonly Mock<IAssessmentRepository> _mockAssessmentRepository;
        private readonly Mock<IAssessmentResponseRepository> _mockAssessmentResponseRepository;
        private readonly DistributeAssessmentCommandHandler _handler;

        public DistributeAssessmentCommandHandlerTests()
        {
            _mockRepository = new Mock<IRepositoryManager>();
            _mockLocalizer = new Mock<IStringLocalizer<SharedResources>>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockAssessmentRepository = new Mock<IAssessmentRepository>();
            _mockAssessmentResponseRepository = new Mock<IAssessmentResponseRepository>();

            // Setup repository manager to return mocked repositories
            _mockRepository.Setup(r => r.Assessments).Returns(_mockAssessmentRepository.Object);
            _mockRepository.Setup(r => r.AssessmentResponses).Returns(_mockAssessmentResponseRepository.Object);

            // Setup current user service
            _mockCurrentUserService.Setup(u => u.GetUserId()).Returns(1);

            // Setup localizer with default returns
            _mockLocalizer.Setup(l => l[It.IsAny<string>()])
                .Returns(new LocalizedString("key", "localized value"));
            _mockLocalizer.Setup(l => l[It.IsAny<string>(), It.IsAny<object[]>()])
                .Returns(new LocalizedString("key", "localized value"));

            _handler = new DistributeAssessmentCommandHandler(
                _mockRepository.Object,
                _mockLocalizer.Object,
                _mockCurrentUserService.Object);
        }

        [Fact]
        public async Task Handle_AssessmentNotFound_ReturnsNotFound()
        {
            // Arrange
            var command = new DistributeAssessmentCommand { AssessmentId = 999 };
            _mockAssessmentRepository.Setup(r => r.GetByIdAsync(999))
                .ReturnsAsync((Assessment?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_AssessmentNotApproved_ReturnsBadRequest()
        {
            // Arrange
            var command = new DistributeAssessmentCommand { AssessmentId = 1 };
            var assessment = new Assessment
            {
                Id = 1,
                Title = "Test Assessment",
                Status = AssessmentStatus.Draft,
                FundId = 1
            };

            _mockAssessmentRepository.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(assessment);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Handle_NoBoardMembers_ReturnsBadRequest()
        {
            // Arrange
            var command = new DistributeAssessmentCommand { AssessmentId = 1 };
            var assessment = new Assessment
            {
                Id = 1,
                Title = "Test Assessment",
                Status = AssessmentStatus.Approved,
                FundId = 1
            };

            _mockAssessmentRepository.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(assessment);
            _mockAssessmentResponseRepository.Setup(r => r.CreateResponsesForBoardMembersAsync(1, 1))
                .ReturnsAsync(0);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccess()
        {
            // Arrange
            var command = new DistributeAssessmentCommand { AssessmentId = 1 };
            var assessment = new Assessment
            {
                Id = 1,
                Title = "Test Assessment",
                Status = AssessmentStatus.Approved,
                FundId = 1
            };

            _mockAssessmentRepository.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(assessment);
            _mockAssessmentResponseRepository.Setup(r => r.CreateResponsesForBoardMembersAsync(1, 1))
                .ReturnsAsync(3);
            _mockRepository.Setup(r => r.SaveAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.AssessmentId.Should().Be(1);
            result.Data.BoardMemberCount.Should().Be(3);
            assessment.Status.Should().Be(AssessmentStatus.Active);
            assessment.DistributionDate.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_ExceptionThrown_ReturnsServerError()
        {
            // Arrange
            var command = new DistributeAssessmentCommand { AssessmentId = 1 };
            _mockAssessmentRepository.Setup(r => r.GetByIdAsync(1))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
        }
    }
}
