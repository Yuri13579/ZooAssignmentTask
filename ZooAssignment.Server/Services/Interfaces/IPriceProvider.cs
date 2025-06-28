namespace ZooAssignment.Server.Services.Interfaces
{
    public interface IPriceProvider
    {
        IDictionary<string, double> GetPrices();
    }
}
