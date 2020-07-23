using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets
{
    public static class Extensions
    {
        public static void InvokeAll(this IEnumerable<Action> actions)
        {
            foreach (var action in actions)
                action();
        }

        public static void InvokeAll<T>(this IEnumerable<Action<T>> actions, T arg)
        {
            foreach(var action in actions)
                action(arg);
        }

        public static void InvokeAll<T1, T2>(this IEnumerable<Action<T1, T2>> actions, T1 arg1, T2 arg2)
        {
            foreach (var action in actions)
                action(arg1, arg2);
        }

        public static bool isPathComplete(this UnityEngine.AI.NavMeshAgent agent, Vector3? destination = null, float? stoppingDistance = null)
        {
            var dist = Vector3.Distance(destination.HasValue ? destination.Value : agent.destination, agent.transform.position);
            var stopDist = stoppingDistance.HasValue ? stoppingDistance : agent.stoppingDistance;

            return (dist <= stopDist
                && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f));
        }
    }
}
