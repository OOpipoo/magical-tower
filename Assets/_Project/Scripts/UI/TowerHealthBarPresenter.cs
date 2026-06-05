using MagicalTower.Core;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace MagicalTower.UI
{
	public class TowerHealthBarPresenter : MonoBehaviour, IInitializable
	{
		[SerializeField] private Image _fillImage;

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
			_eventBus.Unsubscribe<GameEvents.TowerHealthChangedEvent>(OnTowerHealthChanged);
		}

		private void OnTowerHealthChanged(GameEvents.TowerHealthChangedEvent evt)
		{
			_fillImage.fillAmount = evt.CurrentHealth / evt.MaxHealth;
		}
	}
}