namespace PragueParkingV2
{
    // ParkingGarage klass som representerar ett parkeringsgarage med en lista av parkeringsplatser
    public class ParkingGarage
    {
        // Lista av parkeringsplatser i garaget
        public List<ParkingSpot> Spots { get; } = new();

        // Platsnummer i spotCapacities ersätter standardkapaciteten för just den platsen.
        public ParkingGarage(int numberOfSpots, int spotCapacity, Dictionary<int, int> spotCapacities)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(numberOfSpots);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(spotCapacity);
            ArgumentNullException.ThrowIfNull(spotCapacities);

            foreach (int spotNumber in spotCapacities.Keys)
            {
                int capacity = spotCapacities[spotNumber];

                if (spotNumber < 1 || spotNumber > numberOfSpots)
                {
                    throw new ArgumentException(
                        "Platsnumret finns inte i garaget.");
                }

                if (capacity <= 0)
                {
                    throw new ArgumentException(
                        "Platsens kapacitet måste vara större än noll.");
                }
            }

            for (int number = 1; number <= numberOfSpots; number++)
            {
                int capacity = spotCapacity;

                if (spotCapacities.ContainsKey(number))
                {
                    capacity = spotCapacities[number];
                }

                Spots.Add(new ParkingSpot(number, capacity));
            }
        }

        // Hittar en tillgänglig parkeringsplats för ett fordon
        public ParkingSpot? FindAvailableSpot(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            ParkingSpot? sharedSpot = Spots.FirstOrDefault(
                spot => spot.Vehicles.Count > 0 && spot.CanFit(vehicle));

            return sharedSpot ?? Spots.FirstOrDefault(
                spot => spot.CanFit(vehicle));
        }

        // Försök parkera ett fordon i garaget
        public bool TryPark(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            string plate = vehicle.LicensePlate;

            // Kontrollera om registreringsnumret är giltigt
            if (string.IsNullOrWhiteSpace(plate) ||
                plate.Length > 10 ||
                !plate.All(char.IsLetterOrDigit))
            {
                throw new ArgumentException("Ogiltigt registreringsnummer.");
            }

            // Kontrollera om fordonet redan är parkerat
            bool alreadyParked = Spots.Any(spot =>
                spot.Vehicles.Any(parkedVehicle =>
                    string.Equals(
                        parkedVehicle.LicensePlate,
                        plate,
                        StringComparison.OrdinalIgnoreCase)));

            // Om fordonet redan är parkerat, skicka tillbaka false
            if (alreadyParked)
            {
                return false;
            }

            // Hitta en tillgänglig parkeringsplats för fordonet
            ParkingSpot? availableSpot = FindAvailableSpot(vehicle);

            // Om ingen tillgänglig plats hittas, skicka tillbaka false
            if (availableSpot == null)
            {
                return false;
            }

            // Parkera fordonet på den tillgängliga platsen
            availableSpot.Vehicles.Add(vehicle);
            return true;
        }

        // Hitta ett fordon i garaget baserat på registreringsnummer
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

        // Ta bort ett fordon från garaget baserat på registreringsnummer
        public bool RemoveVehicle(string licensePlate)
        {
            // Hitta fordonet i garaget
            Vehicle? vehicle = FindVehicle(licensePlate);

            // Om fordonet inte hittas, skicka tillbaka false
            if (vehicle == null)
            {
                return false;
            }

            // Hitta parkeringsplatsen där fordonet är parkerat
            ParkingSpot spot = Spots.First(
                spot => spot.Vehicles.Contains(vehicle));

            // Ta bort fordonet
            return spot.Vehicles.Remove(vehicle);
        }

        // Flytta ett fordon från en parkeringsplats till en annan
        public bool MoveVehicle(string licensePlate, int destinationNumber)
        {
            // Hitta fordonet och destinationen
            Vehicle? vehicle = FindVehicle(licensePlate);

            // Hitta destinationen baserat på destinationens nummer
            ParkingSpot? destination = Spots.FirstOrDefault(
                spot => spot.Number == destinationNumber);

            // Fordonet eller destinationen saknas, skicka tillbaka false
            if (vehicle == null || destination == null)
            {
                return false;
            }

            // Hitta parkeringsplatsen där fordonet är parkerat
            ParkingSpot source = Spots.First(
                spot => spot.Vehicles.Contains(vehicle));

            // Fordonet är redan på plats eller ifalla den inte får plats, skicka tillbaka false
            if (source == destination || !destination.CanFit(vehicle))
            {
                return false;
            }

            // Tar bort fordonet från original platsen och lägger till den på den nya destinationen
            source.Vehicles.Remove(vehicle);
            destination.Vehicles.Add(vehicle);

            return true;
        }

        // Sök efter fordon i garaget baserat på ett sökord (registreringsnummer)
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
