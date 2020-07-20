using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public static class Helper
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
    }
}
