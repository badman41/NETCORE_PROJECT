using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Common
{
    public class ImportExcelCommon
    {
        public static ISheet GetSheetFromFile(IFormFile file, string folderUpload)
        {
            // string webRootPath = _hostingEnvironment.;
            string newPath = Path.Combine("D:\\UploadFile", folderUpload);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    return sheet;
                }
            }
            return null;
            
        }
        public static string GetRowCellValue(IRow row, int cellIndex)
        {
            return (row.GetCell(cellIndex) == null) ? "" : row.GetCell(cellIndex).ToString();
        }
        public static string[] GetRowValue(IRow row,int startCell,int cellCount)
        {
            string[] result = new string[cellCount];
            for(int i = startCell; i < cellCount; i++)
            {
                result[i] = GetRowCellValue(row, i);
            }
            return result;
        }
    }
    public enum StatusInvoice
    {
        DangXuLy = 0,
        ThieuHang = 1,
        HoanThanh = 2,
        DaHuy = 3,
        DaXepLich = 4
    }
    public class Common
    {
        public static string GetStatusInvoice(int status)
        {
            if (status == (int)StatusInvoice.DangXuLy) return "Đang xử lý";
            if (status == (int)StatusInvoice.ThieuHang) return "Thiếu Hàng/ Cần đổi trả";
            if (status == (int)StatusInvoice.HoanThanh) return "Hoàn thành";
            if (status == (int)StatusInvoice.DaHuy) return "Đã hủy";
            else return "Đã xếp lịch";
        }
    }
}
