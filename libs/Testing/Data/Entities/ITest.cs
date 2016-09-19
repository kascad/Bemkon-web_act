using System;

namespace Testing.Data.Entities
{
    public interface ITest
    {
        int TestID { get; set; }
        string ShortName { get; set; }
        string FullName { get; set; }
        int CategoryID { get; set; }
        string Author { get; set; }
        DateTime? Date { get; set; }
        string Description { get; set; }
        int TestingCount { get; set; }
        string Preamble { get; set; }
        bool HorisontalAnswers { get; set; }
    }
}
