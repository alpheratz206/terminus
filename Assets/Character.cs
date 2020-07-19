using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace Assets
{
    public class Character : Interactable
    {
        public TextAsset DialogueJson;

        private DialogueTree Dialogue;

        private Interactable Focus { get; set; }
        private Guid InteractionID { get; set; }
            = Guid.Empty;

        private void Start()
        {
            if(DialogueJson)
                Dialogue = JsonConvert.DeserializeObject<DialogueTree>(DialogueJson.text);
        }

        public override void Interact()
        {
            base.Interact();

            DialogueController.Instance.BeginDialogue(Dialogue);
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
                Focus.CancelInteract(InteractionID);
                Focus = null;
                InteractionID = Guid.Empty;
            }
        }
    }
}
