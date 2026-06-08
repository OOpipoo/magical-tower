using MagicalTower.Core;
using MagicalTower.Domain.Tower;
using TMPro;
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
		[SerializeField] private TextMeshProUGUI _healthText;
		
		private EventBus _eventBus;
		private Domain.Tower.Tower _tower;
		
		
		[Inject]
		public void Construct(EventBus eventBus, Tower tower)
		{
			_eventBus = eventBus;
			_tower = tower;
		}

		public void Initialize()
		{
			_eventBus.Subscribe<GameEvents.TowerHealthChangedEvent>(OnTowerHealthChanged);
			InitTowerHealth();
		}

		private void InitTowerHealth()
		{
			_healthFill.fillAmount = 1f;
			if (_healthText != null)
				_healthText.text = $"{Mathf.RoundToInt(_tower.MaxHealth)} / {Mathf.RoundToInt(_tower.MaxHealth)}";
		}

		private void OnDestroy()
		{
			_eventBus?.Unsubscribe<GameEvents.TowerHealthChangedEvent>(OnTowerHealthChanged);
		}

		private void OnTowerHealthChanged(GameEvents.TowerHealthChangedEvent evt)
		{
			var percent = evt.CurrentHealth / evt.MaxHealth;
			_healthFill.fillAmount = percent;
			UpdateHealthText(evt.CurrentHealth,  evt.MaxHealth);
		}

		private void UpdateHealthText(float currentHealth, float maxHealth)
		{
			if (_healthText != null)
				_healthText.text = $"{Mathf.RoundToInt(currentHealth)} / {Mathf.RoundToInt(maxHealth)}";
		}
	}
}