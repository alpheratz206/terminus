using Assets.Enums;
using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Models
{
    public class Equipment : Item
    {
        public string EquipmentType { private get; set; }

        public EquipmentType Type => (EquipmentType)Enum.Parse(typeof(EquipmentType), this.EquipmentType);

        public string MeshPath { get; set; }

        private SkinnedMeshRenderer mesh;

        public SkinnedMeshRenderer Mesh
        {
            get
            {
                if(mesh == null)
                {
                    mesh = Resources.Load<SkinnedMeshRenderer>(MeshPath);
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
