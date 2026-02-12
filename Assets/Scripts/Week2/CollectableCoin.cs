using UnityEngine;

public class CollectableCoin : MonoBehaviour
{
    public int value = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InventorySystem inventorySystem = FindFirstObjectByType<InventorySystem>();

            if (inventorySystem != null)
            {
                inventorySystem.ModifyCoin(value);
                Destroy(gameObject);
            }
        }
    }
}
