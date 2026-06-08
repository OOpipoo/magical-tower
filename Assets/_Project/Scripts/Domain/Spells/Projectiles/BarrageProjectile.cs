using System;
using MagicalTower.Domain.Enemy;
using MagicalTower.Systems;
using UnityEngine;

namespace MagicalTower.Domain.Spells.Projectiles
{
	public class BarrageProjectile : ProjectileBase
	{
		private Enemy.Enemy _target;
		private float _damage;
		private float _speed;
		private DamageSystem _damageSystem;

		private Action _onReturn;
		private Vector3 _startPosition;
		private Vector3 _targetPosition;
		private float _progress;
		private float _arcHeight = 3f;
		private bool _hasHit;

		public void Initialize(
			Enemy.Enemy target,
			float speed,
			float damage,
			float arcHeight,
			DamageSystem damageSystem,
			System.Action onReturn)
		{
			_target = target;
			_speed = speed;
			_damage = damage;
			_arcHeight = arcHeight;
			_damageSystem = damageSystem;
			_onReturn = onReturn;
			_startPosition = transform.position;
			_targetPosition = target.transform.position;
			_progress = 0f;
			_hasHit = false;
		}

		protected override void Tick(float deltaTime)
		{
			if (_hasHit) return;

			if (_target != null && _target.IsAlive)
				_targetPosition = _target.transform.position;

			_progress += _speed * deltaTime;

			var linearPosition = Vector3.Lerp(_startPosition, _targetPosition, _progress);
			var arc = Mathf.Sin(_progress * Mathf.PI) * _arcHeight;
			transform.position = linearPosition + Vector3.up * arc;

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
				_damageSystem.DealDamage(_target, _damage);

			_onReturn?.Invoke();
		}
	}
}