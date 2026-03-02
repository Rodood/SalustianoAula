using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleState
{
    Prepare,
    PlayerTurn,
    EnemyTurn,
    Victory,
    Defeat
}

public class TurnSystem : MonoBehaviour
{
    public BattleState currentState;
    public Slider heroSlider;

    CombatAttributes hero;
    List<CombatAttributes> aliveEnemies = new List<CombatAttributes>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = BattleState.Prepare;
        StartCoroutine(ConfigureBattle());
    }

    IEnumerator ConfigureBattle()
    {
        Debug.Log("Preparing Battle...");
        yield return new WaitForSeconds(1f);

        hero = GameObject.FindGameObjectWithTag("Player").
            GetComponent<CombatAttributes>();

        hero.healthBar = heroSlider;
        hero.UpdateBar();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in enemies)
        {
            aliveEnemies.Add(enemy.GetComponent<CombatAttributes>());
        }

        currentState = BattleState.PlayerTurn;
        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        Debug.Log("Player turn!");
    }

    public void AttackBtn()
    {
        if (currentState != BattleState.PlayerTurn) return;

        CombatAttributes target = aliveEnemies[0];

        target.TakeDamage(hero.baseDamage);

        if(target.currentHP <= 0)
        {
            hero.GainXP(target.xpDrop);

            GlobalData.playerEcon += target.coinDrop;

            aliveEnemies.RemoveAt(0);
        }

        VerifyEndPlayerTurn();
    }

    public void PotionBtn()
    {
        if (currentState != BattleState.PlayerTurn) return;

        hero.Heal(30);

        VerifyEndPlayerTurn();
    }

    private void VerifyEndPlayerTurn()
    {
        if (aliveEnemies.Count == 0)
        {
            currentState = BattleState.Victory;

            GlobalData.playerHealth = hero.currentHP;
            GlobalData.playerLevel = hero.level;
            GlobalData.playerXP = hero.curXP;
            GlobalData.nextLevelXP = hero.nextLevelXP;

            StartCoroutine(EndBattle(true));
        }
        else
        {
            currentState = BattleState.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        Debug.Log("Enemies are thinking...");
        yield return new WaitForSeconds(2f);

        Debug.Log("Enemy Attacked Hero!");
        foreach(CombatAttributes enemy in aliveEnemies)
        {
            yield return new WaitForSeconds(1f);
            hero.TakeDamage(enemy.baseDamage);
            if (hero.currentHP <= 0) break;
        }

        if(hero.isDead)
        {
            currentState = BattleState.Defeat;
            StartCoroutine(EndBattle(false));
        }
        else
        {
            currentState = BattleState.PlayerTurn;
            StartPlayerTurn();
        }
    }

    IEnumerator EndBattle(bool playerWon)
    {
        yield return new WaitForSeconds(2f);

        if (playerWon)
        {
            Debug.Log("VICTORY!");

            GlobalData.defeatedEnemies.Add(GlobalData.enemyCombatID);

            yield return new WaitForSeconds(2f);

            SceneManager.LoadScene("Exploration");
        }
        else
        {
            Debug.Log("DEFEAT... Game Over.");
            SceneManager.LoadScene("DefeatMenu");
        }
    }
}
