using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts
{
    public class DraggablePanel : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        protected RectTransform rectTransform;
        protected Vector2 initRectPos;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            initRectPos = rectTransform.position;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            rectTransform.position += new Vector3(eventData.delta.x, eventData.delta.y);
        }
    }
}
