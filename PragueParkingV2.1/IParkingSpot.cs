namespace PragueParkingV2_1
{
    public interface IParkingSpot
    {
        int Number { get; }
        int Capacity { get; }
        int UsedCapacity { get; }
        List<IVehicle> Vehicles { get; }

        bool CanFit(IVehicle vehicle);
    }
}