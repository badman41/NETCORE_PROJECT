using GraphQL.Types;
using OrderService.Application.Interfaces;
using OrderService.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Model.ObjectTypes
{
    public class GetAllUnitResponsType : ObjectGraphType<GetAllUnitResponse>
    {
        public GetAllUnitResponsType(IUnitAppService unitAppService)
        {
            
            //Field(h => h.PageSize, type: typeof(IntGraphType)).Description("The Page Size of the response.");
            //Field(h => h.PageNumber, type: typeof(IntGraphType)).Description("The Page Number of the response.");

            Field<ListGraphType<UnitType>>(
                "units",
                resolve: context =>
                {
                    return unitAppService.getAllUnit();
                }
            );
        }
    }
}
