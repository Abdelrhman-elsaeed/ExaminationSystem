using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExaminationSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedingRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles", 
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" }, 
                values: new object[]
                {
                    Guid.NewGuid().ToString(),
                    Role.Admin.ToString(), 
                    Role.Admin.ToString().ToUpper(),
                    Guid.NewGuid().ToString()
                }
            );

            
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[]
                {
                    Guid.NewGuid().ToString(),
                    Role.Instructor.ToString(), 
                    Role.Instructor.ToString().ToUpper(),
                    Guid.NewGuid().ToString()
                }
            );

            
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[]
                {
                    Guid.NewGuid().ToString(),
                    Role.Student.ToString(), 
                    Role.Student.ToString().ToUpper(),
                    Guid.NewGuid().ToString()
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("DELETE FROM AspNetRoles");

        }
    }
}
