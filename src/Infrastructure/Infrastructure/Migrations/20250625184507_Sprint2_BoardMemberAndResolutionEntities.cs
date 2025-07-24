using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Sprint2_BoardMemberAndResolutionEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoardMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FundId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key reference to Fund entity"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key reference to User entity"),
                    MemberType = table.Column<int>(type: "int", nullable: false, comment: "Type of board member (Independent=1, NotIndependent=2)"),
                    IsChairman = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indicates if this board member is the chairman"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true, comment: "Status of the board member (Active/Inactive)"),
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
                    table.PrimaryKey("PK_BoardMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardMembers_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoardMembers_AspNetUsers_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoardMembers_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoardMembers_Funds",
                        column: x => x.FundId,
                        principalTable: "Funds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoardMembers_Users",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResolutionStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResolutionStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResolutionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsOther = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResolutionTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResolutionTypes_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionTypes_AspNetUsers_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionTypes_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Resolutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Auto-generated resolution code (fund code/resolution year/resolution no.)"),
                    ResolutionDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date of the resolution"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Description of the resolution (optional, max 500 characters)"),
                    ResolutionTypeId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key reference to ResolutionType entity"),
                    NewType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Custom resolution type name when 'Other' is selected"),
                    AttachmentId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key reference to main Attachment entity"),
                    VotingMethodology = table.Column<int>(type: "int", nullable: false, comment: "Voting methodology (AllMembers=1, Majority=2)"),
                    MemberVotingResult = table.Column<int>(type: "int", nullable: false, defaultValue: 2, comment: "How member voting results are calculated"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1, comment: "Current status of the resolution"),
                    FundId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key reference to Fund entity"),
                    ParentResolutionId = table.Column<int>(type: "int", nullable: true, comment: "Parent resolution identifier for tracking relationships"),
                    OldResolutionCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Old resolution code when this is an update to existing resolution"),
                    FundId1 = table.Column<int>(type: "int", nullable: true),
                    ResolutionTypeId1 = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_Resolutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resolutions_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resolutions_AspNetUsers_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resolutions_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resolutions_Attachments",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resolutions_Funds",
                        column: x => x.FundId,
                        principalTable: "Funds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resolutions_Funds_FundId1",
                        column: x => x.FundId1,
                        principalTable: "Funds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Resolutions_ParentResolution",
                        column: x => x.ParentResolutionId,
                        principalTable: "Resolutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resolutions_ResolutionTypes",
                        column: x => x.ResolutionTypeId,
                        principalTable: "ResolutionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resolutions_ResolutionTypes_ResolutionTypeId1",
                        column: x => x.ResolutionTypeId1,
                        principalTable: "ResolutionTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ResolutionAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResolutionId = table.Column<int>(type: "int", nullable: false),
                    AttachmentId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResolutionAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResolutionAttachments_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionAttachments_AspNetUsers_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionAttachments_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionAttachments_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionAttachments_Resolutions_ResolutionId",
                        column: x => x.ResolutionId,
                        principalTable: "Resolutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResolutionItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResolutionId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key reference to Resolution entity"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Auto-generated title (Item1, Item2, etc.)"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Description of the resolution item (max 500 characters)"),
                    HasConflict = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indicates if there is a conflict of interest with board members"),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, comment: "Display order for sorting resolution items"),
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
                    table.PrimaryKey("PK_ResolutionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResolutionItems_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionItems_AspNetUsers_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionItems_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionItems_Resolutions",
                        column: x => x.ResolutionId,
                        principalTable: "Resolutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResolutionStatusHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResolutionId = table.Column<int>(type: "int", nullable: false),
                    ResolutionStatusId = table.Column<int>(type: "int", nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreviousStatus = table.Column<int>(type: "int", nullable: true),
                    NewStatus = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResolutionStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResolutionStatusHistories_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionStatusHistories_AspNetUsers_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionStatusHistories_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionStatusHistories_ResolutionStatus_ResolutionStatusId",
                        column: x => x.ResolutionStatusId,
                        principalTable: "ResolutionStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionStatusHistories_Resolutions_ResolutionId",
                        column: x => x.ResolutionId,
                        principalTable: "Resolutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResolutionItemConflicts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResolutionItemId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key reference to ResolutionItem entity"),
                    BoardMemberId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key reference to BoardMember entity"),
                    ConflictNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Optional notes about the nature of the conflict"),
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
                    table.PrimaryKey("PK_ResolutionItemConflicts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResolutionItemConflicts_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionItemConflicts_AspNetUsers_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionItemConflicts_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionItemConflicts_BoardMembers",
                        column: x => x.BoardMemberId,
                        principalTable: "BoardMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionItemConflicts_ResolutionItems",
                        column: x => x.ResolutionItemId,
                        principalTable: "ResolutionItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResolutionVotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResolutionId = table.Column<int>(type: "int", nullable: false),
                    ResolutionItemId = table.Column<int>(type: "int", nullable: true),
                    BoardMemberId = table.Column<int>(type: "int", nullable: false),
                    VoteValue = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResolutionVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResolutionVotes_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionVotes_AspNetUsers_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionVotes_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionVotes_BoardMembers_BoardMemberId",
                        column: x => x.BoardMemberId,
                        principalTable: "BoardMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionVotes_ResolutionItems_ResolutionItemId",
                        column: x => x.ResolutionItemId,
                        principalTable: "ResolutionItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionVotes_Resolutions_ResolutionId",
                        column: x => x.ResolutionId,
                        principalTable: "Resolutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardMembers_CreatedBy",
                table: "BoardMembers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BoardMembers_DeletedBy",
                table: "BoardMembers",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BoardMembers_Fund_Chairman",
                table: "BoardMembers",
                columns: new[] { "FundId", "IsChairman" },
                filter: "[IsChairman] = 1 AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_BoardMembers_Fund_Type_Active",
                table: "BoardMembers",
                columns: new[] { "FundId", "MemberType", "IsActive" },
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_BoardMembers_Fund_User_Unique",
                table: "BoardMembers",
                columns: new[] { "FundId", "UserId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_BoardMembers_FundId",
                table: "BoardMembers",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_BoardMembers_UpdatedBy",
                table: "BoardMembers",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BoardMembers_UserId",
                table: "BoardMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionAttachments_AttachmentId",
                table: "ResolutionAttachments",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionAttachments_CreatedBy",
                table: "ResolutionAttachments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionAttachments_DeletedBy",
                table: "ResolutionAttachments",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionAttachments_ResolutionId",
                table: "ResolutionAttachments",
                column: "ResolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionAttachments_UpdatedBy",
                table: "ResolutionAttachments",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionItemConflicts_BoardMemberId",
                table: "ResolutionItemConflicts",
                column: "BoardMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionItemConflicts_CreatedBy",
                table: "ResolutionItemConflicts",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionItemConflicts_DeletedBy",
                table: "ResolutionItemConflicts",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionItemConflicts_Item_Member_Unique",
                table: "ResolutionItemConflicts",
                columns: new[] { "ResolutionItemId", "BoardMemberId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionItemConflicts_ResolutionItemId",
                table: "ResolutionItemConflicts",
                column: "ResolutionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionItemConflicts_UpdatedBy",
                table: "ResolutionItemConflicts",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionItems_CreatedBy",
                table: "ResolutionItems",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionItems_DeletedBy",
                table: "ResolutionItems",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionItems_Resolution_Conflict",
                table: "ResolutionItems",
                columns: new[] { "ResolutionId", "HasConflict" });

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionItems_Resolution_Order",
                table: "ResolutionItems",
                columns: new[] { "ResolutionId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionItems_ResolutionId",
                table: "ResolutionItems",
                column: "ResolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionItems_UpdatedBy",
                table: "ResolutionItems",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_AttachmentId",
                table: "Resolutions",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_Code_Unique",
                table: "Resolutions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_CreatedBy",
                table: "Resolutions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_Date",
                table: "Resolutions",
                column: "ResolutionDate");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_DeletedBy",
                table: "Resolutions",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_Fund_Status",
                table: "Resolutions",
                columns: new[] { "FundId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_FundId",
                table: "Resolutions",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_FundId1",
                table: "Resolutions",
                column: "FundId1");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_ParentId",
                table: "Resolutions",
                column: "ParentResolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_ResolutionTypeId",
                table: "Resolutions",
                column: "ResolutionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_ResolutionTypeId1",
                table: "Resolutions",
                column: "ResolutionTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_Status",
                table: "Resolutions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_UpdatedBy",
                table: "Resolutions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionStatusHistories_CreatedBy",
                table: "ResolutionStatusHistories",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionStatusHistories_DeletedBy",
                table: "ResolutionStatusHistories",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionStatusHistories_ResolutionId",
                table: "ResolutionStatusHistories",
                column: "ResolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionStatusHistories_ResolutionStatusId",
                table: "ResolutionStatusHistories",
                column: "ResolutionStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionStatusHistories_UpdatedBy",
                table: "ResolutionStatusHistories",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionTypes_CreatedBy",
                table: "ResolutionTypes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionTypes_DeletedBy",
                table: "ResolutionTypes",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionTypes_UpdatedBy",
                table: "ResolutionTypes",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionVotes_BoardMemberId",
                table: "ResolutionVotes",
                column: "BoardMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionVotes_CreatedBy",
                table: "ResolutionVotes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionVotes_DeletedBy",
                table: "ResolutionVotes",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionVotes_ResolutionId",
                table: "ResolutionVotes",
                column: "ResolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionVotes_ResolutionItemId",
                table: "ResolutionVotes",
                column: "ResolutionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionVotes_UpdatedBy",
                table: "ResolutionVotes",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResolutionAttachments");

            migrationBuilder.DropTable(
                name: "ResolutionItemConflicts");

            migrationBuilder.DropTable(
                name: "ResolutionStatusHistories");

            migrationBuilder.DropTable(
                name: "ResolutionVotes");

            migrationBuilder.DropTable(
                name: "ResolutionStatus");

            migrationBuilder.DropTable(
                name: "BoardMembers");

            migrationBuilder.DropTable(
                name: "ResolutionItems");

            migrationBuilder.DropTable(
                name: "Resolutions");

            migrationBuilder.DropTable(
                name: "ResolutionTypes");
        }
    }
}
