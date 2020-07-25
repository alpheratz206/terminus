using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Models;
using Scripts.Controllers;
using UnityEditor.SceneManagement;

namespace Scripts
{
    [RequireComponent(typeof(CharacterBehaviour))]
    [RequireComponent(typeof(Inventory))]
    public class Character : Interactable
    {
        public CharacterBehaviour Ai { get; set; }
        public Inventory Inventory { get; set; }

        private void Start()
        {
            Ai = GetComponent<CharacterBehaviour>();
            Inventory = GetComponent<Inventory>();
            DialogueTree = DialogueJson ? JsonConvert.DeserializeObject<DialogueTree>(DialogueJson.text) : new DialogueTree();
            DialogueTree.Owner = gameObject;
        }

        public void EnableUI(bool b = true)
        {
            if (Ai.HasPath || !b)
                Ai.destinationMarker.SetActive(b);

            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = b;
        }

        #region Interactable

        public TextAsset DialogueJson;

        private DialogueTree DialogueTree;

        public override string ActionName => $"Talk to {name}";

        public override bool IsAccessible => true; //is hostile

        protected override void OnInteract(Transform interestedParty)
            => DialogueController.Instance.BeginDialogue(DialogueTree);

        protected override void OnStopInteract()
            => DialogueController.Instance.EndDialogue();

        #endregion

        #region Interaction

        private Interactable Focus { get; set; }
        private Guid InteractionID { get; set; }
            = Guid.Empty;

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

        #endregion
    }
}
