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
                inv.OverrideOnItemUse =
                    x => ItemUseOverride(x, userInventory);
                invRenderer.SetActive(true);
            }
        }

        private void ItemUseOverride(InventoryItem invItem, Inventory userInventory)
        {
            if(Input.GetKeyDown(KeyCode.LeftShift))
                invItem.onUse(invItem);
            else
            {
                inv.Remove(invItem);
                userInventory.Add(invItem.Item);
            }
        }

        protected override void OnStopInteract()
        {
            inv.OverrideOnItemUse = null;
            invRenderer.SetActive(false);
        }
    }
}
