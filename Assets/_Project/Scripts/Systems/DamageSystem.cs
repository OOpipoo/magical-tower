using System.Collections.Generic;
using MagicalTower.Core;
using MagicalTower.Domain.Enemy;
using MagicalTower.Domain.Tower;
using UnityEngine;

namespace MagicalTower.Systems
{
	public class DamageSystem
	{
		private readonly EventBus _eventBus;
		private readonly TowerHealth _towerHealth;

		public DamageSystem(EventBus eventBus, TowerHealth towerHealth)
		{
			_eventBus = eventBus;
			_towerHealth = towerHealth;

			_eventBus.Subscribe<GameEvents.EnemyAttackedTowerEvent>(OnEnemyAttackedTower);
		}

		public void DealDamage(Enemy enemy, float amount)
		{
			enemy.TakeDamage(amount);
		}

		public void DealAoeDamage(Vector3 center, float radius, float amount, List<Enemy> enemies)
		{
			foreach (var enemy in enemies)
			{
				if (!enemy.IsAlive) continue;
				if (Vector3.Distance(center, enemy.transform.position) <= radius)
					enemy.TakeDamage(amount);
			}
		}

		public void ApplyBurn(Enemy enemy, float damagePerSecond, float duration)
		{
			// enemy.ApplyBurn(damagePerSecond, duration);
		}

		private void OnEnemyAttackedTower(GameEvents.EnemyAttackedTowerEvent evt)
		{
			_towerHealth.TakeDamage(evt.Damage);
		}

		public void Dispose()
		{
			_eventBus.Unsubscribe<GameEvents.EnemyAttackedTowerEvent>(OnEnemyAttackedTower);
		}
	}
}