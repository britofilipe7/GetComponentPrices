using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetComponentPrices.Digikey.Models
{
    internal class TokensFile
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public int access_token_expire { get; set; }
        public int refresh_token_expire { get; set; }
        public int last_modified { get; set; }
    }
}
