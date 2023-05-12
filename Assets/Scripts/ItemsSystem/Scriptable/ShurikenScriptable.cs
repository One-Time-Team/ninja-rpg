using ItemsSystem.Data;
using UnityEngine;

namespace ItemsSystem.Scriptable
{
    [CreateAssetMenu(fileName = "Shuriken", menuName = "ItemsSystem/Shuriken")]
    public class ShurikenScriptable : BaseItemScriptable
    {
        [SerializeField] private ShurikenDescriptor _shurikenDescriptor;
        
        public override ItemDescriptor ItemDescriptor => _shurikenDescriptor;
    }
}