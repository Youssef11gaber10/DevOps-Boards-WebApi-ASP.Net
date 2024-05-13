using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Repository.Data.Migrations
{
    public partial class AddRealationWithIdentity22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeveloperId",
                table: "Tasks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_DeveloperId",
                table: "Tasks",
                column: "DeveloperId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_DeveloperId",
                table: "Tasks",
                column: "DeveloperId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);//modified from cascade to  no action
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_DeveloperId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_DeveloperId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "DeveloperId",
                table: "Tasks");
        }
    }
}
