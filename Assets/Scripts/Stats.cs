using Assets.Models;
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
        [Header("Combat")]

        public int damage = 10;
        public int attackCooldown = 2;
        public int damageThreshhold = 0;
        public int maxHealth = 100;

        private Stat<int> HealthStat;

        [Header("Skills")]
        public int identity = 10;

        private void Start()
        {
            HealthStat = new Stat<int>(0, maxHealth);
        }

        public bool TakeDamage(int incomingDamage)
        {
            int damageTaken = CalculateDamage(incomingDamage);

            HealthStat.Value -= damageTaken;

            if (HealthStat.Value == HealthStat.minValue)
            {
                if (TryGetComponent(out Actor thisActor))
                    thisActor.RemoveFocus();
                OnDeath();
                return false;
            }

            Debug.Log($"{name} takes {damageTaken} damage!");
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
            HealthStat.Value += restored;

            if (HealthStat.Value == HealthStat.maxValue)
                Debug.Log($"{name} healed to maximum!");
            else
                Debug.Log($"{name} healed by {restored}.");
        }

        public virtual void OnDeath() { Destroy(gameObject);  Debug.Log($"{name} died!"); }
    }
}
