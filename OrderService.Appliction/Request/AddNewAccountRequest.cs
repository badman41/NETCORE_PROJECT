using OrderService.Domain.ReadModels;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderService.Application.Request
{
    public class AddNewAccountRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
