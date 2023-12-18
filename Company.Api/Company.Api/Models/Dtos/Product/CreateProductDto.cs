using Newtonsoft.Json;

namespace Company.Api.Models.Dtos.Product;

public class CreateProductDto
{
    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("price")] public decimal Price { get; set; }
}