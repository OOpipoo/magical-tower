using System.Collections.Generic;
using MagicalTower.Domain.Spells.Projectiles;
using MagicalTower.Systems;
using UnityEngine;

namespace MagicalTower.Domain.Spells
{
	[CreateAssetMenu(fileName = "ChainLightningBehaviour", menuName = "MagicalTower/Behaviours/Spells/ChainLightning")]
	public class ChainLightningBehaviour : SpellBehaviourBase
	{
		[Tooltip("Сколько раз снаряд рикошетит между врагами")]
		public int MaxBounces = 3;

		[Tooltip("Радиус поиска следующей цели при каждом рикошете")]
		public float BounceRadius = 8f;

		[Tooltip("Множитель урона на каждый рикошет (0.7 = -30%)")]
		[Range(0.1f, 1f)]
		public float DamageDropoff = 0.7f;

		[Tooltip("Множитель скорости замедленного врага (0.5 = -50%)")]
		[Range(0.1f, 1f)]
		public float SlowMultiplier = 0.5f;

		[Tooltip("Длительность замедления в секундах")]
		public float SlowDuration = 2f;

		public override ISpellCommand CreateCommand() => new ChainLightningCommand(this);
	}

	public class ChainLightningCommand : ISpellCommand
	{
		private readonly ChainLightningBehaviour _config;
		private DamageSystem _damageSystem;
		private ProjectileFactory _projectileFactory;
		private float _cooldownTimer;

		public bool IsReady => _cooldownTimer <= 0f;
		public float Range => _config.Range;

		public ChainLightningCommand(ChainLightningBehaviour config)
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

		private List<Enemy.Enemy> _allTargets;

		public void Execute(Vector3 origin, List<Enemy.Enemy> targets)
		{
			if (targets.Count == 0) return;

			_allTargets = targets;
			var target = targets[Random.Range(0, targets.Count)];
			var hitEnemies = new HashSet<Enemy.Enemy>();

			SpawnProjectile(origin, target, _config.Damage, _config.MaxBounces, hitEnemies);

			_cooldownTimer = _config.Cooldown;
		}

		private void SpawnProjectile(
			Vector3 from,
			Enemy.Enemy target,
			float damage,
			int bouncesLeft,
			HashSet<Enemy.Enemy> hitEnemies)
		{
			var projectile = _projectileFactory.Get<ChainLightningProjectile>(_config.ProjectilePrefab);
			projectile.transform.position = from;
			projectile.Initialize(
				target,
				_config.ProjectileSpeed,
				damage,
				bouncesLeft,
				_config.BounceRadius,
				_config.DamageDropoff,
				_config.SlowMultiplier,
				_config.SlowDuration,
				_damageSystem,
				_allTargets,
				hitEnemies,
				(p, nextTarget, nextDamage, nextBounces, hits) =>
					SpawnProjectile(p.transform.position + Vector3.up * 0.5f, nextTarget, nextDamage, nextBounces, hits),
				() => _projectileFactory.Return(projectile, _config.ProjectilePrefab));
		}
	}
}
