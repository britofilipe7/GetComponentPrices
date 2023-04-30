using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetComponentPrices.Digikey.Models
{
    internal class SearchLocaleUsed
    {
        public string Site { get; set; }
        public string Language { get; set; }
        public string Currency { get; set; }
        public string ShipToCountry { get; set; }
    }
}
