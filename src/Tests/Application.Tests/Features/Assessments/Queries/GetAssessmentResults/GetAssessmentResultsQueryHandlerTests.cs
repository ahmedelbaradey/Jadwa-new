using Application.Features.Assessments.Queries.GetAssessmentResults;
using Abstraction.Contracts.Repository;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;
using Domain.Entities.AssessmentManagement;
using Domain.Entities.FundManagement;
using Domain.Entities.Users;
using Domain.Entities.Shared;
using Moq;
using Xunit;
using FluentAssertions;
using Abstraction.Contract.Repository.AssessmentManagement;

namespace Application.Tests.Features.Assessments.Queries.GetAssessmentResults
{
    /// <summary>
    /// Unit tests for GetAssessmentResultsQueryHandler
    /// Tests the business logic for viewing compiled assessment results
    /// Based on User Story 5: View Compiled Assessment Results from AssessmentStories.md
    /// </summary>
    public class GetAssessmentResultsQueryHandlerTests
    {
        private readonly Mock<IRepositoryManager> _mockRepository;
        private readonly Mock<IStringLocalizer<SharedResources>> _mockLocalizer;
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;
        private readonly Mock<IAssessmentRepository> _mockAssessmentRepository;
        private readonly Mock<IAssessmentResponseRepository> _mockAssessmentResponseRepository;
        private readonly Mock<IAnswerRepository> _mockAnswerRepository;
        private readonly GetAssessmentResultsQueryHandler _handler;

        public GetAssessmentResultsQueryHandlerTests()
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

            _handler = new GetAssessmentResultsQueryHandler(
                _mockRepository.Object,
                _mockLocalizer.Object,
                _mockCurrentUserService.Object);
        }

