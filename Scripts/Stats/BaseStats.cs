using RTS.Core;
using UnityEngine;

namespace RTS.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        private void Update()
        {

        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, CalculateLevel());
        }

        public int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();

            if (experience == null) return startingLevel;

            float currentExp = experience.GetPoints();
            int maxLevel = progression.GetLevels(Stat.ExpToLevelUp, characterClass);
            for (int level = 0; level <= maxLevel; level++)
            {
                float ExpToLevelUp = progression.GetStat(Stat.ExpToLevelUp, characterClass, level);
                if (ExpToLevelUp > currentExp)
                {
                    return level;
                }
            }
            return maxLevel + 1;
        }
    }
}