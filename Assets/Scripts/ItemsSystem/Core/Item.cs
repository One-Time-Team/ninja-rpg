using ItemsSystem.Data;

namespace ItemsSystem.Core
{
    public abstract class Item
    {
         public abstract ItemDescriptor ItemDescriptor { get; }
         public abstract int Amount { get; }

         public abstract void Use();
    }
}