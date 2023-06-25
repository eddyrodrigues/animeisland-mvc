using AnimeIsland.Data.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AnimeIsland.Data.ViewModel;
public class AnimeViewModel
{
    public AnimeViewModel()
    {

    }
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required(ErrorMessage = "O anime precisa ter um título")]
    public string Titulo { get; set; }
    [Required(ErrorMessage ="É necessário ter uma sinopse")]
    [MinLength(10, ErrorMessage = "Minimo de 10 caractetres")]
    [MaxLength(2048, ErrorMessage = "Máximo de 2048 caractetres")]
    public string Sinopse { get;  set; }
    [Required(ErrorMessage ="Data de estréia deve ser válida!")]
    public DateTime DataEstreia { get; set; }
    public Guid DiretorId { get; set; }
    public DiretorLookupViewModel? Diretor { get; set; }
    public IList<DiretorLookupViewModel> Diretores { get; set; } = new List<DiretorLookupViewModel>();
    public IFormFile? Image { get; set; }
    public string? LocalImagem { get; set; }
}
