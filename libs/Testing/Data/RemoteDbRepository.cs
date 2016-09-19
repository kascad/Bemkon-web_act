using System.Collections.Generic;
using System.Linq;
using Shared;
using Testing.Data.Entities;

namespace Testing.Data
{
    public class RemoteDbRepository : IDbRepository
    {
        private readonly TestingDataContext _dataContext;
        // кэш для ускорения
        private static List<IScale> _tableScales;
        private static List<IScaleWeight> _tableScaleWeights;
        private static List<IAnswer> _tableAnswers;

        private static List<IConseq> _internalTableConseqs;
        private List<IConseq> _tableConseqs
        {
            get
            {
                if (_internalTableConseqs == null)
                {
                    var conseqs = _dataContext.Conseqs.ToList();
                    _internalTableConseqs = conseqs.Select(t => (IConseq)t).ToList();
                }
                return _internalTableConseqs;
            }
        }

        private static List<IQuestion> _internaleTableQuestions;
        private List<IQuestion> _tableQuestions
        {
            get
            {
                if (_internaleTableQuestions == null)
                {
                    var questions = _dataContext.Questions.ToList();
                    _internaleTableQuestions = questions.Select(t => (IQuestion)t).ToList();
                }
                return _internaleTableQuestions;
            }
        }
        private static Dictionary<int, int> _tableQuestionCount;
        private static List<ITest> _tableTests;

        public RemoteDbRepository()
        {
            _dataContext = new TestingDataContext(GlobalOptions.RemoteConnectionString);

            // заполняем кэш
            if (_tableScales == null)
            {
                var scales = _dataContext.Scales.ToList();
                _tableScales = scales.Select(t => (IScale) t).ToList();
            }

            if (_tableScaleWeights == null)
            {
                var scaleWeights = _dataContext.ScaleWeights.ToList();
                _tableScaleWeights = scaleWeights.Select(t => (IScaleWeight) t).ToList();
            }

            if (_tableAnswers == null)
            {
                var answers = _dataContext.Answers.ToList();
                _tableAnswers = answers.Select(t => (IAnswer)t).ToList();
            }

            //if (_tableConseqs == null)
            {
                //var count = _dataContext.Conseqs.Count();
                //var conseqs = _dataContext.Conseqs.ToList();
                //_tableConseqs = conseqs.Select(t => (IConseq)t).ToList();
            }

            //if (_tableQuestions == null)
            {
                //var count = _dataContext.Questions.Count();
                //var questions = _dataContext.Questions.ToList();
                //_tableQuestions = questions.Select(t => (IQuestion)t).ToList();
            }

            if (_tableQuestionCount == null)
                _tableQuestionCount = new Dictionary<int, int>();

            if (_tableTests == null)
            {
                var tests = _dataContext.Tests.ToList();
                _tableTests = tests.Select(t => (ITest)t).ToList();
            }

        }
        
        public ITest GetTest(int testId)
        {
            return _tableTests.FirstOrDefault(t => t.TestID == testId);
        }

        public IQuestion GetQuestionByNumber(int testId, int questNum)
        {
            return _tableQuestions.FirstOrDefault(tq => tq.TestID == testId && tq.QuestNum == questNum);
        }

        public IQuestion GetQuestion(int questId)
        {
            return _tableQuestions.First(tq => tq.QuestID == questId);
        }

        public int GetQuestionsCount(int testId)
        {
            int questionCount;
            if (_tableQuestionCount.ContainsKey(testId))
            {
                _tableQuestionCount.TryGetValue(testId, out questionCount);
            }
            else
            {
                questionCount = _tableQuestions.Count(t => t.TestID == testId);
                _tableQuestionCount.Add(testId, questionCount);
            }
            return questionCount;
        }

        public IAnswer GetAnswer(int ansId)
        {
            return _tableAnswers.FirstOrDefault(a => a.AnsID == ansId);
        }

        public List<IAnswer> GetQuestionAnswers(int questId)
        {
            return (from ans in _tableAnswers
                    where ans.QuestID == questId
                    orderby ans.AnsNum
                    select (IAnswer)ans).ToList();

            //return (from ans in _dataContext.Answers
            //        where ans.QuestID == questId
            //        orderby ans.AnsNum
            //        select (IAnswer)ans).ToList();
        }

        public List<IBatteriesView> GetBatteriesViews()
        {
            return (from bw in _dataContext.BatteriesViews
                    select (IBatteriesView)bw).ToList();
        }

        public List<IBatteryTest> GetBatteryTestsByBatteryID(int batteryId)
        {
            return (from bt in _dataContext.BatteryTests
                    where bt.BatteryID == batteryId
                    orderby bt.Number
                    select (IBatteryTest)bt).ToList();
        }

