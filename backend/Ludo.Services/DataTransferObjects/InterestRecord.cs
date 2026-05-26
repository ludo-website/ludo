namespace Ludo.Services.DataTransferObjects;

public record InterestCreateRecord
{
    public Guid AccountId { get; set; }
    public Guid GameId { get; set; }
    public decimal? Ammount { get; set; }
}

public record InterestRecord
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid GameId { get; set; }
    public decimal Ammount { get; set; }
}

public record InterestUpdateRecord
{
    public Guid Id { get; set; }
    public decimal? Ammount { get; set; }
}
