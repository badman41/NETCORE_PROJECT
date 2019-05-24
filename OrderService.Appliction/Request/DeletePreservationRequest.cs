using OrderService.Domain.ReadModels;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderService.Application.Request
{
    public class DeletePreservationRequest
    {
        public int Id { get; set; }
        public DeletePreservationRequest(int id)
        {
            Id = id;
        }
    }
}
