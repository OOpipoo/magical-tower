using MagicalTower.Domain.Tower;
using VContainer.Unity;

namespace MagicalTower.Systems
{
	public class CombatSystem : ITickable
	{
		private readonly Tower _tower;
		private readonly SpawnSystem _spawnSystem;

		public CombatSystem(Tower tower, SpawnSystem spawnSystem)
		{
			_tower = tower;
			_spawnSystem = spawnSystem;
		}

		public void Tick()
		{
			_tower.ExecuteSpells(_spawnSystem.ActiveEnemies as System.Collections.Generic.List<Domain.Enemy.Enemy>);
		}
	}
}