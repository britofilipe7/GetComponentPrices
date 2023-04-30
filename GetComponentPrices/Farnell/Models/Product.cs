using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetComponentPrices.Farnell.Models
{
    internal class Product
    {
        public string sku { get; set; }
        public string displayName { get; set; }
        public string productStatus { get; set; }
        public string rohsStatusCode { get; set; }
        public int packSize { get; set; }
        public string unitOfMeasure { get; set; }
        public string id { get; set; }
        public Image image { get; set; }
        public List<Datasheet> datasheets { get; set; }
        public List<Price> prices { get; set; }
        public int inv { get; set; }
        public string vendorId { get; set; }
        public string vendorName { get; set; }
        public string brandName { get; set; }
        public string translatedManufacturerPartNumber { get; set; }
        public int translatedMinimumOrderQuality { get; set; }
        public List<Attribute> attributes { get; set; }
        public Related related { get; set; }
        public Stock stock { get; set; }
        public string countryOfOrigin { get; set; }
        public bool comingSoon { get; set; }
        public int inventoryCode { get; set; }
        public object nationalClassCode { get; set; }
        public object publishingModule { get; set; }
        public string vatHandlingCode { get; set; }
        public int releaseStatusCode { get; set; }
        public bool isSpecialOrder { get; set; }
        public bool isAwaitingRelease { get; set; }
        public bool reeling { get; set; }
        public int discountReason { get; set; }
        public string brandId { get; set; }
        public string commodityClassCode { get; set; }
    }
}
