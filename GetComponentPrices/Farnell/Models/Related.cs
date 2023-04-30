using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetComponentPrices.Farnell.Models
{
    internal class Related
    {
        public bool containAlternatives { get; set; }
        public bool containcontainRoHSAlternatives { get; set; }
        public bool containAccessories { get; set; }
        public bool containcontainRoHSAccessories { get; set; }
    }
}
