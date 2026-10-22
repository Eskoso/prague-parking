namespace PragueParkingV2
{
    public class ParkingGarage
    {
        public List<ParkingSpot> Spots { get; } = new();

        public ParkingGarage(int numberOfSpots, int spotCapacity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(numberOfSpots);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(spotCapacity);

            for (int number = 1; number <= numberOfSpots; number++)
            {
                Spots.Add(new ParkingSpot(number, spotCapacity));
            }
        }

        public ParkingSpot? FindAvailableSpot(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            ParkingSpot? sharedSpot = Spots.FirstOrDefault(
                spot => spot.Vehicles.Count > 0 && spot.CanFit(vehicle));

            return sharedSpot ?? Spots.FirstOrDefault(
                spot => spot.CanFit(vehicle));
        }

        public bool TryPark(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            string plate = vehicle.LicensePlate;

            if (string.IsNullOrWhiteSpace(plate) ||
                plate.Length > 10 ||
                !plate.All(char.IsLetterOrDigit))
            {
                throw new ArgumentException("Ogiltigt registreringsnummer.");
            }

            bool alreadyParked = Spots.Any(spot =>
                spot.Vehicles.Any(parkedVehicle =>
                    string.Equals(
                        parkedVehicle.LicensePlate,
                        plate,
                        StringComparison.OrdinalIgnoreCase)));

            if (alreadyParked)
            {
                return false;
            }

            ParkingSpot? availableSpot = FindAvailableSpot(vehicle);

            if (availableSpot == null)
            {
                return false;
            }

            availableSpot.Vehicles.Add(vehicle);
            return true;
        }

        public Vehicle? FindVehicle(string licensePlate)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(licensePlate);

            return Spots
                .SelectMany(spot => spot.Vehicles)
                .FirstOrDefault(vehicle =>
                    string.Equals(
                        vehicle.LicensePlate,
                        licensePlate,
                        StringComparison.OrdinalIgnoreCase));
        }

        public bool RemoveVehicle(string licensePlate)
        {
            Vehicle? vehicle = FindVehicle(licensePlate);

            if (vehicle == null)
            {
                return false;
            }

            ParkingSpot spot = Spots.First(
                spot => spot.Vehicles.Contains(vehicle));

            return spot.Vehicles.Remove(vehicle);
        }

        public bool MoveVehicle(string licensePlate, int destinationNumber)
        {
            Vehicle? vehicle = FindVehicle(licensePlate);

            ParkingSpot? destination = Spots.FirstOrDefault(
                spot => spot.Number == destinationNumber);

            if (vehicle == null || destination == null)
            {
                return false;
            }

            ParkingSpot source = Spots.First(
                spot => spot.Vehicles.Contains(vehicle));

            if (source == destination || !destination.CanFit(vehicle))
            {
                return false;
            }

            source.Vehicles.Remove(vehicle);
            destination.Vehicles.Add(vehicle);

            return true;
        }
        public List<Vehicle> SearchVehicles(string searchTerm)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(searchTerm);

            return Spots
                .SelectMany(spot => spot.Vehicles)
                .Where(vehicle => vehicle.LicensePlate.Contains(
                    searchTerm,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}