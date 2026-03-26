using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleStarter : MonoBehaviour
{
    public static void SalvarDadosJogador(GameObject player)
    {
        if (player == null) return;
        CombatAttributes atributos = player.GetComponent<CombatAttributes>();
        PlayerEvolution progresso = player.GetComponent<PlayerEvolution>();
        InventorySystem inventario = player.GetComponent<InventorySystem>();

        if (atributos != null)
        {
            GlobalData.playerHealth = atributos.currentHP;
            GlobalData.playerLevel = atributos.level;

            // Salvando a nossa nova Fonte de Verdade
            GlobalData.attackBonus = atributos.bonusAtk;
            GlobalData.defenseBonus = atributos.bonusDef;
        }
        if (inventario != null) GlobalData.playerEcon = inventario.coin;
        if (progresso != null) GlobalData.playerXP = progresso.curXP;

    }

    public static void CarregarDadosJogador(GameObject player)
    {
        // Jogo Novo, ignora!
        if (player == null || GlobalData.playerHealth == -1) return;

        CombatAttributes atributos = player.GetComponent<CombatAttributes>();
        PlayerEvolution progresso = player.GetComponent<PlayerEvolution>();

        if (atributos != null)
        {
            atributos.level = GlobalData.playerLevel;
            atributos.bonusAtk = GlobalData.attackBonus;
            atributos.bonusDef = GlobalData.defenseBonus;

            // Avisa o Herói para recalcular a matemática usando os dados recém-carregados!
            atributos.CalculateStatus();

            atributos.currentHP = GlobalData.playerHealth; // Devolve as feridas da batalha
            atributos.UpdateBar();
        }
        if (progresso != null) progresso.curXP = GlobalData.playerXP;
    }

    public void StartBattle(GameObject player, string id, List<GameObject> enemyGroup,
        List<int> levels)
    {
        GlobalData.playerReturnPos = player.transform.position;

        GlobalData.enemyCombatID = id;
        GlobalData.enemyPrefabs = new List<GameObject>(enemyGroup);
        GlobalData.niveisInimigosParaArena = new List<int>(levels);

        SceneManager.LoadScene("Combat");
    }
}
