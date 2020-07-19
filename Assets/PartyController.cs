using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class PartyController : MonoBehaviour
    {
        #region Singleton
        public static PartyController Instance;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion

        public GameObject playerCharacter;

        public GameObject testPartyMember;

        public IList<GameObject> party
            = new List<GameObject>();

        public IList<Action<GameObject>> OnPlayerChange
            = new List<Action<GameObject>>();

        private void Start()
        {
            party.Add(playerCharacter);
            party.Add(testPartyMember);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                SwitchPlayerControl(1);

            if (Input.GetKeyDown(KeyCode.Alpha2))
                SwitchPlayerControl(2);

            if (Input.GetKeyDown(KeyCode.Alpha3))
                SwitchPlayerControl(3);

            if (Input.GetKeyDown(KeyCode.Alpha4))
                SwitchPlayerControl(4);

        }

        private void SwitchPlayerControl(GameObject character)
        {
            if (!party.Contains(character) || playerCharacter == character)
                return;

            playerCharacter = character;

            OnPlayerChange.InvokeAll(playerCharacter);
        }

        private void SwitchPlayerControl(int position)
        {
            if (party.Count < position || playerCharacter == party[position-1])
                return;

            playerCharacter = party[position-1];

            OnPlayerChange.InvokeAll(playerCharacter);
        }
    }
}
