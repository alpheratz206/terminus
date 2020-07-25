using Helpers;
using Scripts.Interactables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class ActorBehaviour : MonoBehaviour
    {
        #region Editor Variables

        public Sprite destinationSprite;

        public float followingDistance = 4f;
        public float lookRotationSpeed = 5f;
        public float LookRotationSpeed
        {
            get => lookRotationSpeed * Time.deltaTime;
            private set => lookRotationSpeed = value;
        }

        #endregion

        [NonSerialized]
        public GameObject destinationMarker;
        private void InitMarker()
        {
            destinationMarker = new GameObject();
            destinationMarker.SetActive(false);
            var spriteRenderer = destinationMarker.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = destinationSprite;
            destinationMarker.transform.Rotate(90, 0, 0);

        }

        NavMeshAgent agent;
        public bool isResponsive = true;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            InitMarker();
        }

        public bool IsStopped
            => agent.isStopped;

        public bool HasPath
            => agent.hasPath;

        public void MoveTo(Vector3 point, bool DisplayPath = false)
        {
            agent.SetDestination(point);

            if(DrawingSprite != null)
                StopCoroutine(DrawingSprite);

            destinationMarker.SetActive(false);

            if (DisplayPath)
            {
                DrawingSprite = DrawSprite(point);
                StartCoroutine(DrawingSprite);
            }
        }


        private IEnumerator DrawingSprite; 
        private IEnumerator DrawSprite(Vector3 dest)
        {
            var ray = new Ray(dest + new Vector3(0, 100, 0), Vector3.down);
            Physics.Raycast(ray, out var hit);

            destinationMarker.transform.position = hit.point + new Vector3(0, .1f, 0);

            destinationMarker.SetActive(true);

            while (agent.pathPending)
                yield return null;

            while (!agent.isPathComplete(stoppingDistance: .1f))
                yield return null;


            destinationMarker.SetActive(false);
        }

        public void BeginTeleport()
        {
            MoveTo(transform.position);
            isResponsive = false;
            StartCoroutine(
                InputHelper.WaitForMouseClick(pos => 
                    { StartCoroutine(DrawSprite(pos)); agent.Warp(pos); isResponsive = true; },
                    0,
                    1.5f
                )
            );
        }

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
            MoveTo(focus.interactionTransform.position);

            Interacting = Interact(focus.interactionTransform);
            StartCoroutine(Interacting);
        }

        public void StopInteracting()
        {
            if (!isInteracting())
                return;

            agent.stoppingDistance = 0f;

            StopCoroutine(Interacting);
            Interacting = null;
            agent.updateRotation = true;
        }

        public bool isInteracting()
            => Interacting != null;

        #endregion

        #region Follow

        public void StartFollowing(Transform leader, Vector3? offset = null)
        {
            StopFollowing();

            if(offset == null)
                agent.stoppingDistance = followingDistance;

            MoveTo(leader.position);

            Following = Follow(leader, offset ?? Vector3.zero);
            StartCoroutine(Following);
        }

        public void StopFollowing()
        {
            if (!isFollowing())
                return;

            agent.ResetPath();
            agent.stoppingDistance = 0f;

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

            while (focus)
            {
                MoveTo(focus.position);
                Face(focus);
                yield return null;
            }
        }

        private IEnumerator Following;
        private IEnumerator Follow(Transform leader, Vector3 offset)
        {
            while (agent.pathPending)
                yield return null;

            bool idling = false;

            while (true)
            {
                if (agent.isPathComplete(leader.position) || (idling && !Input.GetMouseButtonDown(0)))
                {
                    idling = true;
                }
                else
                {
                    idling = false;
                    MoveTo(leader.position + offset);
                }

                yield return null;
            }
        }

        #endregion
    }
}
