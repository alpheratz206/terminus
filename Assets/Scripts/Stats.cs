using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    public class Stats : MonoBehaviour
    {
        public int maxHealth = 100;
        private int Health { get; set; }

        private void Start()
        {
            Health = maxHealth;
        }

        public bool TakeDamage(int damage)
        {
            if (Health <= damage)
            {
                Health = 0;
                OnDeath();
                return false;
            }

            Debug.Log($"{name} takes {damage} damage!");
            Health -= damage;
            return true;
        }

        public virtual void OnDeath() { Debug.Log($"{name} died!"); }
    }
}
