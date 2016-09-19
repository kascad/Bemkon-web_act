namespace Testing.Data.Entities
{
    public interface IBatteryTest
    {
        int BatteryTestID { get; set; }
        int BatteryID { get; set; }
        int TestID { get; set; }
        int Number { get; set; }
    }
}
