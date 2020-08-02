using Models;
using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class DragAndDrop : DraggablePanel, IEndDragHandler
    {
        public InventoryItem Item { get; set; }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var maybeSlot = eventData.pointerCurrentRaycast.gameObject;
            rectTransform.position = initRectPos;

            if (!maybeSlot.TryGetComponent(out DragAndDropTarget dropTarget))
                return;

            Item.Inventory.Remove(Item);

            Item.Slot = dropTarget.slotNum;
            dropTarget.inventory.Add(Item);

            Debug.Log($"Dropped over {eventData.pointerCurrentRaycast.gameObject.name}");
        }
    }
}
