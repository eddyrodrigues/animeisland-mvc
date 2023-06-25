using AnimeIsland.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeIsland.Data.Mapping;
public class AnimeMap : IEntityTypeConfiguration<Anime>
{
    public void Configure(EntityTypeBuilder<Anime> builder)
    {
        builder.ToTable("Animes");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Titulo).HasColumnType("VARCHAR").HasMaxLength(255).IsRequired();
        builder.Property(p => p.DataEstreia).HasColumnType("DATETIME").IsRequired();
        builder.Property(p => p.Sinopse).HasColumnType("VARCHAR").HasMaxLength(2048);
        builder.Property(p => p.LocalImagem).HasColumnName("LocalImagem").HasColumnType("NVARCHAR").HasMaxLength(2048);
        //builder.Property(p => p.DiretorId).HasColumnType("INT");

        //builder.HasOne(p => p.Diretor)
        //       .WithMany()
        //       .HasForeignKey(p => p.DiretorId)
        //       .HasPrincipalKey(d => d.Id);
    }
}
