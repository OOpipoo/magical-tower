using UnityEngine;

namespace MagicalTower.Core
{
	public static class GameEvents
	{
		public struct DamageDealtEvent
		{
			public Vector3 Position;
			public float Amount;

			public DamageDealtEvent(Vector3 position, float amount)
			{
				Position = position;
				Amount = amount;
			}
		}

		public struct EnemyDiedEvent
		{
			public GameObject Enemy;

			public EnemyDiedEvent(GameObject enemy)
			{
				Enemy = enemy;
			}
		}

		public struct EnemyAttackedTowerEvent
		{
			public float Damage;

			public EnemyAttackedTowerEvent(float damage)
			{
				Damage = damage;
			}
		}

		public struct TowerHealthChangedEvent
		{
			public float CurrentHealth;
			public float MaxHealth;

			public TowerHealthChangedEvent(float currentHealth, float maxHealth)
			{
				CurrentHealth = currentHealth;
				MaxHealth = maxHealth;
			}
		}

		public struct GameOverEvent { }
	}
}