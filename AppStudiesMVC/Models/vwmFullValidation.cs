using System;
using System.ComponentModel.DataAnnotations;
using AppStudies.SeidoHelpers;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AppStudiesMVC.Models
{
    public enum enStatusIM { Unknown, Unchanged, Inserted, Modified, Deleted }

    public class vwmFullValidation
	{
        //InputModel (IM) is locally declared classes that contains ONLY the properties of the Model
        //that are bound to the <form> tag
        //EVERY property must be bound to an <input> tag in the <form>
        [BindProperty]
        public List<csFamousQuoteIM> QuotesIM { get; set; }

        [BindProperty]
        public csFamousQuoteIM NewQuoteIM { get; set; } = new csFamousQuoteIM();

        //For Validation
        public reModelValidationResult ValidationResult { get; set; } = new reModelValidationResult(false, null, null);

        #region Input Model
        //InputModel (IM) is locally declared classes that contains ONLY the properties of the Model
        //that are bound to the <form> tag
        //EVERY property must be bound to an <input> tag in the <form>
        //These classes are in center of ModelBinding and Validation
        public class csFamousQuoteIM
        {
            //Status of InputModel
            public enStatusIM StatusIM { get; set; }

            //Properties from Model which is to be edited in the <form>
            public Guid QuoteId { get; set; } = Guid.NewGuid();

            [Required(ErrorMessage = "You type provide a quote")]
            public string Quote { get; set; }

            [Required(ErrorMessage = "You must provide an author")]
            public string Author { get; set; }

            //Added properites to edit in the list with undo
            [Required(ErrorMessage = "You must provide an quote")]
            public string editQuote { get; set; }

            [Required(ErrorMessage = "You must provide an author")]
            public string editAuthor { get; set; }

            #region constructors and model update
            public csFamousQuoteIM() { StatusIM = enStatusIM.Unchanged; }

            //Copy constructor
            public csFamousQuoteIM(csFamousQuoteIM original)
            {
                StatusIM = original.StatusIM;

                QuoteId = original.QuoteId;
                Quote = original.Quote;
                Author = original.Author;

                editQuote = original.editQuote;
                editAuthor = original.editAuthor;
            }

            //Model => InputModel constructor
            public csFamousQuoteIM(csFamousQuote original)
            {
                StatusIM = enStatusIM.Unchanged;
                QuoteId = original.QuoteId;
                Quote = editQuote = original.Quote;
                Author = editAuthor = original.Author;
            }

            //InputModel => Model
            public csFamousQuote UpdateModel(csFamousQuote model)
            {
                model.QuoteId = QuoteId;
                model.Quote = Quote;
                model.Author = Author;
                return model;
            }
            #endregion

        }
        #endregion

        public vwmFullValidation()
		{
		}
	}
}

