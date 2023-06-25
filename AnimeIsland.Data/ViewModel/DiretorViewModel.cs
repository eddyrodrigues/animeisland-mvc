using AnimeIsland.Data.Models;
using System;

namespace AnimeIsland.Data.ViewModel;

public class DiretorViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public DateTime DataNascimento { get; set; }
    public int QuantidadePublicacoes { get; set; }
    public IReadOnlyCollection<Anime> Producoes { get; set; }
}

public class DiretorLookupViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public DateTime DataNascimento { get; set; }
    public int QuantidadePublicacoes { get; set; }
}