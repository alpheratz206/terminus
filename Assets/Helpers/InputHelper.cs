using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Helpers
{
    public static class InputHelper
    {
        public static void MouseClick(Action<RaycastHit> onHit, LayerMask? mask = null)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            RaycastHit hit;

            bool raycastSuccess =
                mask != null ?
                    Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, mask.Value) :
                    Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f);

            if (raycastSuccess)
                onHit(hit);
        }

        public static void MouseClick(Action<RaycastHit> onHit, Vector3 mousePos)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            RaycastHit hit;

            bool raycastSuccess =
                    Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100f);

            if (raycastSuccess)
                onHit(hit);
        }

        public static IEnumerator WaitForMouseClick(Action<Vector3> onClick, int mouseButton = 0, float delay = 0f)
        {
            while (!Input.GetMouseButtonDown(mouseButton))
                yield return null;

            var mousePos = Input.mousePosition;

            if (delay > 0f)
                yield return new WaitForSeconds(delay);

            MouseClick(hit => 
            {
                onClick(hit.point);
            },
            mousePos
            );
        }

        private static bool IsInsideRect(this Vector3 pos, RectTransform transform)
            => pos.x > transform.position.x
            && pos.x < transform.position.x + (transform.lossyScale.x * transform.rect.width)
            && (Screen.height - pos.y) > Math.Abs(Screen.height - transform.position.y)
            && (Screen.height - pos.y) < Math.Abs(Screen.height - transform.position.y + (transform.lossyScale.y * transform.rect.height));
    }
}
