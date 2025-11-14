using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ResourceRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LectureContents_ResourceId",
                table: "LectureContents",
                column: "ResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_LectureContents_Resources_ResourceId",
                table: "LectureContents",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LectureContents_Resources_ResourceId",
                table: "LectureContents");

            migrationBuilder.DropIndex(
                name: "IX_LectureContents_ResourceId",
                table: "LectureContents");
        }
    }
}
