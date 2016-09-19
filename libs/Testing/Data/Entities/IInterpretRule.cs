namespace Testing.Data.Entities
{
    public interface IInterpretRule
    {
        int RuleID { get; set; }

        int InterpretID { get; set; }

        string RuleText { get; set; }

        int? CON_TXT_N { get; set; }
    }
}