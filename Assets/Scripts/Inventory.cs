﻿using Models;
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

        public Action<InventoryItem> OnItemAdded;
        public Action<InventoryItem> OnStackAmtChange;
        public Action<InventoryItem> OnItemRemoved;

        private void Start()
        {
            OnStackAmtChange = x =>
            {
                if (x.Count <= 0)
                {
                    Remove(x);
                }
            };
        }

        public void Add(Item item)
        {
            InventoryItem itemAdded;

            InventoryItem existingStack = 
                items.Where(x => x.Item.Name == item.Name && x.Count < item.InventoryStackSize)
                     .FirstOrDefault();

            if (existingStack != null)
            {
                existingStack++;
            }
            else
            {
                itemAdded = new InventoryItem()
                {
                    Item = item,
                    Slot = NextAvilableSlot,
                    Count = 1,
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
        }
    }

    public class InventoryItem
    {
        public Inventory Inventory;
        public InventoryItem()
        {
            OnUse = () => Item.OnInventoryUse(this);
        }

        public Item Item { get; set; }
        public int Count { get; set; }
        public int Slot { get; set; }
        public Sprite Icon =>
            Resources.Load<Sprite>(Item.InventoryIconPath);
        public Action OnUse { get; set; }
        public static InventoryItem operator ++(InventoryItem item)
        {
            item.Count++;
            item.Inventory.OnStackAmtChange?.Invoke(item);
            return item;
        }
        public static InventoryItem operator --(InventoryItem item)
        {
            item.Count--;
            item.Inventory.OnStackAmtChange?.Invoke(item);
            return item;
        }

    }
}
