using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MarksPracticeApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedSampleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "courses",
                columns: new[] { "id", "course_code", "course_name", "created_at_utc", "credit_hours" },
                values: new object[,]
                {
                    { 1, "CS301", "Advanced Programming", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3 },
                    { 2, "CS302", "Database Systems", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3 }
                });

            migrationBuilder.InsertData(
                table: "students",
                columns: new[] { "id", "created_at_utc", "email", "full_name", "registration_number" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ali@example.com", "Ali Ahmed", "FA24-BSAI-001" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "sara@example.com", "Sara Khan", "FA24-BSAI-002" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "courses",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "courses",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "students",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "students",
                keyColumn: "id",
                keyValue: 2);
        }
    }
}
