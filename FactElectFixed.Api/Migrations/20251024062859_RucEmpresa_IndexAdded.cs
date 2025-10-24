using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactElectFixed.Api.Migrations;

/// <inheritdoc />
public partial class RucEmpresa_IndexAdded : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "RucEmpresa",
            table: "Configuraciones",
            column: "RucEmpresa",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "RucEmpresa",
            table: "Configuraciones");
    }
}
