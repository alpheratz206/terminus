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
        protected Vector2 mousePos;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            initRectPos = rectTransform.position;
            mousePos = eventData.position;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            var delta = eventData.position - mousePos;
            rectTransform.position += new Vector3(delta.x, delta.y, transform.position.z);

            mousePos = eventData.position;
        }
    }
}
