﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackgroundEmailServiceTest.Migrations
{
    /// <inheritdoc />
    public partial class IsDeletedFieldAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Emails",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Emails");
        }
    }
}