using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace GetComponentPrices.Digikey.Models
{


    public class SearchRequest
    {
        public string Keywords { get; set; }

        public int RecordCount{ get; set; }
    }
}
