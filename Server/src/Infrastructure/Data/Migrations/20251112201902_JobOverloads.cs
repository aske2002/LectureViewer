using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class JobOverloads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "LectureContentId",
                table: "MediaProcessingJobs",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "InputResourceId",
                table: "MediaProcessingJobs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OutputResourceId",
                table: "MediaProcessingJobs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetFormat",
                table: "MediaProcessingJobs",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaProcessingJobs_InputResourceId",
                table: "MediaProcessingJobs",
                column: "InputResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaProcessingJobs_OutputResourceId",
                table: "MediaProcessingJobs",
                column: "OutputResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProcessingJobs_Resources_InputResourceId",
                table: "MediaProcessingJobs",
                column: "InputResourceId",
                principalTable: "Resources",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaProcessingJobs_Resources_InputResourceId",
                table: "MediaProcessingJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaProcessingJobs_Resources_OutputResourceId",
                table: "MediaProcessingJobs");

            migrationBuilder.DropIndex(
                name: "IX_MediaProcessingJobs_InputResourceId",
                table: "MediaProcessingJobs");

            migrationBuilder.DropIndex(
                name: "IX_MediaProcessingJobs_OutputResourceId",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "InputResourceId",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "OutputResourceId",
                table: "MediaProcessingJobs");

            migrationBuilder.DropColumn(
                name: "TargetFormat",
                table: "MediaProcessingJobs");

            migrationBuilder.AlterColumn<Guid>(
                name: "LectureContentId",
                table: "MediaProcessingJobs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
