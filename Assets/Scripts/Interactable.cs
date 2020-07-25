using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts
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
            if (interactionTransform == null)
                interactionTransform = transform;

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(interactionTransform.position, interactionRadius);
        }

        #endregion

        public virtual string ActionName => "Interact";
        public virtual bool IsAccessible => true;
        public virtual void OnInteract() { }
        public virtual void StopInteracting(Guid? id = null)
        {
            if (id.HasValue && interestedParties.TryGetValue(id.Value, out IEnumerator ongoing))
            {
                StopCoroutine(ongoing);
                interestedParties.Remove(id.Value);
            }
        }

        public virtual Guid BeginInteract(Transform interestedParty, Action onInteract = null)
        {
            var interactionId = Guid.NewGuid();

            var whenInRange = CheckForInteraction(interestedParty, interactionId, onInteract);

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

        private IEnumerator CheckForInteraction(Transform interestedParty, Guid id, Action onInteract = null)
        {
            while(Vector3.Distance(interestedParty.position, interactionTransform.position) > interactionRadius)
                yield return null;

            OnInteract();

            onInteract?.Invoke();

            interestedParties.Remove(id);
        }
    }
}
