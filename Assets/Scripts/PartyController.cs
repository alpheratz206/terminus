using Assets.Scripts;
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
        public float FormationRadius = 3f;

        private PartyMember playerPartyMember
        {
            get => Party.FirstOrDefault(x => x.GameObject == playerCharacter);
            set => playerCharacter = value.GameObject;
        }

        public List<GameObject> testPartyMembers
            = new List<GameObject>();

        public Party Party
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
                prevChar.GetComponentInChildren<SpriteRenderer>().enabled = false;
                newChar.GetComponentInChildren<SpriteRenderer>().enabled = true;
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
                if (partyMember.GameObject == playerCharacter)
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
                            x => x.GameObject == character
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
