using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TranslaterServiceDL.Models
{
    public class Translation
    {
        public Guid Id { get; set; }

        public Guid KeywordId { get; set; }
        public Keyword Keyword { get; set; } = null!;

        public Guid LanguageId { get; set; }
        public Language Language { get; set; } = null!;

        public string Value { get; set; } = string.Empty;

    }

    public class TranslationConfiguration : IEntityTypeConfiguration<Translation>
    {
        public void Configure(EntityTypeBuilder<Translation> builder)
        {
            builder.ToTable(nameof(Translation)).HasKey(e => e.Id);
        }
    }
}
