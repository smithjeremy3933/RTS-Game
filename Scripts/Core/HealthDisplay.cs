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

        public Dictionary<GameObject, Text> GoTextMap = new Dictionary<GameObject, Text>();
        public Dictionary<GameObject, int> GoIndexTextMap = new Dictionary<GameObject, int>();
        
        Health health;
        PlayerController playerController;

        private void Awake()
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        private void Start()
        {
            Health.OnDamageTaken += Health_OnDamageTaken;
        }

        private void Health_OnDamageTaken(object sender, Health.OnDamageTakenEventArgs e)
        {
            if (e.gameObject.GetComponent<IUnit>().IsSelected())
            {
                Text healthText = GoTextMap[e.gameObject];
                healthText.text = e.gameObject.GetComponent<Health>().GetPercentage() + "%";
            }
        }

        public void SetInitHealthText()
        {
            if (!playerController.IsSelectListEmpty())
            {
                ClearHealthText();
                foreach (GameObject unit in playerController.SelectedUnitList)
                {
                    Health unitHealth = unit.GetComponent<Health>();
                    float currentHealth = unitHealth.GetPercentage();
                    for (int i = 0; i < playerController.SelectedUnitList.Count; i++)
                    {
                        if (healthSlots[i].text == "0%")
                        {
                            GoTextMap[unit] = healthSlots[i];
                            GoIndexTextMap[unit] = i;
                            healthSlots[i].text = currentHealth + "%";
                            break;
                        }
                    }
                }
            }
        }

        public void ClearHealthText()
        {
            GoTextMap.Clear();
            GoIndexTextMap.Clear();
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