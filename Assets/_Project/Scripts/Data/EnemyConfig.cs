using MagicalTower.Domain.Enemy.Behaviours;
using UnityEngine;

namespace MagicalTower.Data
{
	[CreateAssetMenu(fileName = "EnemyConfig", menuName = "MagicalTower/Configs/Enemy")]
	public class EnemyConfig : ScriptableObject
	{
		[Header("Stats")]
		public float MaxHealth = 100f;
		public float MoveSpeed = 3f;

		[Header("Visual")]
		public Vector3 Scale = Vector3.one;
		public GameObject Prefab;

		[Header("Behaviours")]
		public MovementBehaviourBase Movement;
		public AttackBehaviourBase Attack;
		public DeathBehaviourBase Death;

		[Header("Reward")]
		public int ScoreValue = 10;
	}
}