using System;
using RTS.Core;
using RTS.Movement;
using UnityEngine;

namespace RTS.Resources
{
    public class Gatherer : MonoBehaviour, IAction
    {
        [SerializeField] float resourceRange = 2f;
        [SerializeField] int resourceCapacity = 10;
        [SerializeField] int currentResourceAmount = 0;

        Transform resourceTarget;
        int gatherAmountIncrement = 5; 
        float timeToGather = 5f;
        float timeSinceLastGather = Mathf.Infinity;

        private void Update()
        {
            timeSinceLastGather += Time.deltaTime;

            if (resourceTarget != null)
            {
                bool isInRange = Vector3.Distance(transform.position, resourceTarget.position) < resourceRange;
                if (!isInRange && !IsFull())
                {
                    GetComponent<Mover>().MoveTo(resourceTarget.position, 10f);
                }
                else if (IsFull())
                {
                    ReturnToBase();
                }
                else if (isInRange && !IsFull())
                {
                    GetComponent<Mover>().Cancel();
                    GathererBehavior();
                }
            }
        }

        private void ReturnToBase()
        {
            throw new NotImplementedException();
        }

        private void GathererBehavior()
        {

        }

        public bool IsFull()
        {
            if (currentResourceAmount >= resourceCapacity)
            {
                return true;
            }
            return false;
        }

        public bool CanGather(GameObject target)
        {
            if (target == null)
            {
                return false;
            }
            else
                return true;
        }

        public void Gather(GameObject target)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            resourceTarget = target.transform;
        }

        public void Cancel()
        {
            resourceTarget = null;
        }
    }
}

