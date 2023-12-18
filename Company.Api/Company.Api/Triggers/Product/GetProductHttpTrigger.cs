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
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(string))]
    public static async Task<IActionResult> GetProduct(
        [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "product/{id}")]
        HttpRequest req,
        string id,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        return new OkObjectResult($"Product: {id}");
    }
}