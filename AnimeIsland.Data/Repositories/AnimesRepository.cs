using AnimeIsland.Data.Context;
using AnimeIsland.Data.Contracts;
using AnimeIsland.Data.Models;
using AnimeIsland.Data.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace AnimeIsland.Data.Repositories;
public class AnimesRepository : BaseRepository<Anime>, IAnimeRepository
{
	public AnimesRepository(AnimeIslandDbContext context) : base(context)
	{
	}

	public async Task<IEnumerable<Diretor>> GetAllDirectorsName()
	{
		return await _context.Diretores.ToListAsync();
	}

	public async Task<IEnumerable<Anime>> GetAllAnimesCompleteLookup()
	{
		var animes =  _context.Animes.ToList();

		return animes;
		
		
		//.Include(a => a.Diretor).OrderByDescending(a => a.DataEstreia).ToListAsync();
	}

	public Anime Update(Anime animeAtualizar)
	{
		_context.Animes.Update(animeAtualizar);
		_context.SaveChanges();
		return animeAtualizar;
	}
}
