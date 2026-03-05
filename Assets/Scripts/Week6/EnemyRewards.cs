using UnityEngine;

public class EnemyRewards : MonoBehaviour
{
    [Header("Drops (Enemy)")]
    public int xpDrop = 20;
    public int coinDrop = 30;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CombatAttributes attributes = GetComponent<CombatAttributes>();

        if(attributes != null && attributes.level > 1)
        {
            xpDrop += Mathf.RoundToInt(xpDrop * 0.5f * (attributes.level - 1));

            coinDrop += Mathf.RoundToInt(coinDrop * 0.5f * (attributes.level - 1));
        }
    }
}
