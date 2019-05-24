using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.ReadModels
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public decimal WeightPerUnit { get; set; }
        public int UnitId { get; set; }
        public UnitModel Unit { get; set; }
        public IEnumerable<ProductUnitModel> OtherUnitOfProduct { get; set; }
        public int PreservationId { get; set; }
        public PreservationModel Preservation { get; set; }
    }
}
