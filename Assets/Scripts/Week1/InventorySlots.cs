using UnityEngine;

public class InventorySlots
{
    public DataItem itemData;
    public int quantity;

    public InventorySlots(DataItem item, int qtd)
    {
        itemData = item; 
        quantity = qtd;
    }

    public void AddQuantity(int qtd)
    {
        Debug.Log("Added Item " + qtd);
        quantity += qtd;
    }

    public void RemoveQuantity(int qtd)
    {
        Debug.Log("Subtracted Item " + qtd);
        quantity -= qtd;
    }
}
