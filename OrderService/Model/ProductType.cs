using GraphQL.Types;
using OrderService.Domain.Entities;
using OrderService.Model.ObjectTypes;
using System.Collections.Generic;

namespace OrderService.Model
{
    public class ProductType : ObjectGraphType<Product>
    {
        public ProductType()
        {
            Name = "Product";

            Field(h => h.Id, type: typeof(IdentityType)).Description("The id of the Product.");
            Field(h => h.Name, type: typeof(NameType), nullable: true).Description("The name of the Product.");

            Field<ListGraphType<UnitType>>(
                "ListUnit",
                resolve: context =>
                {
                    List<Unit> lst = new List<Unit>();
                    for (uint i = 0; i < 20; i++)
                    {
                        Unit u = new Unit(new Domain.Shared.ValueObject.Identity(i), new Domain.Shared.ValueObject.Name("Unit " + i));
                        lst.Add(u);
                    }
                    return lst;
                }
            );
        }
    }
}
