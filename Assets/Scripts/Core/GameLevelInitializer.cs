using System;
using System.Collections.Generic;
using Assets.Scripts.ItemsSystem;
using Core.Services.Updater;
using Core.Tools;
using InputReader;
using Player;
using UnityEngine;
using ItemsSystem;
using ItemsSystem.Storages;
using ItemsSystem.Data;
using System.Linq;
using Core.Parallax;

namespace Core
{
    public class GameLevelInitializer : MonoBehaviour
    {
        [SerializeField] private WorldBoundaries _levelBorders;
        [SerializeField] private PlayerEntityHandler _player;
        [SerializeField] private GameUIInputView _gameUIInputView;
        [SerializeField] private ItemRarityStorage _itemRarityStorage;
        [SerializeField] private ItemsStorage _itemsStorage;
        [SerializeField] private ParallaxEffect _parallaxEffect;

        private ExternalDeviceInputReader _externalDeviceInputReader;
        private PlayerSystem _playerSystem;
        private ProjectUpdater _projectUpdater;
        private DropGenerator _dropGenerator;
        private ItemSystem _itemSystem;

        private List<IDisposable> _disposables;


        

        private void Awake()
        {
            _disposables = new List<IDisposable>();

            if (ProjectUpdater.Instance == null)
            {
                _projectUpdater = new GameObject().AddComponent<ProjectUpdater>();
                _projectUpdater.gameObject.name = "Game Updater";
            }
            else
                _projectUpdater = ProjectUpdater.Instance as ProjectUpdater;
            
            _levelBorders.OnAwake();
            
            _externalDeviceInputReader = new ExternalDeviceInputReader();
            _disposables.Add(_externalDeviceInputReader);
            
            _playerSystem = new PlayerSystem(_player, new List<IEntityInputSource>
            {
                _gameUIInputView,
                _externalDeviceInputReader,
            });
            _disposables.Add(_playerSystem);

            ItemsFactory itemsFactory = new ItemsFactory(_playerSystem._statsController);
            List<IRarityColor> rarityColors = _itemRarityStorage.RarityDescriptors.Cast<IRarityColor>().ToList();
            _itemSystem = new ItemSystem(rarityColors, itemsFactory);
            List<ItemDescriptor> itemDescriptors = _itemsStorage.ItemScriptables.Select(scriptable => scriptable.ItemDescriptor).ToList();
            _dropGenerator = new DropGenerator(_player, itemDescriptors, _itemSystem);
            _parallaxEffect.Layers.Add(new ParallaxLayer(_itemSystem.Transform, 1));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                _projectUpdater.IsPaused = !_projectUpdater.IsPaused;
        }

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}