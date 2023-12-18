using Newtonsoft.Json;

namespace Company.EntityLibrary.Product;

public class Product
{
    [JsonProperty("id")] public string? Id { get; set; } = new Guid().ToString();

    [JsonProperty("name")] public string? Name { get; set; }

    [JsonProperty("price")] public decimal? Price { get; set; }
}