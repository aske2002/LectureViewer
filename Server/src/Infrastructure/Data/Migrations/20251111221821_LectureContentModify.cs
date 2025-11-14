using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class LectureContentModify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_Jobs_ArtifactFromJobId",
                table: "Flashcards");

            migrationBuilder.DropForeignKey(
                name: "FK_LectureTranscripts_Jobs_ArtifactFromJobId",
                table: "LectureTranscripts");

            migrationBuilder.DropTable(
                name: "JobLogs");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "LectureContents",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LectureContents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMainContent",
                table: "LectureContents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LectureContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MediaProcessingJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    JobType = table.Column<string>(type: "text", nullable: false),
                    LectureContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaProcessingJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaProcessingJobs_LectureContents_LectureContentId",
                        column: x => x.LectureContentId,
                        principalTable: "LectureContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaProcessingJobLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    LoggedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    ProgressPercentage = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaProcessingJobLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaProcessingJobLogs_MediaProcessingJobs_JobId",
                        column: x => x.JobId,
                        principalTable: "MediaProcessingJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaProcessingJobLogs_JobId",
                table: "MediaProcessingJobLogs",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaProcessingJobs_LectureContentId",
                table: "MediaProcessingJobs",
                column: "LectureContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_MediaProcessingJobs_ArtifactFromJobId",
                table: "Flashcards",
                column: "ArtifactFromJobId",
                principalTable: "MediaProcessingJobs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LectureTranscripts_MediaProcessingJobs_ArtifactFromJobId",
                table: "LectureTranscripts",
                column: "ArtifactFromJobId",
                principalTable: "MediaProcessingJobs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_MediaProcessingJobs_ArtifactFromJobId",
                table: "Flashcards");

            migrationBuilder.DropForeignKey(
                name: "FK_LectureTranscripts_MediaProcessingJobs_ArtifactFromJobId",
                table: "LectureTranscripts");

            migrationBuilder.DropTable(
                name: "MediaProcessingJobLogs");

            migrationBuilder.DropTable(
                name: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "LectureContents");

            migrationBuilder.DropColumn(
                name: "IsMainContent",
                table: "LectureContents");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "LectureContents");

            migrationBuilder.AlterColumn<int>(
                name: "ContentType",
                table: "LectureContents",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LectureContentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_LectureContents_LectureContentId",
                        column: x => x.LectureContentId,
                        principalTable: "LectureContents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    LoggedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobLogs_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobLogs_JobId",
                table: "JobLogs",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_LectureContentId",
                table: "Jobs",
                column: "LectureContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_Jobs_ArtifactFromJobId",
                table: "Flashcards",
                column: "ArtifactFromJobId",
                principalTable: "Jobs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LectureTranscripts_Jobs_ArtifactFromJobId",
                table: "LectureTranscripts",
                column: "ArtifactFromJobId",
                principalTable: "Jobs",
                principalColumn: "Id");
        }
    }
}
