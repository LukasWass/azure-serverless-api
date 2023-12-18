using System.IO;
using System.Net;
using System.Threading.Tasks;
using Company.Api.Models.Dtos.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace Company.Api.Triggers.Product;

public static class CreateProductHttpTrigger
{
    [FunctionName("CreateProduct")]
    [OpenApiOperation(
        operationId: "createProduct",
        tags: new[] { "product" },
        Summary = "Create product",
        Description = "Create product")]
    [OpenApiParameter("id", Description = "Product id", Type = typeof(string), Required = true)]
    [OpenApiRequestBody("application/json", typeof(CreateProductDto))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(EntityLibrary.Product.Product))]
    public static async Task<IActionResult> CreateProduct(
        [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "product/{id}")]
        HttpRequest req,
        [CosmosDB(
            databaseName: "CompanyDb",
            containerName: "Products",
            Connection = "COSMOS_CONNECTION_STRING")]
        IAsyncCollector<EntityLibrary.Product.Product> documentsOut,
        string id,
        ILogger log)
    {
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<CreateProductDto>(requestBody);

        EntityLibrary.Product.Product product = new()
        {
            Id = id,
            Name = data.Name,
            Price = data.Price
        };

        await documentsOut.AddAsync(product);

        return new OkObjectResult(product);
    }
}