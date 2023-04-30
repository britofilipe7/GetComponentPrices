using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetComponentPrices.Digikey.Models
{
    internal class Product
    {
        public List<StandardPricing> StandardPricing { get; set; }
        public string RoHSStatus { get; set; }
        public string LeadStatus { get; set; }
        public string ProductUrl { get; set; }
        public string PrimaryDatasheet { get; set; }
        public string PrimaryPhoto { get; set; }
        public string PrimaryVideo { get; set; }
        public Series Series { get; set; }
        public string ManufacturerLeadWeeks { get; set; }
        public string ManufacturerPageUrl { get; set; }
        public string ProductStatus { get; set; }
        public List<object> AlternatePackaging { get; set; }
        public string DetailedDescription { get; set; }
        public string ReachStatus { get; set; }
        public string ExportControlClassNumber { get; set; }
        public string HTSUSCode { get; set; }
        public string TariffDescription { get; set; }
        public string MoistureSensitivityLevel { get; set; }
        public Family Family { get; set; }
        public Category Category { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public int MinimumOrderQuantity { get; set; }
        public bool NonStock { get; set; }
        public Packaging Packaging { get; set; }
        public int QuantityAvailable { get; set; }
        public string DigiKeyPartNumber { get; set; }
        public string ProductDescription { get; set; }
        public double UnitPrice { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public int ManufacturerPublicQuantity { get; set; }
        public int QuantityOnOrder { get; set; }
        public int MaxQuantityForDistribution { get; set; }
        public bool BackOrderNotAllowed { get; set; }
        public bool DKPlusRestriction { get; set; }
        public bool Marketplace { get; set; }
        public bool SupplierDirectShip { get; set; }
        public string PimProductName { get; set; }
        public string Supplier { get; set; }
        public int SupplierId { get; set; }
        public bool IsNcnr { get; set; }
    }
}
