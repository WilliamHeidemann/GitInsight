using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GitInsight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Commits",
                columns: table => new
                {
                    SHA = table.Column<string>(type: "text", nullable: false),
                    AuthorName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Date = table.Column<string>(type: "character varying(48)", nullable: false),
                    RepoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commits", x => x.SHA);
                });

            migrationBuilder.CreateTable(
                name: "Repositories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    NewestCommitSHA = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repositories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Repositories_FilePath",
                table: "Repositories",
                column: "FilePath",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commits");

            migrationBuilder.DropTable(
                name: "Repositories");
        }
    }
}
