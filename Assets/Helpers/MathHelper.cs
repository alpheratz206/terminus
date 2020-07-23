using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Helpers
{
    public static class MathHelper
    {
        public static double InRadians(this double degrees)
            => (degrees * Math.PI) / 180f;

        public static double InRadians(this float degrees)
            => (degrees * Math.PI) / 180f;

        public static double InDegrees(this double radians)
            => (radians * 180f) / Math.PI;

        public static double InDegrees(this float radians)
            => (radians * 180f) / Math.PI;
    }
}
