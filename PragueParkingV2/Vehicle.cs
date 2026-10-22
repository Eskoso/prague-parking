namespace PragueParkingV2
{
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