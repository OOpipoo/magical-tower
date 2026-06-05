using UnityEngine;

namespace MagicalTower.Domain.Enemy.Behaviours
{
	public abstract class DeathBehaviourBase : ScriptableObject
	{
		public abstract void OnDeath(Transform enemy);
	}
}