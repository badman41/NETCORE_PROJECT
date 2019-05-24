﻿using OrderService.Domain.Entities;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.Interface.Service;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Infra.Data.Respository.Services
{
    public class PreservationService : BaseService<PreservationModel, Preservation>, IPreservationService
    {
        public PreservationService(IPreservationRepository repository)
            : base(repository)
        {
        }
        
    }
}
