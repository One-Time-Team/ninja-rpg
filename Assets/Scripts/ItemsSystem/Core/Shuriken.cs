using ItemsSystem.Data;
using ItemsSystem.Enums;

namespace ItemsSystem.Core
{
    public class Shuriken : Item
    {
        private readonly ShurikenDescriptor _shurikenDescriptor;

        private int _amount;
        
        public override ItemDescriptor ItemDescriptor => _shurikenDescriptor;
        public override int Amount => _amount;
        public EquipmentType EquipmentType { get; }


        public Shuriken(ItemDescriptor shurikenDescriptor)
        {
            _shurikenDescriptor = shurikenDescriptor as ShurikenDescriptor;
            EquipmentType = EquipmentType.OnBelt;
            _amount = 1;
        }

        public override void Use()
        {
            _amount--;
            Throw();

            if (_amount <= 0)
                Destroy();
        }

        private void Throw()
        {
            throw new System.NotImplementedException();
        }
        
        public void AddToStack(int amount)
        {
            _amount += amount;
        }
        
        private void Destroy()
        {
            throw new System.NotImplementedException();
        }
    }
}