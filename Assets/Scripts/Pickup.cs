using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    public class Pickup : Interactable
    {
        public override void Interact()
        {
            Debug.Log($"Adding {name} to inventory");

            //add to inventory

            Destroy(gameObject);
        }
    }
}
