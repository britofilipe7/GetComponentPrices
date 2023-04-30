using GetComponentPrices.Farnell.Models;
using Mouser.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GetComponentPrices.Farnell
{
    internal class Farnell
    {
        private string partNumber;
        private int quantity;
        private double finalPrice = -1;
        private int moq = -1;
        private int stock;
        private int leadTime; //in weeks
        private bool haveAlternativePacking = false;
        private string datasheetURL;
        private string productURL;
        private Product exactResult;

        public Farnell(string partNumber, int quantity)
        {
            this.partNumber = partNumber;
            this.quantity = quantity;
            Initialize();
        }

        //fill the fields
        private void Initialize()
        {
            Request search = new Request(partNumber);

            int count = 0;
            //Set exactResult as the FarnellPart that his manufacturer part number match the specified part number
            foreach (Product product in search.Response.keywordSearchReturn.products)
            {
                //Remove "-" from the partNumber
                if (product.translatedManufacturerPartNumber.Replace("-", "").Equals(partNumber.Replace("-", "")))
                {
                    count++;
                    ExactResult = product;
                }
            }

            if (ExactResult != null && ExactResult.prices.Count != 0)
            {

                if (count > 1) HaveAlternativePacking = true;
                FindPrice();
                FindStock();
                FindLeadTime();
                FindDatasheetURL();
                FindProductURL();
            }

        }

        private void FindPrice()
        {
            //Sets all price breaks into an array
            Price[] priceBreaks = ExactResult.prices.ToArray();
            //loop each price break
            foreach (Price priceBreak in priceBreaks)
            {

                //check if the quantity is lower than the first price break, if true set the first price break and set a moq
                if (priceBreak.from > quantity && Array.IndexOf(priceBreaks, priceBreak) == 0)
                {
                    finalPrice = priceBreaks[0].cost;
                    Moq = priceBreaks[0].from;
                    return;
                }
                //check if the quantity value is in the middle of "from" and "to" values
                if (priceBreak.from <= quantity && priceBreak.to >= quantity)
                {
                    finalPrice = priceBreak.cost;
                    return;
                }
            }
            //check if the quantity is higher than the last price break, meaning FinalPrice is not changed
            if (FinalPrice == -1)
            {
                finalPrice = priceBreaks[priceBreaks.Length - 1].cost;
            }
        }

        private void FindStock()
        {
            Stock = ExactResult.stock.level;
        }
        private void FindLeadTime()
        {
            LeadTime = (ExactResult.stock.leastLeadTime % 365) / 7;
        }

        private void FindDatasheetURL()
        {
            DatasheetURL = ExactResult.datasheets[0].url;
        }

        private void FindProductURL()
        {
            string sku = ExactResult.sku;
            productURL = ConfigurationManager.AppSettings["FarnellStore"] + "/" +sku;
        }

        public double FinalPrice { get => finalPrice; set => finalPrice = value; }
        public int Moq { get => moq; set => moq = value; }
        public int Stock { get => stock; set => stock = value; }
        public int LeadTime { get => leadTime; set => leadTime = value; }
        public bool HaveAlternativePacking { get => haveAlternativePacking; set => haveAlternativePacking = value; }
        public string DatasheetURL { get => datasheetURL; set => datasheetURL = value; }
        public string ProductURL { get => productURL; set => productURL = value; }
        public Product ExactResult { get => exactResult; set => exactResult = value; }
    }
}
