using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts
{
    public class DragAndDropTarget : MonoBehaviour, IDropHandler
    {
        public Inventory inventory;
        public int slotNum;

        public void OnDrop(PointerEventData eventData)
        {
            //throw new System.NotImplementedException();
        }
    }
}