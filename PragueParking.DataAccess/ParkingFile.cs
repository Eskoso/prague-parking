using System.Text.Json;

namespace PragueParking.DataAccess
{
    public class ParkingFile
    {
        public List<VehicleData> Load(string filePath)
        {
            string json;

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