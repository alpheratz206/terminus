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


        #region Follow Target

        public void Follow(Interactable focus)
        {
            StopFollowing();

            agent.stoppingDistance = focus.stoppingDistance;
            agent.updateRotation = false;
            agent.SetDestination(focus.transform.position);

            following = Follow(focus.transform);
            StartCoroutine(following);
        }

        public void StopFollowing()
        {
            if (!IsFollowing())
                return;

            StopCoroutine(following);
            following = null;
            agent.updateRotation = true;
        }

        public bool IsFollowing()
            => following != null;

        #endregion

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

        #region Coroutines

        private IEnumerator following;
        private IEnumerator Follow(Transform focus)
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

        #endregion
    }
}
