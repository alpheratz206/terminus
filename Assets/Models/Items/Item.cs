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

namespace Models.Items
{
    public class Item
    {
        [JsonIgnore]
        public string ID { get; set; }
        public string Name { get; set; }
        public string InventoryIconPath { get; set; }
        public int InventoryStackSize { get; set; } = 1;

        public virtual void OnInventoryUse(InventoryItem invDetails) { Debug.Log($"Using {Name}"); }
    }
}
