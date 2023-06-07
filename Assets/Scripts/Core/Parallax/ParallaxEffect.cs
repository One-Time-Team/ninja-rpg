using System.Collections.Generic;
using UnityEngine;

namespace Core.Parallax
{
    public class ParallaxEffect : MonoBehaviour, IParallaxTargetMovement
    {
        [SerializeField] private Transform _target;
        
        private float _previousTargetPosition;

        [field: SerializeField] public List<ParallaxLayer> Layers { get; private set; }

        public float ParallaxSpeedCoef { get; private set; } = 1f;
        

        private void OnEnable()
        {
            ParallaxSpeedCoef /= 2;
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
            ParallaxSpeedCoef *= 2;
        }
    }
}