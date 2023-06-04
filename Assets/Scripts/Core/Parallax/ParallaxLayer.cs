using System;
using UnityEngine;

namespace Core.Parallax
{
    [Serializable]
    public class ParallaxLayer
    {
        [field: SerializeField] public Transform Transform { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }

        public ParallaxLayer(Transform transform, float speed)
        {
            Transform = transform;
            Speed = speed;
        }
    }
}