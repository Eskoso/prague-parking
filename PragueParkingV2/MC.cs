namespace PragueParkingV2
{
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