using System;
using UnityEngine;

namespace Core.Tools
{
    [Serializable]
    public class WorldBoundaries
    {
        [SerializeField] private Cameras _cameras;
        [SerializeField] private Transform _levelLeftBorder;
        [SerializeField] private Transform _levelRightBorder;
        

        private Vector2 _horizontalPosition;

        public void OnAwake()
        {
            _horizontalPosition.x =  _levelLeftBorder.position.x + (_levelLeftBorder.localScale.x / 2) + _cameras.MainCamera.aspect * _cameras.MainCamera.orthographicSize;
            Vector3 startCameraPosition = _cameras.StartCamera.transform.position;
            startCameraPosition = new Vector3(_horizontalPosition.x, startCameraPosition.y, startCameraPosition.z);
            _cameras.StartCamera.transform.position = startCameraPosition;

            _horizontalPosition.x =  _levelRightBorder.position.x - ((_levelRightBorder.localScale.x / 2) + _cameras.MainCamera.aspect * _cameras.MainCamera.orthographicSize);
            Vector3 finalCameraPosition = _cameras.FinalCamera.transform.position;
            finalCameraPosition = new Vector3(_horizontalPosition.x, finalCameraPosition.y, finalCameraPosition.z);
            _cameras.FinalCamera.transform.position = finalCameraPosition;
        }
    }
}