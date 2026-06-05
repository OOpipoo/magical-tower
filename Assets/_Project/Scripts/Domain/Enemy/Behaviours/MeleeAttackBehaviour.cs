using UnityEngine;

namespace MagicalTower.Domain.Enemy.Behaviours
{
	[CreateAssetMenu(fileName = "MeleeAttack", menuName = "MagicalTower/Behaviours/Attack/Melee")]
	public class MeleeAttackBehaviour : AttackBehaviourBase
	{
		private float _attackTimer;

		public override void Attack(Transform enemy, Transform target, float deltaTime)
		{
			_attackTimer += deltaTime;
			if (_attackTimer < 1f / AttackRate)
				return;

			_attackTimer = 0f;
			var distance = Vector3.Distance(enemy.position, target.position);
			if (distance <= AttackRange)
				OnAttackLanded(target);
		}

		protected virtual void OnAttackLanded(Transform target)
		{ }
	}
}