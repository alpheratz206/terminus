using Models;
using Newtonsoft.Json;
using Scripts.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Interactables
{
    //An object that can be interacted with for dialogue

    public class Character : Interactable
    {
        private void Start()
        {
            DialogueTree = DialogueJson ? JsonConvert.DeserializeObject<DialogueTree>(DialogueJson.text) : new DialogueTree();
            DialogueTree.Owner = gameObject;
        }

        public TextAsset DialogueJson;

        private DialogueTree DialogueTree;

        public override string ActionName => $"Talk to {name}";

        public override bool IsAccessible => true; //is hostile

        protected override void OnInteract(Transform interestedParty)
            => DialogueController.Instance.BeginDialogue(DialogueTree);

        protected override void OnStopInteract()
            => DialogueController.Instance.EndDialogue();
    }
}
