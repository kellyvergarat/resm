namespace RealStateManagement.Model
{
    public class Property
    {
        public int? id { get; set; }
        public string? ImageUrl { get; set; }
        public decimal PricePerMonth { get; set; }
        public int Bathrooms { get; set; }
        public int Rooms { get; set; }
        public string? Owner { get; set; }
        public string? Pourpose { get; set; }
        public bool Status { get; set; }


    }
}
