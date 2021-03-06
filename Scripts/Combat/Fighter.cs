﻿using RTS.Core;
using RTS.Movement;
using UnityEngine;

namespace RTS.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float damage = 5f;

        Transform target;
        float timeSinceLastAttack = Mathf.Infinity;

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target != null)
            {
                bool isInRange = Vector3.Distance(transform.position, target.position) < weaponRange;
                if (!isInRange)
                {
                    GetComponent<Mover>().MoveTo(target.position, 10f);
                }
                else
                {
                    GetComponent<Mover>().Cancel();
                    AttackBehavior();
                }
            }
        }

        private void AttackBehavior()
        {
            transform.LookAt(target);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                print("Attack Enemy!");
                TriggerAttack();
                timeSinceLastAttack = 0;
                Health health = target.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(gameObject, damage);
                }
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
            {
                return false;
            }
            else
                return true;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
    }
}