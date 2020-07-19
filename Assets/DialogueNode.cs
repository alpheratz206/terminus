using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class DialogueNode
    {
        public string Prompt { get; set; }
        public IList<DialogueLine> Lines { get; set; }
        public IList<DialogueNode> Dialogue { get; set; }
    }
}
