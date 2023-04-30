using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetComponentPrices.Farnell.Models
{
    internal class RegionalBreakdown
    {
        public int level { get; set; }
        public int leastLeadTime { get; set; }
        public int status { get; set; }
        public string warehouse { get; set; }
        public bool shipsFromMultipleWarehouses { get; set; }
    }
}
