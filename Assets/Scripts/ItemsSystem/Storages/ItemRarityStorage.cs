using System.Collections.Generic;
using ItemsSystem.Data;
using UnityEngine;

namespace ItemsSystem.Storages
{
    [CreateAssetMenu(fileName = "ItemRarityStorage", menuName = "ItemsSystem/ItemRarityStorage")]
    public class ItemRarityStorage : ScriptableObject
    {
        [field: SerializeField] public List<RarityDescriptor> RarityDescriptors { get; private set; }
    }
}