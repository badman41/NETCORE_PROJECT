using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
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
    public class InvoiceController : ControllerBase
    {
        private IInvoiceAppService _InvoiceAppService;
        private ICustomerRepository _CustomerAppService;
        private IProductRepository _ProductRepository;
        private IUnitRepository _UnitRepository;
        private IHostingEnvironment _hostingEnvironment;

        public InvoiceController(IInvoiceAppService InvoiceAppService, ICustomerRepository ICustomerAppService
                                , IProductRepository IProductRepository, IUnitRepository IUnitRepository
                                , IHostingEnvironment hostingEnvironment)
        {
            _InvoiceAppService = InvoiceAppService;
            _CustomerAppService = ICustomerAppService;
            _ProductRepository = IProductRepository;
             _UnitRepository=  IUnitRepository;
            _hostingEnvironment = hostingEnvironment;
        }
        // GET: orderApi/Invoice
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] SearchInvoiceRequest request)
        {
            if(request.id > 0)
            {
                var resultGetByid = await _InvoiceAppService.getInvoice(request.id);
                return Ok(resultGetByid);
            }
            var result = await _InvoiceAppService.getAllInvoice(request);
            return Ok(result);
        }

        // GET: orrderApi/Unit/5
        [HttpGet("{id}", Name = "GetInvoice")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _InvoiceAppService.getInvoice(id);
            return Ok(result);
        }

        //POST: orrderApi/Customer
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddNewInvoiceRequest request)
        {
            AddNewInvoiceResponse result = await _InvoiceAppService.addNewInvoice(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        //POST: orrderApi/Customer
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateInvoiceRequest request)
        {
            request.ID = id;
            UpdateInvoiceResponse result = await _InvoiceAppService.updateInvoice(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        
        [HttpPost]
        [Route("[action]/{date}")]
        public async Task<IActionResult> ImportExcel(DateTime date)
        {
            IFormFile file = Request.Form.Files[0];
            ISheet sheet = ImportExcelCommon.GetSheetFromFile(file, "Product");
            if (sheet != null)
            {

                IRow headerRow = sheet.GetRow(0); //Get Header Row
                int cellCount = headerRow.LastCellNum;
                int startRow = sheet.FirstRowNum + 1;
                int lastRow = sheet.LastRowNum;
                List<int> customerIds = new List<int>();
                DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan elapsedTime = date.AddDays(-1) - Epoch;
                double dateDelvery = elapsedTime.TotalSeconds;
                //lay danh sach khach hang import
                for (int j = 3; j < cellCount; j++)
                {
                    ICell cell = headerRow.GetCell(j);
                    string[] value = ImportExcelCommon.GetRowCellValue(headerRow, j).Split('_');
                    if (value.Length == 0) continue;
                    string customerCode = value[0].Trim();
                    CustomerModel customerModel = _CustomerAppService.GetByCode(customerCode);
                    if (customerModel == null) continue;
                    customerIds.Add(customerModel.ID);
                    AddNewInvoiceRequest request = new AddNewInvoiceRequest()
                    {
                        Address = null,
                        CustomerCode = customerCode,
                        CustomerID = customerModel.ID,
                        CustomerName = "",
                        DeliveryTime = dateDelvery
                    };
                    List<InvoiceItemModel> lstItem = new List<InvoiceItemModel>();
                    for (int i = startRow; i <= lastRow; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                        InvoiceItemModel item = new InvoiceItemModel();
                        int startCell = row.FirstCellNum;
                        string productCode = ImportExcelCommon.GetRowCellValue(row, startCell++);
                        ProductModel product = _ProductRepository.GetByCode(productCode);
                        if (product == null) continue;
                        item.ProductID = product.Id;

                        item.Deliveried = false;
                        item.DeliveriedQuantity = 0;
                        item.Note = "";
                        item.ProductName = ImportExcelCommon.GetRowCellValue(row, startCell++);
                        
                        UnitModel unit = _UnitRepository.GetByName(row.GetCell(startCell++).ToString().ToLower());
                        if (unit == null) continue;
                        item.UnitID = unit.ID;
                        item.Quantity = int.Parse(ImportExcelCommon.GetRowCellValue(row, startCell++).Trim());

                        lstItem.Add(item);
                    }
                    request.Items = lstItem;
                    await _InvoiceAppService.addNewInvoice(request);
                }
                
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost, DisableRequestSizeLimit]
        [Route("[action]")]
        public async Task<IActionResult> Export([FromQuery] DateTime date)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath + "/Export";
            string d = date.ToShortDateString().Split(@"/").Join("");
            string sFileName = @"OrderReport_" + d + @".xlsx";
            string URL = string.Format("{0}://{1}/{2}/{3}", Request.Scheme, "localhost:44309", "Export", sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(date.ToShortDateString().Split(@"/").Join(""));
                IRow rowHeader = excelSheet.CreateRow(0);

                ICellStyle defaultStyle = workbook.CreateCellStyle();
                defaultStyle.Alignment = HorizontalAlignment.Center;
                defaultStyle.VerticalAlignment = VerticalAlignment.Center;
                defaultStyle.WrapText = true; //wrap the text in the cell
                defaultStyle.BorderBottom = BorderStyle.Thin;
                defaultStyle.BorderTop = BorderStyle.Thin;
                defaultStyle.BorderLeft = BorderStyle.Thin;
                defaultStyle.BorderRight = BorderStyle.Thin;
                

                rowHeader.CreateCell(0).SetCellValue("Hóa đơn");
                rowHeader.CreateCell(1).SetCellValue("Khách hàng");
                rowHeader.CreateCell(2).SetCellValue("Địa chỉ");
                rowHeader.CreateCell(3).SetCellValue("Danh sách mặt hàng");
                rowHeader.CreateCell(4).SetCellValue("");
                rowHeader.CreateCell(5).SetCellValue("");
                rowHeader.CreateCell(6).SetCellValue("Trạng thái");
                rowHeader.CreateCell(7).SetCellValue("Tổng giá");
                rowHeader.GetCell(0).CellStyle = defaultStyle;
                rowHeader.GetCell(1).CellStyle = defaultStyle;
                rowHeader.GetCell(2).CellStyle = defaultStyle;
                rowHeader.GetCell(3).CellStyle = defaultStyle;
                rowHeader.GetCell(4).CellStyle = defaultStyle;
                rowHeader.GetCell(5).CellStyle = defaultStyle;
                rowHeader.GetCell(6).CellStyle = defaultStyle;
                rowHeader.GetCell(7).CellStyle = defaultStyle;
                IRow rowHeader2 = excelSheet.CreateRow(1);

                rowHeader2.CreateCell(0).SetCellValue("");
                rowHeader2.CreateCell(1).SetCellValue("");
                rowHeader2.CreateCell(2).SetCellValue("");
                rowHeader2.CreateCell(3).SetCellValue("Mặt hàng");
                rowHeader2.CreateCell(4).SetCellValue("Số lượng");
                rowHeader2.CreateCell(5).SetCellValue("Giá");
                rowHeader2.CreateCell(6).SetCellValue("");
                rowHeader2.CreateCell(7).SetCellValue("");
                rowHeader2.GetCell(0).CellStyle = defaultStyle;
                rowHeader2.GetCell(1).CellStyle = defaultStyle;
                rowHeader2.GetCell(2).CellStyle = defaultStyle;
                rowHeader2.GetCell(3).CellStyle = defaultStyle;
                rowHeader2.GetCell(4).CellStyle = defaultStyle;
                rowHeader2.GetCell(5).CellStyle = defaultStyle;
                rowHeader2.GetCell(6).CellStyle = defaultStyle;
                rowHeader2.GetCell(7).CellStyle = defaultStyle;

                var cra = new NPOI.SS.Util.CellRangeAddress(0, 0, 3, 5);
                var cr1 = new NPOI.SS.Util.CellRangeAddress(0, 1, 0, 0);
                var cr2 = new NPOI.SS.Util.CellRangeAddress(0, 1, 1, 1);
                var cr3 = new NPOI.SS.Util.CellRangeAddress(0, 1, 2, 2);
                var cr4 = new NPOI.SS.Util.CellRangeAddress(0, 1, 6, 6);
                var cr5 = new NPOI.SS.Util.CellRangeAddress(0, 1, 7, 7);
                excelSheet.AddMergedRegion(cra);
                excelSheet.AddMergedRegion(cr1);
                excelSheet.AddMergedRegion(cr2);
                excelSheet.AddMergedRegion(cr3);
                excelSheet.AddMergedRegion(cr4);
                excelSheet.AddMergedRegion(cr5);

                excelSheet.SetColumnWidth(0, 8000);
                excelSheet.SetColumnWidth(1, 6000);
                excelSheet.SetColumnWidth(2, 9000);
                excelSheet.SetColumnWidth(3, 6000);
                //in ra danh sach cac khach hang
                IEnumerable<CustomerModel> customers = _CustomerAppService.All();
                SearchInvoiceRequest request = new SearchInvoiceRequest()
                {
                    customer_code = null,
                    customer_id = 0,
                    from_time = date,
                    page = 0,
                    page_size = 0
                };
                var result = await _InvoiceAppService.getAllInvoice(request);
                if (result.Data == null) return BadRequest();
                int rowIndex = 2;
                int startRowIndex = 2;
                string address = "";
                foreach (InvoiceModel invoice in result.Data)
                {
                    startRowIndex = rowIndex;
                    IRow row = excelSheet.CreateRow(rowIndex);
                    row.CreateCell(0).SetCellValue(invoice.Code);
                    row.CreateCell(1).SetCellValue(invoice.CustomerName); 
                    address = invoice.Address == null ? "" :
                        invoice.Address.StreetNumber + ", "
                        + invoice.Address.Street + ", "
                        + invoice.Address.District + ", "
                        + invoice.Address.City + ", "
                        + invoice.Address.Country;
                    row.CreateCell(2).SetCellValue(address);               
                    row.CreateCell(6).SetCellValue(Common.Common.GetStatusInvoice(invoice.Status));
                    row.CreateCell(7).SetCellValue(invoice.TotalPrice);
                    
                    foreach (InvoiceItemModel item in invoice.Items)
                    {
                        if(rowIndex != startRowIndex)
                        {
                            row = excelSheet.CreateRow(rowIndex);
                            row.CreateCell(0).CellStyle = defaultStyle;
                            row.CreateCell(1).CellStyle = defaultStyle;
                            row.CreateCell(2).CellStyle = defaultStyle;
                            row.CreateCell(6).CellStyle = defaultStyle;
                            row.CreateCell(7).CellStyle = defaultStyle;
                        }
                        row.CreateCell(3).SetCellValue(item.ProductName);
                        row.GetCell(3).CellStyle = defaultStyle;
                        row.CreateCell(4).SetCellValue(item.Quantity + " "+ item.UnitName);
                        row.GetCell(4).CellStyle = defaultStyle;
                        row.CreateCell(5).SetCellValue(item.TotalPrice);
                        row.GetCell(5).CellStyle = defaultStyle;
                        rowIndex++;
                    }

                    var cra1 = new NPOI.SS.Util.CellRangeAddress(startRowIndex, rowIndex - 1, 0, 0);
                    var cra2 = new NPOI.SS.Util.CellRangeAddress(startRowIndex, rowIndex - 1, 1, 1);
                    var cra3 = new NPOI.SS.Util.CellRangeAddress(startRowIndex, rowIndex - 1, 2, 2);
                    var cra4 = new NPOI.SS.Util.CellRangeAddress(startRowIndex, rowIndex - 1, 6, 6);
                    var cra5 = new NPOI.SS.Util.CellRangeAddress(startRowIndex, rowIndex - 1, 7, 7);
                    excelSheet.AddMergedRegion(cra1);
                    excelSheet.AddMergedRegion(cra2);
                    excelSheet.AddMergedRegion(cra3);
                    excelSheet.AddMergedRegion(cra4);
                    excelSheet.AddMergedRegion(cra5);
                    IRow rowStart = excelSheet.GetRow(startRowIndex);
                    rowStart.GetCell(0).CellStyle = defaultStyle;
                    rowStart.GetCell(1).CellStyle = defaultStyle;
                    rowStart.GetCell(2).CellStyle = defaultStyle;
                    rowStart.GetCell(6).CellStyle = defaultStyle;
                    rowStart.GetCell(7).CellStyle = defaultStyle;
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            //FileStreamResult f =  File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
            return Ok(URL);
        }

    }
}