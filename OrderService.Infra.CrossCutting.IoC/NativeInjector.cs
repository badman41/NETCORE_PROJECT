using GreenPipes;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Implements;
using OrderService.Application.Interfaces;
using OrderService.Domain.Bus;
using OrderService.Domain.CommandHandlers;
using OrderService.Domain.Commands.Customer;
using OrderService.Domain.Commands.CustomerGroup;
using OrderService.Domain.Commands.Invoices;
using OrderService.Domain.Commands.Preservation;
using OrderService.Domain.Commands.Product;
using OrderService.Domain.Commands.ProductRequest;
using OrderService.Domain.Commands.Quotation;
using OrderService.Domain.Commands.Unit;
using OrderService.Domain.Customer.CommandHandlers;
using OrderService.Domain.CustomerGroup.CommandHandlers;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.Interface.Service;
using OrderService.Domain.Invoice.CommandHandlers;
using OrderService.Domain.Notifications;
using OrderService.Domain.Preservation.CommandHandlers;
using OrderService.Domain.Product.CommandHandlers;
using OrderService.Domain.ProductRequest.CommandHandlers;
using OrderService.Domain.Quotation.CommandHandlers;
using OrderService.Domain.Unit.CommandHandlers;
using OrderService.Infra.Data.Respository.Bus;
using OrderService.Infra.Data.Respository.Dapper;
using OrderService.Infra.Data.Respository.Services;
using SimpleInjector;
using System;
using System.Configuration;
using TransitionApp.Domain.ReadModel.Invoice;
using TransitionApp.Models.VehicleXXX;

namespace OrderService.Infra.CrossCutting.IoC
{
    public class NativeInjector
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration _config)
        {
            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, Data.Respository.Bus.InMemoryBus>();

            //RabbitMQ
            services.AddScoped<UnitCommandhandlerMQ>();
            services.AddScoped<UpdateInvoiceConsumer>();
            Container container = new Container();
            container.Register<IInvoiceRepository, InvoiceRepository>(Lifestyle.Singleton);
            container.RegisterInstance(_config);
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(_config.GetSection("RabbitMQHost").Value), settings =>
                {
                    settings.Username("tuandv");
                    settings.Password("tuandv");
                });
                //cfg.ReceiveEndpoint(host, "unit", e =>
                //{
                //    //e.Consumer<UnitCommandhandlerMQ>();
                //    e.Handler<AddNewCustomerGroupCommand>(context =>
                //    {
                //        return Console.Out.WriteLineAsync($"Received: {context.Message}");
                //    });

                //});
                cfg.ReceiveEndpoint(host, "InvoiceStatus", e =>
                {
                    //e.Bind("exchangeTuanDv");
                    //e.Bind<Abc>();
                    e.Consumer<UpdateInvoiceConsumer>(container);

                });
            });


            services.AddSingleton<IPublishEndpoint>(bus);
            services.AddSingleton<ISendEndpointProvider>(bus);
            services.AddSingleton<IBus>(bus);

            bus.Start();
            //AppServices
            services.AddTransient<IUnitAppService, UnitAppService>();
            services.AddTransient<ICustomerGroupAppService, CustomerGroupAppService>();
            services.AddTransient<ICustomerAppService, CustomerAppService>();
            services.AddTransient<IProductAppService, ProductAppService>();
            services.AddTransient<IQuotationAppService, QuotationAppService>();
            services.AddTransient<IQuotationItemAppService, QuotationItemAppService>();
            services.AddTransient<IInvoiceAppService, InvoiceAppService>();
            services.AddTransient<IPreservationAppService, PreservationAppService>();
            services.AddTransient<IProductRequestAppService, ProductRequestAppService>();

            // Services
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<ICustomerGroupService, CustomerGroupService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IQuotationService, QuotationService>();
            services.AddScoped<IQuotationItemService, QuotationItemService>();
            services.AddScoped<IInvoiceItemService, InvoiceItemService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IPreservationService, PreservationService>();
            services.AddScoped<IProductRequestService, ProductRequestService>();

            // Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            // Domain - Commands
            services.AddScoped<IRequestHandler<AddNewUnitCommand, object>, UnitCommandhandler>();
            services.AddScoped<IRequestHandler<DeleteUnitCommand, object>, UnitCommandhandler>();
            services.AddScoped<IRequestHandler<DeleteCustomerGroupCommand, object>, CustomerGroupCommandhandler>();
            services.AddScoped<IRequestHandler<AddNewCustomerGroupCommand, object>, CustomerGroupCommandhandler>();
            services.AddScoped<IRequestHandler<DeleteCustomerGroupCommand, object>, CustomerGroupCommandhandler>();
            services.AddScoped<IRequestHandler<AddNewCustomerCommand, object>, CustomerCommandhandler>();
            services.AddScoped<IRequestHandler<UpdateCustomerCommand, object>, CustomerCommandhandler>();
            services.AddScoped<IRequestHandler<AddNewProductCommand, object>, ProductCommandhandler>();
            services.AddScoped<IRequestHandler<UpdateProductCommand, object>, ProductCommandhandler>();
            services.AddScoped<IRequestHandler<AddNewQuotationItemCommand, object>, QuotationItemCommandhandler>();
            services.AddScoped<IRequestHandler<UpdateQuotationItemCommand, object>, QuotationItemCommandhandler>();
            services.AddScoped<IRequestHandler<AddNewInvoiceCommand, object>, InvoiceCommandhandler>();
            services.AddScoped<IRequestHandler<UpdateInvoiceCommand, object>, InvoiceCommandhandler>();
            services.AddScoped<IRequestHandler<AddNewPreservationCommand, object>, PreservationCommandhandler>();
            services.AddScoped<IRequestHandler<DeletePreservationCommand, object>, PreservationCommandhandler>();
            services.AddScoped<IRequestHandler<AddNewQuotationCommand, object>, QuotationCommandhandler>();
            services.AddScoped<IRequestHandler<ChangeCustomerGroupCommand, object>, CustomerCommandhandler>();
            services.AddScoped<IRequestHandler<AddNewProductRequestCommand, object>, ProductRequestCommandhandler>();
            services.AddScoped<IRequestHandler<UpdateProductRequestCommand, object>, ProductRequestCommandhandler>();
            services.AddScoped<IRequestHandler<DeleteProductRequestCommand, object>, ProductRequestCommandhandler>();

            // Respository
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<ICustomerGroupRepository, CustomerGroupRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IQuotationRepository, QuotationRepository>();
            services.AddScoped<IQuotationItemRepository, QuotationItemRepository>();
            services.AddScoped<IInvoiceItemRepository, InvoiceItemRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IPreservationRepository, PreservationRepository>();
            services.AddScoped<IProductRequestRepository, ProductRequestRepository>();
        }
    }
}
