namespace Calculator.Domain.Entities
{
    public record User : BaseEntity
    {
        public string? Username { get; set; }
    }
}
