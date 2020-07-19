using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (Input.GetMouseButtonDown(0))
            HandleLeftClick();

        if (Input.GetMouseButtonDown(1))
            HandleRightClick();
    }

    private void HandleLeftClick()
        => MouseClick(hit =>
               {
                   character.RemoveFocus();
                   ai.MoveTo(hit.point);
                   ai.StopInteracting();
               }, 
               mask: moveablePlaces
           );

    private void HandleRightClick()
        => MouseClick(hit =>
               {
                   if(hit.collider.TryGetComponent(out Interactable focus))
                   {
                       character.SetFocus(focus);
                       ai.Interact(focus);
                   }
               }
           );

    private void MouseClick(Action<RaycastHit> onHit, LayerMask? mask = null)
    {
        RaycastHit hit;

        bool raycastSuccess =
            mask != null ?
                Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100f, mask.Value) :
                Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100f);

        if (raycastSuccess)
            onHit(hit);
    }

    public void Migrate(GameObject newPlayer)
    {
        var successor = newPlayer.AddComponent<PlayerController>();
        successor.moveablePlaces = this.moveablePlaces;
    }
}
