using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Interfaces
{
    public interface IUnitAppService
    {
        //Task<GetAllUnitResponse> getAllUnitResponse(int pageNumber = 1, int pageSize = 10);
        Task<GetAllUnitResponse> getAllUnit();
        Task<AddNewUnitResponse> addNewUnit(AddNewUnitRequest request);
        Task<DeleteUnitResponse> deleteUnit(DeleteUnitRequest request);
    }
}
