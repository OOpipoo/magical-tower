using DG.Tweening;
using UnityEngine;

namespace MagicalTower.Domain
{
	public class FlashEffect : MonoBehaviour
	{
		[SerializeField] private Renderer[] _renderers;
		[SerializeField] private float _flashDuration = 0.1f;

		private MaterialPropertyBlock _propertyBlock;
		private static readonly int _baseColor = Shader.PropertyToID("_BaseColor");

		
		private void Awake()
		{
			_propertyBlock = new MaterialPropertyBlock();
		}

		public void Flash()
		{
			var originalColors = new Color[_renderers.Length];
			for (int i = 0; i < _renderers.Length; i++)
				originalColors[i] = _renderers[i].material.GetColor(_baseColor);

			DOTween.To(
				() => 0f,
				t =>
				{
					for (int i = 0; i < _renderers.Length; i++)
					{
						var color = Color.Lerp(Color.white, originalColors[i], t);
						_renderers[i].GetPropertyBlock(_propertyBlock);
						_propertyBlock.SetColor(_baseColor, color);
						_renderers[i].SetPropertyBlock(_propertyBlock);
					}
				},
				1f,
				_flashDuration);
		}
	}
}