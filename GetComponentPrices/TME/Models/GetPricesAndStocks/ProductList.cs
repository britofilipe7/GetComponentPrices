using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace GetComponentPrices.TME.Models.GetPricesAndStocks
{
    internal class ProductList
    {
        public string Symbol { get; set; }
        public List<PriceList> PriceList { get; set; }
        public string Unit { get; set; }
        public int VatRate { get; set; }
        public string VatType { get; set; }
        public int Amount { get; set; }
    }
}
