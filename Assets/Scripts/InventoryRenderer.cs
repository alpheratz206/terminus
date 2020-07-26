using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    public class InventoryRenderer : MonoBehaviour
    {
        public GameObject InventoryUI;
        public GameObject SlotPrefab;

        public int size;
        
        public int rows;

        public float slotSize;
        public float paddingLeft;
        public float paddingTop;

        private void Start()
        {
            Resize();
            AddSlots();
        }

        private void Resize()
        {
            var rect = InventoryUI.GetComponent<RectTransform>();

            rect.SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Horizontal,
                    (size / rows) * (slotSize + paddingLeft) + paddingLeft
                );


            rect.SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Vertical,
                    rows * (slotSize + paddingTop) + paddingTop
                );
        }

        private void AddSlots()
        {
            for (int rowNum = 0; rowNum < rows; rowNum++)
            {
                for (int colNum = 0; colNum < (size / rows); colNum++)
                {
                    var newSlot = Instantiate(SlotPrefab);
                    var rect = newSlot.GetComponent<RectTransform>();

                    newSlot.transform.SetParent(InventoryUI.transform);

                    rect.localPosition
                        = new Vector3(
                            x: (colNum * (slotSize + paddingLeft)) + paddingLeft,
                            y: -((rowNum * (slotSize + paddingTop)) + paddingTop)
                        );

                    Debug.Log(rect.localPosition - rect.position);

                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);
                }
            }
        }
    }
}
