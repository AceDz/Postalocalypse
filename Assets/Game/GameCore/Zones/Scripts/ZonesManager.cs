using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MEC;
using Sirenix.OdinInspector;
using TeamTheDream;
using TeamTheDream.Delivery;
using UnityEngine;
using UnityEngine.Serialization;

namespace _TeamTheDream._Delivery
{
    public class ZonesManager : SerializedMonoBehaviour
    {
        [SerializeField, BoxGroup("Player")]private PlayerMovementController _playerMovementController;
        [SerializeField, BoxGroup("Initial Zone")]private GameObject _initialZone;
        [SerializeField, BoxGroup("Fading Panel")]private CanvasGroup _fadingCanvasGroup;
        [SerializeField, BoxGroup("Fading Panel")]private float _fadeInDuration = 2f;
        [SerializeField, BoxGroup("Fading Panel")]private float _fadeOutDuration = 2f;
        [SerializeField, BoxGroup("UniqueZoneItems")]private UniqueItemsManager _uniqueItemsManager;
        [SerializeField, BoxGroup("Audio")] private AudioManager _audio;
        [SerializeField, BoxGroup("Test")]private string _testZone;


        public static Action<string, int> OnLoadZone;
        public static Action OnEndGame;
        
        private GameObject _activeZone;
        private int _lastSpawnIndex;
        private Zone _lastZone;

        private bool _isFaded;
        private bool _isInitialZone;

        private void Awake()
        {
            _activeZone = _initialZone;
            _lastZone = _initialZone.GetComponent<Zone>();
            _isInitialZone = true;
            //_playerMovementController = _playerTransform.GetComponent<PlayerMovementController>();
        }

        private void OnEnable()
        {
            OnLoadZone += HandleLoadZone;
            OnEndGame += HandleEndGame;
            HealthManager.OnPlayerDeath += HealthManager_OnPlayerDeath;
        }

        private void OnDisable()
        {
            OnLoadZone -= HandleLoadZone;
            OnEndGame -= HandleEndGame;
            HealthManager.OnPlayerDeath -= HealthManager_OnPlayerDeath;
        }

        private void HandleLoadZone(string zoneReference, int spawnIndex)
        {
            LoadZone(zoneReference, spawnIndex);
        }

        private void HandleEndGame()
        {
            _playerMovementController.Stop();
            UnloadActiveZone();
        }

        private void LoadZone(string zoneReference, int spawnIndex, bool revive = false)
        {
            _isFaded = true;

            DOTween.Sequence()
                .AppendCallback(FadePanel)
                .AppendInterval(_fadeInDuration)
                .AppendCallback(() =>
                {
                    UnloadOldAndLoadNew(zoneReference, spawnIndex, revive);
                });
        }

        private async void UnloadOldAndLoadNew(string zoneReference, int spawnIndex, bool revive)
        {
            _playerMovementController.gameObject.SetActive(false);
            UnloadActiveZone();
            Debug.Log($"Loading {zoneReference}");
            var levelResource = await Resources.LoadAsync($"Levels/{zoneReference}") as GameObject;
            _activeZone = Instantiate(levelResource);
            _activeZone.name = zoneReference;
            _uniqueItemsManager.DeleteAlreadyPickedUniqueItems();

            _lastZone = _activeZone.GetComponent<Zone>();
            _lastSpawnIndex = spawnIndex;
            
            if (_lastZone.SpawnPoints == null || _lastZone.SpawnPoints.Count == 0)
            {
                Debug.LogError($"Zone {_lastZone.name} has no Spawn Points");
            }
            
            _playerMovementController.transform.position = _lastZone.SpawnPoints[_lastSpawnIndex].position;
            await UniTask.DelayFrame(1);
            _lastZone.SetPlayer(_playerMovementController);
            await UniTask.DelayFrame(1);

            FadePanel();
            
            _playerMovementController.gameObject.SetActive(true);

            if (revive)
            {
                _playerMovementController.Revive();
                return;
            }
            _playerMovementController.Resume();
            
        }

        private void UnloadActiveZone()
        {
            _playerMovementController.Stop();
            if (_activeZone == null)
                _activeZone = _initialZone;
            
            if (_activeZone != null)
            {
                // Addressables.ReleaseInstance(_activeZone);
                Destroy(_activeZone);
                Resources.UnloadUnusedAssets();
            }

            if (_isInitialZone)
            {
                _isInitialZone = false;
                Destroy(_activeZone);
            }
        }

        private void FadePanel()
        {
            _isFaded = !_isFaded;

            if (_isFaded)
            {
                _fadingCanvasGroup.DOFade(0, _fadeInDuration);
            }
            else
            {
                _fadingCanvasGroup.DOFade(1, _fadeOutDuration);
            }
        }

        private void HealthManager_OnPlayerDeath()
        {
            Timing.RunCoroutine(WaitAndRevivePlayer());
        }

        private IEnumerator<float> WaitAndRevivePlayer()
        {
            yield return Timing.WaitForSeconds(1f);
            RevivePlayerInLastSpawnPoint();
        }
        
        public void RevivePlayerInLastSpawnPoint()
        {
            LoadZone(_lastZone.name, _lastSpawnIndex, true);
            _audio.PlayGameMusic();
        }

        [Button]
        public void TestLoadZone()
        {
            LoadZone(_testZone, 0);
        }

        
    }
}