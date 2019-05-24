using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Interface.Service
{
    public interface IQuotationItemService : IService<QuotationItemModel, QuotationItem>
    {
        //function lay danh sach bao gia cho moi loai don vi tinh theo product
        IEnumerable<QuotationItemModel> GetListItemOfQuotation(int quotationId, int productId);
    }
}
