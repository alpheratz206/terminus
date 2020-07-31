using Enums;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.UIElements;
using UnityEngine;

namespace Scripts
{
    public class EquipmentRenderer : MonoBehaviour
    {
        [Serializable] public class EquipmentMeshDictionary : SerializableDictionary<EquipmentType, SkinnedMeshRenderer> { }

        public SkinnedMeshRenderer baseMesh;

        public EquipmentMeshDictionary defaultEquipment
            = new EquipmentMeshDictionary();

        public EquipmentMeshDictionary Equipment
            = new EquipmentMeshDictionary();

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
            => Equip(invItem.Item as Equipment);

        private void Equip(Equipment equipment)
            => Equip(equipment.Type, equipment.Mesh);

        private void Equip(EquipmentType slot, SkinnedMeshRenderer mesh)
        {
            var newMesh = Instantiate(mesh);

            Equipment.Add(slot, newMesh);

            newMesh.transform.parent = transform;

            newMesh.bones = baseMesh.bones;
            newMesh.rootBone = baseMesh.rootBone;
        }

        private void OnUnequip(Equipment item)
        {
            if(Equipment.TryGetValue(item.Type, out SkinnedMeshRenderer mesh))
            {
                Equipment.Remove(item.Type);
                Destroy(mesh.gameObject);
            }

            if(defaultEquipment.TryGetValue(item.Type, out SkinnedMeshRenderer defaultMesh))
            {
                Equip(item.Type, defaultMesh);
            }
        }
    }
}
