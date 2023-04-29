using Core.Parallax;
using Core.Tools;
using Player;
using UnityEngine;

namespace StaticCameras
{
    public class StaticCameraActivator : MonoBehaviour
    {
        [SerializeField] private Cameras _cameras;
        [SerializeField] private ParallaxEffect _parallax;
        

        private void OnTriggerEnter2D(Collider2D other)
        {
            _parallax.enabled = false;
            
            foreach (Transform staticCamera in transform)
            {
                if (!staticCamera.GetComponent<BoxCollider2D>().IsTouching(other)) continue;

                if (staticCamera.CompareTag("StartCamera"))
                    _cameras.StartCamera.enabled = true;
                else if (staticCamera.CompareTag("FinalCamera"))
                    _cameras.FinalCamera.enabled = true;
            }
            
            foreach (var cameraPair in _cameras.DirectionalCameras)
                cameraPair.Value.enabled = false;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _parallax.enabled = true;
            
            foreach (Transform staticCamera in transform)
            {
                if (staticCamera.GetComponent<BoxCollider2D>().IsTouching(other)) continue;
                
                if (staticCamera.CompareTag("StartCamera"))
                    _cameras.StartCamera.enabled = false;
                else if (staticCamera.CompareTag("FinalCamera"))
                    _cameras.FinalCamera.enabled = false;
            }
            
            other.GetComponent<PlayerEntityHandler>()?.UpdateCameras();
        }
    }
}