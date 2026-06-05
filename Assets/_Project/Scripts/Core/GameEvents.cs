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
			public float Damage;

			public TowerHealthChangedEvent(float currentHealth, float maxHealth, float damage)
			{
				CurrentHealth = currentHealth;
				MaxHealth = maxHealth;
				Damage = damage;
			}
		}

		public struct GameOverEvent { }
	}
}