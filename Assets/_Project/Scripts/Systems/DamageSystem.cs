using System;
using System.Collections.Generic;
using MagicalTower.Core;
using MagicalTower.Domain.Effects;
using MagicalTower.Domain.Enemy;
using MagicalTower.Domain.Tower;
using UnityEngine;
using VContainer.Unity;

namespace MagicalTower.Systems
{
	public class DamageSystem : IInitializable, IDisposable
	{
		private readonly EventBus _eventBus;
		private readonly TowerHealth _towerHealth;

		public DamageSystem(EventBus eventBus, TowerHealth towerHealth)
		{
			_eventBus = eventBus;
			_towerHealth = towerHealth;
		}

		public void Initialize()
		{
			_eventBus.Subscribe<GameEvents.EnemyAttackedTowerEvent>(OnEnemyAttackedTower);
		}

		public void DealDamage(Enemy enemy, float amount)
		{
			enemy.TakeDamage(amount);
		}

		public void DealAoeDamage(Vector3 center, float radius, float amount, List<Enemy> enemies)
		{
			var snapshot = new List<Enemy>(enemies);
			foreach (var enemy in snapshot)
			{
				if (!enemy.IsAlive) continue;
				if (Vector3.Distance(center, enemy.transform.position) <= radius)
					enemy.TakeDamage(amount);
			}
		}

		public void ApplyBurn(Enemy enemy, float damagePerSecond, float duration)
		{
			enemy.AddEffect(new BurnEffect(damagePerSecond, duration));
		}

		public void ApplySlow(Enemy enemy, float speedMultiplier, float duration)
		{
			enemy.AddEffect(new SlowEffect(speedMultiplier, duration));
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