using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineForms.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SurveyId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    OptionA = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    OptionB = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    OptionC = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    OptionD = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Responses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SurveyId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ParticipantId = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Responses_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResponseAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ResponseId = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuestionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SelectedOption = table.Column<string>(type: "TEXT", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResponseAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResponseAnswers_Responses_ResponseId",
                        column: x => x.ResponseId,
                        principalTable: "Responses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_SurveyId_Order",
                table: "Questions",
                columns: new[] { "SurveyId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_ResponseAnswers_QuestionId",
                table: "ResponseAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponseAnswers_ResponseId",
                table: "ResponseAnswers",
                column: "ResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_Responses_SurveyId_ParticipantId",
                table: "Responses",
                columns: new[] { "SurveyId", "ParticipantId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResponseAnswers");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Responses");

            migrationBuilder.DropTable(
                name: "Surveys");
        }
    }
}
