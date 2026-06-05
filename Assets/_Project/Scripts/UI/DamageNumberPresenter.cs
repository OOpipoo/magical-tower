using MagicalTower.Core;
using MagicalTower.UI.Core;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MagicalTower.UI
{
	public class DamageNumberPresenter : MonoBehaviour, IInitializable
	{
		private EventBus _eventBus;
		private UIWorldProjectionContainer<DamageNumber> _container;

		[Inject]
		public void Construct(
			EventBus eventBus,
			UIWorldProjectionContainer<DamageNumber> container)
		{
			_eventBus = eventBus;
			_container = container;
		}

		public void Initialize()
		{
			_eventBus.Subscribe<GameEvents.DamageDealtEvent>(OnDamageDealt);
		}

		private void OnDestroy()
		{
			_eventBus.Unsubscribe<GameEvents.DamageDealtEvent>(OnDamageDealt);
		}

		private void OnDamageDealt(GameEvents.DamageDealtEvent evt)
		{
			var tempGo = new GameObject("TempTarget");
			tempGo.transform.position = evt.Position;

			var damageNumber = _container.Attach(tempGo.transform);
			damageNumber.Show(evt.Amount, evt.Position);

			Object.Destroy(tempGo, 1f);
		}
	}
}