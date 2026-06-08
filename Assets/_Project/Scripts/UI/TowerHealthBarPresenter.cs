using MagicalTower.Core;
using MagicalTower.Data;
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
		[Space]
		[SerializeField] private TextMeshProUGUI _healthText;
		[SerializeField] private TextMeshProUGUI _waveText;
		
		private EventBus _eventBus;
		private Tower _tower;
		private WaveConfig _waveConfig;
		private int _currentWave = 1;
		
		
		public void Construct(EventBus eventBus, Tower tower, WaveConfig waveConfig)
		{
			_eventBus = eventBus;
			_tower = tower;
			_waveConfig = waveConfig;
		}

		public void Initialize()
		{
			_eventBus.Subscribe<GameEvents.TowerHealthChangedEvent>(OnTowerHealthChanged);
			_eventBus.Subscribe<GameEvents.WaveChangedEvent>(OnWaveChanged);
			
			InitTowerHealth();
			InitWave();
		}

		private void InitTowerHealth()
		{
			_healthFill.fillAmount = 1f;
			if (_healthText != null)
				_healthText.text = $"{Mathf.RoundToInt(_tower.MaxHealth)} / {Mathf.RoundToInt(_tower.MaxHealth)}";
		}
		
		private void InitWave()
		{
			if (_waveText != null)
				_waveText.text = $"Wave {_currentWave} / {_waveConfig.Periods.Count}";
		}

		private void OnDestroy()
		{
			_eventBus?.Unsubscribe<GameEvents.TowerHealthChangedEvent>(OnTowerHealthChanged);
			_eventBus?.Unsubscribe<GameEvents.WaveChangedEvent>(OnWaveChanged);
		}

		private void OnTowerHealthChanged(GameEvents.TowerHealthChangedEvent evt)
		{
			var percent = evt.CurrentHealth / evt.MaxHealth;
			_healthFill.fillAmount = percent;
			UpdateHealthText(evt.CurrentHealth,  evt.MaxHealth);
		}

		private void OnWaveChanged(GameEvents.WaveChangedEvent evt)
		{
			_currentWave = evt.CurrentWave;
			if (_waveText != null)
				_waveText.text = $"Wave {_currentWave} / {_waveConfig.Periods.Count}";
		}

		private void UpdateHealthText(float currentHealth, float maxHealth)
		{
			if (_healthText != null)
				_healthText.text = $"{Mathf.RoundToInt(currentHealth)} / {Mathf.RoundToInt(maxHealth)}";
		}
		
		
	}
}