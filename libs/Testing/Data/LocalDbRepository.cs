using System.Collections.Generic;
using Testing.Data.Entities;
using System.Linq;

namespace Testing.Data
{
    public class LocalDbRepository : IDbRepository
    {
        private const string ConStrPatt =
            "metadata=res://*/Data.LocalDbModel.csdl|res://*/Data.LocalDbModel.ssdl|res://*/Data.LocalDbModel.msl;provider=System.Data.SqlServerCe.3.5;provider connection string=\"Data Source={0}\\professor_testing.sdf;Max Database Size=2047\"";

        private static readonly string ConnectionString = string.Format(ConStrPatt, Shared.GlobalOptions.ProgrammDataFolder);
        private static readonly professor_testingEntities DataContext = new professor_testingEntities(ConnectionString);

        public ITest GetTest(int testId)
        {
            return (ITest)DataContext.Tests.FirstOrDefault(t => t.TestID == testId);
        }

        public ITest GetTest(string shortName)
        {
            return (ITest)DataContext.Tests.FirstOrDefault(t => t.ShortName == shortName);
        }

        public IQuestion GetQuestionByNumber(int testId, int questNum)
        {
            return (IQuestion)DataContext.Questions.FirstOrDefault(tq => tq.TestID == testId && tq.QuestNum == questNum);
        }

        public IQuestion GetQuestion(int questId)
        {
            return (IQuestion)DataContext.Questions.First(tq => tq.QuestID == questId);
        }

        public int GetQuestionsCount(int testId)
        {
            return DataContext.Questions.Count(t => t.TestID == testId);
        }

        public IAnswer GetAnswer(int ansId)
        {
            return (IAnswer)DataContext.Answers.FirstOrDefault(a => a.AnsID == ansId);
        }

        private static List<IAnswer> _answers;
        private List<IAnswer> Answers
        {
            get
            {
                if (_answers == null)
                {
                    var answers = (from ans in DataContext.Answers
                                   select ans).ToList();
                    _answers = answers.Select(t => (IAnswer)t).ToList();
                }
                return _answers;
            }
        }

        public List<IAnswer> GetQuestionAnswers(int questId)
        {
            return (from ans in Answers
                    where ans.QuestID == questId
                    orderby ans.AnsNum
                    select ans).ToList();
        }

        public List<IBatteriesView> GetBatteriesViews()
        {
            var bateries = DataContext.Batteries.ToList();
            var bateryTests = DataContext.BatteryTests.ToList();
            var tests = DataContext.Tests.ToList();

            var result = new List<IBatteriesView>();
            foreach (var b in bateries)
            {
                var BatteryTests = "";
                var testBateryTests = bateryTests.Where(t => t.BatteryID == b.BatteryID);
                foreach (var t in testBateryTests)
                {
                    var test = tests.FirstOrDefault(ts => ts.TestID == t.TestID);
                    if (test != null)
                    {
                        if (BatteryTests != "")
                            BatteryTests += ",";
                        BatteryTests += test.ShortName;
                    }
                }

                result.Add(new BatteriesView
                {
                    BatteryID = b.BatteryID,
                    BatteryName = b.BatteryName,
                    Description = b.Description,
                    BatteryTests = BatteryTests
                });
            }

            return result;
        }

        public List<IBatteryTest> GetBatteryTestsByBatteryID(int batteryId)
        {
            var answers = (from bt in DataContext.BatteryTests
                           where bt.BatteryID == batteryId
                           orderby bt.Number
                           select bt).ToList();

            return answers.Select(t => (IBatteryTest)t).ToList();
        }

        public IBattery GetBattery(int BatteryId)
        {
            return (IBattery)DataContext.Batteries.FirstOrDefault(t => t.BatteryID == BatteryId);
        }

        public List<IBattery> GetBatteries()
        {
            var temp = DataContext.Batteries.ToList();
            return temp.Select(t => (IBattery)t).ToList();
        }

        public List<ICategory> GetCategories()
        {
            var result = DataContext.Categories.ToList();
            return result.Select(t => (ICategory)t).ToList();
        }

