using Enums;
using Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentAnimatorController : MonoBehaviour
    {
        private string locomationBlendVarName = "speedPercent";
        public float locomotionDampTime = .1f;

        private string bCombatVarName = "bCombat";

        private NavMeshAgent agent;
        private Animator animator;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();

            if(TryGetComponent(out EquipmentRig rig))
            {
                rig.OnItemAdded += x => OnItemEquipped(x);
                rig.OnItemRemoved += x => OnItemRemoved(x);
            }
        }

        private void OnItemRemoved(Equipment equipment)
        {
            if (equipment?.Type == EquipmentType.HandItemR)
                animator.SetBool(bCombatVarName, false);
        }

        private void OnItemEquipped(InventoryItem invItem)
        {
            var equipment = invItem.Item as Equipment;

            if (equipment?.Type == EquipmentType.HandItemR)
                animator.SetBool(bCombatVarName, true);
        }

        private void Update()
            => animator.SetFloat(
                locomationBlendVarName,
                agent.velocity.magnitude / agent.speed,
                locomotionDampTime,
                Time.deltaTime);
    }
}
