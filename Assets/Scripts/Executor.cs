using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    public class Executor : MonoBehaviour
    {
        public static Executor Instance;
        public static ItemRepository itemRepository;

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

            itemRepository = new ItemRepository();
            itemRepository.Init();
        }

    }
}
