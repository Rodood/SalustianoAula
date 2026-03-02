using System;
using UnityEngine;
using UnityEngine.UI;

public class CombatAttributes : MonoBehaviour
{
    public string characterName;
    public int maxHP = 100;
    public int currentHP;
    public int baseDamage;
    public bool isDead = false;

    public Slider healthBar;

    [Header("Progression")]
    public int level = 1;
    public int curXP;
    public int nextLevelXP = 100;

    [Header("Drops (Enemy)")]
    public int xpDrop = 20;
    public int coinDrop = 30;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            if (GlobalData.playerHealth != -1)
                currentHP = GlobalData.playerHealth;
            else
                currentHP = maxHP;

            level = GlobalData.playerLevel;
            curXP = GlobalData.playerXP;
            nextLevelXP = GlobalData.nextLevelXP;

            maxHP = 100 + ((level - 1) * 20);
            baseDamage = 10 + (level - 1) * 5;

            if (currentHP > maxHP)
                currentHP = maxHP;
        }
        else
            currentHP = maxHP;

        isDead = false;
        UpdateBar();
    }

    public void GainXP(int amount)
    {
        curXP += amount;

        if(curXP >= nextLevelXP)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        curXP -= nextLevelXP;

        nextLevelXP = Mathf.RoundToInt(nextLevelXP * 1.5f);
        maxHP += 20;
        currentHP = maxHP;
        baseDamage += 5;

        UpdateBar();
    }

    public void TakeDamage(int damageValue)
    {
        currentHP -= damageValue;

        Debug.Log($"{characterName} took {damageValue} damage");
        Debug.Log($"Current HP: {currentHP}/{maxHP}");

        UpdateBar();

        if (currentHP <= 0)
        {
            currentHP = 0;
            isDead = true;
            gameObject.SetActive(false);
        }
    }

    public void Heal(int healAmount)
    {
        currentHP += healAmount;

        Debug.Log($"{characterName} recovered {healAmount} damage");
        Debug.Log($"Current HP: {currentHP}/{maxHP}");

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        UpdateBar();
    }

    public void UpdateBar()
    {
        if(healthBar != null)
        {
            healthBar.maxValue = maxHP;
            healthBar.value = currentHP;
        }
    }
}
