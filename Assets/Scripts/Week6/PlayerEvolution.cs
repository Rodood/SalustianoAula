using UnityEngine;

public class PlayerEvolution : MonoBehaviour
{
    [Header("Progression")]
    public int curXP;

    public int[] nextLevelXP = new int[] { 100, 250, 5000 };
    private CombatAttributes attributes;

    private void Start()
    {
        attributes = GetComponent<CombatAttributes>();

        if(GlobalData.playerLevel > 1 || GlobalData.playerXP > 0)
        {
            attributes.level = GlobalData.playerLevel;
            curXP = GlobalData.playerXP;

            attributes.CalculateStatus();
        }
    }

    public void GainXP(int amount)
    {
        curXP += amount;

        if(attributes.level < nextLevelXP.Length + 1)
        {
            int xpGoal = nextLevelXP[attributes.level - 1];

            if(xpGoal > 0 && curXP >= xpGoal)
            {
                LevelUp(xpGoal);
            }
        }
    }

    private void LevelUp(int xpSpent)
    {
        attributes.level++;
        curXP -= xpSpent;

        attributes.CalculateStatus();
        attributes.currentHP = attributes.maxHP;

        attributes.UpdateBar();

        GainXP(0);
    }
}