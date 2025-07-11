using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TranslaterServiceDL.Models
{
    public class Language
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Translation> Translations { get; set; } = new List<Translation>();
    }

    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable(nameof(Language)).HasKey(e => e.Id);

            builder.Property(x => x.Name)
              .IsRequired()
              .HasMaxLength(25)
              .HasColumnName(nameof(Language.Name));

            builder.HasMany(x => x.Translations)
                .WithOne(x => x.Language)
                .HasForeignKey(x => x.LanguageId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
