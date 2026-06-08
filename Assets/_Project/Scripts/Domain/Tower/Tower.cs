using System.Collections.Generic;
using MagicalTower.Core;
using MagicalTower.Domain.Spells;
using MagicalTower.Systems;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MagicalTower.Domain.Tower
{
	public class Tower : MonoBehaviour, IInitializable
	{
		[SerializeField] private Transform _shootingPoint;
		[SerializeField] private float _maxHealth = 500f;
		[SerializeField] private List<SpellBehaviourBase> _spells = new();

		private TowerHealth _health;
		private EventBus _eventBus;
		private DamageSystem _damageSystem;
		private List<ISpellCommand> _spellCommands = new();

		public Transform Transform => transform;
		public float MaxHealth => _maxHealth;

		
		[Inject]
		public void Construct(EventBus eventBus, TowerHealth health, DamageSystem damageSystem)
		{
			_eventBus = eventBus;
			_health = health;
			_damageSystem = damageSystem;
		}

		public void Initialize()
		{
			_health.Initialize(_maxHealth, _eventBus);

			foreach (var spell in _spells)
			{
				var command = spell.CreateCommand();
				command.SetContext(_damageSystem);
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
				if (command.IsReady)
					command.Execute(_shootingPoint.position, activeEnemies);
			}
		}
	}
}