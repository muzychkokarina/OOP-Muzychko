namespace lab7v13
{
    public class City : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public long Population { get; set; }
    }
}