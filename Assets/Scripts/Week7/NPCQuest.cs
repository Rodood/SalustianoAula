using System;
using UnityEngine;

public class NPCQuest: MonoBehaviour
{
    [Header("ID")]
    public string npcName;

    [Tooltip("Only for the first NPC of the game")]
    public Quest gameStartingQuest;

    [Header("Visuals")]
    public SpriteRenderer visualIndicator;
    bool playerNear = false;
    GameObject playerRef;

    private void Start()
    {
        if(gameStartingQuest != null && GlobalData.availableQuest == null &&
            GlobalData.currentQuest == null && !GlobalData.historyFinished)
        {
            GlobalData.currentQuest = gameStartingQuest;
        }
    }

    private void Update()
    {
        UpdateIconVisual();
        if (playerNear && Input.GetKeyDown(KeyCode.E))
            Interact();
    }

    private void UpdateIconVisual()
    {
        if (!visualIndicator) return;
        visualIndicator.gameObject.SetActive(false);

        if (GlobalData.historyFinished) return;

        if(GlobalData.currentQuest == null && GlobalData.availableQuest != null && 
            GlobalData.currentQuest.npcGiverName == npcName)
        {
            visualIndicator.color = Color.blue;
            visualIndicator.gameObject.SetActive(true);
        }
        else if(GlobalData.currentQuest != null && GlobalData.currentQuest.npcReceiverName == npcName)
        {
            visualIndicator.color = new Color(1f, 0.5f, 0f);
            visualIndicator.gameObject.SetActive(true);
        }
    }

    private void Interact()
    {
        if (GlobalData.historyFinished)
        {
            Debug.Log(npcName + " diz: A paz reina na nossa floresta graças a você!");
            return;
        }

        if(GlobalData.currentQuest != null)
        {
            Quest quest = GlobalData.currentQuest;

            if(quest.npcReceiverName == npcName)
            {
                bool finishedHunt = (quest.questType == QuestType.HuntCreatures &&
                    GlobalData.currentQuestProgress >= quest.objectiveAmount);
                bool finishedGathering = (quest.questType == QuestType.CollecItems
                    && GlobalData.currentQuestProgress >= quest.objectiveAmount);
                bool finishedDelivery = (quest.questType == QuestType.FindNPC);

                if(finishedHunt || finishedGathering || finishedDelivery)
                {
                    Debug.Log(npcName + " diz: " + quest.endDialogue + "(Received " +
                        quest.goldRewards + " Ouro!)");
                }
                else
                {
                    Debug.Log(npcName + " diz: " + quest.middleDialogue + " (Progresso: " 
                        + GlobalData.currentQuestProgress + 
                        "/" + quest.objectiveAmount + " " + quest.collectableObjectiveItem + ")");

                }
            }
            else
            {
                Debug.Log(npcName + " diz: O " + quest.npcReceiverName + 
                    " está à sua espera. Não perca tempo aqui!");
            }
            return;
        }

        if(GlobalData.availableQuest != null)
        {
            if(GlobalData.availableQuest.npcGiverName == npcName)
            {
                Debug.Log(npcName + " diz: " + GlobalData.availableQuest.startDialogue);
                GlobalData.currentQuest = GlobalData.availableQuest;
                GlobalData.availableQuest = null;
                GlobalData.currentQuestProgress = 0;
            }
            else
            {
                Debug.Log(npcName + " diz: Ouvi dizer que o " + 
                    GlobalData.availableQuest.npcReceiverName + " está à sua procura.");
            }
            return;
        }

        Debug.Log(npcName + " diz: Olá aventureiro! O dia está lindo hoje.");
    }

    void DeliverReward(Quest finishedQuest)
    {
        GlobalData.playerEcon += finishedQuest.goldRewards;

        GlobalData.currentQuest = null;
        GlobalData.availableQuest = finishedQuest.nextAvailableQuest;

        if(GlobalData.availableQuest == null)
        {
            GlobalData.historyFinished = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNear = true;
            playerRef = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerNear = false;
    }
}