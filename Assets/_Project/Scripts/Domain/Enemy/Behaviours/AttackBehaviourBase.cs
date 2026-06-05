using UnityEngine;

namespace MagicalTower.Domain.Enemy.Behaviours
{
	public abstract class AttackBehaviourBase : ScriptableObject
	{
		public float Damage = 10f;
		public float AttackRate = 1f;
		public float AttackRange = 1.5f;

		public abstract bool TryAttack(Transform enemy, Transform target, ref float attackTimer, float deltaTime);
	}
}