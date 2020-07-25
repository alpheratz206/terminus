using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Scripts.Controllers
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(CharacterBehaviour))]
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

        private Character character;
        public LayerMask moveablePlaces;

        void Start()
        {
            character = GetComponent<Character>();
        }

        void Update()
        {
            if (!character.Ai.isResponsive)
                return;

            if (Input.GetMouseButtonDown(0))
                HandleLeftClick();

            if (Input.GetMouseButtonDown(1))
                HandleRightClick();

            if (Input.GetKeyDown(KeyCode.T))
                character.Ai.BeginTeleport();

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

        private void HandleLeftClick()
            => InputHelper.MouseClick(hit =>
                {
                    character.RemoveFocus();
                    character.Ai.MoveTo(hit.point, true);
                    character.Ai.StopInteracting();
                },
                mask: moveablePlaces
            );

        private void HandleRightClick()
            => InputHelper.MouseClick(hit =>
                {
                    var possibleInteracts = hit.collider.GetComponents<Interactable>().Where(x => x.IsAccessible);

                    if (possibleInteracts.Count() == 1)
                    {
                        var focus = possibleInteracts.First();

                        character.SetFocus(focus);
                        character.Ai.Interact(focus);
                    }

                    if(possibleInteracts.Count() > 1)
                    {
                        Debug.Log($"Opening context menu on {hit.collider.name}");
                        foreach(var interact in possibleInteracts)
                        {
                            Debug.Log(interact.ActionName);
                        }
                    }
                }
            );

        public void Migrate(GameObject newPlayer)
        {
            var successor = newPlayer.AddComponent<PlayerController>();
            successor.moveablePlaces = this.moveablePlaces;
        }
    }
}
