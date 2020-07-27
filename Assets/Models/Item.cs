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
        public string InventoryIconPath { get; set; }
        public int InventoryStackSize { get; set; } = 1;
    }
}
