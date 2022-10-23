using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreStorage.Migrations
{
    public partial class Expiration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Expiration",
                table: "Licenses",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expiration",
                table: "Licenses");
        }
    }
}
