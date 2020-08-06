using Enums;
using Models;
using Models.Items;
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
        [Serializable] public class EquipmentMeshDictionary : SerializableDictionary<EquipmentType, Renderer> { }

        public SkinnedMeshRenderer baseMesh;

        public Transform RightHandItemBone;
        public Transform LeftHandItemBone;

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

        private void Equip(EquipmentType slot, Renderer mesh)
        {
            var newMesh = Instantiate(mesh);

            var skinnedMesh = newMesh as SkinnedMeshRenderer;
            var staticMesh = newMesh as MeshRenderer;

            TryUnequip(slot);

            if(skinnedMesh != null)
            {
                Equipment.Add(slot, skinnedMesh);

                skinnedMesh.transform.parent = transform;

                skinnedMesh.bones = baseMesh.bones;
                skinnedMesh.rootBone = baseMesh.rootBone;
            }

            if(staticMesh != null)
            {
                Equipment.Add(slot, staticMesh);

                staticMesh.transform.SetParent(
                    slot == EquipmentType.HandItemR ? 
                        RightHandItemBone :
                        LeftHandItemBone,
                    false
                );
            }
        }

        private void Equip(EquipmentType slot, MeshRenderer mesh)
        {

        }

        private void OnUnequip(Equipment item)
        {
            TryUnequip(item.Type);
            TryEquipDefault(item.Type);
        }

        private void TryUnequip(EquipmentType slot)
        {
            if (Equipment.TryGetValue(slot, out Renderer mesh))
            {
                Equipment.Remove(slot);
                Destroy(mesh.gameObject);
            }
        }

        private void TryEquipDefault(EquipmentType slot)
        {
            if (defaultEquipment.TryGetValue(slot, out Renderer defaultMesh))
            {
                Equip(slot, defaultMesh);
            }
        }
    }
}
