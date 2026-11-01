namespace PragueParkingV2_1
{
    // ParkingSpot klass som representerar en parkeringsplats med nummer, kapacitet och en lista av fordon
    public class ParkingSpot : IParkingSpot
    {
        public int Number { get; }
        public int Capacity { get; }
        public List<IVehicle> Vehicles { get; } = new();

        // Konstruktor som tar emot nummer och kapacitet för parkeringsplatsen
        public ParkingSpot(int number, int capacity)
        {
            Number = number;
            Capacity = capacity;
        }

        // Beräknar den använda kapaciteten baserat på fordonens storlek och hanterar specialfallet för bussar
        public int UsedCapacity
        {
            get
            {
                foreach (IVehicle vehicle in Vehicles)
                {
                    if (vehicle is Bus)
                    {
                        return Capacity;
                    }
                }

                return Vehicles.Sum(vehicle => vehicle.Size);
            }
        }

        // Kontrollerar om ett fordon kan passa på parkeringsplatsen baserat på kapacitet och specialfallet för bussar
        public bool CanFit(IVehicle vehicle)
        {
            if (vehicle is Bus)
            {
                return false;
            }

            return UsedCapacity + vehicle.Size <= Capacity;
        }
    }
}
