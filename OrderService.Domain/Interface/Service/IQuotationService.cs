using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Interface.Service
{
    public interface IQuotationService : IService<QuotationModel, Entities.Quotation>
    {
        PagedModel Search(string customerCode, int page, int pageSize);
        //function lay danh sach san pham co trong bao gia
        IEnumerable<ProductPriceModel> GetListProductOfQuotation(int quotationId);
    }
}
