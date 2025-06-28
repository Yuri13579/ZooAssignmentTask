namespace ZooAssignment.Server.Models
{
    public class DailyCostResult
    {
        public double TotalCost { get; set; }
        public List<AnimalCostDetail> Breakdown { get; set; }
    }

}
