using AnimeIsland.Data.Context;
using AnimeIsland.Data.Contracts;
using AnimeIsland.Data.Models;
using AnimeIsland.Data.ViewModel;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AnimeIsland.Data.Repositories;
public class DiretoresRepository : BaseRepository<Diretor>, IDiretoresRepository
{
	private readonly IMapper _mapper;

	public DiretoresRepository(AnimeIslandDbContext context, IMapper mapper) : base(context)
	{
		_mapper = mapper;
	}

	public async Task<IEnumerable<DiretorLookupViewModel>> GetAllLookup()
	{
        var diretores = _mapper.Map<List<DiretorLookupViewModel>>(await _context.Diretores.ToListAsync());



		return diretores;

    }

}

public interface IDiretoresRepository: IBaseRepository<Diretor>
{
	Task<IEnumerable<DiretorLookupViewModel>> GetAllLookup();
}