namespace PragueParking.DataAccess
{
    public class VehicleDataV21
    {
        public string VehicleType { get; set; } = "";
        public string LicensePlate { get; set; } = "";
        public DateTimeOffset ArrivalTime { get; set; }
        public List<int> SpotNumbers { get; set; } = new();
    }
}