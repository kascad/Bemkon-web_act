namespace Testing.Data.Entities
{
    public interface IBatteriesView
    {
        int BatteryID { get; set; }
        string BatteryName { get; set; }
        string Description { get; set; }
        string BatteryTests { get; set; }
    }
}
