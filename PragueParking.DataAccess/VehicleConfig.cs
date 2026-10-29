namespace PragueParking.DataAccess
{
    // VehicleConfig klass som representerar konfigurationen för ett fordon med typ och storlek
    public class VehicleConfig
    {
        public string Type { get; set; } = "";
        public int Size { get; set; }
        public required decimal HourlyRate { get; set; }
    }
}