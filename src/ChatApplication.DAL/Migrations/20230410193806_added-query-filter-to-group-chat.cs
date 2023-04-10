using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApplication.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addedqueryfiltertogroupchat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "GroupChats",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_GroupChats_Name",
                table: "GroupChats",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GroupChats_Name",
                table: "GroupChats");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "GroupChats",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);
        }
    }
}
