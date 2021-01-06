using UnityEngine;
using UnityEngine.AI;
using RTS.Control;
using RTS.Core;

namespace RTS.Movement
{
    public class Mover : MonoBehaviour, IUnit, IAction
    {
        [SerializeField] float maxSpeed = 10f;

        NavMeshAgent navMeshAgent;
        SelectionDict selectedTable;
        GameObject go;
        int id;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            selectedTable = FindObjectOfType<PlayerController>().GetComponent<SelectionDict>();
            go = gameObject;
            id = go.GetInstanceID();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            if (speed < 0.2)
                speed = 0;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, maxSpeed);
        }

        public bool IsSelected()
        {
            return selectedTable.Contains(id);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }
    }
}

