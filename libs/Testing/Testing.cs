using Testing.Data.Entities;

namespace Testing
{
    partial class TestingDataContext
    {
    }

    public partial class Answer : IAnswer
    {
    }

    public partial class Battery : IBattery
    {
    }

    public partial class BatteryTest : IBatteryTest
    {
    }

    public partial class Category : ICategory
    {
    }

    public partial class Question : IQuestion
    {
        public byte[] QuestImgByte
        {
            get { return QuestImg.ToArray(); }
            set { QuestImg = value; }
        }
    }

    public partial class Scales : IScale
    {
    }

    public partial class ScaleWeight : IScaleWeight
    {
    }

    public partial class Test : ITest
    {
    }

    public partial class BatteriesView : IBatteriesView
    {
    }

    public partial class InterpretRule : IInterpretRule
    {

    }

    public partial class Conseq : IConseq 
    {
    }

    public partial class Interpret : IInterpret
    {
        
    }
}
