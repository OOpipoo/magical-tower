using System.Collections.Generic;
using MagicalTower.Systems;
using UnityEngine;

namespace MagicalTower.Domain.Spells.Projectiles
{
    public class FireballProjectile : MonoBehaviour
    {
        private float _speed;
        private float _damage;
        private float _aoeRadius;
        private float _burnDamagePerSecond;
        private float _burnDuration;
        private Vector3 _direction;
        private DamageSystem _damageSystem;
        private List<Enemy.Enemy> _activeEnemies;
        private bool _hasExploded;

        public void Initialize(
            Vector3 direction,
            float speed,
            float damage,
            float aoeRadius,
            float burnDamagePerSecond,
            float burnDuration,
            DamageSystem damageSystem,
            List<Enemy.Enemy> activeEnemies)
        {
            _direction = direction;
            _speed = speed;
            _damage = damage;
            _aoeRadius = aoeRadius;
            _burnDamagePerSecond = burnDamagePerSecond;
            _burnDuration = burnDuration;
            _damageSystem = damageSystem;
            _activeEnemies = activeEnemies;
            _hasExploded = false;
        }

        private void Update()
        {
            transform.position += _direction * (_speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
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

            gameObject.SetActive(false);
        }
    }
}