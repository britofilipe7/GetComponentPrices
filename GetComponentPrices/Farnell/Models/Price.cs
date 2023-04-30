using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetComponentPrices.Farnell.Models
{
    internal class Price
    {
        public int to { get; set; }
        public int from { get; set; }
        public double cost { get; set; }
    }
}
