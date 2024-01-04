using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AppStudiesMVC.Models;
using Services;

namespace AppStudiesMVC.Controllers;

public class FormValidationController : Controller
{
    private readonly ILogger<FormValidationController> _logger;
    IQuoteService _service = null;

    public FormValidationController(ILogger<FormValidationController> logger, IQuoteService service)
    {
        _service = service;
        _logger = logger;
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

