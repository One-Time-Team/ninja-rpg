using System;
using System.Collections.Generic;
using ItemsSystem.Core;
using ItemsSystem.Enums;

namespace ItemsSystem
{
    public class EquipmentConditionChecker
    {
        public bool IsEquipmentConditionFits(Equipment equipment, List<Equipment> currentEquipment)
        {
            return true;
        }

        public bool TryReplaceEquipment(Equipment equipment, List<Equipment> currentEquipment,
            out Equipment oldEquipment)
        {
            oldEquipment = currentEquipment.Find(slot => slot.EquipmentType == equipment.EquipmentType);
            if (oldEquipment != null)
                return true;

            switch (equipment.EquipmentType)
            {
                case EquipmentType.Cloak:
                case EquipmentType.Gloves:
                case EquipmentType.Scabbard:
                case EquipmentType.OnBelt:
                    return true;
                case EquipmentType.None:
                default:
                    throw new NullReferenceException($"Equipment type of item {equipment.ItemDescriptor.Id} is not valid");
            }
        }
    }
}