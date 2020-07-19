using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class Character : Interactable
    {
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
                Focus.CancelInteract(InteractionID);
                Focus = null;
                InteractionID = Guid.Empty;
            }
        }
    }
}
