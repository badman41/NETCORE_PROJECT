using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.ReadModels
{
    public class ProductUnitModel
    {
        public int ID { get; set; }
        public int ProductId { get; set; }
        public int UnitId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public decimal WPU { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyAt { get; set; }
        public ProductUnitModel() { }
    }
}
