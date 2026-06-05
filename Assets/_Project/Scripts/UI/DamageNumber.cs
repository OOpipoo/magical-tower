using DG.Tweening;
using TMPro;
using UnityEngine;
using System;

namespace MagicalTower.UI
{
	public class DamageNumber : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _text;

		public RectTransform RectTransform { get; private set; }
		private Sequence _sequence;

		private void Awake()
		{
			RectTransform = GetComponent<RectTransform>();
		}

		public void Show(float amount, System.Action onComplete)
		{
			if (amount < 1f)
			{
				onComplete?.Invoke();
				return;
			}

			_sequence?.Kill();

			var color = _text.color;
			color.a = 1f;
			_text.color = color;
			_text.text = Mathf.RoundToInt(amount).ToString();

			var startPos = RectTransform.position;
			var endPos = startPos + Vector3.up * 50f;

			_sequence = DOTween.Sequence();
			_sequence.Append(RectTransform.DOMove(endPos, 0.8f).SetEase(Ease.OutCubic));
			_sequence.Join(_text.DOFade(0f, 0.8f).SetEase(Ease.InCubic));
			_sequence.OnComplete(() => onComplete?.Invoke());
		}
	}
}