using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dialogue
{
    public class DialogueLogic
    {
        public string Key { get; set; }
        public List<string> Keys { get; set; }
            = new List<string>();
        public string Name { get; set; }
        public int? Value { get; set; }
    }
}
