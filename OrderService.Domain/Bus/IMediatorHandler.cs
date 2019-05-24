using OrderService.Domain.Commands;
using System.Threading.Tasks;

namespace OrderService.Domain.Bus
{
    public interface IMediatorHandler
    {
        Task SendCommand<T>(T command) where T : BaseCommand;
        Task RaiseEvent<T>(T @event) where T : Event.Event;
    }
}
