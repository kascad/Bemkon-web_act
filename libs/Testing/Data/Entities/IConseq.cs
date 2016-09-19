namespace Testing.Data.Entities
{
    public interface IConseq
    {
        int ConseqID { get; set; }

        int RuleID { get; set; }

        string ConseqText { get; set; }

        int? TXT_TXT_N { get; set; } 
    }
}