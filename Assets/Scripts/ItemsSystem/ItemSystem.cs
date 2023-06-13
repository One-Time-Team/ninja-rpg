﻿using System;
using System.Collections.Generic;
using Core.Services.Updater;
using ItemsSystem.Behaviour;
using ItemsSystem.Core;
using ItemsSystem.Data;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ItemsSystem
{
    public class ItemSystem : IDisposable
    {
        private SceneItem _sceneItem;
        private ItemsFactory _itemsFactory;
        private List<IRarityColor> _colors;
        private Dictionary<SceneItem, Item> _itemsOnScene;
        private Button _interactButton;
        private LayerMask _playerLayer;

        public Transform Transform { get; }

        public ItemSystem(List<IRarityColor> colors, ItemsFactory itemsFactory, Button interactButton, LayerMask playerLayer)
        {
            _sceneItem = Resources.Load<SceneItem>($"{nameof(ItemSystem)}/{nameof(SceneItem)}");
            _itemsOnScene = new Dictionary<SceneItem, Item>();
            GameObject gameObject = new GameObject
            {
                name = nameof(ItemSystem)
            };
            Transform = gameObject.transform;
            _colors = colors;
            _itemsFactory = itemsFactory;
            _interactButton = interactButton;
            _playerLayer = playerLayer;
            
            ProjectUpdater.Instance.UpdateCalled += OnUpdateButtonState;
        }

        public void Dispose()
        {
            ProjectUpdater.Instance.UpdateCalled -= OnUpdateButtonState;
        }

        public void DropItem(ItemDescriptor descriptor, Vector2 position) =>
            DropItem(_itemsFactory.CreateItem(descriptor), position);

        private void DropItem(Item item, Vector2 position)
        {
            SceneItem sceneItem = Object.Instantiate(_sceneItem, Transform);
            sceneItem.SetItem(item.ItemDescriptor.Sprite, item.ItemDescriptor.Id.ToString(),
                _colors.Find(color => color.Rarity == item.ItemDescriptor.Rarity).Color, _interactButton, _playerLayer);
            sceneItem.Drop(position);
            sceneItem.ItemClicked += PickItem;
            _itemsOnScene.Add(sceneItem, item);
        }

        private void PickItem(SceneItem sceneItem)
        {
            Item item = _itemsOnScene[sceneItem];
            Debug.Log($"Adding item {item.ItemDescriptor.Id} to inventory");
            _itemsOnScene.Remove(sceneItem);
            sceneItem.ItemClicked -= PickItem;
            Object.Destroy(sceneItem.gameObject);
        }

        private void OnUpdateButtonState()
        {
            bool buttonState = false; 
            foreach (var sceneItem in _itemsOnScene.Keys)
            {
                Collider2D player = Physics2D.OverlapCircle(sceneItem.Position, sceneItem.InteractionDistance, _playerLayer);
                if (player == null || player.isTrigger) continue;
                buttonState = true;
            }
            
            _interactButton.gameObject.SetActive(buttonState);
        }
    }
}