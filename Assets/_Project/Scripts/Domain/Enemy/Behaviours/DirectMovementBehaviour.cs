using UnityEngine;

namespace MagicalTower.Domain.Enemy.Behaviours
{
	[CreateAssetMenu(fileName = "DirectMovement", menuName = "MagicalTower/Behaviours/Movement/Direct")]
	public class DirectMovementBehaviour : MovementBehaviourBase
	{
		public override void Move(Transform enemy, Transform target, float speed, float deltaTime)
		{
			var direction = (target.position - enemy.position).normalized;
			enemy.position += direction * speed * deltaTime;
			enemy.LookAt(target.position);
		}
	}
}