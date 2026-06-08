using System.Collections.Generic;
using MagicalTower.Core;
using MagicalTower.Data;
using MagicalTower.Domain.Enemy;
using MagicalTower.Domain.Spells.Projectiles;
using MagicalTower.Domain.Tower;
using MagicalTower.Systems;
using MagicalTower.UI;
using MagicalTower.UI.Core;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MagicalTower.Infrastructure
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Configs")]
        [SerializeField] private WaveConfig _waveConfig;

        [Header("Scene References")]
        [SerializeField] private Tower _tower;
		[SerializeField] private TowerHealth _towerHealth;
        [SerializeField] private Camera _camera;

        [Header("Prefabs")]
        [SerializeField] private DamageNumber _damageNumberPrefab;

        [Header("Pool Parents")]
        [SerializeField] private Transform _enemyPoolParent;
        [SerializeField] private Transform _uiPoolParent;


		protected override void Configure(IContainerBuilder builder)
		{
			RegisterCore(builder);
			RegisterFactories(builder);
			RegisterSystems(builder);
			RegisterUI(builder);
		}

		private void RegisterCore(IContainerBuilder builder)
		{
			var eventBus = new EventBus();
			var activeEnemies = new List<Enemy>();

			builder.RegisterInstance(eventBus);
			builder.RegisterInstance(activeEnemies);
			builder.RegisterInstance(_waveConfig);
			builder.RegisterInstance(_camera);
			builder.RegisterInstance(_towerHealth);
    
			builder.RegisterComponent(_tower)
				.AsImplementedInterfaces()
				.AsSelf();
		}

		private void RegisterFactories(IContainerBuilder builder)
		{
			builder.Register(resolver => new EnemyFactory(
				_enemyPoolParent,
				_tower.transform,
				resolver.Resolve<EventBus>()
			), Lifetime.Singleton);
			
			builder.Register(resolver => new ProjectileFactory(
				_enemyPoolParent), Lifetime.Singleton);
		}

		private void RegisterSystems(IContainerBuilder builder)
		{
			builder.Register<SpawnSystem>(Lifetime.Singleton)
				.AsImplementedInterfaces()
				.AsSelf();

			builder.Register<CombatSystem>(Lifetime.Singleton)
				.AsImplementedInterfaces()
				.AsSelf();

			builder.Register<DamageSystem>(Lifetime.Singleton);
		}

		private void RegisterUI(IContainerBuilder builder)
		{
			builder.RegisterInstance(
				new ObjectPool<DamageNumber>(_damageNumberPrefab, _uiPoolParent, 10));

			builder.RegisterComponentInHierarchy<UIManager>()
				.AsImplementedInterfaces()
				.AsSelf();
		}
	}
}