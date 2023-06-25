using AnimeIsland.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeIsland.Data.Mapping;
public class DiretorMap : IEntityTypeConfiguration<Diretor>
{
    public void Configure(EntityTypeBuilder<Diretor> builder)
    {
        builder.ToTable("Diretores");

        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.QuantidadePublicacoes).HasColumnType("INT");
        builder.Property(p => p.DataNascimento).HasColumnType("DATETIME").IsRequired();
        builder.Property(p => p.Nome).HasColumnType("VARCHAR").HasMaxLength(255).IsRequired();

        builder.Ignore(p => p.Producoes);

    }
}
