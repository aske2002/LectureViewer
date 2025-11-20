using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Dunno2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaProcessingJobs_Resources_InputResourceId",
                table: "MediaProcessingJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaProcessingJobs_Resources_OfficeConversionMediaProcessi~",
                table: "MediaProcessingJobs");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProcessingJobs_Resources_InputResourceId",
                table: "MediaProcessingJobs",
                column: "InputResourceId",
                principalTable: "Resources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProcessingJobs_Resources_OfficeConversionMediaProcessi~",
                table: "MediaProcessingJobs",
                column: "OfficeConversionMediaProcessingJob_InputResourceId",
                principalTable: "Resources",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaProcessingJobs_Resources_InputResourceId",
                table: "MediaProcessingJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaProcessingJobs_Resources_OfficeConversionMediaProcessi~",
                table: "MediaProcessingJobs");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProcessingJobs_Resources_InputResourceId",
                table: "MediaProcessingJobs",
                column: "InputResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProcessingJobs_Resources_OfficeConversionMediaProcessi~",
                table: "MediaProcessingJobs",
                column: "OfficeConversionMediaProcessingJob_InputResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
