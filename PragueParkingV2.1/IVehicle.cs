namespace PragueParkingV2_1
{
    public interface IVehicle
    {
        string LicensePlate { get; }
        DateTimeOffset ArrivalTime { get; }
        int Size { get; }
    }
}