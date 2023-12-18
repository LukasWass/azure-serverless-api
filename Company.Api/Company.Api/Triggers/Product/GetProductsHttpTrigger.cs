using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;

namespace Company.Api.Triggers.Product;

public static class GetProductsHttpTrigger
{
    [FunctionName("GetProducts")]
    [OpenApiOperation(
        operationId: "getProducts",
        tags: new[] { "product" },
        Summary = "Get products",
        Description =
            "This is meant for batch fetching products. {ids} can contain up to 100 semicolon delimited ids.")]
    [OpenApiParameter("ids", Description = "Semicolon delimited product ids", Type = typeof(string), Required = true)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(string))]
    public static async Task<IActionResult> GetProducts(
        [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "products/{ids}")]
        HttpRequest req,
        string ids,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        return new OkObjectResult($"Products: {ids}");
    }
}