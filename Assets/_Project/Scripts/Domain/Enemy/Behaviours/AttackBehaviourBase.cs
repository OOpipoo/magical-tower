using UnityEngine;

namespace MagicalTower.Domain.Enemy.Behaviours
{
	public abstract class AttackBehaviourBase : ScriptableObject
	{
		public float Damage = 10f;
		public float AttackRate = 1f;
		public float AttackRange = 1.5f;

		public abstract void Attack(Transform enemy, Transform target, float deltaTime);
	}
}