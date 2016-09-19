namespace Testing.Data.Entities
{
    public interface IBattery
    {
        int BatteryID { get; set; }
        string BatteryName { get; set; }
        string Description { get; set; }
    }
}