namespace PragueParkingV2_1
{
    // ParkingSpot klass som representerar en parkeringsplats med nummer, kapacitet och en lista av fordon
    public class ParkingSpot : IParkingSpot
    {
        public int Number { get; }
        public int Capacity { get; }
        public List<IVehicle> Vehicles { get; } = new();

        public ParkingSpot(int number, int capacity)
        {
            Number = number;
            Capacity = capacity;
        }

        public int UsedCapacity => Vehicles.Sum(vehicle => vehicle.Size);

        public bool CanFit(IVehicle vehicle)
        {
            return UsedCapacity + vehicle.Size <= Capacity;
        }
    }
}
