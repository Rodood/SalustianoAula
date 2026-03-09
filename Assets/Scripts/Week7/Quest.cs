using UnityEngine;

public enum QuestType
{
    FindNPC,
    HuntCreatures,
    CollecItems,
}

[CreateAssetMenu(fileName = "New Quest", menuName ="RPG/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public QuestType questType;

    [Header("Quest actors")]
    [Tooltip("NPC name that gives the quest")]
    public string npcGiverName;
    [Tooltip("NPC name that concludes the quest")]
    public string npcReceiverName;

    [Header("Quest Texts")]
    [TextArea] public string startDialogue;
    [TextArea] public string middleDialogue;
    [TextArea] public string endDialogue;
    [TextArea] public string hudDescription;

    [Header("Objectives")]
    public int objectiveAmount;
    public string collectableObjectiveItem;

    [Header("Rewards")]
    public int goldRewards;
    public int xpRewards;

    [Header("Questline")]
    public Quest nextAvailableQuest;
}
