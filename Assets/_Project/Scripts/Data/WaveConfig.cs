using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagicalTower.Data
{
	[Serializable]
	public class WavePeriod
	{
		[Tooltip("Game time in seconds when this period starts")]
		public float StartTime = 0f;

		[Tooltip("Interval between enemy spawns in seconds")]
		public float SpawnInterval = 3f;

		[Tooltip("Enemy pool for this period")]
		public List<EnemyConfig> EnemyPool = new();
	}

	[CreateAssetMenu(fileName = "WaveConfig", menuName = "MagicalTower/Configs/Wave")]
	public class WaveConfig : ScriptableObject
	{
		public List<WavePeriod> Periods = new();
	}
}