        public IBattery GetBattery(int BatteryId)
        {
            return _dataContext.Batteries.FirstOrDefault(t => t.BatteryID == BatteryId);
        }

        public List<IBattery> GetBatteries()
        {
            return _dataContext.Batteries.Select(t => (IBattery)t).ToList();
        }

        public List<ICategory> GetCategories()
        {
            var result = _dataContext.Categories.ToList();
            return result.Select(t => (ICategory)t).ToList();
        }

        public List<ITest> GetTestsByCategory(int categoryId)
        {
            var result = _tableTests.Where(t => t.CategoryID == categoryId).ToList();
            return result.Select(t => (ITest)t).ToList();
        }

        public List<ITest> GetTestsFor10Top()
        {
            var result = _tableTests.Where(t1 => t1.TestingCount > 0).OrderByDescending(t2 => t2.TestingCount).ToList();
            return result.Select(t => (ITest)t).ToList();
        }

        public void IncreateTestingCount(int testId)
        {
            var test = _dataContext.Tests.First(t => t.TestID == testId);
            if (test != null)
            {
                test.TestingCount++;
                _dataContext.SubmitChanges();
            }
        }

        public List<IScale> GetScales(List<int> scaleIds)
        {
            var result = (from s in _tableScales
                          where scaleIds.Contains(s.ScaleID)
                          select s).ToList();
            return result.Select(t => (IScale)t).ToList();

            //var result = (from s in _dataContext.Scales
            //              where scaleIds.Contains(s.ScaleID)
            //              select s).ToList();
            //return result.Select(t => (IScale)t).ToList();
        }

        public List<IScale> GetScalesForTest(int testId)
        {
            var result = (from p in _tableScales
                          where p.TestID == testId
                          orderby p.ScaleID
                          select p).ToList();
            return result.Select(t => (IScale)t).ToList();
            //var result = (from p in _dataContext.Scales
            //              where p.TestID == testId
            //              orderby p.ScaleID
            //              select p).ToList();
            //return result.Select(t => (IScale)t).ToList();
        }

        public List<IScaleWeight> GetScaleWeightForScale(int scaleId)
        {
            var result = (from p in _tableScaleWeights
                          where p.ScaleID == scaleId
                          select p).ToList();
            return result.Select(t => (IScaleWeight)t).ToList();
        }

        public IScale GetScale(int scaleId)
        {
            return _tableScales.FirstOrDefault(s => s.ScaleID == scaleId);
        }

        public List<IInterpretRule> GetInterpretRules(int interpretId)
        {
            var result = (from p in _dataContext.InterpretRules
                          where p.InterpretID == interpretId
                          select p).ToList();
            return result.Select(t => (IInterpretRule)t).ToList();
        }

        public ITest GetTest(string shortName)
        {
            return _tableTests.FirstOrDefault(t => t.ShortName == shortName);
        }

        public IScale GetScale(string shortName)
        {
            return _tableScales.FirstOrDefault(s => s.ScaleShortName == shortName);
        }

        public List<IConseq> GetConseqsByRule(int ruleId)
        {
            var result = (from f in _tableConseqs
                          where f.RuleID == ruleId
                          select f).ToList();
            return result.Select(t => (IConseq)t).ToList();
        }

        public List<IScaleWeight> GetScaleWeightForAnswers(List<int> answerIds)
        {
            var result = (from p in _tableScaleWeights
                          where answerIds.Contains(p.AnsID.HasValue ? p.AnsID.Value : 0)
                          select p).ToList();
            return result.Select(t => (IScaleWeight)t).ToList();
        }

        public double GetScaleWeightForScaleAndAnswer(int scaleId, int ansId)
        {
            IScaleWeight scaleWeight = _tableScaleWeights.FirstOrDefault(p => p.AnsID == ansId && p.ScaleID == scaleId);
            if (scaleWeight == null || !scaleWeight.Weight.HasValue)
                return 0.0;
            return scaleWeight.Weight.Value;
        }

        public List<IScaleWeight> GetScaleWeightForScaleAndAnswer(List<int> scaleIds, List<int> ansIds)
        {
            var result = (from p in _tableScaleWeights
                          where ansIds.Contains(p.AnsID.HasValue ? p.AnsID.Value : -1) && scaleIds.Contains(p.ScaleID.HasValue ? p.ScaleID.Value : -1)
                          select p).ToList();
            return result.Select(t => (IScaleWeight)t).ToList();
        }

        public List<IScale> GetScalesAll()
        {
            return _tableScales;
        }

		public List<IInterpret> GetInterprets()
		{
			var result = _dataContext.Interprets.ToList();
			return result.Select(t => (IInterpret)t).ToList();
		}

        public void Dispose()
        {
            if (_dataContext != null)
                _dataContext.Dispose();
        }
    }
}
