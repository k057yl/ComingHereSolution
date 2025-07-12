using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComingHereServer.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizerAndParticipantCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventOrganizers_OrganizerCategory_CategoryId",
                table: "EventOrganizers");

            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipants_ParticipantCategory_CategoryId",
                table: "EventParticipants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParticipantCategory",
                table: "ParticipantCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizerCategory",
                table: "OrganizerCategory");

            migrationBuilder.RenameTable(
                name: "ParticipantCategory",
                newName: "ParticipantCategories");

            migrationBuilder.RenameTable(
                name: "OrganizerCategory",
                newName: "OrganizerCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParticipantCategories",
                table: "ParticipantCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizerCategories",
                table: "OrganizerCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventOrganizers_OrganizerCategories_CategoryId",
                table: "EventOrganizers",
                column: "CategoryId",
                principalTable: "OrganizerCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipants_ParticipantCategories_CategoryId",
                table: "EventParticipants",
                column: "CategoryId",
                principalTable: "ParticipantCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventOrganizers_OrganizerCategories_CategoryId",
                table: "EventOrganizers");

            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipants_ParticipantCategories_CategoryId",
                table: "EventParticipants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParticipantCategories",
                table: "ParticipantCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizerCategories",
                table: "OrganizerCategories");

            migrationBuilder.RenameTable(
                name: "ParticipantCategories",
                newName: "ParticipantCategory");

            migrationBuilder.RenameTable(
                name: "OrganizerCategories",
                newName: "OrganizerCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParticipantCategory",
                table: "ParticipantCategory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizerCategory",
                table: "OrganizerCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventOrganizers_OrganizerCategory_CategoryId",
                table: "EventOrganizers",
                column: "CategoryId",
                principalTable: "OrganizerCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipants_ParticipantCategory_CategoryId",
                table: "EventParticipants",
                column: "CategoryId",
                principalTable: "ParticipantCategory",
                principalColumn: "Id");
        }
    }
}
