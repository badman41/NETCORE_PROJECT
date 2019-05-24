using GraphQL.Types;
using OrderService.Application.Interfaces;
using OrderService.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Model.ObjectTypes
{
    public class AddNewUnitResponseType : ObjectGraphType<AddNewUnitResponse>
    {
        public AddNewUnitResponseType(IUnitAppService unitAppService)
        {
            Field(h => h.Message);
            Field(h => h.Status);
            //Field(h => h.Result, type: typeof(UnitType));
        }
    }
}
