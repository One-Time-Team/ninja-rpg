using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Core.Animations;
using Player.Enums;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerSkinChanger : MonoBehaviour
    {
        [SerializeField] private AnimatorController _animator;
        [SerializeField] private List<Skin> _skins;

        private SpriteRenderer _spriteRenderer;
        private int _skinNumber;
        private int _animationNumber;
        
        [field: SerializeField] public PlayerSkin PlayerSkin { get; private set; }

        
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void LateUpdate()
        {
            _skinNumber = (int)PlayerSkin;
            _animationNumber = (int)_animator.CurrentAnimationType;
            
            if (CheckChangeAvailability())
                ChangeSkin();
        }

        private void ChangeSkin()
        {
            string spriteName = _spriteRenderer.sprite.name;

            if (!spriteName.Contains("MainNinja")) return;

            spriteName = Regex.Replace(spriteName, "[^0-9]", "");
            int spriteNumber = int.Parse(spriteName);
            
            Sprite newSprite = _skins[_skinNumber].Animations[_animationNumber]?.Sprites[spriteNumber];

            if (newSprite != null)
                _spriteRenderer.sprite = newSprite;
        }

        private bool CheckChangeAvailability()
        {
            return (_skinNumber < _skins.Count) && (_skinNumber >= 0) &&
                   (_animationNumber < _skins[_skinNumber].Animations.Count) &&
                   (_animationNumber >= 0);
        }

        
        [Serializable]
        private class Skin
        {
            [field: SerializeField] public List<SkinAnimation> Animations { get; private set; }
        }
        
        [Serializable]
        private class SkinAnimation
        {
            [field: SerializeField] public List<Sprite> Sprites { get; private set; }
        }
    }
}