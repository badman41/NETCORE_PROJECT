using OrderService.Domain.Shared;
using OrderService.Domain.Shared.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Entities
{
    public class Product : IEntity<Product>
    {
        public Identity Id { get; }
        public Name Name { get; }
        public Code Code { get; }
        public Note Note { get; }
        public Dictionary<Unit,decimal> OtherUnitOfProduct { get; }
        public Unit CommonUnit { get; }
        public Preservation Preservation { get; }
        public WeightPerUnit WeightPerUnit { get; }
        public Product(Identity id, Name name,Code code, Note note, Dictionary<Unit, decimal> otherUnitOfProduct, Unit commonUnit, WeightPerUnit weightPerUnit, Preservation preservation)
        {
            Id = id;
            Name = name;
            OtherUnitOfProduct = otherUnitOfProduct;
            CommonUnit = commonUnit;
            Code = code;
            Note = note;
            WeightPerUnit = weightPerUnit;
            Preservation = preservation;
        }
        public bool sameIdentityAs(Product other)
        {
            return Id.Equals(Id);
        }
    }
}
