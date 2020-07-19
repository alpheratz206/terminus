using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public static class Extensions
    {
        public static void InvokeAll<T>(this IEnumerable<Action<T>> actions, T arg)
        {
            Debug.Log(actions.Count());
            foreach(var action in actions)
                action(arg);
        }

        public static bool isPathComplete(this UnityEngine.AI.NavMeshAgent agent)
        {
            var dist = Vector3.Distance(agent.destination, agent.transform.position);

            return (dist <= agent.stoppingDistance
                && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f));
        }
    }
}
