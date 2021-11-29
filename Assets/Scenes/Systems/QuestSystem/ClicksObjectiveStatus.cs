using Assets.UnityFoundation.Systems.QuestSystem;

public class ClicksObjectiveStatus : ObjectiveStatus
{
    public int value;

    public ClicksObjectiveStatus(ClicksQuestObjectiveSO objective) : base(objective)
    {
    }

    public override void UpdateObjectiveProgress(object parameters)
    {
        if(IsComplete) return;

        value += (int)parameters;

        if(value >= GetObjective<ClicksQuestObjectiveSO>().clicksNumber)
            IsComplete = true;
    }
}