using Assets.Scripts;
using Newtonsoft.Json;
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
        public IList<DialogueNode> Children { get; set; }
        public IList<DialogueNode> Siblings { get; set; }
        public bool Persist { get; set; }
        public string Action { get; set; }

        public void ExecuteAction()
        {
            if(Enum.TryParse(Action, out DialogueActionType actionType)
            && DialogueActionDictionary.TryGetValue(actionType, out Action f))
                f();
        }

        [JsonIgnore]
        public static Dictionary<DialogueActionType, Action> DialogueActionDictionary
            = new Dictionary<DialogueActionType, Action>() 
            { 
                { DialogueActionType.EndDialogue, () => DialogueController.Instance.EndDialogue() },
                //{ DialogueActionType.Return, () => DialogueController.Instance.
            };

    }
}
