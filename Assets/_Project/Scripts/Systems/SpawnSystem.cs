using System.Collections.Generic;
using MagicalTower.Core;
using MagicalTower.Data;
using MagicalTower.Domain.Enemy;
using UnityEngine;
using VContainer.Unity;

namespace MagicalTower.Systems
{
    public class SpawnSystem : ITickable, IInitializable
    {
        private readonly EnemyFactory _enemyFactory;
        private readonly WaveConfig _waveConfig;
        private readonly Camera _camera;
        private readonly List<Enemy> _activeEnemies;
        private readonly EventBus _eventBus;
        private readonly Collider[] _spawnCheckBuffer = new Collider[10];
        private int _enemyLayerMask;

        private float _gameTime;
        private float _spawnTimer;
        private WavePeriod _currentPeriod;

        public IReadOnlyList<Enemy> ActiveEnemies => _activeEnemies;

        public SpawnSystem(
            EnemyFactory enemyFactory,
            WaveConfig waveConfig,
            Camera camera,
            List<Enemy> activeEnemies,
            EventBus eventBus)
        {
            _enemyFactory = enemyFactory;
            _waveConfig = waveConfig;
            _camera = camera;
            _activeEnemies = activeEnemies;
            _eventBus = eventBus;
            _currentPeriod = _waveConfig.Periods[0];
            _enemyLayerMask = LayerMask.GetMask("Enemy");
        }

        public void Initialize()
        {
            _eventBus.Subscribe<GameEvents.EnemyDiedEvent>(OnEnemyDied);
        }

        public void Tick()
        {
            _gameTime += Time.deltaTime;
            UpdateCurrentPeriod();

            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= _currentPeriod.SpawnInterval)
            {
                _spawnTimer = 0f;
                SpawnEnemy();
            }
        }

        private void UpdateCurrentPeriod()
        {
            WavePeriod latest = _waveConfig.Periods[0];
            foreach (var period in _waveConfig.Periods)
            {
                if (_gameTime >= period.StartTime)
                    latest = period;
            }
            _currentPeriod = latest;
        }

        private void SpawnEnemy()
        {
            if (_currentPeriod.EnemyPool.Count == 0)
            {
                return;
            }

            var config = _currentPeriod.EnemyPool[Random.Range(0, _currentPeriod.EnemyPool.Count)];
            var spawnPosition = GetValidSpawnPosition(config);

            if (spawnPosition == null)
            {
                return;
            }

            var enemy = _enemyFactory.Create(config, spawnPosition.Value);
            _activeEnemies.Add(enemy);
        }

        private Vector3? GetValidSpawnPosition(EnemyConfig config, int maxAttempts = 10)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                var position = GetSpawnPositionOutsideScreen();
                var hits = Physics.OverlapSphereNonAlloc(
                    position, 
                    config.SpawnCheckRadius, 
                    _spawnCheckBuffer,
                    _enemyLayerMask);

                if (hits == 0)
                    return position;
            }

            return null;
        }

        private Vector3 GetSpawnPositionOutsideScreen()
        {
            var side = Random.Range(0, 4);
            var screenPos = side switch
            {
                0 => new Vector3(Random.Range(0f, 1f), 1.1f, 0f),
                1 => new Vector3(Random.Range(0f, 1f), -0.1f, 0f),
                2 => new Vector3(-0.1f, Random.Range(0f, 1f), 0f),
                _ => new Vector3(1.1f, Random.Range(0f, 1f), 0f)
            };

            var ray = _camera.ViewportPointToRay(screenPos);
            if (Physics.Raycast(ray, out var hit, 100f))
                return new Vector3(hit.point.x, 0f, hit.point.z);

            var point = ray.GetPoint(20f);
            return new Vector3(point.x, 0f, point.z);
        }

        private void OnEnemyDied(GameEvents.EnemyDiedEvent evt)
        {
            var enemy = evt.Enemy.GetComponent<Enemy>();
            if (enemy == null) return;

            _activeEnemies.Remove(enemy);
            _enemyFactory.Return(enemy);
        }
    }
}