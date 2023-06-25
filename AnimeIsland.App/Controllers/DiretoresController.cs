using AnimeIsland.Data.Models;
using AnimeIsland.Data.Repositories;
using AnimeIsland.Data.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AnimeIsland.App.Controllers;
[Route("diretores")]
public class DiretoresController : Controller
{
    private readonly IDiretoresRepository _diretoresRepository;
    private readonly IMapper _mapper;
    public DiretoresController(IDiretoresRepository diretoresRepository, IMapper mapper)
    {
        _diretoresRepository = diretoresRepository;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {

        //var list = await _diretoresRepository.GetAllAsync();
        IEnumerable<DiretorLookupViewModel> todosDiretores = await _diretoresRepository.GetAllLookup();

        return View(todosDiretores);
    }

    [HttpGet("adicionar")]
    [Conf.PermissaoAcesso("directors", "create")]
    public async Task<IActionResult> Create()
    {
        return View();
    }
    [HttpPost("adicionar")]
    [Conf.PermissaoAcesso("directors", "create")]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> CreatePost(DiretorViewModel dvm)
    {
        var diretor = _mapper.Map<Diretor>(dvm);

        if (diretor == null)
        {
            ModelState.AddModelError("","Os dados parecem inconsistentes, favor tentar novamente.");
            return View();
        }

        _diretoresRepository.Add(diretor);

        return RedirectToAction(nameof(Index));

    }

    [HttpGet("edit/{id}")]
    [Conf.PermissaoAcesso("directors", "edit")]
    public IActionResult EditDirector(Guid Id)
    {
        var director = _diretoresRepository.GetById(Id);

        if (director == null)
        {
            return NotFound();
        }

        return View("./Edit", _mapper.Map<DiretorLookupViewModel>(director));
    }
    [HttpGet("details/{id}")]
    public IActionResult detailsdirector(Guid Id)
    {
        var director = _diretoresRepository.GetById(Id);

        if (director == null)
        {
            return NotFound();
        }

        return View("./Details", _mapper.Map<DiretorLookupViewModel>(director));
    }
    [HttpGet("delete/{id}")]
    public IActionResult DeleteGet(Guid Id)
    {
        var director = _diretoresRepository.GetById(Id);

        if (director == null)
        {
            return NotFound();
        }

        return View("./Delete", _mapper.Map<DiretorLookupViewModel>(director));
    }
    [HttpPost("delete/{id}")]
    public IActionResult DeleteDelete(Guid Id)
    {
        var director = _diretoresRepository.GetById(Id);

        if (director == null)
        {
            return NotFound();
        }

        _diretoresRepository.Remove(director);

        return RedirectToAction(nameof(Index));
    }
}
