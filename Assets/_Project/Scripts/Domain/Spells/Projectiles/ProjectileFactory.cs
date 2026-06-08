using System.Collections.Generic;
using MagicalTower.Core;
using UnityEngine;

namespace MagicalTower.Domain.Spells.Projectiles
{
	public class ProjectileFactory
	{
		private readonly Dictionary<GameObject, ObjectPool<ProjectileBase>> _pools = new();
		private readonly Transform _poolParent;

		public ProjectileFactory(Transform poolParent)
		{
			_poolParent = poolParent;
		}

		public T Get<T>(GameObject prefab) where T : ProjectileBase
		{
			return GetOrCreatePool(prefab).Get() as T;
		}

		public void Return(ProjectileBase projectile, GameObject prefab)
		{
			GetOrCreatePool(prefab).Return(projectile);
		}

		private ObjectPool<ProjectileBase> GetOrCreatePool(GameObject prefab)
		{
			if (_pools.TryGetValue(prefab, out var pool))
				return pool;

			var parent = new GameObject($"Pool_{prefab.name}").transform;
			parent.SetParent(_poolParent);

			var newPool = new ObjectPool<ProjectileBase>(
				prefab.GetComponent<ProjectileBase>(),
				parent,
				5);

			_pools.Add(prefab, newPool);
			return newPool;
		}
	}
}