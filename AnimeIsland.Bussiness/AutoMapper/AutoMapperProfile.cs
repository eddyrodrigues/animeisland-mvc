using AnimeIsland.Data.ViewModel;
using AnimeIsland.Data.Models;
using AutoMapper;

namespace AnimeIsland.Bussiness.AutoMapper;
public class AnimeAutoMapperProfile : Profile
{
	public AnimeAutoMapperProfile()
	{
		CreateMap<Anime, AnimeViewModel>().ReverseMap();
		CreateMap<Diretor, DiretorViewModel>().ReverseMap();
		CreateMap<Diretor, DiretorLookupViewModel>();
	}
}
