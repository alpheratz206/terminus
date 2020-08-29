using Enums;
using Newtonsoft.Json;
using Scripts;
using Scripts.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Models
{
    public class DialogueNode
    {
        public string Prompt { get; set; }
        public IList<DialogueLine> Lines { get; set; }
        public IList<DialogueNode> Children { get; set; }
        public IList<DialogueNode> Siblings { get; set; }
        public bool Persist { get; set; }
        public string Action { get; set; }
        public List<string> Actions { get; set; }
            = new List<string>();
        public string Predicate { get; set; }
        public List<string> Predicates { get; set; }
            = new List<string>();
        public string PredicateName { get; set; }
        public int? PredicateValue { get; set; }


        public bool Display(GameObject obj = null)
        {
            if (!string.IsNullOrEmpty(Predicate))
                Predicates.Add(Predicate);

            foreach (string predName in Predicates)
            {
                if (Enum.TryParse(predName, out DialoguePredicateType predType)
                && DialoguePredicateDictionary.TryGetValue(predType, out Func<GameObject, bool> f)
                && !f(obj))
                    return false;
            }

            return true;

        }

        public void ExecuteAction(GameObject obj = null)
        {
            if (!string.IsNullOrEmpty(Action))
                Actions.Add(Action);

            foreach (string actionName in Actions)
            {
                if (Enum.TryParse(actionName, out DialogueActionType actionType)
                && DialogueActionDictionary.TryGetValue(actionType, out Action<GameObject> f))
                    f(obj);
            }
        }

        [JsonIgnore]
        public static Dictionary<DialogueActionType, Action<GameObject>> DialogueActionDictionary
            = new Dictionary<DialogueActionType, Action<GameObject>>()
            {
                { DialogueActionType.EndDialogue, x => DialogueController.Instance.EndDialogue() },
                //{ DialogueActionType.Return, x => DialogueController.Instance.
                { DialogueActionType.JoinParty, x => PartyController.Instance.Add(x) },
                { DialogueActionType.LeaveParty, x => PartyController.Instance.Remove(x) }
            };

        [JsonIgnore]
        public static Dictionary<DialoguePredicateType, Func<GameObject, bool>> DialoguePredicateDictionary
            = new Dictionary<DialoguePredicateType, Func<GameObject, bool>>()
            {
                { DialoguePredicateType.InParty, x => PartyController.Instance.IsInParty(x) },
                { DialoguePredicateType.NotInParty, x => !PartyController.Instance.IsInParty(x) },
                { DialoguePredicateType.HasStat, x => true /*HasStat(x)*/ }
            };

        //private bool HasStat(GameObject player)
        //{
        //    if (!player.TryGetComponent(out Stats stats))
        //        return false;

        //    if(string.IsNullOrEmpty(PredicateName) || !PredicateValue.HasValue)

        //    return true;
        //}
    }
}
