using ZooAssignment.Server.Models;
using ZooAssignment.Server.Services.Interfaces;

namespace ZooAssignment.Server.Services
{
    public class DailyCostCalculator(IPriceProvider prices, IAnimalInfoProvider info, IZooProvider zoo) : ICostCalculator
    {
        public double CalculateDailyCost()
        {
            var priceMap = prices.GetPrices();
            var types = info.GetAnimalTypes().ToDictionary(t => t.Species, t => t, System.StringComparer.OrdinalIgnoreCase);
            double meatKg = 0, fruitKg = 0;
            foreach (var animal in zoo.GetAnimals())
            {
                if (!types.TryGetValue(animal.Species, out var info))
                    continue;
                double food = info.Rate * animal.WeightKg;
                switch (info.Diet)
                {
                    case DietType.Meat:
                        meatKg += food;
                        break;
                    case DietType.Fruit:
                        fruitKg += food;
                        break;
                    case DietType.Both:
                        meatKg += food * info.MeatPercentage / 100.0;
                        fruitKg += food * (100 - info.MeatPercentage) / 100.0;
                        break;
                }
            }
            double total = 0;
            if (priceMap.TryGetValue("Meat", out var meatPrice))
                total += meatPrice * meatKg;
            if (priceMap.TryGetValue("Fruit", out var fruitPrice))
                total += fruitPrice * fruitKg;
            return total;
        }
    }
}
