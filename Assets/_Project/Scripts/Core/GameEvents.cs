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

		public struct TowerDamagedEvent
		{
			public float CurrentHealth;
			public float MaxHealth;

			public TowerDamagedEvent(float currentHealth, float maxHealth)
			{
				CurrentHealth = currentHealth;
				MaxHealth = maxHealth;
			}
		}

		public struct GameOverEvent
		{
		}
	}
}