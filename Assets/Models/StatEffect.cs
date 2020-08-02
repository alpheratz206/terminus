using Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class StatEffect
    {
        public int Magnitude { get; set; }
        public float Duration { get; set; }
        public StatEffectType Type { get; set; }
    }
}
