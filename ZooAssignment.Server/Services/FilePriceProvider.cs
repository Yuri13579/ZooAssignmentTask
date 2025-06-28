using Microsoft.Extensions.Options;
using ZooAssignment.Server.Models;
using ZooAssignment.Server.Services.Interfaces;

namespace ZooAssignment.Server.Services
{
    public class FilePriceProvider(IOptions<DataFilesOptions> opts,
        IWebHostEnvironment env) : IPriceProvider
    {
        private readonly string _filePath = Path.Combine(env.ContentRootPath, opts.Value.Prices);

        public IDictionary<string, double> GetPrices()
        {
            var dict = new Dictionary<string, double>(System.StringComparer.OrdinalIgnoreCase);
            foreach (var line in File.ReadAllLines(_filePath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split('=');
                if (parts.Length == 2 && double.TryParse(parts[1], out double price))
                {
                    dict[parts[0]] = price;
                }
            }
            return dict;
        }
    }
}
