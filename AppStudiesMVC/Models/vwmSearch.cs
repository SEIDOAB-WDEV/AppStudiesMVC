using System;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AppStudiesMVC.Models
{
	public class vwmSearch
	{
        //public member becomes part of the Model in the Razor page
        public List<csFamousQuote> Quotes { get; set; } = new List<csFamousQuote>();

        //Pagination
        public int NrOfPages { get; set; }
        public int PageSize { get; } = 5;

        public int ThisPageNr { get; set; } = 0;
        public int PrevPageNr { get; set; } = 0;
        public int NextPageNr { get; set; } = 0;
        public int PresentPages { get; set; } = 0;

        //ModelBinding for the form
        [BindProperty]
        public string SearchFilter { get; set; }

        public vwmSearch()
		{
		}
	}
}

