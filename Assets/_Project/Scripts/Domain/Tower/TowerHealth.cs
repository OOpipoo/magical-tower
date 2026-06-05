using MagicalTower.Core;
using UnityEngine;

namespace MagicalTower.Domain.Tower
{
	public class TowerHealth : MonoBehaviour
	{
		private float _currentHealth;
		private float _maxHealth;
		private EventBus _eventBus;
		private bool _isDead;

		public float HealthPercent => _currentHealth / _maxHealth;
		public bool IsDead => _isDead;

		public void Initialize(float maxHealth, EventBus eventBus)
		{
			_maxHealth = maxHealth;
			_currentHealth = maxHealth;
			_eventBus = eventBus;
			_isDead = false;
		}

		public void TakeDamage(float amount)
		{
			if (_isDead) return;

			_currentHealth = Mathf.Max(0f, _currentHealth - amount);
			_eventBus.Publish(new GameEvents.TowerHealthChangedEvent(_currentHealth, _maxHealth));

			if (_currentHealth <= 0f)
				Die();
		}

		private void Die()
		{
			_isDead = true;
			_eventBus.Publish(new GameEvents.GameOverEvent());
		}
	}
}