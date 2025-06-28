using ZooAssignment.Server.Models;

namespace ZooAssignment.Server.Services.Interfaces
{
    public interface IZooProvider
    {
        IEnumerable<ZooAnimal> GetAnimals();
    }
}
