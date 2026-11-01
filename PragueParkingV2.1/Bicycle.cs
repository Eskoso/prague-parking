namespace PragueParkingV2_1
{
    // Bicycle klass som representerar en cykel med registreringsnummer som id som kunden kan få på sitt kvitto, ankomsttid och storlek
    public class Bicycle : Vehicle
    {
        // Storlek på cykeln
        public override int Size { get; }

        public Bicycle(
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