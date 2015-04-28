using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSourcing.Tests.Domain
{
    public interface IEventBus
    {
        Task PublishAsync(string streamName, IDictionary<string, object> headers, IEnumerable<object> events);
        IEnumerable<object> GetEventsFromStream(string streamName);
    }
}