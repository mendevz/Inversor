using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inversor.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLinkBidirectionalTrace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TraceId",
                table: "TranslationSubmissions",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TraceId",
                table: "TranslationSubmissions");
        }
    }
}
