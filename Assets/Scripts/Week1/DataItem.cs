using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "RPG/Item")]
public class DataItem : ScriptableObject
{
    [Header("Identifier")]
    public string itemName;
    public string id;

    [Header("Visual")]
    public Sprite icon;

    [TextArea]
    public string description;

    [Header("Economy & Logistics")]
    public int value;
    public bool stackable;
}
