using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Positions")]
    public Transform enemyPoint;
    public Transform heroPoint;

    [Header("Prefabs")]
    public GameObject prefabMole;
    public GameObject prefabTreant;
    public GameObject prefabPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject hero = Instantiate(prefabPlayer, heroPoint.position, Quaternion.identity);

        if(hero.GetComponent<PlayerMovement>() != null)
            hero.GetComponent<PlayerMovement>().enabled = false;

        string enemy = GlobalData.enemyToGenerate;
        GameObject genereatedMonster = null;

        if(enemy == "Mole")
            genereatedMonster = Instantiate(prefabMole, enemyPoint.position, Quaternion.identity);
        else if(enemy == "Treant")
            genereatedMonster = Instantiate(prefabTreant, enemyPoint.position, Quaternion.identity);

        if(genereatedMonster != null)
            genereatedMonster.GetComponent<EnemyController>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
