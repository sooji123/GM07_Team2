using System.Collections;
using UnityEngine;

namespace GM07.Map
{
    public sealed class CustomerSpawner : MonoBehaviour
    {
        [SerializeField] 
        private TableManager _tableManager;
        [SerializeField]
        private GameFlowManager _gameFlowManager;
        [SerializeField] 
        private GameObject _customerPrefab;
        [SerializeField]
        private Transform _spawnPoint;
        [SerializeField]
        private CustomerSpawnSettingData _spawnSettings;

        private Coroutine _spawnCoroutine;

        private void OnEnable()
        {
            if (_gameFlowManager != null)
            {
                _gameFlowManager.OnGameStateChanged += OnGameStateChanged;

                OnGameStateChanged(_gameFlowManager.GameState);
            }
        }

        private void OnDisable()
        {
            if (_gameFlowManager != null)
            {
                _gameFlowManager.OnGameStateChanged -= OnGameStateChanged;
            }

            StopSpawn();
        }

        private void OnGameStateChanged(EGameState gameState)
        {
            if (gameState == EGameState.Open)
            {
                StartSpawn();
                return;
            }

            StopSpawn();
        }

        private void StartSpawn()
        {
            if (_spawnCoroutine != null)
            {
                return;
            }

            _spawnCoroutine = StartCoroutine(StartSpawnCo());
        }

        private void StopSpawn()
        {
            if (_spawnCoroutine == null)
            {
                return;
            }

            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }

        private IEnumerator StartSpawnCo()
        {
            if (_tableManager == null)
            {
                _tableManager = FindFirstObjectByType<TableManager>();
            }

            while(_gameFlowManager.GameState == EGameState.Open)
            {
                if (!TryGetSpawnInterval(out float spawnInterval))
                {
                    _spawnCoroutine = null;
                    yield break;
                }

                yield return new WaitForSeconds(spawnInterval);

                if (_gameFlowManager.GameState != EGameState.Open)
                {
                    break;
                }

                TrySpawn();
            }
            _spawnCoroutine = null;
        }

        public bool TrySpawn()
        {
            if (_tableManager == null || !_tableManager.TryUseSeat(out Table table, out Seat seat) || _customerPrefab == null)
            {
                return false;
            }
            GameObject customer = Instantiate(_customerPrefab, _spawnPoint.position, _spawnPoint.rotation);
            if(customer.TryGetComponent<Customer>(out Customer customerComponent))
            {
                customerComponent.Init(_tableManager, table, seat);
                return true;
            }
            else
            {
                Destroy(customer);
                _tableManager.ReleaseSeat(table, seat);
                return false;
            }
        }

        private bool TryGetSpawnInterval(out float spawnInterval)
        {
            spawnInterval = 0f;
            if (_spawnSettings == null)
            {
                return false;
            }
            float openProgress = GetOpenProgress();
            if (!_spawnSettings.TryGetSpawnPeriod(openProgress, out CustomerSpawnPeriod spawnPeriod))
            {
                return false;
            }

            float baseInterval = spawnPeriod.GetRandomInterval();
            float storeLevelSpawnRate = _spawnSettings.GetStoreLevelSpawnRate(0); //매장의 레벨에 따라 속도 조절 / 현재는 1로 고정
            spawnInterval = baseInterval / storeLevelSpawnRate;
            return true;
        }

        private float GetOpenProgress()
        {
            if (_gameFlowManager.OpenDuration <= 0f)
            {
                return 0f;
            }

            float elapsedTime = _gameFlowManager.OpenDuration - _gameFlowManager.RemainingTime;

            return Mathf.Clamp01( elapsedTime / _gameFlowManager.OpenDuration);
        }
    }
}
