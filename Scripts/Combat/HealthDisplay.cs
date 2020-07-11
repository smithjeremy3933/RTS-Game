using RTS.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTS.Core
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] Text[] healthSlots;

        Health health;
        PlayerController playerController;

        private void Awake()
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        private void Update()
        {
            if (!playerController.IsSelectListEmpty())
            {
                foreach (GameObject unit in playerController.SelectedUnitList)
                {
                    Health unitHealth = unit.GetComponent<Health>();
                    for (int i = 0; i < playerController.SelectedUnitList.Count; i++)
                    {
                        healthSlots[i].text = unitHealth.GetPercentage() + "%";
                    }
                }
            }

            if (playerController.IsSelectListEmpty())
            {
                foreach (Text healthSlot in healthSlots)
                {
                    healthSlot.text = "0%";
                }
            }
        }
    }
}