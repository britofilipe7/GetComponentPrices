using Mouser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mouser.Models
{
    public class Results
    {
        public List<ErrorEntity> Errors { get; set; }
        public SearchResponse SearchResults { get; set; }
    }
}
