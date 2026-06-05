namespace MagicalTower.Domain.Effects
{
	public abstract class StatusEffect
	{
		public abstract bool IsExpired { get; }
		public abstract void Tick(Enemy.Enemy enemy, float deltaTime);
	}
}