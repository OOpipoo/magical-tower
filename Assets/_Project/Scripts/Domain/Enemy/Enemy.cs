using MagicalTower.Core;
using MagicalTower.Data;
using MagicalTower.Domain.Enemy.Behaviours;
using UnityEngine;

namespace MagicalTower.Domain.Enemy
{
    public class Enemy : MonoBehaviour
    {
        private MovementBehaviourBase _movement;
        private AttackBehaviourBase _attack;
        private DeathBehaviourBase _death;
        private Transform _towerTransform;
        private EventBus _eventBus;

        private float _currentHealth;
        private float _attackTimer;
        private float _rangeCheckTimer;
        private bool _isInAttackRange;

        public float HealthPercent => _currentHealth / Config.MaxHealth;
        public bool IsAlive { get; private set; }
        public EnemyConfig Config { get; private set; }

        
        public void Initialize(
            EnemyConfig config,
            Transform towerTransform,
            EventBus eventBus)
        {
            Config = config;
            _towerTransform = towerTransform;
            _eventBus = eventBus;

            _movement = config.Movement;
            _attack = config.Attack;
            _death = config.Death;

            _currentHealth = config.MaxHealth;
            IsAlive = true;
            _isInAttackRange = false;
            _rangeCheckTimer = 0f;
            _attackTimer = 0f;

            transform.localScale = config.Scale;
        }

        private void Update()
        {
            if (!IsAlive) return;

            UpdateRangeCheck();

            if (_isInAttackRange)
            {
                if (_attack.TryAttack(transform, _towerTransform, ref _attackTimer, Time.deltaTime))
                    _eventBus.Publish(new GameEvents.EnemyAttackedTowerEvent(Config.Attack.Damage));
            }
            else
            {
                _movement.Move(transform, _towerTransform, Config.MoveSpeed, Time.deltaTime);
            }
        }

        private void UpdateRangeCheck()
        {
            _rangeCheckTimer += Time.deltaTime;
            if (_rangeCheckTimer < Config.AttackRangeCheckInterval)
                return;

            _rangeCheckTimer = 0f;
            _isInAttackRange = Vector3.Distance(transform.position, _towerTransform.position) 
                               <= _attack.AttackRange;
        }

        public void TakeDamage(float amount)
        {
            if (!IsAlive) return;

            _currentHealth -= amount;
            _eventBus.Publish(new GameEvents.DamageDealtEvent(transform.position, amount));

            if (_currentHealth <= 0f)
                Die();
        }

        private void Die()
        {
            IsAlive = false;
            _death.OnDeath(transform);
            _eventBus.Publish(new GameEvents.EnemyDiedEvent(gameObject));
        }

        public void ResetState()
        {
            IsAlive = false;
            _isInAttackRange = false;
            _currentHealth = 0f;
            _attackTimer = 0f;
            _rangeCheckTimer = 0f;
        }
    }
}