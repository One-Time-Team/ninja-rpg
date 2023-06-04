using System;
using System.Collections.Generic;
using Cinemachine;
using Core.Enums;
using UnityEngine;

namespace Core.Tools
{
    [Serializable]
    public class Cameras
    {
        [SerializeField] private CinemachineVirtualCamera _rightDirectionCamera;
        [SerializeField] private CinemachineVirtualCamera _leftDirectionCamera;
        [field: SerializeField] public CinemachineVirtualCamera StartCamera { get; private set; }
        [field: SerializeField] public CinemachineVirtualCamera FinalCamera { get; private set; }
        [field: SerializeField] public Camera MainCamera { get; private set; }

        private Dictionary<Direction, CinemachineVirtualCamera> _directionalCameras;
        public Dictionary<Direction, CinemachineVirtualCamera> DirectionalCameras
        {
            get
            {
                if (_directionalCameras == null)
                {
                    _directionalCameras = new Dictionary<Direction, CinemachineVirtualCamera>()
                    {
                        { Direction.Right, _rightDirectionCamera },
                        { Direction.Left, _leftDirectionCamera }
                    };
                }

                return _directionalCameras;
            }
        }
    }
}