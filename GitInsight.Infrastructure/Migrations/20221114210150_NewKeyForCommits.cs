using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GitInsight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewKeyForCommits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Commits",
                table: "Commits");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Commits",
                table: "Commits",
                columns: new[] { "SHA", "RepoId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Commits",
                table: "Commits");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Commits",
                table: "Commits",
                column: "SHA");
        }
    }
}
