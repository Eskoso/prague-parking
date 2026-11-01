namespace PragueParkingV2_1
{
    // Vehicle klass som representerar ett fordon med registreringsnummer, ankomsttid och storlek
    public abstract class Vehicle : IVehicle
    {
        public string LicensePlate { get; }
        public DateTimeOffset ArrivalTime { get; }
        public abstract int Size { get; }

        protected Vehicle(
            string licensePlate,
            DateTimeOffset arrivalTime)
        {
            LicensePlate = licensePlate;
            ArrivalTime = arrivalTime;
        }
    }
}