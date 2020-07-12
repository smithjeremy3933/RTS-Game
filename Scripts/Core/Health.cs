using RTS.Stats;
using System;
using UnityEngine;

namespace RTS.Core
{
    public class Health : MonoBehaviour
    {
        public static event EventHandler<OnPlayerDeathEventArgs> OnPlayerDeath;
        public class OnPlayerDeathEventArgs : EventArgs
        {
            public GameObject gameObject;
        }

        public static event EventHandler<OnDamageTakenEventArgs> OnDamageTaken;
        public class OnDamageTakenEventArgs : EventArgs
        {
            public GameObject gameObject;
        }

        public static event EventHandler<OnExpAwardedEventArgs> OnExpAwarded;
        public class OnExpAwardedEventArgs : EventArgs
        {
            public GameObject instigator;
        }

        public bool IsSlotted;

        float health = 15f;

        private void Start()
        {
            health = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            health = Mathf.Max(health - damage, 0);
            OnDamageTaken?.Invoke(this, new OnDamageTakenEventArgs { gameObject = gameObject });
            print(health);
            if (health == 0)
            {
                Die(instigator);
            }
        }
 
        private void Die(GameObject instigator)
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            Experience exp = instigator.GetComponent<Experience>();
            if (exp != null)
            {
                exp.AwardExp(GetComponent<BaseStats>().GetStat(Stat.ExpReward));
                OnExpAwarded?.Invoke(this, new OnExpAwardedEventArgs { instigator = instigator });
            }
            OnPlayerDeath?.Invoke(this, new OnPlayerDeathEventArgs { gameObject = gameObject });
            Destroy(gameObject);
        }

        public float GetPercentage()
        {
            return 100 * (health / GetComponent<BaseStats>().GetStat(Stat.Health));
        }
    }
}