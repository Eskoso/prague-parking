namespace PragueParkingV2_1
{
    // ParkingGarage klass som representerar ett parkeringsgarage med en lista av parkeringsplatser
    public class ParkingGarage
    {
        // Lista av parkeringsplatser i garaget
        public List<IParkingSpot> Spots { get; } = new();

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
        public IParkingSpot? FindAvailableSpot(IVehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            IParkingSpot? sharedSpot = Spots.FirstOrDefault(
                spot => spot.Vehicles.Count > 0 && spot.CanFit(vehicle));

            return sharedSpot ?? Spots.FirstOrDefault(
                spot => spot.CanFit(vehicle));
        }

        // Hitta parkeringsplatser för en buss som börjar från ett specifikt platsnummer
        public List<IParkingSpot> FindBusSpots(IVehicle vehicle, int startNumber)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            // Kontrollera om fordonet är en buss och om startnumret är giltigt
            var busSpots = new List<IParkingSpot>();
            int lastBusSpot = Math.Min(50, Spots.Count);

            // om fordonet inte är en buss eller startnumret är utanför giltigt intervall, returnera en tom lista
            if (vehicle is not Bus ||
                startNumber < 1 ||
                startNumber > lastBusSpot - 3)
            {
                return busSpots;
            }

            long totalCapacity = 0;

            // går igenom fyra parkeringsplatser och kontrollera om de kan rymma bussen
            for (int number = startNumber; number < startNumber + 4; number++)
            {
                IParkingSpot? spot = Spots.FirstOrDefault(
                    spot => spot.Number == number);

                if (spot == null)
                {
                    return new List<IParkingSpot>();
                }

                // Kontrollera om parkeringsplatsen redan har fordon som inte är bussen
                foreach (IVehicle parkedVehicle in spot.Vehicles)
                {
                    if (parkedVehicle != vehicle)
                    {
                        return new List<IParkingSpot>();
                    }
                }

                busSpots.Add(spot);
                totalCapacity += spot.Capacity;
            }

            if (totalCapacity < vehicle.Size)
            {
                return new List<IParkingSpot>();
            }

            return busSpots;
        }

        // Försök parkera ett fordon i garaget
        public bool TryPark(IVehicle vehicle)
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

            // specialfall för bussar och försöker hitta fyra sammanhängande platser
            if (vehicle is Bus)
            {
                for (int start = 1; start <= Math.Min(50, Spots.Count) - 3; start++)
                {
                    List<IParkingSpot> busSpots = FindBusSpots(vehicle, start);

                    if (busSpots.Count == 4)
                    {
                        foreach (IParkingSpot spot in busSpots)
                        {
                            spot.Vehicles.Add(vehicle);
                        }

                        return true;
                    }
                }

                return false;
            }

            // Hitta en tillgänglig parkeringsplats för fordonet
            IParkingSpot? availableSpot = FindAvailableSpot(vehicle);

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
        public IVehicle? FindVehicle(string licensePlate)
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
            IVehicle? vehicle = FindVehicle(licensePlate);

            // Om fordonet inte hittas, skicka tillbaka false
            if (vehicle == null)
            {
                return false;
            }

            // Ta bort fordonet från alla parkeringsplatser där det är parkerat
            foreach (IParkingSpot spot in Spots)
            {
                spot.Vehicles.Remove(vehicle);
            }

            return true;
        }

        // Flytta ett fordon från en parkeringsplats till en annan
        public bool MoveVehicle(string licensePlate, int destinationNumber)
        {
            // Hitta fordonet och destinationen
            IVehicle? vehicle = FindVehicle(licensePlate);

            // Hitta destinationen baserat på destinationens nummer
            IParkingSpot? destination = Spots.FirstOrDefault(
                spot => spot.Number == destinationNumber);

            // Fordonet eller destinationen saknas, skicka tillbaka false
            if (vehicle == null || destination == null)
            {
                return false;
            }

            if (vehicle is Bus)
            {
                List<IParkingSpot> busSpots = FindBusSpots(
                    vehicle, destinationNumber);

                if (busSpots.Count != 4)
                {
                    return false;
                }

                if (busSpots.All(spot => spot.Vehicles.Contains(vehicle)))
                {
                    return false;
                }

                RemoveVehicle(licensePlate);

                foreach (IParkingSpot spot in busSpots)
                {
                    spot.Vehicles.Add(vehicle);
                }

                return true;
            }

            // Hitta parkeringsplatsen där fordonet är parkerat
            IParkingSpot source = Spots.First(
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
        public List<IVehicle> SearchVehicles(string searchTerm)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(searchTerm);

            return Spots
                .SelectMany(spot => spot.Vehicles)
                .Distinct()
                .Where(vehicle => vehicle.LicensePlate.Contains(
                    searchTerm,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
