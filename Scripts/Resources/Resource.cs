using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.Resources
{
    public enum ResourceType
    {
        Gold,
        Wood
    }

    public class Resource : MonoBehaviour
    {
        [SerializeField] ResourceType resourceType = ResourceType.Gold;
        int amount = 1000;
    }
}