        public List<ITest> GetTestsByCategory(int categoryId)
        {
            var result = DataContext.Tests.Where(t => t.CategoryID == categoryId).ToList();
            return result.Select(t => (ITest)t).ToList();
        }

        public List<ITest> GetTestsFor10Top()
        {
            var result = DataContext.Tests.Where(t1 => t1.TestingCount > 0).OrderByDescending(t2 => t2.TestingCount).ToList();
            return result.Select(t => (ITest)t).ToList();
        }

        public void IncreateTestingCount(int testId)
        {
            var test = DataContext.Tests.First(t => t.TestID == testId);
            if (test != null)
            {
                test.TestingCount++;
                DataContext.SaveChanges();
            }
        }

        public List<IScale> GetScales(List<int> scaleIds)
        {
            return (from s in Scales
                    where scaleIds.Contains(s.ScaleID)
                    select s).ToList();
        }

        private static List<IScale> _scales;
        private static List<IScale> Scales
        {
            get
            {
                if (_scales == null)
                {
                    var scales = DataContext.Scales.ToList();
                    _scales = scales.Select(t => (IScale)t).ToList();
                }
                return _scales;
            }
        }


        public IScale GetScale(int scaleId)
        {
            return Scales.FirstOrDefault(s => s.ScaleID == scaleId);
        }

        public IScale GetScale(string shortName)
        {
            return Scales.FirstOrDefault(s => s.ScaleShortName == shortName);
        }

        public List<IScale> GetScalesForTest(int testId)
        {
            return (from p in Scales
                    where p.TestID == testId
                    orderby p.ScaleID
                    select p).ToList();
        }

        public List<IScale> GetScalesAll()
        {
            //var result = (from p in DataContext.Scales
            //              select p).ToList();
            //return result.Select(t => (IScale)t).ToList();

            return Scales;
        }

        public List<IScaleWeight> GetScaleWeightForAnswers(List<int> answerIds)
        {
            return (from p in ScaleWeight
                    where answerIds.Contains(p.AnsID.HasValue ? p.AnsID.Value : 0)
                    select p).ToList();
        }

        private static List<IScaleWeight> _scaleWeight;
        private static IEnumerable<IScaleWeight> ScaleWeight
        {
            get
            {
                if (_scaleWeight == null)
                {
                    var scaleWeight = DataContext.ScaleWeights.ToList();
                    _scaleWeight = scaleWeight.Select(t => (IScaleWeight)t).ToList();
                }
                return _scaleWeight;
            }
        }

        public List<IScaleWeight> GetScaleWeightForScale(int scaleId)
        {
            return (from p in ScaleWeight
                    where p.ScaleID == scaleId
                    select p).ToList();
        }

        public double GetScaleWeightForScaleAndAnswer(int scaleId, int ansId)
        {
            IScaleWeight scaleWeight = ScaleWeight.FirstOrDefault(p => p.AnsID == ansId && p.ScaleID == scaleId);
            if (scaleWeight == null || !scaleWeight.Weight.HasValue)
                return 0.0;
            return scaleWeight.Weight.Value;
        }

        public List<IScaleWeight> GetScaleWeightForScaleAndAnswer(List<int> scaleIds, List<int> ansIds)
        {
            return (from p in ScaleWeight
                    where ansIds.Contains(p.AnsID.HasValue ? p.AnsID.Value : -1) && scaleIds.Contains(p.ScaleID.HasValue ? p.ScaleID.Value : -1)
                    select p).ToList();
        }

        public List<IInterpretRule> GetInterpretRules(int interpretId)
        {
            var result = (from p in DataContext.InterpretRules
                          where p.InterpretID == interpretId
                          select p).ToList();
            return result.Select(t => (IInterpretRule)t).ToList();
        }

        public List<IConseq> GetConseqsByRule(int ruleId)
        {
            var result = (from f in DataContext.Conseqs
                          where f.RuleID == ruleId
                          select f).ToList();
            return result.Select(t => (IConseq)t).ToList();
        }

        public List<IInterpret> GetInterprets()
        {
            var result = DataContext.Interprets.ToList();
            return result.Select(t => (IInterpret)t).ToList();
        }

        public void Dispose()
        {
            //if (_dataContext != null)
            //    _dataContext.Dispose();
        }
    }
}
