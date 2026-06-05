using DG.Tweening;
using MagicalTower.UI.Core;
using TMPro;
using UnityEngine;

namespace MagicalTower.UI
{
	public class DamageNumber : MonoBehaviour, IProjectedUI
	{
		[SerializeField] private TextMeshProUGUI _text;

		public RectTransform RectTransform { get; private set; }
		public Vector3 WorldOffset => Vector3.up * 2f;

		private void Awake()
		{
			RectTransform = GetComponent<RectTransform>();
		}

		public void Show(float amount, Vector3 worldPosition)
		{
			_text.text = Mathf.RoundToInt(amount).ToString();

			RectTransform
				.DOAnchorPosY(RectTransform.anchoredPosition.y + 50f, 0.8f)
				.SetEase(Ease.OutCubic);

			_text.DOFade(0f, 0.8f)
				.SetEase(Ease.InCubic)
				.OnComplete(OnReturn);
		}

		public void OnSpawn()
		{
			gameObject.SetActive(true);
			var color = _text.color;
			color.a = 1f;
			_text.color = color;
		}

		public void OnReturn()
		{
			gameObject.SetActive(false);
		}
	}
}