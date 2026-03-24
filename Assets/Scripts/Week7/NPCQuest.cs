using System;
using TMPro;
using UnityEngine;
using static UnityEditor.Rendering.MaterialUpgrader;

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

    [Header("Interface de Diálogo")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    private void Start()
    {
        if(gameStartingQuest != null && GlobalData.availableQuest == null &&
            GlobalData.currentQuest == null && !GlobalData.historyFinished)
        {
            GlobalData.availableQuest = gameStartingQuest;
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
            GlobalData.availableQuest.npcGiverName == npcName)
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
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if(dialogueText == null) return;

        if (GlobalData.historyFinished)
        {
            dialogueText.text = "A paz reina na nossa floresta graças a você!";
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
                    dialogueText.text = quest.endDialogue + "\n\n(Recebeu " +
                        quest.goldRewards + " Ouro!)";
                    DeliverReward(quest);
                }
                else
                {
                    dialogueText.text = quest.middleDialogue + "\n(Progresso: " 
                        + GlobalData.currentQuestProgress + "/" + quest.objectiveAmount +
                        " " + quest.collectableObjectiveItem + ")";
                }
            }
            else
            {
                dialogueText.text = "O " + quest.npcReceiverName + 
                    " está à sua espera. Não perca tempo aqui!";
            }
            return;
        }

        if(GlobalData.availableQuest != null)
        {
            if(GlobalData.availableQuest.npcGiverName == npcName)
            {
                dialogueText.text = GlobalData.availableQuest.startDialogue;
                GlobalData.currentQuest = GlobalData.availableQuest;
                GlobalData.availableQuest = null;
                GlobalData.currentQuestProgress = 0;
            }
            else
            {
                dialogueText.text = " diz: Ouvi dizer que o " + 
                    GlobalData.availableQuest.npcReceiverName + " está à sua procura.";
            }
            return;
        }

        dialogueText.text = "Olá aventureiro! O dia está lindo hoje.";
    }

    public void CloseDialogue()
    {
        if(dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
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
        {
            playerNear = false;
            CloseDialogue();
        }
    }
}