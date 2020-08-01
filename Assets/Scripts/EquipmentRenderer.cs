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

        private EquipmentMeshDictionary Equipment
            = new EquipmentMeshDictionary();

        public void Start()
        {
            var actor = transform.parent;

            if (actor.TryGetComponent(out EquipmentRig rig))
            {
                rig.OnItemAdded += x => OnEquip(x);
                rig.OnItemRemoved += x => OnUnequip(x);
            }

            foreach(var slotMeshPair in defaultEquipment)
                Equip(slotMeshPair.Key, slotMeshPair.Value);
        }

        private void OnEquip(InventoryItem invItem)
            => Equip(invItem.Item as Equipment);

        private void Equip(Equipment equipment)
            => Equip(equipment.Type, equipment.Mesh);

        private void Equip(EquipmentType slot, SkinnedMeshRenderer mesh)
        {
            var newMesh = Instantiate(mesh);

            TryUnequip(slot);

            Equipment.Add(slot, newMesh);

            newMesh.transform.parent = transform;

            newMesh.bones = baseMesh.bones;
            newMesh.rootBone = baseMesh.rootBone;
        }

        private void OnUnequip(Equipment item)
        {
            TryUnequip(item.Type);
            TryEquipDefault(item.Type);
        }

        private void TryUnequip(EquipmentType slot)
        {
            if (Equipment.TryGetValue(slot, out SkinnedMeshRenderer mesh))
            {
                Equipment.Remove(slot);
                Destroy(mesh.gameObject);
            }
        }

        private void TryEquipDefault(EquipmentType slot)
        {
            if (defaultEquipment.TryGetValue(slot, out SkinnedMeshRenderer defaultMesh))
            {
                Equip(slot, defaultMesh);
            }
        }
    }
}
