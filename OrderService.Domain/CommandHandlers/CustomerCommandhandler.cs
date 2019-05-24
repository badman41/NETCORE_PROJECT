using MediatR;
using Newtonsoft.Json;
using OrderService.Domain.Bus;
using OrderService.Domain.CommandHandlers;
using OrderService.Domain.Commands.Customer;
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

namespace OrderService.Domain.Customer.CommandHandlers
{
    public class CustomerCommandhandler :   BaseCommandHandler
                                        , IRequestHandler<AddNewCustomerCommand, object>
                                        , IRequestHandler<UpdateCustomerCommand, object>
                                        , IRequestHandler<ChangeCustomerGroupCommand,object>
                                        //, IRequestHandler<DeleteCustomerCommand, object>
    {

        private readonly ICustomerRepository _CustomerRepository;
        private readonly IAccountRepository _AccountRepository;
        private readonly IAddressRepository _AddressRepository;
        private readonly IMediatorHandler _bus;

        public CustomerCommandhandler(ICustomerRepository CustomerRepository,
                                      IAddressRepository AddressRepository,
                                      IAccountRepository AccountRepository,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(bus, notifications)
        {
            _CustomerRepository = CustomerRepository;
            _AddressRepository = AddressRepository;
            _bus = bus;
            _AccountRepository = AccountRepository;
        }

        public Task<object> Handle(AddNewCustomerCommand command, CancellationToken cancellationToken)
        {
            
            
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
            }
            else
            {
                AddressModel addressModel = _AddressRepository.GetByLatLng(command.Lat, command.Lng);
                if (addressModel == null)
                {
                    Entities.Address address = new Entities.Address(
                        null,
                        new City(command.City),
                        new Country(command.Country),
                        new District(command.District),
                        new Lat(command.Lat),
                        new Lng(command.Lng),
                        new Street(command.Street),
                        new StreetNumber(command.StreetNumber)
                    );
                    addressModel = _AddressRepository.Add(address);
                }
                Entities.Customer u = new Entities.Customer(
                null,
                new Name(command.Name),
                new Note(command.Note),
                new CartCode(command.CartCode),
                new PhoneNumber(command.PhoneNumber),
                new Email(command.Email),
                new Code(command.Code),
                new Status(command.Status),
                new Entities.Address(
                        new Identity((uint)addressModel.ID),
                        new City(command.City),
                        new Country(command.Country),
                        new District(command.District),
                        new Lat(command.Lat),
                        new Lng(command.Lng),
                        new Street(command.Street),
                        new StreetNumber(command.StreetNumber)
                    )
                );
                CustomerModel model = _CustomerRepository.Add(u);
                if (model != null)
                {
                    AccountModel account = new AccountModel()
                    {
                        DisplayName = model.Name,
                        Name = model.Name,
                        Role = 2,
                        Status = 0,
                        UserName = model.Code,
                        UserTypeID = model.ID,
                        UpdatedAt = DateTime.Now,
                        Password = Guid.NewGuid().ToString("d").Substring(0, 8)
                    };
                    _AccountRepository.Add(account);
                    string message = "Bạn vừa đăng ký thành công trên hệ thống Anvita. Tài khoản của bạn là: " +
                        account.UserName + ", password: " + account.Password +
                        ". Vui lòng truy cập trang đặt hàng để thay đổi mật khẩu!";
                    SendSMS.SendSMSCommon.SendSMS(message, "+84" + model.PhoneNumber.Substring(1));
                    return Task.FromResult(model as object);
                }
                _bus.RaiseEvent(new DomainNotification("Customer", "Server error", NotificationCode.Error));

            }
            return Task.FromResult(null as object);
        }

        public Task<object> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
            }
            else
            {
                
                AddressModel addressModel = _AddressRepository.GetByLatLng(command.Lat, command.Lng);
                if (addressModel == null)
                {
                    Entities.Address address = new Entities.Address(
                        null,
                        new City(command.City),
                        new Country(command.Country),
                        new District(command.District),
                        new Lat(command.Lat),
                        new Lng(command.Lng),
                        new Street(command.Street),
                        new StreetNumber(command.StreetNumber)
                    );
                    addressModel = _AddressRepository.Add(address);
                }
                Entities.Customer u = new Entities.Customer(
                    new Identity((uint)command.Id),
                    new Name(command.Name),
                    new Note(command.Note),
                    new CartCode(command.CartCode),
                    new PhoneNumber(command.PhoneNumber),
                    new Email(command.Email),
                    new Code(command.Code),
                    new Status(command.Status),
                    new Entities.Address(
                        new Identity((uint)addressModel.ID),
                        new City(command.City),
                        new Country(command.Country),
                        new District(command.District),
                        new Lat(command.Lat),
                        new Lng(command.Lng),
                        new Street(command.Street),
                        new StreetNumber(command.StreetNumber)
                    )
                );
                bool resullt = _CustomerRepository.Update(u);
                if (!resullt)
                {
                    _bus.RaiseEvent(new DomainNotification("Customer", "Server error", NotificationCode.Error));
                    
                }
                return Task.FromResult(resullt as object);
            }
            return Task.FromResult(false as object);
        }

        public Task<object> Handle(ChangeCustomerGroupCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
            }
            else
            {
                bool resullt = _CustomerRepository.ChangeCustomerGroup(command.CustomerGroupId,command.Id);
                if (!resullt)
                {
                    _bus.RaiseEvent(new DomainNotification("Customer", "Server error", NotificationCode.Error));

                }
                return Task.FromResult(resullt as object);
            }
            return Task.FromResult(false as object);
        }

        //public Task<object> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
        //{
        //    if (!command.IsValid(_CustomerRepository))
        //    {
        //        NotifyValidationErrors(command);
        //        return Task.FromResult(false as object);
        //    }
        //    else
        //    {
        //        bool result = _CustomerRepository.Delete(command.Id);
        //        return Task.FromResult(true as object);
        //    }

        //}
    }
}
