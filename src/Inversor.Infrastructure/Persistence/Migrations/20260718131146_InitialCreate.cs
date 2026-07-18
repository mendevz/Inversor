using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Inversor.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MacroTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Tag = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacroTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    SubscriptionTier = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GrammarTopics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MacroTagId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tag = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    TheoryDescription = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrammarTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrammarTopics_MacroTags_MacroTagId",
                        column: x => x.MacroTagId,
                        principalTable: "MacroTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLanguageProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    AssessedLevel = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    DailyRequestCount = table.Column<int>(type: "integer", nullable: false),
                    LastRequestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLanguageProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLanguageProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopicMasteries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserLanguageId = table.Column<Guid>(type: "uuid", nullable: false),
                    GrammarTopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    MasteryScore = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    TotalAttempts = table.Column<int>(type: "integer", nullable: false),
                    ConsecutiveSuccesses = table.Column<int>(type: "integer", nullable: false),
                    CurrentIntervalDays = table.Column<int>(type: "integer", nullable: false),
                    EasinessFactor = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    NextReviewDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicMasteries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopicMasteries_GrammarTopics_GrammarTopicId",
                        column: x => x.GrammarTopicId,
                        principalTable: "GrammarTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TopicMasteries_UserLanguageProfiles_UserLanguageId",
                        column: x => x.UserLanguageId,
                        principalTable: "UserLanguageProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TranslationSubmissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserLanguageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Mode = table.Column<string>(type: "text", nullable: false),
                    OriginalInput = table.Column<string>(type: "text", nullable: false),
                    CorrectedOutput = table.Column<string>(type: "text", nullable: false),
                    GeneralFeedback = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserLanguageProfileId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslationSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TranslationSubmissions_UserLanguageProfiles_UserLanguagePro~",
                        column: x => x.UserLanguageProfileId,
                        principalTable: "UserLanguageProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubmitTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TranslationSubmissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    GrammarTopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsError = table.Column<bool>(type: "boolean", nullable: false),
                    OriginalFragment = table.Column<string>(type: "text", nullable: false),
                    CorrectedFragment = table.Column<string>(type: "text", nullable: false),
                    BriefExplanation = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmitTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubmitTags_GrammarTopics_GrammarTopicId",
                        column: x => x.GrammarTopicId,
                        principalTable: "GrammarTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubmitTags_TranslationSubmissions_TranslationSubmissionId",
                        column: x => x.TranslationSubmissionId,
                        principalTable: "TranslationSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MacroTags",
                columns: new[] { "Id", "Description", "Tag" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-100000000000"), "Escritura correcta de las palabras.", "ORTHOGRAPHY" },
                    { new Guid("22222222-2222-2222-2222-200000000000"), "Forma y conjugación de las palabras.", "MORPHOLOGY" },
                    { new Guid("33333333-3333-3333-3333-300000000000"), "Orden y estructura de la oración.", "SYNTAX" },
                    { new Guid("44444444-4444-4444-4444-400000000000"), "Vocabulario y significado literal.", "LEXICON" },
                    { new Guid("55555555-5555-5555-5555-500000000000"), "Sentido lógico de la oración.", "SEMANTICS" },
                    { new Guid("66666666-6666-6666-6666-600000000000"), "Uso adecuado según el contexto.", "PRAGMATICS" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrammarTopics_MacroTagId",
                table: "GrammarTopics",
                column: "MacroTagId");

            migrationBuilder.CreateIndex(
                name: "IX_GrammarTopics_Tag",
                table: "GrammarTopics",
                column: "Tag",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MacroTags_Tag",
                table: "MacroTags",
                column: "Tag",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubmitTags_GrammarTopicId",
                table: "SubmitTags",
                column: "GrammarTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmitTags_TranslationSubmissionId",
                table: "SubmitTags",
                column: "TranslationSubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicMasteries_GrammarTopicId",
                table: "TopicMasteries",
                column: "GrammarTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicMasteries_UserLanguageId",
                table: "TopicMasteries",
                column: "UserLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_TranslationSubmissions_UserLanguageProfileId",
                table: "TranslationSubmissions",
                column: "UserLanguageProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguageProfiles_UserId",
                table: "UserLanguageProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubmitTags");

            migrationBuilder.DropTable(
                name: "TopicMasteries");

            migrationBuilder.DropTable(
                name: "TranslationSubmissions");

            migrationBuilder.DropTable(
                name: "GrammarTopics");

            migrationBuilder.DropTable(
                name: "UserLanguageProfiles");

            migrationBuilder.DropTable(
                name: "MacroTags");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
