using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public Transform[] enemyPoints;
    public Transform heroPoint;
    public GameObject prefabPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject hero = Instantiate(prefabPlayer, heroPoint.position, Quaternion.identity);

        if (hero.GetComponent<PlayerMovement>() != null)
            hero.GetComponent<PlayerMovement>().enabled = false;

        if (hero.GetComponent<InventoryInputs>() != null)
            hero.GetComponent<InventoryInputs>().enabled = false;

        List<GameObject> enemies = GlobalData.enemyPrefabs;

        for(int i = 0; i < enemies.Count; i++)
        {
            if (i >= enemyPoints.Length) break;

            Instantiate(enemies[i], enemyPoints[i].position, Quaternion.identity);
        }
    }
}
