using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CharacterBehaviour : MonoBehaviour
    {
        #region Editor Variables

        public float followingDistance = 4f;
        public float lookRotationSpeed = 5f;
        public float LookRotationSpeed
        {
            get => lookRotationSpeed * Time.deltaTime;
            private set => lookRotationSpeed = value;
        }

        #endregion

        NavMeshAgent agent;

        private void Start()
            => agent = GetComponent<NavMeshAgent>();

        public void MoveTo(Vector3 point)
            => agent.SetDestination(point);

        private void Face(Transform focus)
        {
            var vectorDirection = (focus.position - transform.position).normalized;

            Quaternion newRotation
                = Quaternion.LookRotation(
                    new Vector3(vectorDirection.x, 0f, vectorDirection.z)
                );

            transform.rotation
                = Quaternion.Slerp(
                    transform.rotation,
                    newRotation,
                    LookRotationSpeed
                );
        }


        #region Interact

        public void Interact(Interactable focus)
        {
            StopInteracting();

            agent.stoppingDistance = focus.stoppingDistance;
            agent.updateRotation = false;
            agent.SetDestination(focus.interactionTransform.position);

            Interacting = Interact(focus.interactionTransform);
            StartCoroutine(Interacting);
        }

        public void StopInteracting()
        {
            if (!isInteracting())
                return;

            StopCoroutine(Interacting);
            Interacting = null;
            agent.updateRotation = true;
        }

        public bool isInteracting()
            => Interacting != null;

        #endregion

        #region Follow

        public void StartFollowing(Transform leader)
        {
            StopFollowing();

            agent.stoppingDistance = followingDistance;

            agent.SetDestination(leader.position);

            Following = Follow(leader);
            StartCoroutine(Following);
        }

        public void StopFollowing()
        {
            if (!isFollowing())
                return;

            StopCoroutine(Following);
            Following = null;
        }

        public bool isFollowing()
            => Following != null;

        #endregion

        #region Coroutines

        private IEnumerator Interacting;
        private IEnumerator Interact(Transform focus)
        {
            while (agent.pathPending)
                yield return null;

            while (true)
            {
                agent.SetDestination(focus.position);
                Face(focus);
                yield return null;
            }
        }

        private IEnumerator Following;
        private IEnumerator Follow(Transform leader)
        {
            while (agent.pathPending)
                yield return null;

            while (true)
            {
                if (agent.isPathComplete())
                {
                    //formation / idle
                }
                else
                {
                    agent.SetDestination(leader.position);
                }

                yield return null;
            }
        }

        #endregion
    }
}
