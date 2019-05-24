using OrderService.Domain.ReadModels;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderService.Application.Request
{
    public class ChangePasswordRequest
    {
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
