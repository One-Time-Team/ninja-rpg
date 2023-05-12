using ItemsSystem.Data;
using ItemsSystem.Enums;
using StatsSystem;

namespace ItemsSystem.Core
{
    public class Equipment : Item
    {
        private readonly StatChangingItemDescriptor _statChangingItemDescriptor;
        private readonly StatsController _statsController;

        private bool _equipped;

        public override ItemDescriptor ItemDescriptor => _statChangingItemDescriptor;
        public override int Amount => -1;
        public EquipmentType EquipmentType { get; }

        
        public Equipment(ItemDescriptor itemDescriptor, StatsController statsController, EquipmentType equipmentType)
        {
            _statChangingItemDescriptor = itemDescriptor as StatChangingItemDescriptor;
            _statsController = statsController;
            EquipmentType = equipmentType;
        }

        public override void Use()
        {
            if (_equipped)
                Unequip();
            else Equip();
        }

        private void Equip()
        {
            _equipped = true;
            foreach (var stat in _statChangingItemDescriptor.Stats)
            {
                _statsController.ProcessModificator(stat);
            }
        }
        
        private void Unequip()
        {
            _equipped = false;
            foreach (var stat in _statChangingItemDescriptor.Stats)
            {
                _statsController.ProcessModificator(stat.GetReversed());
            }
        }
    }
}