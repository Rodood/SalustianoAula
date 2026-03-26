using System;
using UnityEngine;
using UnityEngine.UI;

public class CombatAttributes : MonoBehaviour
{
    public string characterName;
    public int maxHP = 100;
    public int currentHP;
    public int baseDamage;
    public int baseHP = 100;
    public bool isDead = false;

    public int bonusAtk = 0;
    public int bonusDef = 0;

    public Slider healthBar;

    public int level = 1;

    public int curDamage;

    private void Awake()
    {
        CalculateStatus();
        currentHP = maxHP;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateBar();
    }

    public void CalculateStatus()
    {
        maxHP = baseHP + ((level - 1) * 20) + bonusDef;
        curDamage = baseDamage + ((level - 1) * 5) + bonusAtk;

        if(currentHP > maxHP) currentHP = maxHP;
    }

    public void TakeDamage(int damageValue)
    {
        currentHP -= damageValue;

        if (currentHP <= 0)
        {
            currentHP = 0;
            isDead = true;
            gameObject.SetActive(false);
        }

        UpdateBar();
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
