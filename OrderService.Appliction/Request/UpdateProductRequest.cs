using OrderService.Domain.ReadModels;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderService.Application.Request
{
    public class UpdateProductRequest
    {
        public ProductModel Product { get; set; }
    }
}
