using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mouser.Models
{
    public class SearchResponse
    {
        public int NumberOfResult { get; set; }
        public List<MouserPart> Parts { get; set; }
    }
}
