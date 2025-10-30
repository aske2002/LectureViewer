using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedSecondaryIdFromBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trip_Id",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_TripDescription_Id",
                table: "TripDescriptions");

            migrationBuilder.DropIndex(
                name: "IX_TodoList_Id",
                table: "TodoLists");

            migrationBuilder.DropIndex(
                name: "IX_TodoItem_Id",
                table: "TodoItems");

            migrationBuilder.DropIndex(
                name: "IX_Resource_Id",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Destination_Id",
                table: "Destinations");

            migrationBuilder.DropIndex(
                name: "IX_Country_Id",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_ClassYear_Id",
                table: "ClassYears");

            migrationBuilder.DropColumn(
                name: "_id",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "_id",
                table: "TripDescriptions");

            migrationBuilder.DropColumn(
                name: "_id",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "_id",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "_id",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "_id",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "_id",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "_id",
                table: "ClassYears");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "_id",
                table: "Trips",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "_id",
                table: "TripDescriptions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "_id",
                table: "TodoLists",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "_id",
                table: "TodoItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "_id",
                table: "Resources",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "_id",
                table: "Destinations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "_id",
                table: "Countries",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "_id",
                table: "ClassYears",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Trip_Id",
                table: "Trips",
                column: "_id");

            migrationBuilder.CreateIndex(
                name: "IX_TripDescription_Id",
                table: "TripDescriptions",
                column: "_id");

            migrationBuilder.CreateIndex(
                name: "IX_TodoList_Id",
                table: "TodoLists",
                column: "_id");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItem_Id",
                table: "TodoItems",
                column: "_id");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_Id",
                table: "Resources",
                column: "_id");

            migrationBuilder.CreateIndex(
                name: "IX_Destination_Id",
                table: "Destinations",
                column: "_id");

            migrationBuilder.CreateIndex(
                name: "IX_Country_Id",
                table: "Countries",
                column: "_id");

            migrationBuilder.CreateIndex(
                name: "IX_ClassYear_Id",
                table: "ClassYears",
                column: "_id");
        }
    }
}
