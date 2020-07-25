using Helpers;
using Scripts.Interactables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Scripts.Controllers
{
    [RequireComponent(typeof(Actor))]
    [RequireComponent(typeof(ActorBehaviour))]
    public class PlayerController : MonoBehaviour
    {
        #region Singleton
        public static PlayerController Instance;

        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance);

            Instance = this;
        }
        #endregion

        private Actor actor;
        public LayerMask moveablePlaces;

        void Start()
        {
            actor = GetComponent<Actor>();
        }

        void Update()
        {
            //if (!actor.Ai.isResponsive)
            //    return;

            if (Input.GetMouseButtonDown(0))
                InputHelper.MouseClick(hit => HandleLeftClick(hit), mask: moveablePlaces);

            if (Input.GetMouseButtonDown(1))
                InputHelper.MouseClick(hit => HandleRightClick(hit));

            //if (Input.GetKeyDown(KeyCode.T))
            //    actor.Ai.BeginTeleport();

            if (Input.GetKeyDown(KeyCode.Space))
                ToggleGamePlayPause();
        }

        private void ToggleGamePlayPause()
        {
            if (Time.timeScale == 1)
                Time.timeScale = 0;
            else if (Time.timeScale == 0)
                Time.timeScale = 1;
        }

        private void HandleLeftClick(RaycastHit hit)
            => actor.MoveTo(hit.point);

        private void HandleRightClick(RaycastHit hit)
        {
            var possibleInteracts = hit.collider.GetComponents<Interactable>().Where(x => x.IsAccessible);

            if (possibleInteracts.Count() == 1)
                actor.SetFocus(possibleInteracts.First());

            if(possibleInteracts.Count() > 1)
            {
                Debug.Log($"Opening context menu on {hit.collider.name}");
                foreach(var interact in possibleInteracts)
                {
                    Debug.Log(interact.ActionName);
                }
            }
        }

        public void Migrate(GameObject newPlayer)
        {
            var successor = newPlayer.AddComponent<PlayerController>();
            successor.moveablePlaces = this.moveablePlaces;
        }
    }
}
