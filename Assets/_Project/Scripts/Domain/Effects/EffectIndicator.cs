using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagicalTower.Domain.Effects
{
	public class EffectIndicator : MonoBehaviour
	{
		private static readonly Dictionary<PrimitiveType, Mesh> MeshCache = new();

		private MeshRenderer _renderer;
		private MeshFilter _meshFilter;
		private Material _material;
		private Color _baseColor;
		private float _duration;
		private float _elapsed;
		private Action _onComplete;
		private bool _active;

		private void Awake()
		{
			var child = new GameObject("Visual");
			child.transform.SetParent(transform, false);

			_meshFilter = child.AddComponent<MeshFilter>();
			_renderer = child.AddComponent<MeshRenderer>();
			_material = CreateTransparentMaterial();
			_renderer.material = _material;
		}

		public void Initialize(EffectVisualEntry config, float duration, Transform enemyTransform, Action onComplete)
		{
			transform.SetParent(enemyTransform, false);
			transform.localPosition = new Vector3(0f, config.YOffset, 0f);

			_meshFilter.sharedMesh = GetCachedMesh(config.Shape);
			_renderer.transform.localScale = Vector3.one * config.Scale;

			_baseColor = config.Color;
			_duration = Mathf.Max(duration, 0.01f);
			_elapsed = 0f;
			_onComplete = onComplete;
			_active = true;

			ApplyAlpha(1f);
		}

		private void Update()
		{
			if (!_active) return;

			_elapsed += Time.deltaTime;
			ApplyAlpha(1f - Mathf.Clamp01(_elapsed / _duration));

			if (_elapsed >= _duration)
				Complete();
		}

		private void Complete()
		{
			_active = false;
			transform.SetParent(null);
			_onComplete?.Invoke();
		}

		private void ApplyAlpha(float alpha)
		{
			var c = _baseColor;
			c.a = alpha;
			_material.color = c;
		}

		private static Mesh GetCachedMesh(PrimitiveType type)
		{
			if (MeshCache.TryGetValue(type, out var cached))
				return cached;

			var go = GameObject.CreatePrimitive(type);
			var mesh = go.GetComponent<MeshFilter>().sharedMesh;
			Destroy(go);
			MeshCache[type] = mesh;
			return mesh;
		}

		private static Material CreateTransparentMaterial()
		{
			var mat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
			mat.SetFloat("_Surface", 1f);
			mat.SetFloat("_Blend", 0f);
			mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
			mat.renderQueue = 3000;
			return mat;
		}
	}
}
