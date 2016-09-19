namespace Testing.Data.Entities
{
    public interface IScale
    {
        int ScaleID { get; set; }
        string ScaleName { get; set; }
        string ScaleShortName { get; set; }
        int TestID { get; set; }
        double? BallAVR { get; set; }

        double? BallMin { get; set; }

        double? BallMax { get; set; }

        double? BallSTD { get; set; }

        double? Point0 { get; set; }

        double? Point1 { get; set; }

        double? Point2 { get; set; }

        double? Point3 { get; set; }

        double? Point4 { get; set; }
    }
}