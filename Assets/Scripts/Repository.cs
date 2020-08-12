using Models;
using Models.Items;
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
    public class Repository<T> where T : new()
    {
        private Dictionary<string, JObject> AllItems
            = new Dictionary<string, JObject>();

        public void Init(string folderName)
        {
            var allJson = Resources.LoadAll<TextAsset>(folderName);

            foreach (var json in allJson)
            {
                var jObj = JsonConvert.DeserializeObject<JObject>(json.text);

                if (jObj.TryGetValue("ID", out var id))
                {
                    AllItems.Add(id.Value<string>(), jObj);
                }
            }
        }

        public T Get(string ID)
        {
            if (AllItems.TryGetValue(ID, out JObject foundItem))
            {
                return GetAsType(foundItem);
            }

            Debug.Log($"{typeof(T).Name} with ID of {ID} not found.");
            return new T();
        }

        private T GetAsType(JObject jObj)
        {
            return jObj.ToObject<T>();
        }
    }
}
