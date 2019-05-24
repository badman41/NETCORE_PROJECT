using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OrderService.Application.Interfaces;
using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Common;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.ReadModels;

namespace OrderService.Controllers
{
    [Route("orderApi/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomerAppService _CustomerAppService;
        private IQuotationAppService _QuotationAppService;
        private ICustomerRepository _customerRepository;
        private IHostingEnvironment _hostingEnvironment;

        public CustomerController(ICustomerAppService CustomerAppService, IQuotationAppService QuotationAppService, ICustomerRepository CustomerRepository, IHostingEnvironment hostingEnvironment)
        {
            _CustomerAppService = CustomerAppService;
            _customerRepository = CustomerRepository;
            _hostingEnvironment = hostingEnvironment;
            _QuotationAppService = QuotationAppService;
        }
        // GET: orderApi/Customer
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] SearchCustomerRequest request)
        {
            var result =  await _CustomerAppService.getAllCustomer(request);
            return Ok(result);
        }

        // GET: orrderApi/Customer/5
        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _CustomerAppService.getCustomerById(id);
            return Ok(result);
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetByToken()
        {
            var accesToken = Request.Headers["Authorization"];
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(accesToken) as JwtSecurityToken;
            var id = Int32.Parse(jsonToken.Claims.First().Value);
            var result = await _CustomerAppService.getCustomerById(id);
            List<CustomerModel> lst = new List<CustomerModel>();
            lst.Add(result.Data);
            GetAllCustomerResponse customerResponse = new GetAllCustomerResponse()
            {
                Status = result.Status,
                Total = 1,
                Data = lst
            };
            return Ok(customerResponse);
        }

        //POST: orrderApi/Customer
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddNewCustomerRequest request)
        {
            AddNewCustomerResponse result = await _CustomerAppService.addNewCustomer(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        // PUT: api/Customer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateCustomerRequest request)
        {
            request.Id = id;
            UpdateCustomerResponse result = await _CustomerAppService.updateCustomer(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        // DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    DeleteCustomerRequest request = new DeleteCustomerRequest(id);
        //    DeleteCustomerResponse result = await _CustomerAppService.deleteCustomer(request);
        //    if (result.Success)
        //    {
        //        return Ok(result);
        //    }
        //    return NotFound(result);
        //}

        [HttpPost, DisableRequestSizeLimit]
        [Route("[action]")]
        public async Task<IActionResult> UploadFile()
        {
            IFormFile file = Request.Form.Files[0];
            ISheet sheet = ImportExcelCommon.GetSheetFromFile(file, "Customer");
            if (sheet != null)
            {

                IRow headerRow = sheet.GetRow(1); //Get Header Row
                int cellCount = headerRow.LastCellNum;
                int startRow = sheet.FirstRowNum + 2;
                int lastRow = sheet.LastRowNum;

                for (int i = startRow; i <= lastRow; i++) //Read Excel File
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    int startCell = row.FirstCellNum;
                    
                    //lay cac gia tri tren row
                    string[] rowrequests = ImportExcelCommon.GetRowValue(row, startCell, cellCount);

                    string Code = rowrequests[0];
                    string CartCode = rowrequests[1];
                    string Name = rowrequests[2];
                    string PhoneNumber = rowrequests[3];
                    string Email = rowrequests[4];
                    string StreetNumber = rowrequests[5];
                    string Street = rowrequests[6];
                    string District = rowrequests[7];
                    string City = rowrequests[8];
                    string Country = rowrequests[9];
                    string Lat = rowrequests[10];
                    string Lng = rowrequests[11];

                    CustomerModel customerModel = _customerRepository.GetByCode(Code);
                    if (customerModel != null)
                    {
                        UpdateCustomerRequest updateCustomerRequest = new UpdateCustomerRequest()
                        {
                            Id = customerModel.ID,
                            Code = Code,
                            CartCode = CartCode,
                            Name = Name,
                            PhoneNumber = PhoneNumber,
                            Email = Email,
                            Address = new AddressModel()
                            {
                                StreetNumber = StreetNumber,
                                Street = Street,
                                District = District,
                                City = City,
                                Country = Country,
                                Lat = Double.Parse(Lat.Trim()),
                                Lng = Double.Parse(Lng.Trim())
                            }
                        };
                        await _CustomerAppService.updateCustomer(updateCustomerRequest);
                    }
                    else
                    {
                        AddNewCustomerRequest addNewCustomerRequest = new AddNewCustomerRequest()
                        {
                            Code = Code,
                            CartCode = CartCode,
                            Name = Name,
                            PhoneNumber = PhoneNumber,
                            Email = Email,
                            Address = new AddressModel()
                            {
                                StreetNumber = StreetNumber,
                                Street = Street,
                                District = District,
                                City = City,
                                Country = Country,
                                Lat = Double.Parse(Lat.Trim()),
                                Lng = Double.Parse(Lng.Trim())
                            }
                        };
                        await _CustomerAppService.addNewCustomer(addNewCustomerRequest);
                    }
                    
                }
                return Ok();
            }
            return BadRequest();
        }
    }
}
