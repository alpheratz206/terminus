using Assets.Enums;
using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Equipment : Item
    {
        public string EquipmentType { private get; set; }

        public EquipmentType Type => (EquipmentType)Enum.Parse(typeof(EquipmentType), this.EquipmentType);

        public override void OnInventoryUse(InventoryItem invDetails)
        {
            invDetails.Inventory.GetComponent<EquipmentRig>().Add(this);
            invDetails--;
        }
    }
}
