using Assets.Scripts.ItemsSystem.Behaviour;
using ItemsSystem.Core;
using ItemsSystem.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ItemsSystem
{
    class ItemSystem
    {
        private SceneItem _sceneItem;
        private ItemsFactory _itemsFactory;

        private List<IRarityColor> _colors;
        private Dictionary<SceneItem, Item> _itemsOnScene;

        public Transform Transform { get; }

        public ItemSystem(List<IRarityColor> colors, ItemsFactory itemsFactory)
        {
            _sceneItem = Resources.Load<SceneItem>($"{nameof(ItemSystem)}/SceneItem(UIButton)");  //{nameof(SceneItem)}");
            _itemsOnScene = new Dictionary<SceneItem, Item>();
            GameObject gameObject = new GameObject();
            gameObject.name = nameof(ItemSystem);
            Transform = gameObject.transform;
            _colors = colors;
            _itemsFactory = itemsFactory;
        }

        public void DropItem(ItemDescriptor descriptor, Vector2 position) =>
            DropItem(_itemsFactory.CreateItem(descriptor), position);

        private void DropItem(Item item, Vector2 position)
        {
            SceneItem sceneItem = Object.Instantiate(_sceneItem, Transform);
            sceneItem.SetItem(item.ItemDescriptor.Sprite, item.ItemDescriptor.Id.ToString(),
                _colors.Find(color => color.Rarity == item.ItemDescriptor.Rarity).Color);
            sceneItem.ItemClicked += TryPickItem;
            sceneItem.transform.position = position;
            _itemsOnScene.Add(sceneItem, item);
        }

        private void TryPickItem(SceneItem sceneItem)
        {
            Item item = _itemsOnScene[sceneItem];
            Debug.Log($"Adding item {item.ItemDescriptor.Id} to inventory");
            _itemsOnScene.Remove(sceneItem);
            sceneItem.ItemClicked -= TryPickItem;
            Object.Destroy(sceneItem.gameObject);
        }
    }
}
