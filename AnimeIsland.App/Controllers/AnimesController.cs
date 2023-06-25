using AnimeIsland.Data.Contracts;
using AnimeIsland.Data.Models;
using AnimeIsland.Data.Repositories;
using AnimeIsland.Data.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimeIsland.App.Controllers;
[Route("animes")]
public class AnimesController : Controller
{
    private readonly IAnimeRepository _animesRepository;
    private readonly IMapper _mapper;
    private readonly IDiretoresRepository _diretoresRepository;

    public AnimesController(IAnimeRepository animesRepository, IMapper mapper, IDiretoresRepository diretoresRepository)
    {
        _animesRepository = animesRepository;
        _mapper = mapper;
        _diretoresRepository = diretoresRepository;
    }
    // GET: AnimesController
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult> Index()
    {
        List<AnimeViewModel> allAnimesViewModel = _mapper.Map<List<AnimeViewModel>>(await _animesRepository.GetAllAnimesCompleteLookup());
        var diretoresExistentes = await GetDiretorLookupViewModelsAsync();
        for(int i = 0; i < allAnimesViewModel.Count; i++)
        {
            await PopulaDiretoresAnimes(allAnimesViewModel[i], diretoresExistentes);
            allAnimesViewModel[i].Diretor = allAnimesViewModel[i].Diretores.Where(d => d.Id == allAnimesViewModel[i].DiretorId).FirstOrDefault();
        }

        return View(allAnimesViewModel);
    }


    // GET: AnimesController/Details/5
    [HttpGet("details/{id}")]
    [AllowAnonymous]
    public async Task<ActionResult> Details(Guid id)
    {
        var vm = _mapper.Map<AnimeViewModel>(_animesRepository.GetById(id));

        if (vm == null) return NotFound();

        await PopulaDiretoresAnimes(vm);
        return View(vm);
    }

    // GET: AnimesController/Create
    [Conf.PermissaoAcesso("animes", "create")]
    [HttpGet("adicionar")]
    public async Task<ActionResult> Create()
    {
        var vm = new AnimeViewModel();
        await PopulaDiretoresAnimes(vm);
        return View(vm);
    }

