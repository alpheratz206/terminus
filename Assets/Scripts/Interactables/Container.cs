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

        private Inventory inv;
        private InventoryRenderer invRenderer;

        private void Start()
        {
            inv = GetComponent<Inventory>();
            invRenderer = GetComponent<InventoryRenderer>();
        }

        protected override void OnInteract(Transform interestedParty)
        {
            if(interestedParty.TryGetComponent(out Inventory userInventory))
            {
                inv.OverrideOnItemUse = x =>
                {
                    inv.Remove(x);
                    userInventory.Add(x.Item);
                };
                invRenderer.SetActive(true);
            }
        }

        protected override void OnStopInteract()
        {
            inv.OverrideOnItemUse = null;
            invRenderer.SetActive(false);
        }
    }
}
