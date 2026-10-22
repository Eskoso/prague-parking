namespace PragueParkingV2
{
    public class Car : Vehicle
    {
        public override int Size { get; }

        public Car(
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