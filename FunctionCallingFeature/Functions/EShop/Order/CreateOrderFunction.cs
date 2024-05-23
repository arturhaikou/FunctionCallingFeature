using Azure.AI.OpenAI;
using System.Text.Json;

namespace FunctionCallingFeature.Functions.EShop.Order
{
    public class CreateOrderFunction
    {
        public const string FunctionName = "create_order";

        public static ChatCompletionsFunctionToolDefinition GetFunctionDefinition()
        {
            return new ChatCompletionsFunctionToolDefinition
            {
                Name = FunctionName,
                Description = "Create an order from the cart",
                Parameters = BinaryData.FromObjectAsJson(
                    new
                    {
                        Type = "object",
                        Properties = new
                        {
                            CartId = new
                            {
                                Type = "number",
                                Description = "The id of the cart."
                            }
                        },
                        Required = new[] { "cartId" }
                    },
                    new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            };
        }
    }
}
