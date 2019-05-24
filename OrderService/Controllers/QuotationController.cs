using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using OrderService.Domain.Shared.ValueObject;

namespace OrderService.Controllers
{
    [Route("orderApi/[controller]")]
    [ApiController]
    public class QuotationController : ControllerBase
    {
        private IQuotationAppService _QuotationAppService;
        private IQuotationItemAppService _QuotationItemAppService;
        private IUnitRepository _UnitRepository;
        private IHostingEnvironment _hostingEnvironment;

        public QuotationController(IQuotationAppService QuotationAppService, IUnitRepository UnitRepository
                                , IQuotationItemAppService QuotationItemAppService, IHostingEnvironment hostingEnvironment)
        {
            _QuotationAppService = QuotationAppService;
            _UnitRepository = UnitRepository;
            _QuotationItemAppService = QuotationItemAppService;
            _hostingEnvironment = hostingEnvironment;
        }
        // GET: orderApi/Quotation
        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code,[FromQuery] SearchQuotationRequest request)
        {
            request.Code = code;
            var result = await _QuotationAppService.getAllQuotation(request);
            return Ok(result);
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("[action]")]
        public async Task<IActionResult> ImportQuotation([FromQuery] AddNewQuotationRequest request)
        {
            IFormFile file = Request.Form.Files[0];
            ISheet sheet = ImportExcelCommon.GetSheetFromFile(file, "Quotation\\Customer_" + request.customer_code);
            if (sheet != null)
            {
                IRow headerRow = sheet.GetRow(11); //Get Header Row
                int cellCount = headerRow.LastCellNum;
                int startRow = 12;
                int lastRow = sheet.LastRowNum;
                //thuc hien them moi dot bao gia cho khach hang
                int QuotationId = (await _QuotationAppService.addNewQuotation(request)).Data;
                if (QuotationId <= 0) return BadRequest();
                //them cac san pham vao dot bao gia
                for (int i = startRow; i <= lastRow; i++) //Read Excel File
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    int startCell = row.FirstCellNum + 1;

                    AddNewQuotationItemRequest itemRequest = new AddNewQuotationItemRequest();
                    string code = row.GetCell(startCell++).ToString();
                    if (string.IsNullOrWhiteSpace(code)) continue;
                    itemRequest.ProductCode = code;
                    itemRequest.QuotationID = QuotationId;
                    //gia
                    int price = -1;
                    Int32.TryParse(row.GetCell(4).ToString(), out price);
                    if (price == -1) continue;
                    itemRequest.Price = price;
                    

                    UnitModel unit = _UnitRepository.GetByName(row.GetCell(3).ToString().ToLower());
                    if (unit == null) continue;
                    itemRequest.UnitID = unit.ID;
                   
                    
                    await _QuotationItemAppService.addNewQuotation(itemRequest);
                }
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost, DisableRequestSizeLimit]
        [Route("[action]")]
        public async Task<IActionResult> ImportQuotationByGroup([FromQuery] AddNewQuotationByGroupRequest request)
        {
            IFormFile file = Request.Form.Files[0];
            ISheet sheet = ImportExcelCommon.GetSheetFromFile(file, "Quotation\\Customer_Group" + request.group_id);
            if (sheet != null)
            {
                IRow headerRow = sheet.GetRow(11); //Get Header Row
                int cellCount = headerRow.LastCellNum;
                int startRow = 13;
                int lastRow = sheet.LastRowNum;
                //thuc hien them moi dot bao gia cho khach hang
                List<int> QuotationIds = (await _QuotationAppService.addNewQuotationByCustomerGroup(request)).Data;
                if (QuotationIds.Count() <= 0) return BadRequest();
                //them cac san pham vao dot bao gia
                for (int i = startRow; i <= lastRow; i++) //Read Excel File
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    int startCell = row.FirstCellNum + 1;

                    AddNewQuotationItemRequest itemRequest = new AddNewQuotationItemRequest();
                    string code = row.GetCell(startCell++).ToString();
                    if (string.IsNullOrWhiteSpace(code)) continue;
                    itemRequest.ProductCode = code;
                    
                    
                    //gia
                    int price = -1;
                    Int32.TryParse(row.GetCell(4).ToString(), out price);
                    if (price == -1) continue;
                    itemRequest.Price = price;


                    UnitModel unit = _UnitRepository.GetByName(row.GetCell(3).ToString().ToLower());
                    if (unit == null) continue;
                    itemRequest.UnitID = unit.ID;

                    foreach (int QuotationId in QuotationIds)
                    {
                        itemRequest.QuotationID = QuotationId;
                        await _QuotationItemAppService.addNewQuotation(itemRequest);
                    }

                }
                return Ok();
            }
            return BadRequest();
        }
    }
}