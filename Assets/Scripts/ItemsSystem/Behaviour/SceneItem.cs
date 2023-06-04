using System;
using Core.Services.Updater;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ItemsSystem.Behaviour
{
    public class SceneItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Transform _itemTransform;

        [Header("DropAnimation")] 
        [SerializeField] private float _dropAnimDuration;
        [SerializeField] private float _rotateAnimDuration;
        [SerializeField] private float _dropRotation;
        [SerializeField] private float _dropDistance;

        private Button _interactButton;
        private LayerMask _playerLayer;
        private Sequence _sequence;
        
        public Vector2 Position => _itemTransform.position;
        [field: SerializeField] public float InteractionDistance { get; private set; }

        public event Action<SceneItem> ItemClicked;

        private bool _textEnabled = true;
        public bool TextEnabled
        {
            set
            {
                if (_textEnabled == value)
                    return;

                _textEnabled = value;
                _canvas.enabled = value;
            }
        }

        private void Awake()
        {
            ProjectUpdater.Instance.LateUpdateCalled += UpdateCollisionWithPlayer;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_itemTransform.position, InteractionDistance);
        }
        private void OnDestroy()
        {
            ProjectUpdater.Instance.LateUpdateCalled -= UpdateCollisionWithPlayer;
        }

        public void SetItem(Sprite sprite, string itemName, Color textColor, Button interactButton, LayerMask playerLayer)
        {
            _sprite.sprite = sprite;
            _text.text = itemName;
            _text.color = textColor;
            _interactButton = interactButton;
            _playerLayer = playerLayer;
            _canvas.enabled = false;
        }

        public void Drop(Vector2 position)
        {
            transform.position = position;
            Vector2 movedPosition = transform.position + new Vector3(Random.Range(-_dropDistance, _dropDistance), 0, 0);
            _sequence = DOTween.Sequence();
            _sequence.Join(transform.DOMove(movedPosition, _dropAnimDuration));
            _sequence.Join(_itemTransform.DORotate
                (new Vector3(0, 0, Random.Range(-_dropRotation, _dropRotation)), _rotateAnimDuration));
            _sequence.OnComplete((() => _canvas.enabled = _textEnabled));
        }
        
        private void UpdateCollisionWithPlayer()
        {
            Collider2D player = Physics2D.OverlapCircle(Position, InteractionDistance, _playerLayer);
            ToggleButton(player != null);
        }
        
        private void ToggleButton(bool state)
        {
            if (state)
                _interactButton.onClick.AddListener(OnButtonClicked);
            else
                _interactButton.onClick.RemoveListener(OnButtonClicked);
        }
        
        private void OnButtonClicked() => ItemClicked?.Invoke(this);
    }
}
