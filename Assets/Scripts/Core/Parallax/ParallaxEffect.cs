using System;
using System.Collections.Generic;
using Core.Enums;
using Player;
using UnityEngine;

namespace Core.Parallax
{
    public class ParallaxEffect : MonoBehaviour
    {
        private const float TargetSpeedCoef = 2f;
        
        [SerializeField] private List<ParallaxLayer> _layers;
        [SerializeField] private GameObject _target;
        
        private float _previousTargetPosition;
        

        private void OnEnable()
        {
            if (_target != null)
            {
                var speed = _target.GetComponent<PlayerEntityHandler>().StatGiver.GetStat(StatType.Speed);
                speed.SetValue(speed / TargetSpeedCoef);
            }
            _previousTargetPosition = _target.transform.position.x;
        }

        private void LateUpdate()
        {
            float deltaMovement = _previousTargetPosition - _target.transform.position.x;
            
            foreach (var layer in _layers)
            {
                Vector2 layerPosition = layer.Transform.position;
                layerPosition.x += deltaMovement * layer.Speed;
                layer.Transform.position = layerPosition;
            }

            _previousTargetPosition = _target.transform.position.x;
        }

        private void OnDisable()
        {
            if (_target != null)
            {
                var speed = _target.GetComponent<PlayerEntityHandler>().StatGiver.GetStat(StatType.Speed);
                speed.SetValue(speed * TargetSpeedCoef);
            }
        }

        
        [Serializable]
        private class ParallaxLayer
        {
            [field: SerializeField] public Transform Transform { get; private set; }
            [field: SerializeField] public float Speed { get; private set; }
        }
    }
}