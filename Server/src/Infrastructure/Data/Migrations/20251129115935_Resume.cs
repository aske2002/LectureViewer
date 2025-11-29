using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Resume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transcripts_JobId",
                table: "Transcripts");

            migrationBuilder.AddColumn<string>(
                name: "Resume",
                table: "MediaProcessingJobs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceText",
                table: "MediaProcessingJobs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetLineLength",
                table: "MediaProcessingJobs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transcripts_JobId",
                table: "Transcripts",
                column: "JobId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transcripts_JobId",
                table: "Transcripts");

            migrationBuilder.DropColumn(
                name: "Resume",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "SourceText",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "TargetLineLength",
                table: "MediaProcessingJobs");

            migrationBuilder.CreateIndex(
                name: "IX_Transcripts_JobId",
                table: "Transcripts",
                column: "JobId");
        }
    }
}
