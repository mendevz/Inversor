using Inversor.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inversor.Infrastructure.Persistence.Configurations;

public class MacroTagConfiguration : IEntityTypeConfiguration<MacroTag>
{
    public void Configure(EntityTypeBuilder<MacroTag> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Tag).HasMaxLength(50).IsRequired();
        builder.HasIndex(m => m.Tag).IsUnique();

        builder.HasData(
            MacroTag.CreateWithId(Guid.Parse("11111111-1111-1111-1111-100000000000"), "ORTHOGRAPHY", "Escritura correcta de las palabras."),
            MacroTag.CreateWithId(Guid.Parse("22222222-2222-2222-2222-200000000000"), "MORPHOLOGY", "Forma y conjugación de las palabras."),
            MacroTag.CreateWithId(Guid.Parse("33333333-3333-3333-3333-300000000000"), "SYNTAX", "Orden y estructura de la oración."),
            MacroTag.CreateWithId(Guid.Parse("44444444-4444-4444-4444-400000000000"), "LEXICON", "Vocabulario y significado literal."),
            MacroTag.CreateWithId(Guid.Parse("55555555-5555-5555-5555-500000000000"), "SEMANTICS", "Sentido lógico de la oración."),
            MacroTag.CreateWithId(Guid.Parse("66666666-6666-6666-6666-600000000000"), "PRAGMATICS", "Uso adecuado según el contexto.")
        );
    }
}