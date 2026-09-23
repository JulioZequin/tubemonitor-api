using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TubeMonitor.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Canais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Handle = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Nicho = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Inscritos = table.Column<long>(type: "INTEGER", nullable: false),
                    CadastradoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Canais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    YoutubeVideoId = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Visualizacoes = table.Column<long>(type: "INTEGER", nullable: false),
                    Curtidas = table.Column<long>(type: "INTEGER", nullable: false),
                    Comentarios = table.Column<long>(type: "INTEGER", nullable: false),
                    PublicadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CadastradoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CanalId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videos_Canais_CanalId",
                        column: x => x.CanalId,
                        principalTable: "Canais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Canais",
                columns: new[] { "Id", "AtualizadoEm", "CadastradoEm", "Handle", "Inscritos", "Nicho", "Nome" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 9, 1, 12, 0, 0, 0, DateTimeKind.Utc), "@codigofacil", 185000L, "Tecnologia", "Código Fácil" },
                    { 2, null, new DateTime(2026, 9, 1, 12, 0, 0, 0, DateTimeKind.Utc), "@cozinha10min", 920000L, "Culinária", "Cozinha em 10 Minutos" },
                    { 3, null, new DateTime(2026, 9, 1, 12, 0, 0, 0, DateTimeKind.Utc), "@treinoemcasabr", 430000L, "Fitness", "Treino em Casa BR" }
                });

            migrationBuilder.InsertData(
                table: "Videos",
                columns: new[] { "Id", "AtualizadoEm", "CadastradoEm", "CanalId", "Comentarios", "Curtidas", "PublicadoEm", "Titulo", "Visualizacoes", "YoutubeVideoId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 9, 1, 12, 0, 0, 0, DateTimeKind.Utc), 1, 412L, 3900L, new DateTime(2026, 8, 20, 18, 0, 0, 0, DateTimeKind.Utc), "C# em 15 minutos: do zero à primeira API", 48300L, "cF4sPq9LmZa" },
                    { 2, null, new DateTime(2026, 9, 1, 12, 0, 0, 0, DateTimeKind.Utc), 1, 156L, 1820L, new DateTime(2026, 8, 28, 18, 0, 0, 0, DateTimeKind.Utc), "Entity Framework Core explicado com exemplos", 21750L, "Ht7kWq2NbXe" },
                    { 3, null, new DateTime(2026, 9, 1, 12, 0, 0, 0, DateTimeKind.Utc), 2, 1230L, 18400L, new DateTime(2026, 8, 15, 12, 0, 0, 0, DateTimeKind.Utc), "Lasanha de frigideira em 10 minutos", 312000L, "Rt9yUi3OpLs" },
                    { 4, null, new DateTime(2026, 9, 1, 12, 0, 0, 0, DateTimeKind.Utc), 2, 380L, 6100L, new DateTime(2026, 8, 30, 12, 0, 0, 0, DateTimeKind.Utc), "3 cafés da manhã rápidos e baratos", 97500L, "Qw1eRt5YuIo" },
                    { 5, null, new DateTime(2026, 9, 1, 12, 0, 0, 0, DateTimeKind.Utc), 3, 640L, 9870L, new DateTime(2026, 8, 25, 7, 0, 0, 0, DateTimeKind.Utc), "Treino de 20 minutos sem equipamento", 154200L, "Zx8cVb4NmAs" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Canais_Handle",
                table: "Canais",
                column: "Handle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_CanalId",
                table: "Videos",
                column: "CanalId");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_YoutubeVideoId",
                table: "Videos",
                column: "YoutubeVideoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "Canais");
        }
    }
}
