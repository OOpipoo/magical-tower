using System;
using System.Collections.Generic;
using MagicalTower.Systems;
using UnityEngine;

namespace MagicalTower.Domain.Spells.Projectiles
{
	public class ChainLightningProjectile : ProjectileBase
	{
		private Enemy.Enemy _target;
		private float _damage;
		private float _speed;
		private int _bouncesLeft;
		private float _bounceRadius;
		private float _damageDropoff;
		private float _slowMultiplier;
		private float _slowDuration;

		private DamageSystem _damageSystem;
		private List<Enemy.Enemy> _activeEnemies;
		private HashSet<Enemy.Enemy> _hitEnemies;
		private Action<ChainLightningProjectile, Enemy.Enemy, float, int, HashSet<Enemy.Enemy>> _onBounce;
		private Action _onReturn;

		private Vector3 _startPosition;
		private Vector3 _targetPosition;
		private float _distance;
		private float _progress;
		private bool _hasHit;

		public void Initialize(
			Enemy.Enemy target,
			float speed,
			float damage,
			int bouncesLeft,
			float bounceRadius,
			float damageDropoff,
			float slowMultiplier,
			float slowDuration,
			DamageSystem damageSystem,
			List<Enemy.Enemy> activeEnemies,
			HashSet<Enemy.Enemy> hitEnemies,
			Action<ChainLightningProjectile, Enemy.Enemy, float, int, HashSet<Enemy.Enemy>> onBounce,
			Action onReturn)
		{
			_target = target;
			_speed = speed;
			_damage = damage;
			_bouncesLeft = bouncesLeft;
			_bounceRadius = bounceRadius;
			_damageDropoff = damageDropoff;
			_slowMultiplier = slowMultiplier;
			_slowDuration = slowDuration;
			_damageSystem = damageSystem;
			_activeEnemies = activeEnemies;
			_hitEnemies = hitEnemies;
			_onBounce = onBounce;
			_onReturn = onReturn;

			_startPosition = transform.position;
			_targetPosition = target.transform.position;
			_distance = Mathf.Max(Vector3.Distance(_startPosition, _targetPosition), 0.01f);
			_progress = 0f;
			_hasHit = false;
		}

		[SerializeField] private float _rotationSpeed = 360f;

		protected override void Tick(float deltaTime)
		{
			if (_hasHit) return;

			if (_target != null && _target.IsAlive)
				_targetPosition = _target.transform.position;

			_progress += (_speed / _distance) * deltaTime;
			transform.position = Vector3.Lerp(_startPosition, _targetPosition, _progress);
			transform.Rotate(Vector3.up, _rotationSpeed * deltaTime, Space.World);

			if (_progress >= 1f)
				Hit();
		}

		protected override void OnLifetimeExpired()
		{
			_onReturn?.Invoke();
		}

		private void Hit()
		{
			_hasHit = true;

			if (_target != null && _target.IsAlive)
			{
				_damageSystem.DealDamage(_target, _damage);
				_damageSystem.ApplySlow(_target, _slowMultiplier, _slowDuration);
				_hitEnemies.Add(_target);
			}

			if (_bouncesLeft > 0)
			{
				var nextTarget = FindNextTarget();
				if (nextTarget != null)
					_onBounce?.Invoke(this, nextTarget, _damage * _damageDropoff, _bouncesLeft - 1, _hitEnemies);
			}

			_onReturn?.Invoke();
		}

		private Enemy.Enemy FindNextTarget()
		{
			Enemy.Enemy closest = null;
			var closestDist = float.MaxValue;

			foreach (var enemy in _activeEnemies)
			{
				if (!enemy.IsAlive) continue;
				if (_hitEnemies.Contains(enemy)) continue;

				var dist = Vector3.Distance(_targetPosition, enemy.transform.position);
				if (dist <= _bounceRadius && dist < closestDist)
				{
					closestDist = dist;
					closest = enemy;
				}
			}

			return closest;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_hitEnemies = null;
			transform.rotation = Quaternion.identity;
		}
	}
}
