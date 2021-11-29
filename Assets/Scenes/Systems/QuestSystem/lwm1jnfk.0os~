using Assets.UnityFoundation.Systems.QuestSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "QuestSystem/Clicks Objective")]
public class ClicksQuestObjectiveSO : QuestObjectiveSO
{
    public int clicksNumber;

    public override ObjectiveStatus Initiate()
    {
        return new ClicksObjectiveStatus(this);
    }
}