using System.Collections.Generic;
using MagicalTower.Core;
using MagicalTower.Data;
using UnityEngine;

namespace MagicalTower.Domain.Enemy
{
	public class EnemyFactory
	{
		private readonly Dictionary<EnemyConfig, ObjectPool<Enemy>> _pools = new();
		private readonly Transform _poolParent;
		private readonly Transform _towerTransform;
		private readonly EventBus _eventBus;

		public EnemyFactory(
			Transform poolParent,
			Transform towerTransform,
			EventBus eventBus)
		{
			_poolParent = poolParent;
			_towerTransform = towerTransform;
			_eventBus = eventBus;
		}

		public Enemy Create(EnemyConfig config, Vector3 spawnPosition)
		{
			var pool = GetOrCreatePool(config);
			var enemy = pool.Get();
			enemy.transform.position = spawnPosition;
			enemy.transform.rotation = Quaternion.identity;
			enemy.Initialize(config, _towerTransform, _eventBus);
			return enemy;
		}

		public void Return(Enemy enemy)
		{
			var pool = GetOrCreatePool(enemy.Config);
			enemy.ResetState();
			pool.Return(enemy);
		}

		private ObjectPool<Enemy> GetOrCreatePool(EnemyConfig config)
		{
			if (_pools.TryGetValue(config, out var pool))
				return pool;

			var prefabEnemy = config.Prefab.GetComponent<Enemy>();
			var parent = new GameObject($"Pool_{config.name}").transform;
			parent.SetParent(_poolParent);

			var newPool = new ObjectPool<Enemy>(
				prefabEnemy,
				parent,
				initialSize: 5
			);

			_pools.Add(config, newPool);
			return newPool;
		}
	}
}