using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Interactables
{
    [RequireComponent(typeof(Inventory))]
    [RequireComponent(typeof(InventoryRenderer))]
    public class Container : Interactable
    {
        public override string ActionName => $"Open {name}";

        public override bool IsAccessible => true;

        protected override void OnInteract(Transform interestedParty)
        {
            throw new NotImplementedException();
        }

        protected override void OnStopInteract()
        {
            throw new NotImplementedException();
        }
    }
}
