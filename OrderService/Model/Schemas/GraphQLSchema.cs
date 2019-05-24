using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Model.Schemas
{
    public class GraphQLSchema : Schema
    {
        public GraphQLSchema(Func<Type, GraphType> resolveType): base(resolveType)
        {
            Query = (OrderQuery)resolveType(typeof(OrderQuery));
            Mutation = (OrderMutation)resolveType(typeof(OrderMutation));
        }
    }
}
