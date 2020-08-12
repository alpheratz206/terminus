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

        public List<Stat<IComparable>> AllStats { get; set; }

        [Header("Skills")]

        public int identity = 10;

        private void Start()
        {
            var Health = new Stat<int>("Health", 0, maxHealth);
            AllStats.Add(Health as Stat<IComparable>);
        }

        public bool TakeDamage(int incomingDamage)
        {
            int damageTaken = CalculateDamage(incomingDamage);

            Health.Value -= damageTaken;

            if (Health.Value == Health.minValue)
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
            Health.Value += restored;

            if (Health.Value == Health.maxValue)
                Debug.Log($"{name} healed to maximum!");
            else
                Debug.Log($"{name} healed by {restored}.");
        }

        public virtual void OnDeath() { Destroy(gameObject);  Debug.Log($"{name} died!"); }
    }
}
