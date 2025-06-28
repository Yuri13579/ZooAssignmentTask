namespace ZooAssignment.Server.Models
{
    public enum DietType
    {
        Meat,
        Fruit,
        Both
    }

    public record AnimalTypeInfo(string Species, double Rate, DietType Diet, int MeatPercentage = 0);
}
