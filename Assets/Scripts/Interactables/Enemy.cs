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

        public override bool IsAccessible(Transform InterestedParty)
            => isHostile && InterestedParty != transform;

        protected override void OnInteract(Transform interestedParty)
        {
            Debug.Log($"{name} is being attacked by {interestedParty.name}!");

            if(TryGetComponent(out Actor thisActor) && interestedParty.TryGetComponent(out Enemy enemy))
            {
                thisActor.SetFocus(enemy);
            }

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
            var enemyStats = interestedParty.GetComponent<Stats>();

            interestedParty.GetChild(3).TryGetComponent(out Animator enemyAnimator);

            while (stats.TakeDamage(enemyStats.damage))
            {
                if (enemyAnimator)
                    StartCoroutine(PlayAnimation(enemyAnimator, 1));
                yield return new WaitForSeconds(enemyStats.attackCooldown);
            }
        }

        private IEnumerator PlayAnimation(Animator enemyAnimator, float duration)
        {
            enemyAnimator.SetInteger("attackAnim", 1);
            yield return new WaitForSeconds(duration);
            enemyAnimator.SetInteger("attackAnim", 0);
        }

    }
}
