using UnityEngine;

namespace MagicalTower.UI.Core
{
	public interface IProjectedUI
	{
		RectTransform RectTransform { get; }
		Vector3 WorldOffset { get; }
		void OnSpawn();
		void OnReturn();
	}
}