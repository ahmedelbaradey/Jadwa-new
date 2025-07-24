using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAssessmentEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FundId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key reference to Fund entity"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "Title of the assessment"),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "Type of assessment (Questionnaire=1, Attachment=2)"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1, comment: "Current status of the assessment (Draft=1, WaitingForApproval=2, Approved=3, Rejected=4, Active=5, Completed=6)"),
                    AttachmentId = table.Column<int>(type: "int", nullable: true, comment: "Foreign key reference to Attachment entity (required for Attachment type assessments)"),
                    ReviewerComments = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Comments from reviewer when assessment is rejected"),
                    ReviewedBy = table.Column<int>(type: "int", nullable: true, comment: "User ID of the reviewer who approved/rejected the assessment"),
                    ReviewedDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Date when the assessment was reviewed"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assessments_Funds_FundId",
                        column: x => x.FundId,
                        principalTable: "Funds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assessments_AspNetUsers_ReviewedBy",
                        column: x => x.ReviewedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assessments_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assessments_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assessments_AspNetUsers_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assessments_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key reference to Assessment entity"),
                    QuestionText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, comment: "Text content of the question"),
                    QuestionType = table.Column<int>(type: "int", nullable: false, comment: "Type of question (SingleChoice=1, Text=2)"),
                    Options = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "JSON string containing options for single choice questions"),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Display order of the question within the assessment"),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false, defaultValue: true, comment: "Indicates if this question is required to be answered"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentQuestions_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssessmentQuestions_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssessmentQuestions_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssessmentQuestions_AspNetUsers_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key reference to Assessment entity"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key reference to User (Board Member) entity"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1, comment: "Status of the response (Pending=1, Completed=2)"),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Date when the response was submitted"),
                    GeneralComments = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true, comment: "General comments provided by the board member"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentResponses_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssessmentResponses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssessmentResponses_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssessmentResponses_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssessmentResponses_AspNetUsers_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResponseId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key reference to AssessmentResponse entity"),
                    QuestionId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key reference to AssessmentQuestion entity"),
                    AnswerValue = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true, comment: "The actual answer value provided by the board member"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_AssessmentResponses_ResponseId",
                        column: x => x.ResponseId,
                        principalTable: "AssessmentResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answers_AssessmentQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "AssessmentQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Answers_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Answers_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Answers_AspNetUsers_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // Create indexes for performance
            migrationBuilder.CreateIndex(
                name: "IX_Assessments_FundId",
                table: "Assessments",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_Status",
                table: "Assessments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_CreatedBy",
                table: "Assessments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_ReviewedBy",
                table: "Assessments",
                column: "ReviewedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_AttachmentId",
                table: "Assessments",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentQuestions_AssessmentId",
                table: "AssessmentQuestions",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentQuestions_DisplayOrder",
                table: "AssessmentQuestions",
                columns: new[] { "AssessmentId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentResponses_AssessmentId",
                table: "AssessmentResponses",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentResponses_UserId",
                table: "AssessmentResponses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentResponses_AssessmentId_UserId",
                table: "AssessmentResponses",
                columns: new[] { "AssessmentId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_ResponseId",
                table: "Answers",
                column: "ResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_ResponseId_QuestionId",
                table: "Answers",
                columns: new[] { "ResponseId", "QuestionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "AssessmentResponses");

            migrationBuilder.DropTable(
                name: "AssessmentQuestions");

            migrationBuilder.DropTable(
                name: "Assessments");
        }
    }
}
