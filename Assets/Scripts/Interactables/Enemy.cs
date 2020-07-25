using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Interactables
{
    public class Enemy : Interactable
    {
        public override string ActionName => $"Attack {name}";

        public override bool IsAccessible => true;

        protected override void OnInteract(Transform interestedParty)
        {
            Debug.Log($"{name} is being attacked by {interestedParty.name}!");
        }
    }
}
