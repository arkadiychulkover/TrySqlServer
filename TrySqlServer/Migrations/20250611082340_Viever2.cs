using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrySqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Viever2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW UserMoviesView AS
                SELECT 
                    u.Username,
                    m.Title
                FROM Users u
                JOIN Movie m ON m.UserId = u.Id
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS UserMoviesView");
        }
    }
}
