using ZooAssignment.Server.Models;
using ZooAssignment.Server.Services.Interfaces;

namespace ZooAssignment.Server.Services
{
    public class DailyCostCalculator(IPriceProvider prices, IAnimalInfoProvider info, IZooProvider zoo)
        : ICostCalculator
    {
        public DailyCostResult CalculateDailyCost()
        {
            var priceMap = prices.GetPrices();
            var types = info.GetAnimalTypes().ToDictionary(t => t.Species, t => t, StringComparer.OrdinalIgnoreCase);
            var details = new List<AnimalCostDetail>();

            double total = 0;

            foreach (var animal in zoo.GetAnimals())
            {
                if (!types.TryGetValue(animal.Species, out var animalInfo))
                    continue;

                double foodAmount = animalInfo.Rate * animal.WeightKg;
                double meatKg = 0, fruitKg = 0;

                switch (animalInfo.Diet)
                {
                    case DietType.Meat:
                        meatKg = foodAmount;
                        break;

                    case DietType.Fruit:
                        fruitKg = foodAmount;
                        break;

                    case DietType.Both:
                        meatKg = foodAmount * animalInfo.MeatPercentage / 100.0;
                        fruitKg = foodAmount - meatKg;
                        break;
                }

                double cost = 0;
                if (priceMap.TryGetValue("Meat", out var meatPrice))
                    cost += meatKg * meatPrice;
                if (priceMap.TryGetValue("Fruit", out var fruitPrice))
                    cost += fruitKg * fruitPrice;

                details.Add(new AnimalCostDetail
                {
                    Animal = animal.Name,
                    Species = animal.Species,
                    MeatKg = meatKg,
                    FruitKg = fruitKg,
                    Cost = cost
                });

                total += cost;
            }

            return new DailyCostResult
            {
                TotalCost = total,
                Breakdown = details
            };
        }
    }
}
