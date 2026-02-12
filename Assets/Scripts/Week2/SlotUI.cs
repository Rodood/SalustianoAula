using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class SlotUI : MonoBehaviour
{
    [Header("Slot Info")]
    public Image iconImage;
    public TextMeshProUGUI amountText;

    [Header("Hover Info")]
    public Image descriptionArea;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI nameText;

    InventorySlots slotInfo;
    Transform newParent;
    Transform oldParent;

    public void ConfigureSlots(InventorySlots slot)
    {
        if(slot != null && slot.itemData != null)
        {
            iconImage.enabled = true;
            iconImage.sprite = slot.itemData.icon;

            slotInfo = slot;
            slotInfo.itemData = slot.itemData;

            oldParent = transform;
            newParent = transform.parent.parent;

            if (slot.quantity > 1)
                amountText.text = slot.quantity.ToString();
            else
                amountText.text = "";
        }
        else
        {
            iconImage.enabled = false;
            amountText.text = "";

            slotInfo.itemData = null;
            slotInfo = null;
        }
    }

    public void OnMouseEnter()
    {
        if (slotInfo != null && slotInfo.itemData != null)
        {
            descriptionArea.transform.SetParent(newParent, true);
            descriptionArea.gameObject.SetActive(true);
            descriptionText.text = slotInfo.itemData.description;
            nameText.text = slotInfo.itemData.name;
        }
        else
        {
            descriptionArea.transform.SetParent(oldParent, true);
            descriptionArea.gameObject.SetActive(false);
            descriptionText.text = null;
            nameText.text = null;
        }
    }

    public void OnMouseExit()
    {
        descriptionArea.transform.SetParent(oldParent, true);
        descriptionArea.gameObject.SetActive(false);
        descriptionText.text = null;
        nameText.text = null;
    }
}
