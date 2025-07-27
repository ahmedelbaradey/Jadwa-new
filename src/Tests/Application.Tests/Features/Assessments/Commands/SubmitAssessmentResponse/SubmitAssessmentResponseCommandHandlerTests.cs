using Application.Features.Assessments.Commands.SubmitAssessmentResponse;
using Abstraction.Contracts.Repository;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;
using Domain.Entities.AssessmentManagement;
using Moq;
using Xunit;
using FluentAssertions;
using Abstraction.Contract.Repository.AssessmentManagement;

namespace Application.Tests.Features.Assessments.Commands.SubmitAssessmentResponse
{
    /// <summary>
    /// Unit tests for SubmitAssessmentResponseCommandHandler
    /// Tests the business logic for board members responding to assessments
    /// Based on User Story 4: Respond to Assessment from AssessmentStories.md
    /// </summary>
    public class SubmitAssessmentResponseCommandHandlerTests
    {
        private readonly Mock<IRepositoryManager> _mockRepository;
        private readonly Mock<IStringLocalizer<SharedResources>> _mockLocalizer;
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;
        private readonly Mock<IAssessmentRepository> _mockAssessmentRepository;
        private readonly Mock<IAssessmentResponseRepository> _mockAssessmentResponseRepository;
        private readonly Mock<IAnswerRepository> _mockAnswerRepository;
        private readonly SubmitAssessmentResponseCommandHandler _handler;

        public SubmitAssessmentResponseCommandHandlerTests()
        {
            _mockRepository = new Mock<IRepositoryManager>();
            _mockLocalizer = new Mock<IStringLocalizer<SharedResources>>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockAssessmentRepository = new Mock<IAssessmentRepository>();
            _mockAssessmentResponseRepository = new Mock<IAssessmentResponseRepository>();
            _mockAnswerRepository = new Mock<IAnswerRepository>();

            // Setup repository manager to return mocked repositories
            _mockRepository.Setup(r => r.Assessments).Returns(_mockAssessmentRepository.Object);
            _mockRepository.Setup(r => r.AssessmentResponses).Returns(_mockAssessmentResponseRepository.Object);
            _mockRepository.Setup(r => r.Answers).Returns(_mockAnswerRepository.Object);

            // Setup current user service
            _mockCurrentUserService.Setup(u => u.GetUserId()).Returns(1);

            // Setup localizer with default returns
            _mockLocalizer.Setup(l => l[It.IsAny<string>()])
                .Returns(new LocalizedString("key", "localized value"));
            _mockLocalizer.Setup(l => l[It.IsAny<string>(), It.IsAny<object[]>()])
                .Returns(new LocalizedString("key", "localized value"));

            _handler = new SubmitAssessmentResponseCommandHandler(
                _mockRepository.Object,
                _mockLocalizer.Object,
                _mockCurrentUserService.Object);
        }

