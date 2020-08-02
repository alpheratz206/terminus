using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    public class PanelManager : MonoBehaviour
    {
        #region Singleton
        public static PanelManager Instance;

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
        #endregion

        public Vector2 InitialPosition = new Vector2(50, -50);
        public float spacing = 2f;

        public List<GameObject> ManagedPanels = new List<GameObject>();

        public Vector3 GetNewPanelPosition(RectTransform toPlace)
        {
            var totalWidth =
                ManagedPanels.Where(x => x.activeSelf)
                             .Sum(x => x.GetComponent<RectTransform>().rect.width + spacing);

            return new Vector3(InitialPosition.x + totalWidth, InitialPosition.y);
        }
    }
}
