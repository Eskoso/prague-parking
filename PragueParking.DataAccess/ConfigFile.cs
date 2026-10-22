using System.Text.Json;

namespace PragueParking.DataAccess
{
    public class ConfigFile
    {
        public GarageConfig Load(string filePath)
        {
            string json = File.ReadAllText(filePath);

            GarageConfig config =
                JsonSerializer.Deserialize<GarageConfig>(json)
                ?? throw new JsonException(
                    "Konfigurationsfilen saknar inställningar.");

            if (config.NumberOfSpots <= 0 || config.SpotCapacity <= 0)
            {
                throw new InvalidDataException(
                    "Antal platser och platskapacitet måste vara större än noll.");
            }

            if (config.VehicleTypes == null || config.VehicleTypes.Count != 2)
            {
                throw new InvalidDataException(
                    "Konfigurationen måste innehålla Bil (CAR) och MC.");
            }

            var types = new HashSet<string>();

            foreach (VehicleConfig vehicle in config.VehicleTypes)
            {
                if (vehicle == null ||
                    (vehicle.Type != "CAR" && vehicle.Type != "MC"))
                {
                    throw new InvalidDataException(
                        "Tillåtna fordonstyper i V2.0 är CAR och MC.");
                }

                if (!types.Add(vehicle.Type))
                {
                    throw new InvalidDataException(
                        "Samma fordonstyp får inte anges flera gånger.");
                }

                if (vehicle.Size <= 0 || vehicle.Size > config.SpotCapacity)
                {
                    throw new InvalidDataException(
                        "Fordonsstorleken måste vara större än noll och rymmas på en plats.");
                }
            }

            return config;
        }
    }
}