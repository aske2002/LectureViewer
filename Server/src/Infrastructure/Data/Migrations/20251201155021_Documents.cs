using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Documents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentKeywords_Resources_ResourceId",
                table: "DocumentKeywords");

            migrationBuilder.RenameColumn(
                name: "ResourceId",
                table: "DocumentKeywords",
                newName: "DocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentKeywords_ResourceId",
                table: "DocumentKeywords",
                newName: "IX_DocumentKeywords_DocumentId");

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

            migrationBuilder.AddColumn<Guid>(
                name: "MediaTranscodingMediaProcessingJob_InputResourceId",
                table: "MediaProcessingJobs",
                type: "uuid",
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

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentId",
                table: "LectureContents",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumberOfPages = table.Column<int>(type: "integer", nullable: false),
                    TextContent = table.Column<string>(type: "text", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaProcessingJobs_MediaTranscodingMediaProcessingJob_Inpu~",
                table: "MediaProcessingJobs",
                column: "MediaTranscodingMediaProcessingJob_InputResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_LectureContents_DocumentId",
                table: "LectureContents",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ResourceId",
                table: "Documents",
                column: "ResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentKeywords_Documents_DocumentId",
                table: "DocumentKeywords",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LectureContents_Documents_DocumentId",
                table: "LectureContents",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProcessingJobs_Resources_MediaTranscodingMediaProcessi~",
                table: "MediaProcessingJobs",
                column: "MediaTranscodingMediaProcessingJob_InputResourceId",
                principalTable: "Resources",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentKeywords_Documents_DocumentId",
                table: "DocumentKeywords");

            migrationBuilder.DropForeignKey(
                name: "FK_LectureContents_Documents_DocumentId",
                table: "LectureContents");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaProcessingJobs_Resources_MediaTranscodingMediaProcessi~",
                table: "MediaProcessingJobs");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_MediaProcessingJobs_MediaTranscodingMediaProcessingJob_Inpu~",
                table: "MediaProcessingJobs");

            migrationBuilder.DropIndex(
                name: "IX_LectureContents_DocumentId",
                table: "LectureContents");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "ExtractedText",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "MediaTranscodingMediaProcessingJob_InputResourceId",
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
                name: "DocumentId",
                table: "LectureContents");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                table: "DocumentKeywords",
                newName: "ResourceId");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentKeywords_DocumentId",
                table: "DocumentKeywords",
                newName: "IX_DocumentKeywords_ResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentKeywords_Resources_ResourceId",
                table: "DocumentKeywords",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
