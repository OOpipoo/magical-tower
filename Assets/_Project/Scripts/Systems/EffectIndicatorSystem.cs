using System.Collections.Generic;
using MagicalTower.Domain.Effects;
using UnityEngine;

namespace MagicalTower.Systems
{
	public class EffectIndicatorSystem
	{
		private readonly EffectIndicatorConfig _config;
		private readonly Transform _poolParent;
		private readonly Stack<EffectIndicator> _pool = new();

		public EffectIndicatorSystem(EffectIndicatorConfig config, Transform poolParent)
		{
			_config = config;
			_poolParent = poolParent;
		}

		public void Show(Domain.Enemy.Enemy enemy, string effectId, float duration)
		{
			if (_config == null) return;
			if (!_config.TryGet(effectId, out var entry)) return;

			var indicator = GetOrCreate();
			indicator.gameObject.SetActive(true);
			indicator.Initialize(entry, duration, enemy.EffectHolder, () => Return(indicator));
		}

		private EffectIndicator GetOrCreate()
		{
			if (_pool.Count > 0)
				return _pool.Pop();

			var go = new GameObject("EffectIndicator");
			go.transform.SetParent(_poolParent);
			return go.AddComponent<EffectIndicator>();
		}

		private void Return(EffectIndicator indicator)
		{
			indicator.gameObject.SetActive(false);
			indicator.transform.SetParent(_poolParent);
			_pool.Push(indicator);
		}
	}
}
