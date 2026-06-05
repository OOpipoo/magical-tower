using System.Collections.Generic;
using MagicalTower.Core;
using UnityEngine;
using VContainer.Unity;

namespace MagicalTower.UI.Core
{
	public class UIWorldProjectionContainer<T> : ILateTickable where T : Component, IProjectedUI
	{
		private readonly ObjectPool<T> _pool;
		private readonly Camera _camera;
		private readonly Dictionary<int, T> _active = new();
		private readonly Dictionary<int, Transform> _targets = new();

		public UIWorldProjectionContainer(ObjectPool<T> pool, Camera camera)
		{
			_pool = pool;
			_camera = camera;
		}

		public T Attach(Transform worldTarget)
		{
			var ui = _pool.Get();
			var id = worldTarget.GetInstanceID();
			_active[id] = ui;
			_targets[id] = worldTarget;
			ui.OnSpawn();
			return ui;
		}

		public void Detach(Transform worldTarget)
		{
			var id = worldTarget.GetInstanceID();
			if (!_active.TryGetValue(id, out var ui))
				return;

			ui.OnReturn();
			_pool.Return(ui);
			_active.Remove(id);
			_targets.Remove(id);
		}

		public void LateTick()
		{
			foreach (var (id, ui) in _active)
			{
				if (!_targets.TryGetValue(id, out var target) || target == null)
					continue;

				var screenPos = _camera.WorldToScreenPoint(target.position + ui.WorldOffset);
				ui.RectTransform.position = screenPos;
			}
		}
	}
}