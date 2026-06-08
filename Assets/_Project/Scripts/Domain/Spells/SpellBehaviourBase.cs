using System.Collections.Generic;
using UnityEngine;

namespace MagicalTower.Domain.Spells
{
	public abstract class SpellBehaviourBase : ScriptableObject
	{
		public GameObject ProjectilePrefab;
		[Space]
		public float Range = 20f;
		public float Cooldown = 3f;
		public float Damage = 30f;
		public float ProjectileSpeed = 10f;

		public abstract ISpellCommand CreateCommand();
	}
}