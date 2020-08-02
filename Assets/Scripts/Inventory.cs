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
        public TextAsset[] startingItems; 

        public List<InventoryItem> items
            = new List<InventoryItem>();

        public int maxSlots = 64;

        public Action<InventoryItem> OnItemAdded;
        public Action<InventoryItem> OnStackAmtChange;
        public Action<InventoryItem> OnItemRemoved;

        public Action<InventoryItem> OverrideOnItemUse;

        private void Start()
        {
            OnStackAmtChange = x => { if (x.Count <= 0) { Remove(x); } };

            if(TryGetComponent(out EquipmentRig rig))
            {
                rig.OnItemAdded += x => Remove(x);
                rig.OnItemRemoved += x => Add(x);
            }

            foreach(var json in startingItems)
                Add(Item.Init(json));
        }

        public void Add(InventoryItem item)
            => Add(item.Item, item.Count, item.Slot);

        public void Add(Item item, int count = 1, int? slot = null)
        {
            InventoryItem itemAdded;

            InventoryItem existingStack = 
                items.Where(x => x.Item.Name == item.Name && x.Count < item.InventoryStackSize)
                     .FirstOrDefault();

            if (existingStack != null)
            {
                existingStack.Increment(count);
            }
            else
            {
                itemAdded = new InventoryItem()
                {
                    Item = item,
                    Slot = slot ?? NextAvilableSlot,
                    Count = count,
                    Inventory = this
                };

                items.Add(itemAdded);
                OnItemAdded?.Invoke(itemAdded);
            }
                
        }

        private int NextAvilableSlot
            => Enumerable.Range(1, maxSlots)
                         .Where(x => !items.Any(y => y.Slot == x))
                         .Min();

        public void Remove(InventoryItem item)
        {
            items.Remove(item);
            OnItemRemoved?.Invoke(item);
        }
    }

    public class InventoryItem
    {
        public Inventory Inventory;

        public InventoryItem()
        {
            OnUse = x => Item.OnInventoryUse(x);
        }

        public Item Item { get; set; }
        public int Count { get; set; }
        public int Slot { get; set; }
        public Sprite Icon =>
            Resources.Load<Sprite>(Item.InventoryIconPath);

        public Action<InventoryItem> onUse;
        public Action<InventoryItem> OnUse
        {
            get => Inventory.OverrideOnItemUse ?? onUse;
            set => onUse = value;
        }

        public void Increment(int by)
        {
            this.Count += by;
            this.Inventory.OnStackAmtChange?.Invoke(this);
        }

        public static InventoryItem operator ++(InventoryItem item)
        {
            item.Count++;
            item.Inventory.OnStackAmtChange?.Invoke(item);
            return item;
        }

        public static InventoryItem operator +(InventoryItem item1, InventoryItem item2)
        {
            item1.Count += item2.Count;
            item1.Inventory.OnStackAmtChange?.Invoke(item1);
            item2.Inventory.Remove(item2);
            return item1;
        }

        public static InventoryItem operator --(InventoryItem item)
        {
            item.Count--;
            item.Inventory.OnStackAmtChange?.Invoke(item);
            return item;
        }

    }
}
