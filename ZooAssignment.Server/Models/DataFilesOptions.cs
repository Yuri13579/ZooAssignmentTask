namespace ZooAssignment.Server.Models
{
    public record DataFilesOptions
    {
        public string Animals { get; init; } = default!;
        public string Prices  { get; init; } = default!;
        public string Zoo     { get; init; } = default!;
    }
}
