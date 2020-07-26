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
        private Stats stats;

        private void Start()
        {
            stats = GetComponent<Stats>();
        }

        public override string ActionName => $"Attack {name}";

        public override bool IsAccessible => true;

        protected override void OnInteract(Transform interestedParty)
        {
            Debug.Log($"{name} is being attacked by {interestedParty.name}!");
            Engaging = Engage(interestedParty);
            StartCoroutine(Engaging);
        }

        protected override void OnStopInteract()
        {
            StopCoroutine(Engaging);
        }

        private IEnumerator Engaging;
        private IEnumerator Engage(Transform interestedParty)
        {
            while (stats.TakeDamage(20))
                yield return new WaitForSeconds(3);
        }
    }
}
