using MassTransit;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Application.Shared;
using OrderService.Domain.Bus;
using OrderService.Domain.Commands.Preservation;
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
    public class PreservationAppService : IPreservationAppService
    {
        private readonly IPreservationService _service;
        private readonly IMediatorHandler Bus;
        private readonly IBus RabbitMQBus;
        private readonly DomainNotificationHandler _notifications;

        public PreservationAppService(IPreservationService PreservationService, IMediatorHandler bus, IBus rabbitMQBus, INotificationHandler<DomainNotification> notifications)
        {
            _service = PreservationService;
            Bus = bus;
            _notifications = (DomainNotificationHandler)notifications;
            RabbitMQBus = rabbitMQBus;

        }

        public Task<GetAllPreservationResponse> getAllPreservation()
        {
            GetAllPreservationResponse response = new GetAllPreservationResponse();
            try
            {
                response.Data = _service.All();
                response.Status = new StatusResponse("",true);
                if(response.Data != null)
                {
                    response.Total = response.Data.Count();
                }
            }
            catch(Exception e)
            {
                response.Status = new StatusResponse("", false);
            }
            return Task.FromResult(response);
        }

        public Task<AddNewPreservationResponse> addNewPreservation(AddNewPreservationRequest request)
        {
            AddNewPreservationCommand command = new AddNewPreservationCommand(request.Description);
            Task<object> Preservation = (Task<object>) Bus.SendCommand(command);
            //RabbitMQBus.Publish(command);
            AddNewPreservationResponse response = new AddNewPreservationResponse();
            response = Common<AddNewPreservationResponse>.checkHasNotification(_notifications, response);
            response.OK = false;
            if (response.Success)
            {
                PreservationModel PreservationModel = (PreservationModel)Preservation.Result;
                response.Content = PreservationModel.ID;
                response.OK = true;
            }
            return Task.FromResult(response);
        }

        public Task<DeletePreservationResponse> deletePreservation(DeletePreservationRequest request)
        {
            DeletePreservationCommand command = new DeletePreservationCommand(request.Id);
            Task<object> Preservation = (Task<object>)Bus.SendCommand(command);
            //RabbitMQBus.Publish(command);
            DeletePreservationResponse response = new DeletePreservationResponse();
            response = Common<DeletePreservationResponse>.checkHasNotification(_notifications, response);
            return Task.FromResult(response);
        }
    }
}
