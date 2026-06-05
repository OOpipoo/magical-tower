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
		public float BurnDuration = 3f;

		public override ISpellCommand CreateCommand() => new FireballCommand(this);
	}

	public class FireballCommand : ISpellCommand
	{
		private readonly FireballBehaviour _config;
		private DamageSystem _damageSystem;
		private float _cooldownTimer;

		public bool IsReady => _cooldownTimer <= 0f;

		public FireballCommand(FireballBehaviour config)
		{
			_config = config;
			_cooldownTimer = config.Cooldown;;
		}

		public void SetContext(DamageSystem damageSystem)
		{
			_damageSystem = damageSystem;
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

			var projectileGo = Object.Instantiate(_config.ProjectilePrefab, origin, Quaternion.identity);
			var projectile = projectileGo.GetComponent<FireballProjectile>();
			projectile.Initialize(
				direction,
				_config.ProjectileSpeed,
				_config.Damage,
				_config.AoeRadius,
				_config.BurnDamagePerSecond,
				_config.BurnDuration,
				_damageSystem,
				targets);

			_cooldownTimer = _config.Cooldown;
		}
	}
}