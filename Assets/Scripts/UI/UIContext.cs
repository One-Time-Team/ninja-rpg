using System;
using System.Collections.Generic;
using InputReader;
using ItemsSystem;
using ItemsSystem.Data;
using UI.Core;
using UI.Enum;
using UI.InventoryUI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI
{
    public class UIContext : IDisposable
    {
        private const string LoadPath = "UI/";

        private readonly Dictionary<ScreenType, IScreenController> _presenters;
        private readonly Transform _uiContainer;
        private readonly List<IWindowsInputSource> _inputSources;
        private readonly Data _data;

        private IScreenController _currentController;

        public UIContext(List<IWindowsInputSource> inputSources, Data data)
        {
            _presenters = new Dictionary<ScreenType, IScreenController>();
            _inputSources = inputSources;
            foreach (IWindowsInputSource inputSource in inputSources)
            {
                inputSource.InventoryRequested += OpenInventory;
            }

            GameObject container = new GameObject()
            {
                name = nameof(UIContext)
            };
            _uiContainer = container.transform;
            _data = data;
        }

        private void OpenInventory() => OpenScreen(ScreenType.Inventory);

        private void OpenScreen(ScreenType screenType)
        {
            _currentController?.Complete();

            if (!_presenters.TryGetValue(screenType, out IScreenController screenController))
            {
                screenController = GetPresenter(screenType);
                screenController.CloseRequested += CloseCurrentScreen;
                screenController.OpenScreenRequested += OpenScreen;
                _presenters.Add(screenType, screenController);
            }

            _currentController = screenController;
            _currentController.Initialize();
        }


        private void CloseCurrentScreen()
        {
            _currentController.Complete();
            _currentController = null;
        }

        private IScreenController GetPresenter(ScreenType screenType)
        {
            switch (screenType)
            {
                case ScreenType.Inventory:
                    return new InventoryScreenPresenter(GetView<InventoryScreenView>(screenType), _data.Inventory, _data.RarityDescriptors);
                default:
                    throw new NullReferenceException();
            }
        }

        private TView GetView<TView>(ScreenType screenType) where TView : ScreenView
        {
            TView prefab = Resources.Load<TView>($"{LoadPath}{screenType.ToString()}");
            return Object.Instantiate(prefab, _uiContainer);
        }

        public void Dispose()
        {
        }

        public struct Data
        {
            public Inventory Inventory { get; }
            public List<RarityDescriptor> RarityDescriptors { get; }

            public Data(Inventory inventory, List<RarityDescriptor> rarityDescriptors)
            {
                Inventory = inventory;
                RarityDescriptors = rarityDescriptors;
            }
        }
    }
}