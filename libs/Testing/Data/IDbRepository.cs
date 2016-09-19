using System;
using System.Collections.Generic;
using Testing.Data.Entities;

namespace Testing.Data
{
    public interface IDbRepository : IDisposable
    {
        ITest GetTest(int testId);
        ITest GetTest(string shortName);
        IQuestion GetQuestionByNumber(int testId, int questNum);
        IQuestion GetQuestion(int questId);
        int GetQuestionsCount(int testId);        
        IAnswer GetAnswer(int ansId);
        List<IAnswer> GetQuestionAnswers(int questId);
        List<IBatteriesView> GetBatteriesViews();
        List<IBatteryTest> GetBatteryTestsByBatteryID(int batteryId);
        IBattery GetBattery(int BatteryId);
        List<IBattery> GetBatteries();
        List<ICategory> GetCategories();
        List<ITest> GetTestsByCategory(int categoryId);
        List<ITest> GetTestsFor10Top();
        void IncreateTestingCount(int testId);

        List<IScale> GetScales(List<int> scaleIds);
        IScale GetScale(int scaleId);
        IScale GetScale(string shortName);
        List<IScale> GetScalesForTest(int testId);
        List<IScale> GetScalesAll();
        List<IScaleWeight> GetScaleWeightForAnswers(List<int> answerIds);
        List<IScaleWeight> GetScaleWeightForScale(int scaleId);
        double GetScaleWeightForScaleAndAnswer(int scaleId, int ansId);
        List<IScaleWeight> GetScaleWeightForScaleAndAnswer(List<int> scaleId, List<int> ansId);
        List<IInterpretRule> GetInterpretRules(int interpretId);
        List<IConseq> GetConseqsByRule(int ruleId);

	    List<IInterpret> GetInterprets();
    }
}
