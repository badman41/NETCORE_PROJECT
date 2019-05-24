using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.ReadModels
{
    public class QuotationItemModel
    {
        public int Id { get; set; }
        public int QuotationId { get; set; }
        public int ProductUnitId { get; set; }
        public int Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        //Unit
        public int UnitID { get; set; }
        public string UnitName { get; set; }

    }
}
