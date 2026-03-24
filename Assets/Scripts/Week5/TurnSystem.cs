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

    public DataItem lifePotion;
    public Button btnPotion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = BattleState.Prepare;
        StartCoroutine(ConfigureBattle());
    }

    IEnumerator ConfigureBattle()
    {
        hero = GameObject.FindGameObjectWithTag("Player").
            GetComponent<CombatAttributes>();

        hero.healthBar = heroSlider;
        hero.UpdateBar();

        if(!GetComponent<InventorySystem>().HasItem(lifePotion, 1))
        {
            btnPotion.interactable = false;
        }

        Debug.Log("Preparing Battle...");
        yield return new WaitForSeconds(1f);

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

        target.TakeDamage(hero.curDamage);

        if(target.currentHP <= 0)
        {
            EnemyRewards loot = target.GetComponent<EnemyRewards>();
            PlayerEvolution evolution = hero.GetComponent<PlayerEvolution>();

            if (loot != null && evolution != null)
            {
                evolution.GainXP(loot.xpDrop);
                GlobalData.playerEcon += loot.coinDrop;

                if (GlobalData.currentQuest != null)
                {
                    if (GlobalData.currentQuest.questType == QuestType.HuntCreatures ||
                        GlobalData.currentQuest.questType == QuestType.CollecItems)
                    {
                        GlobalData.currentQuestProgress++;
                        Debug.Log("Quest Atualizada no Console: " +
                            GlobalData.currentQuestProgress + "/" +
                            GlobalData.currentQuest.objectiveAmount);
                    }
                }
            }

            aliveEnemies.RemoveAt(0);
        }

        VerifyEndPlayerTurn();
    }

    public void PotionBtn()
    {
        if (currentState != BattleState.PlayerTurn) return;

        bool consumiuApenasUma = false;

        // 1. Procura a poção no Inventário Global
        foreach (InventorySlots slot in GlobalData.inventarioAtual)
        {
            if (slot.itemData == lifePotion && slot.quantity > 0)
            {
                slot.quantity--; // Gasta 1
                consumiuApenasUma = true;

                // Limpa da lista se a quantidade chegar a zero
                if (slot.quantity <= 0)
                {
                    GlobalData.inventarioAtual.Remove(slot);
                    //Desabilita Botao
                    btnPotion.interactable = false;
                }

                break; // Para o loop para não gastar 2 poções de uma vez!
            }
        }

        // 2. Aplica a cura se o jogador tinha a poção!
        if (consumiuApenasUma)
        {
            hero.Heal(50);     // Cura
            Debug.Log("Você bebeu a poção deliciosa!");
            VerifyEndPlayerTurn(); // Passa o turno
        }
        else
        {
            Debug.LogWarning("Inventário vazio! Você não tem mais Poções de Vida!");
        }

    }

    private void VerifyEndPlayerTurn()
    {
        if (aliveEnemies.Count == 0)
        {
            currentState = BattleState.Victory;

            PlayerEvolution evo = hero.GetComponent<PlayerEvolution>();

            GlobalData.playerHealth = hero.currentHP;
            GlobalData.playerLevel = hero.level;
            GlobalData.playerXP = evo.curXP;

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
