using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mouser.Models
{
    public class MouserPart
    {
        public string Availability { get; set; }
        public string DataSheetUrl { get; set; }
        public string Description { get; set; }
        public string FactoryStock { get; set; }
        public string ImagePath { get; set; }
        public string Category { get; set; }
        public string LeadTime { get; set; }
        public string LifecycleStatus { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public string Min { get; set; }
        public string Mult { get; set; }
        public string MouserPartNumber { get; set; }
        public List<ProductAttribute> ProductAttributes { get; set; }
        public List<PriceBreak> PriceBreaks { get; set; }
        public List<AlternatePackaging> AlternatePackagings { get; set; }
        public string ProductDetailUrl { get; set; }
        public bool Reeling { get; set; }
        public string ROHSStatus { get; set; }
        public string SuggestedReplacement { get; set; }
        public int MultiSimBlue { get; set; }
        public UnitWeightKgEntity UnitWeightKg { get; set; }
        public StandardCostEntity StandardCost { get; set;}
        public string IsDiscontinued { get; set; }
        public string RTM { get; set; }
        public string MouserProductCategory { get; set; }
        public string IPCCode { get; set; }
        public string SField { get; set; }
        public string VNum { get; set; }
        public string ActualMfrName { get; set; }
        public string AvailableOnOrder { get; set; }
        public List<string> InfoMessages { get; set; }
        public string RestrictionMessage { get; set; }
        public string PID { get; set; }
        public List<ProductComplianceEntity> ProductCompliance { get; set; }

    }
}
