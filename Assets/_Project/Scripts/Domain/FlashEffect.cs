using DG.Tweening;
using UnityEngine;

namespace MagicalTower.Domain
{
	public class FlashEffect : MonoBehaviour
	{
		[SerializeField] private Renderer[] _renderers;
		[SerializeField] private float _flashDuration = 0.1f;

		private MaterialPropertyBlock _propertyBlock;
		private Color[] _originalColors;
		private static readonly int _baseColor = Shader.PropertyToID("_BaseColor");


		private void Awake()
		{
			_propertyBlock = new MaterialPropertyBlock();
			_originalColors = new Color[_renderers.Length];
		}

		public void Flash()
		{
			for (var i = 0; i < _renderers.Length; i++)
				_originalColors[i] = _renderers[i].material.GetColor(_baseColor);

			DOTween.To(
				() => 0f,
				t =>
				{
					for (var i = 0; i < _renderers.Length; i++)
					{
						var color = Color.Lerp(Color.white, _originalColors[i], t);
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