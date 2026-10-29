namespace PragueParking.DataAccess
{
    // VehicleData klass som representerar ett fordon med typ, registreringsnummer, ankomsttid och platsnummer
    public class VehicleData
    {
        public string VehicleType { get; set; } = "";
        public string LicensePlate { get; set; } = "";
        public DateTimeOffset ArrivalTime { get; set; }
        public int SpotNumber { get; set; }
    }
}