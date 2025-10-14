namespace MyCV_Demo.Models
{
    public class TaxResultModel
    {
        public string CountryCode { get; set; } = "";
        public int TaxYear {  get; set; }

        public decimal TaxableIncome { get; set; }
        public decimal TotalTax { get; set; }

        public decimal NetIncome => TaxableIncome - TotalTax;
    }
}
