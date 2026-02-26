using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardianTrigger : MonoBehaviour
{
    [Header("Identifier")]
    public string uniqueID;

    [Header("Enemy formation")]
    public List<GameObject> enemyGroup = new List<GameObject>();

    private void Start()
    {
        if (GlobalData.defeatedEnemies.Contains(uniqueID))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BattleStarter starter = GetComponent<BattleStarter>();
            if(starter != null)
            {
                starter.StartBattle(collision.gameObject, uniqueID, enemyGroup);
            }
        }
    }
}
