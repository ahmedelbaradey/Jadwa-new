using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <summary>
    /// Migration to add Meeting Time Proposal entities
    /// Implements User Stories 1 and 2 from Meetings.md
    /// Creates tables for meeting time proposals, proposed dates, and votes
    /// </summary>
    public partial class AddMeetingTimeProposalEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create MeetingTimeProposals table
            migrationBuilder.CreateTable(
                name: "MeetingTimeProposals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FundId = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Under Voting"),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    AttachmentId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingTimeProposals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeetingTimeProposals_Funds_FundId",
                        column: x => x.FundId,
                        principalTable: "Funds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeetingTimeProposals_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MeetingTimeProposals_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            // Create ProposedDates table
            migrationBuilder.CreateTable(
                name: "ProposedDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProposalId = table.Column<int>(type: "int", nullable: false),
                    ProposedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposedDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProposedDates_MeetingTimeProposals_ProposalId",
                        column: x => x.ProposalId,
                        principalTable: "MeetingTimeProposals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create MeetingTimeVotes table
            migrationBuilder.CreateTable(
                name: "MeetingTimeVotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProposalId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProposedDateId = table.Column<int>(type: "int", nullable: false),
                    VoteTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingTimeVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeetingTimeVotes_MeetingTimeProposals_ProposalId",
                        column: x => x.ProposalId,
                        principalTable: "MeetingTimeProposals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeetingTimeVotes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MeetingTimeVotes_ProposedDates_ProposedDateId",
                        column: x => x.ProposedDateId,
                        principalTable: "ProposedDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // Create indexes for better performance
            migrationBuilder.CreateIndex(
                name: "IX_MeetingTimeProposals_FundId",
                table: "MeetingTimeProposals",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingTimeProposals_CreatedByUserId",
                table: "MeetingTimeProposals",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingTimeProposals_AttachmentId",
                table: "MeetingTimeProposals",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposedDates_ProposalId",
                table: "ProposedDates",
                column: "ProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingTimeVotes_ProposalId",
                table: "MeetingTimeVotes",
                column: "ProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingTimeVotes_UserId",
                table: "MeetingTimeVotes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingTimeVotes_ProposedDateId",
                table: "MeetingTimeVotes",
                column: "ProposedDateId");

            // Create unique constraint to prevent duplicate votes
            migrationBuilder.CreateIndex(
                name: "IX_MeetingTimeVotes_ProposalId_UserId_ProposedDateId",
                table: "MeetingTimeVotes",
                columns: new[] { "ProposalId", "UserId", "ProposedDateId" },
                unique: true);

            // Create MeetingStatusHistories table
            migrationBuilder.CreateTable(
                name: "MeetingStatusHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeetingId = table.Column<int>(type: "int", nullable: false),
                    MeetingStatusId = table.Column<int>(type: "int", nullable: false),
                    PreviousStatus = table.Column<int>(type: "int", nullable: true),
                    NewStatus = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UserRole = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ChangedBy = table.Column<int>(type: "int", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Comments = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ActionDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeetingStatusHistories_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeetingStatusHistories_Users_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // Create indexes for MeetingStatusHistories
            migrationBuilder.CreateIndex(
                name: "IX_MeetingStatusHistories_MeetingId",
                table: "MeetingStatusHistories",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingStatusHistories_ChangedBy",
                table: "MeetingStatusHistories",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingStatusHistories_ChangedAt",
                table: "MeetingStatusHistories",
                column: "ChangedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "MeetingStatusHistories");
            migrationBuilder.DropTable(name: "MeetingTimeVotes");
            migrationBuilder.DropTable(name: "ProposedDates");
            migrationBuilder.DropTable(name: "MeetingTimeProposals");
        }
    }
}
