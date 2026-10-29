using System.Text.Json;

namespace PragueParking.DataAccess
{
    // ParkingFile klass som hanterar inläsning och sparande av parkeringsdata
    public class ParkingFile
    {
        public List<VehicleData> Load(string filePath)
        {
            string json;

            // Försök att läsa in datafilen, om filen inte finns returnera en tom lista
            try
            {
                json = File.ReadAllText(filePath);
            }
            catch (FileNotFoundException)
            {
                return new List<VehicleData>();
            }

            return JsonSerializer.Deserialize<List<VehicleData>>(json)
                ?? throw new JsonException(
                    "Datafilen måste innehålla en lista med fordon.");
        }

        // Save metod som sparar parkeringsdata till en fil
        public void Save(string filePath, List<VehicleData> vehicles)
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