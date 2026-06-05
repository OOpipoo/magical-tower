using UnityEngine;

namespace MagicalTower.Domain.Enemy.Behaviours
{
	public abstract class MovementBehaviourBase : ScriptableObject
	{
		public abstract void Move(Transform enemy, Transform target, float speed, float deltaTime);
	}
}