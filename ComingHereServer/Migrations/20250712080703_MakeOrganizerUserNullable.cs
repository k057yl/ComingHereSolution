using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComingHereServer.Migrations
{
    /// <inheritdoc />
    public partial class MakeOrganizerUserNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventOrganizers_AspNetUsers_ApplicationUserId",
                table: "EventOrganizers");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "EventOrganizers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_EventOrganizers_AspNetUsers_ApplicationUserId",
                table: "EventOrganizers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventOrganizers_AspNetUsers_ApplicationUserId",
                table: "EventOrganizers");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "EventOrganizers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EventOrganizers_AspNetUsers_ApplicationUserId",
                table: "EventOrganizers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
