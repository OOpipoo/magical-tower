using System;
using System.Collections.Generic;

namespace MagicalTower.Core
{
	public class EventBus
	{
		private readonly Dictionary<Type, List<Delegate>> _handlers = new();

		public void Subscribe<T>(Action<T> handler)
		{
			var type = typeof(T);
			if (!_handlers.ContainsKey(type))
				_handlers[type] = new List<Delegate>();

			_handlers[type].Add(handler);
		}

		public void Unsubscribe<T>(Action<T> handler)
		{
			var type = typeof(T);
			if (_handlers.TryGetValue(type, out var handlers))
				handlers.Remove(handler);
		}

		public void Publish<T>(T evt)
		{
			var type = typeof(T);
			if (!_handlers.TryGetValue(type, out var handlers))
				return;

			foreach (var handler in handlers.ToArray())
				(handler as Action<T>)?.Invoke(evt);
		}
	}
}