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
		private float _cooldownTimer;

		public bool IsReady => _cooldownTimer <= 0f;

		public FireballCommand(FireballBehaviour config)
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

			var target = targets[Random.Range(0, targets.Count)];
			var direction = (target.transform.position - origin).normalized;

			var projectileGo = Object.Instantiate(_config.ProjectilePrefab, origin, Quaternion.identity);
			var projectile = projectileGo.GetComponent<FireballProjectile>();

			_cooldownTimer = _config.Cooldown;
		}
	}
}