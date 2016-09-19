using Testing.Data.Entities;

namespace Testing
{
    public class ProfQuest
    {
        public IQuestion Question;
        public int SelAnsID;
        public bool IsSaved;

        public QuestTypes QuestType
        {
            get
            {
                return Question.QuestType < 2 ? QuestTypes.Text : QuestTypes.Graph;
            }
        }

        public ProfQuest(IQuestion question)
        {
            Question = question;
            SelAnsID = -1;
            IsSaved = false;
        }

        public ProfQuest(IQuestion question, int selAnsID)
        {
            Question = question;
            SelAnsID = selAnsID;            
            IsSaved = true;
        }
    }
}
