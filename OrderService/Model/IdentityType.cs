using GraphQL.Types;
using OrderService.Domain.Shared.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Model
{
    public class IdentityType : ObjectGraphType<Identity>
    {
        public IdentityType()
        {
            Name = "Identity";

            Field(h => h.Value, type: typeof(UIntGraphType), nullable: true).Description("request of Identity");
        }
    }
}
