using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    public InventorySystem inventory;

    public DataItem sword;
    public DataItem potion;

    private void Start()
    {
        inventory.AddItem(sword, 1);
        inventory.AddItem(potion, 3);
        inventory.AddItem(potion, 5);
    }
}
