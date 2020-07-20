using Assets;
using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    private CharacterBehaviour ai;

    private Camera mainCamera;
    public LayerMask moveablePlaces;

    void Start()
    {
        character = GetComponent<Character>();
        ai = GetComponent<CharacterBehaviour>();

        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!ai.isResponsive)
            return;

        if (Input.GetMouseButtonDown(0))
            HandleLeftClick();

        if (Input.GetMouseButtonDown(1))
            HandleRightClick();

        if (Input.GetKeyDown(KeyCode.T))
            ai.BeginTeleport();
    }

    private void HandleLeftClick()
        => InputHelper.MouseClick(hit =>
            {
                character.RemoveFocus();
                ai.MoveTo(hit.point);
                ai.StopInteracting();
            }, 
            mask: moveablePlaces
        );

    private void HandleRightClick()
        => InputHelper.MouseClick(hit =>
            {
                if(hit.collider.TryGetComponent(out Interactable focus))
                {
                    character.SetFocus(focus);
                    ai.Interact(focus);
                }
            }
        );

    public void Migrate(GameObject newPlayer)
    {
        var successor = newPlayer.AddComponent<PlayerController>();
        successor.moveablePlaces = this.moveablePlaces;
    }
}
