using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.Core
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] float expPoints = 0;

        public void AwardExp(float exp)
        {
            expPoints += exp;
        }

        public float GetPoints()
        {
            return expPoints;
        }
    }
}