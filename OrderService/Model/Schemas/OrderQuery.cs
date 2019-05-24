using GraphQL.Types;
using OrderService.Application.Implements;
using OrderService.Application.Interfaces;
using OrderService.Application.Response;
using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;
using OrderService.Domain.Shared.ValueObject;
using OrderService.Model.ObjectTypes;
using System.Collections.Generic;

namespace OrderService.Model.Schemas
{
    public class OrderQuery : ObjectGraphType
    {

        public OrderQuery(IUnitAppService unitAppService)
        {

            Name = "Query";

            Field<GetAllUnitResponsType>(
                "getAllUnits",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "pageSize", Description = "pageSize" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "pageNumber", Description = "pageNumber" }
                ),
                resolve: context =>
                {
                    var pageSize = context.GetArgument<int>("pageSize");
                    var pageNumber = context.GetArgument<int>("pageNumber");
                    return unitAppService.getAllUnit();
                }
            );
            Field<ListGraphType<UnitType>>(
                "getAllUnitsa",
                //arguments: new QueryArguments(
                //    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the unit" }
                //),
                resolve: context =>
                {
                    return unitAppService.getAllUnit();
                }
            );
            //Field<ListGraphType<UnitType>>(
            //    "getUnitById",
            //    //arguments: new QueryArguments(
            //    //    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the unit" }
            //    //),
            //    resolve: context =>
            //    {
            //        var id = context.GetArgument<int>("id");
            //        Request r = new Request(1,2,3,4.)
            //        Appservice.search(r)
            //        UnitResponse u = new Unit(1, new Name("Unit " + 1));
            //        return u;
            //    }
            //);
            //Field<ListGraphType<ProductType>>(
            //    "getAllUnits",
            //    Description = "This field returns all units",
            //    resolve: context =>
            //    {
            //        List<Unit> lst = new List<Unit>();
            //        for (uint i = 0; i < 20; i++)
            //        {
            //            Unit u = new Unit(new Identity(i), new Name("Unit " + i));
            //            lst.Add(u);
            //        }
            //        return lst;
            //    }
            //);
            Field<ProductType>(
                "product",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the product" }
                ),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    List<Identity> lst = new List<Identity>();
                    for (uint i = 0; i < 20; i++)
                    {
                        lst.Add(i);
                    }
                    //Product u = new Product((uint)id, new Name("Product " + id), lst);
                    return new object();
                }
            );
        }
    }
}
