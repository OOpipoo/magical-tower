using MagicalTower.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace MagicalTower.UI
{
	public class EnemyHealthBar : MonoBehaviour, IProjectedUI
	{
		[SerializeField] private Image _fillImage;

		public RectTransform RectTransform { get; private set; }
		public Vector3 WorldOffset => Vector3.up * 1.5f;

		private void Awake()
		{
			RectTransform = GetComponent<RectTransform>();
		}

		public void SetHealth(float percent)
		{
			_fillImage.fillAmount = Mathf.Clamp01(percent);
		}

		public void OnSpawn()
		{
			gameObject.SetActive(true);
			SetHealth(1f);
		}

		public void OnReturn()
		{
			gameObject.SetActive(false);
		}
	}
}