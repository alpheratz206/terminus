using Assets.Enums;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class EquipmentRig : MonoBehaviour
    {
        private bool bLinkedInventory = false;
        private Inventory Inventory;

        private Dictionary<EquipmentType, Equipment> Equipment
            = new Dictionary<EquipmentType, Equipment>();


        private void Start()
        {
            if (TryGetComponent(out Inventory))
                bLinkedInventory = true;
        }

        public void Add(Equipment equipment)
        {
            var slot = equipment.Type;

            if(Equipment.TryGetValue(slot, out Equipment foundEquipped))
            {
                if (bLinkedInventory)
                    Inventory.Add(foundEquipped);

                Equipment[slot] = null;
            }

            Equipment[slot] = equipment;
        }
    }
}
