using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class KeywordsInDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TranscriptItem_Transcripts_TranscriptId",
                table: "TranscriptItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TranscriptKeywordOccurrences_TranscriptKeyword_TranscriptKe~",
                table: "TranscriptKeywordOccurrences");

            migrationBuilder.DropTable(
                name: "TranscriptKeyword");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TranscriptItem",
                table: "TranscriptItem");

            migrationBuilder.RenameTable(
                name: "TranscriptItem",
                newName: "TranscriptItems");

            migrationBuilder.RenameColumn(
                name: "TranscriptKeywordId",
                table: "TranscriptKeywordOccurrences",
                newName: "TranscriptId");

            migrationBuilder.RenameColumn(
                name: "OccurrenceTime",
                table: "TranscriptKeywordOccurrences",
                newName: "To");

            migrationBuilder.RenameIndex(
                name: "IX_TranscriptKeywordOccurrences_TranscriptKeywordId",
                table: "TranscriptKeywordOccurrences",
                newName: "IX_TranscriptKeywordOccurrences_TranscriptId");

            migrationBuilder.RenameIndex(
                name: "IX_TranscriptItem_TranscriptId",
                table: "TranscriptItems",
                newName: "IX_TranscriptItems_TranscriptId");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "From",
                table: "TranscriptKeywordOccurrences",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<Guid>(
                name: "KeywordId",
                table: "TranscriptKeywordOccurrences",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TranscriptItems",
                table: "TranscriptItems",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseKeywords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    KeywordId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseKeywords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseKeywords_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseKeywords_Keywords_KeywordId",
                        column: x => x.KeywordId,
                        principalTable: "Keywords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentKeywords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    KeywordId = table.Column<Guid>(type: "uuid", nullable: false),
                    PageNumber = table.Column<int>(type: "integer", nullable: false),
                    SurroundingText = table.Column<string>(type: "text", nullable: false),
                    PositionX = table.Column<int>(type: "integer", nullable: false),
                    PositionY = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentKeywords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentKeywords_Keywords_KeywordId",
                        column: x => x.KeywordId,
                        principalTable: "Keywords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentKeywords_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KeywordKeywordExtractionMediaProcessingJob",
                columns: table => new
                {
                    ExtractedKeywordsId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceJobsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordKeywordExtractionMediaProcessingJob", x => new { x.ExtractedKeywordsId, x.SourceJobsId });
                    table.ForeignKey(
                        name: "FK_KeywordKeywordExtractionMediaProcessingJob_Keywords_Extract~",
                        column: x => x.ExtractedKeywordsId,
                        principalTable: "Keywords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KeywordKeywordExtractionMediaProcessingJob_MediaProcessingJ~",
                        column: x => x.SourceJobsId,
                        principalTable: "MediaProcessingJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TranscriptKeywordOccurrences_KeywordId",
                table: "TranscriptKeywordOccurrences",
                column: "KeywordId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseKeywords_CourseId",
                table: "CourseKeywords",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseKeywords_KeywordId",
                table: "CourseKeywords",
                column: "KeywordId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentKeywords_KeywordId",
                table: "DocumentKeywords",
                column: "KeywordId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentKeywords_ResourceId",
                table: "DocumentKeywords",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_KeywordKeywordExtractionMediaProcessingJob_SourceJobsId",
                table: "KeywordKeywordExtractionMediaProcessingJob",
                column: "SourceJobsId");

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_Text",
                table: "Keywords",
                column: "Text",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TranscriptItems_Transcripts_TranscriptId",
                table: "TranscriptItems",
                column: "TranscriptId",
                principalTable: "Transcripts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TranscriptKeywordOccurrences_Keywords_KeywordId",
                table: "TranscriptKeywordOccurrences",
                column: "KeywordId",
                principalTable: "Keywords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TranscriptKeywordOccurrences_Transcripts_TranscriptId",
                table: "TranscriptKeywordOccurrences",
                column: "TranscriptId",
                principalTable: "Transcripts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TranscriptItems_Transcripts_TranscriptId",
                table: "TranscriptItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TranscriptKeywordOccurrences_Keywords_KeywordId",
                table: "TranscriptKeywordOccurrences");

            migrationBuilder.DropForeignKey(
                name: "FK_TranscriptKeywordOccurrences_Transcripts_TranscriptId",
                table: "TranscriptKeywordOccurrences");

            migrationBuilder.DropTable(
                name: "CourseKeywords");

            migrationBuilder.DropTable(
                name: "DocumentKeywords");

            migrationBuilder.DropTable(
                name: "KeywordKeywordExtractionMediaProcessingJob");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropIndex(
                name: "IX_TranscriptKeywordOccurrences_KeywordId",
                table: "TranscriptKeywordOccurrences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TranscriptItems",
                table: "TranscriptItems");

            migrationBuilder.DropColumn(
                name: "From",
                table: "TranscriptKeywordOccurrences");

            migrationBuilder.DropColumn(
                name: "KeywordId",
                table: "TranscriptKeywordOccurrences");

            migrationBuilder.RenameTable(
                name: "TranscriptItems",
                newName: "TranscriptItem");

            migrationBuilder.RenameColumn(
                name: "TranscriptId",
                table: "TranscriptKeywordOccurrences",
                newName: "TranscriptKeywordId");

            migrationBuilder.RenameColumn(
                name: "To",
                table: "TranscriptKeywordOccurrences",
                newName: "OccurrenceTime");

            migrationBuilder.RenameIndex(
                name: "IX_TranscriptKeywordOccurrences_TranscriptId",
                table: "TranscriptKeywordOccurrences",
                newName: "IX_TranscriptKeywordOccurrences_TranscriptKeywordId");

            migrationBuilder.RenameIndex(
                name: "IX_TranscriptItems_TranscriptId",
                table: "TranscriptItem",
                newName: "IX_TranscriptItem_TranscriptId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TranscriptItem",
                table: "TranscriptItem",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TranscriptKeyword",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TranscriptId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Keyword = table.Column<string>(type: "text", nullable: false),
                    KeywordExtractionMediaProcessingJobId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranscriptKeyword", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TranscriptKeyword_MediaProcessingJobs_KeywordExtractionMedi~",
                        column: x => x.KeywordExtractionMediaProcessingJobId,
                        principalTable: "MediaProcessingJobs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TranscriptKeyword_Transcripts_TranscriptId",
                        column: x => x.TranscriptId,
                        principalTable: "Transcripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TranscriptKeyword_KeywordExtractionMediaProcessingJobId",
                table: "TranscriptKeyword",
                column: "KeywordExtractionMediaProcessingJobId");

            migrationBuilder.CreateIndex(
                name: "IX_TranscriptKeyword_TranscriptId",
                table: "TranscriptKeyword",
                column: "TranscriptId");

            migrationBuilder.AddForeignKey(
                name: "FK_TranscriptItem_Transcripts_TranscriptId",
                table: "TranscriptItem",
                column: "TranscriptId",
                principalTable: "Transcripts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TranscriptKeywordOccurrences_TranscriptKeyword_TranscriptKe~",
                table: "TranscriptKeywordOccurrences",
                column: "TranscriptKeywordId",
                principalTable: "TranscriptKeyword",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
