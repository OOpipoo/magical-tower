using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagicalTower.Data
{
	[Serializable]
	public class EnemySpawnEntry
	{
		public EnemyConfig Enemy;
		public int Count = 5;
	}

	[Serializable]
	public class WavePeriod
	{
		[Tooltip("Game time in seconds when this period starts")]
		public float StartTime = 0f;

		[Tooltip("Interval between enemy spawns in seconds")]
		public float SpawnInterval = 3f;

		[Tooltip("Enemies to spawn in this period")]
		public List<EnemySpawnEntry> EnemyPool = new();
	}

	[CreateAssetMenu(fileName = "WaveConfig", menuName = "MagicalTower/Configs/Wave")]
	public class WaveConfig : ScriptableObject
	{
		public List<WavePeriod> Periods = new();
	}
}