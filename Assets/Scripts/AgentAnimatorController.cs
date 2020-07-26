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
        public string locomationBlendVarName = "speedPercent";
        public float locomotionDampTime = .1f;

        private NavMeshAgent agent;
        private Animator animator;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
        }

        private void Update()
            => animator.SetFloat(
                locomationBlendVarName,
                agent.velocity.magnitude / agent.speed,
                locomotionDampTime,
                Time.deltaTime);
    }
}
