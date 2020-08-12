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
    public class ItemRepository : Repository<Item>
    {
        protected override string FolderName => "Items";
        protected override Item GetAsType(JObject jObj)
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
