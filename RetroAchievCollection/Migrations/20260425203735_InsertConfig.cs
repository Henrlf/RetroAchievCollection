using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetroAchievCollection.Migrations
{
    /// <inheritdoc />
    public partial class InsertConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "configuration",
                columns: new[] { "Id", "ApiKey", "UserName" },
                values: new object[] { new Guid("166117e9-2488-4e1a-98fc-f3ec4ca298aa"), "", "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "configuration",
                keyColumn: "Id",
                keyValue: new Guid("166117e9-2488-4e1a-98fc-f3ec4ca298aa"));
        }
    }
}
