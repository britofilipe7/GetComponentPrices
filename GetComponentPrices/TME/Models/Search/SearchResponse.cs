using Mouser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetComponentPrices.TME.Models.Search
{
    internal class SearchResponse
    {
        public string Status { get; set; }
        public Data Data { get; set; }
    }
}
