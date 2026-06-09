using System.Collections.Generic;
using MagicalTower.Core;
using MagicalTower.Data;
using MagicalTower.Domain.Effects;
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
		private FlashEffect _flashEffect;
		private GameStateService _gameState;

		private float _currentHealth;
		private float _attackTimer;
		private float _rangeCheckTimer;
		private bool _isInAttackRange;

		private readonly List<StatusEffect> _effects = new();

		public float HealthPercent => _currentHealth / Config.MaxHealth;
		public bool IsAlive { get; private set; }
		public EnemyConfig Config { get; private set; }

		public void Initialize(
			EnemyConfig config,
			Transform towerTransform,
			EventBus eventBus,
			GameStateService gameState)
		{
			Config = config;
			_towerTransform = towerTransform;
			_eventBus = eventBus;
			_gameState = gameState;
			_flashEffect = GetComponent<FlashEffect>();

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
			if (!_gameState.IsGameActive) return;

			UpdateEffects();
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

		private void UpdateEffects()
		{
			if (!IsAlive) return;

			for (var i = _effects.Count - 1; i >= 0; i--)
			{
				var effect = _effects[i];
				effect.Tick(this, Time.deltaTime);

				if (!IsAlive) return;

				if (effect.IsExpired)
					_effects.RemoveAt(i);
			}
		}

		public void AddEffect(StatusEffect effect)
		{
			_effects.Add(effect);
		}

		public void TakeDamage(float amount)
		{
			if (!IsAlive) return;

			_currentHealth -= amount;
			_flashEffect?.Flash();
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
			_effects.Clear();
		}
	}
}