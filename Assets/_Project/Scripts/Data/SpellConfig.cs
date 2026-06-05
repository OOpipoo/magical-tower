using MagicalTower.Domain.Spells;
using UnityEngine;

namespace MagicalTower.Data
{
	[CreateAssetMenu(fileName = "SpellConfig", menuName = "MagicalTower/Configs/Spell")]
	public class SpellConfig : ScriptableObject
	{
		public SpellBehaviourBase Behaviour;
	}
}