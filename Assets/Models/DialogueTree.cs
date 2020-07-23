using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DialogueTree
    {
        public string Name { get; set; }
        public IList<DialogueNode> Dialogue { get; set; }
        public string NoDialogueGreeting { get; set; }

        public DialogueTree()
        {
            NoDialogueGreeting = "Hi there!";
        }
    }
}
