using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LexoraED.Migrations
{
    /// <inheritdoc />
    public partial class LexoraEDUpgradeV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "QuestionText",
                table: "QuizItems",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "CorrectAnswer",
                table: "QuizItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1);

            migrationBuilder.AddColumn<string>(
                name: "Explanation",
                table: "QuizItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuestionType",
                table: "QuizItems",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CurrentStreak",
                table: "LearningProgresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExperiencePoints",
                table: "LearningProgresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActivityDate",
                table: "LearningProgresses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecommendedModuleId",
                table: "LearningProgresses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrerequisiteModuleId",
                table: "LearningModules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "LearningModules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Learners",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Learners",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AchievementBadges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconClass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PointsAwarded = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementBadges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LearnerId = table.Column<int>(type: "int", nullable: false),
                    ActivityType = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoggedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLogs_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModuleProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LearnerId = table.Column<int>(type: "int", nullable: false),
                    LearningModuleId = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    BestScore = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastAccessedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleProgresses_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModuleProgresses_LearningModules_LearningModuleId",
                        column: x => x.LearningModuleId,
                        principalTable: "LearningModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LearnerId = table.Column<int>(type: "int", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherProfiles_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LearnerAchievements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LearnerId = table.Column<int>(type: "int", nullable: false),
                    AchievementBadgeId = table.Column<int>(type: "int", nullable: false),
                    EarnedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearnerAchievements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearnerAchievements_AchievementBadges_AchievementBadgeId",
                        column: x => x.AchievementBadgeId,
                        principalTable: "AchievementBadges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearnerAchievements_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LearningModules_PrerequisiteModuleId",
                table: "LearningModules",
                column: "PrerequisiteModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_AchievementBadges_Code",
                table: "AchievementBadges",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_LearnerId",
                table: "ActivityLogs",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_LearnerAchievements_AchievementBadgeId",
                table: "LearnerAchievements",
                column: "AchievementBadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_LearnerAchievements_LearnerId_AchievementBadgeId",
                table: "LearnerAchievements",
                columns: new[] { "LearnerId", "AchievementBadgeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModuleProgresses_LearnerId_LearningModuleId",
                table: "ModuleProgresses",
                columns: new[] { "LearnerId", "LearningModuleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModuleProgresses_LearningModuleId",
                table: "ModuleProgresses",
                column: "LearningModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherProfiles_LearnerId",
                table: "TeacherProfiles",
                column: "LearnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LearningModules_LearningModules_PrerequisiteModuleId",
                table: "LearningModules",
                column: "PrerequisiteModuleId",
                principalTable: "LearningModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningModules_LearningModules_PrerequisiteModuleId",
                table: "LearningModules");

            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "LearnerAchievements");

            migrationBuilder.DropTable(
                name: "ModuleProgresses");

            migrationBuilder.DropTable(
                name: "TeacherProfiles");

            migrationBuilder.DropTable(
                name: "AchievementBadges");

            migrationBuilder.DropIndex(
                name: "IX_LearningModules_PrerequisiteModuleId",
                table: "LearningModules");

            migrationBuilder.DropColumn(
                name: "Explanation",
                table: "QuizItems");

            migrationBuilder.DropColumn(
                name: "QuestionType",
                table: "QuizItems");

            migrationBuilder.DropColumn(
                name: "CurrentStreak",
                table: "LearningProgresses");

            migrationBuilder.DropColumn(
                name: "ExperiencePoints",
                table: "LearningProgresses");

            migrationBuilder.DropColumn(
                name: "LastActivityDate",
                table: "LearningProgresses");

            migrationBuilder.DropColumn(
                name: "RecommendedModuleId",
                table: "LearningProgresses");

            migrationBuilder.DropColumn(
                name: "PrerequisiteModuleId",
                table: "LearningModules");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "LearningModules");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Learners");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Learners");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionText",
                table: "QuizItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "CorrectAnswer",
                table: "QuizItems",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }
    }
}
