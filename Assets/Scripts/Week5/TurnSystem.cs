using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    CombatAttributes hero;
    CombatAttributes enemy;

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

        enemy = GameObject.FindGameObjectWithTag("Enemy").
            GetComponent<CombatAttributes>();

        currentState = BattleState.PlayerTurn;
        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        Debug.Log("Your Turn! Press space to attack!");
    }

    private void Update()
    {
        switch (currentState)
        {
            case BattleState.PlayerTurn:
            
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("Player attacked monster!");
                    enemy.TakeDamage(hero.baseDamage);

                    VerifyEndPlayerTurn();
                }

                break;
        }
    }

    private void VerifyEndPlayerTurn()
    {
        if (enemy.isDead)
        {
            currentState = BattleState.Victory;
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
        hero.TakeDamage(enemy.baseDamage);

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
