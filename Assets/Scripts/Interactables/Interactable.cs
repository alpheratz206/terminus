using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Interactables
{
    public abstract class Interactable : MonoBehaviour
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

        public abstract string ActionName { get; }
        public virtual bool IsAccessible => true;

        private IDictionary<Guid, IEnumerator> interestedParties
            = new Dictionary<Guid, IEnumerator>();

        //the interaction logic
        protected abstract void OnInteract(Transform interestedParty);

        public Guid BeginInteract(Transform interestedParty, Action additionalInteract = null)
        {
            var interactionId = Guid.NewGuid();

            Action onInteract = delegate { OnInteract(interestedParty); } + additionalInteract;

            var whenInRange = CheckForInteraction(interestedParty, interactionId, onInteract);

            interestedParties.Add(interactionId, whenInRange);
            StartCoroutine(whenInRange);

            return interactionId;
        }

        protected virtual void OnStopInteract() { }

        public virtual void StopInteracting(Guid? id = null)
        {
            if (id.HasValue && interestedParties.TryGetValue(id.Value, out IEnumerator ongoing))
            {
                OnStopInteract();
                StopCoroutine(ongoing);
                interestedParties.Remove(id.Value);
            }
        }

        private IEnumerator CheckForInteraction(
            Transform interestedParty,
            Guid id,
            Action onInteract)
        {
            while(Vector3.Distance(interestedParty.position, interactionTransform.position) > interactionRadius)
                yield return null;

            onInteract?.Invoke();

            interestedParties.Remove(id);
        }
    }
}
