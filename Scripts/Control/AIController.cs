using RTS.Combat;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RTS.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        Fighter fighter;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            List<Player> players = FindObjectsOfType<Player>().ToList();
            foreach (Player player in players)
            {
                float distToPlayer = Vector3.Distance(player.transform.position, transform.position);
                if (distToPlayer < chaseDistance)
                {
                    fighter.Attack(player.gameObject);
                }
            }
        }

    }
}