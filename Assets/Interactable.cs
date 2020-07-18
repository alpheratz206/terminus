using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class Interactable : MonoBehaviour
    {
        public float interactionRadius = 1.5f;
        public float stoppingDistance 
        {
            get => interactionRadius * .9f;
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, interactionRadius);
        }

        public Guid TryInteract(Transform interestedParty)
        {
            var interactionId = Guid.NewGuid();

            var whenInRange = CheckForInteraction(interestedParty, interactionId);

            interestedParties.Add(interactionId, whenInRange);
            StartCoroutine(whenInRange);

            return interactionId;
        }

        private IDictionary<Guid, IEnumerator> interestedParties
            = new Dictionary<Guid, IEnumerator>();

        private IEnumerator CheckForInteraction(Transform interestedParty, Guid id)
        {
            while(Vector3.Distance(interestedParty.position, transform.position) > interactionRadius)
                yield return null;

            Debug.Log($"Interacting with {this.name}");

            interestedParties.Remove(id);
        }
    }
}
