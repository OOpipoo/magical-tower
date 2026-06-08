using System.Collections.Generic;
using MagicalTower.Domain.Spells.Projectiles;
using MagicalTower.Systems;
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
		private DamageSystem _damageSystem;
		private ProjectileFactory _projectileFactory;
		private float _cooldownTimer;

		public bool IsReady => _cooldownTimer <= 0f;
		public float Range => _config.Range;

		public BarrageCommand(BarrageBehaviour config)
		{
			_config = config;
			_cooldownTimer = config.Cooldown;
		}

		public void SetContext(DamageSystem damageSystem, ProjectileFactory projectileFactory)
		{
			_damageSystem = damageSystem;
			_projectileFactory = projectileFactory;
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
				var projectile = _projectileFactory.Get<BarrageProjectile>(_config.ProjectilePrefab);
				projectile.transform.position = origin;
				projectile.Initialize(
					target,
					_config.ProjectileSpeed,
					_config.Damage,
					_config.ArcHeight,
					_damageSystem,
					() => _projectileFactory.Return(projectile, _config.ProjectilePrefab));
			}

			_cooldownTimer = _config.Cooldown;
		}
	}
}