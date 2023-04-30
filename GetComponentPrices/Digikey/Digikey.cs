using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms;
using GetComponentPrices.Digikey.Models;
using Mouser.Models;
using System.Text.RegularExpressions;

namespace GetComponentPrices.Digikey
{
    internal class Digikey
    {
        private string partNumber;
        private int quantity;

        private double finalPrice = -1;
        private int moq = -1;
        private int stock;
        private int leadTime; //in weeks
        private bool haveAlternativePacking = false;
        private string datasheetURL;
        private string imageURL;
        private string productURL;
        private Product exactResult;

        public Digikey(string partNumber, int quantity)
        {
            this.partNumber = partNumber;
            this.quantity = quantity;
            this.Initialize();
        }

        //fill the fields
        public void Initialize()
        {
            Request request = new Request();
            Response response = request.KeywordSearch(partNumber);
            if (response.ExactManufacturerProducts.Count == 1) exactResult = response.ExactManufacturerProducts[0];
            else
            {
                foreach(Product product in response.Products)
                {
                    //special trim due to Molex partNumbers, for example "500798000" in digikey are represented as "0500798000"
                    string trimmed = product.ManufacturerPartNumber.TrimStart('0');
                    if (product.ManufacturerPartNumber.Equals(partNumber) || trimmed.Equals(partNumber.TrimStart('0')))
                    {
                        exactResult = product;
                    }
                }
            }

            if (exactResult != null)
            {

                if (ExactResult.AlternatePackaging.Count > 0) HaveAlternativePacking = true;
                FindPrice();
                FindStock();
                FindLeadTime();
                FindDatasheetURL();
                FindImageURL();
                FindProductURL();
            }
        }

        private void FindPrice()
        {
            //Sets all price breaks into an array
            StandardPricing[] priceBreaks = exactResult.StandardPricing.ToArray();
            //loop each price break
            foreach (StandardPricing priceBreak in priceBreaks)
            {
                //check if the quantity is lower than the first price break, if true set the first price break and set a moq
                if (priceBreak.BreakQuantity > quantity && Array.IndexOf(priceBreaks, priceBreak) == 0)
                {
                    //Uses substring to remove the currency sign at the end
                    finalPrice = priceBreaks[0].UnitPrice;
                    moq = priceBreaks[0].BreakQuantity;
                    return;
                }
                //check if the quantity is lower than the actual price break, if true get the price from the previous price break
                if (priceBreak.BreakQuantity > quantity)
                {
                    int actualIndex = Array.IndexOf(priceBreaks, priceBreak);
                    finalPrice = priceBreaks[actualIndex - 1].UnitPrice;
                    return;
                }
            }
            //check if the quantity is higher than the last price break, meaning FinalPrice is not changed
            if (finalPrice == -1)
            {
                finalPrice = priceBreaks[priceBreaks.Length - 1].UnitPrice;
            }
        }

        private void FindStock()
        {
            stock = exactResult.QuantityAvailable;
        }

        private void FindLeadTime()
        {
            string leadTimeFull = exactResult.ManufacturerLeadWeeks;
            //gets only the numbers
            string leadTimeNumbers = Regex.Match(leadTimeFull, @"\d+").Value;
            if (!leadTimeNumbers.Equals("")) leadTime = Convert.ToInt32(leadTimeNumbers);
        }

        private void FindDatasheetURL()
        {
            datasheetURL = exactResult.PrimaryDatasheet;
        }

        private void FindImageURL()
        {
            imageURL = exactResult.PrimaryPhoto;
        }

        private void FindProductURL()
        {
            productURL = exactResult.ProductUrl;
        }

        public double FinalPrice { get => finalPrice; set => finalPrice = value; }
        public int Moq { get => moq; set => moq = value; }
        public int Stock { get => stock; set => stock = value; }
        public int LeadTime { get => leadTime; set => leadTime = value; }
        public string DatasheetURL { get => datasheetURL; set => datasheetURL = value; }
        public string ImageURL { get => imageURL; set => imageURL = value; }
        public string ProductURL { get => productURL; set => productURL = value; }
        internal Product ExactResult { get => exactResult; set => exactResult = value; }
        public bool HaveAlternativePacking { get => haveAlternativePacking; set => haveAlternativePacking = value; }
    }
}
