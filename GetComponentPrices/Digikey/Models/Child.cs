using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetComponentPrices.Digikey.Models
{
    internal class Child
    {
        public List<Child> Children { get; set; }
        public int ProductCount { get; set; }
        public int NewProductCount { get; set; }
        public int ParameterId { get; set; }
        public string ValueId { get; set; }
        public string Parameter { get; set; }
        public string Value { get; set; }
    }
}
