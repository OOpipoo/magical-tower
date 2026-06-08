using System.Collections.Generic;
using MagicalTower.Core;
using MagicalTower.Domain.Enemy;
using MagicalTower.Domain.Tower;
using VContainer.Unity;

namespace MagicalTower.Systems
{
	public class CombatSystem : ITickable
	{
		private readonly Tower _tower;
		private readonly List<Enemy> _activeEnemies;
		private readonly GameStateService _gameState;

		public CombatSystem(Tower tower, List<Enemy> activeEnemies, GameStateService gameState)
		{
			_tower = tower;
			_activeEnemies = activeEnemies;
			_gameState = gameState;
		}

		public void Tick()
		{
			if (!_gameState.IsGameActive) return;
			_tower.ExecuteSpells(_activeEnemies);
		}
	}
}