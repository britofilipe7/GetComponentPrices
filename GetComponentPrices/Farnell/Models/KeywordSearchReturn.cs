using GetComponentPrices.Digikey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetComponentPrices.Farnell.Models
{
    internal class KeywordSearchReturn
    {
        public int numberOfResults { get; set; }
        public List<Product> products { get; set; }
    }
}
