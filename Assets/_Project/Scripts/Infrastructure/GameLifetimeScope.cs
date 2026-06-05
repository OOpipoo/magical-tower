using System.Collections.Generic;
using MagicalTower.Core;
using MagicalTower.Data;
using MagicalTower.Domain.Enemy;
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
        [SerializeField] private Camera _camera;

        [Header("Prefabs")]
        [SerializeField] private EnemyHealthBar _enemyHealthBarPrefab;
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
            builder.RegisterInstance(new EventBus());
            builder.RegisterInstance(new List<Enemy>());
            builder.RegisterInstance(_waveConfig);
            builder.RegisterInstance(_camera);
            builder.RegisterInstance(_tower);
            builder.RegisterInstance(_tower.GetComponent<TowerHealth>());
        }

        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<EnemyFactory>(Lifetime.Singleton).WithParameter(_enemyPoolParent);
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
            builder.Register<ObjectPool<EnemyHealthBar>>(Lifetime.Singleton)
                   .WithParameter(_enemyHealthBarPrefab)
                   .WithParameter(_uiPoolParent);

            builder.Register<ObjectPool<DamageNumber>>(Lifetime.Singleton)
                   .WithParameter(_damageNumberPrefab)
                   .WithParameter(_uiPoolParent);

            builder.Register<UIWorldProjectionContainer<EnemyHealthBar>>(Lifetime.Singleton)
                   .AsImplementedInterfaces()
                   .AsSelf();

            builder.Register<UIWorldProjectionContainer<DamageNumber>>(Lifetime.Singleton)
                   .AsImplementedInterfaces()
                   .AsSelf();

            builder.RegisterComponentInHierarchy<TowerHealthBarPresenter>()
                   .AsImplementedInterfaces()
                   .AsSelf();

            builder.RegisterComponentInHierarchy<DamageNumberPresenter>()
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}