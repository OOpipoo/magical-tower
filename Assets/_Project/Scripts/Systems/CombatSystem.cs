using System.Collections.Generic;
using MagicalTower.Domain.Enemy;
using MagicalTower.Domain.Tower;
using VContainer.Unity;

namespace MagicalTower.Systems
{
	public class CombatSystem : ITickable
	{
		private readonly Tower _tower;
		private readonly List<Enemy> _activeEnemies;

		public CombatSystem(Tower tower, List<Enemy> activeEnemies)
		{
			_tower = tower;
			_activeEnemies = activeEnemies;
		}

		public void Tick()
		{
			_tower.ExecuteSpells(_activeEnemies);
		}
	}
}