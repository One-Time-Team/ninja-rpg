using System;
using System.Collections.Generic;
using System.Linq;
using Core.Parallax;
using Core.Services.Updater;
using Core.Tools;
using InputReader;
using ItemsSystem;
using ItemsSystem.Data;
using ItemsSystem.Storages;
using NPC.Enums;
using NPC.Spawn;
using Player;
using UnityEngine;

namespace Core.Scene
{
    public class GameLevelInitializer : MonoBehaviour
    {
        [Header("Level Managing")]
        [SerializeField] private GameObject _completeLevelUI;
        [SerializeField] private GameObject _restartLevelUI;
        [SerializeField] private float _loadNextLevelDelay;
        [SerializeField] private float _restartLevelDelay;
        [Header("Level")]
        [SerializeField] private WorldBoundaries _levelBorders;
        [SerializeField] private ParallaxEffect _parallaxEffect;
        [SerializeField] private GameUIInputView _gameUIInputView;
        [SerializeField] private List<Transform> _entitySpawnPoints;
        [Header("Player")]
        [SerializeField] private PlayerEntityBehaviour _player;
        [SerializeField] private LayerMask _playerLayer;
        [Header("Items")]
        [SerializeField] private ItemRarityStorage _itemRarityStorage;
        [SerializeField] private ItemsStorage _itemsStorage;
        [Header("Pathfinding")]
        [SerializeField] private AstarPath _astarPath;
        [SerializeField] private float _scanUpdateTime;

        private ExternalDeviceInputReader _externalDeviceInputReader;
        private PlayerSystem _playerSystem;
        private ProjectUpdater _projectUpdater;
        private DropGenerator _dropGenerator;
        private ItemSystem _itemSystem;
        private EntitiesSystem _entitiesSystem;

        private List<IDisposable> _disposables;
        
        
        private void Awake()
        {
            _disposables = new List<IDisposable>();

            if (ProjectUpdater.Instance == null)
            {
                _projectUpdater = new GameObject().AddComponent<ProjectUpdater>();
                _projectUpdater.gameObject.name = nameof(ProjectUpdater);
            }
            else
                _projectUpdater = ProjectUpdater.Instance as ProjectUpdater;

            _projectUpdater.Initialize(_gameUIInputView.Joystick, _gameUIInputView.Buttons);
            
            InvokeRepeating(nameof(ScanAstar), 0f, _scanUpdateTime);

            _levelBorders.OnAwake();
            
            _externalDeviceInputReader = new ExternalDeviceInputReader();
            _disposables.Add(_externalDeviceInputReader);
            
            _playerSystem = new PlayerSystem(_player, _parallaxEffect, new List<IEntityInputSource>
            {
                _gameUIInputView,
                _externalDeviceInputReader,
            });
            _disposables.Add(_playerSystem);

            ItemsFactory itemsFactory = new ItemsFactory(_playerSystem.StatsController);
            List<IRarityColor> rarityColors = _itemRarityStorage.RarityDescriptors.Cast<IRarityColor>().ToList();
            _itemSystem = new ItemSystem(rarityColors, itemsFactory, _gameUIInputView.InteractButton, _playerLayer);
            _parallaxEffect.Layers.Add(new ParallaxLayer(_itemSystem.Transform, 1));
            _disposables.Add(_itemSystem);
            List<ItemDescriptor> itemDescriptors = _itemsStorage.ItemScriptables.Select(scriptable => scriptable.ItemDescriptor).ToList();
            _dropGenerator = new DropGenerator(itemDescriptors, _itemSystem);

            _entitiesSystem = new EntitiesSystem(_parallaxEffect);
            _parallaxEffect.Layers.Add(new ParallaxLayer(_entitiesSystem.Transform, 1));
            _disposables.Add(_entitiesSystem);
        }

        private void Start()
        {
            foreach (var spawnPoint in _entitySpawnPoints)
            {
                _entitiesSystem.SpawnEntity(EntityId.Ronin, spawnPoint.position, _dropGenerator);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                _projectUpdater.IsPaused = !_projectUpdater.IsPaused;

            if (_entitiesSystem.AreEnemiesDead)
                Invoke(nameof(LoadNextLevel), _loadNextLevelDelay);
            else if (_playerSystem.IsPlayerDead)
                Invoke(nameof(RestartLevel), _restartLevelDelay);
        }

        private void OnDestroy()
        {
            CancelInvoke(nameof(ScanAstar));
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }

        private void ScanAstar()
        {
           _astarPath.Scan();
        }

        private void LoadNextLevel()
        {
            _completeLevelUI.SetActive(true);
        }

        private void RestartLevel()
        {
            _restartLevelUI.SetActive(true);
        }
    }
}