using UnityEngine;

namespace MagicalTower.Core
{
	public static class GameEvents
	{
		public struct DamageDealtEvent
		{
			public readonly Vector3 Position;
			public readonly float Amount;

			public DamageDealtEvent(Vector3 position, float amount)
			{
				Position = position;
				Amount = amount;
			}
		}

		public struct EnemyDiedEvent
		{
			public readonly GameObject Enemy;

			public EnemyDiedEvent(GameObject enemy)
			{
				Enemy = enemy;
			}
		}

		public struct EnemyAttackedTowerEvent
		{
			public readonly float Damage;

			public EnemyAttackedTowerEvent(float damage)
			{
				Damage = damage;
			}
		}

		public struct TowerHealthChangedEvent
		{
			public readonly float CurrentHealth;
			public readonly float MaxHealth;
			public readonly float Damage;

			public TowerHealthChangedEvent(float currentHealth, float maxHealth, float damage)
			{
				CurrentHealth = currentHealth;
				MaxHealth = maxHealth;
				Damage = damage;
			}
		}

		public struct WaveChangedEvent
		{
			public readonly int CurrentWave;
			public readonly int TotalWaves;

			public WaveChangedEvent(int currentWave, int totalWaves)
			{
				CurrentWave = currentWave;
				TotalWaves = totalWaves;
			}
		}

		public struct GameOverEvent { }

		public struct GameWonEvent { }
	}
}