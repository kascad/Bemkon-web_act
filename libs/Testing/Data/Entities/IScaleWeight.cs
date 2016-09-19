namespace Testing.Data.Entities
{
    public interface IScaleWeight
    {
        int ScaleWeightID { get; set; }

        int? AnsID { get; set; }

        int? ScaleID { get; set; }

        double? Weight { get; set; }
    }
}