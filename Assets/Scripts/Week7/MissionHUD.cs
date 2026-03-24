using TMPro;
using UnityEngine;

public class MissionHUD : MonoBehaviour
{
    public TextMeshProUGUI questTrackerText;

    // Update is called once per frame
    void Update()
    {
        if (questTrackerText == null) return;

        if (GlobalData.historyFinished)
        {
            questTrackerText.text = "History Finished!";
        }
        else if(GlobalData.currentQuest != null)
        {
            if(GlobalData.currentQuest.questType == QuestType.HuntCreatures ||
                GlobalData.currentQuest.questType == QuestType.CollecItems)
            {
                questTrackerText.text = "Missão Ativa: " + GlobalData.currentQuest.hudDescription +
                    " (" + GlobalData.currentQuestProgress + "/" + GlobalData.currentQuest.
                    objectiveAmount + " " + GlobalData.currentQuest.collectableObjectiveItem + ")";
            }
            else
            {
                questTrackerText.text = "Missão Ativa: " + GlobalData.currentQuest.hudDescription;
            }
        }
        else if(GlobalData.currentQuest != null)
        {
            questTrackerText.text = "Nova Missão: Procure o triângulo azul no(a) " +
                GlobalData.availableQuest.npcGiverName;
        }
        else
            questTrackerText.text = "Nenhuma missão ativa.";
    }
}
