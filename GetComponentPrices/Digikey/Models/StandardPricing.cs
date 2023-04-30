using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetComponentPrices.Digikey.Models
{
    internal class StandardPricing
    {
        public int BreakQuantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
    }
}
