using MagicalTower.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MagicalTower.UI
{
	public class GameOverPresenter : MonoBehaviour
	{
		private EventBus _eventBus;

		public void Construct(EventBus eventBus)
		{
			_eventBus = eventBus;
			_eventBus.Subscribe<GameEvents.GameOverEvent>(OnGameOver);
		}

		private void OnDestroy()
		{
			_eventBus?.Unsubscribe<GameEvents.GameOverEvent>(OnGameOver);
		}

		private void OnGameOver(GameEvents.GameOverEvent evt)
		{
			gameObject.SetActive(true);
		}

		public void OnRestartButton()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}