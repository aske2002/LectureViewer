using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactoreJobsMore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LectureTranscriptKeywordOccurrences");

            migrationBuilder.DropTable(
                name: "LectureTranscriptTimestamps");

            migrationBuilder.DropTable(
                name: "LectureTranscriptKeywords");

            migrationBuilder.DropTable(
                name: "LectureTranscripts");

            migrationBuilder.AddColumn<Guid>(
                name: "LectureContentId1",
                table: "MediaProcessingJobs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TranscriptId",
                table: "LectureContents",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Transcripts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: true),
                    Language = table.Column<string>(type: "text", nullable: false),
                    TranscriptText = table.Column<string>(type: "text", nullable: false),
                    LectureId = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transcripts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transcripts_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transcripts_MediaProcessingJobs_JobId",
                        column: x => x.JobId,
                        principalTable: "MediaProcessingJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transcripts_Resources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TranscriptItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TranscriptId = table.Column<Guid>(type: "uuid", nullable: false),
                    From = table.Column<TimeSpan>(type: "interval", nullable: false),
                    To = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Confidence = table.Column<double>(type: "double precision", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranscriptItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TranscriptItem_Transcripts_TranscriptId",
                        column: x => x.TranscriptId,
                        principalTable: "Transcripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TranscriptKeyword",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TranscriptId = table.Column<Guid>(type: "uuid", nullable: false),
                    Keyword = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranscriptKeyword", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TranscriptKeyword_Transcripts_TranscriptId",
                        column: x => x.TranscriptId,
                        principalTable: "Transcripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TranscriptKeywordOccurrences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TranscriptKeywordId = table.Column<Guid>(type: "uuid", nullable: false),
                    OccurrenceTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    SurroundingText = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranscriptKeywordOccurrences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TranscriptKeywordOccurrences_TranscriptKeyword_TranscriptKe~",
                        column: x => x.TranscriptKeywordId,
                        principalTable: "TranscriptKeyword",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaProcessingJobs_LectureContentId1",
                table: "MediaProcessingJobs",
                column: "LectureContentId1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LectureContents_TranscriptId",
                table: "LectureContents",
                column: "TranscriptId");

            migrationBuilder.CreateIndex(
                name: "IX_TranscriptItem_TranscriptId",
                table: "TranscriptItem",
                column: "TranscriptId");

            migrationBuilder.CreateIndex(
                name: "IX_TranscriptKeyword_TranscriptId",
                table: "TranscriptKeyword",
                column: "TranscriptId");

            migrationBuilder.CreateIndex(
                name: "IX_TranscriptKeywordOccurrences_TranscriptKeywordId",
                table: "TranscriptKeywordOccurrences",
                column: "TranscriptKeywordId");

            migrationBuilder.CreateIndex(
                name: "IX_Transcripts_JobId",
                table: "Transcripts",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Transcripts_LectureId",
                table: "Transcripts",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_Transcripts_SourceId",
                table: "Transcripts",
                column: "SourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_LectureContents_Transcripts_TranscriptId",
                table: "LectureContents",
                column: "TranscriptId",
                principalTable: "Transcripts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProcessingJobs_LectureContents_LectureContentId1",
                table: "MediaProcessingJobs",
                column: "LectureContentId1",
                principalTable: "LectureContents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LectureContents_Transcripts_TranscriptId",
                table: "LectureContents");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaProcessingJobs_LectureContents_LectureContentId1",
                table: "MediaProcessingJobs");

            migrationBuilder.DropTable(
                name: "TranscriptItem");

            migrationBuilder.DropTable(
                name: "TranscriptKeywordOccurrences");

            migrationBuilder.DropTable(
                name: "TranscriptKeyword");

            migrationBuilder.DropTable(
                name: "Transcripts");

            migrationBuilder.DropIndex(
                name: "IX_MediaProcessingJobs_LectureContentId1",
                table: "MediaProcessingJobs");

            migrationBuilder.DropIndex(
                name: "IX_LectureContents_TranscriptId",
                table: "LectureContents");

            migrationBuilder.DropColumn(
                name: "LectureContentId1",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "TranscriptId",
                table: "LectureContents");

            migrationBuilder.CreateTable(
                name: "LectureTranscripts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ArtifactFromJobId = table.Column<Guid>(type: "uuid", nullable: true),
                    LectureId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Summary = table.Column<string>(type: "text", nullable: false),
                    TranscriptText = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LectureTranscripts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LectureTranscripts_LectureContents_SourceId",
                        column: x => x.SourceId,
                        principalTable: "LectureContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LectureTranscripts_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LectureTranscripts_MediaProcessingJobs_ArtifactFromJobId",
                        column: x => x.ArtifactFromJobId,
                        principalTable: "MediaProcessingJobs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LectureTranscriptKeywords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LectureTranscriptId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Keyword = table.Column<string>(type: "text", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LectureTranscriptKeywords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LectureTranscriptKeywords_LectureTranscripts_LectureTranscr~",
                        column: x => x.LectureTranscriptId,
                        principalTable: "LectureTranscripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LectureTranscriptTimestamps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LectureTranscriptId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LectureTranscriptTimestamps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LectureTranscriptTimestamps_LectureTranscripts_LectureTrans~",
                        column: x => x.LectureTranscriptId,
                        principalTable: "LectureTranscripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LectureTranscriptKeywordOccurrences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LectureTranscriptKeywordId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    OccurrenceTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    SurroundingText = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LectureTranscriptKeywordOccurrences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LectureTranscriptKeywordOccurrences_LectureTranscriptKeywor~",
                        column: x => x.LectureTranscriptKeywordId,
                        principalTable: "LectureTranscriptKeywords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LectureTranscriptKeywordOccurrences_LectureTranscriptKeywor~",
                table: "LectureTranscriptKeywordOccurrences",
                column: "LectureTranscriptKeywordId");

            migrationBuilder.CreateIndex(
                name: "IX_LectureTranscriptKeywords_LectureTranscriptId",
                table: "LectureTranscriptKeywords",
                column: "LectureTranscriptId");

            migrationBuilder.CreateIndex(
                name: "IX_LectureTranscripts_ArtifactFromJobId",
                table: "LectureTranscripts",
                column: "ArtifactFromJobId");

            migrationBuilder.CreateIndex(
                name: "IX_LectureTranscripts_LectureId",
                table: "LectureTranscripts",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_LectureTranscripts_SourceId",
                table: "LectureTranscripts",
                column: "SourceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LectureTranscriptTimestamps_LectureTranscriptId",
                table: "LectureTranscriptTimestamps",
                column: "LectureTranscriptId");
        }
    }
}
