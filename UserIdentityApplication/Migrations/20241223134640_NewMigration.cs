using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserIdentityApplication.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("5902e3c0-49f9-4aea-af95-3493fffc79c2"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("bbc34c15-6ff5-422f-85db-6d691377405c"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("6dad7398-0158-4324-8ffd-e7f572045981"), "Admin" },
                    { new Guid("ba997faa-aa61-4909-86b0-939805f9f60b"), "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("6dad7398-0158-4324-8ffd-e7f572045981"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("ba997faa-aa61-4909-86b0-939805f9f60b"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("5902e3c0-49f9-4aea-af95-3493fffc79c2"), "User" },
                    { new Guid("bbc34c15-6ff5-422f-85db-6d691377405c"), "Admin" }
                });
        }
    }
}
