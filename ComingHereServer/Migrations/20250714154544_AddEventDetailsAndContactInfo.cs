using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ComingHereServer.Migrations
{
    /// <inheritdoc />
    public partial class AddEventDetailsAndContactInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MaxAttendees",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "ContactInfoId",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EventContactInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventDetailsId = table.Column<int>(type: "integer", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Website = table.Column<string>(type: "text", nullable: true),
                    Telegram = table.Column<string>(type: "text", nullable: true),
                    Instagram = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventContactInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    Address = table.Column<string>(type: "jsonb", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: true),
                    MaxAttendees = table.Column<int>(type: "integer", nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    ContactInfoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventDetails_EventContactInfos_ContactInfoId",
                        column: x => x.ContactInfoId,
                        principalTable: "EventContactInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventDetails_Events_Id",
                        column: x => x.Id,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_ContactInfoId",
                table: "Events",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_EventDetails_ContactInfoId",
                table: "EventDetails",
                column: "ContactInfoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_EventContactInfos_ContactInfoId",
                table: "Events",
                column: "ContactInfoId",
                principalTable: "EventContactInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_EventContactInfos_ContactInfoId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "EventDetails");

            migrationBuilder.DropTable(
                name: "EventContactInfos");

            migrationBuilder.DropIndex(
                name: "IX_Events_ContactInfoId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ContactInfoId",
                table: "Events");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Events",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Events",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "MaxAttendees",
                table: "Events",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Events",
                type: "numeric",
                nullable: true);
        }
    }
}
