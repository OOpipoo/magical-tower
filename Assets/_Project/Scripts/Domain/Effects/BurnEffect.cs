namespace MagicalTower.Domain.Effects
{
	public class BurnEffect : StatusEffect
	{
		private readonly float _damagePerSecond;
		private float _timeRemaining;
		private float _tickAccumulator;
		private const float TickInterval = 0.5f;

		public override bool IsExpired => _timeRemaining <= 0f;

		public BurnEffect(float damagePerSecond, float duration)
		{
			_damagePerSecond = damagePerSecond;
			_timeRemaining = duration;
		}

		public override void Tick(Enemy.Enemy enemy, float deltaTime)
		{
			_timeRemaining -= deltaTime;
			_tickAccumulator += deltaTime;

			if (_tickAccumulator >= TickInterval)
			{
				_tickAccumulator = 0f;
				enemy.TakeDamage(_damagePerSecond * TickInterval);
			}
		}
	}
}