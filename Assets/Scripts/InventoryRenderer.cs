using Assets.Scripts;
using Models;
using Scripts.Interactables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class InventoryRenderer : MonoBehaviour
    {
        private GameObject InventoryUI;
        private Transform Header => InventoryUI.transform.GetChild(0);
        private Transform Body => InventoryUI.transform.GetChild(1);

        private Inventory LinkedInventory;

        public GameObject canvas;
        public GameObject InventoryPrefab;
        public GameObject SlotPrefab;
        
        public int rows;

        public float slotSize;
        public float paddingLeft;
        public float paddingTop;

        public float TotalWidth => Body.GetComponent<RectTransform>().rect.width;

        public bool isInit = false;

        private void Start()
        {
            if (TryGetComponent(out Inventory inventory))
                Init(inventory);
        }

        public void Init(Inventory inventory)
        {
            LinkedInventory = inventory;

            Draw(inventory.maxSlots);
            SetName(name);
            SetActive(false);

            inventory.OnItemAdded += (newItem) => OnItemAdded(newItem);
            inventory.OnStackAmtChange += (item) => OnStackAmtChange(item);
            inventory.OnItemRemoved += (item) => OnItemRemoved(item);

            isInit = true;
        }

        private void OnItemAdded(InventoryItem newItem)
        {
            var slot = Body.GetChild(newItem.Slot - 1);
            var iconParent = slot.GetChild(0);

            iconParent.gameObject.SetActive(true);
            var dragAndDrop = iconParent.GetComponent<DragAndDrop>();
            dragAndDrop.Item = newItem;

            var sprite = iconParent.GetChild(0);
            var count = iconParent.GetChild(1);

            sprite.name = $"Sprite ({newItem.Item.Name})";

            sprite.GetComponent<Image>().sprite = newItem.Icon;
            sprite.GetComponent<Button>().onClick.AddListener(() => newItem.OnUse(newItem));

            var countText = count.GetComponent<TextMeshProUGUI>();
            countText.text = newItem.Count.ToString();
            countText.enabled = newItem.Count > 1;

        }

        private void OnStackAmtChange(InventoryItem item)
        {
            if(item.Count <= 0)
            {
                OnItemRemoved(item);
                return;
            }

            var slot = Body.GetChild(item.Slot - 1);

            var counter = slot.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();

            counter.text = item.Count.ToString();
            if (item.Count > 1)
                counter.enabled = true;
            else
                counter.enabled = false;
        }

        private void OnItemRemoved(InventoryItem item)
        {
            var slot = Body.GetChild(item.Slot - 1);
            var iconParent = slot.GetChild(0);

            var sprite = iconParent.GetChild(0);
            var count = iconParent.GetChild(1);

            sprite.GetComponent<Image>().sprite = null;
            sprite.GetComponent<Button>().onClick.RemoveAllListeners();
            count.GetComponent<TextMeshProUGUI>().enabled = false;

            iconParent.gameObject.SetActive(false);
        }

        private void SetName(string name)
            => Header.GetComponentInChildren<TextMeshProUGUI>().text = name;

        public void SetActive(bool b = true)
        {
            InventoryUI.GetComponent<RectTransform>().anchoredPosition =
                PanelManager.Instance.GetNewPanelPosition(
                    InventoryUI.GetComponent<RectTransform>()
                );

            InventoryUI.SetActive(b);
        }

        public void ToggleActive()
            => SetActive(!InventoryUI.activeSelf);

        private void Redraw(int size)
        {
            Destroy(InventoryUI);
            Draw(size);
        }

        private void Draw(int size)
        {
            InventoryUI = Instantiate(InventoryPrefab);
            InventoryUI.transform.SetParent(canvas.transform, false);
            PanelManager.Instance.ManagedPanels.Add(InventoryUI);
            Resize(size, rows);
            AddSlots(size, rows);
        }

        private void Resize(int numSlots, int numRows)
        {
            var mainRect = InventoryUI.GetComponent<RectTransform>();
            var rect = Body.GetComponent<RectTransform>();

            SetSizeOfRect(numSlots, numRows, mainRect);
            SetSizeOfRect(numSlots, numRows, rect);
        }

        private void SetSizeOfRect(int numSlots, int numRows, RectTransform rect)
        {
            rect.SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Horizontal,
                    (numSlots / numRows) * (slotSize + paddingLeft) + paddingLeft
                );


            rect.SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Vertical,
                    numRows * (slotSize + paddingTop) + paddingTop
                );
        }

        private void AddSlots(int numSlots, int numRows)
        {
            for (int rowNum = 0; rowNum < numRows; rowNum++)
            {
                for (int colNum = 0; colNum < (numSlots / numRows); colNum++)
                {
                    var newSlot = Instantiate(SlotPrefab);
                    var rect = newSlot.GetComponent<RectTransform>();

                    newSlot.transform.SetParent(Body);
                    newSlot.name = $"Slot {rowNum}-{colNum}";

                    var dropTarget = newSlot.GetComponent<DragAndDropTarget>();
                        
                    dropTarget.inventory = LinkedInventory;
                    dropTarget.slotNum = rowNum * (numSlots / numRows) + colNum + 1;

                    rect.localPosition
                        = new Vector3(
                            x: (colNum * (slotSize + paddingLeft)) + paddingLeft,
                            y: -((rowNum * (slotSize + paddingTop)) + paddingTop)
                        );

                    var scaleFactor = slotSize / rect.rect.width;

                    rect.transform.localScale = new Vector3(scaleFactor, scaleFactor);
                }
            }
        }
    }
}
