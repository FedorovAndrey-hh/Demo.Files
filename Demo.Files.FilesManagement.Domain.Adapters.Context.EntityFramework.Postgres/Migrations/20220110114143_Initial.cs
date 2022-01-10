using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.Postgres.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Storages",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    version = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    limitations_totalSpace = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    limitations_totalFileCount = table.Column<long>(type: "bigint", nullable: false),
                    limitations_singleFileSize = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Directories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    storageId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directories", x => x.id);
                    table.ForeignKey(
                        name: "FK_Directories_Storages_storageId",
                        column: x => x.storageId,
                        principalTable: "Storages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    physicalId = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    size = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    directoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.id);
                    table.ForeignKey(
                        name: "FK_Files_Directories_directoryId",
                        column: x => x.directoryId,
                        principalTable: "Directories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Directories_storageId",
                table: "Directories",
                column: "storageId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_directoryId",
                table: "Files",
                column: "directoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Storages_version",
                table: "Storages",
                column: "version");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Directories");

            migrationBuilder.DropTable(
                name: "Storages");
        }
    }
}
