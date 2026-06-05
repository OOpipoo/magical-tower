using MagicalTower.Core;
using MagicalTower.Domain.Tower;
using MagicalTower.UI.Core;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MagicalTower.UI
{
    public class DamageNumberPresenter : MonoBehaviour, IInitializable
    {
        private EventBus _eventBus;
        private ObjectPool<DamageNumber> _pool;
        private Camera _camera;
        private Tower _tower;

        [Inject]
        public void Construct(
            EventBus eventBus,
            ObjectPool<DamageNumber> pool,
            Camera camera,
            Tower tower)
        {
            _eventBus = eventBus;
            _pool = pool;
            _camera = camera;
            _tower = tower;
        }

        public void Initialize()
        {
            _eventBus.Subscribe<GameEvents.DamageDealtEvent>(OnDamageDealt);
            _eventBus.Subscribe<GameEvents.TowerHealthChangedEvent>(OnTowerHealthChanged);
        }

        private void OnDestroy()
        {
            _eventBus?.Unsubscribe<GameEvents.DamageDealtEvent>(OnDamageDealt);
            _eventBus?.Unsubscribe<GameEvents.TowerHealthChangedEvent>(OnTowerHealthChanged);
        }

        private void OnDamageDealt(GameEvents.DamageDealtEvent evt)
        {
            SpawnDamageNumber(evt.Amount, evt.Position);
        }

        private void OnTowerHealthChanged(GameEvents.TowerHealthChangedEvent evt)
        {
            SpawnDamageNumber(evt.Damage, _tower.Transform.position);
        }

        private void SpawnDamageNumber(float amount, Vector3 worldPosition)
        {
            var screenPos = _camera.WorldToScreenPoint(worldPosition);
            if (screenPos.z < 0) return;

            var damageNumber = _pool.Get();
            damageNumber.transform.SetParent(transform, false);
            damageNumber.RectTransform.position = screenPos;
            damageNumber.Show(amount, () => _pool.Return(damageNumber));
        }
    }
}