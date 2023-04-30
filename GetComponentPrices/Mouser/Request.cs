using Mouser.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetComponentPrices.Mouser
{
    internal class Request
    {

        public Results Response;
        
        public Request (string partNumber)
        {
            this.Execute(partNumber);
        }

        public void Execute(string partNumber)
        {
            var client = new RestClient("https://api.mouser.com/");
            var request = new RestRequest(("api/v1.0/search/keyword?apiKey=" + ConfigurationManager.AppSettings["MouserKey"]), Method.Post);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new {
                SearchByKeywordRequest = new 
                {
                    keyword = partNumber
                }                
            });
            
            RestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                
                Response = JsonConvert.DeserializeObject<Results>(response.Content);
            }
        }
    }
}
