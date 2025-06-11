using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrySqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Podannya : Migration
    {
        /// <inheritdoc />
        // В новій міграції
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE AddUser
                    @Username NVARCHAR(50),
                    @Email NVARCHAR(100),
                    @Password NVARCHAR(50)
                AS
                BEGIN
                    INSERT INTO Users (Username, Email, Password)
                    VALUES (@Username, @Email, @Password)
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS AddUser");
        }

    }
}
