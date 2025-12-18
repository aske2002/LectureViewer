using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedDocumentPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "ExtractedText",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "NumberOfPages",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "TextContent",
                table: "Documents");

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentId",
                table: "MediaProcessingJobs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Documents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Documents",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DocumentPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    PageNumber = table.Column<int>(type: "integer", nullable: false),
                    TextContent = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentPages_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaProcessingJobs_DocumentId",
                table: "MediaProcessingJobs",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPages_DocumentId",
                table: "DocumentPages",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProcessingJobs_Documents_DocumentId",
                table: "MediaProcessingJobs",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaProcessingJobs_Documents_DocumentId",
                table: "MediaProcessingJobs");

            migrationBuilder.DropTable(
                name: "DocumentPages");

            migrationBuilder.DropIndex(
                name: "IX_MediaProcessingJobs_DocumentId",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Documents");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "MediaProcessingJobs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtractedText",
                table: "MediaProcessingJobs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPages",
                table: "MediaProcessingJobs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "MediaProcessingJobs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "MediaProcessingJobs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextContent",
                table: "Documents",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
