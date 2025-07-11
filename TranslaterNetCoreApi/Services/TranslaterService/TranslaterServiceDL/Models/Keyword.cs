using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TranslaterServiceDL.Models
{
    public class Keyword
    {
        public Guid Id { get; set; }
        public string Value { get; set; } = string.Empty;
        public ICollection<Translation> Translations { get; set; } = new List<Translation>();
    }

    public class KeywordConfiguration : IEntityTypeConfiguration<Keyword>
    {
        public void Configure(EntityTypeBuilder<Keyword> builder)
        {
            builder.ToTable(nameof(Keyword)).HasKey(e => e.Id);

            builder.Property(x => x.Value)
            .IsRequired()
            .HasMaxLength(25)
            .HasColumnName(nameof(Keyword.Value));

            builder.HasIndex(x => x.Value).IsUnique();

            builder.HasMany(x => x.Translations)
                .WithOne(x => x.Keyword)
                .HasForeignKey(x => x.KeywordId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
