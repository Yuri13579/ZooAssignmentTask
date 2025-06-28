using Moq;
using System.Collections.Generic;
using Xunit;
using ZooAssignment.Server.Models;
using ZooAssignment.Server.Services;
using ZooAssignment.Server.Services.Interfaces;

namespace ZooAssignment.Server.Tests
{
    public class DailyCostCalculatorTests
{
    // ------------------------- helpers ------------------------------------
    private static IPriceProvider CreatePriceProvider(double? meat = 12.56, double? fruit = 5.60)
    {
        var map = new Dictionary<string, double>();
        if (meat.HasValue) map["Meat"] = meat.Value;
        if (fruit.HasValue) map["Fruit"] = fruit.Value;

        var mock = new Mock<IPriceProvider>();
        mock.Setup(p => p.GetPrices()).Returns(map);
        return mock.Object;
    }

    private static IAnimalInfoProvider CreateAnimalInfoProvider()
    {
        var types = new List<AnimalTypeInfo>
        {
            new("Lion",    0.10, DietType.Meat),
            new("Tiger",   0.09, DietType.Meat),
            new("Giraffe", 0.08, DietType.Fruit),
            new("Zebra",   0.08, DietType.Fruit),
            new("Wolf",    0.07, DietType.Both, 90),
            new("Piranha", 0.50, DietType.Both, 50)
        };

        var mock = new Mock<IAnimalInfoProvider>();
        mock.Setup(ip => ip.GetAnimalTypes()).Returns(types);
        return mock.Object;
    }

    private static IZooProvider CreateZooProvider()
    {
        var animals = new List<ZooAnimal>
        {
            new("Lion",    "Simba",     160),
            new("Lion",    "Nala",      172),
            new("Lion",    "Mufasa",    190),
            new("Giraffe", "Hanna",     200),
            new("Giraffe", "Anna",      202),
            new("Giraffe", "Susanna",   199),
            new("Tiger",   "Dante",     150),
            new("Tiger",   "Asimov",    142),
            new("Tiger",   "Tolkien",   139),
            new("Zebra",   "Chip",      100),
            new("Zebra",   "Dale",       62),
            new("Wolf",    "Pin",        78),
            new("Wolf",    "Pon",        69),
            new("Piranha", "Anastasia",   0.5)
        };

        var mock = new Mock<IZooProvider>();
        mock.Setup(z => z.GetAnimals()).Returns(animals);
        return mock.Object;
    }

    // ------------------------------ tests ----------------------------------

    [Fact]
    public void CalculateDailyCost_ReturnsExpectedTotal()
    {
        // Arrange
        var calculator = new DailyCostCalculator(
            CreatePriceProvider(),
            CreateAnimalInfoProvider(),
            CreateZooProvider());

        // Act
        var result = calculator.CalculateDailyCost();

        // Assert
        Assert.InRange(result.TotalCost, 1609.00, 1609.02);
        Assert.Equal(14, result.Breakdown.Count);
    }

    [Fact]
    public void CalculateDailyCost_IgnoresUnknownSpecies()
    {
        // Arrange – add a dragon!
        var zoo = new List<ZooAnimal>(CreateZooProvider().GetAnimals())
        {
            new("Dragon", "Smaug", 1000)
        };
        var mockZoo = new Mock<IZooProvider>();
        mockZoo.Setup(z => z.GetAnimals()).Returns(zoo);

        var calculator = new DailyCostCalculator(
            CreatePriceProvider(),
            CreateAnimalInfoProvider(),
            mockZoo.Object);

        // Act
        var result = calculator.CalculateDailyCost();

        // Assert – total cost unchanged (dragon ignored)
        Assert.InRange(result.TotalCost, 1609.00, 1609.02);
        Assert.Equal(14, result.Breakdown.Count);
    }

    [Fact]
    public void CalculateDailyCost_MissingFruitPrice_UsesMeatOnly()
    {
        // Arrange – no Fruit price
        var calculator = new DailyCostCalculator(
            CreatePriceProvider(fruit: null),
            CreateAnimalInfoProvider(),
            CreateZooProvider());

        // Act
        var result = calculator.CalculateDailyCost();

        // Expected (meat portions only)
        const double expectedMeatCost = 12.56 * (
            52.2  +  // lions
            38.79 +  // tigers
            9.261 +  // wolves
            0.125);  // piranha

        Assert.InRange(result.TotalCost, expectedMeatCost - 0.01, expectedMeatCost + 0.01);
    }
}

}
