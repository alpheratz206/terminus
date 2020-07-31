using Enums;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    public class EquipmentRenderer : MonoBehaviour
    {
        public SkinnedMeshRenderer baseMesh;

        private Dictionary<EquipmentType, SkinnedMeshRenderer> Equipment
            = new Dictionary<EquipmentType, SkinnedMeshRenderer>();

        public void Start()
        {
            var actor = transform.parent;

            if (actor.TryGetComponent(out EquipmentRig rig))
            {
                rig.OnItemAdded += x => OnEquip(x);
                rig.OnItemRemoved += x => OnUnequip(x);
            }
        }

        private void OnEquip(InventoryItem invItem)
        {
            var equipment = invItem.Item as Equipment;

            var mesh = Instantiate(equipment.Mesh);
            Equipment.Add(equipment.Type, mesh);

            mesh.transform.parent = transform;

            mesh.bones = baseMesh.bones;
            mesh.rootBone = baseMesh.rootBone;
        }

        private void OnUnequip(Equipment item)
        {
            if(Equipment.TryGetValue(item.Type, out SkinnedMeshRenderer mesh))
            {
                Equipment.Remove(item.Type);
                Destroy(mesh.gameObject);
            }
        }
    }
}
