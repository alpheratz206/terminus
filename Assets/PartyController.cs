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

        private PartyMember playerPartyMember
        {
            get => Party.FirstOrDefault(x => x.GameObject == playerCharacter);
            set => playerCharacter = value.GameObject;
        }

        public List<GameObject> testPartyMembers
            = new List<GameObject>();

        public Party Party
            = new Party();

        public IList<Action<GameObject>> OnPlayerChange
            = new List<Action<GameObject>>();

        private void Start()
        {
            var playerController = playerCharacter.AddComponent<PlayerController>();
            playerController.moveablePlaces = LayerMask.GetMask("Ground");

            OnPlayerChange.Add(x => PlayerController.Instance.Migrate(x));

            Party.Add(playerCharacter);
            foreach(var x in testPartyMembers)
            {
                Party.Add(x);
            }
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

            if (Input.GetKeyDown(KeyCode.F))
                ToggleAllFollow();

        }

        private void ToggleAllFollow()
        {
            Party.bAllFollowing = !Party.bAllFollowing;

            foreach (var partyMember in Party)
            {
                if (partyMember.GameObject == playerCharacter)
                    continue;

                SetFollowPlayer(partyMember, Party.bAllFollowing);
            }
        }

        private void SetFollowPlayer(PartyMember partyMember, bool bFollow = true)
        {
            var ai = partyMember.GameObject.GetComponent<CharacterBehaviour>();

            partyMember.bFollowing = bFollow;

            if (bFollow)
                ai.StartFollowing(playerCharacter.transform);
            else
                ai.StopFollowing();
        }

        private void SwitchPlayerControl(GameObject character)
            => SwitchPlayerControl(
                    Party.FindIndex(
                            x => x.GameObject == character
                        )
                );

        private void SwitchPlayerControl(int position)
        {
            if (position < 0 
             || Party.Count < position 
             || playerPartyMember == Party[position-1])
                return;

            PartyMember oldPlayer = playerPartyMember;

            playerPartyMember = Party[position-1];

            SetFollowPlayer(playerPartyMember, false);
            if (Party.bAllFollowing)
                SetFollowPlayer(oldPlayer);

            OnPlayerChange.InvokeAll(playerCharacter);
        }
    }
}
