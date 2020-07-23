using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts
{
    public class FormationTarget : MonoBehaviour
    {
        public Transform leader;
        public Vector3 offset;

        private void Update()
        {
            transform.position = leader.position + offset;
        }
    }
}