        [Fact]
        public async Task Handle_AssessmentNotFound_ReturnsNotFound()
        {
            // Arrange
            var query = new GetAssessmentResultsQuery { AssessmentId = 999 };
            _mockAssessmentRepository.Setup(r => r.GetAssessmentWithDetailsAsync(999, false))
                .ReturnsAsync((Assessment?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_AssessmentNotActiveOrCompleted_ReturnsBadRequest()
        {
            // Arrange
            var query = new GetAssessmentResultsQuery { AssessmentId = 1 };
            var assessment = new Assessment
            {
                Id = 1,
                Title = "Test Assessment",
                Status = AssessmentStatus.Draft,
                Type = AssessmentType.Questionnaire,
                Fund = new Fund { Name = "Test Fund" }
            };

            _mockAssessmentRepository.Setup(r => r.GetAssessmentWithDetailsAsync(1, false))
                .ReturnsAsync(assessment);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Handle_ActiveQuestionnaireAssessment_ReturnsResults()
        {
            // Arrange
            var query = new GetAssessmentResultsQuery { AssessmentId = 1 };
            
            var questions = new List<AssessmentQuestion>
            {
                new AssessmentQuestion
                {
                    Id = 1,
                    QuestionText = "Test Question",
                    QuestionType = QuestionType.Text,
                    DisplayOrder = 1,
                    IsRequired = true,
                    Options = null
                }
            };

            var users = new List<User>
            {
                new User { Id = 1, FullName = "John Doe" },
                new User { Id = 2, FullName = "Jane Smith" }
            };

            var responses = new List<AssessmentResponse>
            {
                new AssessmentResponse
                {
                    Id = 1,
                    AssessmentId = 1,
                    UserId = 1,
                    Status = ResponseStatus.Completed,
                    SubmissionDate = DateTime.UtcNow.AddDays(-1),
                    User = users[0]
                },
                new AssessmentResponse
                {
                    Id = 2,
                    AssessmentId = 1,
                    UserId = 2,
                    Status = ResponseStatus.Pending,
                    User = users[1]
                }
            };

            var answers = new List<Answer>
            {
                new Answer
                {
                    Id = 1,
                    ResponseId = 1,
                    QuestionId = 1,
                    AnswerText = "Test Answer"
                }
            };

            var assessment = new Assessment
            {
                Id = 1,
                Title = "Test Assessment",
                Status = AssessmentStatus.Active,
                Type = AssessmentType.Questionnaire,
                DistributionDate = DateTime.UtcNow.AddDays(-2),
                Fund = new Fund { Name = "Test Fund" },
                Questions = questions
            };

            _mockAssessmentRepository.Setup(r => r.GetAssessmentWithDetailsAsync(1, false))
                .ReturnsAsync(assessment);
            _mockAssessmentResponseRepository.Setup(r => r.GetResponsesByAssessmentIdAsync(1, false))
                .ReturnsAsync(responses);
            _mockAssessmentResponseRepository.Setup(r => r.GetCompletionStatisticsAsync(1))
                .ReturnsAsync((Total: 2, Completed: 1, Pending: 1));
            _mockAnswerRepository.Setup(r => r.GetAnswersByResponseIdAsync(1, false))
                .ReturnsAsync(answers);
            _mockAnswerRepository.Setup(r => r.GetAnswersByResponseIdAsync(2, false))
                .ReturnsAsync(new List<Answer>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Assessment.Id.Should().Be(1);
            result.Data.Assessment.Title.Should().Be("Test Assessment");
            result.Data.Statistics.TotalBoardMembers.Should().Be(2);
            result.Data.Statistics.CompletedResponses.Should().Be(1);
            result.Data.Statistics.PendingResponses.Should().Be(1);
            result.Data.Statistics.CompletionRate.Should().Be(50);
            result.Data.QuestionResults.Should().HaveCount(1);
            result.Data.QuestionResults[0].QuestionId.Should().Be(1);
            result.Data.QuestionResults[0].ResponseCount.Should().Be(1);
            result.Data.QuestionResults[0].TextAnswers.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handle_AttachmentAssessment_ReturnsTextResponses()
        {
            // Arrange
            var query = new GetAssessmentResultsQuery { AssessmentId = 1 };
            
            var users = new List<User>
            {
                new User { Id = 1, FullName = "John Doe" }
            };

            var responses = new List<AssessmentResponse>
            {
                new AssessmentResponse
                {
                    Id = 1,
                    AssessmentId = 1,
                    UserId = 1,
                    Status = ResponseStatus.Completed,
                    SubmissionDate = DateTime.UtcNow.AddDays(-1),
                    User = users[0]
                }
            };

            var answers = new List<Answer>
            {
                new Answer
                {
                    Id = 1,
                    ResponseId = 1,
                    QuestionId = null,
                    AnswerText = "This is my comment on the attachment"
                }
            };

            var assessment = new Assessment
            {
                Id = 1,
                Title = "Test Attachment Assessment",
                Status = AssessmentStatus.Active,
                Type = AssessmentType.Attachment,
                DistributionDate = DateTime.UtcNow.AddDays(-2),
                Fund = new Fund { Name = "Test Fund" },
                Attachment = new Attachment { FilePath = "/docs/test.pdf" },
                Questions = new List<AssessmentQuestion>()
            };

            _mockAssessmentRepository.Setup(r => r.GetAssessmentWithDetailsAsync(1, false))
                .ReturnsAsync(assessment);
            _mockAssessmentResponseRepository.Setup(r => r.GetResponsesByAssessmentIdAsync(1, false))
                .ReturnsAsync(responses);
            _mockAssessmentResponseRepository.Setup(r => r.GetCompletionStatisticsAsync(1))
                .ReturnsAsync((Total: 1, Completed: 1, Pending: 0));
            _mockAnswerRepository.Setup(r => r.GetAnswersByResponseIdAsync(1, false))
                .ReturnsAsync(answers);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Assessment.Type.Should().Be(AssessmentType.Attachment);
            result.Data.Assessment.AttachmentUrl.Should().Be("/docs/test.pdf");
            result.Data.TextResponses.Should().HaveCount(1);
            result.Data.TextResponses[0].Comments.Should().Be("This is my comment on the attachment");
            result.Data.TextResponses[0].RespondentName.Should().Be("John Doe");
            result.Data.QuestionResults.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_SingleChoiceQuestion_ReturnsChoiceResults()
        {
            // Arrange
            var query = new GetAssessmentResultsQuery { AssessmentId = 1 };
            
            var questions = new List<AssessmentQuestion>
            {
                new AssessmentQuestion
                {
                    Id = 1,
                    QuestionText = "Choose your preference",
                    QuestionType = QuestionType.SingleChoice,
                    DisplayOrder = 1,
                    IsRequired = true,
                    Options = "[\"Option A\", \"Option B\", \"Option C\"]"
                }
            };

            var users = new List<User>
            {
                new User { Id = 1, FullName = "John Doe" },
                new User { Id = 2, FullName = "Jane Smith" }
            };

            var responses = new List<AssessmentResponse>
            {
                new AssessmentResponse { Id = 1, AssessmentId = 1, UserId = 1, Status = ResponseStatus.Completed, User = users[0] },
                new AssessmentResponse { Id = 2, AssessmentId = 1, UserId = 2, Status = ResponseStatus.Completed, User = users[1] }
            };

            var answers = new List<Answer>
            {
                new Answer { Id = 1, ResponseId = 1, QuestionId = 1, AnswerText = "Option A" },
                new Answer { Id = 2, ResponseId = 2, QuestionId = 1, AnswerText = "Option A" }
            };

            var assessment = new Assessment
            {
                Id = 1,
                Title = "Test Assessment",
                Status = AssessmentStatus.Active,
                Type = AssessmentType.Questionnaire,
                Fund = new Fund { Name = "Test Fund" },
                Questions = questions
            };

            _mockAssessmentRepository.Setup(r => r.GetAssessmentWithDetailsAsync(1, false))
                .ReturnsAsync(assessment);
            _mockAssessmentResponseRepository.Setup(r => r.GetResponsesByAssessmentIdAsync(1, false))
                .ReturnsAsync(responses);
            _mockAssessmentResponseRepository.Setup(r => r.GetCompletionStatisticsAsync(1))
                .ReturnsAsync((Total: 2, Completed: 2, Pending: 0));
            _mockAnswerRepository.Setup(r => r.GetAnswersByResponseIdAsync(It.IsAny<int>, false))
                .ReturnsAsync((int responseId) => answers.Where(a => a.ResponseId == responseId).ToList());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.QuestionResults.Should().HaveCount(1);
            result.Data.QuestionResults[0].ChoiceResults.Should().HaveCount(3);
            result.Data.QuestionResults[0].ChoiceResults[0].OptionText.Should().Be("Option A");
            result.Data.QuestionResults[0].ChoiceResults[0].Count.Should().Be(2);
            result.Data.QuestionResults[0].ChoiceResults[0].Percentage.Should().Be(100);
            result.Data.QuestionResults[0].ChoiceResults[1].Count.Should().Be(0);
            result.Data.QuestionResults[0].ChoiceResults[2].Count.Should().Be(0);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_ReturnsServerError()
        {
            // Arrange
            var query = new GetAssessmentResultsQuery { AssessmentId = 1 };
            _mockAssessmentRepository.Setup(r => r.GetAssessmentWithDetailsAsync(1, false))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
        }
    }
}
