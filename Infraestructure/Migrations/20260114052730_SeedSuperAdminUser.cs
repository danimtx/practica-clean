using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedSuperAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "CargoId", "Email", "EstaActivo", "FotoPerfil", "Nombre", "PasswordHash", "Permisos", "RefreshToken", "RefreshTokenExpiryTime" },
                values: new object[] { new Guid("f8a8b8e8-8e4f-4b8a-b8e8-f8a8b8e8f8a8"), new Guid("a7e1b52c-1a9d-4da0-9a25-7b3b4f5a7a53"), "superadmin@cybercorp.com", true, "/uploads/profiles/default.png", "SuperAdmin", "superadmin123", "inspeccion:crear,inspeccion:editar,inspeccion:estado,inspeccion:archivo:subir,inspeccion:archivo:borrar,usuario:gestionar,cargo:gestionar", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: new Guid("f8a8b8e8-8e4f-4b8a-b8e8-f8a8b8e8f8a8"));
        }
    }
}
