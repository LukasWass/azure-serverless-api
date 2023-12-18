using System.Collections.Generic;
using System.Linq;
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
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(List<EntityLibrary.Product.Product>))]
    [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest)]
    public static async Task<IActionResult> GetProducts(
        [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "products/{ids}")]
        HttpRequest req,
        [CosmosDB(
            databaseName: "CompanyDb",
            containerName: "Products",
            Connection = "COSMOS_CONNECTION_STRING")]
        IEnumerable<EntityLibrary.Product.Product> documents,
        string ids,
        ILogger log)
    {
        var productIds = ids.Split(";").Where(i => !string.IsNullOrEmpty(i)).ToList();
        if (productIds.Count > 100) return new BadRequestObjectResult("Max allowed product ids is 100");

        var products = documents.Where(i => productIds.Contains(i.Id));

        return new OkObjectResult(products);
    }
}