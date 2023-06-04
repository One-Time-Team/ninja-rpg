using System.Collections.Generic;
using Player;
using StatsSystem.Enums;
using UnityEngine;

namespace Core.Parallax
{
    public class ParallaxEffect : MonoBehaviour
    {
        private const float TargetSpeedCoef = 2f;
        
        [SerializeField] private GameObject _target;
        [field: SerializeField] public List<ParallaxLayer> Layers { get; private set; }
        
        private float _previousTargetPosition;
        

        private void OnEnable()
        {
            if (_target.TryGetComponent(out PlayerEntityHandler player))
            {
                var speed = player.StatGiver.GetStat(StatType.Speed);
                speed.SetValue(speed / TargetSpeedCoef);
            }
            _previousTargetPosition = _target.transform.position.x;
        }

        private void LateUpdate()
        {
            float deltaMovement = _previousTargetPosition - _target.transform.position.x;
            
            foreach (var layer in Layers)
            {
                Vector2 layerPosition = layer.Transform.position;
                layerPosition.x += deltaMovement * layer.Speed;
                layer.Transform.position = layerPosition;
            }

            _previousTargetPosition = _target.transform.position.x;
        }

        private void OnDisable()
        {
            if (_target == null || !_target.TryGetComponent(out PlayerEntityHandler player)) return;
            
            var speed = player.StatGiver.GetStat(StatType.Speed);
            speed.SetValue(speed * TargetSpeedCoef);
        }
    }
}