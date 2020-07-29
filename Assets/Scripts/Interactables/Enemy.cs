using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Interactables
{
    [RequireComponent(typeof(Stats))]
    public class Enemy : Interactable
    {
        public bool isHostile = true;

        private Stats stats;

        private void Start()
        {
            stats = GetComponent<Stats>();
        }

        public override string ActionName => $"Attack {name}";

        public override bool IsAccessible => isHostile;

        protected override void OnInteract(Transform interestedParty)
        {
            Debug.Log($"{name} is being attacked by {interestedParty.name}!");
            Engaging = Engage(interestedParty);
            StartCoroutine(Engaging);
        }

        protected override void OnStopInteract()
        {
            if(Engaging != null)
            {
                StopCoroutine(Engaging);
                Engaging = null;
            }
        }

        private IEnumerator Engaging;
        private IEnumerator Engage(Transform interestedParty)
        {
            while (stats.TakeDamage(20))
                yield return new WaitForSeconds(3);
        }
    }
}
