using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    public class Inventory : MonoBehaviour
    {
        public List<Item> items
            = new List<Item>();

        public void Add(Item item)
            => items.Add(item);

        public void AddRange(IEnumerable<Item> items)
            => this.items.AddRange(items);

        public void Remove(Item item)
            => items.Remove(item);
    }
}
