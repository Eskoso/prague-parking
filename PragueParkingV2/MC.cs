namespace PragueParkingV2
{
    // MC klass som representerar en motorcykel med registreringsnummer, ankomsttid och storlek
    public class MC : Vehicle
    {
        public override int Size { get; }

        public MC(
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