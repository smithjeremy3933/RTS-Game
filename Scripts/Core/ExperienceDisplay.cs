using RTS.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTS.Core
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] Text[] expTextSlots;

        Dictionary<GameObject, Text> goExpTextMap = new Dictionary<GameObject, Text>();
        Experience experience;
        PlayerController playerController;
        HealthDisplay healthDisplay;

        private void Awake()
        {
            playerController = FindObjectOfType<PlayerController>();
            healthDisplay = FindObjectOfType<HealthDisplay>();
        }

        private void Start()
        {
            Health.OnExpAwarded += Health_OnExpAwarded;
        }

        private void Health_OnExpAwarded(object sender, Health.OnExpAwardedEventArgs e)
        {
            if (e.instigator.GetComponent<IUnit>().IsSelected())
            {
                Text expText = goExpTextMap[e.instigator];
                expText.text = e.instigator.GetComponent<Experience>().GetPoints().ToString();
            }
        }

        public void SetInitExpText()
        {
            if (!playerController.IsSelectListEmpty())
            {
                ClearExpText();
                foreach (GameObject unit in playerController.SelectedUnitList)
                {
                    Experience unitExp = unit.GetComponent<Experience>();
                    float currentExp = unitExp.GetPoints();
                    for (int i = 0; i < healthDisplay.transform.childCount; i++)
                    {
                        if (healthDisplay.GoIndexTextMap[unit] == i)
                        {
                            goExpTextMap[unit] = expTextSlots[i];
                            expTextSlots[i].text = currentExp.ToString();
                            break;
                        }
                    }
                }
            }
        }

        public void ClearExpText()
        {
            goExpTextMap.Clear();
            if (playerController.IsSelectListEmpty())
            {
                foreach (Text expSlots in expTextSlots)
                {
                    expSlots.text = "0";
                }
            }
        }
    }
}