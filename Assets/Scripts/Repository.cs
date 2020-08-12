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
    public abstract class Repository<T> where T : new()
    {
        protected abstract string FolderName { get; }

        private Dictionary<string, JObject> AllRecords
            = new Dictionary<string, JObject>();

        public void Init()
        {
            var allJson = Resources.LoadAll<TextAsset>(FolderName);

            foreach (var json in allJson)
            {
                var jObj = JsonConvert.DeserializeObject<JObject>(json.text);

                if (jObj.TryGetValue("ID", out var id))
                {
                    AllRecords.Add(id.Value<string>(), jObj);
                }
            }
        }

        public T Get(string ID)
        {
            if (AllRecords.TryGetValue(ID, out JObject foundItem))
            {
                return GetAsType(foundItem);
            }

            Debug.Log($"{typeof(T).Name} with ID of {ID} not found.");
            return new T();
        }

        protected virtual T GetAsType(JObject jObj)
        {
            return jObj.ToObject<T>();
        }
    }
}
