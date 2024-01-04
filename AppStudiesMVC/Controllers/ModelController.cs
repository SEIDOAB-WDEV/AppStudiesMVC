using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AppStudiesMVC.Models;
using Services;

namespace AppStudiesMVC.Controllers;

public class ModelController : Controller
{
    private readonly ILogger<ModelController> _logger;
    IQuoteService _service = null;

    public ModelController(ILogger<ModelController> logger, IQuoteService service)
    {
        _service = service;
        _logger = logger;
    }

    //Will execute on a Get request
    [HttpGet]
    public IActionResult ModelList()
    {
        var vw = new vwmModelList();

        //Use the Service
        vw.Quotes = _service.ReadQuotes();
        //return View(vw);
        return View("ModelList", vw);
    }

    //Will execute on a Get request
    [HttpGet]
    public IActionResult Search()
    {
        var vwm = new vwmSearch();
        //Read a QueryParameters
        if (int.TryParse(Request.Query["pagenr"], out int _pagenr))
        {
            vwm.ThisPageNr = _pagenr;
        }

        vwm.SearchFilter = Request.Query["search"];

        //Pagination
        vwm.UpdatePagination(_service);

        //Use the Service
        vwm.Quotes = _service.ReadQuotes(vwm.ThisPageNr, vwm.PageSize, vwm.SearchFilter);

        return View(vwm);
    }

    [HttpPost]
    public IActionResult Find(vwmSearch vwm)
    {
        //Pagination
        vwm.UpdatePagination(_service);

        //Use the Service
        vwm.Quotes = _service.ReadQuotes(vwm.ThisPageNr, vwm.PageSize, vwm.SearchFilter);

        return View("Search", vwm);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    /*
    private void UpdatePagination(vwmSearch vwm)
    {
        //Pagination
        vwm.NrOfPages = (int)Math.Ceiling((double)_service.NrOfQuotes(vwm.SearchFilter) / vwm.PageSize);
        vwm.PrevPageNr = Math.Max(0, vwm.ThisPageNr - 1);
        vwm.NextPageNr = Math.Min(vwm.NrOfPages - 1, vwm.ThisPageNr + 1);
        vwm.PresentPages = Math.Min(3, vwm.NrOfPages);
    }
    */
}

