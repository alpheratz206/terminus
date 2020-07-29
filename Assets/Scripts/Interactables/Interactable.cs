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
        public abstract bool IsAccessible { get; }

        private IDictionary<Guid, IEnumerator> interestedParties
            = new Dictionary<Guid, IEnumerator>();

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

        protected abstract void OnStopInteract();

        public virtual void StopInteracting(Guid? id = null)
        {
            OnStopInteract();
            if (id.HasValue && interestedParties.TryGetValue(id.Value, out IEnumerator ongoing))
            {
                StopCoroutine(ongoing);
                interestedParties.Remove(id.Value);
            }
        }

        private IEnumerator CheckForInteraction(
            Transform interestedParty,
            Guid id,
            Action onInteract)
        {
            while(Vector3.Distance(
              new Vector3(interestedParty.position.x, 0, interestedParty.position.z),
              new Vector3(interactionTransform.position.x, 0, interactionTransform.position.z)) > interactionRadius)
                yield return null;

            onInteract?.Invoke();

            interestedParties.Remove(id);
        }
    }
}
