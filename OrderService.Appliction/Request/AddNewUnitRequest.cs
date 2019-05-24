using OrderService.Domain.ReadModels;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderService.Application.Request
{
    public class AddNewUnitRequest
    {
        public UnitModel ProductUnit { get; set; }
    }
}
