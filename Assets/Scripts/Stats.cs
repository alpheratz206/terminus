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
        [SerializeField]
        private int Health;

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

        public void Heal(int restored)
        {
            Health += restored;
            if (Health > maxHealth)
            {
                Debug.Log($"{name} healed by {restored - Health + maxHealth}.");
                Health = maxHealth;
            }
            else
                Debug.Log($"{name} healed by {restored}.");
        }

        public virtual void OnDeath() { Debug.Log($"{name} died!"); }
    }
}
