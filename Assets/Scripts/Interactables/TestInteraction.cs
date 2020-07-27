using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Interactables
{
    public class TestInteraction : Interactable
    {
        public override string ActionName => "Test interact";

        public override bool IsAccessible => true;

        protected override void OnInteract(Transform interestedParty)
        {
            Debug.Log($"Interacting with {name}");
        }

        protected override void OnStopInteract() { }
    }
}
