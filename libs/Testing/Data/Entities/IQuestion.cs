namespace Testing.Data.Entities
{
    public interface IQuestion
    {
        int QuestID { get; set; }
        int TestID { get; set; }
        string QuestText { get; set; }
        int QuestType { get; set; }
        int? QuestNum { get; set; }
        byte[] QuestImgByte { get; set; }
    }
}