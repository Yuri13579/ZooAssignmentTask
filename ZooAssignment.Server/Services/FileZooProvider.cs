using System.Globalization;
using System.Xml.Linq;
using Microsoft.Extensions.Options;
using ZooAssignment.Server.Models;
using ZooAssignment.Server.Services.Interfaces;

namespace ZooAssignment.Server.Services
{
    public class FileZooProvider(IOptions<DataFilesOptions> opts,
        IWebHostEnvironment env) : IZooProvider
    {
        private readonly string _filePath = Path.Combine(env.ContentRootPath, opts.Value.Zoo);

        public IEnumerable<ZooAnimal> GetAnimals()
        {
            var doc = XDocument.Load(_filePath);
            var animals = new List<ZooAnimal>();
            foreach (var element in doc.Descendants())
            {
                if (!element.HasElements && element.Parent != null && element.Attribute("kg") != null)
                {
                    var species = element.Name.LocalName;
                    var nameAttr = element.Attribute("name")?.Value ?? string.Empty;
                    if (double.TryParse(element.Attribute("kg")?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double weight))
                    {
                        animals.Add(new ZooAnimal(species, nameAttr, weight));
                    }
                }
            }
            return animals;
        }
    }
}
