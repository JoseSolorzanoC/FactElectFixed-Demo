using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactElectFixed.Api.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Configuraciones",
            columns: table => new
            {
                Id = table.Column<string>(type: "TEXT", nullable: false),
                RucEmpresa = table.Column<string>(type: "TEXT", nullable: false),
                Password = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Configuraciones", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Configuraciones");
    }
}
