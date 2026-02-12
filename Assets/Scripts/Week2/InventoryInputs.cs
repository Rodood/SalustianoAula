using UnityEngine;

public class InventoryInputs : MonoBehaviour
{
    public GameObject inventory;
    public GameObject craftBtn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory.SetActive(false);
        craftBtn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            inventory.SetActive(!inventory.activeInHierarchy);
            craftBtn.SetActive(false);
        }

        if(Input.GetKeyDown("c"))
        {
            inventory.SetActive(true);
            craftBtn.SetActive(!craftBtn.activeInHierarchy);
        }
    }
}
