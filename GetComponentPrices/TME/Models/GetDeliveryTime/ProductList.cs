using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetComponentPrices.TME.Models.GetDeliveryTime
{
    internal class ProductList
    {
        public string Symbol { get; set; }
        public List<DeliveryList> DeliveryList { get; set; }
    }
}
