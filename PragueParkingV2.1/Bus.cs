namespace PragueParkingV2_1
{
    // Bus klass som representerar en buss med registreringsnummer, ankomsttid och storlek
    public class Bus : Vehicle
    {
        // Storlek på bussen
        public override int Size { get; }

        public Bus(
            string licensePlate,
            DateTimeOffset arrivalTime,
            int size)
            : base(licensePlate, arrivalTime)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(size);
            Size = size;
        }
    }
}