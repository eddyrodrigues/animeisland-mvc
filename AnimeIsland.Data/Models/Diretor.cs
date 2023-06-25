using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.IO;

namespace AnimeIsland.Data.Models;

public abstract class Pessoa : Entity
{
	protected Pessoa(string nome, DateTime dataNascimento)
	{
		Nome = nome;
		DataNascimento = dataNascimento;
	}

	protected Pessoa() { }

	public string Nome { get; private set; }
	public DateTime DataNascimento { get; private set; }
}
public class Diretor : Pessoa
{
	public Diretor(string nome, DateTime dataNascimento) : base(nome, dataNascimento)
	{
         _animes = new List<Anime>();
    }
	
	protected Diretor(): base() { }
	
	public int RemoveAnimes(int value)
	{
		QuantidadePublicacoes -= value;
        if (QuantidadePublicacoes < 0)
		{
            QuantidadePublicacoes = 0;
        }
		return QuantidadePublicacoes;
    }
	public int AddAnimes(int value)
	{
		if (value <= 0) throw new InvalidOperationException("Cannot Add nothing of negative number in publications of author");

		QuantidadePublicacoes += value;
		return QuantidadePublicacoes;
	}

    public int QuantidadePublicacoes { get; set; }
	private List<Anime> _animes { get; set; }
	public virtual IList<Anime> Producoes
		=> _animes == null ? new List<Anime>() : _animes.ToList();

	public class DiretorMapInternal : IEntityTypeConfiguration<Diretor>
	{

		public void Configure(EntityTypeBuilder<Diretor> builder)
		{
			builder.HasMany(p => p._animes)
			.WithOne()
			.HasPrincipalKey(p => p.Id);
		}
	}
}
