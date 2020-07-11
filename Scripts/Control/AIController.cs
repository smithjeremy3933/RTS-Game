using RTS.Combat;
using RTS.Core;
using RTS.Movement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RTS.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;

        Fighter fighter;
        Health health;
        Mover mover;
        List<Player> players = new List<Player>();
        Player closestPlayer = null;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;
        Vector3 guardPos;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            players = FindObjectsOfType<Player>().ToList();
            guardPos = transform.position;
        }

        private void Update()
        {
            players = FindObjectsOfType<Player>().ToList();

            if (InAttackRangeOfPlayer() && fighter.CanAttack(closestPlayer.gameObject))
            {
                AttackBehavior();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehavior();
            }
            else
            {
                PatrolBehavior();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private bool InAttackRangeOfPlayer()
        {
            float distToPlayer = Mathf.Infinity;
            foreach (Player player in players)
            {
                float newDistToPlayer = Vector3.Distance(player.transform.position, transform.position);
                if (newDistToPlayer < chaseDistance)
                {
                    closestPlayer = player;
                    distToPlayer = newDistToPlayer;
                }
            }
            return distToPlayer < chaseDistance;
        }

        private void AttackBehavior()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(closestPlayer.gameObject);
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void PatrolBehavior()
        {
            Vector3 nextPos = guardPos;
            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPos = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPos);
            }

            closestPlayer = null;
        }

        private bool AtWayPoint()
        {
            float distToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}