using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;

namespace OrderService.Domain.Interface.Respository
{
    public interface IAccountRepository : IRepository<AccountModel, AccountModel>
    {
        AccountModel get(string userName, string pass);
        AccountModel getByCustomer(int userTypeId);
        PagedModel Search(AccountModel account, int page, int pageSize);
        bool ChangePassword(AccountModel account, string oldPassword);
    }
}
