using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    public static class ItemRepository
    {
        private static string ItemsFolderName => "Items";

        private static Dictionary<string, JObject> AllItems
            = new Dictionary<string, JObject>();

        public static void Init()
        {
            var allJson = Resources.LoadAll<TextAsset>(ItemsFolderName);

            foreach(var json in allJson)
            {
                var jObj = JsonConvert.DeserializeObject<JObject>(json.text);

                if(jObj.TryGetValue("ID", out var id))
                {
                    AllItems.Add(id.Value<string>(), jObj);
                }
            }
        } 

        public static Item Get(string ID)
        {
            if(AllItems.TryGetValue(ID, out JObject foundItem))
            {
                return GetAsType(foundItem);
            }

            Debug.Log($"Item with ID of {ID} not found.");
            return new Item();
        }

        private static Item GetAsType(JObject jObj)
        {
            if (jObj.ContainsKey("EffectType"))
                return jObj.ToObject<Consumable>();
            else if (jObj.ContainsKey("EquipmentType"))
                return jObj.ToObject<Equipment>();
            else
                return jObj.ToObject<Item>();
        }
    }
}
