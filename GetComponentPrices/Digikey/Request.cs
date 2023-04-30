using GetComponentPrices.Digikey.Models;
using Mouser.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace GetComponentPrices.Digikey
{
    internal class Request
    {
        private TokensFile tokens;
        //Get path for dk_oauth file
        private string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"dk_oauth.json");

        public Request()
        {
            this.GetTokensFromFile();
        }

        //Method to get tokens from file
        public void GetTokensFromFile()
        {
           
            try
            {
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    tokens = JsonConvert.DeserializeObject<TokensFile>(json);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("dk_oauth File not found");
                return;
            }

            //Get actual epoch time
            int actualTime = ((int)((DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds));

            if (tokens.access_token_expire < actualTime || tokens.refresh_token_expire < actualTime)
            {
                this.RefreshTokens();
            }
            this.RefreshTokens();
        }

        private void RefreshTokens()
        {

            var client = new RestClient("https://api.digikey.com/");
            var request = new RestRequest("v1/oauth2/token", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("client_id", ConfigurationManager.AppSettings["DigikeyClient_id"]);
            request.AddParameter("client_secret", ConfigurationManager.AppSettings["DigikeyClient_secret"]);
            request.AddParameter("refresh_token", tokens.refresh_token);
            request.AddParameter("grant_type", "refresh_token");
            RestResponse response = client.Execute(request);

            RefreshTokens newTokens = JsonConvert.DeserializeObject<RefreshTokens>(response.Content);
            tokens.access_token = newTokens.access_token;
            tokens.refresh_token = newTokens.refresh_token;
            tokens.access_token_expire = newTokens.expires_in + ((int)((DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds));
            tokens.refresh_token_expire = newTokens.refresh_token_expires_in + ((int)((DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds));
            tokens.last_modified = ((int)((DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds));

            string tokensSerialized = JsonConvert.SerializeObject(tokens);
            File.WriteAllText(path, tokensSerialized);                    
        }

        public Response KeywordSearch(string partNumber)
        {
            var client = new RestClient("https://api.digikey.com/");
            var request = new RestRequest("Search/v3/Products/Keyword", Method.Post);
            request.AddHeader("X-DIGIKEY-Client-Id", ConfigurationManager.AppSettings["DigikeyClient_id"]);
            request.AddHeader("X-DIGIKEY-Locale-Site", ConfigurationManager.AppSettings["DigikeySiteLocation"]);
            request.AddHeader("X-DIGIKEY-Locale-Language", ConfigurationManager.AppSettings["DigikeyLanguage"]);
            request.AddHeader("X-DIGIKEY-Locale-Currency", ConfigurationManager.AppSettings["DigikeyCurrency"]);
            request.AddHeader("Authorization", "Bearer " + tokens.access_token);
            request.AddHeader("Content-Type", "application/json");
            var body = new SearchRequest();
            body.Keywords = partNumber;
            body.RecordCount = 10;
            
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            Response responseJson = JsonConvert.DeserializeObject<Response>(response.Content);
            return responseJson;
        }




        //public Results Response;

        public static void GetAuthorizationCode()
        {
            var client = new RestClient("https://api.digikey.com/");
            var request = new RestRequest("v1/oauth2/authorize", Method.Get);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            request.AddJsonBody(new
            {
                GetAuthorizationCode = new
                {
                    response_type = "code",
                    client_id = ConfigurationManager.AppSettings["DigikeyClient_id"],
                    redirect_uri = ConfigurationManager.AppSettings["DigikeyCallback_url"]
                }
            });

            RestResponse response = client.Execute(request);
            Uri test = response.ResponseUri;

            MessageBox.Show(response.ResponseUri.Query.ToString());
            MessageBox.Show("END");
            /*
            if (response.StatusCode == HttpStatusCode.OK)
            {

                Response = JsonConvert.DeserializeObject<Results>(response.Content);
            }*/

        }

    }
}
