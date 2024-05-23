using Azure.AI.OpenAI;
using System.Text.Json;

namespace FunctionCallingFeature.Functions.EShop.Cart
{
    public class CreateCartFunction
    {
        public const string FunctionName = "add_product_to_cart";

        public static ChatCompletionsFunctionToolDefinition GetFunctionDefinition()
        {
            return new ChatCompletionsFunctionToolDefinition
            {
                Name = FunctionName,
                Description = "Create a cart and add a catalog product to it",
                Parameters = BinaryData.FromObjectAsJson(
                    new
                    {
                        Type = "object",
                        Properties = new
                        {
                            ProductIds = new
                            {
                                Type = "array",
                                Items = new
                                {
                                    Type = "string",
                                    Description = "The id of the product from catalog"
                                }
                            }
                        },
                        Required = new[] { "productIds" }
                    },
                    new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
            };
        }
    }
}
