using ItemsSystem.Data;
using UnityEngine;

namespace ItemsSystem.Scriptable
{
    [CreateAssetMenu(fileName = "StatChangingItem", menuName = "ItemsSystem/StatChangingItem")]
    public class StatChangingItemScriptable : BaseItemScriptable
    {
        [SerializeField] private StatChangingItemDescriptor _statChangingItemDescriptor;
        
        public override ItemDescriptor ItemDescriptor => _statChangingItemDescriptor;
    }
}