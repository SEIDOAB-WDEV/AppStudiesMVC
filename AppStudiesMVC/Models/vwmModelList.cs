using System;
using Models;

namespace AppStudiesMVC.Models
{
	public class vwmModelList
	{
        //public member becomes part of the Model in the Razor page
        public List<csFamousQuote> Quotes { get; set; } = new List<csFamousQuote>();

        public vwmModelList()
		{
		}
	}
}

