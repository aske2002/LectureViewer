using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Dunno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ThumbnailResourceId",
                table: "Resources",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "MediaProcessingJobs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetBitrateKbps",
                table: "MediaProcessingJobs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetHeight",
                table: "MediaProcessingJobs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetWidth",
                table: "MediaProcessingJobs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ThumbnailExtractionMediaProcessingJob_OutputResourceId",
                table: "MediaProcessingJobs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "MediaProcessingJobs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ThumbnailResourceId",
                table: "Resources",
                column: "ThumbnailResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaProcessingJobs_ThumbnailExtractionMediaProcessingJob_O~",
                table: "MediaProcessingJobs",
                column: "ThumbnailExtractionMediaProcessingJob_OutputResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProcessingJobs_Resources_ThumbnailExtractionMediaProce~",
                table: "MediaProcessingJobs",
                column: "ThumbnailExtractionMediaProcessingJob_OutputResourceId",
                principalTable: "Resources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_Resources_ThumbnailResourceId",
                table: "Resources",
                column: "ThumbnailResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaProcessingJobs_Resources_ThumbnailExtractionMediaProce~",
                table: "MediaProcessingJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Resources_Resources_ThumbnailResourceId",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Resources_ThumbnailResourceId",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_MediaProcessingJobs_ThumbnailExtractionMediaProcessingJob_O~",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "ThumbnailResourceId",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "TargetBitrateKbps",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "TargetHeight",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "TargetWidth",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "ThumbnailExtractionMediaProcessingJob_OutputResourceId",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "MediaProcessingJobs");
        }
    }
}
