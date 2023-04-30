using Mouser.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetComponentPrices.Mouser
{
    public class Mouser
    {  
        private string partNumber;
        private int quantity;
        private float finalPrice = -1;
        private int moq = -1;
        private string stock = "";
        private int leadTime; //in weeks
        private bool haveAlternativePacking = false;
        private string datasheetURL;
        private string imageURL;
        private string productURL;
        private MouserPart exactResult;

        public Mouser (string partNumber, int quantity)
        {
            this.partNumber = partNumber;
            this.quantity = quantity;
            Initialize();
        }

        //fill the fields
        private void Initialize()
        {
            Request search = new Request(partNumber);
           
            //Set exactResult as the MouserPart that his manufacturer part number match the specified part number
            foreach (MouserPart part in search.Response.SearchResults.Parts)
            {   
                //Remove "-" from the partNumber
                if (part.ManufacturerPartNumber.Replace("-", "").Equals(partNumber))
                {
                    ExactResult = part;
                }
            }

            if (ExactResult != null && ExactResult.PriceBreaks.Count != 0)
            {

                if (ExactResult.AlternatePackagings != null) HaveAlternativePacking = true;
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
            PriceBreak[] priceBreaks = ExactResult.PriceBreaks.ToArray();
            //loop each price break
            foreach (PriceBreak priceBreak in priceBreaks)
            {
                //check if the quantity is lower than the first price break, if true set the first price break and set a moq
                if (priceBreak.Quantity > quantity && Array.IndexOf(priceBreaks, priceBreak) == 0)
                {
                    //Uses substring to remove the currency sign at the end
                    string priceStr = priceBreaks[0].Price.Substring(0, priceBreaks[0].Price.Length - 2);
                    finalPrice = float.Parse(priceStr);
                    Moq = priceBreaks[0].Quantity;
                    return;
                }
                //check if the quantity is lower than the actual price break, if true get the price from the previous price break
                if (priceBreak.Quantity > quantity)
                {
                    int actualIndex = Array.IndexOf(priceBreaks, priceBreak);
                    string priceStr = priceBreaks[actualIndex - 1].Price.Substring(0, priceBreaks[Array.IndexOf(priceBreaks, priceBreak) - 1].Price.Length - 2);
                    finalPrice = float.Parse(priceStr);
                    return;
                }
            }
            //check if the quantity is higher than the last price break, meaning FinalPrice is not changed
            if (FinalPrice == -1)
            {
                string priceStr = priceBreaks[priceBreaks.Length - 1].Price.Substring(0, priceBreaks[priceBreaks.Length - 1].Price.Length - 2);
                finalPrice = float.Parse(priceStr);
            }
        }

        private void FindStock()
        {
            string availability = ExactResult.Availability;
            //gets only the numbers
            Stock = Regex.Match(availability, @"\d+").Value;            
        }
        private void FindLeadTime()
        {
            string leadTimeFull = ExactResult.LeadTime;
            //gets only the numbers
            int leadTimeDays = Convert.ToInt32(Regex.Match(leadTimeFull, @"\d+").Value);
            LeadTime = (leadTimeDays % 365) / 7;
        }

        private void FindDatasheetURL()
        {
            DatasheetURL = ExactResult.DataSheetUrl;
        }

        private void FindImageURL()
        {
            ImageURL = ExactResult.ImagePath;
        }

        private void FindProductURL()
        {
            productURL = ExactResult.ProductDetailUrl;
        }

        public float FinalPrice { get => finalPrice; set => finalPrice = value; }
        public int Moq { get => moq; set => moq = value; }
        public string Stock { get => stock; set => stock = value; }
        public int LeadTime { get => leadTime; set => leadTime = value; }
        public bool HaveAlternativePacking { get => haveAlternativePacking; set => haveAlternativePacking = value; }
        public string DatasheetURL { get => datasheetURL; set => datasheetURL = value; }
        public string ImageURL { get => imageURL; set => imageURL = value; }
        public string ProductURL { get => productURL; set => productURL = value; }
        public MouserPart ExactResult { get => exactResult; set => exactResult = value; }
    }

}
