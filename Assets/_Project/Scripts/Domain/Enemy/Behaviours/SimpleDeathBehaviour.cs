using UnityEngine;

namespace MagicalTower.Domain.Enemy.Behaviours
{
	[CreateAssetMenu(fileName = "SimpleDeath", menuName = "MagicalTower/Behaviours/Death/Simple")]
	public class SimpleDeathBehaviour : DeathBehaviourBase
	{
		public override void OnDeath(Transform enemy)
		{ }
	}
}