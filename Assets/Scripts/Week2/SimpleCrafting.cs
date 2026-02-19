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

    public void CraftArrows()
    {
        if (inventorySystem.HasItem(wood, woodCost))
        {
            inventorySystem.RemoveItem(wood, woodCost);

            inventorySystem.AddItem(arrow, amountProduced);

            Debug.Log($"Success! {amountProduced} arrows crafted!");
        }
        else
        {
            Debug.Log("You need wood!");
        }
    }
}
