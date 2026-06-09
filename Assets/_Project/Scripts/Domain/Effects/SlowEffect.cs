namespace MagicalTower.Domain.Effects
{
	public class SlowEffect : StatusEffect
	{
		private readonly float _speedMultiplier;
		private float _timeRemaining;

		public override bool IsExpired => _timeRemaining <= 0f;

		public SlowEffect(float speedMultiplier, float duration)
		{
			_speedMultiplier = speedMultiplier;
			_timeRemaining = duration;
		}

		public override void Tick(Enemy.Enemy enemy, float deltaTime)
		{
			_timeRemaining -= deltaTime;
		}

		public override float GetSpeedMultiplier() => _speedMultiplier;
	}
}
