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
    public class Actor : MonoBehaviour
    {
        private ActorBehaviour Ai { get; set; }

        private void Start()
        {
            Ai = GetComponent<ActorBehaviour>();
        }

        public void EnableUI(bool b = true)
        {
            if (Ai.HasPath || !b)
                Ai.destinationMarker.SetActive(b);

            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = b;
        }

        public void MoveTo(Vector3 position)
        {
            RemoveFocus();
            Ai.StopInteracting();
            Ai.MoveTo(position, true);
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
            Ai.Interact(newFocus);
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
