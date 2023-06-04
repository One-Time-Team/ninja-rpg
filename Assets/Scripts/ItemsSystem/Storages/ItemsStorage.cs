using System.Collections.Generic;
using ItemsSystem.Scriptable;
using UnityEngine;

namespace ItemsSystem.Storages
{
    [CreateAssetMenu(fileName = "ItemsStorage", menuName = "ItemsSystem/ItemsStorage")]
    public class ItemsStorage : ScriptableObject
    {
        [field: SerializeField] public List<BaseItemScriptable> ItemScriptables { get; private set; }
    }
}