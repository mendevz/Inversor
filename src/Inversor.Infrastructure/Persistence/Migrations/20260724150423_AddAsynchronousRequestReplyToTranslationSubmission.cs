using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inversor.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAsynchronousRequestReplyToTranslationSubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Mode",
                table: "TranslationSubmissions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "GeneralFeedback",
                table: "TranslationSubmissions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CorrectedOutput",
                table: "TranslationSubmissions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "FailureReason",
                table: "TranslationSubmissions",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessedAt",
                table: "TranslationSubmissions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "TranslationSubmissions",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailureReason",
                table: "TranslationSubmissions");

            migrationBuilder.DropColumn(
                name: "ProcessedAt",
                table: "TranslationSubmissions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TranslationSubmissions");

            migrationBuilder.AlterColumn<string>(
                name: "Mode",
                table: "TranslationSubmissions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "GeneralFeedback",
                table: "TranslationSubmissions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CorrectedOutput",
                table: "TranslationSubmissions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
