using GraphQL.Types;
using OrderService.Domain.Shared.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Model
{
    public class NameType : ObjectGraphType<Name>
    {
        public NameType()
        {
            Name = "Name";

            Field(h => h.Value).Description("request of name.");
        }
    }
}