        [Fact]
        public async Task Handle_AssessmentNotFound_ReturnsNotFound()
        {
            // Arrange
            var command = new SubmitAssessmentResponseCommand { AssessmentId = 999 };
            _mockAssessmentRepository.Setup(r => r.GetAssessmentWithQuestionsAsync(999, false))
                .ReturnsAsync((Assessment?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_AssessmentNotActive_ReturnsBadRequest()
        {
            // Arrange
            var command = new SubmitAssessmentResponseCommand { AssessmentId = 1 };
            var assessment = new Assessment
            {
                Id = 1,
                Title = "Test Assessment",
                Status = AssessmentStatus.Draft,
                Type = AssessmentType.Questionnaire,
                Questions = new List<AssessmentQuestion>()
            };

            _mockAssessmentRepository.Setup(r => r.GetAssessmentWithQuestionsAsync(1, false))
                .ReturnsAsync(assessment);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Handle_QuestionnaireWithValidAnswers_ReturnsSuccess()
        {
            // Arrange
            var questions = new List<AssessmentQuestion>
            {
                new AssessmentQuestion
                {
                    Id = 1,
                    QuestionText = "Test Question",
                    QuestionType = QuestionType.Text,
                    IsRequired = true
                }
            };

            var command = new SubmitAssessmentResponseCommand
            {
                AssessmentId = 1,
                Answers = new List<SubmitAnswerDto>
                {
                    new SubmitAnswerDto { QuestionId = 1, AnswerText = "Test Answer" }
                }
            };

            var assessment = new Assessment
            {
                Id = 1,
                Title = "Test Assessment",
                Status = AssessmentStatus.Active,
                Type = AssessmentType.Questionnaire,
                Questions = questions
            };

            _mockAssessmentRepository.Setup(r => r.GetAssessmentWithQuestionsAsync(1, false))
                .ReturnsAsync(assessment);
            _mockAssessmentResponseRepository.Setup(r => r.GetResponseByAssessmentAndUserAsync(1, 1, false))
                .ReturnsAsync((AssessmentResponse?)null);
            _mockAssessmentResponseRepository.Setup(r => r.AddAsync(It.IsAny<AssessmentResponse>()))
                .Returns(Task.CompletedTask);
            _mockAnswerRepository.Setup(r => r.AddAsync(It.IsAny<Answer>()))
                .Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.SaveAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.AssessmentId.Should().Be(1);
            result.Data.AnswerCount.Should().Be(1);
            result.Data.Status.Should().Be(ResponseStatus.Completed);
        }

        [Fact]
        public async Task Handle_AttachmentWithComments_ReturnsSuccess()
        {
            // Arrange
            var command = new SubmitAssessmentResponseCommand
            {
                AssessmentId = 1,
                Comments = "Test comments for attachment"
            };

            var assessment = new Assessment
            {
                Id = 1,
                Title = "Test Assessment",
                Status = AssessmentStatus.Active,
                Type = AssessmentType.Attachment,
                Questions = new List<AssessmentQuestion>()
            };

            _mockAssessmentRepository.Setup(r => r.GetAssessmentWithQuestionsAsync(1, false))
                .ReturnsAsync(assessment);
            _mockAssessmentResponseRepository.Setup(r => r.GetResponseByAssessmentAndUserAsync(1, 1, false))
                .ReturnsAsync((AssessmentResponse?)null);
            _mockAssessmentResponseRepository.Setup(r => r.AddAsync(It.IsAny<AssessmentResponse>()))
                .Returns(Task.CompletedTask);
            _mockAnswerRepository.Setup(r => r.AddAsync(It.IsAny<Answer>()))
                .Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.SaveAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.AssessmentId.Should().Be(1);
            result.Data.Status.Should().Be(ResponseStatus.Completed);
        }

        [Fact]
        public async Task Handle_MissingRequiredAnswer_ReturnsBadRequest()
        {
            // Arrange
            var questions = new List<AssessmentQuestion>
            {
                new AssessmentQuestion
                {
                    Id = 1,
                    QuestionText = "Required Question",
                    QuestionType = QuestionType.Text,
                    IsRequired = true
                },
                new AssessmentQuestion
                {
                    Id = 2,
                    QuestionText = "Another Required Question",
                    QuestionType = QuestionType.Text,
                    IsRequired = true
                }
            };

            var command = new SubmitAssessmentResponseCommand
            {
                AssessmentId = 1,
                Answers = new List<SubmitAnswerDto>
                {
                    new SubmitAnswerDto { QuestionId = 1, AnswerText = "Test Answer" }
                    // Missing answer for question 2
                }
            };

            var assessment = new Assessment
            {
                Id = 1,
                Title = "Test Assessment",
                Status = AssessmentStatus.Active,
                Type = AssessmentType.Questionnaire,
                Questions = questions
            };

            _mockAssessmentRepository.Setup(r => r.GetAssessmentWithQuestionsAsync(1, false))
                .ReturnsAsync(assessment);
            _mockAssessmentResponseRepository.Setup(r => r.GetResponseByAssessmentAndUserAsync(1, 1, false))
                .ReturnsAsync((AssessmentResponse?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Handle_UpdateExistingResponse_ReturnsSuccess()
        {
            // Arrange
            var questions = new List<AssessmentQuestion>
            {
                new AssessmentQuestion
                {
                    Id = 1,
                    QuestionText = "Test Question",
                    QuestionType = QuestionType.Text,
                    IsRequired = true
                }
            };

            var command = new SubmitAssessmentResponseCommand
            {
                AssessmentId = 1,
                Answers = new List<SubmitAnswerDto>
                {
                    new SubmitAnswerDto { QuestionId = 1, AnswerText = "Updated Answer" }
                }
            };

            var assessment = new Assessment
            {
                Id = 1,
                Title = "Test Assessment",
                Status = AssessmentStatus.Active,
                Type = AssessmentType.Questionnaire,
                Questions = questions
            };

            var existingResponse = new AssessmentResponse
            {
                Id = 1,
                AssessmentId = 1,
                UserId = 1,
                Status = ResponseStatus.Pending
            };

            _mockAssessmentRepository.Setup(r => r.GetAssessmentWithQuestionsAsync(1, false))
                .ReturnsAsync(assessment);
            _mockAssessmentResponseRepository.Setup(r => r.GetResponseByAssessmentAndUserAsync(1, 1, false))
                .ReturnsAsync(existingResponse);
            _mockAnswerRepository.Setup(r => r.GetAnswersByResponseIdAsync(1, false))
                .ReturnsAsync(new List<Answer>());
            _mockAnswerRepository.Setup(r => r.AddAsync(It.IsAny<Answer>()))
                .Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.SaveAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.IsUpdate.Should().BeTrue();
            existingResponse.Status.Should().Be(ResponseStatus.Completed);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_ReturnsServerError()
        {
            // Arrange
            var command = new SubmitAssessmentResponseCommand { AssessmentId = 1 };
            _mockAssessmentRepository.Setup(r => r.GetAssessmentWithQuestionsAsync(1, false))
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
