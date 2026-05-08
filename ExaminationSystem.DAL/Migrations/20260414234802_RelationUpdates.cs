using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExaminationSystem.Migrations
{
    /// <inheritdoc />
    public partial class RelationUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Choices_ChoiceID",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_ChoiceID",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "ChoiceID",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "text",
                table: "Choices",
                newName: "Text");

            migrationBuilder.AddColumn<int>(
                name: "InstructorId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_InstructorId",
                table: "Questions",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Instructors_InstructorId",
                table: "Questions",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Instructors_InstructorId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_InstructorId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Choices",
                newName: "text");

            migrationBuilder.AddColumn<int>(
                name: "ChoiceID",
                table: "Exams",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ChoiceID",
                table: "Exams",
                column: "ChoiceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Choices_ChoiceID",
                table: "Exams",
                column: "ChoiceID",
                principalTable: "Choices",
                principalColumn: "ID");
        }
    }
}
