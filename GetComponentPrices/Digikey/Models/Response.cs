using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetComponentPrices.Digikey.Models
{
    internal class Response
    {
        public LimitedTaxonomy LimitedTaxonomy { get; set; }
        public List<Product> Products { get; set; }
        public int ProductsCount { get; set; }
        public int ExactManufacturerProductsCount { get; set; }
        public List<ExactManufacturerProduct> ExactManufacturerProducts { get; set; }
        public object ExactDigiKeyProduct { get; set; }
        public SearchLocaleUsed SearchLocaleUsed { get; set; }
    }
}
