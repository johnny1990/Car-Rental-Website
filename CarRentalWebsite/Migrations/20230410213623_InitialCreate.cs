using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalWebsite.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kit_Nr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Licence_Plate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vehicle_Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vehicle_Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Purchase_Year = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activation_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Validity = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
