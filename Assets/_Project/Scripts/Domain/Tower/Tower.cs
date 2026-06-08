using System.Collections.Generic;
using MagicalTower.Core;
using MagicalTower.Domain.Spells;
using MagicalTower.Domain.Spells.Projectiles;
using MagicalTower.Systems;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MagicalTower.Domain.Tower
{
	public class Tower : MonoBehaviour, IInitializable
	{
		[Header("Base Settings")]
		[SerializeField] private float _maxHealth = 500f;
		[SerializeField] private Transform _shootingPoint;
		[Header("Spells Settings")]
		[SerializeField] private List<SpellBehaviourBase> _spells = new();

		private TowerHealth _health;
		private EventBus _eventBus;
		private DamageSystem _damageSystem;
		private ProjectileFactory _projectileFactory;
		
		private readonly List<ISpellCommand> _spellCommands = new();
		private readonly List<Enemy.Enemy> _targetsInRange = new();
	

		public Transform Transform => transform;
		public float MaxHealth => _maxHealth;

		
		[Inject]
		public void Construct(EventBus eventBus, TowerHealth health, DamageSystem damageSystem, ProjectileFactory projectileFactory)
		{
			_eventBus = eventBus;
			_health = health;
			_damageSystem = damageSystem;
			_projectileFactory = projectileFactory;
		}

		public void Initialize()
		{
			_health.Initialize(_maxHealth, _eventBus);

			foreach (var spell in _spells)
			{
				var command = spell.CreateCommand();
				command.SetContext(_damageSystem, _projectileFactory);
				_spellCommands.Add(command);
			}

			_eventBus.Subscribe<GameEvents.TowerHealthChangedEvent>(OnTowerHealthChanged);
		}

		private void OnDestroy()
		{
			_eventBus?.Unsubscribe<GameEvents.TowerHealthChangedEvent>(OnTowerHealthChanged);
		}

		private void OnTowerHealthChanged(GameEvents.TowerHealthChangedEvent evt)
		{
			if (evt.CurrentHealth <= 0f)
				gameObject.SetActive(false);
		}

		public void ExecuteSpells(List<Enemy.Enemy> activeEnemies)
		{
			foreach (var command in _spellCommands)
				command.Tick(Time.deltaTime);

			foreach (var command in _spellCommands)
			{
				if (!command.IsReady) continue;

				_targetsInRange.Clear();
				foreach (var enemy in activeEnemies)
				{
					if (enemy.IsAlive &&
						Vector3.Distance(transform.position, enemy.transform.position) <= command.Range)
						_targetsInRange.Add(enemy);
				}

				command.Execute(_shootingPoint.position, _targetsInRange);
			}
		}
	}
}