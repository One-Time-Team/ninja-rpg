using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Core.Services.Updater;

namespace Assets.Scripts.ItemsSystem.Behaviour
{
    public class SceneItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Transform _itemTransform;
        [SerializeField] private LayerMask _playerLayerMask;

        public Vector2 Position => _itemTransform.position;

        [field: SerializeField] public float InteractionDistance { get; private set; }

        public event Action<SceneItem> ItemClicked;

        private void Awake()
        {
            _button.onClick.AddListener(() => ItemClicked?.Invoke(this));
            ProjectUpdater.Instance.UpdateCalled += UpdateCollisionWithPlayer;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_itemTransform.position, InteractionDistance);
        }
        private void OnDestroy()
        {
            ProjectUpdater.Instance.UpdateCalled -= UpdateCollisionWithPlayer;
        }

        public void SetItem(Sprite sprite, string itemName, Color textColor)
        {
            _sprite.sprite = sprite;
            _text.text = itemName;
            _text.color = textColor;
        }

        public void ToggleButton(bool state)
        {
            _canvas.gameObject.SetActive(state);
        }

        private void UpdateCollisionWithPlayer()
        {
            Collider2D player = Physics2D.OverlapCircle(Position, InteractionDistance, _playerLayerMask);
            ToggleButton(player != null);
        }
    }
}
