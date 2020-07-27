using Models;
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

        public GameObject canvas;
        public GameObject InventoryPrefab;
        public GameObject SlotPrefab;

        public int size;
        
        public int rows;

        public float slotSize;
        public float paddingLeft;
        public float paddingTop;

        public bool isInit = false;

        private void Start()
        {
            if (TryGetComponent(out Inventory inventory))
                Init(inventory);
        }

        public void Init(Inventory inventory)
        {
            Draw(inventory.maxSlots);
            SetName(name);
            SetActive(false);

            inventory.OnItemAdded += (newItem) => OnItemAdded(newItem);
            inventory.OnStackIncrement += (newItem) => OnStackIncrement(newItem);

            isInit = true;
        }

        private void OnItemAdded(InventoryItem newItem)
        {
            var slot = Body.GetChild(newItem.Slot - 1);
            var iconParent = slot.GetChild(0);

            iconParent.gameObject.SetActive(true);
            var sprite = iconParent.GetChild(0);
            var count = iconParent.GetChild(1);

            sprite.GetComponent<Image>().sprite = newItem.Icon;
            count.GetComponent<TextMeshProUGUI>().text = "1";
        }

        private object OnStackIncrement(object newItem)
        {
            throw new NotImplementedException();
        }

        private void SetName(string name)
            => Header.GetComponentInChildren<TextMeshProUGUI>().text = name;

        public void SetActive(bool b = true)
            => InventoryUI.SetActive(b);

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
            Resize(size, rows);
            AddSlots(size, rows);
        }

        private void Resize(int numSlots, int numRows)
        {
            var rect = Body.GetComponent<RectTransform>();

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

                    rect.localPosition
                        = new Vector3(
                            x: (colNum * (slotSize + paddingLeft)) + paddingLeft,
                            y: -((rowNum * (slotSize + paddingTop)) + paddingTop)
                        );

                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);
                }
            }
        }
    }
}
