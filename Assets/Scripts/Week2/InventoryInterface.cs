using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class InventoryInterface : MonoBehaviour
{
    public InventorySystem inventorySystem;
    public Transform gridContainer;
    public GameObject slotPrefab;

    [Header("Economy")]
    public TextMeshProUGUI coinText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventorySystem.onInventoryChanged += UpdateInterface;

        UpdateInterface();
    }

    public void UpdateInterface()
    {
        if(coinText != null)
        {
            coinText.text = "Gold: " + inventorySystem.coin.ToString();
        }

        for (int i = gridContainer.childCount - 1; i >= 0; i--)
        {
            GameObject child = gridContainer.GetChild(i).gameObject;

            // O pulo do gato: Tiramos do grid imediatamente para o novo 
            // Instantiate não se confundir com ele
            child.transform.SetParent(null);

            Destroy(child);
        }

        foreach (InventorySlots slot in inventorySystem.inventory)
        {
            GameObject newSlot = Instantiate(slotPrefab, gridContainer);
            newSlot.GetComponent<SlotUI>().ConfigureSlots(slot);
        }
    }
}
