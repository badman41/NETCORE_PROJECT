using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Model.InputTypes
{
    public class ProductInputType : InputObjectGraphType
    {
        public ProductInputType()
        {
            Name = "ProductInputType";
            Field<IntGraphType>("Id");
            Field<NonNullGraphType<StringGraphType>>("Name");
        }
    }
}
