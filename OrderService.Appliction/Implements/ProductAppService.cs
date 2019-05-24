using MassTransit;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Application.Shared;
using OrderService.Domain.Bus;
//using OrderService.Domain.Commands.Product;
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
using OrderService.Domain.Commands.Product;

namespace OrderService.Application.Implements
{
    public class ProductAppService : IProductAppService
    {
        private readonly IProductService _service;
        private readonly IUnitService _unitService;
        private readonly IPreservationService _preservationService;
        private readonly IMediatorHandler Bus;
        private readonly DomainNotificationHandler _notifications;

        public ProductAppService(IProductService ProductService, IUnitService UnitService, IPreservationService PreservationService, IMediatorHandler bus
                                    , INotificationHandler<DomainNotification> notifications)
        {
            _service = ProductService;
            _unitService = UnitService;
            _preservationService = PreservationService;
            Bus = bus;
            _notifications = (DomainNotificationHandler)notifications;

        }

        public Task<GetAllProductResponse> getAllProduct(SearchProductRequest request)
        {
            GetAllProductResponse response = new GetAllProductResponse();
            try
            {
                //dieu kien search
                ProductModel productModel = new ProductModel();
                productModel.Name = request.Name;
                productModel.Code = request.Code;
                productModel.PreservationId = request.StoreCondition;
                List<DataProductResponse> datas = new List<DataProductResponse>();
                PagedModel pagedModel = _service.Search(productModel, request.Page, request.PageSize);
                
                if (pagedModel.Data != null)
                {
                    foreach (ProductModel p in pagedModel.Data)
                    {
                        p.Unit = _unitService.Get(p.UnitId);//lay don vi tinh thong thuong
                        p.OtherUnitOfProduct = _unitService.GetALlOtherUnitOfProduct(p.Id);//lay don vi tinh trong bao gia
                        p.Preservation = _preservationService.Get(p.PreservationId);
                        DataProductResponse d = new DataProductResponse();
                        d.ID = p.Id;
                        d.UpdatedAt = p.UpdatedAt;
                        d.CreatedAt = p.CreatedAt;
                        d.ProductInfo = p;
                        datas.Add(d);
                    }
                }
                response.Data = datas;
                response.Metadata = new Metadata();
                response.Metadata.Page = pagedModel.Page;
                response.Metadata.PageSize = pagedModel.PageSize;
                response.Metadata.Total = pagedModel.PageTotal;
                response.Success = true;
            }
            catch(Exception e)
            {
                response.Message = e.ToString();
                response.Success = false;
            }
            return Task.FromResult(response);
        }

        public Task<AddNewProductResponse> addNewProduct(AddNewProductRequest request)
        {
            AddNewProductCommand command = new AddNewProductCommand 
            (
                request.Product.Name,
                request.Product.Note,
                request.Product.Code,
                request.Product.OtherUnitOfProduct,
                request.Product.UnitId,
                request.Product.WeightPerUnit,
                request.Product.Preservation.ID
            );
            Task<object> product = (Task<object>)Bus.SendCommand(command);
            //RabbitMQBus.Publish(command);
            AddNewProductResponse response = new AddNewProductResponse();
            response = Common<AddNewProductResponse>.checkHasNotification(_notifications, response);
            if (response.Success)
            {
                ProductModel ProductModel = (ProductModel)product.Result;
                response.Data = ProductModel.Id;
            }
            return Task.FromResult(response);
        }

        public Task<GetProductResponse> getProductById(int id)
        {
            GetProductResponse response = new GetProductResponse();
            try
            {
                ProductModel p = _service.Get(id);
                p.Unit = _unitService.Get(p.UnitId);//lay don vi tinh thong thuong
                p.OtherUnitOfProduct = _unitService.GetALlOtherUnitOfProduct(p.Id);//lay don vi tinh trong bao gia
                p.Preservation = _preservationService.Get(p.PreservationId);

                DataProductResponse d = new DataProductResponse();
                d.ID = p.Id;
                d.UpdatedAt = p.UpdatedAt;
                d.CreatedAt = p.CreatedAt;
                d.ProductInfo = p;

                response.Data = d;

                response.Message = "Success";
                response.Success = true;
            }
            catch (Exception e)
            {
                response.Message = e.ToString();
                response.Success = true;
            }
            return Task.FromResult(response);
        }

        public Task<UpdateProductResponse> updateProduct(UpdateProductRequest request)
        {
            UpdateProductCommand command = new UpdateProductCommand
            (
                request.Product.Id,
                request.Product.Name,
                request.Product.Note,
                request.Product.Code,
                request.Product.OtherUnitOfProduct,
                request.Product.UnitId,
                request.Product.WeightPerUnit,
                request.Product.Preservation.ID
            );
            Task<object> product = (Task<object>)Bus.SendCommand(command);
            UpdateProductResponse response = new UpdateProductResponse();
            response = Common<UpdateProductResponse>.checkHasNotification(_notifications, response);
            if (response.Success)
            {
                response.Data = (bool) product.Result;
            }
            return Task.FromResult(response);
        }

        public Task<GetAllProductResponse> getAllProductByCustomer(GetAllProductByCustomerRequest request)
        {
            GetAllProductResponse response = new GetAllProductResponse();
            try
            {
                //dieu kien search
                List<DataProductResponse> datas = new List<DataProductResponse>();
                PagedModel pagedModel = _service.GetAllProductByCustomer(request.Id, request.Page, request.PageSize);

                if (pagedModel.Data != null)
                {
                    foreach (ProductModel p in pagedModel.Data)
                    {
                        p.Unit = _unitService.Get(p.UnitId);//lay don vi tinh thong thuong
                        p.OtherUnitOfProduct = _unitService.GetALlProductUnitOfCustomer(p.Id,request.Id);//lay don vi tinh trong bao gia
                        p.Preservation = _preservationService.Get(p.PreservationId);
                        DataProductResponse d = new DataProductResponse();
                        d.ID = p.Id;
                        d.UpdatedAt = p.UpdatedAt;
                        d.CreatedAt = p.CreatedAt;
                        d.ProductInfo = p;
                        datas.Add(d);
                    }
                }
                response.Data = datas;
                response.Metadata = new Metadata();
                response.Metadata.Page = pagedModel.Page;
                response.Metadata.PageSize = pagedModel.PageSize;
                response.Metadata.Total = pagedModel.PageTotal;
                response.Success = true;
            }
            catch (Exception e)
            {
                response.Message = e.ToString();
                response.Success = false;
            }
            return Task.FromResult(response);
        }
    }
}
