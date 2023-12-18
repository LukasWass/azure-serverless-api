using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace Company.Api.Triggers.Product;

public static class GetProductHttpTrigger
{
    [FunctionName("GetProduct")]
    [OpenApiOperation(
        operationId: "getProduct",
        tags: new[] { "product" },
        Summary = "Get product",
        Description = "Get one product")]
    [OpenApiParameter("id", Description = "Product id", Type = typeof(string), Required = true)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(EntityLibrary.Product.Product))]
    [OpenApiResponseWithoutBody(HttpStatusCode.NotFound)]
    public static async Task<IActionResult> GetProduct(
        [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "product/{id}")]
        HttpRequest req,
        [CosmosDB(
            databaseName: "CompanyDb",
            containerName: "Products",
            Connection = "COSMOS_CONNECTION_STRING")]
        IEnumerable<EntityLibrary.Product.Product> documents,
        string id,
        ILogger log)
    {
        var product = documents.Where(i => i.Id == id)?.FirstOrDefault();

        if (product == null) return new NotFoundResult();

        return new OkObjectResult(product);
    }
}