using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class initalCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_Donors_DonorId1",
                table: "Gifts");

            migrationBuilder.DropIndex(
                name: "IX_Gifts_DonorId1",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "DonorId1",
                table: "Gifts");

            migrationBuilder.AlterColumn<int>(
                name: "DonorId",
                table: "Gifts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_DonorId",
                table: "Gifts",
                column: "DonorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_Donors_DonorId",
                table: "Gifts",
                column: "DonorId",
                principalTable: "Donors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_Donors_DonorId",
                table: "Gifts");

            migrationBuilder.DropIndex(
                name: "IX_Gifts_DonorId",
                table: "Gifts");

            migrationBuilder.AlterColumn<string>(
                name: "DonorId",
                table: "Gifts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DonorId1",
                table: "Gifts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_DonorId1",
                table: "Gifts",
                column: "DonorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_Donors_DonorId1",
                table: "Gifts",
                column: "DonorId1",
                principalTable: "Donors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
