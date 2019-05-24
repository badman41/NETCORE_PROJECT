using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;
using System.Collections.Generic;

namespace OrderService.Domain.Interface.Respository
{
    public interface IQuotationItemRepository : IRepository<QuotationItemModel, QuotationItem>
    {
        IEnumerable<QuotationItemModel> GetListItemOfQuotation(int quotationId, int productId);
        QuotationItemModel GetByProperties(QuotationItem entity);
    }
}