    // POST: AnimesController/Create
    [HttpPost("adicionar")]
    [Conf.PermissaoAcesso("animes", "create")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(AnimeViewModel vm)
    {
        await PopulaDiretoresAnimes(vm);

        if (!ModelState.IsValid) return View(vm);

        if (vm.Image == null)
        {
            ModelState.AddModelError("Capa do anime", "O anime precisa ter um capa.");
            return View(vm);
        }

        var novoAnime = _mapper.Map<Anime>(vm);

        if (novoAnime == null) return View(vm);

        if (novoAnime.DataEstreia < new DateTime(2000, 01, 01))
        {
            ModelState.AddModelError("Data de estréria", "A data de estréia não pode ser inferior a 01/01/2021");
            return View(vm);
        }
        
        string fileName = "";
        if (vm.Image != null)
        {
            var prefixo = Guid.NewGuid().ToString();
            if (!UploadImage(vm.Image, prefixo, out fileName))
            {
                return View(vm);
            }
            novoAnime.SetLocalImagem(fileName);
        }

        Diretor? choosenDirector = _diretoresRepository.GetById(novoAnime.DiretorId);

        if (choosenDirector is null)
        {
            ModelState.AddModelError("Diretor", "Diretor escolhido é inválido.");
            return View(vm);
        }


        choosenDirector.AddAnimes(1);

        _diretoresRepository.Update(choosenDirector);
        _animesRepository.Add(novoAnime);
        

        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: AnimesController/Edit/5
    [Conf.PermissaoAcesso("animes", "edit")]
    [HttpGet("editar/{id}")]
    public async Task<ActionResult> Edit(Guid id)
    {
        var vm = _mapper.Map<AnimeViewModel>(_animesRepository.GetById(id));

        if (vm == null) return NotFound();

        await PopulaDiretoresAnimes(vm);
        return View(vm);
    }

    // POST: AnimesController/Edit/5
    [HttpPost("editar/{id}"), ]
    //[Conf.PermissaoAcesso("Animes", "Adicionar/Editar")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(Guid id, [Bind("Id", "Titulo", "Sinopse", "DataEstreia", "DiretorId", "Image")]AnimeViewModel animeViewModel)
    {
        
        if (animeViewModel.Id != id) return NotFound();
        
        await PopulaDiretoresAnimes(animeViewModel);


        if (!ModelState.IsValid)
        {
            return View(animeViewModel);
        }

        var animeAtualizar = _animesRepository.GetById(id);

        string fileName = "";
        if (animeViewModel.Image != null)
        {
            var prefixo = Guid.NewGuid().ToString();
            if (!UploadImage(animeViewModel.Image, prefixo, out fileName))
            {
                return View(animeViewModel);
            }
        }
        
        

        animeAtualizar.SetTitulo(animeViewModel.Titulo);
        animeAtualizar.SetSinopse(animeViewModel.Sinopse);
        animeAtualizar.SetDataEstreia(animeViewModel.DataEstreia);
        animeAtualizar.SetLocalImagem(fileName);

        if (animeAtualizar.DiretorId != animeViewModel.DiretorId)
        {
            var diretorEscolhido = ObterDiretorId(animeViewModel.DiretorId);
            
            if (diretorEscolhido == null)
            {
                ModelState.AddModelError("Diretor", "Diretor escolhdo não existe");
                return View(animeViewModel);
            }

            Diretor? diretorAntigo = ObterDiretorId(animeAtualizar.DiretorId);
            if (diretorAntigo == null)
            {
                ModelState.AddModelError("Diretor", "Diretor antigo não existe");
                return View(animeViewModel);
            }

            // remove anime quantity from actual director
            diretorAntigo.RemoveAnimes(1);
            _diretoresRepository.Update(diretorAntigo);

            // Add a new anime quantity for new director
            diretorEscolhido.AddAnimes(1);
            _diretoresRepository.Update(diretorEscolhido);

            animeAtualizar.SetDiretor(diretorEscolhido);
        }

        _animesRepository.Update(animeAtualizar);

        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
    // GET: AnimesController/Delete/5
    [Conf.PermissaoAcesso("animes", "delete")]
    [HttpGet("deletar/{id:guid}")]
    public ActionResult Delete(Guid id)
    {
        if (id == null)
        {
            NotFound();
        }
        var avm = _mapper.Map<AnimeViewModel>(_animesRepository.GetById(id));
        return View(avm);
    }

    // POST: AnimesController/Delete/5
    [HttpPost("deletar/{id:guid}")]
    [Conf.PermissaoAcesso("animes", "delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ConfirmDelete(Guid id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var anime = _animesRepository.GetById(id);

        if (anime == null)
        {
            return NotFound();
        }


        var director = _diretoresRepository.GetById(anime.DiretorId);
        director.RemoveAnimes(1);
        _diretoresRepository.Update(director);
        await _animesRepository.Remove(anime);

        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    private async Task PopulaDiretoresAnimes(List<AnimeViewModel> vms)
    {
        List<DiretorLookupViewModel> listaDiretores = _mapper.Map<List<DiretorLookupViewModel>>(await _animesRepository.GetAllDirectorsName());
        foreach(AnimeViewModel vm in vms)
        {
            await PopulaDiretoresAnimes(vm, listaDiretores);
        }
    }
    private async Task PopulaDiretoresAnimes(AnimeViewModel vm_unico, List<DiretorLookupViewModel>? diretores = null)
    {
        
        if (diretores == null)
        {
            vm_unico.Diretores = _mapper.Map<List<DiretorLookupViewModel>>(await _animesRepository.GetAllDirectorsName());
        }
        else
        {
            vm_unico.Diretores = diretores;
        }
    }
    private async Task<List<DiretorLookupViewModel>> GetDiretorLookupViewModelsAsync() => _mapper.Map<List<DiretorLookupViewModel>>(await _animesRepository.GetAllDirectorsName());



    private Diretor ObterDiretorId(Guid id)
    {
        var diretorEncontrado = _animesRepository.GetAllDirectorsName().Result.Where(d => d.Id == id).FirstOrDefault();
        if (diretorEncontrado == null)
        {
            ModelState.AddModelError("DiretorId", "DiretorId Nao foi informado corretamente");
            return null;
        }
        return diretorEncontrado;
    }
    private bool UploadImage(IFormFile image, string prefixo, out string fileName)
    {
        fileName = "";
        if (image.Length <= 0) return false;

        var fileExtension = Path.GetExtension(image.FileName);
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", prefixo + fileExtension);
        fileName = prefixo + fileExtension;


        if (System.IO.File.Exists(path))
        {
            ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
            return false;
        }

        using (var stream = new FileStream(path, FileMode.Create))
        {
            image.CopyToAsync(stream);
        }

        return true;
    }

}
