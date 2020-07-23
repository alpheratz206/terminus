using System;
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
    public class Character : Interactable
    {
        public TextAsset DialogueJson;

        private DialogueTree Dialogue;
        public CharacterBehaviour Ai { get; set; }

        private Interactable Focus { get; set; }
        private Guid InteractionID { get; set; }
            = Guid.Empty;

        private void Start()
        {
            Ai = GetComponent<CharacterBehaviour>();
            Dialogue = DialogueJson ? JsonConvert.DeserializeObject<DialogueTree>(DialogueJson.text) : new DialogueTree();
        }

        public override void Interact()
        {
            base.Interact();

            DialogueController.Instance.BeginDialogue(Dialogue);
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
            if(Ai.HasPath)
                Ai.destinationMarker.SetActive(b);

            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = b;
        }
    }
}
