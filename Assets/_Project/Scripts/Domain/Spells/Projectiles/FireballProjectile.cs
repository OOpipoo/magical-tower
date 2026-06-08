using System.Collections.Generic;
using System;
using MagicalTower.Systems;
using UnityEngine;

namespace MagicalTower.Domain.Spells.Projectiles
{
    public class FireballProjectile : ProjectileBase
    {
        private float _speed;
        private float _damage;
        private float _aoeRadius;
        private float _hitRadius;
        private float _burnDamagePerSecond;
        private float _burnDuration;
        
        private Action _onReturn;
        private Vector3 _direction;
        private DamageSystem _damageSystem;
        private List<Enemy.Enemy> _activeEnemies;
        private bool _hasExploded;

        public void Initialize(
            Vector3 direction,
            float speed,
            float damage,
            float aoeRadius,
            float hitRadius,
            float burnDamagePerSecond,
            float burnDuration,
            DamageSystem damageSystem,
            List<Enemy.Enemy> activeEnemies,
            System.Action onReturn)
        {
            _direction = direction;
            _speed = speed;
            _damage = damage;
            _aoeRadius = aoeRadius;
            _hitRadius= hitRadius;
            _burnDamagePerSecond = burnDamagePerSecond;
            _burnDuration = burnDuration;
            _damageSystem = damageSystem;
            _activeEnemies = activeEnemies;
            _onReturn = onReturn;
            _hasExploded = false;
        }
        
        protected override void Tick(float deltaTime)
        {
            transform.position += _direction * (_speed * deltaTime);

            foreach (var enemy in _activeEnemies)
            {
                if (!enemy.IsAlive) continue;
                if (Vector3.Distance(transform.position, enemy.transform.position) <= _hitRadius)
                {
                    Explode();
                    return;
                }
            }
        }

        protected override void OnLifetimeExpired()
        {
            Explode();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"OnTriggerEnter: {other.gameObject.name}, layer={other.gameObject.layer}");
            if (_hasExploded) return;
            Explode();
        }

        private void Explode()
        {
            _hasExploded = true;
            _damageSystem.DealAoeDamage(transform.position, _aoeRadius, _damage, _activeEnemies);

            foreach (var enemy in _activeEnemies)
            {
                if (!enemy.IsAlive) continue;
                if (Vector3.Distance(transform.position, enemy.transform.position) <= _aoeRadius)
                    _damageSystem.ApplyBurn(enemy, _burnDamagePerSecond, _burnDuration);
            }

            _onReturn?.Invoke();
        }
    }
}