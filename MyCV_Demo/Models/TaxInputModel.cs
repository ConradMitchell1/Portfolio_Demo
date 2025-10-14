namespace MyCV_Demo.Models
{
    public class TaxInputModel
    {
        public string CountryCode { get; set; } = "Uk";

        public decimal AnnualIncome { get; set; }

        public decimal Deductions { get; set; } = 0; // pension, allowance, etc.

        public int TaxYear { get; set; } = DateTime.UtcNow.Year;
    }
}
