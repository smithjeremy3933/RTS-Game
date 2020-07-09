using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RTS.Control;

namespace RTS.Movement
{
    public class Mover : MonoBehaviour
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
            if (Input.GetMouseButtonDown(1))
            {
                bool isSelected = selectedTable.Contains(id);
                Debug.Log(isSelected + " " + id);
                if (isSelected)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    bool hasHit = Physics.Raycast(ray, out hit);
                    if (hasHit)
                    {
                        MoveTo(hit.point, 10f);
                    }
                }
                else
                {
                    Debug.Log("Not selected");
                }
            }
        }

        void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }
    }
}

