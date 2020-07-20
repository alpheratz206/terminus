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
        #region Editor Variables

        public Transform interactionTransform;
        public float interactionRadius = 1.5f;
        public float stoppingDistance 
        {
            get => interactionRadius * .9f;
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(interactionTransform.position, interactionRadius);
        }

        #endregion

        public virtual void Interact() { }
        public virtual void StopInteracting(Guid? id = null)
        {
            if (id.HasValue && interestedParties.TryGetValue(id.Value, out IEnumerator ongoing))
            {
                StopCoroutine(ongoing);
                interestedParties.Remove(id.Value);
            }
        }

        public Guid BeginInteract(Transform interestedParty)
        {
            var interactionId = Guid.NewGuid();

            var whenInRange = CheckForInteraction(interestedParty, interactionId);

            interestedParties.Add(interactionId, whenInRange);
            StartCoroutine(whenInRange);

            return interactionId;
        }

        public void CancelInteract(Guid id)
        {
            if(interestedParties.TryGetValue(id, out IEnumerator ongoing))
            {
                StopCoroutine(ongoing);
                interestedParties.Remove(id);
            }
        }

        private IDictionary<Guid, IEnumerator> interestedParties
            = new Dictionary<Guid, IEnumerator>();

        private IEnumerator CheckForInteraction(Transform interestedParty, Guid id)
        {
            while(Vector3.Distance(interestedParty.position, interactionTransform.position) > interactionRadius)
                yield return null;

            Interact();

            interestedParties.Remove(id);
        }
    }
}
