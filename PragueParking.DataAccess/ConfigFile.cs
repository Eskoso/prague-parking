using System.Text.Json;

namespace PragueParking.DataAccess
{
    // ConfigFile klass som hanterar inläsning och validering av konfigurationsfilen
    public class ConfigFile
    {
        // Load metod som läser in konfigurationsfilen och validerar dess innehåll
        public GarageConfig Load(string filePath)
        {
            string json = File.ReadAllText(filePath);

            // Deserialisera JSON-strängen till ett GarageConfig-objekt och kasta ett undantag om det misslyckas
            GarageConfig config =
                JsonSerializer.Deserialize<GarageConfig>(json)
                ?? throw new JsonException(
                    "Konfigurationsfilen saknar inställningar.");

            // Validera konfigurationsfilens innehåll och kasta undantag vid ogiltiga värden
            if (config.NumberOfSpots <= 0 || config.SpotCapacity <= 0)
            {
                throw new InvalidDataException(
                    "Antal platser och platskapacitet måste vara större än noll.");
            }


            // Validera att SpotCapacities inte är null, annars kasta ett undantag
            if (config.SpotCapacities == null)
            {
                throw new InvalidDataException(
                    "Inställningarna för platsstorlekar saknas.");
            }

            int largestCapacity = 0;

            // Standardstorleken gäller för platser utan en egen inställning.
            if (config.SpotCapacities.Count < config.NumberOfSpots)
            {
                largestCapacity = config.SpotCapacity;
            }

            foreach (int spotNumber in config.SpotCapacities.Keys)
            {
                int capacity = config.SpotCapacities[spotNumber];

                if (spotNumber < 1 || spotNumber > config.NumberOfSpots)
                {
                    throw new InvalidDataException(
                        "Platsnumret finns inte i garaget.");
                }

                if (capacity <= 0)
                {
                    throw new InvalidDataException(
                        "Platsens kapacitet måste vara större än noll.");
                }

                if (capacity > largestCapacity)
                {
                    largestCapacity = capacity;
                }
            }

            // Validera att konfigurationsfilen innehåller exakt två fordonstyper: CAR och MC
            if (config.VehicleTypes == null || config.VehicleTypes.Count != 2)
            {
                throw new InvalidDataException(
                    "Konfigurationen måste innehålla Bil (CAR) och MC.");
            }

            var types = new HashSet<string>();

            // Validera varje fordonstyp i konfigurationsfilen och kasta undantag vid ogiltiga värden
            foreach (VehicleConfig vehicle in config.VehicleTypes)
            {
                if (vehicle == null ||
                    (vehicle.Type != "CAR" && vehicle.Type != "MC"))
                {
                    throw new InvalidDataException(
                        "Tillåtna fordonstyper i V2.0 är CAR och MC.");
                }

                // Kontrollera att fordonstypen inte redan har lagts till i HashSet
                if (!types.Add(vehicle.Type))
                {
                    throw new InvalidDataException(
                        "Samma fordonstyp får inte anges flera gånger.");
                }


                // Validera fordonets storlek och timpris
                if (vehicle.Size <= 0)
                {
                    throw new InvalidDataException(
                        "Fordonsstorleken måste vara större än noll.");
                }

                if (vehicle.Size > largestCapacity)
                {
                    throw new InvalidDataException(
                        "Fordonet är för stort för alla platser i garaget.");
                }

                if (vehicle.HourlyRate <= 0)
                {
                    throw new InvalidDataException(
                        "Timpriset måste vara större än noll.");
                }
            }

            return config;
        }
    }
}
