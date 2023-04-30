using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetComponentPrices.Digikey.Models
{
    internal class Series
    {
        public int ParameterId { get; set; }
        public string ValueId { get; set; }
        public string Parameter { get; set; }
        public string Value { get; set; }
    }
}
