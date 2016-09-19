namespace Testing.Data.Entities
{
    public interface IAnswer
    {
        int AnsID { get; set; }
        int QuestID { get; set; }
        int? AnsNum { get; set; }
        string AnsText { get; set; }
        int? NextQuestNum { get; set; }
    }
}