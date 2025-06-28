using ZooAssignment.Server.Models;

namespace ZooAssignment.Server.Services.Interfaces
{
    public interface ICostCalculator
    {
        DailyCostResult  CalculateDailyCost();
    }
}
