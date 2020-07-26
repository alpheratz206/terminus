using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Interactables
{
    public class Pickup : Interactable
    {
        public override string ActionName => $"Pick up {name}";

        public override bool IsAccessible => true;

        public TextAsset itemJson;
        public Item item;

        private void Start()
        {
            item = itemJson ? JsonConvert.DeserializeObject<Item>(itemJson.text) : new Item();
        }

        protected override void OnInteract(Transform interestedParty)
        {
            if (interestedParty.TryGetComponent(out Inventory inventory))
            {
                Debug.Log($"Adding {item.Name} to inventory");
                inventory.Add(item);
                Destroy(gameObject);
            }
        }

        protected override void OnStopInteract() { }
    }
}
