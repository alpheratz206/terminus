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

        private int formationPos;
        private int FormationPos
        {
            get => formationPos;
            set => formationPos = value > 3 ? 3 : value;
        }

        public void Add(GameObject newMember)
           => this.Add(new PartyMember(newMember));

        public void AddRange(IEnumerable<GameObject> newMembers)
            => this.AddRange(newMembers);

        public bool Contains(GameObject predMember)
            => this.Any(x => x.GameObject == predMember);

        public void FormAround(PartyMember focus, float radius)
        {
            if (!this.Contains(focus))
                return;

            var playerRotation = focus.GameObject.transform.eulerAngles.y.InRadians();

            float a = (float)(radius * Math.Sin(Math.PI - playerRotation));
            float b = (float)(radius * Math.Cos(Math.PI - playerRotation));

            this.ForEach(x => x.Ai.MoveTo(GetNextFormationPos(focus.GameObject.transform.position, a, b)));
            formationPos = -1;
        }

        private Vector3 GetNextFormationPos(Vector3 position, float a, float b)
        {
            //swastika

            switch (++formationPos)
            {
                case 0: return position;
                case 1: return position - new Vector3(a + b, 0, -a + b);
                case 2: return position - new Vector3(-a + b, 0, -a - b);
                case 3: return position - new Vector3(2*b, 0, 2*a);
                default: return position;
            }
        }
    }

    public class PartyMember
    {
        public CharacterBehaviour Ai { get; set; }

        public PartyMember(GameObject GameObject, bool bFollowing = true)
        {
            this.GameObject = GameObject;
            this.bFollowing = bFollowing;

            Ai = GameObject.GetComponent<CharacterBehaviour>();
        }

        public GameObject GameObject { get; set; }
        public bool bFollowing { get; set; }
    }
}
