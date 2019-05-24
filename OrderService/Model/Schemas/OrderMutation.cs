using GraphQL.Types;
using OrderService.Application.Interfaces;
using OrderService.Application.Request;
using OrderService.Model.InputTypes;
using OrderService.Model.ObjectTypes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Model.Schemas
{
    public class OrderMutation : ObjectGraphType<object>
    {
        public OrderMutation(IUnitAppService unitAppService)
        {
            Name = "OrderMuatation";

            #region Unit
            Field<AddNewUnitResponseType>(
                "addUnit",
                Description = "This field adds new product",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UnitInputType>> { Name = "unit" }
                ),
                resolve: context =>
                {
                    var unitInput = context.GetArgument<AddNewUnitRequest>(Name = "unit");
                    //return product.Create(productInput);
                    return unitAppService.addNewUnit(unitInput);
                });
            #endregion
        }


    }
}
