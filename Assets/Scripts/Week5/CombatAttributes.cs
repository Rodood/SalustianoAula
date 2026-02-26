using UnityEngine;

public class CombatAttributes : MonoBehaviour
{
    public string characterName;
    public int maxHP = 100;
    int currentHP;
    public int baseDamage;
    public bool isDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;
        isDead = false;
    }

    public void TakeDamage(int damageValue)
    {
        currentHP -= damageValue;

        Debug.Log($"{characterName} took {damageValue} damage");
        Debug.Log($"Current HP: {currentHP}/{maxHP}");

        if (currentHP <= 0)
        {
            currentHP = 0;
            isDead = true;
            gameObject.SetActive(false);
        }
    }
}
