using GraphQL.Types;
using OrderService.Application.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Model.InputTypes
{
    public class UnitInputType : InputObjectGraphType<AddNewUnitRequest>
    {
        public UnitInputType()
        {
            Name = "UnitInputType";
            //Field("Name", x => x.Name, nullable: true);
                //<StringGraphType>("Name");
        }
    }
}
