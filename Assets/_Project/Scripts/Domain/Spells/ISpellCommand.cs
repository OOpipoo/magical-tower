using System.Collections.Generic;
using MagicalTower.Systems;
using UnityEngine;

namespace MagicalTower.Domain.Spells
{
	public interface ISpellCommand
	{
		bool IsReady { get; }
		void Execute(Vector3 origin, List<Enemy.Enemy> targets);
		void Tick(float deltaTime);
		void SetContext(DamageSystem damageSystem);
	}
}