using OrderService.Domain.ReadModels;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderService.Application.Request
{
    public class AddNewPreservationRequest
    {
        public string Description { get; set; }
    }
}
