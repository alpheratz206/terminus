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
        public Transform rootBone;
        public GameObject armature;

        public void Start()
        {
            if (TryGetComponent(out EquipmentRig rig))
            {
                rig.OnItemAdded += x => OnEquip(x);
                rig.OnItemRemoved += x => OnUnequip(x);
            }
        }

        private void OnEquip(InventoryItem invItem)
        {
            var equipment = invItem.Item as Equipment;

            var mesh = Instantiate(equipment.Mesh);

            mesh.transform.parent = transform.GetChild(2).transform;
            mesh.bones = transform.GetChild(2).GetChild(1).GetComponent<SkinnedMeshRenderer>().bones;
            mesh.rootBone = rootBone;
        }

        private void OnUnequip(Equipment item)
        {
            throw new NotImplementedException();
        }
    }
}
