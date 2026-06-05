using UnityEngine;

namespace MagicalTower.Domain.Enemy.Behaviours
{
	[CreateAssetMenu(fileName = "MeleeAttack", menuName = "MagicalTower/Behaviours/Attack/Melee")]
	public class MeleeAttackBehaviour : AttackBehaviourBase
	{
		public override bool TryAttack(Transform enemy, Transform target, ref float attackTimer, float deltaTime)
		{
			attackTimer += deltaTime;
			if (attackTimer < 1f / AttackRate)
				return false;

			attackTimer = 0f;
			return Vector3.Distance(enemy.position, target.position) <= AttackRange;
		}
	}
}