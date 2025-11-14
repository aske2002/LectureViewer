using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedAdditionalModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Question = table.Column<string>(type: "text", nullable: false),
                    Answer = table.Column<string>(type: "text", nullable: false),
                    LectureId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseCategories_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LectureContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LectureId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentType = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LectureContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LectureContents_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    LectureContentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_LectureContents_LectureContentId",
                        column: x => x.LectureContentId,
                        principalTable: "LectureContents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Flashcards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ArtifactFromJobId = table.Column<Guid>(type: "uuid", nullable: true),
                    FlashcardType = table.Column<int>(type: "integer", nullable: false),
                    Question = table.Column<string>(type: "text", nullable: false),
                    LectureId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CorrectAnswer = table.Column<bool>(type: "boolean", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flashcards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flashcards_CourseCategories_ContentCategoryId",
                        column: x => x.ContentCategoryId,
                        principalTable: "CourseCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flashcards_Jobs_ArtifactFromJobId",
                        column: x => x.ArtifactFromJobId,
                        principalTable: "Jobs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Flashcards_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    LoggedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobLogs_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LectureTranscripts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LectureId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ArtifactFromJobId = table.Column<Guid>(type: "uuid", nullable: true),
                    Summary = table.Column<string>(type: "text", nullable: false),
                    TranscriptText = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LectureTranscripts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LectureTranscripts_Jobs_ArtifactFromJobId",
                        column: x => x.ArtifactFromJobId,
                        principalTable: "Jobs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LectureTranscripts_LectureContents_SourceId",
                        column: x => x.SourceId,
                        principalTable: "LectureContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LectureTranscripts_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlashCardChoiceAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    MultipleChoicecardId = table.Column<Guid>(type: "uuid", nullable: true),
                    CorrectAnswerForMultipleChoicecardId = table.Column<Guid>(type: "uuid", nullable: true),
                    CorrectAnswerForSingleChoicecardId = table.Column<Guid>(type: "uuid", nullable: true),
                    SingleChoicecardId = table.Column<Guid>(type: "uuid", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashCardChoiceAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashCardChoiceAnswers_Flashcards_CorrectAnswerForMultipleC~",
                        column: x => x.CorrectAnswerForMultipleChoicecardId,
                        principalTable: "Flashcards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FlashCardChoiceAnswers_Flashcards_CorrectAnswerForSingleCho~",
                        column: x => x.CorrectAnswerForSingleChoicecardId,
                        principalTable: "Flashcards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FlashCardChoiceAnswers_Flashcards_MultipleChoicecardId",
                        column: x => x.MultipleChoicecardId,
                        principalTable: "Flashcards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FlashCardChoiceAnswers_Flashcards_SingleChoicecardId",
                        column: x => x.SingleChoicecardId,
                        principalTable: "Flashcards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LectureTranscriptKeywords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LectureTranscriptId = table.Column<Guid>(type: "uuid", nullable: false),
                    Keyword = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LectureTranscriptKeywords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LectureTranscriptKeywords_LectureTranscripts_LectureTranscr~",
                        column: x => x.LectureTranscriptId,
                        principalTable: "LectureTranscripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LectureTranscriptTimestamps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LectureTranscriptId = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LectureTranscriptTimestamps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LectureTranscriptTimestamps_LectureTranscripts_LectureTrans~",
                        column: x => x.LectureTranscriptId,
                        principalTable: "LectureTranscripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlashCardChoiceAnswerOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FlashcardType = table.Column<int>(type: "integer", nullable: false),
                    FlashcardId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseEnrollmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AnswerText = table.Column<string>(type: "text", nullable: true),
                    SelectedChoiceId = table.Column<Guid>(type: "uuid", nullable: true),
                    AnswerBool = table.Column<bool>(type: "boolean", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashCardChoiceAnswerOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashCardChoiceAnswerOptions_CourseEnrollments_CourseEnroll~",
                        column: x => x.CourseEnrollmentId,
                        principalTable: "CourseEnrollments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlashCardChoiceAnswerOptions_FlashCardChoiceAnswers_Selecte~",
                        column: x => x.SelectedChoiceId,
                        principalTable: "FlashCardChoiceAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlashCardChoiceAnswerOptions_Flashcards_FlashcardId",
                        column: x => x.FlashcardId,
                        principalTable: "Flashcards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlashCardPairAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KeyChoiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ValueChoiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    FlashcardId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashCardPairAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashCardPairAnswers_FlashCardChoiceAnswers_KeyChoiceId",
                        column: x => x.KeyChoiceId,
                        principalTable: "FlashCardChoiceAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlashCardPairAnswers_FlashCardChoiceAnswers_ValueChoiceId",
                        column: x => x.ValueChoiceId,
                        principalTable: "FlashCardChoiceAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlashCardPairAnswers_Flashcards_FlashcardId",
                        column: x => x.FlashcardId,
                        principalTable: "Flashcards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LectureTranscriptKeywordOccurrences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LectureTranscriptKeywordId = table.Column<Guid>(type: "uuid", nullable: false),
                    OccurrenceTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    SurroundingText = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LectureTranscriptKeywordOccurrences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LectureTranscriptKeywordOccurrences_LectureTranscriptKeywor~",
                        column: x => x.LectureTranscriptKeywordId,
                        principalTable: "LectureTranscriptKeywords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlashcardChoiceMultipleChoiceFlashcardAnswer",
                columns: table => new
                {
                    AnswersForMultipleChoicecardsId = table.Column<Guid>(type: "uuid", nullable: false),
                    SelectedChoicesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardChoiceMultipleChoiceFlashcardAnswer", x => new { x.AnswersForMultipleChoicecardsId, x.SelectedChoicesId });
                    table.ForeignKey(
                        name: "FK_FlashcardChoiceMultipleChoiceFlashcardAnswer_FlashCardChoic~",
                        column: x => x.AnswersForMultipleChoicecardsId,
                        principalTable: "FlashCardChoiceAnswerOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlashcardChoiceMultipleChoiceFlashcardAnswer_FlashCardChoi~1",
                        column: x => x.SelectedChoicesId,
                        principalTable: "FlashCardChoiceAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlashcardPairAnswer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SelectedKeyId = table.Column<Guid>(type: "uuid", nullable: false),
                    SelectedValueId = table.Column<Guid>(type: "uuid", nullable: false),
                    FlashcardAnswerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardPairAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashcardPairAnswer_FlashCardChoiceAnswerOptions_FlashcardA~",
                        column: x => x.FlashcardAnswerId,
                        principalTable: "FlashCardChoiceAnswerOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlashcardPairAnswer_FlashCardChoiceAnswers_SelectedKeyId",
                        column: x => x.SelectedKeyId,
                        principalTable: "FlashCardChoiceAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlashcardPairAnswer_FlashCardChoiceAnswers_SelectedValueId",
                        column: x => x.SelectedValueId,
                        principalTable: "FlashCardChoiceAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseCategories_LectureId",
                table: "CourseCategories",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCardChoiceAnswerOptions_CourseEnrollmentId",
                table: "FlashCardChoiceAnswerOptions",
                column: "CourseEnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCardChoiceAnswerOptions_FlashcardId",
                table: "FlashCardChoiceAnswerOptions",
                column: "FlashcardId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCardChoiceAnswerOptions_SelectedChoiceId",
                table: "FlashCardChoiceAnswerOptions",
                column: "SelectedChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCardChoiceAnswers_CorrectAnswerForMultipleChoicecardId",
                table: "FlashCardChoiceAnswers",
                column: "CorrectAnswerForMultipleChoicecardId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCardChoiceAnswers_CorrectAnswerForSingleChoicecardId",
                table: "FlashCardChoiceAnswers",
                column: "CorrectAnswerForSingleChoicecardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlashCardChoiceAnswers_MultipleChoicecardId",
                table: "FlashCardChoiceAnswers",
                column: "MultipleChoicecardId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCardChoiceAnswers_SingleChoicecardId",
                table: "FlashCardChoiceAnswers",
                column: "SingleChoicecardId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardChoiceMultipleChoiceFlashcardAnswer_SelectedChoice~",
                table: "FlashcardChoiceMultipleChoiceFlashcardAnswer",
                column: "SelectedChoicesId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardPairAnswer_FlashcardAnswerId",
                table: "FlashcardPairAnswer",
                column: "FlashcardAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardPairAnswer_SelectedKeyId",
                table: "FlashcardPairAnswer",
                column: "SelectedKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardPairAnswer_SelectedValueId",
                table: "FlashcardPairAnswer",
                column: "SelectedValueId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCardPairAnswers_FlashcardId",
                table: "FlashCardPairAnswers",
                column: "FlashcardId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCardPairAnswers_KeyChoiceId",
                table: "FlashCardPairAnswers",
                column: "KeyChoiceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlashCardPairAnswers_ValueChoiceId",
                table: "FlashCardPairAnswers",
                column: "ValueChoiceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_ArtifactFromJobId",
                table: "Flashcards",
                column: "ArtifactFromJobId");

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_ContentCategoryId",
                table: "Flashcards",
                column: "ContentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_LectureId",
                table: "Flashcards",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_JobLogs_JobId",
                table: "JobLogs",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_LectureContentId",
                table: "Jobs",
                column: "LectureContentId");

            migrationBuilder.CreateIndex(
                name: "IX_LectureContents_LectureId",
                table: "LectureContents",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_LectureTranscriptKeywordOccurrences_LectureTranscriptKeywor~",
                table: "LectureTranscriptKeywordOccurrences",
                column: "LectureTranscriptKeywordId");

            migrationBuilder.CreateIndex(
                name: "IX_LectureTranscriptKeywords_LectureTranscriptId",
                table: "LectureTranscriptKeywords",
                column: "LectureTranscriptId");

            migrationBuilder.CreateIndex(
                name: "IX_LectureTranscripts_ArtifactFromJobId",
                table: "LectureTranscripts",
                column: "ArtifactFromJobId");

            migrationBuilder.CreateIndex(
                name: "IX_LectureTranscripts_LectureId",
                table: "LectureTranscripts",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_LectureTranscripts_SourceId",
                table: "LectureTranscripts",
                column: "SourceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LectureTranscriptTimestamps_LectureTranscriptId",
                table: "LectureTranscriptTimestamps",
                column: "LectureTranscriptId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlashcardChoiceMultipleChoiceFlashcardAnswer");

            migrationBuilder.DropTable(
                name: "FlashcardPairAnswer");

            migrationBuilder.DropTable(
                name: "FlashCardPairAnswers");

            migrationBuilder.DropTable(
                name: "JobLogs");

            migrationBuilder.DropTable(
                name: "LectureTranscriptKeywordOccurrences");

            migrationBuilder.DropTable(
                name: "LectureTranscriptTimestamps");

            migrationBuilder.DropTable(
                name: "FlashCardChoiceAnswerOptions");

            migrationBuilder.DropTable(
                name: "LectureTranscriptKeywords");

            migrationBuilder.DropTable(
                name: "FlashCardChoiceAnswers");

            migrationBuilder.DropTable(
                name: "LectureTranscripts");

            migrationBuilder.DropTable(
                name: "Flashcards");

            migrationBuilder.DropTable(
                name: "CourseCategories");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "LectureContents");
        }
    }
}
