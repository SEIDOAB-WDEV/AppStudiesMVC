using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AppStudiesMVC.Models;
using Services;
using static AppStudiesMVC.Models.vwmFullValidation;
using AppStudies.SeidoHelpers;
using System.ComponentModel.DataAnnotations;
using Models;

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

    [HttpGet]
    public IActionResult FullValidationListAdd()
    {
        var vwm = new vwmFullValidation();
        vwm.QuotesIM = _service.ReadQuotes().Select(q => new csFamousQuoteIM(q)).ToList();
        return View(vwm);
    }

    [HttpPost]
    public IActionResult fvlaDelete(Guid quoteId, vwmFullValidation vwm)
    {
        //Set the Quote as deleted, it will not be rendered
        vwm.QuotesIM.First(q => q.QuoteId == quoteId).StatusIM = enStatusIM.Deleted;
        return View("FullValidationListAdd", vwm);
    }

    [HttpPost]
    public IActionResult fvlaEdit(Guid quoteId, vwmFullValidation vwm)
    {
        int idx = vwm.QuotesIM.FindIndex(q => q.QuoteId == quoteId);
        string[] keys = { $"QuotesIM[{idx}].editQuote",
                            $"QuotesIM[{idx}].editAuthor"};
        if (!ModelState.IsValidPartially(out reModelValidationResult validationResult, keys))
        {
            vwm.ValidationResult = validationResult;
            return View("FullValidationListAdd", vwm);
        }

        //Set the Quote as Modified, it will later be updated in the database
        var q = vwm.QuotesIM.First(q => q.QuoteId == quoteId);
        q.StatusIM = enStatusIM.Modified;

        //Implement the changes
        q.Author = q.editAuthor;
        q.Quote = q.editQuote;
        return View("FullValidationListAdd", vwm);
    }

    [HttpPost]
    public IActionResult fvlaAdd(vwmFullValidation vwm)
    {
        string[] keys = { $"NewQuoteIM.Quote",
                            $"NewQuoteIM.Author"};
        if (!ModelState.IsValidPartially(out reModelValidationResult validationResult, keys))
        {
            vwm.ValidationResult = validationResult;
            return View("FullValidationListAdd", vwm);
        }

        //Set the Artist as Inserted, it will later be inserted in the database
        vwm.NewQuoteIM.StatusIM = enStatusIM.Inserted;

        //Need to add a temp Guid so it can be deleted and editited in the form
        //A correct Guid will be created by the DTO when Inserted into the database
        vwm.NewQuoteIM.QuoteId = Guid.NewGuid();

        //Add it to the Input Models artists
        vwm.QuotesIM.Add(new csFamousQuoteIM(vwm.NewQuoteIM));

        //Clear the NewArtist so another album can be added
        vwm.NewQuoteIM = new csFamousQuoteIM();

        return View("FullValidationListAdd", vwm);
    }

    public IActionResult fvlaUndo(vwmFullValidation vwm)
    {
        //Reload the InputModel
        vwm.QuotesIM = _service.ReadQuotes().Select(q => new csFamousQuoteIM(q)).ToList();
        return View("FullValidationListAdd", vwm);
    }

    public IActionResult fvlaSave(vwmFullValidation vwm)
    {
        //Note: Here I will not do any validation as all validation is done during the
        //OnPostEdit and OnPostAdd

        //Check if there are deleted quotes, if so simply remove them
        var _deletes = vwm.QuotesIM.FindAll(q => (q.StatusIM == enStatusIM.Deleted));
        foreach (var item in _deletes)
        {
            //Remove from the database
            _service.DeleteQuote(item.QuoteId);
        }

        #region Add quotes
        //Check if there are any new quotes added, if so create them in the database
        var _newies = vwm.QuotesIM.FindAll(q => (q.StatusIM == enStatusIM.Inserted));
        foreach (var item in _newies)
        {
            //Create the corresposning model
            var model = item.UpdateModel(new csFamousQuote());

            //create in the database
            model = _service.CreateQuote(model);
        }
        #endregion

        //Check if there are any modified quotes , if so update them in the database
        var _modyfies = vwm.QuotesIM.FindAll(a => (a.StatusIM == enStatusIM.Modified));
        foreach (var item in _modyfies)
        {
            //get model
            var model = _service.ReadQuote(item.QuoteId);

            //update the changes and save
            model = item.UpdateModel(model);
            model = _service.UpdateQuote(model);
        }

        //Reload the InputModel
        vwm.QuotesIM = _service.ReadQuotes().Select(q => new csFamousQuoteIM(q)).ToList();
        return View("FullValidationListAdd", vwm);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

