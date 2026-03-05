using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleStarter : MonoBehaviour
{
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
