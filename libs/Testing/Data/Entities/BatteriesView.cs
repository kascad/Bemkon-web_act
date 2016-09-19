namespace Testing.Data.Entities
{
    public class BatteriesView : IBatteriesView
    {
        public int BatteryID { get; set; }
        public string BatteryName { get; set; }
        public string Description { get; set; }
        public string BatteryTests { get; set; }
    }
}
