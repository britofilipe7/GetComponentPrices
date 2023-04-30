using GetComponentPrices.Farnell.Models;
using Mouser.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetComponentPrices.Farnell
{
    internal class Request
    {
        public Models.Response Response;

        public Request(string partNumber)
        {
            this.Execute(partNumber);
        }

        public void Execute(string partNumber)
        {
            var client = new RestClient("https://api.element14.com/");
            var request = new RestRequest("catalog/products", Method.Get);

            request.AddParameter("term", "any:" + partNumber);
            request.AddParameter("storeInfo.id", ConfigurationManager.AppSettings["FarnellStore"]);
            request.AddParameter("resultsSettings.offset", 0);
            request.AddParameter("resultsSettings.numberOfResults", 10);
            request.AddParameter("callInfo.responseDataFormat", "json");
            request.AddParameter("callInfo.apiKey", ConfigurationManager.AppSettings["FarnellKey"]);
            request.AddParameter("resultsSettings.responseGroup", "large");

            if (!ConfigurationManager.AppSettings["FarnellClientID"].Equals(""))
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                string data = "searchByKeyword" + timestamp;
                // Calculate HMAC-SHA1 from signature and encode by Base64 function
                byte[] hmacSha1 = HashHmac(data, ConfigurationManager.AppSettings["FarnellKey"]);
                string apiSignature = Convert.ToBase64String(hmacSha1);
                MessageBox.Show(apiSignature);
                string apiSignatureEncoded = UrlEncode(apiSignature);
                MessageBox.Show(apiSignatureEncoded);
                request.AddParameter("userInfo.customerId", ConfigurationManager.AppSettings["FarnellClientID"]);
                request.AddParameter("userInfo.timestamp", timestamp);
                request.AddParameter("userInfo.signature", apiSignatureEncoded);
            }

            RestResponse response = client.Execute(request);

            Response = JsonConvert.DeserializeObject<Models.Response>(response.Content);
            
        }
        private byte[] HashHmac(string input, string key)
        {
            HMACSHA1 hmac = new HMACSHA1(Encoding.ASCII.GetBytes(key));
            byte[] byteArray = Encoding.ASCII.GetBytes(input);
            return hmac.ComputeHash(byteArray);
        }
        private string UrlEncode(string s)
        {
            char[] temp = System.Web.HttpUtility.UrlEncode(s).ToCharArray();
            for (int i = 0; i < temp.Length - 2; i++)
            {
                if (temp[i] == '%')
                {
                    temp[i + 1] = char.ToUpper(temp[i + 1]);
                    temp[i + 2] = char.ToUpper(temp[i + 2]);
                }
            }
            return new string(temp); ;
        }
    }
}
