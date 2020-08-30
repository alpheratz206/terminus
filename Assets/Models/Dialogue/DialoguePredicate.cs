using Assets.Enums;
using Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dialogue
{
    public class DialoguePredicate
    {
        public string Key { get; set; }
        public string Stat { get; set; }
        public int? Value { get; set; }
        public string CheckType { get; set; }
    }
}
