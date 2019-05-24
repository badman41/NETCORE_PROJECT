using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;
using System.Collections.Generic;

namespace OrderService.Domain.Interface.Respository
{
    public interface IPreservationRepository : IRepository<PreservationModel, Entities.Preservation>
    {
       
    }
}
