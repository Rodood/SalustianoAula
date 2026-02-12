using UnityEngine;

public class Collectable : MonoBehaviour
{
    public DataItem itemToGive;
    public int quantity = 1;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if(itemToGive  != null)
        {
            sr.sprite = itemToGive.icon;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InventorySystem inventorySystem = FindFirstObjectByType<InventorySystem>();

            if (inventorySystem != null)
            {
                inventorySystem.AddItem(itemToGive, quantity);
                Destroy(gameObject);
            }
        }
    }
}
