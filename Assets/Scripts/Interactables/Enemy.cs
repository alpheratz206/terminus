using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    public class Enemy : Interactable
    {
        public override string ActionName => throw new NotImplementedException();

        public override bool IsAccessible => throw new NotImplementedException();

        protected override void OnInteract(Transform interestedParty)
        {
            throw new NotImplementedException();
        }
    }
}
