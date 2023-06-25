using AnimeIsland.Data.Models;

namespace AnimeIsland.Data.Contracts;

public interface IAnimeRepository : IBaseRepository<Anime>
{
    Task<IEnumerable<Anime>> GetAllAnimesCompleteLookup();
    Task<IEnumerable<Diretor>> GetAllDirectorsName();
    Anime Update(Anime animeAtualizar);
}