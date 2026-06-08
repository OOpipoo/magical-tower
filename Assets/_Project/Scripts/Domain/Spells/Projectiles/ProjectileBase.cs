using UnityEngine;

namespace MagicalTower.Domain.Spells.Projectiles
{
	public abstract class ProjectileBase : MonoBehaviour
	{
		[SerializeField] private float _maxLifetime = 10f;
		[SerializeField] private float _destroyBelowY = -5f;

		private float _lifetime;

		protected virtual void Update()
		{
			_lifetime += Time.deltaTime;

			if (_lifetime >= _maxLifetime || transform.position.y < _destroyBelowY)
			{
				OnLifetimeExpired();
				return;
			}

			Tick(Time.deltaTime);
		}

		protected abstract void Tick(float deltaTime);
		protected abstract void OnLifetimeExpired();

		protected virtual void OnDisable()
		{
			_lifetime = 0f;
		}
	}
}