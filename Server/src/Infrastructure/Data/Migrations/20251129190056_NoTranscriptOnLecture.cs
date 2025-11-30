using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class NoTranscriptOnLecture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transcripts_Lectures_LectureId",
                table: "Transcripts");

            migrationBuilder.DropIndex(
                name: "IX_Transcripts_LectureId",
                table: "Transcripts");

            migrationBuilder.DropColumn(
                name: "LectureId",
                table: "Transcripts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LectureId",
                table: "Transcripts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transcripts_LectureId",
                table: "Transcripts",
                column: "LectureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transcripts_Lectures_LectureId",
                table: "Transcripts",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id");
        }
    }
}
