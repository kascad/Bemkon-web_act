using System.Collections.Generic;
using System.Linq;
using Testing.Data;
using Testing.Data.Entities;

namespace Testing
{
    public class ProfessorTest
    {
        public ProfessorTest()
        {
        }
        
        private readonly ITest _test;
        public ITest Test
        {
            get { return _test; }
        }

        private readonly List<ProfQuest> _profQuestsList;

        #region Position
        public int Position;
        public bool IsFirsQtuest
        {
            get
            {
                return Position == 0;
            }
        }

        public bool IsFinishedQuest
        {
            get
            {
                using (var dbRepository = DbRepositoryFactory.GetRepository())
                {
                    return Position == dbRepository.GetQuestionsCount(Test.TestID) - 1;                    
                }
            }
        }

        private bool IsEndQuest
        {
            get
            {
                return Position >= _profQuestsList.Count - 1;
            }
        }
        #endregion

        private readonly Archive.ExamineeTest _examineeTest;

        public bool HorisontalAnswers { get; private set; }

        public ProfessorTest(int testID, int examineeID, Archive.Archive archive) : this()
        {
            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {
                _test = dbRepository.GetTest(testID);

                HorisontalAnswers = _test.HorisontalAnswers;

                var examinee = archive.getExaminee(examineeID);
                _examineeTest = examinee.GetTest(testID);

                _profQuestsList = new List<ProfQuest>();

                if (_examineeTest == null)
                {
                    // Create new test 
                    examinee.AddTest(testID, _test.ShortName);
                    _examineeTest = examinee.GetTest(testID);
                    //var quest = _test.Questions.First(tq => tq.QuestNum == 1);
                    var quest = dbRepository.GetQuestionByNumber(Test.TestID, 1);
                    _profQuestsList.Add(new ProfQuest(quest));
                }
                else
                {
                    // Begin from end test
                    List<Archive.TestResult> tests = _examineeTest.GetSavedTestResults();
                    if (tests.Count > 0)
                    {
                        if (_examineeTest.Name != "")
                        {

                        }
                        foreach (var etr in tests)
                        {
                            //var quest = _test.Questions.First(tq => tq.QuestID == etr.QuestID);
                            var quest = dbRepository.GetQuestion(etr.QuestID);
                            _profQuestsList.Add(new ProfQuest(quest, etr.AnsID));
                        }
                    }
                    else
                    {
                        //var quest = _test.Questions.First(tq => tq.QuestNum == 1);
                        var quest = dbRepository.GetQuestionByNumber(Test.TestID, 1);
                        _profQuestsList.Add(new ProfQuest(quest));
                    }
                }
            }

            Position = _profQuestsList.Count - 1;            
        }

        public void NextQuest(int ansID, string time, string fullTime)
        {
            if (!IsFinishedQuest)
            {
                if (IsEndQuest)
                {
                    // Add ProfQuest to profQuestsList
                    int questType = CurrentQuest.Question.QuestType;
                    using (var dbRepository = DbRepositoryFactory.GetRepository())
                    {
                        // ищем следующий вопрос теста
                        var ans = dbRepository.GetAnswer(ansID);
                        var nextQuestNum = (int)CurrentQuest.Question.QuestNum + 1;
                        if (ans.NextQuestNum.HasValue)
                        {
                            var ansNextQuestNum = (int) ans.NextQuestNum;
                            if ((questType == 0 || questType == 3) && ansNextQuestNum != 0)
                            {
                                nextQuestNum = ansNextQuestNum;
                            }
                        }

                        var nextQuest = dbRepository.GetQuestionByNumber(Test.TestID, nextQuestNum);
                        _profQuestsList.Add(new ProfQuest(nextQuest));

                        // сохраняем текущий ответ
                        if (!CurrentQuest.IsSaved)
                        {
                            SaveQuestRes(ansID, time, ans);
                        }
                    }
                }

                Position++;
            }
            else
            {
                //var ans = CurrentQuest.Question.Answers.First(an => an.AnsID == ansID);
                using (var dbRepository = DbRepositoryFactory.GetRepository())
                {
                    var ans = dbRepository.GetAnswer(ansID);
                    SaveQuestRes(ansID, time, ans);
                }

                _examineeTest.FinishTest(fullTime);
            }
        }

        public void NextQuest()
        {
            if (IsEndQuest)
            {
                Position++;
            }
        }

        private void SaveQuestRes(int ansID, string time, IAnswer ans)
        {
            // Save results
            var questNum = CurrentQuest.Question.QuestNum.HasValue ? (int)CurrentQuest.Question.QuestNum : 0;

            //var ansVar = CurrentQuest.Question.Answers.First(an => an.AnsID == ansID);
            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {
                var ansVar = dbRepository.GetAnswer(ansID);
                var ansNumber = ansVar.AnsNum.HasValue ? (int) ansVar.AnsNum : 0;

                _examineeTest.AddTestResult(CurrentQuest.Question.QuestID, ansID,
                                            CurrentQuest.Question.QuestText, ans.AnsText, time, questNum, ansNumber);
            }
            // Save answer
            CurrentQuest.SelAnsID = ansID;
            CurrentQuest.IsSaved = true;
        }    

        public void PrevQuest()
        {
            if (!IsFirsQtuest)
            {
                Position--;
            }
        }

        public void SetQuestByID(int questID)
        {
            if (_profQuestsList.Any(pq => pq.Question.QuestID == questID))
                Position = _profQuestsList.IndexOf(_profQuestsList.First(pq => pq.Question.QuestID == questID));
            else
            {
                ;
            }
        }

        public void FinishTest(string fullTime)
        {
            _examineeTest.FinishTest(fullTime);
        }

        public ProfQuest CurrentQuest
        {
            get
            {
                return _profQuestsList[Position];
            }
        }

        public int SavedQuestCount
        {
            get
            {
                return _profQuestsList.Count;
            }
        }

        public List<Archive.TestResult> SavedTests
        {
            get
            {
                return _examineeTest.GetSavedTestResults();
            }
        }
    }
}
