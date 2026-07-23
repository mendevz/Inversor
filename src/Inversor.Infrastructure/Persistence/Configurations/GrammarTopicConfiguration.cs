
using Inversor.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inversor.Infrastructure.Persistence.Configurations;

public class GrammarTopicConfiguration : IEntityTypeConfiguration<GrammarTopic>
{
    public void Configure(EntityTypeBuilder<GrammarTopic> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Tag).HasMaxLength(100).IsRequired();
        builder.HasIndex(g => g.Tag).IsUnique();

        builder.Property(g => g.Title).HasMaxLength(150);

        builder.HasOne(g => g.MacroTag)
               .WithMany(m => m.GrammarTopics)
               .HasForeignKey(g => g.MacroTagId)
               .OnDelete(DeleteBehavior.Restrict);

        var orthoId = Guid.Parse("11111111-1111-1111-1111-100000000000");
        var morphId = Guid.Parse("22222222-2222-2222-2222-200000000000");
        var syntaId = Guid.Parse("33333333-3333-3333-3333-300000000000");
        var lexicId = Guid.Parse("44444444-4444-4444-4444-400000000000");
        var semanId = Guid.Parse("55555555-5555-5555-5555-500000000000");
        var pragmaId = Guid.Parse("66666666-6666-6666-6666-600000000000");

