using System.Globalization;
using Microsoft.Extensions.Options;
using ZooAssignment.Server.Models;
using ZooAssignment.Server.Services.Interfaces;

namespace ZooAssignment.Server.Services
{
    public class FileAnimalInfoProvider(IOptions<DataFilesOptions> opts,
        IWebHostEnvironment env) : IAnimalInfoProvider
    {
        private readonly string _filePath = Path.Combine(env.ContentRootPath, opts.Value.Animals);

        public IEnumerable<AnimalTypeInfo> GetAnimalTypes()
        {
            var list = new List<AnimalTypeInfo>();
            foreach (var line in File.ReadAllLines(_filePath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(';');
                if (parts.Length >= 3 && double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double rate))
                {
                    var diet = parts[2].ToLowerInvariant();
                    if (diet == "meat")
                    {
                        list.Add(new AnimalTypeInfo(parts[0], rate, DietType.Meat));
                    }
                    else if (diet == "fruit")
                    {
                        list.Add(new AnimalTypeInfo(parts[0], rate, DietType.Fruit));
                    }
                    else if (diet == "both" && parts.Length >= 4 && parts[3].EndsWith("%") && int.TryParse(parts[3].TrimEnd('%'), out int meatPercent))
                    {
                        list.Add(new AnimalTypeInfo(parts[0], rate, DietType.Both, meatPercent));
                    }
                }
            }
            return list;
        }
    }
}
