using Unity.VisualScripting;
using UnityEngine;

public class SimpleCrafting : MonoBehaviour
{
    public InventorySystem inventorySystem;

    [Header("Arrow Recipe")]
    public DataItem wood;
    public DataItem arrow;

    public int woodCost = 1;
    public int amountProduced = 3;

    public InventoryInterface Interface;

    public void CraftArrows()
    {
        if (inventorySystem.HasItem(wood, woodCost))
        {
            inventorySystem.RemoveItem(wood, woodCost);

            int i = 0;
            while(i <10)
                i++;

            inventorySystem.AddItem(arrow, amountProduced);

            Debug.Log($"Success! {amountProduced} arrows crafted!");
        }
        else
        {
            Debug.Log("You need wood!");
        }
    }
}
