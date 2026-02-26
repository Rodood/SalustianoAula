using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleStarter : MonoBehaviour
{
    public void StartBattle(GameObject player, string id, List<GameObject> enemyGroup)
    {
        GlobalData.playerReturnPos = player.transform.position;

        GlobalData.enemyCombatID = id;
        GlobalData.enemyPrefabs = new List<GameObject>(enemyGroup);

        SceneManager.LoadScene("Combat");
    }
}
