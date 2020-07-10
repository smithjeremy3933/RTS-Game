using System;
using System.Collections;
using System.Collections.Generic;
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
       
    }
}