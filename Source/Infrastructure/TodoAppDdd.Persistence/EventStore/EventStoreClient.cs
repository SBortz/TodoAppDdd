using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Newtonsoft.Json;
using TodoAppDdd.Domain.DDDBase;
using TodoAppDdd.Domain.Event;

namespace TodoAppDdd.Persistence.EventStore
{
    public class EventStoreClient : IEventStore
    {
        private readonly IEventStoreConnection _conn;
        private readonly UserCredentials userCredentials;

        public EventStoreClient()
        {
            this.userCredentials = new UserCredentials("admin", "changeit");
            this._conn = EventStoreConnection.Create(new Uri("tcp://admin:changeit@localhost:1113"),
                "InputFromFileConsoleApp");
            this._conn.ConnectAsync();
        }

        public async Task AppendEvents(IEnumerable<IDomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                var serializedDomainEvent = JsonConvert.SerializeObject(domainEvent);
                var serializedDomainEventData = this.StringToByteArray(serializedDomainEvent);
                var eventData = new EventData(Guid.NewGuid(), domainEvent.EventType, true, serializedDomainEventData, null);
                await this._conn.AppendToStreamAsync(domainEvent.Id, ExpectedVersion.Any, eventData);
            }
        }

        public async Task<IEnumerable<TEventType>> Get<TEventType>(string id) where TEventType : class, IDomainEvent
        {
            var allDomainEvents = await this.GetAll<TEventType>();
            return allDomainEvents.Where(x => x.Id == id);
        }

        public async Task<IEnumerable<TEventType>> GetAll<TEventType>() where TEventType : class, IDomainEvent
        {
            var allDomainEvents = new List<TEventType>();
            var readEvents = await this._conn.ReadAllEventsForwardAsync(Position.Start, 500, false, this.userCredentials);
            foreach (var readEventsEvent in readEvents.Events)
            {
                var domainEventJson = this.ByteArrayToString(readEventsEvent.Event.Data);

                var eventType = typeof(TEventType).Name;
                if (readEventsEvent.Event.EventType == eventType)
                {
                    var domainEvent = JsonConvert.DeserializeObject<TEventType>(domainEventJson);
                    allDomainEvents.Add(domainEvent);
                }
                    
            }

            return allDomainEvents;
        }

        public IEnumerable<string> OutputRawEvents()
        {
            throw new NotImplementedException();
        }

        public void DropAllEvents()
        {
            throw new NotImplementedException();
        }

        private byte[] StringToByteArray(string str)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetBytes(str);
        }

        private string ByteArrayToString(byte[] arr)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetString(arr);
        }
    }
}
