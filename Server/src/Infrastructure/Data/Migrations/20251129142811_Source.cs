using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Source : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "KeywordExtractionMediaProcessingJobId",
                table: "TranscriptKeyword",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResumeExtractionMediaProcessingJob_SourceText",
                table: "MediaProcessingJobs",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TranscriptKeyword_KeywordExtractionMediaProcessingJobId",
                table: "TranscriptKeyword",
                column: "KeywordExtractionMediaProcessingJobId");

            migrationBuilder.AddForeignKey(
                name: "FK_TranscriptKeyword_MediaProcessingJobs_KeywordExtractionMedi~",
                table: "TranscriptKeyword",
                column: "KeywordExtractionMediaProcessingJobId",
                principalTable: "MediaProcessingJobs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TranscriptKeyword_MediaProcessingJobs_KeywordExtractionMedi~",
                table: "TranscriptKeyword");

            migrationBuilder.DropIndex(
                name: "IX_TranscriptKeyword_KeywordExtractionMediaProcessingJobId",
                table: "TranscriptKeyword");

            migrationBuilder.DropColumn(
                name: "KeywordExtractionMediaProcessingJobId",
                table: "TranscriptKeyword");

            migrationBuilder.DropColumn(
                name: "ResumeExtractionMediaProcessingJob_SourceText",
                table: "MediaProcessingJobs");
        }
    }
}
