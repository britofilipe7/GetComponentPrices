using GetComponentPrices.TME.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetComponentPrices.TME.Models.GetPricesAndStocks
{
    internal class Data
    {
        public string Currency { get; set; }
        public string Language { get; set; }
        public string PriceType { get; set; }
        public List<ProductList> ProductList { get; set; }
    }


}

