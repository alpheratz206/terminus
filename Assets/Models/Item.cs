using Newtonsoft.Json;
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
        public string InventoryIconPath { private get; set; }
        [JsonIgnore]
        public Sprite InventoryIcon =>
            Resources.Load<Sprite>(InventoryIconPath);
    }
}
