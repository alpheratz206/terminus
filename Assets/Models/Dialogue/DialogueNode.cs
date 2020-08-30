using Assets.Enums;
using Enums;
using Models.Dialogue;
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
        public IList<DialoguePredicate> Predicates { get; set; }
            = new List<DialoguePredicate>();

        public bool Display(GameObject obj = null)
        {
            foreach (var pred in Predicates)
            {
                if (Enum.TryParse(pred.Key, out DialoguePredicateType predType)
                && DialoguePredicateDictionary.TryGetValue(predType, out Func<GameObject, DialoguePredicate, bool> f)
                && !f(obj, pred))
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
        public static Dictionary<DialoguePredicateType, Func<GameObject, DialoguePredicate, bool>> DialoguePredicateDictionary
            = new Dictionary<DialoguePredicateType, Func<GameObject, DialoguePredicate, bool>>()
            {
                { DialoguePredicateType.InParty, (speaker, logic) => PartyController.Instance.IsInParty(speaker) },
                { DialoguePredicateType.NotInParty, (speaker, logic) => !PartyController.Instance.IsInParty(speaker) },
                { DialoguePredicateType.SkillCheck, (speaker, logic) => PlayerHasStat(logic) }
            };

        private static bool PlayerHasStat(DialoguePredicate check)
        {
            if (!PartyController.Instance.playerCharacter.TryGetComponent(out Stats stats))
                return false;

            if (!Enum.TryParse(check.Stat, out StatName statToCheck) || !check.Value.HasValue)
                return false;

            if (!Enum.TryParse(check.CheckType, out CheckType checkType))
                checkType = CheckType.GreaterThanOrEqualTo;

            //can't use expression because Unity is too dumb
            switch(checkType)
            {
                case CheckType.GreaterThanOrEqualTo: return stats.Get(statToCheck).Value >= check.Value;
                case CheckType.LessThan: return stats.Get(statToCheck).Value < check.Value;
                default: return stats.Get(statToCheck).Value >= check.Value;
            }
        }
    }
}
