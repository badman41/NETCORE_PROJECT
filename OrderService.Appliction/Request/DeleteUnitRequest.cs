using OrderService.Domain.ReadModels;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderService.Application.Request
{
    public class DeleteUnitRequest
    {
        public int Id { get; set; }
        public DeleteUnitRequest(int id)
        {
            Id = id;
        }
    }
}
