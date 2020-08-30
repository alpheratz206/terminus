using Assets.Models;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Enums;

namespace Scripts
{
    public class Stats : MonoBehaviour
    {
        private List<Stat<int>> AllStats { get; set; } = new List<Stat<int>>();

        [Header("Combat")]

        public int damage = 10;
        public int attackCooldown = 2;
        public int damageThreshhold = 0;
        public int maxHealth = 100;

        public Stat<int> Health => AllStats.Where(x => x.Name == StatName.Health).FirstOrDefault();

        [Header("Skills")]

        public int maxIdentity = 20;
        public Stat<int> Identity => AllStats.Where(x => x.Name == StatName.Identity).FirstOrDefault();

        private void Start()
        {
            var Health = new Stat<int>(StatName.Health, 0, maxHealth);
            AllStats.Add(Health);

            var Identity = new Stat<int>(StatName.Identity, 0, maxIdentity);
            AllStats.Add(Identity);
        }

        public Stat<int> Get(StatName name)
            => AllStats.Where(x => x.Name == name).FirstOrDefault();

        public bool TakeDamage(int incomingDamage)
        {
            var damageTaken = CalculateDamage(incomingDamage);

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
