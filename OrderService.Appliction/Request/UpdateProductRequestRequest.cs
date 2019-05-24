using OrderService.Domain.ReadModels;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderService.Application.Request
{
    public class UpdateProductRequestRequest
    {
        public string response { get; set; }
        public int Id { get; set; }
    }
}
