namespace PragueParking.DataAccess
{
    public class GarageConfig
    {
        public int NumberOfSpots { get; set; } = 100;
        public int SpotCapacity { get; set; } = 4;

        public List<VehicleConfig> VehicleTypes { get; set; } = new();
    }
}