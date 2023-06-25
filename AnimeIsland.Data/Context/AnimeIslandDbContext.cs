using AnimeIsland.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static AnimeIsland.Data.Models.Diretor;

namespace AnimeIsland.Data.Context;
public class AnimeIslandDbContext : IdentityDbContext
{
    public DbSet<Diretor> Diretores => Set<Diretor>();
    public DbSet<Anime> Animes => Set<Anime>();
    public AnimeIslandDbContext(DbContextOptions<AnimeIslandDbContext> options) : base(options)
    {
        //this.Database.SetCommandTimeout(TimeSpan.FromSeconds(100));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        modelBuilder.ApplyConfiguration(new DiretorMapInternal());
    }

}
