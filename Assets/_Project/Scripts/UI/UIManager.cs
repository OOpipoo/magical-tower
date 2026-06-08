using MagicalTower.Core;
using MagicalTower.Data;
using MagicalTower.Domain.Tower;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MagicalTower.UI
{
	public class UIManager : MonoBehaviour, IInitializable
	{
		[SerializeField] private Transform _uiRoot;

		[Header("Prefabs")]
		[SerializeField] private GameObject _healthBarCanvasPrefab;
		[SerializeField] private GameObject _damageNumbersCanvasPrefab;
		[SerializeField] private GameObject _gameWonCanvasPrefab;
		[SerializeField] private GameObject _gameOverCanvasPrefab;

		private EventBus _eventBus;
		private ObjectPool<DamageNumber> _damageNumberPool;
		private Camera _camera;
		private Tower _tower;
		private WaveConfig _waveConfig;

		
		[Inject]
		public void Construct(EventBus eventBus, ObjectPool<DamageNumber> damageNumberPool, Camera cameraMain, Tower tower, WaveConfig waveConfig)
		{
			_eventBus = eventBus;
			_damageNumberPool = damageNumberPool;
			_camera = cameraMain;
			_tower = tower;
			_waveConfig = waveConfig;
		}

		public void Initialize()
		{
			var healthBarGo = Instantiate(_healthBarCanvasPrefab, _uiRoot);
			var healthBarPresenter = healthBarGo.GetComponent<TowerHealthBarPresenter>();
			healthBarPresenter.Construct(_eventBus, _tower, _waveConfig);
			healthBarPresenter.Initialize();

			var damageNumbersGo = Instantiate(_damageNumbersCanvasPrefab, _uiRoot);
			var damageNumberPresenter = damageNumbersGo.GetComponent<DamageNumberPresenter>();
			damageNumberPresenter.Construct(_eventBus, _damageNumberPool, _camera, _tower);
			damageNumberPresenter.Initialize();

			var gameWonGo = Instantiate(_gameWonCanvasPrefab, _uiRoot);
			gameWonGo.SetActive(false);
			var gameWonPresenter = gameWonGo.GetComponent<GameWonPresenter>();
			gameWonPresenter.Construct(_eventBus);
			
			var gameOverGo = Instantiate(_gameOverCanvasPrefab, _uiRoot);
			gameOverGo.SetActive(false);
			var gameOverPresenter = gameOverGo.GetComponentInChildren<GameOverPresenter>();
			gameOverPresenter.Construct(_eventBus);

		}
	}
}