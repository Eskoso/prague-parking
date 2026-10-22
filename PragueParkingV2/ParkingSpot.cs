namespace PragueParkingV2
{
    public class ParkingSpot
    {
        public int Number { get; }
        public int Capacity { get; }
        public List<Vehicle> Vehicles { get; } = new();

        public ParkingSpot(int number, int capacity)
        {
            Number = number;
            Capacity = capacity;
        }

        public int UsedCapacity => Vehicles.Sum(vehicle => vehicle.Size);

        public bool CanFit(Vehicle vehicle)
        {
            return UsedCapacity + vehicle.Size <= Capacity;
        }
    }
}
