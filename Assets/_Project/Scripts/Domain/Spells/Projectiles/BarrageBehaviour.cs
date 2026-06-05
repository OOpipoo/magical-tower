using System.Collections.Generic;
using MagicalTower.Domain.Spells.Projectiles;
using UnityEngine;

namespace MagicalTower.Domain.Spells
{
	[CreateAssetMenu(fileName = "BarrageBehaviour", menuName = "MagicalTower/Behaviours/Spells/Barrage")]
	public class BarrageBehaviour : SpellBehaviourBase
	{
		public float ArcHeight = 3f;

		public override ISpellCommand CreateCommand() => new BarrageCommand(this);
	}

	public class BarrageCommand : ISpellCommand
	{
		private readonly BarrageBehaviour _config;
		private float _cooldownTimer;

		public bool IsReady => _cooldownTimer <= 0f;

		public BarrageCommand(BarrageBehaviour config)
		{
			_config = config;
			_cooldownTimer = 0f;
		}

		public void Tick(float deltaTime)
		{
			if (_cooldownTimer > 0f)
				_cooldownTimer -= deltaTime;
		}

		public void Execute(Vector3 origin, List<Enemy.Enemy> targets)
		{
			if (targets.Count == 0) return;

			foreach (var target in targets)
			{
				var projectileGo = Object.Instantiate(_config.ProjectilePrefab, origin, Quaternion.identity);
				var projectile = projectileGo.GetComponent<BarrageProjectile>();
			}

			_cooldownTimer = _config.Cooldown;
		}
	}
}