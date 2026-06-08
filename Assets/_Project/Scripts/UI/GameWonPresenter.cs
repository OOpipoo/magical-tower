using MagicalTower.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MagicalTower.UI
{
	public class GameWonPresenter : MonoBehaviour
	{
		private EventBus _eventBus;

		public void Construct(EventBus eventBus)
		{
			_eventBus = eventBus;
			_eventBus.Subscribe<GameEvents.GameWonEvent>(OnGameWon);
		}

		private void OnDestroy()
		{
			_eventBus?.Unsubscribe<GameEvents.GameWonEvent>(OnGameWon);
		}

		private void OnGameWon(GameEvents.GameWonEvent evt)
		{
			gameObject.SetActive(true);
		}

		public void OnRestartButton()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}