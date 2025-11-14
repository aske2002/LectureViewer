using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AssociatedResources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentResourceId",
                table: "Resources",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ParentResourceId",
                table: "Resources",
                column: "ParentResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_Resources_ParentResourceId",
                table: "Resources",
                column: "ParentResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_Resources_ParentResourceId",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Resources_ParentResourceId",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "ParentResourceId",
                table: "Resources");
        }
    }
}
