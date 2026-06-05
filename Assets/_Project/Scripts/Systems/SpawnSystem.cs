using System.Collections.Generic;
using MagicalTower.Data;
using MagicalTower.Domain.Enemy;
using UnityEngine;
using VContainer.Unity;

namespace MagicalTower.Systems
{
    public class SpawnSystem : ITickable
    {
        private readonly EnemyFactory _enemyFactory;
        private readonly WaveConfig _waveConfig;
        private readonly Camera _camera;
        private readonly List<Enemy> _activeEnemies;

        private float _gameTime;
        private float _spawnTimer;
        private WavePeriod _currentPeriod;

        public IReadOnlyList<Enemy> ActiveEnemies => _activeEnemies;

        public SpawnSystem(
            EnemyFactory enemyFactory,
            WaveConfig waveConfig,
            Camera camera,
            List<Enemy> activeEnemies)
        {
            _enemyFactory = enemyFactory;
            _waveConfig = waveConfig;
            _camera = camera;
            _activeEnemies = activeEnemies;
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
            if (_currentPeriod.EnemyPool.Count == 0) return;

            var config = _currentPeriod.EnemyPool[Random.Range(0, _currentPeriod.EnemyPool.Count)];
            var spawnPosition = GetSpawnPositionOutsideScreen();
            var enemy = _enemyFactory.Create(config, spawnPosition);
            _activeEnemies.Add(enemy);
        }

        private Vector3 GetSpawnPositionOutsideScreen()
        {
            var side = Random.Range(0, 4);
            Vector3 screenPos = side switch
            {
                0 => new Vector3(Random.Range(0f, 1f), 1.1f, 0f),
                1 => new Vector3(Random.Range(0f, 1f), -0.1f, 0f),
                2 => new Vector3(-0.1f, Random.Range(0f, 1f), 0f),
                _ => new Vector3(1.1f, Random.Range(0f, 1f), 0f)
            };

            var ray = _camera.ViewportPointToRay(screenPos);
            if (Physics.Raycast(ray, out var hit, 100f))
                return hit.point;

            return ray.GetPoint(20f);
        }

        public void RemoveEnemy(Enemy enemy)
        {
            _activeEnemies.Remove(enemy);
            _enemyFactory.Return(enemy);
        }
    }
}