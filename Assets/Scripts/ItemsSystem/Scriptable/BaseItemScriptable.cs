using ItemsSystem.Data;
using UnityEngine;

namespace ItemsSystem.Scriptable
{
    public abstract class BaseItemScriptable : ScriptableObject
    {
        public abstract ItemDescriptor ItemDescriptor { get; }
    }
}