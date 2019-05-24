using GraphQL.Types;
using OrderService.Domain.ReadModels;
using System.Collections;

namespace OrderService.Model.ObjectTypes
{
    public class UnitType : ObjectGraphType<UnitModel>
    {
        public UnitType()
        {
            Name = "Unit";

            Field(h => h.ID, type: typeof(IntGraphType)).Description("The id of the Unit.");
            Field(h => h.Name, type: typeof(StringGraphType), nullable: true).Description("The name of the Unit.");
            Field(h => h.CreatedAt, nullable: true).Description("The name of the Unit.");
            Field(h => h.CreatedBy, nullable: true).Description("The name of the Unit.");
        }
    }
}
