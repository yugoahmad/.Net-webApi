using Microsoft.EntityFrameworkCore.Migrations;

namespace Northwind.Entities.Migrations
{
    public partial class AddRolesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "67dd70ba-7f66-48d0-bbd3-98d6010cf3a9", "53bfa066-483e-42c6-b185-a0995c318265", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fce28dca-2375-42d9-a496-0a9c19415325", "65c0d0ee-4e02-4fd2-b4ed-23537bc98b9c", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "67dd70ba-7f66-48d0-bbd3-98d6010cf3a9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fce28dca-2375-42d9-a496-0a9c19415325");
        }
    }
}
