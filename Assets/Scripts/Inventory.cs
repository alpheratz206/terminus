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
        public List<InventoryItem> items
            = new List<InventoryItem>();

        public int maxSlots = 64;

        public Action<InventoryItem, bool> OnItemAdded; //item added, isIncremement
        public Action<InventoryItem> OnItemRemoved;

        public void Add(Item item)
        {
            InventoryItem itemAdded;

            InventoryItem existingStack = 
                items.Where(x => x.Item == item && x.Count < item.InventoryStackSize)
                     .FirstOrDefault();

            if (existingStack != null)
            {
                existingStack.Count++;
                OnItemAdded?.Invoke(existingStack, true);
            }
            else
            {
                itemAdded = new InventoryItem()
                {
                    Item = item,
                    Slot = NextAvilableSlot,
                    Count = 1
                };
                items.Add(itemAdded);
                OnItemAdded?.Invoke(itemAdded, false);
            }
                
        }

        private int NextAvilableSlot
            => Enumerable.Range(1, maxSlots)
                         .Where(x => !items.Any(y => y.Slot == x))
                         .Min();

        //public void AddRange(IEnumerable<Item> items)
        //    => this.items.AddRange(items);

        //public void Remove(Item item)
        //{
        //    items.Remove(item);
        //    OnItemRemoved?.Invoke(item);
        //}
    }

    public class InventoryItem
    {
        public Item Item { get; set; }
        public int Count { get; set; }
        public int Slot { get; set; }
        public Sprite Icon =>
            Resources.Load<Sprite>(Item.InventoryIconPath);
    }
}
