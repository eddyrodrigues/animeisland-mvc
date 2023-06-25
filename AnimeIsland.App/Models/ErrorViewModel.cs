namespace AnimeIsland.App.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public string MessageInfo { get; set; }
    public string Mensagem { get; internal set; }
    public string Titulo { get; internal set; }
    public int ErroCode { get; internal set; }
}
