using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySystem : MonoBehaviour
{
    [Header("Economy")]
    public int coin = 0;

    public List<InventorySlots> inventory = new List<InventorySlots>();

    public event Action onInventoryChanged;

    private void Awake()
    {
        if(GlobalData.inventarioAtual.Count == 0 && inventory.Count > 0)
        {
            GlobalData.inventarioAtual = new List<InventorySlots>(inventory);
            coin = GlobalData.playerEcon;
        }

        inventory = GlobalData.inventarioAtual;

        coin = GlobalData.playerEcon;
    }

    public void AddItem(DataItem item, int qtd)
    {
        if (item.stackable)
        {
            for(int i = 0; i < inventory.Count; i++)
            {
                if(inventory[i].itemData == item)
                {
                    inventory[i].AddQuantity(qtd);
                    Debug.Log($"Added + {qtd}");

                    if (onInventoryChanged != null)
                        onInventoryChanged.Invoke();

                    return;
                }
            }
        }

        InventorySlots newSlot = new InventorySlots(item, qtd);

        inventory.Add(newSlot);

        if (onInventoryChanged != null)
            onInventoryChanged.Invoke();

        Debug.Log($"Added + {item} Quantity: {qtd}");
    }

    public void RemoveItem(DataItem item, int qtd)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemData == item)
            {
                inventory[i].RemoveQuantity(qtd);
                Debug.Log($"Removed + {qtd}");

                if (inventory[i].quantity <= 0)
                {
                    inventory.RemoveAt(i);
                }

                if (onInventoryChanged != null)
                {
                    onInventoryChanged.Invoke();
                }

                return;
            }
        }
    }

    public void ModifyCoin(int value)
    {
        coin += value;

        if(coin < 0) 
            coin = 0;

        if (onInventoryChanged != null) 
            onInventoryChanged.Invoke();
    }

    public bool HasItem(DataItem item, int qtd)
    {
        foreach(InventorySlots slot in inventory)
        {
            if(slot.itemData == item && slot.quantity >= qtd)
                return true;
        }

        return false;
    }

    // --- MÁGICA PARA O EDITOR ---
    // Esta função é chamada automaticamente pela Unity quando você altera
    // um valor no Inspector. Assim, podemos ver a UI mudar em tempo real!
    private void OnValidate()
    {
        // Só executa se o jogo estiver rodando para evitar erros
        if (Application.isPlaying && onInventoryChanged != null)
        {
            onInventoryChanged.Invoke();
        }
    }
}
