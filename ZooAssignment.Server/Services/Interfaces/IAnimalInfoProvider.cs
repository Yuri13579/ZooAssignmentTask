using ZooAssignment.Server.Models;

namespace ZooAssignment.Server.Services.Interfaces
{
    public interface IAnimalInfoProvider
    {
        IEnumerable<AnimalTypeInfo> GetAnimalTypes();
    }
}
