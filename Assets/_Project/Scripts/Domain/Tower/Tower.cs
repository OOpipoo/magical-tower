using System.Collections.Generic;
using MagicalTower.Core;
using MagicalTower.Domain.Spells;
using UnityEngine;
using VContainer;

namespace MagicalTower.Domain.Tower
{
	public class Tower : MonoBehaviour
	{
		[SerializeField] private float _maxHealth = 500f;
		[SerializeField] private List<SpellBehaviourBase> _spells = new();

		private TowerHealth _health;
		private EventBus _eventBus;
		private List<ISpellCommand> _spellCommands = new();

		public Transform Transform => transform;

		[Inject]
		public void Construct(EventBus eventBus)
		{
			_eventBus = eventBus;
		}

		private void Start()
		{
			_health = GetComponent<TowerHealth>();
			_health.Initialize(_maxHealth, _eventBus);

			foreach (var spell in _spells)
				_spellCommands.Add(spell.CreateCommand());

			_eventBus.Subscribe<GameEvents.TowerHealthChangedEvent>(OnTowerHealthChanged);
		}

		private void OnDestroy()
		{
			_eventBus.Unsubscribe<GameEvents.TowerHealthChangedEvent>(OnTowerHealthChanged);
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
					command.Execute(transform.position, activeEnemies);
			}
		}
	}
}