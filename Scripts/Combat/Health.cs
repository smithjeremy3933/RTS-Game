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

        float health = 15f;

        private void Start()
        {
            health = GetComponent<BaseStats>().GetHealth();
        }

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            print(health);
            if (health == 0)
            {
                GetComponent<ActionScheduler>().CancelCurrentAction();
                OnPlayerDeath?.Invoke(this, new OnPlayerDeathEventArgs { gameObject = gameObject });
                Destroy(gameObject);
            }
        }
       
        public float GetPercentage()
        {
            return 100 * (health / GetComponent<BaseStats>().GetHealth());
        }
    }
}