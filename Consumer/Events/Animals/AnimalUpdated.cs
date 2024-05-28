namespace Consumer.Events.Animals
{
    public class AnimalUpdated
    {
        public string? Id { get; set; }
        public int Age { get; set; }
        public int Weight { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
    }
}