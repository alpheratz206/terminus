using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Controllers
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
        public float FormationRadius = 3f;

        private PartyMember playerPartyMember
        {
            get => Party.FirstOrDefault(x => x.gameObject == playerCharacter);
            set => playerCharacter = value.gameObject;
        }

        public List<GameObject> testPartyMembers
            = new List<GameObject>();

        private Party Party
            = new Party();

        public IList<Action<GameObject, GameObject>> OnPlayerChange
            = new List<Action<GameObject, GameObject>>();

        private void Start()
        {
            var playerController = playerCharacter.AddComponent<PlayerController>();
            playerController.moveablePlaces = LayerMask.GetMask("Ground");

            OnPlayerChange.Add((prevChar, newChar) => PlayerController.Instance.Migrate(newChar));
            OnPlayerChange.Add((prevChar, newChar) => prevChar.GetComponent<Character>().StopInteracting());
            OnPlayerChange.Add((prevChar, newChar) =>
            {
                prevChar.GetComponent<Character>().EnableUI(false);
                newChar.GetComponent<Character>().EnableUI(true);
            });

            Party.Add(playerCharacter);
            foreach(var x in testPartyMembers)
            {
                Party.Add(x);
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                HandleLeftClick();

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

        public void Add(GameObject newMember)
        {
            Party.Add(newMember);
            if (Party.bAllFollowing)
                SetFollowPlayer(Party.Get(newMember), true);
        }

        public void Remove(GameObject member)
        {
            Party.Get(member).StopFollowing();
            Party.Remove(member);
        }

        public bool IsInParty(GameObject gameObject)
            => Party.Contains(gameObject);

        private void HandleLeftClick()
            => InputHelper.MouseClick(hit =>
            {
                if (Party.Contains(hit.collider.gameObject))
                    SwitchPlayerControl(hit.collider.gameObject);
            }
        );

        private void ToggleAllFollow()
        {
            Party.bAllFollowing = !Party.bAllFollowing;

            SetAllFollow(Party.bAllFollowing);
        }

        private void SetAllFollow(bool bFollow = true)
        {
            foreach (var partyMember in Party)
            {
                if (partyMember.gameObject == playerCharacter)
                    continue;

                SetFollowPlayer(partyMember, bFollow);
            }
        }

        private void SetFollowPlayer(PartyMember partyMember, bool bFollow = true)
        {
            partyMember.bFollowing = bFollow;

            if (bFollow)
                partyMember.StartFollowing(playerCharacter.transform);
            else
                partyMember.StopFollowing();
        }

        private void SwitchPlayerControl(GameObject character)
            => SwitchPlayerControl(
                    Party.FindIndex(
                            x => x.gameObject == character
                        ) + 1
                );

        private void SwitchPlayerControl(int position)
        {
            if (position < 0 
             || Party.Count < position 
             || playerPartyMember == Party[position-1])
                return;

            var oldPlayerChararcter = playerCharacter;

            playerPartyMember = Party[position-1];

            SetFollowPlayer(playerPartyMember, false);
            if (Party.bAllFollowing)
                SetAllFollow(true);

            OnPlayerChange.InvokeAll(oldPlayerChararcter, playerCharacter);
        }
    }
}
