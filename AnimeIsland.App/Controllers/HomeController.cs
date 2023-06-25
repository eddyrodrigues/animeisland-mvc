using AnimeIsland.App.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace AnimeIsland.App.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    //[Route("error/{statusCode}")]
    //public IActionResult Error(int statusCode)
    //{

    //    var erroViewModel = new ErrorViewModel()
    //    {
    //        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
    //    };
    //    erroViewModel.MessageInfo = $"Erro interno não identificado! {statusCode}";

    //    return View(erroViewModel);
    //}
    [Route("erro/{id:length(3,3)}")]
    public IActionResult Errors(int id)
    {
        var modelErro = new ErrorViewModel();

        var exceptionHandlerPathFeature =
            HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        if (id == 500)
        {
            //if (exceptionHandlerPathFeature.Error)
            Console.WriteLine(exceptionHandlerPathFeature.Error.Message);
            if (exceptionHandlerPathFeature.Error.GetType() == typeof(SqlException))
            {
                modelErro.Mensagem = $"Ocorreu um erro de conexão com o servidor de banco de dados! Tenta novamente em alguns instantes.";
            }
            else
            {
                modelErro.Mensagem = $"Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte. {exceptionHandlerPathFeature.Error.GetBaseException().GetType().Name}";
            }
            modelErro.Titulo = "Ocorreu um erro!";
            modelErro.ErroCode = id;
        }
        else if (id == 404)
        {
            modelErro.Mensagem = "A página que está procurando não existe! <br/> Em caso de dúvidas entre em contato com nosso suporte";
            modelErro.Titulo = "Ops! Página não encontrada.";
            modelErro.ErroCode = id;
        }
        else if (id == 403)
        {
            modelErro.Mensagem = "Você não tem permissão para fazer isto.";
            modelErro.Titulo = "Acesso Negado";
            modelErro.ErroCode = id;
        }
        else
        {
            return StatusCode(500);
        }

        return View("error", modelErro);
    }
}
