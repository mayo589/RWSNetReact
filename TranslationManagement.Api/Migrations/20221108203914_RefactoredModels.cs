using Microsoft.EntityFrameworkCore.Migrations;

namespace TranslationManagement.Api.Migrations
{
    public partial class RefactoredModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Translators",
                type: "TEXT",
                nullable: false,
                defaultValue: "Applicant",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Translators",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "HourlyRate",
                table: "Translators",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TranslationJobs",
                type: "TEXT",
                nullable: false,
                defaultValue: "New",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OriginalContent",
                table: "TranslationJobs",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                table: "TranslationJobs",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TranslatorId",
                table: "TranslationJobs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TranslationJobs_TranslatorId",
                table: "TranslationJobs",
                column: "TranslatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_TranslationJobs_Translators_TranslatorId",
                table: "TranslationJobs",
                column: "TranslatorId",
                principalTable: "Translators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TranslationJobs_Translators_TranslatorId",
                table: "TranslationJobs");

            migrationBuilder.DropIndex(
                name: "IX_TranslationJobs_TranslatorId",
                table: "TranslationJobs");

            migrationBuilder.DropColumn(
                name: "TranslatorId",
                table: "TranslationJobs");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Translators",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "Applicant");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Translators",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "HourlyRate",
                table: "Translators",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TranslationJobs",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "New");

            migrationBuilder.AlterColumn<string>(
                name: "OriginalContent",
                table: "TranslationJobs",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                table: "TranslationJobs",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
