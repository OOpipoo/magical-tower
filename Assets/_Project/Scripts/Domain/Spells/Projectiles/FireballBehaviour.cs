using System.Collections.Generic;
using MagicalTower.Domain.Spells.Projectiles;
using MagicalTower.Systems;
using UnityEngine;

namespace MagicalTower.Domain.Spells
{
	[CreateAssetMenu(fileName = "FireballBehaviour", menuName = "MagicalTower/Behaviours/Spells/Fireball")]
	public class FireballBehaviour : SpellBehaviourBase
	{
		public float AoeRadius = 3f;
		public float BurnDamagePerSecond = 5f;
		public float HitRadius = 0.8f;
		public float BurnDuration = 3f;

		public override ISpellCommand CreateCommand() => new FireballCommand(this);
	}

	public class FireballCommand : ISpellCommand
	{
		private readonly FireballBehaviour _config;
		private DamageSystem _damageSystem;
		private ProjectileFactory _projectileFactory;
		private float _cooldownTimer;

		public bool IsReady => _cooldownTimer <= 0f;
		public float Range => _config.Range;

		public FireballCommand(FireballBehaviour config)
		{
			_config = config;
			_cooldownTimer = config.Cooldown;;
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

			var target = targets[Random.Range(0, targets.Count)];
			var direction = (target.transform.position - origin).normalized;

			var projectile = _projectileFactory.Get<FireballProjectile>(_config.ProjectilePrefab);
			projectile.transform.position = origin;
			projectile.Initialize(
				direction,
				_config.ProjectileSpeed,
				_config.Damage,
				_config.AoeRadius,
				_config.HitRadius,
				_config.BurnDamagePerSecond,
				_config.BurnDuration,
				_damageSystem,
				targets,
				() => _projectileFactory.Return(projectile, _config.ProjectilePrefab));

			_cooldownTimer = _config.Cooldown;
		}
	}
}