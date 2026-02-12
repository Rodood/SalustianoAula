using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI amountText;

    public void ConfigureSlots(InventorySlots slot)
    {
        if(slot != null && slot.itemData != null)
        {
            iconImage.enabled = true;
            iconImage.sprite = slot.itemData.icon;

            if (slot.quantity > 1)
                amountText.text = slot.quantity.ToString();
            else
                amountText.text = "";
        }
        else
        {
            iconImage.enabled = false;
            amountText.text = "";
        }
    }
}
