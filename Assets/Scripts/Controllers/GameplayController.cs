using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Controllers
{
    public class GameplayController : MonoBehaviour
    {
        public static GameplayController Instance;

        [SerializeField]
        private bool PlayerCanTogglePause = true;
        [SerializeField]
        private bool _GamePlayPause = false;

        public bool GamePlayPause
        {
            get => _GamePlayPause;
            set
            {
                _GamePlayPause = value;
                Time.timeScale = value ? 0 : 1; // side effect
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DialogueController.Instance.onDialogueDecisionTime.Add(() => DisableControlAndPause()); 
            DialogueController.Instance.onDialogueDecisionMade.Add(() => EnableControlAndUnpause());
        }

        private void EnableControlAndUnpause()
        {
            if (DialogueController.Instance.Conversation.Locking)
            {
                PlayerCanTogglePause = true;
                GamePlayPause = false;
            } 
        }

        private void DisableControlAndPause()
        {
            PlayerCanTogglePause = false;
            GamePlayPause = true;
        }

        private void Update()
        {
            if (PlayerCanTogglePause && Input.GetKeyDown(KeyCode.Space))
                ToggleGamePlayPause();
        }

        public void ToggleGamePlayPause()
            => GamePlayPause = !GamePlayPause;
    }
}
