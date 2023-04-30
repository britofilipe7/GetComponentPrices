using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetComponentPrices.Farnell.Models
{
    internal class Breakdown
    {
        public int inv { get; set; }
        public string region { get; set; }
        public int lead { get; set; }
        public string warehouse { get; set; }
    }
}
