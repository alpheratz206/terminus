using Assets.Scripts;
using System;
using System.Collections;
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

        public bool Contains(GameObject predMember)
            => this.Any(x => x.GameObject == predMember);

        //public void SetFormation(PartyMember focus, float radius)
        //{
        //    if (!this.Contains(focus))
        //        return;

        //    var playerDirection = focus.GameObject.transform.forward;
        //    var orthPlayerDirection = Vector3.Cross(playerDirection, Vector3.up).normalized;

        //    var alpha = radius * playerDirection;
        //    var beta = radius * orthPlayerDirection;

        //    foreach (var partyMember in this)
        //    {
        //        if (partyMember == focus)
        //            continue;

        //        partyMember.FormationOffset = GetNextFormationPos(focus.GameObject.transform.position, alpha, beta);
        //    }

        //    formationPos = -1;
        //}

        //private Vector3 GetNextFormationPos(Vector3 position, Vector3 a, Vector3 b)
        //{
        //    switch (++formationPos)
        //    {
        //        case 0: return -a + b;
        //        case 1: return -a - b;
        //        case 2: return -2 * a;
        //        default: return new Vector3(50, 0, 50);
        //    }
        //}
    }

    public class PartyMember
    {
        public CharacterBehaviour Ai { get; set; }

        public PartyMember(GameObject GameObject, Vector3 formationPos = new Vector3(), bool bFollowing = false)
        {
            this.GameObject = GameObject;
            this.bFollowing = bFollowing;

            Ai = GameObject.GetComponent<CharacterBehaviour>();
        }

        public GameObject GameObject { get; set; }
        public bool bFollowing { get; set; }

        public void StartFollowing(Transform leader)
        {
            Ai.StartFollowing(leader);
        }

        public void StopFollowing()
        {
            Ai.StopFollowing();
            Ai.onFollowingIdle.Clear();
        }
    }
}
