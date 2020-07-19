using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public static class Extensions
    {
        public static void InvokeAll<T>(this IEnumerable<Action<T>> actions, T arg)
        {
            foreach(var action in actions)
                action(arg);
        }
    }
}
