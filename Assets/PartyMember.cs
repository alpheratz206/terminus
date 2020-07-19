using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class Party : List<PartyMember>
    {
        public bool bAllFollowing = false;

        public void Add(GameObject newMember)
           => this.Add(new PartyMember(newMember));

        public void AddRange(IEnumerable<GameObject> newMembers)
            => this.AddRange(newMembers);
    }

    public class PartyMember
    {
        public PartyMember(GameObject GameObject, bool bFollowing = true)
        {
            this.GameObject = GameObject;
            this.bFollowing = bFollowing;
        }

        public GameObject GameObject { get; set; }
        public bool bFollowing { get; set; }
    }
}
