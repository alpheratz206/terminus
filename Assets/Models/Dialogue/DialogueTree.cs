using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Models
{
    public class DialogueTree
    {
        [JsonIgnore]
        public GameObject Owner { get; set; }
        public string Name { get; set; }
        public IList<DialogueNode> Dialogue { get; set; }
        public string NoDialogueGreeting { get; set; }
        public bool Locking { get; set; }

        public DialogueTree()
        {
            NoDialogueGreeting = "Hi there!";
            Locking = false;
        }
    }
}
