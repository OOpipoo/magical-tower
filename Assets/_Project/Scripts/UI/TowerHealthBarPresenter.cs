using MagicalTower.Core;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace MagicalTower.UI
{
	public class TowerHealthBarPresenter : MonoBehaviour, IInitializable
	{
		[SerializeField] private Image _healthFill;
		[SerializeField] private Image _damageFill;

		private EventBus _eventBus;

		[Inject]
		public void Construct(EventBus eventBus)
		{
			_eventBus = eventBus;
		}

		public void Initialize()
		{
			_eventBus.Subscribe<GameEvents.TowerHealthChangedEvent>(OnTowerHealthChanged);
		}

		private void OnDestroy()
		{
			_eventBus?.Unsubscribe<GameEvents.TowerHealthChangedEvent>(OnTowerHealthChanged);
		}

		private void OnTowerHealthChanged(GameEvents.TowerHealthChangedEvent evt)
		{
			var percent = evt.CurrentHealth / evt.MaxHealth;
			_healthFill.fillAmount = percent;
		}
	}
}