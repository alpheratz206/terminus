using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    public class Pickup : Interactable
    {
        override public string ActionName => $"Pick up {name}";

        public TextAsset itemJson;
        public Item item;

        private void Start()
        {
            item = itemJson ? JsonConvert.DeserializeObject<Item>(itemJson.text) : new Item();
        }

        public override Guid BeginInteract(Transform interestedParty, Action onInteract = null)
        {
            if(interestedParty.TryGetComponent(out Inventory inventory))
                onInteract += () => inventory.Add(item);

            return base.BeginInteract(interestedParty, onInteract);
        }

        public override void OnInteract()
        {
            Debug.Log($"Adding {item.Name} to inventory");

            Destroy(gameObject);
        }
    }
}
