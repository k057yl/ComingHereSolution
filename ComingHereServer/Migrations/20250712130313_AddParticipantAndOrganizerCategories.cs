using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ComingHereServer.Migrations
{
    /// <inheritdoc />
    public partial class AddParticipantAndOrganizerCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "EventParticipants",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "EventOrganizers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrganizerCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizerCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParticipantCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipants_CategoryId",
                table: "EventParticipants",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EventOrganizers_CategoryId",
                table: "EventOrganizers",
                column: "CategoryId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventOrganizers_OrganizerCategory_CategoryId",
                table: "EventOrganizers");

            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipants_ParticipantCategory_CategoryId",
                table: "EventParticipants");

            migrationBuilder.DropTable(
                name: "OrganizerCategory");

            migrationBuilder.DropTable(
                name: "ParticipantCategory");

            migrationBuilder.DropIndex(
                name: "IX_EventParticipants_CategoryId",
                table: "EventParticipants");

            migrationBuilder.DropIndex(
                name: "IX_EventOrganizers_CategoryId",
                table: "EventOrganizers");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "EventParticipants");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "EventOrganizers");
        }
    }
}
