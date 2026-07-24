using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Inversor.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InboxState",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsumerId = table.Column<Guid>(type: "uuid", nullable: false),
                    LockId = table.Column<Guid>(type: "uuid", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    Received = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReceiveCount = table.Column<int>(type: "integer", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Consumed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Delivered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSequenceNumber = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxState", x => x.Id);
                    table.UniqueConstraint("AK_InboxState_MessageId_ConsumerId", x => new { x.MessageId, x.ConsumerId });
                });

            migrationBuilder.CreateTable(
                name: "MacroTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Tag = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacroTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxState",
                columns: table => new
                {
                    OutboxId = table.Column<Guid>(type: "uuid", nullable: false),
                    LockId = table.Column<Guid>(type: "uuid", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Delivered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSequenceNumber = table.Column<long>(type: "bigint", nullable: true),
                    BusName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxState", x => x.OutboxId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    SubscriptionTier = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GrammarTopics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MacroTagId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tag = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    TheoryDescription = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrammarTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrammarTopics_MacroTags_MacroTagId",
                        column: x => x.MacroTagId,
                        principalTable: "MacroTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessage",
                columns: table => new
                {
                    SequenceNumber = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EnqueueTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SentTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Headers = table.Column<string>(type: "text", nullable: true),
                    Properties = table.Column<string>(type: "text", nullable: true),
                    InboxMessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    InboxConsumerId = table.Column<Guid>(type: "uuid", nullable: true),
                    OutboxId = table.Column<Guid>(type: "uuid", nullable: true),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentType = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    MessageType = table.Column<string>(type: "text", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: true),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: true),
                    InitiatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: true),
                    SourceAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DestinationAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ResponseAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    FaultAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessage", x => x.SequenceNumber);
                    table.ForeignKey(
                        name: "FK_OutboxMessage_InboxState_InboxMessageId_InboxConsumerId",
                        columns: x => new { x.InboxMessageId, x.InboxConsumerId },
                        principalTable: "InboxState",
                        principalColumns: new[] { "MessageId", "ConsumerId" });
                    table.ForeignKey(
                        name: "FK_OutboxMessage_OutboxState_OutboxId",
                        column: x => x.OutboxId,
                        principalTable: "OutboxState",
                        principalColumn: "OutboxId");
                });

            migrationBuilder.CreateTable(
                name: "UserLanguageProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    NativeLanguageCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    LearnLanguageCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    AssessedLevel = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    DailyRequestCount = table.Column<int>(type: "integer", nullable: false),
                    LastRequestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLanguageProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLanguageProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopicMasteries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserLanguageProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    GrammarTopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    MasteryScore = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    TotalAttempts = table.Column<int>(type: "integer", nullable: false),
                    ConsecutiveSuccesses = table.Column<int>(type: "integer", nullable: false),
                    CurrentIntervalDays = table.Column<int>(type: "integer", nullable: false),
                    EasinessFactor = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    NextReviewDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicMasteries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopicMasteries_GrammarTopics_GrammarTopicId",
                        column: x => x.GrammarTopicId,
                        principalTable: "GrammarTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TopicMasteries_UserLanguageProfiles_UserLanguageProfileId",
                        column: x => x.UserLanguageProfileId,
                        principalTable: "UserLanguageProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TranslationSubmissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserLanguageProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Mode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OriginalInput = table.Column<string>(type: "text", nullable: false),
                    CorrectedOutput = table.Column<string>(type: "text", nullable: true),
                    GeneralFeedback = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FailureReason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslationSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TranslationSubmissions_UserLanguageProfiles_UserLanguagePro~",
                        column: x => x.UserLanguageProfileId,
                        principalTable: "UserLanguageProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubmitTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TranslationSubmissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    GrammarTopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsError = table.Column<bool>(type: "boolean", nullable: false),
                    OriginalFragment = table.Column<string>(type: "text", nullable: false),
                    CorrectedFragment = table.Column<string>(type: "text", nullable: false),
                    BriefExplanation = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmitTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubmitTags_GrammarTopics_GrammarTopicId",
                        column: x => x.GrammarTopicId,
                        principalTable: "GrammarTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubmitTags_TranslationSubmissions_TranslationSubmissionId",
                        column: x => x.TranslationSubmissionId,
                        principalTable: "TranslationSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MacroTags",
                columns: new[] { "Id", "Description", "Tag" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-100000000000"), "Escritura correcta de las palabras.", "ORTHOGRAPHY" },
                    { new Guid("22222222-2222-2222-2222-200000000000"), "Forma y conjugación de las palabras.", "MORPHOLOGY" },
                    { new Guid("33333333-3333-3333-3333-300000000000"), "Orden y estructura de la oración.", "SYNTAX" },
                    { new Guid("44444444-4444-4444-4444-400000000000"), "Vocabulario y significado literal.", "LEXICON" },
                    { new Guid("55555555-5555-5555-5555-500000000000"), "Sentido lógico de la oración.", "SEMANTICS" },
                    { new Guid("66666666-6666-6666-6666-600000000000"), "Uso adecuado según el contexto.", "PRAGMATICS" }
                });

            migrationBuilder.InsertData(
                table: "GrammarTopics",
                columns: new[] { "Id", "MacroTagId", "Tag", "TheoryDescription", "Title" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-100000000000"), "orthography_general_error", "Válvula de escape", "Error ortográfico general" },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("11111111-1111-1111-1111-100000000000"), "orthography_spelling_mistake", "", "Error de deletreo/escritura" },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new Guid("11111111-1111-1111-1111-100000000000"), "orthography_capitalization_missing", "", "Falta mayúscula" },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new Guid("11111111-1111-1111-1111-100000000000"), "orthography_capitalization_unnecessary", "", "Mayúscula innecesaria" },
                    { new Guid("10000000-0000-0000-0000-000000000005"), new Guid("11111111-1111-1111-1111-100000000000"), "orthography_punctuation_missing", "", "Signo de puntuación faltante" },
                    { new Guid("10000000-0000-0000-0000-000000000006"), new Guid("11111111-1111-1111-1111-100000000000"), "orthography_punctuation_incorrect", "", "Signo de puntuación incorrecto" },
                    { new Guid("10000000-0000-0000-0000-000000000007"), new Guid("11111111-1111-1111-1111-100000000000"), "orthography_accentuation_missing", "", "Falta de acento/tilde" },
                    { new Guid("10000000-0000-0000-0000-000000000008"), new Guid("11111111-1111-1111-1111-100000000000"), "orthography_accentuation_incorrect", "", "Acento/tilde incorrecto" },
                    { new Guid("10000000-0000-0000-0000-000000000009"), new Guid("11111111-1111-1111-1111-100000000000"), "orthography_hyphenation_error", "", "Error en uso de guion" },
                    { new Guid("10000000-0000-0000-0000-000000000010"), new Guid("11111111-1111-1111-1111-100000000000"), "orthography_apostrophe_missing", "", "Apóstrofo faltante" },
                    { new Guid("10000000-0000-0000-0000-000000000011"), new Guid("11111111-1111-1111-1111-100000000000"), "orthography_apostrophe_misused", "", "Apóstrofo mal usado" },
                    { new Guid("10000000-0000-0000-0000-000000000012"), new Guid("11111111-1111-1111-1111-100000000000"), "orthography_abbreviation_error", "", "Error de abreviatura" },
                    { new Guid("10000000-0000-0000-0000-000000000013"), new Guid("11111111-1111-1111-1111-100000000000"), "orthography_contraction_error", "", "Error en contracción" },
                    { new Guid("10000000-0000-0000-0000-000000000014"), new Guid("11111111-1111-1111-1111-100000000000"), "orthography_spacing_error", "", "Error de espaciado" },
                    { new Guid("10000000-0000-0000-0000-000000000015"), new Guid("11111111-1111-1111-1111-100000000000"), "orthography_symbol_misused", "", "Uso incorrecto de símbolo" },
                    { new Guid("20000000-0000-0000-0000-000000000001"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_general_error", "Válvula de escape", "Error morfológico general" },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_verb_conjugation", "", "Conjugación verbal incorrecta" },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_verb_tense_wrong", "", "Tiempo verbal incorrecto" },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_verb_infinitive_needed", "", "Se requiere verbo en infinitivo" },
                    { new Guid("20000000-0000-0000-0000-000000000005"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_verb_gerund_needed", "", "Se requiere gerundio/participio presente" },
                    { new Guid("20000000-0000-0000-0000-000000000006"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_verb_participle_needed", "", "Se requiere participio pasado" },
                    { new Guid("20000000-0000-0000-0000-000000000007"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_verb_irregular_error", "", "Error en verbo irregular" },
                    { new Guid("20000000-0000-0000-0000-000000000008"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_noun_pluralization", "", "Error de pluralización" },
                    { new Guid("20000000-0000-0000-0000-000000000009"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_noun_gender_agreement", "", "Error de concordancia de género" },
                    { new Guid("20000000-0000-0000-0000-000000000010"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_noun_number_agreement", "", "Error de concordancia de número" },
                    { new Guid("20000000-0000-0000-0000-000000000011"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_adjective_agreement", "", "Concordancia de adjetivo incorrecta" },
                    { new Guid("20000000-0000-0000-0000-000000000012"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_adjective_comparative", "", "Error en forma comparativa" },
                    { new Guid("20000000-0000-0000-0000-000000000013"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_adjective_superlative", "", "Error en forma superlativa" },
                    { new Guid("20000000-0000-0000-0000-000000000014"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_adverb_formation", "", "Formación incorrecta del adverbio" },
                    { new Guid("20000000-0000-0000-0000-000000000015"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_pronoun_case_error", "", "Error de caso en pronombre (ej. me vs I)" },
                    { new Guid("20000000-0000-0000-0000-000000000016"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_pronoun_reflexive_error", "", "Error en pronombre reflexivo" },
                    { new Guid("20000000-0000-0000-0000-000000000017"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_pronoun_possessive_error", "", "Error en pronombre posesivo" },
                    { new Guid("20000000-0000-0000-0000-000000000018"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_article_definite_missing", "", "Falta artículo definido" },
                    { new Guid("20000000-0000-0000-0000-000000000019"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_article_indefinite_missing", "", "Falta artículo indefinido" },
                    { new Guid("20000000-0000-0000-0000-000000000020"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_article_unnecessary", "", "Uso innecesario de artículo" },
                    { new Guid("20000000-0000-0000-0000-000000000021"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_preposition_wrong_form", "", "Forma incorrecta de preposición" },
                    { new Guid("20000000-0000-0000-0000-000000000022"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_prefix_error", "", "Error en prefijo" },
                    { new Guid("20000000-0000-0000-0000-000000000023"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_suffix_error", "", "Error en sufijo" },
                    { new Guid("20000000-0000-0000-0000-000000000024"), new Guid("22222222-2222-2222-2222-200000000000"), "morphology_word_class_confusion", "", "Confusión de categoría gramatical (ej. usar adjetivo como verbo)" },
                    { new Guid("30000000-0000-0000-0000-000000000001"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_general_error", "Válvula de escape", "Error sintáctico general" },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_word_order_subject_verb", "", "Orden incorrecto de sujeto y verbo" },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_word_order_adjective_noun", "", "Orden incorrecto de adjetivo y sustantivo" },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_word_order_adverb", "", "Posición incorrecta del adverbio" },
                    { new Guid("30000000-0000-0000-0000-000000000005"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_word_order_question", "", "Estructura incorrecta de pregunta" },
                    { new Guid("30000000-0000-0000-0000-000000000006"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_word_order_negation", "", "Estructura incorrecta de negación" },
                    { new Guid("30000000-0000-0000-0000-000000000007"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_subject_missing", "", "Sujeto omitido/faltante" },
                    { new Guid("30000000-0000-0000-0000-000000000008"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_subject_unnecessary", "", "Sujeto redundante" },
                    { new Guid("30000000-0000-0000-0000-000000000009"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_verb_missing", "", "Verbo principal faltante" },
                    { new Guid("30000000-0000-0000-0000-000000000010"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_auxiliary_verb_missing", "", "Verbo auxiliar faltante" },
                    { new Guid("30000000-0000-0000-0000-000000000011"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_auxiliary_verb_unnecessary", "", "Verbo auxiliar innecesario" },
                    { new Guid("30000000-0000-0000-0000-000000000012"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_object_missing", "", "Objeto directo/indirecto faltante" },
                    { new Guid("30000000-0000-0000-0000-000000000013"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_object_position", "", "Posición incorrecta del objeto" },
                    { new Guid("30000000-0000-0000-0000-000000000014"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_preposition_missing", "", "Preposición faltante" },
                    { new Guid("30000000-0000-0000-0000-000000000015"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_preposition_unnecessary", "", "Preposición innecesaria" },
                    { new Guid("30000000-0000-0000-0000-000000000016"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_preposition_stranded", "", "Preposición mal posicionada al final" },
                    { new Guid("30000000-0000-0000-0000-000000000017"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_conjunction_missing", "", "Conjunción faltante" },
                    { new Guid("30000000-0000-0000-0000-000000000018"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_conjunction_misused", "", "Conjunción mal utilizada" },
                    { new Guid("30000000-0000-0000-0000-000000000019"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_clause_dependent_error", "", "Error en cláusula subordinada/dependiente" },
                    { new Guid("30000000-0000-0000-0000-000000000020"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_clause_relative_error", "", "Error en cláusula relativa" },
                    { new Guid("30000000-0000-0000-0000-000000000021"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_conditional_structure", "", "Estructura de condicional incorrecta" },
                    { new Guid("30000000-0000-0000-0000-000000000022"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_passive_voice_error", "", "Error en estructura de voz pasiva" },
                    { new Guid("30000000-0000-0000-0000-000000000023"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_reported_speech_error", "", "Error en estilo indirecto" },
                    { new Guid("30000000-0000-0000-0000-000000000024"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_double_negative", "", "Doble negación no permitida" },
                    { new Guid("30000000-0000-0000-0000-000000000025"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_parallelism_error", "", "Falta de paralelismo estructural" },
                    { new Guid("30000000-0000-0000-0000-000000000026"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_run_on_sentence", "", "Oración continua sin separación (Run-on)" },
                    { new Guid("30000000-0000-0000-0000-000000000027"), new Guid("33333333-3333-3333-3333-300000000000"), "syntax_fragment_sentence", "", "Oración fragmentada/incompleta" },
                    { new Guid("40000000-0000-0000-0000-000000000001"), new Guid("44444444-4444-4444-4444-400000000000"), "lexicon_general_error", "Válvula de escape", "Error léxico general" },
                    { new Guid("40000000-0000-0000-0000-000000000002"), new Guid("44444444-4444-4444-4444-400000000000"), "lexicon_vocabulary_wrong_word", "", "Palabra incorrecta para el contexto" },
                    { new Guid("40000000-0000-0000-0000-000000000003"), new Guid("44444444-4444-4444-4444-400000000000"), "lexicon_vocabulary_false_friend", "", "Falso amigo (interferencia de idioma nativo)" },
                    { new Guid("40000000-0000-0000-0000-000000000004"), new Guid("44444444-4444-4444-4444-400000000000"), "lexicon_vocabulary_invented_word", "", "Palabra inexistente/inventada" },
                    { new Guid("40000000-0000-0000-0000-000000000005"), new Guid("44444444-4444-4444-4444-400000000000"), "lexicon_verb_collocation", "", "Colocación verbal incorrecta (ej. make vs do)" },
                    { new Guid("40000000-0000-0000-0000-000000000006"), new Guid("44444444-4444-4444-4444-400000000000"), "lexicon_noun_collocation", "", "Colocación de sustantivo incorrecta" },
                    { new Guid("40000000-0000-0000-0000-000000000007"), new Guid("44444444-4444-4444-4444-400000000000"), "lexicon_adjective_collocation", "", "Colocación de adjetivo incorrecta" },
                    { new Guid("40000000-0000-0000-0000-000000000008"), new Guid("44444444-4444-4444-4444-400000000000"), "lexicon_preposition_choice", "", "Elección incorrecta de preposición" },
                    { new Guid("40000000-0000-0000-0000-000000000009"), new Guid("44444444-4444-4444-4444-400000000000"), "lexicon_phrasal_verb_choice", "", "Verbo preposicional/compuesto incorrecto" },
                    { new Guid("40000000-0000-0000-0000-000000000010"), new Guid("44444444-4444-4444-4444-400000000000"), "lexicon_idiom_misused", "", "Uso incorrecto de modismo/frase hecha" },
                    { new Guid("40000000-0000-0000-0000-000000000011"), new Guid("44444444-4444-4444-4444-400000000000"), "lexicon_idiom_literal_translation", "", "Traducción literal de modismo" },
                    { new Guid("40000000-0000-0000-0000-000000000012"), new Guid("44444444-4444-4444-4444-400000000000"), "lexicon_register_too_formal", "", "Vocabulario demasiado formal" },
                    { new Guid("40000000-0000-0000-0000-000000000013"), new Guid("44444444-4444-4444-4444-400000000000"), "lexicon_register_too_informal", "", "Vocabulario demasiado informal" },
                    { new Guid("40000000-0000-0000-0000-000000000014"), new Guid("44444444-4444-4444-4444-400000000000"), "lexicon_slang_misused", "", "Uso incorrecto de jerga/slang" },
                    { new Guid("40000000-0000-0000-0000-000000000015"), new Guid("44444444-4444-4444-4444-400000000000"), "lexicon_repetition_redundancy", "", "Repetición innecesaria de vocabulario" },
                    { new Guid("40000000-0000-0000-0000-000000000016"), new Guid("44444444-4444-4444-4444-400000000000"), "lexicon_transition_word_choice", "", "Conector lógico mal seleccionado" },
                    { new Guid("40000000-0000-0000-0000-000000000017"), new Guid("44444444-4444-4444-4444-400000000000"), "lexicon_quantifier_choice", "", "Cuantificador incorrecto (ej. much vs many)" },
                    { new Guid("40000000-0000-0000-0000-000000000018"), new Guid("44444444-4444-4444-4444-400000000000"), "lexicon_determiner_choice", "", "Determinante incorrecto (ej. this vs that)" },
                    { new Guid("50000000-0000-0000-0000-000000000001"), new Guid("55555555-5555-5555-5555-500000000000"), "semantics_general_error", "Válvula de escape", "Error semántico general" },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("55555555-5555-5555-5555-500000000000"), "semantics_ambiguous_meaning", "", "Significado ambiguo o poco claro" },
                    { new Guid("50000000-0000-0000-0000-000000000003"), new Guid("55555555-5555-5555-5555-500000000000"), "semantics_illogical_statement", "", "Declaración ilógica o contradictoria" },
                    { new Guid("50000000-0000-0000-0000-000000000004"), new Guid("55555555-5555-5555-5555-500000000000"), "semantics_literal_translation_error", "", "Traducción literal que pierde sentido" },
                    { new Guid("50000000-0000-0000-0000-000000000005"), new Guid("55555555-5555-5555-5555-500000000000"), "semantics_redundancy", "", "Redundancia semántica (pleonasmo)" },
                    { new Guid("50000000-0000-0000-0000-000000000006"), new Guid("55555555-5555-5555-5555-500000000000"), "semantics_nuance_lost", "", "Pérdida de matiz o tono sutil" },
                    { new Guid("50000000-0000-0000-0000-000000000007"), new Guid("55555555-5555-5555-5555-500000000000"), "semantics_scope_ambiguity", "", "Ambigüedad de alcance (ej. negación)" },
                    { new Guid("50000000-0000-0000-0000-000000000008"), new Guid("55555555-5555-5555-5555-500000000000"), "semantics_idiomatic_meaning_lost", "", "Pérdida de significado idiomático" },
                    { new Guid("50000000-0000-0000-0000-000000000009"), new Guid("55555555-5555-5555-5555-500000000000"), "semantics_reference_unclear", "", "Referente de pronombre poco claro" },
                    { new Guid("60000000-0000-0000-0000-000000000001"), new Guid("66666666-6666-6666-6666-600000000000"), "pragmatics_general_error", "Válvula de escape", "Error pragmático general" },
                    { new Guid("60000000-0000-0000-0000-000000000002"), new Guid("66666666-6666-6666-6666-600000000000"), "pragmatics_impolite_tone", "", "Tono descortés o muy directo" },
                    { new Guid("60000000-0000-0000-0000-000000000003"), new Guid("66666666-6666-6666-6666-600000000000"), "pragmatics_overly_polite", "", "Tono excesivamente cortés para el contexto" },
                    { new Guid("60000000-0000-0000-0000-000000000004"), new Guid("66666666-6666-6666-6666-600000000000"), "pragmatics_inappropriate_register", "", "Registro inapropiado para la situación social" },
                    { new Guid("60000000-0000-0000-0000-000000000005"), new Guid("66666666-6666-6666-6666-600000000000"), "pragmatics_cultural_misunderstanding", "", "Malentendido o tabú cultural" },
                    { new Guid("60000000-0000-0000-0000-000000000006"), new Guid("66666666-6666-6666-6666-600000000000"), "pragmatics_indirectness_missing", "", "Falta de tacto/mitigación en una petición" },
                    { new Guid("60000000-0000-0000-0000-000000000007"), new Guid("66666666-6666-6666-6666-600000000000"), "pragmatics_response_unnatural", "", "Respuesta gramatical pero antinatural" },
                    { new Guid("60000000-0000-0000-0000-000000000008"), new Guid("66666666-6666-6666-6666-600000000000"), "pragmatics_greeting_inappropriate", "", "Saludo inapropiado" },
                    { new Guid("60000000-0000-0000-0000-000000000009"), new Guid("66666666-6666-6666-6666-600000000000"), "pragmatics_closing_inappropriate", "", "Despedida inapropiada" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrammarTopics_MacroTagId",
                table: "GrammarTopics",
                column: "MacroTagId");

            migrationBuilder.CreateIndex(
                name: "IX_GrammarTopics_Tag",
                table: "GrammarTopics",
                column: "Tag",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InboxState_Delivered",
                table: "InboxState",
                column: "Delivered");

            migrationBuilder.CreateIndex(
                name: "IX_MacroTags_Tag",
                table: "MacroTags",
                column: "Tag",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_EnqueueTime",
                table: "OutboxMessage",
                column: "EnqueueTime");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_ExpirationTime",
                table: "OutboxMessage",
                column: "ExpirationTime");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_InboxMessageId_InboxConsumerId_SequenceNumber",
                table: "OutboxMessage",
                columns: new[] { "InboxMessageId", "InboxConsumerId", "SequenceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_OutboxId_SequenceNumber",
                table: "OutboxMessage",
                columns: new[] { "OutboxId", "SequenceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutboxState_BusName_Created",
                table: "OutboxState",
                columns: new[] { "BusName", "Created" });

            migrationBuilder.CreateIndex(
                name: "IX_OutboxState_Created",
                table: "OutboxState",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_SubmitTags_GrammarTopicId",
                table: "SubmitTags",
                column: "GrammarTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmitTags_TranslationSubmissionId",
                table: "SubmitTags",
                column: "TranslationSubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicMasteries_GrammarTopicId",
                table: "TopicMasteries",
                column: "GrammarTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicMasteries_UserLanguageProfileId",
                table: "TopicMasteries",
                column: "UserLanguageProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_TranslationSubmissions_UserLanguageProfileId",
                table: "TranslationSubmissions",
                column: "UserLanguageProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguageProfiles_UserId",
                table: "UserLanguageProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutboxMessage");

            migrationBuilder.DropTable(
                name: "SubmitTags");

            migrationBuilder.DropTable(
                name: "TopicMasteries");

            migrationBuilder.DropTable(
                name: "InboxState");

            migrationBuilder.DropTable(
                name: "OutboxState");

            migrationBuilder.DropTable(
                name: "TranslationSubmissions");

            migrationBuilder.DropTable(
                name: "GrammarTopics");

            migrationBuilder.DropTable(
                name: "UserLanguageProfiles");

            migrationBuilder.DropTable(
                name: "MacroTags");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
