using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Interfaces
{
    public interface IPreservationAppService
    {
        Task<GetAllPreservationResponse> getAllPreservation();
        Task<AddNewPreservationResponse> addNewPreservation(AddNewPreservationRequest request);
        Task<DeletePreservationResponse> deletePreservation(DeletePreservationRequest request);
    }
}
