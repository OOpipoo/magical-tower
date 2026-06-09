using System;
using VContainer.Unity;

namespace MagicalTower.Core
{
	public class GameStateService : IInitializable, IDisposable
	{
		public bool IsGameActive { get; private set; } = true;

		private readonly EventBus _eventBus;

		public GameStateService(EventBus eventBus)
		{
			_eventBus = eventBus;
		}

		public void Initialize()
		{
			_eventBus.Subscribe<GameEvents.GameOverEvent>(OnGameEnded);
			_eventBus.Subscribe<GameEvents.GameWonEvent>(OnGameEnded);
		}

		public void Dispose()
		{
			_eventBus.Unsubscribe<GameEvents.GameOverEvent>(OnGameEnded);
			_eventBus.Unsubscribe<GameEvents.GameWonEvent>(OnGameEnded);
		}

		private void OnGameEnded<T>(T _)
		{
			IsGameActive = false;
		}
	}
}