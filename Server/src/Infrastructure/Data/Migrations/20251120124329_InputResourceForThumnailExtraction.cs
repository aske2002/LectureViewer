using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InputResourceForThumnailExtraction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaProcessingJobs_Resources_ThumbnailExtractionMediaProce~",
                table: "MediaProcessingJobs");

            migrationBuilder.AddColumn<Guid>(
                name: "ThumbnailExtractionMediaProcessingJob_InputResourceId",
                table: "MediaProcessingJobs",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaProcessingJobs_ThumbnailExtractionMediaProcessingJob_I~",
                table: "MediaProcessingJobs",
                column: "ThumbnailExtractionMediaProcessingJob_InputResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProcessingJobs_Resources_ThumbnailExtractionMediaProce~",
                table: "MediaProcessingJobs",
                column: "ThumbnailExtractionMediaProcessingJob_InputResourceId",
                principalTable: "Resources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProcessingJobs_Resources_ThumbnailExtractionMediaProc~1",
                table: "MediaProcessingJobs",
                column: "ThumbnailExtractionMediaProcessingJob_OutputResourceId",
                principalTable: "Resources",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaProcessingJobs_Resources_ThumbnailExtractionMediaProce~",
                table: "MediaProcessingJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaProcessingJobs_Resources_ThumbnailExtractionMediaProc~1",
                table: "MediaProcessingJobs");

            migrationBuilder.DropIndex(
                name: "IX_MediaProcessingJobs_ThumbnailExtractionMediaProcessingJob_I~",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "ThumbnailExtractionMediaProcessingJob_InputResourceId",
                table: "MediaProcessingJobs");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProcessingJobs_Resources_ThumbnailExtractionMediaProce~",
                table: "MediaProcessingJobs",
                column: "ThumbnailExtractionMediaProcessingJob_OutputResourceId",
                principalTable: "Resources",
                principalColumn: "Id");
        }
    }
}
