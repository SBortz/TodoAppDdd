using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TodoAppDdd.Domain.DDDBase;

namespace TodoAppDdd.Persistence.EventStore
{
	public class TextFileEventStore : IEventStore
	{
		public static string Filename = "events.txt";

		public async Task AppendEvents(IEnumerable<IDomainEvent> domainEvents)
		{
			List<string> lines = new List<string>();

			foreach (var domainEvent in domainEvents)
			{
				var serializedDomainEvent = JsonConvert.SerializeObject(domainEvent);
				lines.Add(serializedDomainEvent);
			}

			var path = GetEventFilePath();
			File.AppendAllLines(path, lines);
		}

		public async Task<IEnumerable<TEventType>> Get<TEventType>(string id)
			where TEventType : class, IDomainEvent
		{
			var selectedEvents = await this.GetAll<TEventType>();
                return selectedEvents.Where(x => x.Id == id);
		}

		public async Task<IEnumerable<TEventType>> GetAll<TEventType>()
			where TEventType : class, IDomainEvent
		{
			var path = GetEventFilePath();
			var allLines = File.ReadAllLines(path);
			var selectedLines = allLines
				.Where(line => JsonConvert.DeserializeObject<StoredEvent>(line).EventType == typeof(TEventType).Name)
				.Select(JsonConvert.DeserializeObject<TEventType>);

			return selectedLines;
		}

		private static string GetEventFilePath()
		{
			var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), Filename);
			return path;
		}

		#region These functions exist, because they make it easier to demonstrate EventSourcing.
		public IEnumerable<String> OutputRawEvents()
		{
			var path = GetEventFilePath();
			try
			{
				var allLines = File.ReadAllLines(path);
				return allLines;
			}
			catch
			{

			}

			return new List<string>();
		}

		public void DropAllEvents()
		{
			File.Delete(GetEventFilePath());
		}
		#endregion
	}
}
