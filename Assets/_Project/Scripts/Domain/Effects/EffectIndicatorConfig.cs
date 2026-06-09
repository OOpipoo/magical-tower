using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagicalTower.Domain.Effects
{
	[CreateAssetMenu(fileName = "EffectIndicatorConfig", menuName = "MagicalTower/Configs/EffectIndicatorConfig")]
	public class EffectIndicatorConfig : ScriptableObject
	{
		public List<EffectVisualEntry> Entries = new();

		public bool TryGet(string effectId, out EffectVisualEntry entry)
		{
			foreach (var e in Entries)
			{
				if (e.EffectId == effectId)
				{
					entry = e;
					return true;
				}
			}
			entry = default;
			return false;
		}
	}

	[Serializable]
	public struct EffectVisualEntry
	{
		public string EffectId;
		public Color Color;
		public PrimitiveType Shape;
		public float Scale;
		public float YOffset;
	}
}
