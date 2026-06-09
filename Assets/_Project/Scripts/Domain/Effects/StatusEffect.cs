namespace MagicalTower.Domain.Effects
{
	public abstract class StatusEffect
	{
		public abstract string EffectId { get; }
		public abstract float Duration { get; }
		public abstract bool IsExpired { get; }
		public abstract void Tick(Enemy.Enemy enemy, float deltaTime);
		public virtual float GetSpeedMultiplier() => 1f;
	}
}