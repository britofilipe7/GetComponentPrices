using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Mouser.Models
{

  
    public class SearchByKeyword
    {
        public string keyword { get; set; }
        
        public int? records { get; set; }

        public int? startingRecord { get; set; }

        public string? searchOptions { get; set; }

        public string? searchWithYourSignUpLanguage { get; set; }
    }
}
