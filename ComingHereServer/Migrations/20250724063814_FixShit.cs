using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComingHereServer.Migrations
{
    /// <inheritdoc />
    public partial class FixShit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentReviewId",
                table: "EventReviews",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventReviews_ParentReviewId",
                table: "EventReviews",
                column: "ParentReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventReviews_EventReviews_ParentReviewId",
                table: "EventReviews",
                column: "ParentReviewId",
                principalTable: "EventReviews",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventReviews_EventReviews_ParentReviewId",
                table: "EventReviews");

            migrationBuilder.DropIndex(
                name: "IX_EventReviews_ParentReviewId",
                table: "EventReviews");

            migrationBuilder.DropColumn(
                name: "ParentReviewId",
                table: "EventReviews");
        }
    }
}
