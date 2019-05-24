using MediatR;
using Newtonsoft.Json;
using OrderService.Domain.Bus;
using OrderService.Domain.CommandHandlers;
using OrderService.Domain.Commands.Product;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.Notifications;
using OrderService.Domain.ReadModels;
using OrderService.Domain.Shared.ValueObject;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Domain.Product.CommandHandlers
{
    public class ProductCommandhandler :   BaseCommandHandler
                                        , IRequestHandler<AddNewProductCommand, object>
                                        , IRequestHandler<UpdateProductCommand, object>
                                        //, IRequestHandler<DeleteProductCommand, object>
    {

        private readonly IProductRepository _ProductRepository;
        private readonly IAddressRepository _AddressRepository;
        private readonly IMediatorHandler _bus;

        public ProductCommandhandler(IProductRepository ProductRepository,
                                      IAddressRepository AddressRepository,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(bus, notifications)
        {
            _ProductRepository = ProductRepository;
            _AddressRepository = AddressRepository;
            _bus = bus;
        }

        public Task<object> Handle(AddNewProductCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
            }
            else
            {
                Dictionary<Entities.Unit, decimal> otherUnits = new Dictionary<Entities.Unit, decimal>();
                foreach (ProductUnitModel productUnitModel in command.ProductUnitModels)
                {
                    Entities.Unit unit = new Entities.Unit(new Identity((uint)productUnitModel.ID), null);
                    otherUnits.Add(unit, productUnitModel.WPU);
                }
                Entities.Product product = new Entities.Product
                (
                    null,
                    new Name(command.Name),
                    new Code(command.Code),
                    new Note(command.Note),
                    otherUnits,
                    new Entities.Unit(new Identity((uint)command.CommonUnitId), null),
                    command.WeightPerUnit,
                    new Entities.Preservation((uint)command.PreservationId, null)
                );
                ProductModel model = _ProductRepository.Add(product);
                if (model != null)
                {
                    return Task.FromResult(model as object);
                }
                _bus.RaiseEvent(new DomainNotification("Product", "Server error", NotificationCode.Error));

            }
            return Task.FromResult(null as object);
        }

        public Task<object> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
            }
            else
            {
                Dictionary<Entities.Unit, decimal> otherUnits = new Dictionary<Entities.Unit, decimal>();
                foreach (ProductUnitModel productUnitModel in command.ProductUnitModels)
                {
                    Entities.Unit unit = new Entities.Unit(new Identity((uint)productUnitModel.ID), null);
                    otherUnits.Add(unit, productUnitModel.WPU);
                }
                Entities.Product product = new Entities.Product
                (
                    new Identity((uint)command.Id),
                    new Name(command.Name),
                    new Code(command.Code),
                    new Note(command.Note),
                    otherUnits,
                    new Entities.Unit(new Identity((uint)command.CommonUnitId), null),
                    command.WeightPerUnit,
                    new Entities.Preservation((uint)command.PreservationId, null)
                );
                bool result = _ProductRepository.Update(product);
                if (!result)
                {
                    _bus.RaiseEvent(new DomainNotification("Product", "Server error", NotificationCode.Error));
                }
                return Task.FromResult(result as object);

            }
            return Task.FromResult(null as object);
        }

        //public Task<object> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        //{
        //    if (!command.IsValid(_ProductRepository))
        //    {
        //        NotifyValidationErrors(command);
        //        return Task.FromResult(false as object);
        //    }
        //    else
        //    {
        //        bool result = _ProductRepository.Delete(command.Id);
        //        return Task.FromResult(true as object);
        //    }

        //}
    }
}
