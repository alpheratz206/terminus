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
        private RectTransform rectTransform;
        private Vector2 mousePos;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            mousePos = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var delta = eventData.position - mousePos;
            rectTransform.position += new Vector3(delta.x, delta.y, transform.position.z);

            mousePos = eventData.position;
        }
    }
}
