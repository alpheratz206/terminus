using Models;
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
        public int damage = 10;
        public int attackCooldown = 2;

        public int damageThreshhold = 0;

        public int maxHealth = 100;
        [SerializeField]
        private int Health;

        private void Start()
        {
            Health = maxHealth;
        }

        public bool TakeDamage(int incomingDamage)
        {
            int damageTaken = CalculateDamage(incomingDamage);

            if (Health <= damageTaken)
            {
                Health = 0;
                if (TryGetComponent(out Actor thisActor))
                    thisActor.RemoveFocus();
                OnDeath();
                return false;
            }

            Debug.Log($"{name} takes {damageTaken} damage!");
            Health -= damageTaken;
            return true;
        }

        private int CalculateDamage(int incoming)
        {
            if (incoming < damageThreshhold)
                return 0;

            return incoming;
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

        public virtual void OnDeath() { Destroy(gameObject);  Debug.Log($"{name} died!"); }
    }
}
