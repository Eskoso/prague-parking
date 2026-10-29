namespace PragueParkingV2
{
    // Vehicle klass som representerar ett fordon med registreringsnummer, ankomsttid och storlek
    public abstract class Vehicle
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