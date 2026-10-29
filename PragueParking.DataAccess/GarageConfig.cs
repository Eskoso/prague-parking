namespace PragueParking.DataAccess
{
    // GarageConfig klass som representerar konfigurationen för garaget med antal platser, platskapacitet och en lista av fordonstyper
    public class GarageConfig
    {
        public int NumberOfSpots { get; set; } = 100;
        public int SpotCapacity { get; set; } = 4;
        public Dictionary<int, int> SpotCapacities { get; set; } = new();

        public List<VehicleConfig> VehicleTypes { get; set; } = new();
    }
}
