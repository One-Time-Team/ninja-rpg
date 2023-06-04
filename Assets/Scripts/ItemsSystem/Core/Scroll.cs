using ItemsSystem.Data;
using ItemsSystem.Enums;
using StatsSystem;

namespace ItemsSystem.Core
{
    public class Scroll : Item
    {
        private readonly StatChangingItemDescriptor _statChangingItemDescriptor;
        private readonly StatsController _statsController;
            
        private int _amount;

        public override ItemDescriptor ItemDescriptor => _statChangingItemDescriptor;
        public override int Amount => _amount;
        public EquipmentType EquipmentType { get; }


        public Scroll(ItemDescriptor itemDescriptor, StatsController statsController)
        {
            _statChangingItemDescriptor = itemDescriptor as StatChangingItemDescriptor;
            _statsController = statsController;
            EquipmentType = EquipmentType.OnBelt;
            _amount = 1;
        }

        public override void Use()
        {
            _amount--;
            
            foreach (var stat in _statChangingItemDescriptor.Stats)
            {
                _statsController.ProcessModificator(stat);
            }
            
            if (_amount <= 0)
                Destroy();
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