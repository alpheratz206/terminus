﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Models;
using Scripts.Controllers;

namespace Scripts
{
    [RequireComponent(typeof(CharacterBehaviour))]
    [RequireComponent(typeof(Inventory))]
    public class Character : Interactable
    {
        override public string ActionName => $"Talk to {name}";
        //override public bool IsAccessible => isHostile

        public TextAsset DialogueJson;

        private DialogueTree DialogueTree;
        public CharacterBehaviour Ai { get; set; }
        public Inventory Inventory { get; set; }

        private Interactable Focus { get; set; }
        private Guid InteractionID { get; set; }
            = Guid.Empty;

        private void Start()
        {
            Ai = GetComponent<CharacterBehaviour>();
            Inventory = GetComponent<Inventory>();
            DialogueTree = DialogueJson ? JsonConvert.DeserializeObject<DialogueTree>(DialogueJson.text) : new DialogueTree();
            DialogueTree.Owner = gameObject;
        }

        public override void OnInteract()
        {
            base.OnInteract();

            DialogueController.Instance.BeginDialogue(DialogueTree);
        }

        public override void StopInteracting(Guid? interactionId = null)
        {
            base.StopInteracting(interactionId);

            DialogueController.Instance.EndDialogue();
        }

        public void SetFocus(Interactable newFocus)
        {
            RemoveFocus();
            Focus = newFocus;
            InteractionID = newFocus.BeginInteract(transform);
        }

        public void RemoveFocus()
        {
            if (InteractionID != Guid.Empty)
            {
                Focus.StopInteracting(InteractionID);
                Focus = null;
                InteractionID = Guid.Empty;
            }
        }

        public void EnableUI(bool b = true)
        {
            if(Ai.HasPath || !b)
                Ai.destinationMarker.SetActive(b);

            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = b;
        }
    }
}
