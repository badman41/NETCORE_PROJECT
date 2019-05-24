using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
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
    public class ProductController : ControllerBase
    {
        private IProductAppService _ProductAppService;
        private IUnitRepository _UnitRepository;
        public ProductController(IProductAppService ProductAppService, IUnitRepository UnitRepository)
        {
            _ProductAppService = ProductAppService;
            _UnitRepository = UnitRepository;
        }
        // GET: orderApi/Product
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] SearchProductRequest request)
        {
            var result = await _ProductAppService.getAllProduct(request);
            return Ok(result);
        }
        // GET: orrderApi/Unit/5
        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _ProductAppService.getProductById(id);
            return Ok(result);
        }

        //POST: orrderApi/Customer
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddNewProductRequest request)
        {
            AddNewProductResponse result = await _ProductAppService.addNewProduct(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        //POST: orrderApi/Customer
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateProductRequest request)
        {
            request.Product.Id = id;
            UpdateProductResponse result = await _ProductAppService.updateProduct(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        [HttpPost, DisableRequestSizeLimit]
        [Route("[action]")]
        public async Task<IActionResult> UploadFile()
        {
            IFormFile file = Request.Form.Files[0];
            ISheet sheet = ImportExcelCommon.GetSheetFromFile(file, "Product");
            if (sheet !=null)
            {
                
                IRow headerRow = sheet.GetRow(0); //Get Header Row
                int cellCount = headerRow.LastCellNum;
                int startRow = sheet.FirstRowNum + 1;
                int lastRow = sheet.LastRowNum;
                
                for (int j = 0; j < cellCount; j++)
                {
                    NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                }
                for (int i = startRow; i <= lastRow; i++) //Read Excel File
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    int startCell = row.FirstCellNum;

                    ProductModel p = new ProductModel();
                    p.Code = row.GetCell(startCell++).ToString();
                    p.Name = row.GetCell(startCell++).ToString();
                    p.Note = "";
                    p.Preservation = new PreservationModel();
                    p.OtherUnitOfProduct = new List<ProductUnitModel>();
                    UnitModel unit = _UnitRepository.GetByName(row.GetCell(startCell++).ToString().ToLower());
                    if (unit == null) continue;
                    p.UnitId = unit.ID;
                    int wpu = -1;
                    Int32.TryParse(row.GetCell(startCell).ToString(), out wpu);
                    if (wpu == -1) continue;
                    p.WeightPerUnit = wpu;
                    AddNewProductRequest request = new AddNewProductRequest() { Product = p };
                    await _ProductAppService.addNewProduct(request);
                }
                return Ok();
            }
            return BadRequest();
        }
    }
}