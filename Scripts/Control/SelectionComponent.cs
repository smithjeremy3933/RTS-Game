using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.Control
{
    public class SelectionComponent : MonoBehaviour
    {
        void Start()
        {
            GetComponentInChildren<Renderer>().material.color = Color.green;
        }

        private void OnDestroy()
        {
            GetComponentInChildren<Renderer>().material.color = Color.cyan;
        }
    }
}