using OrderService.Domain.ReadModels;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderService.Application.Request
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class LoginRequestViewModel
    {
        [Required(ErrorMessage = "Enter UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }
        public string Token { get; set; }
        public int Usertype { get; set; }
    }
}
