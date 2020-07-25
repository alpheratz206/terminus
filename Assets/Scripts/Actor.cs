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
using Scripts.Interactables;

namespace Scripts
{
    //Agent in the world who can interact with things using ActorBehaviour

    [RequireComponent(typeof(ActorBehaviour))]
    [RequireComponent(typeof(Inventory))]
    public class Actor : MonoBehaviour
    {
        public ActorBehaviour Ai { get; set; }
        public Inventory Inventory { get; set; }

        private void Start()
        {
            Ai = GetComponent<ActorBehaviour>();
            Inventory = GetComponent<Inventory>();
        }

        public void EnableUI(bool b = true)
        {
            if (Ai.HasPath || !b)
                Ai.destinationMarker.SetActive(b);

            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = b;
        }

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
