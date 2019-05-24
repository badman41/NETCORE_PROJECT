using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;
using System.Collections.Generic;

namespace OrderService.Domain.Interface.Respository
{
    public interface IQuotationRepository : IRepository<QuotationModel, Entities.Quotation>
    {
        PagedModel Search(string customerCode, int page, int pageSize);
        IEnumerable<ProductPriceModel> GetListProductOfQuotation(int quotationId);
    }
}
