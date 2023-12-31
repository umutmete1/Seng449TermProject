using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TermProject.Migrations
{
    /// <inheritdoc />
    public partial class v4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_AspNetUsers_MyUserId",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_MyUserId",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "MyUserId",
                table: "Stocks");

            migrationBuilder.CreateTable(
                name: "UserWatchlist",
                columns: table => new
                {
                    MyUserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StockId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWatchlist", x => new { x.MyUserId, x.StockId });
                    table.ForeignKey(
                        name: "FK_UserWatchlist_AspNetUsers_MyUserId",
                        column: x => x.MyUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWatchlist_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserWatchlist_StockId",
                table: "UserWatchlist",
                column: "StockId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserWatchlist");

            migrationBuilder.AddColumn<string>(
                name: "MyUserId",
                table: "Stocks",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_MyUserId",
                table: "Stocks",
                column: "MyUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_AspNetUsers_MyUserId",
                table: "Stocks",
                column: "MyUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
