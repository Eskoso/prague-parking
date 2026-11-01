using System.Text.Json;

namespace PragueParking.DataAccess
{
    // ParkingFile klass som hanterar inläsning och sparande av parkeringsdata
    public class ParkingFileV21
    {
        public List<VehicleDataV21> Load(string filePath)
        {
            string json = File.ReadAllText(filePath);

            using JsonDocument document = JsonDocument.Parse(json);

            // Validera att JSON-dokumentet är en array, annars kasta ett undantag
            if (document.RootElement.ValueKind != JsonValueKind.Array)
            {
                throw new JsonException(
                    "Datafilen måste innehålla en lista med fordon.");
            }

            // Skapa en lista av VehicleDataV21-objekt och validera varje fordon i arrayen
            var vehicles = new List<VehicleDataV21>();

            // Gå genom varje element i JSON-arrayen och deserialisera till VehicleDataV21
            foreach (JsonElement element in document.RootElement.EnumerateArray())
            {
                VehicleDataV21 vehicle = element.Deserialize<VehicleDataV21>()
                    ?? throw new JsonException(
                        "Datafilen innehåller ett tomt fordon.");

                bool hasOldNumber = element.TryGetProperty(
                    "SpotNumber", out JsonElement oldNumber);

                bool hasNewNumbers = element.TryGetProperty(
                    "SpotNumbers", out _);

                if (hasOldNumber && hasNewNumbers)
                {
                    throw new InvalidDataException(
                        "Ange antingen SpotNumber eller SpotNumbers, inte båda.");
                }

                // Validera att SpotNumbers är en array av heltal
                if (!hasNewNumbers)
                {
                    if (!hasOldNumber)
                    {
                        throw new InvalidDataException(
                            "Fordonet saknar platsnummer.");
                    }

                    int number = oldNumber.GetInt32();

                    if (number <= 0)
                    {
                        throw new InvalidDataException(
                            "Platsnumret måste vara större än noll.");
                    }

                    vehicle.SpotNumbers.Add(number);
                }

                // Validera att SpotNumbers inte är null eller tom
                if (vehicle.SpotNumbers == null || vehicle.SpotNumbers.Count == 0)
                {
                    throw new InvalidDataException(
                        "Fordonet måste ha minst ett platsnummer.");
                }

                vehicles.Add(vehicle);
            }

            return vehicles;
        }

        // Save metod som sparar parkeringsdata till en fi
        public void Save(string filePath, List<VehicleDataV21> vehicles)
        {
            string json = JsonSerializer.Serialize(
                vehicles,
                new JsonSerializerOptions { WriteIndented = true });

            string temporaryPath = filePath + ".tmp";

            File.WriteAllText(temporaryPath, json);
            File.Move(temporaryPath, filePath, overwrite: true);
        }
    }
}