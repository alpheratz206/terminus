using Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class Item
    {
        public string Name;
        public string InventoryIconPath { get; set; }
        public int InventoryStackSize { get; set; } = 1;

        public virtual void OnInventoryUse(InventoryItem invDetails) { Debug.Log($"Using {Name}"); }

        public static Item Init(TextAsset json)
        {
            var jObj = json ? JsonConvert.DeserializeObject<JObject>(json.text) : null;

            //needs improvement - dictionary?
            if (jObj.ContainsKey("EffectType"))
                return jObj.ToObject<Consumable>();
            else if (jObj.ContainsKey("EquipmentType"))
                return jObj.ToObject<Equipment>();
            else
                return jObj.ToObject<Item>();
        }
    }
}
