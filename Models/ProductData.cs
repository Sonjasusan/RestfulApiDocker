namespace RestfulApi.Models
{
    public class ProductData
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = "default";
        public string CategoryName { get; set; } = "default";

        public string Supplier { get; set; } = "default";

    }
}
