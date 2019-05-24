using MassTransit;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Application.Shared;
using OrderService.Domain.Bus;
using OrderService.Domain.Commands.Unit;
using OrderService.Domain.Interface.Service;
using OrderService.Domain.Notifications;
using OrderService.Domain.ReadModels;
using OrderService.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using TransitionApp.Models.VehicleXXX;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;

namespace OrderService.Application.Implements
{
    public class UnitAppService : IUnitAppService
    {
        private readonly IUnitService _service;
        private readonly IMediatorHandler Bus;
        private readonly IBus RabbitMQBus;
        private readonly DomainNotificationHandler _notifications;

        public UnitAppService(IUnitService unitService, IMediatorHandler bus, IBus rabbitMQBus, INotificationHandler<DomainNotification> notifications)
        {
            _service = unitService;
            Bus = bus;
            _notifications = (DomainNotificationHandler)notifications;
            RabbitMQBus = rabbitMQBus;

        }

        public Task<GetAllUnitResponse> getAllUnit()
        {
            GetAllUnitResponse response = new GetAllUnitResponse();
            try
            {
                response.Data = _service.All();
                response.Message = "";
                response.Success = true;
                if(response.Data != null)
                {
                    response.Total = response.Data.Count();
                }
            }
            catch(Exception e)
            {
                response.Message = e.ToString();
                response.Success = false ;
            }
            return Task.FromResult(response);
        }

        public Task<AddNewUnitResponse> addNewUnit(AddNewUnitRequest request)
        {
            AddNewUnitCommand command = new AddNewUnitCommand(request.ProductUnit.Name);
            Task<object> unit = (Task<object>) Bus.SendCommand(command);
            //RabbitMQBus.Publish(command);
            AddNewUnitResponse response = new AddNewUnitResponse();
            response = Common<AddNewUnitResponse>.checkHasNotification(_notifications, response);
            if (response.Success)
            {
                UnitModel unitModel = (UnitModel)unit.Result;
                response.Data = unitModel.ID;
            }
            return Task.FromResult(response);
        }

        public Task<DeleteUnitResponse> deleteUnit(DeleteUnitRequest request)
        {
            DeleteUnitCommand command = new DeleteUnitCommand(request.Id);
            Task<object> unit = (Task<object>)Bus.SendCommand(command);
            //RabbitMQBus.Publish(command);
            DeleteUnitResponse response = new DeleteUnitResponse();
            response = Common<DeleteUnitResponse>.checkHasNotification(_notifications, response);
            return Task.FromResult(response);
        }
    }
}