        var topics = new List<GrammarTopic>
        {
            // ==========================================
            // 1. ORTHOGRAPHY (15 Topics) -> Rango 1000...
            // ==========================================
            GrammarTopic.CreateWithId(Guid.Parse("10000000-0000-0000-0000-000000000001"), orthoId, "orthography_general_error", "Error ortográfico general", "Válvula de escape"),
            GrammarTopic.CreateWithId(Guid.Parse("10000000-0000-0000-0000-000000000002"), orthoId, "orthography_spelling_mistake", "Error de deletreo/escritura", ""),
            GrammarTopic.CreateWithId(Guid.Parse("10000000-0000-0000-0000-000000000003"), orthoId, "orthography_capitalization_missing", "Falta mayúscula", ""),
            GrammarTopic.CreateWithId(Guid.Parse("10000000-0000-0000-0000-000000000004"), orthoId, "orthography_capitalization_unnecessary", "Mayúscula innecesaria", ""),
            GrammarTopic.CreateWithId(Guid.Parse("10000000-0000-0000-0000-000000000005"), orthoId, "orthography_punctuation_missing", "Signo de puntuación faltante", ""),
            GrammarTopic.CreateWithId(Guid.Parse("10000000-0000-0000-0000-000000000006"), orthoId, "orthography_punctuation_incorrect", "Signo de puntuación incorrecto", ""),
            GrammarTopic.CreateWithId(Guid.Parse("10000000-0000-0000-0000-000000000007"), orthoId, "orthography_accentuation_missing", "Falta de acento/tilde", ""),
            GrammarTopic.CreateWithId(Guid.Parse("10000000-0000-0000-0000-000000000008"), orthoId, "orthography_accentuation_incorrect", "Acento/tilde incorrecto", ""),
            GrammarTopic.CreateWithId(Guid.Parse("10000000-0000-0000-0000-000000000009"), orthoId, "orthography_hyphenation_error", "Error en uso de guion", ""),
            GrammarTopic.CreateWithId(Guid.Parse("10000000-0000-0000-0000-000000000010"), orthoId, "orthography_apostrophe_missing", "Apóstrofo faltante", ""),
            GrammarTopic.CreateWithId(Guid.Parse("10000000-0000-0000-0000-000000000011"), orthoId, "orthography_apostrophe_misused", "Apóstrofo mal usado", ""),
            GrammarTopic.CreateWithId(Guid.Parse("10000000-0000-0000-0000-000000000012"), orthoId, "orthography_abbreviation_error", "Error de abreviatura", ""),
            GrammarTopic.CreateWithId(Guid.Parse("10000000-0000-0000-0000-000000000013"), orthoId, "orthography_contraction_error", "Error en contracción", ""),
            GrammarTopic.CreateWithId(Guid.Parse("10000000-0000-0000-0000-000000000014"), orthoId, "orthography_spacing_error", "Error de espaciado", ""),
            GrammarTopic.CreateWithId(Guid.Parse("10000000-0000-0000-0000-000000000015"), orthoId, "orthography_symbol_misused", "Uso incorrecto de símbolo", ""),

            // ==========================================
            // 2. MORPHOLOGY (24 Topics) -> Rango 2000...
            // ==========================================
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000001"), morphId, "morphology_general_error", "Error morfológico general", "Válvula de escape"),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000002"), morphId, "morphology_verb_conjugation", "Conjugación verbal incorrecta", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000003"), morphId, "morphology_verb_tense_wrong", "Tiempo verbal incorrecto", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000004"), morphId, "morphology_verb_infinitive_needed", "Se requiere verbo en infinitivo", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000005"), morphId, "morphology_verb_gerund_needed", "Se requiere gerundio/participio presente", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000006"), morphId, "morphology_verb_participle_needed", "Se requiere participio pasado", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000007"), morphId, "morphology_verb_irregular_error", "Error en verbo irregular", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000008"), morphId, "morphology_noun_pluralization", "Error de pluralización", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000009"), morphId, "morphology_noun_gender_agreement", "Error de concordancia de género", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000010"), morphId, "morphology_noun_number_agreement", "Error de concordancia de número", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000011"), morphId, "morphology_adjective_agreement", "Concordancia de adjetivo incorrecta", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000012"), morphId, "morphology_adjective_comparative", "Error en forma comparativa", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000013"), morphId, "morphology_adjective_superlative", "Error en forma superlativa", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000014"), morphId, "morphology_adverb_formation", "Formación incorrecta del adverbio", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000015"), morphId, "morphology_pronoun_case_error", "Error de caso en pronombre (ej. me vs I)", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000016"), morphId, "morphology_pronoun_reflexive_error", "Error en pronombre reflexivo", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000017"), morphId, "morphology_pronoun_possessive_error", "Error en pronombre posesivo", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000018"), morphId, "morphology_article_definite_missing", "Falta artículo definido", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000019"), morphId, "morphology_article_indefinite_missing", "Falta artículo indefinido", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000020"), morphId, "morphology_article_unnecessary", "Uso innecesario de artículo", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000021"), morphId, "morphology_preposition_wrong_form", "Forma incorrecta de preposición", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000022"), morphId, "morphology_prefix_error", "Error en prefijo", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000023"), morphId, "morphology_suffix_error", "Error en sufijo", ""),
            GrammarTopic.CreateWithId(Guid.Parse("20000000-0000-0000-0000-000000000024"), morphId, "morphology_word_class_confusion", "Confusión de categoría gramatical (ej. usar adjetivo como verbo)", ""),

            // ==========================================
            // 3. SYNTAX (27 Topics) -> Rango 3000...
            // ==========================================
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000001"), syntaId, "syntax_general_error", "Error sintáctico general", "Válvula de escape"),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000002"), syntaId, "syntax_word_order_subject_verb", "Orden incorrecto de sujeto y verbo", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000003"), syntaId, "syntax_word_order_adjective_noun", "Orden incorrecto de adjetivo y sustantivo", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000004"), syntaId, "syntax_word_order_adverb", "Posición incorrecta del adverbio", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000005"), syntaId, "syntax_word_order_question", "Estructura incorrecta de pregunta", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000006"), syntaId, "syntax_word_order_negation", "Estructura incorrecta de negación", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000007"), syntaId, "syntax_subject_missing", "Sujeto omitido/faltante", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000008"), syntaId, "syntax_subject_unnecessary", "Sujeto redundante", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000009"), syntaId, "syntax_verb_missing", "Verbo principal faltante", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000010"), syntaId, "syntax_auxiliary_verb_missing", "Verbo auxiliar faltante", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000011"), syntaId, "syntax_auxiliary_verb_unnecessary", "Verbo auxiliar innecesario", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000012"), syntaId, "syntax_object_missing", "Objeto directo/indirecto faltante", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000013"), syntaId, "syntax_object_position", "Posición incorrecta del objeto", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000014"), syntaId, "syntax_preposition_missing", "Preposición faltante", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000015"), syntaId, "syntax_preposition_unnecessary", "Preposición innecesaria", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000016"), syntaId, "syntax_preposition_stranded", "Preposición mal posicionada al final", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000017"), syntaId, "syntax_conjunction_missing", "Conjunción faltante", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000018"), syntaId, "syntax_conjunction_misused", "Conjunción mal utilizada", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000019"), syntaId, "syntax_clause_dependent_error", "Error en cláusula subordinada/dependiente", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000020"), syntaId, "syntax_clause_relative_error", "Error en cláusula relativa", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000021"), syntaId, "syntax_conditional_structure", "Estructura de condicional incorrecta", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000022"), syntaId, "syntax_passive_voice_error", "Error en estructura de voz pasiva", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000023"), syntaId, "syntax_reported_speech_error", "Error en estilo indirecto", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000024"), syntaId, "syntax_double_negative", "Doble negación no permitida", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000025"), syntaId, "syntax_parallelism_error", "Falta de paralelismo estructural", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000026"), syntaId, "syntax_run_on_sentence", "Oración continua sin separación (Run-on)", ""),
            GrammarTopic.CreateWithId(Guid.Parse("30000000-0000-0000-0000-000000000027"), syntaId, "syntax_fragment_sentence", "Oración fragmentada/incompleta", ""),

            // ==========================================
            // 4. LEXICON (18 Topics) -> Rango 4000...
            // ==========================================
            GrammarTopic.CreateWithId(Guid.Parse("40000000-0000-0000-0000-000000000001"), lexicId, "lexicon_general_error", "Error léxico general", "Válvula de escape"),
            GrammarTopic.CreateWithId(Guid.Parse("40000000-0000-0000-0000-000000000002"), lexicId, "lexicon_vocabulary_wrong_word", "Palabra incorrecta para el contexto", ""),
            GrammarTopic.CreateWithId(Guid.Parse("40000000-0000-0000-0000-000000000003"), lexicId, "lexicon_vocabulary_false_friend", "Falso amigo (interferencia de idioma nativo)", ""),
            GrammarTopic.CreateWithId(Guid.Parse("40000000-0000-0000-0000-000000000004"), lexicId, "lexicon_vocabulary_invented_word", "Palabra inexistente/inventada", ""),
            GrammarTopic.CreateWithId(Guid.Parse("40000000-0000-0000-0000-000000000005"), lexicId, "lexicon_verb_collocation", "Colocación verbal incorrecta (ej. make vs do)", ""),
            GrammarTopic.CreateWithId(Guid.Parse("40000000-0000-0000-0000-000000000006"), lexicId, "lexicon_noun_collocation", "Colocación de sustantivo incorrecta", ""),
            GrammarTopic.CreateWithId(Guid.Parse("40000000-0000-0000-0000-000000000007"), lexicId, "lexicon_adjective_collocation", "Colocación de adjetivo incorrecta", ""),
            GrammarTopic.CreateWithId(Guid.Parse("40000000-0000-0000-0000-000000000008"), lexicId, "lexicon_preposition_choice", "Elección incorrecta de preposición", ""),
            GrammarTopic.CreateWithId(Guid.Parse("40000000-0000-0000-0000-000000000009"), lexicId, "lexicon_phrasal_verb_choice", "Verbo preposicional/compuesto incorrecto", ""),
            GrammarTopic.CreateWithId(Guid.Parse("40000000-0000-0000-0000-000000000010"), lexicId, "lexicon_idiom_misused", "Uso incorrecto de modismo/frase hecha", ""),
            GrammarTopic.CreateWithId(Guid.Parse("40000000-0000-0000-0000-000000000011"), lexicId, "lexicon_idiom_literal_translation", "Traducción literal de modismo", ""),
            GrammarTopic.CreateWithId(Guid.Parse("40000000-0000-0000-0000-000000000012"), lexicId, "lexicon_register_too_formal", "Vocabulario demasiado formal", ""),
            GrammarTopic.CreateWithId(Guid.Parse("40000000-0000-0000-0000-000000000013"), lexicId, "lexicon_register_too_informal", "Vocabulario demasiado informal", ""),
            GrammarTopic.CreateWithId(Guid.Parse("40000000-0000-0000-0000-000000000014"), lexicId, "lexicon_slang_misused", "Uso incorrecto de jerga/slang", ""),
            GrammarTopic.CreateWithId(Guid.Parse("40000000-0000-0000-0000-000000000015"), lexicId, "lexicon_repetition_redundancy", "Repetición innecesaria de vocabulario", ""),
            GrammarTopic.CreateWithId(Guid.Parse("40000000-0000-0000-0000-000000000016"), lexicId, "lexicon_transition_word_choice", "Conector lógico mal seleccionado", ""),
            GrammarTopic.CreateWithId(Guid.Parse("40000000-0000-0000-0000-000000000017"), lexicId, "lexicon_quantifier_choice", "Cuantificador incorrecto (ej. much vs many)", ""),
            GrammarTopic.CreateWithId(Guid.Parse("40000000-0000-0000-0000-000000000018"), lexicId, "lexicon_determiner_choice", "Determinante incorrecto (ej. this vs that)", ""),

            // ==========================================
            // 5. SEMANTICS (9 Topics) -> Rango 5000...
            // ==========================================
            GrammarTopic.CreateWithId(Guid.Parse("50000000-0000-0000-0000-000000000001"), semanId, "semantics_general_error", "Error semántico general", "Válvula de escape"),
            GrammarTopic.CreateWithId(Guid.Parse("50000000-0000-0000-0000-000000000002"), semanId, "semantics_ambiguous_meaning", "Significado ambiguo o poco claro", ""),
            GrammarTopic.CreateWithId(Guid.Parse("50000000-0000-0000-0000-000000000003"), semanId, "semantics_illogical_statement", "Declaración ilógica o contradictoria", ""),
            GrammarTopic.CreateWithId(Guid.Parse("50000000-0000-0000-0000-000000000004"), semanId, "semantics_literal_translation_error", "Traducción literal que pierde sentido", ""),
            GrammarTopic.CreateWithId(Guid.Parse("50000000-0000-0000-0000-000000000005"), semanId, "semantics_redundancy", "Redundancia semántica (pleonasmo)", ""),
            GrammarTopic.CreateWithId(Guid.Parse("50000000-0000-0000-0000-000000000006"), semanId, "semantics_nuance_lost", "Pérdida de matiz o tono sutil", ""),
            GrammarTopic.CreateWithId(Guid.Parse("50000000-0000-0000-0000-000000000007"), semanId, "semantics_scope_ambiguity", "Ambigüedad de alcance (ej. negación)", ""),
            GrammarTopic.CreateWithId(Guid.Parse("50000000-0000-0000-0000-000000000008"), semanId, "semantics_idiomatic_meaning_lost", "Pérdida de significado idiomático", ""),
            GrammarTopic.CreateWithId(Guid.Parse("50000000-0000-0000-0000-000000000009"), semanId, "semantics_reference_unclear", "Referente de pronombre poco claro", ""),

            // ==========================================
            // 6. PRAGMATICS (9 Topics) -> Rango 6000...
            // ==========================================
            GrammarTopic.CreateWithId(Guid.Parse("60000000-0000-0000-0000-000000000001"), pragmaId, "pragmatics_general_error", "Error pragmático general", "Válvula de escape"),
            GrammarTopic.CreateWithId(Guid.Parse("60000000-0000-0000-0000-000000000002"), pragmaId, "pragmatics_impolite_tone", "Tono descortés o muy directo", ""),
            GrammarTopic.CreateWithId(Guid.Parse("60000000-0000-0000-0000-000000000003"), pragmaId, "pragmatics_overly_polite", "Tono excesivamente cortés para el contexto", ""),
            GrammarTopic.CreateWithId(Guid.Parse("60000000-0000-0000-0000-000000000004"), pragmaId, "pragmatics_inappropriate_register", "Registro inapropiado para la situación social", ""),
            GrammarTopic.CreateWithId(Guid.Parse("60000000-0000-0000-0000-000000000005"), pragmaId, "pragmatics_cultural_misunderstanding", "Malentendido o tabú cultural", ""),
            GrammarTopic.CreateWithId(Guid.Parse("60000000-0000-0000-0000-000000000006"), pragmaId, "pragmatics_indirectness_missing", "Falta de tacto/mitigación en una petición", ""),
            GrammarTopic.CreateWithId(Guid.Parse("60000000-0000-0000-0000-000000000007"), pragmaId, "pragmatics_response_unnatural", "Respuesta gramatical pero antinatural", ""),
            GrammarTopic.CreateWithId(Guid.Parse("60000000-0000-0000-0000-000000000008"), pragmaId, "pragmatics_greeting_inappropriate", "Saludo inapropiado", ""),
            GrammarTopic.CreateWithId(Guid.Parse("60000000-0000-0000-0000-000000000009"), pragmaId, "pragmatics_closing_inappropriate", "Despedida inapropiada", "")
        };

        builder.HasData(topics);
    }
}
