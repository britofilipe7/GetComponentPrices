using GetComponentPrices.TME.Models.Search;
using GetComponentPrices.TME.Models.GetPricesAndStocks;
using Mouser.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using GetComponentPrices.TME.Models.GetDeliveryTime;

namespace GetComponentPrices.TME
{
    public class TME
    {
        private string partNumber;
        private int quantity;
        private double finalPrice = -1;
        private int moq = -1;
        private int stock = -1;
        private string leadTime = ""; //in weeks
        private bool haveAlternativePacking = false;
        private string productURL;
        private string symbolTME;

        private Models.GetPricesAndStocks.ProductList exactResult;

        public TME(string partNumber, int quantity)
        {
            this.PartNumber = partNumber;
            this.Quantity = quantity;
            Initialize();
        }

        //fill the fields
        private void Initialize()
        {   
            

            symbolTME = GetTMESymbol(PartNumber);
            if (symbolTME != null)
            {
                FindPrice();
                FindStock();
                FindLeadTime();
            }

            // Parsing whole .json from content returned by API
            // JObject json = JObject.Parse(jsonContent);
            // Console.WriteLine(json.ToString());
            // Console.WriteLine();

            // Another example with using objects
            //Console.WriteLine("Call api action: Products/GetParameters then parse .json and show result as list in format: \"ProductSymbol\"; \"ParameterName\"; \"ParameterValue\"");
            //Console.WriteLine();
        }

        private string GetTMESymbol(string partNumber)
        {
            //Build Parameters
            Dictionary<string, string> apiParams = new Dictionary<string, string>()
                {
                    { "Country", ConfigurationManager.AppSettings["TmeCountry"] },
                    { "Language", ConfigurationManager.AppSettings["TmeLang"]},
                    { "SearchPlain", partNumber },
                    { "Token", ConfigurationManager.AppSettings["TmeToken"] },
                };

            //Make request
            Request request = new Request();
            string jsonContent = request.Execute("Products/Search", apiParams);
            

            try
            {
                //Deserialize json
                Models.Search.SearchResponse response = JsonConvert.DeserializeObject<Models.Search.SearchResponse>(jsonContent);
                if (!response.Status.Equals("OK")) return null;

                //Loop over all products found and get the one that matches the manufacturer part number
                foreach (Models.Search.ProductList product in response.Data.ProductList)
                {
                    if (product.OriginalSymbol.Equals(partNumber))
                    {
                        ProductURL = product.ProductInformationPage;
                        SymbolTME = product.Symbol;
                        return product.Symbol;
                    }
                }
                return null;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
        }

        private void FindPrice()
        {
            //Build Parameters
            Dictionary<string, string> apiParams = new Dictionary<string, string>()
                {
                    { "Country", ConfigurationManager.AppSettings["TmeCountry"]},
                    { "Currency", ConfigurationManager.AppSettings["TmeCurr"]},
                    { "Language", ConfigurationManager.AppSettings["TmeLang"]},
                    { "SymbolList[0]", symbolTME},
                    { "Token", ConfigurationManager.AppSettings["TmeToken"] },
                };


            //Make request
            Request request = new Request();
            string jsonContent = request.Execute("Products/GetPricesAndStocks", apiParams);

            try
            {
                //Deserialize json
                Models.GetPricesAndStocks.SearchResponse response = JsonConvert.DeserializeObject<Models.GetPricesAndStocks.SearchResponse>(jsonContent);

                ExactResult = response.Data.ProductList[0];

                if (!response.Status.Equals("OK")) return;

                //Sets all price breaks into an array
                PriceList[] priceBreaks = ExactResult.PriceList.ToArray();
                //loop each price break
                foreach (PriceList priceBreak in priceBreaks)
                {
                    //check if the quantity is lower than the first price break, if true set the first price break and set a moq
                    if (priceBreak.Amount > quantity && Array.IndexOf(priceBreaks, priceBreak) == 0)
                    {
                        //Uses substring to remove the currency sign at the end
                        FinalPrice = priceBreaks[0].PriceValue;
                        Moq = priceBreaks[0].Amount;
                        return;
                    }
                    //check if the quantity is lower than the actual price break, if true get the price from the previous price break
                    if (priceBreak.Amount > quantity)
                    {
                        int actualIndex = Array.IndexOf(priceBreaks, priceBreak);
                        FinalPrice = priceBreaks[actualIndex - 1].PriceValue;
                        return;
                    }
                }
                //check if the quantity is higher than the last price break, meaning FinalPrice is not changed
                if (FinalPrice == -1)
                {
                    FinalPrice = priceBreaks[priceBreaks.Length - 1].PriceValue;
                }
            } 
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            
        }

        private void FindStock()
        {
            if (exactResult != null) Stock = ExactResult.Amount;
        }
        private void FindLeadTime()
        {
            //Build Parameters
            Dictionary<string, string> apiParams = new Dictionary<string, string>()
                {
                    { "AmountList[0]", Quantity.ToString()},
                    { "Country", ConfigurationManager.AppSettings["TmeCountry"]},
                    { "Language", ConfigurationManager.AppSettings["TmeLang"]},
                    { "SymbolList[0]", SymbolTME},
                    { "Token", ConfigurationManager.AppSettings["TmeToken"] },
                };

            //Make request
            Request request = new Request();
            string jsonContent = request.Execute("Products/GetDeliveryTime", apiParams);

            try
            {
                //Deserialize json
                Models.GetDeliveryTime.SearchResponse response = JsonConvert.DeserializeObject<Models.GetDeliveryTime.SearchResponse>(jsonContent);

                List<DeliveryList> deliveryList = response.Data.ProductList[0].DeliveryList;
                foreach (DeliveryList delivery in deliveryList)
                {
                    if (!delivery.Status.Equals("DS_AVAILABLE_IN_STOCK"))
                    {
                        //Could be improved
                        if (!delivery.Week.Equals("null")) LeadTime = delivery.Week;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                
            }
            
        }

        public string PartNumber { get => partNumber; set => partNumber = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public double FinalPrice { get => finalPrice; set => finalPrice = value; }
        public int Moq { get => moq; set => moq = value; }
        public int Stock { get => stock; set => stock = value; }
        public string LeadTime { get => leadTime; set => leadTime = value; }
        public bool HaveAlternativePacking { get => haveAlternativePacking; set => haveAlternativePacking = value; }
        public string ProductURL { get => productURL; set => productURL = value; }
        public string SymbolTME { get => symbolTME; set => symbolTME = value; }
        internal Models.GetPricesAndStocks.ProductList ExactResult { get => exactResult; set => exactResult = value; }
    }
}
