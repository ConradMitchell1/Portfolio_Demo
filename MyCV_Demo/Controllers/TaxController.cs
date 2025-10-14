using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCV_Demo.Models;

namespace MyCV_Demo.Controllers
{
    public class TaxController : Controller
    {
        // GET: TaxController
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CalculatePage(TaxInputModel input)
        {
            if (!ModelState.IsValid) 
            {
                return View("Index", input);
            }
            var result = CalculateTax(input);
            return View("Calculated", result);
        }
        public TaxResultModel CalculateTax(TaxInputModel tm)
        {
            var taxable = Math.Max(0, tm.AnnualIncome - tm.Deductions);
            decimal rate = tm.CountryCode.ToUpper() switch
            {
                "Uk" => 0.20m,
                "US" => 0.22m,
                "FR" => 0.25m,
                _ => 0.20m
            };
            var tax = taxable * rate;
            return new TaxResultModel
            {
                CountryCode = tm.CountryCode,
                TaxYear = tm.TaxYear,
                TaxableIncome = decimal.Round(taxable, 2),
                TotalTax = decimal.Round(tax, 2)
            };
        }

    }
}
