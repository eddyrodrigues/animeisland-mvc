namespace AnimeIsland.Data.Models;
public class Anime : Entity
{
    protected Anime(string titulo, string sinopse, DateTime dataEstreia)
    {
        Titulo = titulo;
        Sinopse = sinopse;
        DataEstreia = dataEstreia;
    }
    protected Anime() { }

    public string Titulo { get; private set; }
    public string Sinopse { get; private set; }
    public DateTime DataEstreia { get; private set; }
    public string LocalImagem { get; set;  }
    public Guid DiretorId { get; set; }


    public void SetLocalImagem(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            LocalImagem = value;
        }
    }
    public void SetTitulo(string value)
    {
        Titulo = value;
    }
    public void SetSinopse(string value)
    {
        Sinopse = value;
    }
    public void SetDataEstreia(DateTime value)
    {
        DataEstreia = value;
    }
    public void SetDiretor(Diretor diretor)
    {
        DiretorId = diretor.Id;
    }
}
