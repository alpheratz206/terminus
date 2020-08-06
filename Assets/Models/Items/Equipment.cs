using Enums;
using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Models.Items
{
    public class Equipment : Item
    {
        public string EquipmentType { private get; set; }

        public EquipmentType Type => (EquipmentType)Enum.Parse(typeof(EquipmentType), this.EquipmentType);

        public string MeshPath { get; set; }

        private Renderer mesh;

        public Renderer Mesh
        {
            get
            {
                if(mesh == null)
                {
                    mesh = Resources.Load<Renderer>(MeshPath);
                }

                return mesh;
            }
        }

        public override void OnInventoryUse(InventoryItem invDetails)
        {
            invDetails.Inventory.GetComponent<EquipmentRig>().Add(invDetails);
        }
    }
}
