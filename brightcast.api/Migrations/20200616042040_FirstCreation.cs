﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace brightcast.Migrations
{
    public partial class FirstCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Businesses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Membership = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResetPasswords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResetCode = table.Column<Guid>(nullable: false),
                    Activated = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetPasswords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResetPasswords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserActivations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivationCode = table.Column<Guid>(nullable: false),
                    Activated = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserActivations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    PictureUrl = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Default = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    BusinessId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<int>(nullable: false),
                    UserProfileId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campaigns_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactLists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<int>(nullable: false),
                    UserProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactLists_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Scope = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<int>(nullable: false),
                    UserProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignContactLists",
                columns: table => new
                {
                    CampaignId = table.Column<int>(nullable: false),
                    ContactListId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignContactLists", x => new { x.CampaignId, x.ContactListId });
                    table.ForeignKey(
                        name: "FK_CampaignContactLists_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignContactLists_ContactLists_ContactListId",
                        column: x => x.ContactListId,
                        principalTable: "ContactLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignSents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<int>(nullable: false),
                    ContactListId = table.Column<int>(nullable: false),
                    CampaignId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignSents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignSents_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignSents_ContactLists_ContactListId",
                        column: x => x.ContactListId,
                        principalTable: "ContactLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Subscribed = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<int>(nullable: false),
                    ContactListId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_ContactLists_ContactListId",
                        column: x => x.ContactListId,
                        principalTable: "ContactLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignSentStatses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Delivered = table.Column<int>(nullable: false),
                    Read = table.Column<int>(nullable: false),
                    NewSubscriber = table.Column<int>(nullable: false),
                    Unsubscribed = table.Column<int>(nullable: false),
                    Replies = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<int>(nullable: false),
                    CampaignSentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignSentStatses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignSentStatses_CampaignSents_CampaignSentId",
                        column: x => x.CampaignSentId,
                        principalTable: "CampaignSents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignMessages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageSid = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    Date_Created = table.Column<DateTime>(nullable: false),
                    Date_Sent = table.Column<DateTime>(nullable: false),
                    Date_Updated = table.Column<DateTime>(nullable: false),
                    Error_Code = table.Column<string>(nullable: true),
                    Error_Message = table.Column<string>(nullable: true),
                    From = table.Column<string>(nullable: true),
                    To = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<int>(nullable: false),
                    ContactId = table.Column<int>(nullable: false),
                    CampaignId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignMessages_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignMessages_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceiveMessages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageSid = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    Date_Created = table.Column<DateTime>(nullable: false),
                    Date_Sent = table.Column<DateTime>(nullable: false),
                    Date_Updated = table.Column<DateTime>(nullable: false),
                    Error_Code = table.Column<string>(nullable: true),
                    Error_Message = table.Column<string>(nullable: true),
                    From = table.Column<string>(nullable: true),
                    To = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<int>(nullable: false),
                    ContactId = table.Column<int>(nullable: false),
                    CampaignId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiveMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiveMessages_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiveMessages_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateMessages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageSid = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    Date_Created = table.Column<DateTime>(nullable: false),
                    Date_Sent = table.Column<DateTime>(nullable: false),
                    Date_Updated = table.Column<DateTime>(nullable: false),
                    Error_Code = table.Column<string>(nullable: true),
                    Error_Message = table.Column<string>(nullable: true),
                    From = table.Column<string>(nullable: true),
                    To = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<int>(nullable: false),
                    ContactId = table.Column<int>(nullable: false),
                    CampaignId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateMessages_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateMessages_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignContactLists_ContactListId",
                table: "CampaignContactLists",
                column: "ContactListId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignMessages_CampaignId",
                table: "CampaignMessages",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignMessages_ContactId",
                table: "CampaignMessages",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_UserProfileId",
                table: "Campaigns",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignSents_CampaignId",
                table: "CampaignSents",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignSents_ContactListId",
                table: "CampaignSents",
                column: "ContactListId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignSentStatses_CampaignSentId",
                table: "CampaignSentStatses",
                column: "CampaignSentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactLists_UserProfileId",
                table: "ContactLists",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_ContactListId",
                table: "Contacts",
                column: "ContactListId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveMessages_CampaignId",
                table: "ReceiveMessages",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveMessages_ContactId",
                table: "ReceiveMessages",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ResetPasswords_UserId",
                table: "ResetPasswords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserProfileId",
                table: "Roles",
                column: "UserProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TemplateMessages_CampaignId",
                table: "TemplateMessages",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateMessages_ContactId",
                table: "TemplateMessages",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivations_UserId",
                table: "UserActivations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_BusinessId",
                table: "UserProfiles",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignContactLists");

            migrationBuilder.DropTable(
                name: "CampaignMessages");

            migrationBuilder.DropTable(
                name: "CampaignSentStatses");

            migrationBuilder.DropTable(
                name: "ReceiveMessages");

            migrationBuilder.DropTable(
                name: "ResetPasswords");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "TemplateMessages");

            migrationBuilder.DropTable(
                name: "UserActivations");

            migrationBuilder.DropTable(
                name: "CampaignSents");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "ContactLists");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Businesses");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
