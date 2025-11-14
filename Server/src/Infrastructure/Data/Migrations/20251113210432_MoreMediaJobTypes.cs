using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MoreMediaJobTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaProcessingJobLogs_MediaProcessingJobs_JobId",
                table: "MediaProcessingJobLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaProcessingJobs_Resources_OutputResourceId",
                table: "MediaProcessingJobs");

            migrationBuilder.RenameColumn(
                name: "RetryCount",
                table: "MediaProcessingJobs",
                newName: "MaxRetries");

            migrationBuilder.RenameColumn(
                name: "JobId",
                table: "MediaProcessingJobLogs",
                newName: "AttemptId");

            migrationBuilder.RenameIndex(
                name: "IX_MediaProcessingJobLogs_JobId",
                table: "MediaProcessingJobLogs",
                newName: "IX_MediaProcessingJobLogs_AttemptId");

            migrationBuilder.AlterColumn<string>(
                name: "ErrorMessage",
                table: "MediaProcessingJobs",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OfficeConversionMediaProcessingJob_InputResourceId",
                table: "MediaProcessingJobs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OfficeConversionMediaProcessingJob_OutputResourceId",
                table: "MediaProcessingJobs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficeConversionMediaProcessingJob_TargetFormat",
                table: "MediaProcessingJobs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentJobId",
                table: "MediaProcessingJobs",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MediaProcessingJobAttempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaProcessingJobAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaProcessingJobAttempts_MediaProcessingJobs_JobId",
                        column: x => x.JobId,
                        principalTable: "MediaProcessingJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaProcessingJobs_OfficeConversionMediaProcessingJob_Inpu~",
                table: "MediaProcessingJobs",
                column: "OfficeConversionMediaProcessingJob_InputResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaProcessingJobs_OfficeConversionMediaProcessingJob_Outp~",
                table: "MediaProcessingJobs",
                column: "OfficeConversionMediaProcessingJob_OutputResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaProcessingJobs_ParentJobId",
                table: "MediaProcessingJobs",
                column: "ParentJobId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaProcessingJobAttempts_JobId",
                table: "MediaProcessingJobAttempts",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProcessingJobLogs_MediaProcessingJobAttempts_AttemptId",
                table: "MediaProcessingJobLogs",
                column: "AttemptId",
                principalTable: "MediaProcessingJobAttempts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProcessingJobs_MediaProcessingJobs_ParentJobId",
                table: "MediaProcessingJobs",
                column: "ParentJobId",
                principalTable: "MediaProcessingJobs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProcessingJobs_Resources_OfficeConversionMediaProcessi~",
                table: "MediaProcessingJobs",
                column: "OfficeConversionMediaProcessingJob_InputResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProcessingJobs_Resources_OfficeConversionMediaProcess~1",
                table: "MediaProcessingJobs",
                column: "OfficeConversionMediaProcessingJob_OutputResourceId",
                principalTable: "Resources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProcessingJobs_Resources_OutputResourceId",
                table: "MediaProcessingJobs",
                column: "OutputResourceId",
                principalTable: "Resources",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaProcessingJobLogs_MediaProcessingJobAttempts_AttemptId",
                table: "MediaProcessingJobLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaProcessingJobs_MediaProcessingJobs_ParentJobId",
                table: "MediaProcessingJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaProcessingJobs_Resources_OfficeConversionMediaProcessi~",
                table: "MediaProcessingJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaProcessingJobs_Resources_OfficeConversionMediaProcess~1",
                table: "MediaProcessingJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaProcessingJobs_Resources_OutputResourceId",
                table: "MediaProcessingJobs");

            migrationBuilder.DropTable(
                name: "MediaProcessingJobAttempts");

            migrationBuilder.DropIndex(
                name: "IX_MediaProcessingJobs_OfficeConversionMediaProcessingJob_Inpu~",
                table: "MediaProcessingJobs");

            migrationBuilder.DropIndex(
                name: "IX_MediaProcessingJobs_OfficeConversionMediaProcessingJob_Outp~",
                table: "MediaProcessingJobs");

            migrationBuilder.DropIndex(
                name: "IX_MediaProcessingJobs_ParentJobId",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "OfficeConversionMediaProcessingJob_InputResourceId",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "OfficeConversionMediaProcessingJob_OutputResourceId",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "OfficeConversionMediaProcessingJob_TargetFormat",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "ParentJobId",
                table: "MediaProcessingJobs");

            migrationBuilder.RenameColumn(
                name: "MaxRetries",
                table: "MediaProcessingJobs",
                newName: "RetryCount");

            migrationBuilder.RenameColumn(
                name: "AttemptId",
                table: "MediaProcessingJobLogs",
                newName: "JobId");

            migrationBuilder.RenameIndex(
                name: "IX_MediaProcessingJobLogs_AttemptId",
                table: "MediaProcessingJobLogs",
                newName: "IX_MediaProcessingJobLogs_JobId");

            migrationBuilder.AlterColumn<string>(
                name: "ErrorMessage",
                table: "MediaProcessingJobs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProcessingJobLogs_MediaProcessingJobs_JobId",
                table: "MediaProcessingJobLogs",
                column: "JobId",
                principalTable: "MediaProcessingJobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProcessingJobs_Resources_OutputResourceId",
                table: "MediaProcessingJobs",
                column: "OutputResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
