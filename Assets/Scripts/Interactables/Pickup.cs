using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            var jObj = itemJson ? JsonConvert.DeserializeObject<JObject>(itemJson.text) : null;

            if (jObj.ContainsKey("EffectType"))
                item = jObj.ToObject<Consumable>();
            else
                item = jObj.ToObject<Item>();
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
