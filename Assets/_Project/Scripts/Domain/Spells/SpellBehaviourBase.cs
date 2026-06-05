using UnityEngine;
using System.Collections.Generic;

namespace MagicalTower.Domain.Spells
{
	public abstract class SpellBehaviourBase : ScriptableObject
	{
		public float Cooldown = 3f;
		public float Damage = 30f;
		public float ProjectileSpeed = 10f;
		public GameObject ProjectilePrefab;

		public abstract void Execute(Vector3 origin, List<GameObject> targets);
	}
}