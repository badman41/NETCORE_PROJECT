using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Commands.Product
{
    public abstract class ProductCommand: BaseCommand
    {
        public int Id { get; protected set; }
        public string Name { get; protected set; }
        public string Note { get; protected set; }
        public string Code { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }

        //Product Unit
        public IEnumerable<ProductUnitModel> ProductUnitModels { get; set; }
        public int CommonUnitId { get; set; }
        public decimal WeightPerUnit { get; set; }

        public int PreservationId { get; set; }
    }
}
