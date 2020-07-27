using Enums;
using Newtonsoft.Json;
using Scripts;
using Scripts.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Consumable : Item
    {
        public string EffectType { get; set; }
        public int EffectSize { get; set; }

        public override void OnInventoryUse(InventoryItem invDetails)
        {
            invDetails--;
            if(Enum.TryParse(EffectType, out ConsumableEffectType enumEffectType)
            && ConsumableEffectsDictionary.TryGetValue(enumEffectType, out Action<Consumable, Stats> f))
            {
                f(this, PartyController.Instance.playerCharacter.GetComponent<Stats>());
            }
            
        }

        [JsonIgnore]
        public static Dictionary<ConsumableEffectType, Action<Consumable, Stats>> ConsumableEffectsDictionary
            = new Dictionary<ConsumableEffectType, Action<Consumable, Stats>>()
            {
                { ConsumableEffectType.Heal, (x,y) => y.Heal(x.EffectSize) }
            };
    }
}
